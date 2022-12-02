# Album Messages

[![send media group method](https://img.shields.io/badge/Bot_API_method-sendMediaGroup-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendmediagroup)
[![tests](https://img.shields.io/badge/Examples-Album_Messages-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/AlbumMessageTests.cs)

Using [`sendMediaGroup`] method you can send a group of photos, videos, documents or audios as an album. Documents and audio files can be only grouped in an album with messages of the same type.

```c#
{{#include ../../../Examples/2/SendMessage.cs:send-media-group}}
```

[`sendMediaGroup`]: https://core.telegram.org/bots/api#sendmediagroup
