# Sending Messages

There are many different types of message that a bot can send.
Fortunately, methods for sending such messages are similar. Take a look at these examples:

**Sending text message**:

![text message screenshot](../docs/shot-text_msg.jpg)

```c#
var message = await botClient.SendTextMessageAsync(
  chatId: chatId,
  text:   "Hello, World!"
);
```

**Sending sticker message**:

![sticker message screenshot](../docs/shot-sticker.jpg)

```c#
await botClient.SendStickerAsync(
  chatId:  chatId,
  sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"
);
```

**Sending video message**:

![video message screenshot](../docs/shot-video.jpg)

```c#
await botClient.SendVideoAsync(
  chatId:  chatId,
  video: "https://github.com/TelegramBots/book/raw/master/src/docs/video-bulb.mp4"
);
```
