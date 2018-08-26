# Video and Video Note Messages

You can send MP4 files as a regular video or a video note.
A video note is short and usually is shown in a circle to user. See examples below.

## Video

## Video Note

You can send a video note only by uploading the video file or reusing file_id of another video note.
Sending video note by HTTP URL is not supported currently.

Download the [Sea Waves video] to your disk for this example. A video note file has the same width and height.

```c#
Message msg;
using (var stream = System.IO.File.OpenRead("/path/to/video-waves.mp4")) {
  msg = await botClient.SendVideoNoteAsync(
    chatId: e.Message.Chat,
    videoNote: stream,
    duration: 47,
    length: 360
  );
}
```

[Sea Waves video]: https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/video-waves.mp4