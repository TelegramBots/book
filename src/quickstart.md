# Quickstart

## Bot Father

Before you start developing a bot, you need to talk to [Bot Father `@botfather`](https://t.me/botfather) on Telegram. Register a bot with him and get an access token. Access token is a secret key used to identify and authorize your bot in API requests. It looks like this:

```text
123456789:AAE5cvFhxQ9C7vFGElcpMTNrYrkl3OBFo45
```

![Bot Father](./docs/bot-father.jpg)

## Hello World

Now you have a bot, it's time to bring it to life! Create a new console project for your bot. It could be a .NET Core project or a .NET project targeting versions 4.5+.

> This guide uses .NET Core examples but full .NET framework projects work as well.

```bash
dotnet new console
```

Add a reference to `Telegram.Bot` package.

```bash
dotnet add package Telegram.Bot
```

Open `Program.cs` file and use the following content. This code fetches Bot information based on its access token by calling [`getMe`] method on the Bot API.

> Replace `YOUR_ACCESS_TOKEN_HERE` with the access token from Bot Father.

```c#
using System;
using Telegram.Bot;

namespace Awesome {
  class Program {
    static void Main() {
      var botClient = new TelegramBotClient("YOUR_ACCESS_TOKEN_HERE");
      var me = botClient.GetMeAsync().Result;
      Console.WriteLine(
        $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
      );
    }
  }
}
```

Running the program gives you the following output:

```bash
dotnet run
```

```text
Hello, World! I am user 123456789 and my name is Awesome Bot.
```

Great! This bot is self-aware. To make the bot interact with user, head to the next page.

[`getMe`]: https://core.telegram.org/bots/api#getme