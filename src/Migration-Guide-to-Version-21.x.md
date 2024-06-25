# Migration guide for version 21.x

Important notes:
- Don't bother about version 20, migrate directly to version 21.*
- You won't find this version on Nuget: [See this guide to install it in your programs](https://telegrambots.github.io/book/index.html#-obtain-latest-versions).
- Version 21.1 supports [Bot API 7.5](https://core.telegram.org/bots/api-changelog#june-18-2024) _(including [Telegram Stars payments](#payments-with-telegram-stars))_
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
(Other methods based on those requests will be removed soon)

## Payments with Telegram Stars

To make a payment in [Telegram Stars](https://t.me/BotNews/90) with SendInvoiceAsync, set the following parameters:
- `providerToken:` `null` or `""`
- `currency:` `"XTR"`
- `prices:` with a single price
- no tip amounts

## Webhooks with System.Text.Json

The library now uses `System.Text.Json` instead of `NewtonsoftJson`.

To make it work in your ASP.NET projects, you'll need to:
- Remove package **Microsoft.AspNetCore.Mvc.NewtonsoftJson** from your project dependencies
- Configure your webapp services in your Startup with
`services.ConfigureTelegramBot<Microsoft.AspNetCore.Mvc.JsonOptions>(opt => opt.JsonSerializerOptions);`
- or if you use minimal APIs, use this line instead:
`builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);`
