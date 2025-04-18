﻿# Telegram Passport

[![Passport Documentation](https://img.shields.io/badge/Telegram-Passport_Manual-blue.svg?style=flat-square)](https://core.telegram.org/passport)
[![Passport Test page](https://img.shields.io/badge/Telegram-Test_Passport-violet.svg?style=flat-square)](https://core.telegram.org/passport/example)
[![Passport Quickstart example](https://img.shields.io/badge/Examples-Passport_Quickstart-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Extensions.Passport/tree/TestMergeInTB/src/Quickstart)

[Telegram Passport](https://telegram.org/blog/passport) is a unified authorization method for services that require personal identification.
As a bot developer, you can use it to receive confidential user data in an end-to-end encrypted fashion.  
There are several Know Your Customer (KYC) solutions that have already added support for Telegram Passport.

This guide is targeted at bot developers and presents our library support for Telegram Passport.
Click the badges above for more documentation.

# Quickstart

This guide teaches the basics of working with Telegram Passport.
See the complete version of the code at [Quickstart project](https://github.com/TelegramBots/Telegram.Bot.Extensions.Passport/tree/TestMergeInTB/src/Quickstart).
Code snippets on this page are in the context of that project.

## Encryption Keys

You don't really need to generate any RSA key. Use our sample keys for this demo.
Send the public key to [@BotFather](https://t.me/BotFather) using `/setpublickey` command:

![Setting public key](../docs/shot-passport_botfather.jpg)

Copy this public key and send it to BotFather.

```text
-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA0VElWoQA2SK1csG2/sY/
wlssO1bjXRx+t+JlIgS6jLPCefyCAcZBv7ElcSPJQIPEXNwN2XdnTc2wEIjZ8bTg
BlBqXppj471bJeX8Mi2uAxAqOUDuvGuqth+mq7DMqol3MNH5P9FO6li7nZxI1FX3
9u2r/4H4PXRiWx13gsVQRL6Clq2jcXFHc9CvNaCQEJX95jgQFAybal216EwlnnVV
giT/TNsfFjW41XJZsHUny9k+dAfyPzqAk54cgrvjgAHJayDWjapq90Fm/+e/DVQ6
BHGkV0POQMkkBrvvhAIQu222j+03frm9b2yZrhX/qS01lyjW4VaQytGV0wlewV6B
FwIDAQAB
-----END PUBLIC KEY-----
```

Now Telegram client app can encrypt the data for your bot using this key.

## Request Information

Bot waits for a text message from user. Once it receives a text message, it generates an authorization request link
and sends that to the user.

### Authorization Request

[![Request Parameters type](https://img.shields.io/badge/Passport_API_type-Request_Parameters-blue.svg?style=flat-square)](https://core.telegram.org/passport#request-parameters)

A passport authorization request means that the bot should ask the user to open a `tg://resolve` URI in the browser
with specific parameters in its query string.
You can alternatively have a button in an HTML page on your website for that.

Type `AuthorizationRequestParameters` helps you in creating such an URI.

```c#
AuthorizationRequestParameters authReq = new AuthorizationRequestParameters(
    botId: 123456, // bot user ID
    publicKey: "...", // public key in PEM format. same as the key above.
    nonce: "unique nonce for this request",
    scope: new PassportScope { // a PassportScope object
        Data = [
            new PassportScopeElementOne(EncryptedPassportElementType.Address),
            new PassportScopeElementOne(EncryptedPassportElementType.PhoneNumber)
        ]
    });
```

In `SendAuthorizationRequestAsync` method, we ask for `address` and `phone_number` scopes.
Then, we generate the query string and ask user to open the link.

You might be wondering what is the magic in here?

Our webpage <https://telegrambots.github.io/Telegram.Bot.Extensions.Passport/redirect.html>  
redirects the user to `tg://resolve` URI, appending whatever query string was passed to it.

> If a user is using an Android device, the URI will start with `tg:` instead of the default `tg://`.

![Passport link 1](../docs/shot-passport_link1.jpg)
![Passport link 2](../docs/shot-passport_link2.jpg)

### Passport Data

You, the user, should now be redirected to the Telegram Passport screen in your Telegram client app.
Enter your password and log in.

> Note that the app will ask you to register if this is the first time you are using Telegram Passport.

![Passport login](../docs/shot-passport_login.jpg)

Fill in the _address_ and _phone number_ data. Click on the _Authorize_ button at the end.

![Passport info 1](../docs/shot-passport_adress_phone1.jpg)
![address info](../docs/shot-passport_adress.jpg)
![phone info](../docs/shot-passport_phone.jpg)
![Passport info 2](../docs/shot-passport_adress_phone2.jpg)

At this point, your Telegram client app encrypts the actual Telegram Passport data (e.g. address) using the
AES algorithm, and then encrypts the info required for decryption using your bot's public RSA key.
Finally, it sends the result of both encryptions to Telegram servers.

### Data Decryption

[![Passport Data type](https://img.shields.io/badge/Bot_API_type-Passport_Data-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#passportdata)

Your bot now receives a new message update with the encrypted Passport data. The user is also notified in the chat:

![Passport update](../docs/shot-passport_update.jpg)

Let's decrypt that gibberish to get the information. That's what `DecryptPassportDataAsync` method does.

#### Step 1: Credentials

[![EncryptedCredentials type](https://img.shields.io/badge/Bot_API_type-EncryptedCredentials-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#encryptedcredentials)
[![Credentials type](https://img.shields.io/badge/Passport_API_type-Credentials-blue.svg?style=flat-square)](https://core.telegram.org/passport#credentials)

You can't just access the encrypted data in the `message.PassportData.Data` array.
Required parameters for their decryption are in the `message.PassportData.Credentials` object.
But that credentials object is encrypted using bot's public key!

We first take the bot's _private key_ this time and decrypt the credentials.

> Note: We are loading the RSA key from PEM format thanks to the method `RSA.ImportFromPem` (.NET 5+).

```c#
var decrypter = new Decrypter();
var credentials = decrypter.DecryptCredentials(message.PassportData.Credentials, GetRsaPrivateKey());
```

#### Step 2: Nonce

[![Credentials type](https://img.shields.io/badge/Passport_API_type-Credentials-blue.svg?style=flat-square)](https://core.telegram.org/passport#credentials)
[![Request Parameters type](https://img.shields.io/badge/Passport_API_type-Request_Parameters-blue.svg?style=flat-square)](https://core.telegram.org/passport#request-parameters)

There is a `nonce` property on the credentials (now decrypted) object.
In order to prevent certain attacks, ensure its value is exactly the same as the nonce you set in the authorization request.
Read more about [nonce on Wikipedia](https://en.wikipedia.org/wiki/Cryptographic_nonce).

#### Step 3: Residential Address

[![EncryptedPassportElement type](https://img.shields.io/badge/Bot_API_type-EncryptedPassportElement-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#encryptedpassportelement)
[![ResidentialAddress type](https://img.shields.io/badge/Passport_API_type-ResidentialAddress-blue.svg?style=flat-square)](https://core.telegram.org/passport#residentialaddress)

[![SecureData type](https://img.shields.io/badge/Passport_API_type-SecureData-blue.svg?style=flat-square)](https://core.telegram.org/passport#securedata)
[![SecureValue type](https://img.shields.io/badge/Passport_API_type-SecureValue-blue.svg?style=flat-square)](https://core.telegram.org/passport#securevalue)
[![DataCredentials type](https://img.shields.io/badge/Passport_API_type-DataCredentials-blue.svg?style=flat-square)](https://core.telegram.org/passport#datacredentials)

It's finally time to see the user's address.
We are looking for an encrypted element with type of _address_ in `message.PassportData.Data` array.
Also, decryption parameters for that are in `Credentials.SecureData.Address.Data`.
Here is how the decryption magic happens:

```c#
var addressElement = message.PassportData.Data.Single(el => el.Type == PassportEnums.Scope.Address);
var address = decrypter.DecryptData<ResidentialAddress>(addressElement.Data, credentials.SecureData.Address.Data);
```

`DecryptData` method does 3 tasks here:

1. Decrypts the data into a JSON-serialized string
1. Verifies that the data hashes match
1. Converts from JSON to a .NET object

#### Step 4: Phone Number

[![EncryptedPassportElement type](https://img.shields.io/badge/Bot_API_type-EncryptedPassportElement-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#encryptedpassportelement)

Values for phone number and email address are not end-to-end encrypted in Telegram Passport and
Telegram stores these values after being verified.

There is no need for decryption at this point.
Just find the element with the type of _phone\_number_ in the `message.PassportData.Data` array.

### Information Demo

At the end, bot sends some of the information received to the user for demo purposes.

![info demo](../docs/shot-passport_quickstart.jpg)
