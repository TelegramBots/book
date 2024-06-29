// ANCHOR: usings
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
// ANCHOR_END: usings

namespace BookExamples.Chapter1;

internal class ExampleBot
{
    private async Task BookExamples()
    {
// ANCHOR: example-bot
using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}", cancellationToken: cts.Token);
bot.StartReceiving(HandleUpdate, async (bot, ex, ct) => Console.WriteLine(ex));

var me = await bot.GetMeAsync();
Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// method that handle updates coming for the bot:
async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
{
    if (update.Message is null) return;			// we want only updates about new Message
    if (update.Message.Text is null) return;	// we want only updates about new Text Message
    var msg = update.Message;
    Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");
    // let's echo back received text in the chat
    await bot.SendTextMessageAsync(msg.Chat, $"{msg.From} said: {msg.Text}");
}
// ANCHOR_END: example-bot
    }
}
