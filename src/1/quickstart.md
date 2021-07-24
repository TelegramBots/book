# Quickstart

## Bot Father

Before you start developing a bot, you need to talk to [`@BotFather`](https://t.me/botfather) on Telegram. Register a
bot with him and get an access token.

[![Bot Father](docs/logo-bot-father.jpg)](https://t.me/botfather)

Access token is a key used to identify and authorize your bot in API
requests so keep it with yourself as a secret. It looks like this:

```text
1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy
```

## Hello World

Now you have a bot, it's time to bring it to life! Create a new console project for your bot.
It could be a .NET Core project or a .NET project targeting versions 4.5+.

> This guide uses .NET Core examples but full .NET framework projects work as well.

```bash
dotnet new console
```

Add a reference to `Telegram.Bot` package.

```bash
dotnet add package Telegram.Bot
```

Open `Program.cs` file and use the following content. This code fetches Bot information based on its access token by calling [`getMe`] method on the Bot API.

> Replace `{YOUR_ACCESS_TOKEN_HERE}` with the access token from Bot Father.

```c#
using System;
using Telegram.Bot;

var botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");
var me = await botClient.GetMeAsync();
Console.WriteLine(
  $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
);

```

Running the program gives you the following output:

```bash
dotnet run
```

```text
Hello, World! I am user 1234567 and my name is Awesome Bot.
```

Great! This bot is self-aware. To make the bot interact with a user, head to the next page.

[`getMe`]: https://core.telegram.org/bots/api#getme
