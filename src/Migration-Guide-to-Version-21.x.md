# Migration guide for version 21.x

Important notes:
- Don't bother about version 20, migrate directly to version 21.*
- You won't find this version on Nuget: [See this guide to install it in your programs](https://telegrambots.github.io/book/index.html#-installation).
- Version 21.6 supports [Bot API 7.7](https://core.telegram.org/bots/api-changelog) _(including [Telegram Stars payments](#payments-with-telegram-stars))_
- Library is now based on System.Text.Json and doesn't depend on NewtonsoftJson anymore. _([See below](#webhooks-with-systemtextjson))_

## Renamed parameter _replyToMessageId:_ → _replyParameters:_

That parameter was renamed and you can still pass a **messageId** for simple replies.

Or you can pass a [ReplyParameters](https://core.telegram.org/bots/api#replyparameters) structure for more advanced reply configuration.  

## Renamed parameter _disableWebPagePreview:_ → _linkPreviewOptions:_

That parameter was renamed and you can still pass `true` to disable web preview.

Or you can pass a [LinkPreviewOptions](https://core.telegram.org/bots/api#linkpreviewoptions) structure for more precise preview configuration.  

## Changed `bool?` → `bool`

Many boolean parameters or fields are now simply of type `bool`.

In most cases, it shouldn't impact your existing code, or rather simplify it. Previously `null` values are now just `false`.

## Changed `ParseMode?` → `ParseMode`

When you don't need to specify a ParseMode, just pass `default` or `ParseMode.None`.

## Better backward-compatibility and simplification of code

We added/restored features & implicit conversions that make your code simpler:

- `InputFile`: just pass a `string`/`Stream` for file_id/url/stream content _(as was possible in previous versions of Telegram.Bot)_
- `InputMedia*`: just pass an `InputFile` when you don't need to associate caption or such
- `MessageId`: auto-converts to/from `int` (and also from `Message`)
- `ReactionType`: just pass a `string` when you want to send an emoji
- `ReactionType`: just pass a `long` when you want to send a custom emoji (id)
- Some other obvious implicit conversion operators for structures containing a single property
- No more enforcing `init;` properties, so you can adjust the content of fields as you wish or modify a structure returned by the API _(before passing it back to the API if you want)_
- No more JSON "required properties" during deserialization, so your old saved JSON files won't break if a field is added/renamed.
- Restored some `MessageType` enum values that were removed (renamed) recently (easier compatibility)

## MaybeInaccessibleMessage

This class hierarchy was introduced in Bot API 7.0 and broke existing code and added unnecessary complexity.

This was removed in our library v21 and you will just receive directly a Message _(as before)_.

To identify an "inaccessible message", you can just check `message.Type == MessageType.Unknown` or `message.Date == default`.

## Chat and ChatFullInfo

In previous versions, the big `Chat` structure contained many fields that were filled only after a call to GetChatAsync.

This structure is now split into `Chat` and `ChatFullInfo` structures.

The new `Chat` structure contains only common fields that are always filled.
The new `ChatFullInfo` structure inherits from `Chat` and is returned only by GetChatAsync method, with all the extra fields.

## Request structures

Request structures _(types ending with `Request`)_ are NOT the recommended way to use the library in your projects.

They are to be considered as low-level raw access to Bot API structures for advanced programmers, and might change/break at any time in the future.

If you have existing code using them, you can use the `MakeRequestAsync` method to send those requests.
_(Other methods based on those requests will be removed soon)_

## Payments with Telegram Stars

To make a payment in [Telegram Stars](https://t.me/BotNews/90) with SendInvoiceAsync, set the following parameters:
- `providerToken:` `null` or `""`
- `currency:` `"XTR"`
- `prices:` with a single price
- no tip amounts

## Webhooks with System.Text.Json

The library now uses `System.Text.Json` instead of `NewtonsoftJson`.

To make it work in your ASP.NET projects, you should now:
- Remove package **Microsoft.AspNetCore.Mvc.NewtonsoftJson** from your project dependencies
- Follow our [Webhook page](3/updates/webhook.md) to configure your web app correctly

## InputPollOption in SendPollAsync

SendPollAsync now expect an array of InputPollOption instead of string.

But we added an implicit conversion from string to InputPollOption, so the change is minimal:
```csharp
// before:
await bot.SendPollAsync(chatId, "question", new[] { "answer1", "answer2" });
// after:
await bot.SendPollAsync(chatId, "question", new InputPollOption[] { "answer1", "answer2" });
```

## Global cancellation token (v21.2)

You can now specify a global `CancellationToken` directly in TelegramBotClient constructor.

This way, you won't need to pass a cancellationToken to every method call after that
(if you just need one single cancellation token for stopping your bot)

## Polling system now catch exceptions in your HandleUpdate code (v21.3)

> [!WARNING]  
> That's a change of behaviour, but most of you will probably welcome this change

If you forgot to wrap your HandleUpdateAsync code in a big `try..catch`, and your code happen to throw an exception,
this would previously stop the polling completely.

Now the Polling system will catch your exceptions, pass them to your HandleErrorAsync method
**and continue the polling**.

>In previous versions of the library:
>- ReceiveAsync would throw out the exception (therefore stopping the polling)
>- StartReceiving would pass the exception to HandlePollingErrorAsync and silently stop the polling
>
>If you still want the previous behaviour, have your HandleErrorAsync start like this:
>```csharp
>Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, HandleErrorSource source, CancellationToken ct)
>{
>    if (source is HandleErrorSource.HandleUpdateError) throw ex;
>    ...
>```

## New helpers/extensions to simplify your code (v21.5)

- When replying to a message, you can now simply pass a `Message` for _replyParameters:_ rather than a `Message.MessageId`
- `Update.AllTypes` is a constant array containing all `UpdateType`s. You can pass it for the _allowedUpdates:_ parameter (`GetUpdatesAsync`/`SetWebhookAsync`)
- Message has now 2 extensions methods: `.ToHtml()` and `.ToMarkdown()` to convert the message text/caption and their entities into a simple Html or Markdown string.
- You can also use methods `Markdown.Escape()` and `HtmlText.Escape()` to sanitize reserved characters from strings
- Reply/Inline Keyboard Markup now have construction methods to simplify building keyboards dynamically:
```csharp
var replyMarkup = new InlineKeyboardMarkup()
    .AddButton(InlineKeyboardButton.WithUrl("Link to Repository", "https://github.com/TelegramBots/Telegram.Bot"))
    .AddNewRow().AddButton("callback").AddButton("caption", "data")
    .AddNewRow("with", "three", "buttons")
    .AddNewRow().AddButtons("A", "B", InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("switch"));
```
- Same for ReplyKeyboardMarkup (and you can use `new ReplyKeyboardMarkup(true)` to resize keyboard)

As [previously announced](#request-structures), the Request-typed methods are gone.
But you can still send Request structures via the `MakeRequestAsync` method.

## Simplified polling with events (v21.7)

Instead of `StartReceiving`/`ReceiveAsync` system, you can now simply set 2 or 3 events on the botClient:
- `bot.OnMessage += ...` to receive Message updates
- `bot.OnUpdate += ...` to receive other updates _(or <ins>all</ins> updates if you don't set `OnMessage`)_
- `bot.OnError += ...` to handle errors/exceptions during polling or your handlers

Note: The second argument to `OnMessage` events specify which kind of update it was (if it's an _edited_, _channel_ or _business_ message)

When you assign those events, polling starts automatically, you don't have anything to do.  
Polling will stop when you remove (`-=`) your events, or when you cancel the [global cancellation token](#global-cancellation-token-v212)

You can also use `await bot.DropPendingUpdateAsync()` before setting those events in order to ignore past updates.

The [Console example project](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Console) has been updated to demonstrate these events.