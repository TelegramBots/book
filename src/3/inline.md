# Inline Mode

[![inline mode bot API](https://img.shields.io/badge/Bot_API_Object-Inline%20Mode-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#inline-mode)
[![inline queries example](https://img.shields.io/badge/Examples-Inline%20Queries-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/blob/master/Telegram.Bot.Examples.InlineQueries/Program.cs)

Telegram bots can be queried directly in the chat or via inline queries.

To use inline queries in your bot, you need to set up inline mode by command:

![/setinline command in BotFather](docs/shot-setinline_command.png)

Import `Telegram.Bot.Types.InlineQueryResults` namespace for inline query types.

There are two types that allow you to work with inline queries - `InlineQuery` and `ChosenInlineResult`:

```c#
{{#include ../../Examples/3/Inline.cs:switch-statement}}
```

## `InlineQuery`

[![inline query result bot API](https://img.shields.io/badge/Bot_API_Object-InlineQueryResult-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#inlinequeryresult)

Suppose we have two arrays:

```c#
{{#include ../../Examples/3/Inline.cs:arrays}}
```

So we can handle inline queries this way:

```c#
{{#include ../../Examples/3/Inline.cs:on-inline-query-received}}
```

`InlineQueryResult` is an abstract type used to create a response for inline queries. You can use these result types for inline queries: `InlineQueryResultArticle` for articles, `InlineQueryResultPhoto` for photos, etc.

## `ChosenInlineResult`

[![chosen inline result bot API](https://img.shields.io/badge/Bot_API_Object-ChosenInlineResult-blue.svg?style=flat-square)](https://core.telegram.org/bots/api#choseninlineresult)

This type helps to handle chosen inline result. For example, you may want to know which result users chose:

```csharp
{{#include ../../Examples/3/Inline.cs:on-chosen-inline-result-received}}
```

To use the feature you need to enable "inline feedback" in BotFather by `/setinlinefeedback` command:

![set inline feedback command](docs/shot-setinlinefeedback_command.png)

Final result:

![result](docs/shot-inline_bot_showcase.png)