# Webhooks

[![Webhook guide](https://img.shields.io/badge/Bot_API-Webhook%20guide-blue.svg?style=flat-square)](https://core.telegram.org/bots/webhooks)

With Webhook, your web application gets notified automatically by Telegram when new updates arrive for your bot.

Your application will receive HTTP POST requests with an Update structure in the body, using specific JSON serialization settings `Telegram.Bot.JsonBotAPI.Options`.

Below, you will find how to configure an **ASP.NET Core Web API** project to make it work with Telegram.Bot, either with Controllers or Minimal APIs

## ASP.NET Core with Controllers (MVC)
[![ASP.NET example with Controllers](https://img.shields.io/badge/Examples-Webhook.Controllers-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Webhook.Controllers)

First you need to configure your Web App startup code:
- Locate the line `services.AddControllers();` _(in Program.cs or Startup.cs)_
- If you're using .NET 6.0 or more recent, add the line:
    ```csharp
    services.ConfigureTelegramBotMvc();
    ```
- For older .NET versions, add the line:
    ```csharp
    services.ConfigureTelegramBot<Microsoft.AspNetCore.Mvc.JsonOptions>(opt => opt.JsonSerializerOptions);
    ```

Next, in a controller class (like BotController.cs), you need to add an action for the updates. Typically:
```csharp
[HttpPost]
public async Task HandleUpdate([FromBody] Update update)
{
    // put your code to handle one Update here.
}
```

Good, now skip to [SetWebHookAsync](#setwebhookasync) below

## ASP.NET Core with Minimal APIs
[![ASP.NET example with Minimal APIs](https://img.shields.io/badge/Examples-Webhook.MinimalAPIs-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Webhook.MinimalAPIs)

First you need to configure your Web App startup code:
- Locate the line `builder.Build();` _(in Program.cs)_
- Above it, insert the line:
    ```csharp
    builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);
    ```

Next, you need to map an action for the updates. Typically:
```csharp
app.MapPost("/bot", (Update update) => HandleUpdate(update));
...

async Task HandleUpdate(Update update)
{
    // put your code to handle one Update here.
}
```

Good, now skip to [SetWebHookAsync](#setwebhookasync) below

## Old ASP.NET 4.x support

For older .NET Framework usage, you may use the following code:
```csharp
public async Task<IHttpActionResult> Post()
{
    Update update;
    using (var body = await Request.Content.ReadAsStreamAsync())
        update = System.Text.Json.JsonSerializer.Deserialize<Update>(body, JsonBotAPI.Options);
    await HandleUpdate(update);
    return Ok();
}
```

## SetWebHookAsync
Your update handler code is ready, now you need to instruct Telegram to send updates to your URL, by running:
```csharp
var bot = new TelegramBotClient("YOUR_BOT_TOKEN");
await bot.SetWebhookAsync("https://your.public.host:port/bot", allowedUpdates: []);
```

You can now deploy your app to your webapp host machine.

_Note: If you decide to switch back to [Long Polling](polling.md), remember to call `bot.DeleteWebhookAsync()`_

## Common issues

- You need a [supported certificate](https://core.telegram.org/bots/faq#i-39m-having-problems-with-webhooks)  
  If your host doesn't provide one, or you want to develop on your own machine, consider using [ngrok](https://ngrok.com/):  
See this useful [step-by-step guide](https://medium.com/@oktaykopcak/81c8c4a9a853)
- You must use HTTPS (TLS 1.2+), IPv4, and ports 443, 80, 88, or 8443
- The [Official webhook guide](https://core.telegram.org/bots/webhooks) gives a lot of details
- If your update handler throws an exception or takes too much time to return,
Telegram will consider it a temporary failure and will RESEND the same update a bit later.  
  You may want to prevent handling the same update.Id twice:
  ```csharp
  if (update.Id <= LastUpdateId) return;
  LastUpdateId = update.Id;
  // your code to handle the Update here.
  ```
- Most web hostings will recycle your app after some HTTP inactivity (= stop your app and restart it on the next HTTP request)  
  To prevent issues with this:
  - Search for an Always-On option with your host _(usually not free)_
  - Make sure your web app can be safely stopped (saved state) and restarted later (reloading state)
  - Make sure you don't have critical background code that needs to keep running at all time
  - Have a service like [cron-job.org](https://cron-job.org/) ping your webapp every 5 minutes to keep it active.
    _(host will likely still recycle your app after a few days)_
  - Host your app on a VPS machine rather than a webapp host.
- Updates are arriving sequentially, is this behaviour correct?
  - Check question [Question 28 in the FAQ](../../FAQ.md#28-why-are-my-updates-being-processed-sequentially-when-im-using-webhooks).