# Example - First Chat Bot

On the previous page, we got an access token and used the [`getMe`] method to check our setup.
Now, it is time to make an _interactive_ bot that gets users' messages and replies to them like in this screenshot:

![Example Image](docs/shot-example_bot.jpg)

Add a reference to `Telegram.Bot.Extensions.Polling` package.

```bash
dotnet add package Telegram.Bot.Extensions.Polling
```

Copy the following code to `Program.cs`.

> Replace `{YOUR_ACCESS_TOKEN_HERE}` with the access token from Bot Father.

```c#
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");

var me = await botClient.GetMeAsync();
Console.WriteLine(
    $"Hello, World! I am user {me.Id} and my name is {me.FirstName}."
);

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
botClient.StartReceiving(
    new DefaultUpdateHandler(HandleUpdateAsync, HandleErrorAsync),
    cts.Token);

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _                                       => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type != UpdateType.Message)
        return;
    if (update.Message.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;
    
    Console.WriteLine($"Received a '{update.Message.Text}' message in chat {chatId}.");

    await botClient.SendTextMessageAsync(
        chatId: chatId,
        text:   "You said:\n" + update.Message.Text
    );
}
```

Run the program.

```bash
dotnet run
```

It runs waiting for text messages unless forcefully stopped by pressing Enter. Open a private chat with your bot in
Telegram and send a text message to it. Bot should reply in no time.

In the code above, we instantiate a [`DefaultUpdateHandler`] and pass to it our update handlers so we can take action on updates sent to the bot.

By invoking [`StartReceiving(...)`] bot client starts fetching updates using [`getUpdates`] method for the bot
from Telegram Servers. This operation does not block the caller thread, because it is done on the ThreadPool. We use `Console.ReadLine()` to keep the app running.

When a user sends a message the `HandleUpdateAsync(...)` method gets invoked with the `Update` object passed as an argument.
We check `Message.Type` and skip the rest if it is not a text message.
Finally, we send a text message back to the same chat we got the message from.

The `HandleErrorAsync(...)` method is invoked in case of an error.

If you take a look at the console, the program outputs the `chatId` value. **Copy the chat id number** to make testing easier
for yourself in the next pages.

```text
Received a 'text' message in chat 123456789.
```

[`getMe`]: https://core.telegram.org/bots/api#getme
[`getUpdates`]: https://core.telegram.org/bots/api#getupdates
[`DefaultUpdateHandler`]: https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling/blob/master/src/Telegram.Bot.Extensions.Polling/DefaultUpdateHandler.cs
[`StartReceiving(...)`]: https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling/blob/master/src/Telegram.Bot.Extensions.Polling/Extensions/TelegramBotClientPollingExtensions.cs
