# Introduction

**Telegram.Bot** is the most popular .NET Client for [Telegram Bot API]. The Bot API is an HTTP-based interface created for developers keen on building bots for Telegram.

Check [_Bots: An introduction for developers_] to understand what a Telegram bot is and what it can do.

We, [Telegram Bots team], mainly focus on developing multiple [NuGet packages] for creating chat bots.

|Packages|Team|News Channel|Group Chat|
|:------:|:--:|:----------:|:--------:|
| [![Packages](docs/intro-nuget-logo.png)](https://www.nuget.org/profiles/TelegramBots) | [![Team](docs/intro-gh-logo.png)](https://github.com/orgs/TelegramBots/people) | [![News Channel](docs/intro-channel-logo.jpg)](https://t.me/tgbots_dotnet) | [![Group Chat](docs/intro-chat-logo.jpg)](https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA) |
| Packages we release on NuGet | The team contributing to this work | Subscribe to [`@tgbots_dotnet`] channel to get our latest news | [Join our chat] to talk about bots and ask questions |

All Bot API methods are already documented by Telegram[^1] but our guide covers all you need to create a chat bot in .NET and there are many concrete examples written in C#.

This project is fully tested using Unit and Systems Integration tests before each release. In fact, our test cases are
self-documenting and serve as examples for Bot API methods. Once you learn the basics of Telegram chat bots, you will
be able to easily copy test methods to your own program and make your bot smarter.

Begin your bot development journey from [**Quickstart**](quickstart.md) guide.

---

[Telegram Bot API]: https://core.telegram.org/bots/api
[_Bots: An introduction for developers_]: https://core.telegram.org/bots
[Telegram Bots team]: https://github.com/orgs/TelegramBots/people
[NuGet packages]: https://www.nuget.org/profiles/TelegramBots
[`@tgbots_dotnet`]: https://t.me/tgbots_dotnet
[Join our chat]: https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA
[^1]: [Telegram Bot API](https://core.telegram.org/bots/api)
