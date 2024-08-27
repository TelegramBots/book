# Telegram Bots Book
[![NuGet](https://img.shields.io/nuget/dt/Telegram.Bot.svg?style=flat-square)](https://dev.azure.com/tgbots/Telegram.Bot/_artifacts/feed/release/NuGet/Telegram.Bot)
[![Repository](https://img.shields.io/github/stars/TelegramBots/Telegram.Bot.svg?style=social&label=Stars)](https://github.com/TelegramBots/Telegram.Bot)

**[Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)** is the most popular .NET client for [Telegram Bot API](https://core.telegram.org/bots/api), allowing [developers to build bots](https://core.telegram.org/bots) for [Telegram](https://www.telegram.org) messaging app.

This book covers all you need to know to create a chatbot in .NET, with many concrete examples written in C#.  
Begin with our [_Quickstart_](1/quickstart.md), or choose from the Table Of Content (left/top), and don't miss our useful [Frequently Asked Questions](FAQ.md).

## 🧩 Installation

> [!IMPORTANT]
> _Latest versions of the library are not available on Nuget․org due to false-positive malware detection. We are working with Nuget/ESET teams to resolve this issue._

In the mean time, latest versions are available on our [special nuget source](https://dev.azure.com/tgbots/Telegram.Bot/_artifacts/feed/release/NuGet/Telegram.Bot):  
`https://pkgs.dev.azure.com/tgbots/Telegram.Bot/_packaging/release/nuget/v3/index.json`

See the screenshots below to configure the Package source in Visual Studio:
![In Visual Studio](1/docs/NugetPackageManager.png)
and make sure to follow the [Migration Guide for v21.*](migrate/Version-21.x.md) if you have existing bot code.

Alternatively you can use command line:
```
dotnet nuget add source "https://pkgs.dev.azure.com/tgbots/Telegram.Bot/_packaging/release/nuget/v3/index.json" -n Telegram.Bot
```
Or set up a `nuget.config` file at the root of your project/solution:
```xml
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="Telegram.Bot" value="https://pkgs.dev.azure.com/tgbots/Telegram.Bot/_packaging/release/nuget/v3/index.json" />
  </packageSources>
</configuration>
```

## 🪄 More examples

This book is filled with ready-to-use snippets of code, but you can also find full project examples at our [Telegram.Bot.Examples](https://github.com/TelegramBots/Telegram.Bot.Examples) Github repository, featuring:
- Simple Console apps (long polling)
- Webhook ASP.NET example (with Controllers or Minimal APIs)
- Full-featured advanced solution
- Serverless Functions implementations

## 🔗 More useful links

|Visit our|URL|
|--|--|
|Nuget feed|<https://dev.azure.com/tgbots/Telegram.Bot/_artifacts/feed/release/NuGet/Telegram.Bot>|
|Github repo|<https://github.com/TelegramBots/Telegram.Bot>|
|Examples repo|<https://github.com/TelegramBots/Telegram.Bot.Examples>|
|Telegram news channel|<https://t.me/tgbots_dotnet>|
|Telegram support group|<https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA>|
|Team page|<https://github.com/orgs/TelegramBots/people>|

