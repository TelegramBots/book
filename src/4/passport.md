# Telegram Passport

[Telegram Passport] is Telegram's system, enabling developers to integrate user verification that requires the use of actual user documents.

Since this is a feature that deals with highly sensitive user data, Telegram made sure to implement it in an overly-complicated fashion.

To get started, read the following manuals (twice):
- [Passport manual]
- [Passport API docs]

### Overview

A description of the entire Passport process, after the initial setup, is as follows:

> **`Service`** referes to the organization, servers, websites, applications, connected to the bot
>
> **`User`** referes to a human (or AI), that wants to login/get verified on a website/application belonging to a `service`
>
> **`Client`** refers to the software running on a user's device (e.g. Telegram Android app)

1. The service (website or application) creates a [passport request][SDK]
    * This request contains bot's ID, public key, [unique payload][Request parameters] and selected [scopes][data fields]
    * A request also contains a callback (url, Android intent) - see [SDK]
2. A user clicks on the `Log in With Telegram` button, which opens their Telegram client
    * At this point, the client verifies that the public key in the request is connected to the bot
3. The client will prompt the user to enter their cloud password
    * The client fetches encrypted passport data from the server and decrypts it with this password.
    This lets the client pre-fill the required fields, if the user has already used Telegram Passport in the past
    * You should use a good password here, since it is the only thing protecting your documents in case of a data-breach on Telegram's side
4. The user modifies/adds data fields on the passport authorization screen
   * At this point, the user can visit the service's privacy policy site
   * Data added/modified at this point is synced with Telegram's servers for later use
   * If a field is a phone number, Telegram sends the user an SMS to verify the number
   (the service can therefore trust that the received phone number is valid and belongs to the user)
5. The user either submits the passport response, or terminates the request
   * If a user **terminates** the request, the callback is invoked, notifying the service of a failed request.
   *The process is done here*
   * If a user **submits** the response, the process continues in step 6
6. The client encrypts the response and sends it to Telegram
    * The data is encrypted using AES, using different keys for every field
      * The encrypted data fields are [`EncryptedPassportElement`] objects
    * The keys from the previous step are encrypted in an extra layer of AES
      * These are the [`DataCredentials`] and [`FileCredentials`] objects
    * The encrypted keys are then encrypted using the public RSA key in the request
      * This is the [`EncryptedCredentials`] object
    * The client now sends the encrypted data fields and the RSA-encrypted credentials to Telegram
      * This is the final [`PassportData`][`Message.Passport`] object
7. The client invokes the callback, notifying the service of a successful request
8. The bot receives a Message Update, with the [`Message.Passport`] field set
   * The service decrypts the passport data
   * Checks that the payload field matches the payload used in the request
   * Performs whatever verification of the received data it needs
