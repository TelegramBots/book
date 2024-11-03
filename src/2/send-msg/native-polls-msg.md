# Native Poll Messages

[![native poll tests](https://img.shields.io/badge/Examples-Native_Polls-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/Polls/AnonymousPollTests.cs)

Native poll are a special kind of message with question & answers where users can vote. Options can be set to allow multiple answers, vote anonymously, or be a quizz with a correct choice and explanation.

## Send a poll

[![sendPoll method](https://img.shields.io/badge/Bot_API_method-sendPoll-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#sendpoll)

This is the code to send a poll to a chat.

```c#
{{#include ../../../Examples/2/SendMessage.cs:send-poll}}
```

![native poll](../docs/shot-native_poll_msg.jpeg)

You can optionally send a keyboard with a poll, both an inline or a regular one.

You'll get the message with [`Poll`](https://github.com/TelegramBots/Telegram.Bot/blob/master/src/Telegram.Bot/Types/Poll.cs) object inside it.

## Stop a poll

[![stopPoll method](https://img.shields.io/badge/Bot_API_method-stopPoll-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#stoppoll)

To close a poll you need to know original chat and message ids of the poll that you got from calling `SendPoll` method.

Let's close the poll that we sent in the previous example:

```c#
{{#include ../../../Examples/2/SendMessage.cs:stop-poll}}
```

![closed native poll](../docs/shot-native_poll_closed.jpeg)

You can add an inline keyboard when you close a poll.

As a result of the request you'll get the the final poll state with property `Poll.IsClosed` set to true.

If you'll try to close a forwarded poll using message and chat ids from the received message even if your bot is the author of the poll you'll get an `ApiRequestException` with message `Bad Request: poll can't be stopped`. Polls originated from channels is an exception since forwarded messages originated from channels contain original chat and message ids inside properties `Message.ForwardFromChat.Id` and `Message.ForwardFromMessageId`.

Also if you'll try to close an already closed poll you'll get `ApiRequestException` with message `Bad Request: poll has already been closed`.
