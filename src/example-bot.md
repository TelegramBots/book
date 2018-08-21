# Example

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
        await botClient.SendTextMessageAsync(
          chatId: e.Message.Chat,
          text:   "You said:\n" + e.Message.Text
        );
      }
    }
  }
}
```

When you run this program via `dotnet run`, it runs forever (until forcefully stopped) waiting for
text messages. Open the chat with your bot in Telegram and send him a text message -
you should get a reply in no time.

We subscribe to the `OnMessage` event on bot client to take action on messages that users send to the bot.

By invoking `StartReceiving()`, bot client starts fetching updates([`getUpdates`] method) for the bot
from Telegram Servers. This is an asynchronous operation, so we use `Thread.Sleep()` to keep the app running for a while.

When a user sends a message, `Bot_OnMessage()` gets invoked with the message object in the event argument.
We are expecting a text message so we check for `Message.Text` value.
Finally, we send a text message back to the same chat we got the message from.

[`getMe`]: https://core.telegram.org/bots/api#getme
[`getUpdates`]: https://core.telegram.org/bots/api#getupdates
