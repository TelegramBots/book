# Webhooks

[![Webhook guide](https://img.shields.io/badge/Bot_API-Webhook%20guide-blue.svg?style=flat-square)](https://core.telegram.org/bots/webhooks)
[![Webhook ASP.NET example](https://img.shields.io/badge/Examples-ASP.NET%20WebApp-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Webhook.MinimalAPIs)

With Webhook, your application gets notified automatically by Telegram when new updates arrive for your bot.

Your application will receive HTTP POST requests with an Update structure in the body, using specific JSON serialization settings as presented in `Telegram.Bot.Serialization.JsonSerializerOptionsProvider.Options`.

Below, you will find how to configure an **ASP.NET Core Web API** project to make it work with Telegram.Bot, either with Controllers or Minimal APIs

## ASP.NET Core with Controllers (MVC)

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
        update = System.Text.Json.JsonSerializer.Deserialize<Update>(body, JsonSerializerOptionsProvider.Options);
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

## Common issues

- You need a supported certificate
  If your host doesn't provide one or you want to develop on your own machine, consider using [ngrok](https://ngrok.com/):
Useful [step-by-step guide](https://medium.com/@oktaykopcak/81c8c4a9a853)
- You must use HTTPS (TLS 1.2+), IPv4, and ports 443, 80, 88, or 8443
- [Official webhook guide](https://core.telegram.org/bots/webhooks)
- If your update handler throws an exception or takes too much time to return,
Telegram will consider it a temporary failure and will RESEND the same update a bit later.  
  So you may want to prevent handling the same update.Id twice:
  ```csharp
  if (update.Id <= LastUpdateId) return;
  LastUpdateId = update.Id;
  // your code to handle the Update here.
  ```
- Most web hostings will recycle your app after some HTTP inactivity (= stop your app and restart it on the next HTTP request)  
  To prevent issues like this:
  - Search for an Always-On option with your host _(usually not free)_
  - Make sure your web app can be safely stopped (saved state) and restarted later (reloading state)
  - Make sure you don't have critical background code that needs to keep running at all time
  - Have a service like [cron-job.org](https://cron-job.org/) ping your webapp every 5 minutes to keep it active.
    _(host will likely still recycle your app after a few days)_
  - Host your app on a VPS machine rather than a webapp host.
