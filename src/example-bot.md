# Example

On previous page, we got an access token and used [`getMe`] method to check our setup.
Now, it is time to make an _interactive_ bot that gets user's messages and replies to them like in this screenshot:

![Example Image](docs/example-01.jpg)

Copy the following code to `Program.cs` file.

> Replace `YOUR_ACCESS_TOKEN_HERE` with the access token from Bot Father.

```c#
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Awesome {
  class Program {
    static ITelegramBotClient botClient;

    static void Main() {
      botClient = new TelegramBotClient("YOUR_ACCESS_TOKEN_HERE");
      botClient.OnMessage += Bot_OnMessage;
      botClient.StartReceiving();
      while (true) Console.ReadKey(true);
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

When you run this program via `dotnet run`, it runs forever(until forcefully stopped) waiting for
text messages. Open the chat with your bot in Telegram and send a text message to it. Bot should
reply in no time.

We subscribe to `OnMessage` event on bot client to take action on messages that users send to the bot.

By invoking `StartReceiving()`, bot client starts fetching updates([`getUpdates`] method) for the bot
from Telegram Servers. This is an asynchronous operation so an infinite loop is used right after that
to keep the app running in this demo.

When user sends a message, `Bot_OnMessage()` is invoked with the message object in the event argument.
We are expecting a text message so we check for `Message.Text` value. Finally, we send a text message
back to the same chat that we received the message in.

[`getMe`]: https://core.telegram.org/bots/api#getme
[`getUpdates`]: https://core.telegram.org/bots/api#getupdates
