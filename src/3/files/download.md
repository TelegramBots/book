# Downloading files

First, read the [documentation on `getFile`](https://core.telegram.org/bots/api#getfile) method.

To download file you have to know its file identifier - `FileId`.

## Finding the file identifier

Telegram Bot API has several object types, representing file:
[`PhotoSize`], [`Animation`], [`Audio`], [`Document`], [`Video`], [`VideoNote`], [`Voice`], [`Sticker`].

The file identifier for each file type can be found in their `FileId` property (e.g. `Message.Audio.FileId`).

The exception is photos, which represented as an array of [`PhotoSize[]`][`PhotoSize`] objects.
For each photo Telegram sends you a set of [`PhotoSize`] objects - available resolutions, you can choose from.
Generally, you will want the highest quality - the last [`PhotoSize`] object in the array.
With LINQ, this boils down to `Message.Photo.Last().FileId`.

## Downloading a file

Downloading a file from Telegram is done in two steps:

1. Get file information with `getFile` method. Resulting [`File`] object contains `FilePath` from which we can download the file.
2. Downloading the file.

```C#
var fileId = update.Message.Photo.Last().FileId;
var fileInfo = await botClient.GetFileAsync(fileId);
var filePath = fileInfo.FilePath;
```

The URL from which you can now download the file is `https://api.telegram.org/file/bot<token>/<FilePath>`.

To download file you can use `DownloadFileAsync` function:

```C#
string destinationFilePath = $"../downloaded.file";
await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
await botClient.DownloadFileAsync(
    filePath: filePath,
    destination: fileStream);
```

For your convenience the library provides you a helper function that does both - `GetInfoAndDownloadFileAsync`:

```C#
string destinationFilePath = $"../downloaded.file";
await using FileStream fileStream = System.IO.File.OpenWrite(destinationFilePath);
var file = await botClient.GetInfoAndDownloadFileAsync(
    fileId: fileId,
    destination: fileStream);
```

[`PhotoSize`]: https://core.telegram.org/bots/api#photosize
[`Animation`]: https://core.telegram.org/bots/api#animation
[`Audio`]: https://core.telegram.org/bots/api#audio
[`Document`]: https://core.telegram.org/bots/api#document
[`Video`]: https://core.telegram.org/bots/api#video
[`VideoNote`]: https://core.telegram.org/bots/api#videonote
[`Voice`]: https://core.telegram.org/bots/api#voice
[`Sticker`]: https://core.telegram.org/bots/api#sticker
[`File`]: https://core.telegram.org/bots/api#file
