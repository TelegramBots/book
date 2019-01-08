First, read the [documentation on `getFile`](https://core.telegram.org/bots/api#getfile).

To download a file you will need its ID.

## Finding the FileId

There are many file objects in the Telegram Bot API:
[`Document`], [`Audio`], [`Voice`], [`Video`], [`VideoNote`], [`Animation`], [`Sticker`], `Photo`.

The FileId for each will be found in their `FileId` property (E.g. `Message.Audio.FileId`).

The exception to that is the Photo, which is a [`PhotoSize[]`][`PhotoSize`].
Telegram sends you a few different resolutions for each photo that you can choose from.
Generaly, you will want the highest quality - the last [`PhotoSize`] in the array.
Using Linq, this boils down to `Message.Photo.Last().FileId`.

## Downloading a file

Downloading a file from Telegram servers is done in two steps:
1. Calling `getFile` to receive a URL from which we can download the file
2. Downloading the file

Calling `Bot.GetFileAsync(fileId)` will return a [`File`] object that contains a `file_path`.

The URL from which you can now download the file is `https://api.telegram.org/file/bot<token>/<file_path>`.

The library provides you with a helper function that does both - `GetInfoAndDownloadFileAsync`.

## Downloading into a FileStream

```csharp
using (FileStream fs = System.IO.File.OpenWrite("File.pdf"))
    await Bot.GetInfoAndDownloadFileAsync(message.Document.FileId, fs);
```

## Downloading to a byte array

```csharp
byte[] document = await Bot.GetInfoAndDownloadFileAsync(message.Document.FileId);
```

[`Document`]: https://core.telegram.org/bots/api#document
[`Audio`]: https://core.telegram.org/bots/api#audio
[`Voice`]: https://core.telegram.org/bots/api#voice
[`Video`]: https://core.telegram.org/bots/api#video
[`VideoNote`]: https://core.telegram.org/bots/api#videonote
[`Animation`]: https://core.telegram.org/bots/api#animation
[`Sticker`]: https://core.telegram.org/bots/api#sticker
[`PhotoSize`]: https://core.telegram.org/bots/api#photosize
[`File`]: https://core.telegram.org/bots/api#file