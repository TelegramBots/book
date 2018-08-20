First, read the [documentation on sending files](https://core.telegram.org/bots/api#sending-files).

## Uploading the actual file

```csharp
using (FileStream fs = System.IO.File.OpenRead("Local file.pdf"))
{
    InputOnlineFile inputOnlineFile = new InputOnlineFile(fs, "Name for the user.pdf");
    await Bot.SendDocumentAsync(message.Chat, inputOnlineFile);
}
```

## Uploading by file id

This is the method that you will want to use the most, as it is the most efficient.
After sending a file to the user, or receiving a file, you can send the file again using its ID.

This saves you from uploading the entire file.

```csharp
InputOnlineFile inputOnlineFile = new InputOnlineFile("file id");
await Bot.SendDocumentAsync(message.Chat, inputOnlineFile);
```

## Uploading by URL

```csharp
InputOnlineFile inputOnlineFile = new InputOnlineFile("telegram.org/img/t_logo.png");
await Bot.SendDocumentAsync(message.Chat, inputOnlineFile);
```