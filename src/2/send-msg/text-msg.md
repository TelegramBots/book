# Text Messages and More

[![send message method](https://img.shields.io/badge/Bot_API_method-sendMessage-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendmessage)
[![tests](https://img.shields.io/badge/Examples-Text_Messages-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/TextMessageTests.cs)

Text is a powerful interface for your bot and [`sendMessage`] probably is the most used method of the Telegram Bot API.
Text messages are easy to send and fast to display on devices with slower networking.

_Don't send boring plain text to users all the time_. Telegram allows you to format the text using HTML or Markdown.
> [!IMPORTANT]  
> We highly recommend you use HTML instead of Markdown because Markdown has lots of annoying aspects

## Send Text Message

The code snippet below sends a message with multiple parameters that looks like this:

![text message screenshot](../docs/shot-text_msg2.jpg)

> You can use this code snippet in the event handler from Example Bot page and use `chatId`
> or put the `chatId` value if you know it.

```c#
{{#include ../../../Examples/2/SendMessage.cs:send-text}}
```

The method `SendMessage` of .NET Bot Client maps to [`sendMessage`] on Telegram's Bot API. This method sends a
text message and returns the message object sent.

`text` is written in [HTML format] and `parseMode` indicates that. You can also write in Markdown or plain text.

By passing `protectContent` we prevent the message (and eventual media) to be copiable/forwardable elsewhere.

It's a good idea to make it clear to a user the reason why the bot is sending this message and that's why we pass the user's
message id for `replyParameters`.

You have the option of specifying a `replyMarkup` when sending messages.
Reply markups are explained in details later in this book.
Here we used an _Inline Keyboard Markup_ with a button that attaches to the message itself. Clicking that opens
[`sendMessage`] method documentation in the browser.

## The Sent Message

Almost all of the methods for sending messages return you the message you just sent. Let's have a look at this object. Add this statement after the previous code.

```c#
{{#include ../../../Examples/2/SendMessage.cs:message-contents}}
```

Output should look similar to this:

```text
Awesome bot sent message 123 to chat 123456789 at 8/21/18 11:25:09 AM. It is a reply to message 122 and has 2 message entities.
```

There are a few things to note.

Date and time is in [UTC format] and not your local timezone.
Convert it to local time by calling `message.Date.ToLocalTime()` method.

[Message Entity] refers to those formatted parts of the text: _all the parameters_ in bold and
_sendMessage_ in mono-width font.
Property `message.Entities` holds the formatting information and `message.EntityValues` gives you the actual value.
For example, in the message we just sent:

```c#
message.Entities.First().Type == MessageEntityType.Bold
message.EntityValues.First()  == "all the parameters"
```

Try putting a breakpoint in the code to examine all the properties on a message objects you get.

[`sendMessage`]: https://core.telegram.org/bots/api#sendmessage
[HTML format]: https://core.telegram.org/bots/api#html-style
[UTC format]: https://en.wikipedia.org/wiki/Coordinated_Universal_Time
[Message Entity]: https://core.telegram.org/bots/api#messageentity
