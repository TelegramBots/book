# Audio and Voice Messages

These two types of messages are pretty similar. An audio is in MP3 format and gets displayed in music player.
A voice file has OGG format and is not shown in music player.

## Audio

[![send audio method](https://img.shields.io/badge/Bot_API_method-send_audio-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendaudio)
[![audio tests](https://img.shields.io/badge/Examples-Audio_Messages-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/AudioMessageTests.cs)

```c#
Message msg = await botClient.SendAudioAsync(
  chatId: e.Message.Chat,
  audio: "https://github.com/TelegramBots/book/raw/master/src/docs/audio-fun_guitar.mp3"
  // , performer: "Joel Thomas Hunger"
  // , title: "Fun Guitar and Ukulele"
  // , duration: 91
);
```

## Voice
