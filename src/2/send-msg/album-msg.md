# Album Messages

[![send media group method](https://img.shields.io/badge/Bot_API_method-sendMediaGroup-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendmediagroup)
[![tests](https://img.shields.io/badge/Examples-Album_Messages-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/AlbumMessageTests.cs)

Using [`sendMediaGroup`] method you can send a group of photos, videos, documents or audios as an album. Documents and audio files can be only grouped in an album with messages of the same type.

```c#
Message[] messages = await botClient.SendMediaGroupAsync(
    chatId: chatId,
    media: new IAlbumInputMedia[]
    {
        new InputMediaPhoto("https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg"),
        new InputMediaPhoto("https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg"),
    },
    cancellationToken: cancellationToken);
```

[`sendMediaGroup`]: https://core.telegram.org/bots/api#sendmediagroup
