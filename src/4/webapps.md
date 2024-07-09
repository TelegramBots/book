# Telegram Mini Apps

[![Mini App bot API](https://img.shields.io/badge/Bot_API_Doc-Mini%20Apps-blue.svg?style=flat-square)](https://core.telegram.org/bots/webapps)

If standard Telegram Bot features aren't enough to fit your needs,
you may want to consider building a [Mini App](https://core.telegram.org/bots/webapps) instead.

This take the form of an integrated browser window showing directly web pages from your bot WebApp,
so you have more control with HTML/JS to display the interface you like.

Mini Apps can be launched from various ways:
- [Keyboard Buttons](../2/reply-markup.md#custom-keyboards): `KeyboardButton.WithWebApp`
- [Inline Buttons](../2/reply-markup.md#inline-keyboards): `InlineKeyboardButton.WithWebApp`
- Chat menu button (left of user textbox): via @BotFather or `SetChatMenuButtonAsync`
- Inline-mode results with a "Switch to Mini App" button: `AnswerInlineQueryAsync` with parameter `InlineQueryResultsButton.WebApp`
- Direct link like https://t.me/botusername/appname?startapp=command

To read more about Mini Apps, see <https://core.telegram.org/bots/webapps>
