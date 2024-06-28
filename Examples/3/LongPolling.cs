using Telegram.Bot;

namespace BookExamples.Chapter3;

internal class ExampleBot
{
    private async Task LongPolling()
    {
// ANCHOR: long-polling
CancellationTokenSource cts = new();
var bot = new TelegramBotClient("{YOUR BOT TOKEN HERE}", cancellationToken: cts.Token);

int? offset = null;
while (!cts.IsCancellationRequested)
{
    var updates = await bot.GetUpdatesAsync(offset, timeout: 2);
    foreach (var update in updates)
    {
        offset = update.Id + 1;
        try
        {
            // put your code to handle one Update here.
        }
        catch (Exception ex)
        {
            // log exception and continue
        }
        if (cts.IsCancellationRequested) break;
    }
}
// ANCHOR_END: long-polling
    }
}
