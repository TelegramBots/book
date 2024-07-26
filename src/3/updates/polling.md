# Long Polling

Long Polling is done by calling [getUpdates](https://core.telegram.org/bots/api#getupdates) actively.

With our library, this can be done in one of three ways:

## By setting `bot.OnUpdate` (and/or `bot.OnMessage`)
[![Console application](https://img.shields.io/badge/Examples-Console-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Console)

Setting those events will automatically start a background polling system which will call your events accordingly:
- `OnMessage` for updates about messages (new or edited Message, Channel Post or Business Messages)
- `OnUpdate` for all other type of updates (callback, inline-mode, chat members, polls, etc..)

> [!NOTE]
> If you don't set OnMessage, the OnUpdate event will be triggered for all updates, including messages.

## By using the `StartReceiving` method (or `ReceiveAsync`)
[![Advanced console application](https://img.shields.io/badge/Examples-Console.Advanced-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Console.Advanced)

Those methods start a polling system which will call your method on incoming updates.

As arguments, you can pass either lambdas, methods or a class derived from `IUpdateHandler` that implements the handling of Update and Error.

## By calling `GetUpdatesAsync` manually in a loop

You can specify a timeout so that the call blocks for **up to** X seconds, waiting for an incoming update

Here is an example implementation:
```csharp
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
```