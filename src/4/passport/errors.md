# Handling Passport errors

## Handling Data Errors

[![setPassportDataErrors method](https://img.shields.io/badge/Bot_API_method-setPassportDataErrors-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#setpassportdataerrors)

If the passport data you received contains errors, the bot can use the [SetPassportDataErrors](https://core.telegram.org/bots/api#setpassportdataerrors) method to inform the user and request information again. The user will not be able to resend the data, until all errors are fixed.

Here is an example call using decrypted [credentials](files-docs.md#credentials):

```csharp
//using Telegram.Bot.Types.Passport;

PassportElementError[] errors =
{
    new PassportElementErrorDataField
    {
        Type = EncryptedPassportElementType.Passport,
        FieldName = "document_no",
        DataHash = credentials.SecureData.Passport.Data.DataHash,
        Message = "Invalid passport number"
    },
    new PassportElementErrorFrontSide
    {
        Type = EncryptedPassportElementType.Passport,
        FileHash = credentials.SecureData.Passport.FrontSide.FileHash,
        Message = "Document scan is redacted"
    },
    new PassportElementErrorSelfie
    {
        Type = EncryptedPassportElementType.Passport,
        FileHash = credentials.SecureData.Passport.Selfie.FileHash,
        Message = "Take a selfie without glasses"
    },
    new PassportElementErrorTranslationFile
    {
        Type = EncryptedPassportElementType.Passport,
        FileHash = credentials.SecureData.Passport.Translation[0].FileHash,
        Message = "Document photo is blury"
    },
};

await bot.SetPassportDataErrors(passportMessage.From.Id, errors);
```

## Decryption error (`PassportDataDecryptionException`)

Methods on `Decrypter` might throw `PassportDataDecryptionException` exception if an error happens during decryption.
The exception message tells you what went wrong but there is not much you can do to resolve it.
Maybe let your user know the issue and ask for Passport data again.

It is important to pass each piece of encrypted data, e.g. Id Document, Passport File, etc., with the right
accompanying credentials to decryption methods.

Spot the _problem in this code_ decrypting driver's license files:

```c#
byte[] selfieContent = decrypter.DecryptFile(
    encSelfieContent, // byte array of encrypted selfie file
    credentials.SecureData.DriverLicense.FrontSide // WRONG! use selfie file credentials
);
// throws PassportDataDecryptionException: "Data hash mismatch at position 123."
```
