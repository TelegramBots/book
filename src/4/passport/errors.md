# Passport Data Errors

[![setPassportDataErrors method](https://img.shields.io/badge/Bot_API_method-setPassportDataErrors-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#setpassportdataerrors)
[![Passport Element Errors tests](https://img.shields.io/badge/Examples-Passport_Element_Errors-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Extensions.Passport/tree/master/test/IntegrationTests/Passport%20Element%20Errors)

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
