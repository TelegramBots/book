using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ANCHOR: usings
using Telegram.Bot.Types.InlineQueryResults;
// ANCHOR_END: usings

namespace BookExamples.Chapter3;

internal class Inline
{
    public readonly ITelegramBotClient bot = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");
// ANCHOR: arrays
private readonly string[] sites = { "Google", "Github", "Telegram", "Wikipedia" };
private readonly string[] siteDescriptions =
{
    "Google is a search engine",
    "Github is a git repository hosting",
    "Telegram is a messenger",
    "Wikipedia is an open wiki"
};
// ANCHOR_END: arrays

    public async Task Run(string[] args)
    {
        var me = await bot.GetMeAsync();
        using var cts = new CancellationTokenSource();
        bot.StartReceiving(HandleUpdateAsync, PollingErrorHandler, null, cts.Token);

        Console.WriteLine($"Start listening for @{me.Username}");
        Console.ReadLine();

        cts.Cancel();
    }

    Task PollingErrorHandler(ITelegramBotClient bot, Exception ex, CancellationToken ct)
    {
        Console.WriteLine($"Exception while polling for updates: {ex.Message}");
        return Task.CompletedTask;
    }

    async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        try
        {
// ANCHOR: switch-statement
switch (update.Type)
{
    case UpdateType.InlineQuery:
        await OnInlineQueryReceived(bot, update.InlineQuery!);
        break;
    case UpdateType.ChosenInlineResult:
        await OnChosenInlineResultReceived(bot, update.ChosenInlineResult!);
        break;
};
// ANCHOR_END: switch-statement
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception while handling {update.Type}: {ex}");
        }
    }

// ANCHOR: on-inline-query-received
async Task OnInlineQueryReceived(ITelegramBotClient bot, InlineQuery inlineQuery)
{
    var results = new List<InlineQueryResult>();

    var counter = 0;
    foreach (var site in sites)
    {
        results.Add(new InlineQueryResultArticle(
            $"{counter}", // we use the counter as an id for inline query results
            site, // inline query result title
            new InputTextMessageContent(siteDescriptions[counter])) // content that is submitted when the inline query result title is clicked
        );
        counter++;
    }

    await bot.AnswerInlineQueryAsync(inlineQuery.Id, results); // answer by sending the inline query result list
}
// ANCHOR_END: on-inline-query-received

// ANCHOR: on-chosen-inline-result-received
Task OnChosenInlineResultReceived(ITelegramBotClient bot, ChosenInlineResult chosenInlineResult)
{
    if (uint.TryParse(chosenInlineResult.ResultId, out var resultId) // check if a result id is parsable and introduce variable
        && resultId < sites.Length)
    {
        Console.WriteLine($"User {chosenInlineResult.From} has selected site: {sites[resultId]}");
    }

    return Task.CompletedTask;
}
// ANCHOR_END: on-chosen-inline-result-received
}
