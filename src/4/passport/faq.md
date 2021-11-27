# Telegram Passport Data Decryption - FAQ

## What is `PassportDataDecryptionException`

Methods on [`IDecrypter`] might throw [`PassportDataDecryptionException`] exception
if an error happens during decryption.
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

<!-- ----------- -->

[`IDecrypter`]: https://github.com/TelegramBots/Telegram.Bot.Extensions.Passport/blob/master/src/Telegram.Bot.Extensions.Passport/Decryption/IDecrypter.cs
[`PassportDataDecryptionException`]: https://github.com/TelegramBots/Telegram.Bot.Extensions.Passport/blob/master/src/Telegram.Bot.Extensions.Passport/Decryption/PassportDataDecryptionException.cs
