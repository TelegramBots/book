# Quickstart

## Bot Father

Before you start, you need to talk to [`@BotFather`] on Telegram. [Create a new
bot], acquire the bot token and get back here.

[![Bot Father](docs/logo-bot-father.jpg)](https://t.me/botfather)

Bot token is a key that required to authorize the bot and send requests to the Bot API. Keep your token secure and store it safely, it can be used to control your bot. It should look like this:

```text
1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy
```

## Hello World

Now that you have a bot, it's time to bring it to life! Create a new console project for your bot.
It could be a legacy project targeting .NET Framework 4.6.1-4.8 or a modern .NET Core 3.1-.NET 5+.

> Examples in this guide target .NET 6, but earlier targets should work as well (including .NET Framework).

```bash
dotnet new console
```

Add a reference to [`Telegram.Bot`] package:

```bash
dotnet add package Telegram.Bot
```

This code fetches Bot information based on its access token by calling the Bot API [`getMe`] method. Open `Program.cs` and use the following content:

> ⚠️ Replace `{YOUR_ACCESS_TOKEN_HERE}` with your access token from the [`@BotFather`].

```c#
using Telegram.Bot;

var botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");

var me = await botClient.GetMeAsync();
Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
```

Running the program gives you the following output:

```bash
dotnet run
```

```text
Hello, World! I am user 1234567 and my name is Awesome Bot.
```

Great! This bot is self-aware. To make the bot interact with a user, head to the [next page].

<!-- -->

[`@BotFather`]: https://t.me/botfather
[Create a new
bot]: https://core.telegram.org/bots#6-botfather
[`Telegram.Bot`]: https://www.nuget.org/packages/Telegram.Bot
[`getMe`]: https://core.telegram.org/bots/api#getme
[next page]: example-bot.md
