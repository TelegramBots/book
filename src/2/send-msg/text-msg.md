# Text Messages and More

[![send message method](https://img.shields.io/badge/Bot_API_method-send_message-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendmessage)
[![tests](https://img.shields.io/badge/Test_cases-here-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Sending%20Messages/TextMessageTests.cs)

Text is a powerful interface for your bot and [`sendMessage`] probably is the most used method of Bot API.
Text messages are easy to send and fast to show on devices with slower network.
_Don't send boring plain text to users all the time_. Telegram allows you to format the text using Markdown or HTML.

## Send Text Message

Code snippet below send a message with multiple parameters that looks like this:

![text message screenshot](../docs/shot-text_msg2.jpg)

> You can use this code snippet in the event handler from Example Bot page and use `e.Message.Chat`
> or put the `chatId` value if you know it.

```c#
// using Telegram.Bot.Types;
// using Telegram.Bot.Types.Enums;
// using Telegram.Bot.Types.ReplyMarkups;

Message message = await botClient.SendTextMessageAsync(
  chatId: e.Message.Chat, // or a chat id: 123456789
  text: "Trying *all the parameters* of `sendMessage` method",
  parseMode: ParseMode.Markdown,
  disableNotification: true,
  replyToMessageId: e.Message.MessageId,
  replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(
    "Check sendMessage method",
    "https://core.telegram.org/bots/api#sendmessage"
  ))
);
```

Method `SendTextMessageAsync` of .NET Bot Client maps to [`sendMessage`] on Telegram Bot API. This method sends a
text message and returns the message object sent.

`text` is written in [MarkDown format] and `parseMode` indicates that. You can also write in HTML or plain text.

By passing `disableNotification` we tell Telegram client on user's device not to show/sound a notification.

It's a good idea to make it clear to user the reason bot is sending this message and that's why we pass user's
message id for `replyToMessageId`.

You have the option of specifying a `replyMarkup` when sending messages.
Reply markups are explained in details later in this book.
Here we used an _Inline Keyboard Markup_ with a button that attaches to the message itself. Clicking that opens
[`sendMessage`] method documentation in browser.

## The Message Sent

Almost all of the methods for sending messages return you the message you just sent. Let's have a look at this
message object. Add this statement after after previous code.

```c#
Console.WriteLine(
  $"{message.From.FirstName} sent message {message.MessageId} " +
  $"to chat {message.Chat.Id} at {message.Date}. " +
  $"It is a reply to message {message.ReplyToMessage.MessageId} " +
  $"and has {message.Entities.Length} message entities."
);
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

Currently, message object doesn't contain information about its reply markup.

Try putting a breakpoint in code to examine all the properties on message objects you get.

[`sendMessage`]: https://core.telegram.org/bots/api#sendmessage
[MarkDown format]: https://en.wikipedia.org/wiki/Markdown
[UTC format]: https://en.wikipedia.org/wiki/Coordinated_Universal_Time
[Message Entity]: https://core.telegram.org/bots/api#messageentity