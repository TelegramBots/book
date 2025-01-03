# Forward, Copy or Delete messages

You can forward, copy, or delete a single message, or even a bunch of messages in one go.

You will need to provide the source messageId(s), the source chatId and eventually the target chatId.

_Note: When you use the plural form of the copy/forward methods, it will keep Media Groups (albums) as such._

## Forward message(s)

[![forward message method](https://img.shields.io/badge/Bot_API_method-forwardMessage-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#forwardmessage)
[![forward messages method](https://img.shields.io/badge/Bot_API_method-forwardMessages-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#forwardmessages)

You can **forward** message(s) from a source chat to a target chat _(it can be the same chat)_.
They will appear with a "Forwarded from" header.

```csharp
// Forward a single message
await bot.ForwardMessage(targetChatId, sourceChatId, messageId);

// Forward an incoming message (from the update) onto a target ChatId
await bot.ForwardMessage(chatId, update.Message.Chat, update.Message.Id);

// Forward a bunch of messages from a source ChatId to a target ChatId, using a list of their message ids
await bot.ForwardMessages(targetChatId, sourceChatId, new int[] { 123, 124, 125 });
```

## Copy message(s)

[![copy message method](https://img.shields.io/badge/Bot_API_method-copyMessage-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#copymessage)
[![copy messages method](https://img.shields.io/badge/Bot_API_method-copyMessages-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#copymessages)

If you don't want the "Forwarded from" header, you can instead **copy** the message(s).

This will make them look like new messages.

```csharp
// Copy a single message
await bot.CopyMessage(targetChatId, sourceChatId, messageId);

// Copy an incoming message (from the update) onto a target ChatId
await bot.CopyMessage(targetChatId, update.Message.Chat, update.Message.Id);

// Copy a media message and change its caption at the same time
await bot.CopyMessage(targetChatId, update.Message.Chat, update.Message.Id,
    caption: "New <b>caption</b> for this media", parseMode: ParseMode.Html);

// Copy a bunch of messages from a source ChatId to a target ChatId, using a list of their message ids
await bot.CopyMessages(targetChatId, sourceChatId, new int[] { 123, 124, 125 });
```

## Delete message(s)

[![delete message method](https://img.shields.io/badge/Bot_API_method-deleteMessage-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#deletemessage)
[![delete messages method](https://img.shields.io/badge/Bot_API_method-deleteMessages-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#deletemessages)

Finally you can **delete** message(s).

This is particularly useful for cleaning unwanted messages in groups.

```csharp
// Delete a single message
await bot.DeleteMessage(chatId, messageId);

// Delete an incoming message (from the update)
await bot.DeleteMessage(update.Message.Chat, update.Message.Id);

// Delete a bunch of messages, using a list of their message ids
await bot.DeleteMessages(chatId, new int[] { 123, 124, 125 });
```

## Check if a message is a forward

When receiving an update about a message, you can check if that message is "Forwarded from" somewhere,
by checking if `Message.ForwardOrigin` is set:

```csharp
Console.WriteLine(update.Message.ForwardOrigin switch
{
    MessageOriginChannel moc     => $"Forwarded from channel {moc.Chat.Title}",
    MessageOriginUser mou        => $"Forwarded from user {mou.SenderUser}",
    MessageOriginHiddenUser mohu => $"Forwarded from hidden user {mohu.SenderUserName}",
    MessageOriginChat moch       => $"Forwarded on behalf of {moch.SenderChat}",
    _                            => "Not forwarded"
});
```
