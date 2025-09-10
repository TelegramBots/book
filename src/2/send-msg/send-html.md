# SendHtml helper method

SendHtml is an all-in-one helper method to send various type of messages in a very simple way.  
It's using [HTML-formatted text](https://core.telegram.org/bots/api#html-style), with extra tags to support the sending of medias and [attached keyboard](#sending-keyboards).

_Note: method is only available for .NET 6+_

Look at the examples below to see how easy it is to use:

### Sending a simple HTML text message
```csharp
var msg = await bot.SendHtml(chatId, """
    Try the new <code>SendHtml</code> method...
    It is <tg-spoiler>awesome!!</tg-spoiler>
    """);
```

### Sending a photo with caption
```csharp
var msg = await bot.SendHtml(chatId, """
    <img src="https://telegrambots.github.io/book/docs/photo-ara.jpg">
    <b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>
    """);
```

### Sending a photo with spoiler
```csharp
var msg = await bot.SendHtml(chatId, """
    <img src="https://telegrambots.github.io/book/docs/photo-ara.jpg" spoiler>
    <b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>
    """);
```

### Sending with caption above the photo
```csharp
var msg = await bot.SendHtml(chatId, """
    <b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>
    <img src="https://telegrambots.github.io/book/docs/photo-ara.jpg" spoiler>
    """);
```

The position of text before or after `<img>` and `<video>` determines if the caption should appear above or below the medias.

### Sending a video
```csharp
var msg = await bot.SendHtml(chatId, """
    <video src="https://telegrambots.github.io/book/docs/video-countdown.mp4">
    """);
```

### Sending a video with FileID instead of URL _(and spoiler)_
```csharp
var msg = await bot.SendHtml(chatId, $"""
    <video src="{previousMsg.Video.FileID}" spoiler>
    We use a FileID as the video src
    """);
```

### Sending an album with 2 photos
```csharp
var msg = await bot.SendHtml(chatId, """
    <img src="https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg">
    <img src="https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg">
    Album caption (attached to the last photo)
    """);
```

### Sending an album with caption attached to first media
```csharp
var msg = await bot.SendHtml(chatId, """
    <img src="https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg">
    Album caption (attached to the first photo)
    <img src="https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg">
    """);
```

### Sending an album with caption above the medias
```csharp
var msg = await bot.SendHtml(chatId, """
    Album caption above the photos
    <img src="https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg">
    <img src="https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg">
    """);
```

### Sending an album of files with caption on each file
```csharp
var msg = await bot.SendHtml(chatId, """
    <file src="https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf">
    Caption of 1st file
    <file src="https://www.w3.org/WAI/ER/tests/xhtml/testfiles/resources/pdf/dummy.pdf">
    Caption of 2nd file
    """);
```

### Sending an album of audio files
```csharp
var msg = await bot.SendHtml(chatId, """
    <file src="https://upload.wikimedia.org/wikipedia/commons/transcoded/b/bb/Test_ogg_mp3_48kbps.wav/Test_ogg_mp3_48kbps.wav.mp3">
    <file src="https://upload.wikimedia.org/wikipedia/commons/transcoded/b/bb/Test_ogg_mp3_48kbps.wav/Test_ogg_mp3_48kbps.wav.mp3">
    Listen to these audio files
    """);
```

### Sending an album with uploaded streams
```csharp
await using var stream0 = File.OpenRead(@"C:\Pictures\banner.png");
await using var stream1 = File.OpenRead(@"C:\Pictures\image.jpg");
var msg = await bot.SendHtml(chatId, """
    <img src="image.jpg">
    <img src="stream://0">
    """, streams: [stream0, stream1]);
```

Pass a stream list in method argument `streams:`, then reference them in html `src="..."` as `stream://N` or `stream:N` or just `N`

N being the indice in the streams list (starting with 0), or the filename for `FileStream`s

## Customizing the preview for text messages
```csharp
var msg = await bot.SendHtml(chatId, """
    https://github.com/TelegramBots/book
    <preview disabled>
    """);

var msg2 = await bot.SendHtml(chatId, """
    Hello, world
    <preview url="https://github.com/TelegramBots/book" small above>
    """);
```
Tag `<preview>` supports the following optional attributes _(in this order)_:
- `disable` or `disabled` _(no preview even if text contains links)_
- `url="https://..."` _(force preview for this URL not present in the text)_
- `small` or `large` _(size of the preview image)_
- `above`

## Sending keyboards

Note: the `<keyboard>..</keyboard>` section must be at the end of the message

### Text message with one Inline button
```csharp
var msg = await bot.SendHtml(chatId, """
    Simple message <u>with a button</u>
    <keyboard>
    <button text="URL button" url="example.com">
    </keyboard>
    """);
```

### Photo with two callback buttons
```csharp
var msg = await bot.SendHtml(chatId, """
    <img src="https://telegrambots.github.io/book/docs/photo-ara.jpg">
    Do you like this photo?
    <keyboard>
    <button text="Yes" callback="ara-yes">
    <button text="No" callback="ara-no">
    </keyboard>
    """);
```

### Multi-row keyboard and various types of inline buttons
```csharp
var msg = await bot.SendHtml(chatId, """
    Inline keyboard example
    <keyboard>
    <button text="URL button" url="https://github.com/TelegramBots/Telegram.Bot">
    <button text="Callback btn" callback="data">
    <row>
    <button text="Copy btn" copy="Some Text">
    <button text="Switch inline" switch_inline="query" target="user,bot">
    </row>
    </keyboard>
    """);
```
Note: Button type `app="<AppURL>"` works only in private chats

### Using Reply buttons
```csharp
var msg = await bot.SendHtml(chatId, """
    Reply keyboard example
    <keyboard reply>
    <button text="Yes">
    <button text="No">
    <button text="Maybe">
    <row>
    <button text="Send my contact" request_contact>
    <button text="Send my location" request_location>
    <button text="Send a poll" request_poll="any">
    </keyboard>
    """);
```

Notes:
- Closing tag `</row>` is optional.
- Button type `request_poll_=".."` supports values: `any`, `quiz`, `regular`
- Button type `app="<AppURL>"` works only in private chats

### Removing the Reply keyboard
```csharp
var msg = await bot.SendHtml(chatId, """
    Thank you
    <keyboard reply_remove></keyboard>
    """);
```

### Force the user to reply to this message
```csharp
var msg = await bot.SendHtml(chatId, """
    Please give your name in reply to this message
    <keyboard reply_force="Type your name here"></keyboard>
    """);
```
