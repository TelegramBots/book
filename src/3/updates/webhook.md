# Webhooks

[![Webhook guide](https://img.shields.io/badge/Bot_API-Webhook%20guide-blue.svg?style=flat-square)](https://core.telegram.org/bots/webhooks)

With Webhook, your web application gets notified
[one by one](#updates-are-posted-one-by-one-to-your-webapp),
automatically by Telegram when new updates arrive for your bot.

Your application will receive HTTP POST requests with an Update structure in the JSON body.

> Since version 22.5 of the library, you no longer need to configure the JSON serialization settings for your WebApp in most cases.
> But if necessary, refer to section **[Configure JSON serialization settings](#configure-json-serialization-settings)** below.

Here are example codes for handling updates, depending on the types of ASP.NET projects:

* **ASP.NET Core with Controllers (MVC)** [![ASP.NET example with Controllers](https://img.shields.io/badge/Examples-Webhook.Controllers-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Webhook.Controllers)
    ```csharp
    // Add this action in a controller class (like BotController.cs):
    [HttpPost]
    public async Task HandleUpdate([FromBody] Update update)
    {
        // put your code to handle one Update here.
    }
    ```
* **ASP.NET Core with Minimal APIs** [![ASP.NET example with Minimal APIs](https://img.shields.io/badge/Examples-Webhook.MinimalAPIs-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Webhook.MinimalAPIs)
    ```csharp
    app.MapPost("/bot", (Update update) => HandleUpdate(update));
    ...

    async Task HandleUpdate(Update update)
    {
        // put your code to handle one Update here.
    }
    ```
* **Old ASP.NET 4.x support**
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

Now that your update handler code is ready, you need to instruct Telegram to start sending updates to your URL, by running:
```csharp
var bot = new TelegramBotClient("YOUR_BOT_TOKEN");
await bot.SetWebhook("https://your.public.host:port/bot", allowedUpdates: []);
```

Great! You can now deploy your app to your webapp host machine.

_Note: If you decide to switch back to [Long Polling](polling.md), remember to call `bot.DeleteWebhook()`_


## Common issues

- You need a [supported certificate](https://core.telegram.org/bots/faq#i-39m-having-problems-with-webhooks)  
  If your host doesn't provide one, or you want to develop on your own machine, consider using [ngrok](https://ngrok.com/):  
See this useful [step-by-step guide](https://medium.com/@oktaykopcak/81c8c4a9a853)
- You must use HTTPS (TLS 1.2+), IPv4, and ports 443, 80, 88, or 8443
- The [Official webhook guide](https://core.telegram.org/bots/webhooks) gives a lot of details
- Most web hostings will recycle your app after some HTTP inactivity (= stop your app and restart it on the next HTTP request)  
  To prevent issues with this:
  - Search for an Always-On option with your host _(usually not free)_
  - Make sure your web app can be safely stopped (saved state) and restarted later (reloading state)
  - Make sure you don't have critical background code that needs to keep running at all time
  - Have a service like [cron-job.org](https://cron-job.org/) ping your webapp every 5 minutes to keep it active.
    _(host will likely still recycle your app after a few days)_
  - Host your app on a VPS machine rather than a webapp host.

## Updates are posted one by one to your webapp

> [!NOTE]  
> For high-load/busy webhook bots, Bot API may send multiple updates in parallel to your endpoint,
> in which case you may need a more advanced system to collect & reorder them by ID to ensure correct processing order.

If there are new pending updates, Telegram servers will send a POST request to your Webhook URL with the <u>next</u> update you didn't acknowledge yet.
_(based on incremental `update.Id` values)_

As long as your webapp doesn't acknowledge the update with a 200 OK **within a few seconds**, Telegram will keep resending the **same update** to your endpoint.  
In particular, this will happen if your code is throwing an unhandled exception or taking too long to process an update.

A simple way to prevent handling the same update.Id twice is:
  ```csharp
  if (update.Id <= LastUpdateId) return;
  LastUpdateId = update.Id;
  // your code to handle the Update here.
  ```

Initially Telegram will resend the failed update quickly, then with increasing intervals up to a few minutes. So if your webapp wasn't working for some time, you may have to wait a bit to receive a POST request with the next update.

If you need to process the incoming updates faster, in parallel, you will want to delegate their handling separately and acknowledge the POST request by returning from the controller immediately.  
For more details, refer to [this section of our documentation](.#sequential-vs-parallel-updates).


## Configure JSON serialization settings

Telegram updates & requests are sent using JSON payloads with `snake_case` property names.
Such serialization is normally achieved thanks to `Telegram.Bot.JsonBotAPI.Options` serialization settings.

Since version 22.5, the library should already be compatible with Telegram Webhook without needing extra configuration for your WebApp.

In some rare cases _(like [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot) / [Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-build-tools-and-aot) / [Trimming](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained))_, this might still be necessary and here is how to configure your project:
* **ASP.NET Core with Minimal APIs**  
    Locate the line `builder.Build();` _(in Program.cs)_ and insert this line <u>above</u>:
    ```csharp
    builder.Services.ConfigureTelegramBot<Microsoft.AspNetCore.Http.Json.JsonOptions>(opt => opt.SerializerOptions);
    ```
* **ASP.NET Core with Controllers** _(recommended method for .NET 6.0+)_  
    Add the [nuget package](https://www.nuget.org/packages/Telegram.Bot.AspNetCore) `Telegram.Bot.AspNetCore` to your project.  
    Locate the line `services.AddControllers();` _(in Program.cs or Startup.cs)_ and add this line below:
    ```csharp
    services.ConfigureTelegramBotMvc();
    ```
* **ASP.NET Core with Controllers** _(any .NET version)_  
    Locate the line `services.AddControllers();` _(in Program.cs or Startup.cs)_ and add this line below:
    ```csharp
    services.ConfigureTelegramBot<Microsoft.AspNetCore.Mvc.JsonOptions>(opt => opt.JsonSerializerOptions);
    ```

