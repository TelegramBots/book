# Example - First Chat Bot

On the previous page, we got an access token and used the [`getMe`] method to check our setup.
Now, it is time to make an _interactive_ bot that gets users' messages and replies to them like in this screenshot:

![Example Image](docs/shot-example_bot.jpg)

Copy the following code to `Program.cs`.

> Replace `YOUR_ACCESS_TOKEN_HERE` with the access token from Bot Father.

```c#
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Awesome {
  class Program {
    static ITelegramBotClient botClient;

    static void Main() {
      botClient = new TelegramBotClient("YOUR_ACCESS_TOKEN_HERE");

      var me = botClient.GetMeAsync().Result;
      Console.WriteLine(
        $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
      );

      botClient.OnMessage += Bot_OnMessage;
      botClient.StartReceiving();
      Thread.Sleep(999000);
    }

    static async void Bot_OnMessage(object sender, MessageEventArgs e) {
      if (e.Message.Text != null)
      {
        Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");

        await botClient.SendTextMessageAsync(
          chatId: e.Message.Chat,
          text:   "You said:\n" + e.Message.Text
        );
      }
    }
  }
}
```

Run the program.

```bash
dotnet run
```

It runs for a long time waiting for text messages unless forcefully stopped. Open a private chat with your bot in
Telegram and send a text message to it. Bot should reply in no time.

In the code above, we subscribe to `OnMessage` event on bot client to take action on messages that users send to bot.

By invoking `StartReceiving()`, bot client starts fetching updates using [`getUpdates`] method for the bot
from Telegram Servers. This is an asynchronous operation so `Thread.Sleep()` is used right after that
to keep the app running for a while in this demo.

When a user sends a message, `Bot_OnMessage()` gets invoked with the message object passed via event arguments.
We are expecting a text message so we check for `Message.Text` value.
Finally, we send a text message back to the same chat we got the message from.

If you take a look at console, program outputs the `chatId` value. **Copy chat id number** to make testing easier
for yourself in next pages.

```text
Received a text message in chat 123456789.
```

[`getMe`]: https://core.telegram.org/bots/api#getme
[`getUpdates`]: https://core.telegram.org/bots/api#getupdates
