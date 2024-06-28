// ANCHOR: usings
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
// ANCHOR_END: usings

namespace BookExamples.Chapter1;

internal class ExampleBot
{
    private async Task BookExamples()
    {
// ANCHOR: example-bot
using CancellationTokenSource cts = new();
var bot = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}", cancellationToken: cts.Token);

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
 var receiverOptions = new ReceiverOptions()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
};

bot.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions);

var me = await bot.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    // Echo received message text
    Message sentMessage = await bot.SendTextMessageAsync(chatId, "You said:\n" + messageText);
}

Task HandlePollingErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
// ANCHOR_END: example-bot
    }
}
