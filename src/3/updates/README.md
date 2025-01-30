# Working with Updates & Messages

## Getting Updates

There are two mutually exclusive ways of receiving updates for your bot — the long polling using [`getUpdates`] method on one hand and Webhooks on the other. Telegram is queueing updates until the bot receives them either way, but they will not be kept longer than 24 hours.

- [With long polling](polling.md), the client is actively requesting updates from the server in a blocking way. The call returns if new updates become available or a timeout has expired.
- [Setting a webhook](webhook.md) means you supplying Telegram with a location in the form of an URL, on which your bot listens for updates. Telegram need to be able to connect and post updates to that URL.

## Update types 

[![Update type](https://img.shields.io/badge/Bot_API_type-Update-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#update)

Each user interaction with your bot results in an Update object.
It could be about a Message, some changed status, bot-specific queries, etc...  
You can use `update.Type` to check which kind of update you are dealing with.

However this property is slow and just indicates which field of `update` is set, and the other fields are all null.
So it is recommended to instead directly test the fields of Update you want if they are non-null, like this:
```csharp
switch (update)
{
    case { Message: { } msg }: await HandleMessage(msg); break;
    case { EditedMessage: { } editedMsg }: await HandleEditedMessage(editedMsg); break;
    case { ChannelPost: { } channelMsg }: await HandleChannelMessage(channelMsg); break;
    case { CallbackQuery: { } cbQuery }: await HandleCallbackQuery(cbQuery); break;
    //...
}
```


## Message types

[![Message type](https://img.shields.io/badge/Bot_API_type-Message-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#message)

If the Update is one of the 6 types of update containing a message _(new or edited? channel? business?)_, the contained `Message` object itself can be of various types.

Like above, you can use `message.Type` to determine the type but it is recommended to directly test the non-null fields of `Message` using `if` or `switch`.

There are a few dozens of message types, grouped in two main categories: **Content** and **Service** messages

### Content messages

These messages represent some actual content that someone posted.

Depending on which field is set, it can be:
- `Text`: a basic text message _(with its `Entities` for font effects, and `LinkPreviewOptions` for preview info)_
- `Photo`, `Video`, `Animation` (GIF), `Document` (file), `Audio`, `Voice`, `PaidMedia`: those are media contents which can come with a `Caption` subtext _(and its `CaptionEntities`)_
- `VideoNote`, `Sticker`, `Dice`, `Game`, `Poll`, `Venue`, `Location`, `Story`: other kind of messages without a caption

You can use methods `message.ToHtml()` or `message.ToMarkdown()` to convert the text/caption & entities into HTML **(recommended)** or Markdown.

### Service messages

All other message types represent some action/status that happened in the chat instead of actual content.

We are not listing all types here, but it could be for example:
- members joined/left
- pinned message
- chat info/status/topic changed
- [payment](../../4/payments.md)/[passport](../../4/passport)/giveaway process update
- etc...

### Common properties

There are additional properties that gives you information about the context of the message.

Here are a few important properties:
- `Id`: the ID that you will use if you need to reply or call a method acting on this message
- `Chat`: in which chat the message arrived
- `From`: which user posted it
- `Date`: timestamp of the message (in UTC)
- `ReplyToMessage`: which message this is a reply to
- [`ForwardOrigin`](../../2/forward-copy-delete.md#check-if-a-message-is-a-forward): if it is a Forwarded message
- `MediaGroupId`: albums (group of media) are separate consecutive messages having the same MediaGroupId
- `MessageThreadId`: the topic ID for Forum/Topic type chats

## Sequential vs parallel updates
Whether polling in a loop or with [webhook](webhook.md#updates-are-posted-sequentially-to-your-webapp), you will always receive updates in sequential order of increasing `update.Id`, one after the other.

If you want to parallelize the handling of updates for improved performance, it is up to your async code.  
There are multiple possible approaches:
- write the received update into a [threading Channel](https://learn.microsoft.com/en-us/dotnet/core/extensions/channels)  
  You will need separate consumer Task(s) to process these updates _(see [Background Service](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services))_
- do the same but with a `ConcurrentQueue` or a `Queue` (with `lock`)  
- spawn a new sub-Task for each update, using `Task.Run` for example  
  (if your bot is heavily used, make sure you don't overload your server with concurrent tasks)
- or something as simple as: `bot.OnUpdate += async update => OnUpdate(update);`

However if you're gonna process the updates in parallel, you might want to ensure your code:
- is thread-safe or async-safe when accessing common resources
- has no state-consistency issue processing updates in unsequential order


## Example projects

### Long polling

- [Console application](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Console). Demonstrates a basic bot with some commands.
- [Advanced console application](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Console.Advanced). Demonstrates the use of many advanced programming features.

### Webhook

- [ASP.NET Core](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Webhook.MinimalAPIs) web application with Minimal APIs
- [ASP.NET Core](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Webhook.Controllers) web application with Controllers
- [Azure Functions](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Serverless/AzureFunctions.Webhook)
- [AWS Lambda](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Serverless/AwsLambda.Webhook)

[`getUpdates`]: https://core.telegram.org/bots/api#getupdates
