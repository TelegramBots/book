# Telegram Bots Book

[![NuGet](https://img.shields.io/nuget/dt/Telegram.Bot.svg?style=flat-square)](https://nuget.voids.site/packages/Telegram.Bot)
[![Repository](https://img.shields.io/github/stars/TelegramBots/Telegram.Bot.svg?style=social&label=Stars)](https://github.com/TelegramBots/Telegram.Bot)

[**Telegram.Bot**] is the most popular .NET Client for [Telegram Bot API].

The Bot API is an HTTP-based interface created for developers keen on building bots for [Telegram].
Check [_Bots: An introduction for developers_] to understand what a Telegram bot is and what it can do.

We, the [Telegram Bots team], mainly focus on developing multiple [NuGet packages] for creating chatbots.

|Packages|Team|News Channel|Group Chat|
|:------:|:--:|:----------:|:--------:|
| [![Packages](1/docs/logo-nuget.png)](https://nuget.voids.site/packages/Telegram.Bot) | [![Team](1/docs/logo-gh.png)](https://github.com/orgs/TelegramBots/people) | [![News Channel](1/docs/logo-channel.jpg)](https://t.me/s/tgbots_dotnet) | [![Group Chat](1/docs/logo-chat.jpg)](https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA) |
| Our nuget package feed | The team contributing to this work | Subscribe to [`@tgbots_dotnet`] channel to get our latest news | [Join our chat] to talk about bots and ask questions |

## ℹ️ What Is This Book For

All Bot API methods are already documented by Telegram[^1] but this book covers all you need to know to create a
chatbot in .NET. There are also many concrete examples written in C#.
The guides here can even be useful to bot developers using other languages/platforms as it shows best practices
in developing Telegram chatbots with examples.

## 🧩 Installation
Latest versions of the library are not yet available on Nuget․org due to false-positive malware detection. We are working with Nuget/ESET teams to resolve this issue.

In the mean time, it's available on our [special nuget feed](https://nuget.voids.site/packages/Telegram.Bot): `https://nuget.voids.site/v3/index.json`

Follow the pictures below to configure the Package source in Visual Studio:
![In Visual Studio](1/docs/NugetPackageManager.jpg)
or alternatively in a `nuget.config` file:
![In nuget.config file](1/docs/nuget_config.jpg)


## 🔨 Get Started

**Begin your bot development journey with the [_Quickstart_](1/quickstart.md) guide.**

## 🪄 Examples

Check out our [examples](https://github.com/TelegramBots/Telegram.Bot.Examples) repository for more.

## ✅ Correctness & Testing

This project is fully tested using Unit tests and Systems Integration tests before each release.
In fact, our test cases are self-documenting and serve as examples for Bot API methods.
Once you learn the basics of Telegram chatbots, you will be able to easily understand the code in examples and
use it in your own bot projects.


[**Telegram.Bot**]: https://github.com/TelegramBots/Telegram.Bot
[Telegram Bot API]: https://core.telegram.org/bots/api
[Telegram]: https://www.telegram.org/
[_Bots: An introduction for developers_]: https://core.telegram.org/bots
[Telegram Bots team]: https://github.com/orgs/TelegramBots/people
[NuGet packages]: https://www.nuget.org/profiles/TelegramBots
[`@tgbots_dotnet`]: https://t.me/tgbots_dotnet
[Join our chat]: https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA
[^1]: [Telegram Bot API](https://core.telegram.org/bots/api)
