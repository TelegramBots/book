# Full Example

On the [previous page](example-bot.md) we got a basic bot reacting to messages via `bot.OnMessage`.

Now, we are going to set also `bot.OnUpdate` and `bot.OnError` to make a more complete bot

Modify your `Program.cs` to the following:

```c#
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("YOUR_BOT_TOKEN", cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// method to handle errors in polling or in your OnMessage/OnUpdate code
async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); // just dump the exception to the console
}

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text == "/start")
    {
        await bot.SendMessage(msg.Chat, "Welcome! Pick one direction",
            replyMarkup: new InlineKeyboardMarkup().AddButtons("Left", "Right"));
    }
}

// method that handle other types of updates received by the bot:
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
    {
        await bot.AnswerCallbackQuery(query.Id, $"You picked {query.Data}");
        await bot.SendMessage(query.Message!.Chat, $"User {query.From} clicked on {query.Data}");
    }
}
```

Run the program and send `/start` to the bot.
> [!NOTE]  
> `/start` is the first message your bot receives automatically when a user interacts in private with the bot for the first time

The bot will reply with its welcome message and 2 inline buttons for you to choose.

When you click on a button, your bot receives an Update of type **CallbackQuery** that is not a simple message.  
Therefore it will be handled by `OnUpdate` instead.

We handle this by replying the callback data _(which could be different from the button text)_,
and which user clicked on it _(which could be any user if the message was in a group)_

The `OnError` method handles errors, and you would typically log it to trace problems in your bot.

Look at [the Console example](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Console) in our [Examples repository](https://github.com/TelegramBots/Telegram.Bot.Examples) for an even more complete bot code.

<!-- -->
