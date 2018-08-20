This library is a C# implementation of [Telegram Bot API][API docs].

It is available as a [Telegram.Bot][Nuget package] nuget package.

Before you continue reading, make sure you read the [`Bots: An introduction for developers`][Docs] guide by Telegram.
The article should provide you with the basic knowledge of what Telegram bots are and what they [can (or can't)][Docs differences] do.

## Getting started

### Prerequisites

There are three things you need to get started with making your very own bot:
* [Basic documentation][docs]
* [Advanced documentation][API docs]
* [An authorization token](Authorization-Token.md)


### TelegramBotClient

At the core of the `Telegram.Bot` library is the `TelegramBotClient` class.

It encapsulates the control over your bot in an easy-to-use set of methods.

```csharp
static readonly TelegramBotClient Bot = new TelegramBotClient("API Token");
```

All interaction with the bot api will be done through this class.

Try to reuse the `TelegramBotClient` instance, as it encapsulates an `HttpClient`.


There are two main things this class will help you with:
* [Getting updates](Getting-Updates.md)
* [Sending messages](Sending-Messages.md)

### Example

Take a look at the [Example Hello Bot](Example-Hello-Bot.md), that goes over the basics of making a bot.

You might also find an example [echo bot] helpful.

## Getting help

If you encounter a problem, don't panic!

First, *please* consult the [documentation]. Most of the time the solution is hidden right there.

If you can't find an answer there, check out the [FAQ](FAQ.md). Odds are, someone else already had a similar question.

If that fails, feel free to join our [Group Chat] on Telegram, and ask any questions you might have there.
Remember, [don't ask to ask, just ask][ask to ask].

If you believe you found a bug, or you have a feature request, feel free to open an [issue] on [GitHub].
If you'd like, you can also open a [pull request], but read the [contributing guidelines] first.


[Group chat]: https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA
[NuGet package]: https://www.nuget.org/packages/Telegram.Bot/
[GitHub]: https://github.com/TelegramBots/Telegram.Bot
[Issue]: https://github.com/TelegramBots/Telegram.Bot/issues
[Pull request]: https://github.com/TelegramBots/Telegram.Bot/pulls
[Contributing guidelines]: https://github.com/TelegramBots/Telegram.Bot/blob/master/CONTRIBUTING.md
[Echo bot]: https://github.com/TelegramBots/Telegram.Bot.Examples/blob/master/Telegram.Bot.Examples.Echo/Program.cs

[@BotFather]: https://telegram.me/botfather
[API docs]: https://core.telegram.org/bots/api
[Documentation]: https://core.telegram.org/bots
[Docs]: https://core.telegram.org/bots
[Docs differences]: https://core.telegram.org/bots#4-how-are-bots-different-from-humans
[Docs BotFather]: https://core.telegram.org/bots#6-botfather

[Ask to ask]: http://sol.gfxile.net/dontask.html