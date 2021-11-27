# Uploading files

First, read the [documentation on sending files](https://core.telegram.org/bots/api#sending-files).

## Upload an actual file

```csharp
using (FileStream stream = System.IO.File.OpenRead("Local file.pdf"))
{
    InputOnlineFile inputOnlineFile = new InputOnlineFile(stream, "Name for the user.pdf");
    await botClient.SendDocumentAsync(chatId, inputOnlineFile);
}
```

## Upload file by file_id

This is the method that you will want to use the most, as it is the most efficient.
After sending a file to the user, or receiving a file, you can send the file again using its ID.

This saves you from uploading the entire file.

```csharp
InputOnlineFile inputOnlineFile = new InputOnlineFile("file id");
await botClient.SendDocumentAsync(chatId, inputOnlineFile);
```

## Uploading by URL

```csharp
InputOnlineFile inputOnlineFile = new InputOnlineFile("telegram.org/img/t_logo.png");
await Bot.SendDocumentAsync(chatId, inputOnlineFile);
```
