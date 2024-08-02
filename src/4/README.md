# Advanced topics

- [Proxy](proxy.md)
- [Mini Apps](webapps.md)
- [Business Features](business.md)
- [Passport](passport/)

## Telegram Login Widget

You can use `InlineKeyboardButton.WithLoginUrl` to easily initiate a login connection to your website using the user's Telegram account credentials.
```csharp
replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithLoginUrl(
    "login", new LoginUrl { Url = "https://yourdomain.com/url" }))
```

You'll need to associate your website domain with your bot by sending `/setdomain` to `@BotFather`.

See official documentation about [Telegram Login Widget](https://core.telegram.org/widgets/login) for more information.

Server-side, you can use our separate repository [`Telegram.Bot.Extensions.LoginWidget`](https://github.com/TelegramBots/Telegram.Bot.Extensions.LoginWidget)
to validate the user credentials, or to generate a Javascript to show the login widget directly on your website.