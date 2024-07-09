# Quickstart

## Bot Father

Before you start, you need to talk to [`@BotFather`] on Telegram.
[Create a new bot](https://core.telegram.org/bots/tutorial#obtain-your-bot-token), acquire the bot token and get back here.

[![Bot Father](docs/logo-bot-father.jpg)](https://t.me/botfather)

Bot token is a key that required to authorize the bot and send requests to the Bot API. Keep your token secure and store it safely, it can be used to control your bot. It should look like this:

```text
1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy
```

## Hello World

Now that you have a bot, it's time to bring it to life!

> [!NOTE]  
> We recommend a recent .NET version like .NET 8, but we also support older .NET Framework (4.6.1+), .NET Core (2.0+) or .NET (5.0+)
ne

Create a new console project for your bot and add a reference to `Telegram.Bot` package:

```bash
dotnet new console
dotnet nuget add source https://nuget.voids.site/v3/index.json
dotnet add package Telegram.Bot
```

The code below fetches Bot information based on its access token by calling the Bot API [`getMe`] method. Open `Program.cs` and use the following content:

> ⚠️ Replace `{YOUR_ACCESS_TOKEN_HERE}` with your access token from the [`@BotFather`].

```c#
{{#include ../../Examples/1/Quickstart.cs:usings}}

{{#include ../../Examples/1/Quickstart.cs:quickstart}}
```

Running the program gives you the following output:

```bash
dotnet run

Hello, World! I am user 1234567 and my name is Awesome Bot.
```

Great! This bot is self-aware. To make the bot interact with a user, head to the [next page].

<!-- -->

[`@BotFather`]: https://t.me/botfather
[`getMe`]: https://core.telegram.org/bots/api#getme
[next page]: example-bot.md
