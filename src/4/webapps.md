# Telegram Mini Apps

[![Mini App bot API](https://img.shields.io/badge/Bot_API_Doc-Mini_Apps-blue.svg?style=flat-square)](https://core.telegram.org/bots/webapps)
[![MiniApp example project](https://img.shields.io/badge/Examples-MiniApp-green?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/MiniApp)

If standard Telegram Bot features aren't enough to fit your needs,
you may want to consider building a [Mini App](https://core.telegram.org/bots/webapps) instead.

This take the form of an integrated browser window showing directly web pages from your bot WebApp,
so you have more control with HTML/JS to display the interface you like.

 <video autoplay loop controls muted poster="https://telegram.org/file/464001434/100bf/eWprjdgzEbE.100386/644bbea83084f44c8f" style="width: 100%; max-width: 600px;" title="" alt="Bot Revolution">
  <source src="https://telegram.org/file/464001679/11aa9/KQx_BlPVXRo.4922145.mp4/c65433c8ac11a347a8" type="video/mp4">
 </video>

Check our [full example project](https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/MiniApp) based on Razor pages, and including a clone of the above [@DurgerKingBot](https://t.me/DurgerKingBot) and more demo to test features.

## Starting Mini-Apps

Mini Apps can be launched from various ways:
- [Keyboard Buttons](../2/reply-markup.md#custom-keyboards): `KeyboardButton.WithWebApp`
- [Inline Buttons](../2/reply-markup.md#inline-keyboards): `InlineKeyboardButton.WithWebApp`
- Chat menu button (left of user textbox): via @BotFather or `SetChatMenuButtonAsync`
- Inline-mode results with a "Switch to Mini App" button: `AnswerInlineQueryAsync` with parameter `InlineQueryResultsButton.WebApp`
- Direct link like https://t.me/botusername/appname?startapp=command

## Integration
Your web pages must include this script in the `<head>` part:
```html
<script src="https://telegram.org/js/telegram-web-app.js"></script>
```

Your Javascript can then access a [Telegram.WebApp](https://core.telegram.org/bots/webapps#initializing-mini-apps) object supporting many [properties and methods](https://core.telegram.org/bots/webapps#initializing-mini-apps), as well as [event handlers](https://core.telegram.org/bots/webapps#events-available-for-mini-apps).

In particular, you may want to use your Telegram.Bot backend to validate the authenticity of `Telegram.WebApp.initData`.

This can be done using our `AuthHelpers.ParseValidateData` method and the bot token, to make sure the requests come from Telegram and obtain information about Telegram user and context.

## For more details

To read more about Mini Apps, see <https://core.telegram.org/bots/webapps>

Visit our example project: <https://github.com/TelegramBots/Telegram.Bot.Examples/tree/master/MiniApp>