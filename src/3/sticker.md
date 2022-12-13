# Stickers

## Sticker

[![send sticker method](https://img.shields.io/badge/Bot_API_method-sendSticker-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendsticker)
[![sticker tests](https://img.shields.io/badge/Examples-Sticker_Messages-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Stickers/StickersTests.cs)

Telegram stickers are fun and our bot is about to send its very first sticker.
Sticker files should be in [WebP] format.

This code sends the same sticker twice. First by passing HTTP URL to a [WebP] sticker file and
second by reusing `FileId` of the same sticker on Telegram servers.

```c#
{{#include ../../Examples/2/SendMessage.cs:send-sticker}}
```

![sticker messages](docs/shot-sticker_msgs.jpg)

Try inspecting the `sticker1.Sticker` property. It is of type [`Sticker`] and its schema looks similar to a photo.

[WebP]: https://developers.google.com/speed/webp/
