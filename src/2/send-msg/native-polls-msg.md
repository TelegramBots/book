# Native Poll Messages

[![native poll tests](https://img.shields.io/badge/Examples-Native_Polls-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Polls/PollMessageTests.cs)

Bots just as regular users can send native polls only to groups and channels, but not to private chats.

## Send a poll

[![sendPoll method](https://img.shields.io/badge/Bot_API_method-send_poll-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendpoll)

This is the code to send a poll to a chat.

```c#
ChatId chatId = "@group_or_channel_username";
Message pollMessage = await botClient.SendPollAsync(
    chatId: chatId,
    question: "Did you ever hear the tragedy of Darth Plagueis The Wise?",
    options: new []
    {
        "Yes for the hundredth time!",
        "No, who`s that?"
    },
    replyMarkup: ... // optional IReplyMarkup
);
```

![native poll](../docs/shot-native_poll_msg.jpeg)

You can optionally send a keyboard with a poll, both an inline and a regular one.

You'll get the message with [`Poll`](https://github.com/TelegramBots/Telegram.Bot/blob/master/src/Telegram.Bot/Types/Poll.cs) object inside it. Each poll has a unique `Id`. It doesn't change when you forward the message with a poll. With this `Id` you can track poll state changes when it's updated. To do that you need to store a poll somewhere.

Whenever a user votes in a poll created by your bot the bot receives an `Update` with a new poll state inside `Update.Poll` property. This is the time to update the state of your poll if you track it.

## Stop a poll

[![stopPoll method](https://img.shields.io/badge/Bot_API_method-stop_poll-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#stoppoll)

To close a poll you need to know original chat and message ids of the poll that you got from calling `SendPollAsync` method.

 Let's close the poll that we sent in the previous example:

```c#
Poll poll = await botClient.StopPollAsync(
    chatId: pollMessage.Chat.Id,
    messageId: pollMessage.MessageId,
    replyMarkup: ... // optional InlineKeyboardMarkup
);
```

![closed native poll](../docs/shot-native_poll_closed.jpeg)

You can also send a new inline keyboard when you close a poll.

As a result of the request you'll get the the final poll state with property `Poll.IsClosed` set to true.

If you'll try to close a forwarded poll using message and chat ids from the received message even if your bot is the author of the poll you'll get an `ApiRequestException` with message `Bad Request: poll can't be stopped`. Polls originated from channels is an exception since forwarded messages originated from channels contain original chat and message ids inside properties `Message.ForwardFromChat.Id` and `Message.ForwardFromMessageId`.

Also if you'll try to close an already closed poll you'll get `ApiRequestException` with message `Bad Request: poll has already been closed`.
