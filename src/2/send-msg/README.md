# Sending Messages

There are many different types of message that a bot can send.
Fortunately, methods for sending such messages are similar. Take a look at these examples:

**Sending text message**:

![text message screenshot](../docs/shot-text_msg.jpg)

```c#
await botClient.SendTextMessageAsync(
  chatId: e.Message.Chat,
  text:   "Hello, World!"
);
```

**Sending sticker message**:

![sticker message screenshot](../docs/shot-sticker.jpg)

```c#
await botClient.SendStickerAsync(
  chatId:  e.Message.Chat,
  sticker: "https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"
);
```

**Sending video message**:

![video message screenshot](../docs/shot-video.jpg)

```c#
await botClient.SendVideAsync(
  chatId:  e.Message.Chat,
  video: "https://github.com/TelegramBots/book/raw/master/src/docs/video-bulb.mp4"
);
```
