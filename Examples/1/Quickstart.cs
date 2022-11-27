// ANCHOR: usings
using Telegram.Bot;
// ANCHOR_END: usings

namespace BookExamples.Chapter1;

internal class Quickstart
{
    private async Task BookExamples()
    {
// ANCHOR: quickstart
var botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");

var me = await botClient.GetMeAsync();
Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
// ANCHOR_END: quickstart
    }
}
