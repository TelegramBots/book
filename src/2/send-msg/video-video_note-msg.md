# Video and Video Note Messages

[![tests](https://img.shields.io/badge/Examples-Video_Messages-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/VideoMessageTests.cs)

You can send MP4 files as a regular video or a _video note_.
Other video formats may be sent as documents.

## Video

[![method sendVideo](https://img.shields.io/badge/Bot_API_method-sendVideo-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendvideo)

Videos, like other multimedia messages, can have caption, reply, reply markup, and etc.
You can optionally specify the duration and resolution of the video.

In the example below, we send a video of a 10 minute countdown
and expect the Telegram clients to stream that long video instead of downloading it completely.
We also set a thumbnail image for our video.

```c#
{{#include ../../../Examples/2/SendMessage.cs:send-video}}
```

> Check the Bot API docs for `sendVideo` method to learn more about video size limits and the thumbnail images.

![vide screenshot 1](../docs/shot-video_thumb1.jpg)

User should be able to seek through the video without the video being downloaded completely.

![vide screenshot 2](../docs/shot-video_thumb2.jpg)

## Video Note

[![method sendVideoNote](https://img.shields.io/badge/Bot_API_method-sendVideoNote-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendvideonote)

Video notes, shown in circles to the user, are usually short (1 minute or less) with the same width and height.

You can send a video note only by uploading the video file or reusing the `file_id` of another video note.
Sending video note by its HTTP URL is not supported currently.

Download the [Sea Waves video] to your disk for this example.

```c#
{{#include ../../../Examples/2/SendMessage.cs:send-video-note}}
```

![vide note screenshot](../docs/shot-video_note.jpg)

[Sea Waves video]: https://telegrambots.github.io/book/docs/video-waves.mp4
