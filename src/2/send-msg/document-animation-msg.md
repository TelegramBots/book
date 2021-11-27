# Document and Animation Messages

## Send documents

[![sendDocument method](https://img.shields.io/badge/Bot_API_method-sendDocument-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#senddocument)
[![tests](https://img.shields.io/badge/Examples-Document_Message-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/DocumentMessageTests.cs)

Use [`sendDocument`] method to send general files.

```c#
Message message = await botClient.SendDocumentAsync(
    chatId: chatId,
    document: "https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg",
    caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
    parseMode: ParseMode.Html,
    cancellationToken: cancellationToken);
```

## Send animations

[![sendAnimation method](https://img.shields.io/badge/Bot_API_method-sendAnimation-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendanimation)
[![tests](https://img.shields.io/badge/Examples-Animation_Message-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/AnimationMessageTests.cs)

Use [`sendAnimation`] method to send animation files (GIF or H.264/MPEG-4 AVC video without sound).

```c#
Message message = await botClient.SendAnimationAsync(
    chatId: chatId,
    animation: "https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/video-waves.mp4",
    caption: "Waves",
    cancellationToken: cancellationToken);
```

[`sendDocument`]: https://core.telegram.org/bots/api#senddocument
[`sendAnimation`]: https://core.telegram.org/bots/api#sendanimation
