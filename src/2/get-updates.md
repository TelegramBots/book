## Getting Updates

The developer has two ways to get updates / messages / user actions:
- Longpolling - the bot program constantly sends requests for updates
  and receives them as a response to its request.
- Webhook is a bot application in the form of a website. Through the
  library, the programmer sends a link to the telegram server (to his
  site) where updates should come. In this case, it is necessary to
  implement the processing of what will come to this link. *Main issue with webhook - telegram wants SSL
encryption. So your site needs valid SSL certificate or you need to send
self signed certificate.*

Each user interaction with your bot results in new
[Update](https://github.com/Fedorus/Telegram.Bot/blob/master/src/Telegram.Bot/Types/Update.cs)
object. Its fields will be set depending on update type.

## Examples
**Longpolling examples:**

- [Events, soon will be deprecated](https://github.com/TelegramBots/Telegram.Bot.Examples/blob/master/Telegram.Bot.Examples.Echo/Program.cs)
- [Modern way](https://github.com/TelegramBots/Telegram.Bot.Examples/blob/master/Telegram.Bot.Examples.Polling/Program.cs)
- [Polling library with more approaches](https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling)


**Webhook examples:** 
- [Ngrok version](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.WebHook)
- [AzureFunction](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.AzureFunctions.WebHook)
- [AwsLambda](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.AwsLambda.WebHook)

