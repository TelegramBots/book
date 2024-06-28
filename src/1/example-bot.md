# Full Example - Your first bot

On the [previous page] we got an access token and used the [`getMe`] method to check our setup.
Now, it is time to make an _interactive_ bot that gets users' messages and replies to them like in this screenshot:

![Example Image](docs/shot-example_bot.jpg)

Copy the following code to `Program.cs`.

> ⚠️ Replace `{YOUR_ACCESS_TOKEN_HERE}` with the access token from the [`@BotFather`].

```c#
{{#include ../../Examples/1/ExampleBot.cs:usings}}

{{#include ../../Examples/1/ExampleBot.cs:example-bot}}
```

Run the program:

```bash
dotnet run
```

It runs waiting for text messages unless forcefully stopped by pressing Enter. Open a private chat with your bot in
Telegram and send a text message to it. Bot should reply in no time.

By invoking [`StartReceiving(...)`] bot client starts fetching updates using [`getUpdates`] method for the bot
from Telegram servers. This operation does not block the caller thread, because it is done on the ThreadPool. We use `Console.ReadLine()` to keep the app running.

When user sends a message, the `HandleUpdateAsync(...)` method gets invoked with the `Update` object passed as an argument.
We check `Message.Type` and skip the rest if it is not a text message.
Finally, we send a text message back to the same chat we got the message from.

The `HandlePollingErrorAsync(...)` method is invoked in case of an error that occurred while fetching updates.

If you take a look at the console, the program outputs the `chatId` value. **Copy the chat id number** to make testing easier
for yourself on the next pages.

```text
Received a 'text' message in chat 123456789.
```

<!-- -->

[previous page]: quickstart.md
[`getMe`]: https://core.telegram.org/bots/api#getme
[`getUpdates`]: https://core.telegram.org/bots/api#getupdates
[`StartReceiving(...)`]: https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling/blob/master/src/Telegram.Bot.Extensions.Polling/Extensions/TelegramBotClientPollingExtensions.cs
