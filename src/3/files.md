# Downloading files

To download a file, you have to know its file identifier: `FileId`.  
You find this property in the [`Animation`], [`Audio`], [`Document`], [`Video`], [`VideoNote`], [`Voice`], [`Sticker`] objects from a message.

For a `Photo`, you get an array `PhotoSize[]` with `FileId` for each resolution variants.
The last entry contains the best quality: `message.Photo[^1].FileId`  
For `ChatPhoto`, there is a `BigFileId` for best quality, and `SmallFileId` for low resolution.  

## Methods for downloading a file

Downloading a file from Telegram is done in two steps:
1. Get file information with [`GetFile`] method.
2. Download the file with the `DownloadFile` method

```csharp
var fileId = update.Message.Video.FileId;
var tgFile = await bot.GetFile(fileId);

await using var stream = File.Create("../downloaded.mp4");
await bot.DownloadFile(tgFile, stream);
```

For your convenience, the library provides a helper function that does both steps: `GetInfoAndDownloadFile`

```csharp
await using var ms = new MemoryStream();
var tgFile = await bot.GetInfoAndDownloadFile(fileId, ms);
```

Notes:
- ⚠️ Bot API can download files up to 20 MB only. For bigger files, consider using [library WTelegramBot](https://www.nuget.org/packages/WTelegramBot#readme-body-tab)
- When downloading into a `MemoryStream`, remember to reset its `Position` before processing the content.
- The `tgFile.FilePath` returned by `GetFile` can be used to build the web URL accessing the file: `https://api.telegram.org/file/bot<TOKEN>/<FilePath>`.  
  _(don't give this URL publicly! your TOKEN must remain secret!)_

# Uploading files

We recommend you first read the official [documentation on sending files](https://core.telegram.org/bots/api#sending-files), it contains important information.

## Upload local file

To upload a file from your machine, open the stream and call one of the sending methods:

```csharp
await using var stream = File.OpenRead("../hamlet.pdf");
var message = await bot.SendDocument(chatId, stream, "The Tragedy of Hamlet,\nPrince of Denmark");
```

You can also specify the public filename manually, or use a MemoryStream:
```csharp
var buffer = File.ReadAllBytes("../hamlet.pdf");
await using var ms = new MemoryStream(buffer);
var message = await bot.SendDocument(chatId, InputFile.FromStream(ms, "Tragedy.pdf"),
                                     "The Tragedy of Hamlet,\nPrince of Denmark");
```

Be aware of limitation for this method - 10 MB max size for photos, 50 MB for other files. For bigger files, consider using [library WTelegramBot](https://www.nuget.org/packages/WTelegramBot#readme-body-tab)

## Upload by file identifier

If the file is already stored somewhere on the Telegram servers, you don't need to reupload it: each file object has a `FileId` property. Simply pass this `FileId` as a parameter instead of uploading. There are no size limits for files sent this way.

```csharp
var fileId = update.Message.Photo[^1].FileId;
var message = await bot.SendPhoto(chatId, fileId);
```

## Upload by URL

Provide Telegram with an HTTP URL for the file to be sent. Telegram will download and send the file. 5 MB max size for photos and 20 MB max for other types of content.

```csharp
var message = await bot.SendPhoto(chatId, "https://picsum.photos/640/480.jpg");
```


[`GetFile`]: https://core.telegram.org/bots/api#getfile
[`PhotoSize`]: https://core.telegram.org/bots/api#photosize
[`Animation`]: https://core.telegram.org/bots/api#animation
[`Audio`]: https://core.telegram.org/bots/api#audio
[`Document`]: https://core.telegram.org/bots/api#document
[`Video`]: https://core.telegram.org/bots/api#video
[`VideoNote`]: https://core.telegram.org/bots/api#videonote
[`Voice`]: https://core.telegram.org/bots/api#voice
[`Sticker`]: https://core.telegram.org/bots/api#sticker
[`File`]: https://core.telegram.org/bots/api#file