9. If the service decides that fields are invalid (e.g. invalid document number)
    or that the images are not good enough (e.g. the selfie is blurry, or the driver's licence photo is a troll face),
    it notifies Telegram of [Errors] via the [`SetPassportDataErrors`] method, specifying which fields should be fixed
    * The user will not be able to submit the response again, until the specified fields are modified
    * The user will be prompted to fix the response only after he/she/it presses the 'login' button again

> Only the user and the service ever see the actual data.
>
> Telegram servers only store and relay encrypted data, but can not decrypt it.

### Generating an RSA key pair

Before you can start, you have to generate your own RSA key pair.
Using openssl this can be done like so:

`openssl genrsa 4096 > private.key`

You should be greeted by a message looking something like this:
```text
Generating RSA private key, 4096 bit long modulus
.............................++
...............................................++
e is 65537 (0x10001)
```
> Fixing issues with openssl (as well as its installation) is outside the scope of this guide

**Do not share this private key!**

Go to [@BotFather] and send him the command `/setpublickey`
(button support for passports is not available, *as of writing this article*).

You will be asked to enter the bot's name (e.g. `@MyTelegramBot`).

Now you need to paste in the public key in PEM format.
Using openssl, extract it from the private key with:

`openssl rsa -in private.key -pubout`

The key should look something like this:
```text
-----BEGIN PUBLIC KEY-----
MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAyISVUY2NjPgIMs+of0tM
U/S7T9wmJZbE0BBjxUq7aaz4eK2sBbdE6uYPT== (a bit longer of course)
-----END PUBLIC KEY-----
```
BotFather will want the -BEGIN- and -END- pieces as well

> In case your private key gets lost/exposed, you can repeat these steps to change the key with [@BotFather]

You will have to store the private key safely where you process received data.

### What data to request

You now have to decide what kind of data you need from the user.
Is their phone number enough? Do you need all of their identification documents?
> **Do not request data you don't actually need!**

You can view all possible data fields you can request from the user in the [manual][data fields].
> These fields are refered to as `scope` by the manual

If you select multiple documents of the same type (e.g. identity card and driver's licence), the user will be able to pick which one to share.
Your bot will receive at most:
* Personal details, address
* Phone number and/or an email address
* One type of proof of identity document
* One type of proof of address document

### Integration with your application or website

When you decide on the needed data fields, it's time to integrate the authentication with your website/application.

You can use the [SDK]s listed in the manual.

### Integration with your bot

One more setting you should set with [@BotFather] is your privacy policy.
Use the command `/setprivacypolicy` and send a link to the policy.
Users will see this link when faced with your passport request.

Now it's time for the bot part. Whenever a user fills in and submits the authentication form,
your bot gets all the information the user sent in a [`Message`] object,
with the [`Message.Passport`] field set.


### Decrypting the received passport data

#### Decrypting credentials

First, you have to decrypt the [`EncryptedCredentials`] included in the [`Message.Passport`] object.
The library provides an extension method on [`EncryptedCredentials`] that tries to decrypt them:
```csharp
bool TryDecryptCredentials(this EncryptedCredentials, RSA, out Credentials)
```
You have to pass in the private key you generated at the start as an [RSA object][PEM to RSA].

If the decryption was successful, you are granted with decrypted [`Credentials`].

Now is the time to check the `Credentials.Payload` field.
It should **exactly match** the payload you set in the [authorization request](#integration-with-your-application-or-website).

Description of the payload field, taken from [Telegram's passport manual][Request parameters]:
> **Important:** For security purposes it should be a cryptographically secure unique identifier of the request. In particular, it should be long enough and it should be generated using a cryptographically secure pseudorandom number generator. You should never accept credentials with the same payload twice.

#### Decrypting data fields

If the payload matches, you can continue on and decrypt the data included in the `PassportData.Data` field
(array of [`EncryptedPassportElement`]).

Fields that will be present depend on the data you requested, and the documents the user chose to provide.

The [manual][data fields] is your friend here. You can also check out the passport unit tests
(link to be added when the passport branch is merged) for an example of handling these fields.

Decrypting the encrypted passport data elements is done by calling the `TryDecrypt` extension method
on an [`EncryptedPassportElement`] object, passing in the corresponding [`DataCredentials`] you decrypted before.

These credentials can be found inside of the [`Credentials`] object. (Again, check the unit tests for an example)

#### Decrypting passport files

For getting the document images, you need to go a step further.
Some elements contain the `FrontSide`, `ReverseSide` and `Files` fields.
These contain information on how to download and decrypt said files.
Downloading these files is done like with every other file. See [downloading files](../3/files/download.md).
Note that these files are encrypted!

To decrypt them call the `PassportCryptography.TryDecryptFile` method,
passing in the encrypted file bytes and the corresponding [`FileCredentials`].

The decrypted bytes are a valid jpg file.

[Telegram Passport]: https://telegram.org/blog/passport
[Passport manual]: https://core.telegram.org/passport
[Passport API docs]: https://core.telegram.org/bots/api#telegram-passport
[Data fields]: https://core.telegram.org/passport#fields
[SDK]: https://core.telegram.org/passport#sdk
[`Message`]: https://core.telegram.org/bots/api#message
[`Message.Passport`]: https://core.telegram.org/bots/api#passportdata
[`EncryptedCredentials`]: https://core.telegram.org/bots/api#encryptedcredentials
[`Credentials`]: https://core.telegram.org/passport#credentials
[`DataCredentials`]: https://core.telegram.org/passport#datacredentials
[`FileCredentials`]: https://core.telegram.org/passport#filecredentials
[PEM to RSA]: https://stackoverflow.com/q/243646/6845657
[Request parameters]: https://core.telegram.org/passport#request-parameters
[`EncryptedPassportElement`]: https://core.telegram.org/bots/api#encryptedpassportelement
[Errors]: https://core.telegram.org/passport#fixing-errors
[`SetPassportDataErrors`]: https://core.telegram.org/bots/api#setpassportdataerrors
[@BotFather]: https://t.me/botfather