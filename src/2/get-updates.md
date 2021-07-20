# Getting Updates

There are two mutually exclusive ways of receiving updates for your bot — the long polling using [`getUpdates`] method on one hand and Webhooks on the other. Telegram is queueing updates until the bot receives them either way, but they will not be kept longer than 24 hours.

- With long polling, the client requests information from the server using [`getUpdates`] method, but with the expectation the server may not respond immediately. If the server has no new information for the client when the poll is received, instead of sending an empty response, the server holds the request open and waits for response information to become available. Once it does have new information, the server immediately sends a response to the client, completing the request. Upon receipt of the server response, the client often immediately issues another server request.
- [Setting a webhook] means you supplying Telegram with a location in the form of an URL, on which your bot listens for updates. Telegram need to be able to connect and post updates to that URL.
To be able to handle webhook updates you'll need a server that:
  - Supports IPv4, IPv6 is currently not supported for webhooks.
  - Accepts incoming POSTs from subnets 149.154.160.0/20 and 91.108.4.0/22 on port 443, 80, 88, or 8443.
  - Is able to handle TLS1.2(+) HTTPS-traffic.
  - Provides a supported, non-wildcard, verified or self-signed certificate.
  - Uses a CN or SAN that matches the domain you’ve supplied on setup.
  - Supplies all intermediate certificates to complete a verification chain.

  You can find more useful information on setting webhook in [Marvin's Marvellous Guide to All Things Webhook]

Each user interaction with your bot results in new
[Update] object. Its fields will be set depending on update type.

## Example projects

### Long polling

- [Simple console application]. Demonstrates the use of the [Telegram.Bot.Extensions.Polling] package.

### Webhook

- [ASP.NET Core] application
- [Azure Functions]
- [AWS Lambda]

[`getUpdates`]: https://core.telegram.org/bots/api#getupdates
[Setting a webhook]: https://core.telegram.org/bots/api#setwebhook
[Update]: https://github.com/Fedorus/Telegram.Bot/blob/master/src/Telegram.Bot/Types/Update.cs
[Marvin's Marvellous Guide to All Things Webhook]: https://core.telegram.org/bots/webhooks
[Simple console application]: https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.Polling
[Telegram.Bot.Extensions.Polling]:https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling
[ASP.NET Core]: https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Telegram.Bot.Examples.WebHook
[Azure Functions]: https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Serverless/Telegram.Bot.Examples.AzureFunctions.WebHook
[AWS Lambda]: https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/Serverless/Telegram.Bot.Examples.AwsLambda.WebHook
