# Reply Markup

[![reply markup tests](https://img.shields.io/badge/Examples-Reply_Markup-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/ReplyMarkup/ReplyMarkupTests.cs)

Telegram provides two types of reply markup: [Custom reply keyboards](#custom-reply-keyboards) and [Inline keyboards](#inline-keyboards).

## Custom reply keyboards

> These are buttons visible below the textbox. Pressing such button will make the user send a message in the chat

Whenever your bot sends a message, it can pass along a [special keyboard](https://core.telegram.org/bots/features#keyboards) with predefined reply options.
Regular keyboards are represented by [`KeyboardButton`] and [`ReplyKeyboardMarkup`] objects. 
When the user click such buttons, it will make him send a message to the chat.
It can be a simple predefined text, a contact or location information, or even a poll.

Keyboard is an array of button rows, each represented by an array of [`KeyboardButton`] objects. [`KeyboardButton`] supports text and emoji.

By default, reply keyboards are displayed until a new keyboard is sent by a bot.

### Single-row keyboard markup

A reply keyboard with two buttons in a single row:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:single-row}}
```

Simple text buttons can be passed directly as strings
> If using `ReplyKeyboardMarkup`, we would have to specify `true` on the constructor to resize the keyboard vertically for optimal fit (e.g., make the keyboard smaller if there are just two rows of buttons).

### Multi-row keyboard markup

For a reply keyboard with two rows of buttons, we use an array of arrays:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:multiple-row}}
```

### Requesting information to be sent to the bot

Some special keyboard button types can be used to request information from the user and send them to the bot.

Below are some simple examples of what you can do. More options are available in associated class properties.
- `KeyboardButton.WithRequestLocation("Share your location")`  
	User's position will be transmitted in a `message.Location`
- `KeyboardButton.WithRequestContact("Share your info")`  
	User's phone number will be transmitted in a `message.Contact`
- `KeyboardButton.WithRequestPoll("Create a poll", PollType.Regular)`  
	User must create a poll which gets transmitted in a `message.Poll`
- `KeyboardButton.WithRequestChat("Select a chat", 1234, false)`  
	User must pick a group (false) or channel (true) which gets transmitted in a `message.ChatShared`
- `KeyboardButton.WithRequestUsers("Select user(s)", 5678, 1)`  
	User must pick 1-10 user(s) which get transmitted in a `message.UsersShared`  
- `KeyboardButton.WithWebApp("Launch WebApp", "https://www.example.com/game")`   
	Launch a [Mini-App](../4/webapps.md)

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:request-info}}
```

### Remove keyboard

To remove the keyboard you have to send an instance of [`ReplyKeyboardRemove`] in a new message:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:remove-keyboard}}
```

## Inline keyboards

> These are buttons visible below a bot message. Pressing such button will NOT make the user send a message

There are times when you'd prefer to do things without sending any messages to the chat. For example, when your user is changing settings or flipping through search results. In such cases you can use [Inline Keyboards] that are integrated directly into the messages they belong to.

Unlike custom reply keyboards, pressing buttons on inline keyboards doesn't result in messages sent to the chat. Instead, inline keyboards support buttons that work behind the scenes: [callback buttons](#callback-buttons), [URL buttons](#url-buttons) and [switch to inline buttons](#switch-to-inline-buttons).

You can have several rows and columns of inline buttons of mixed types.

### Callback buttons

When a user presses a [callback button], no messages are sent to the chat, and your bot simply receives an `update.CallbackQuery` instead _(containing many information)_.  
Upon receiving this, your bot should answer to that query within 10 seconds, using `AnswerCallbackQuery` _(or else the button gets momentarily disabled)_

In this example, the arrays of `InlineKeyboardButton` are constructed from tuples `(title, callbackData)`:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:callback-buttons}}
```

> Callback data string can be up to 64 bytes. You can construct them explicitly via `InlineKeyboardButton.WithCallbackData`

### URL buttons

Buttons of this type have a small arrow icon to help the user understand that tapping on a [URL button] will open an external link.
In this example we pass a single `InlineKeyboardButton`, and the constructor understand the second argument is an URL rather than callbackData.

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:url-buttons}}
```

> You can also construct URL buttons via `InlineKeyboardButton.WithUrl`

### Switch to Inline buttons

Pressing a [switch to inline button] prompts the user to select a chat, opens it and inserts the bot's username into the input field. You can also pass a query that will be inserted along with the username – this way your users will immediately get some inline results they can share. In this example we use `InlineKeyboardButton.WithSwitchInlineQuery` and `InlineKeyboardButton.WithSwitchInlineQueryCurrentChat` helper methods to create buttons which will insert the bot's username in the chat's input field.

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:switch-to-inline}}
```

### Other inline button types

Some more special inline button types can be used.

Below are some simple examples of what you can do. More options are available in associated class properties.
- `InlineKeyboardButton.WithCopyText("Copy info", "Text to copy"))`   
	Store a text in the user clipboard
- `InlineKeyboardButton.WithWebApp("Launch WebApp", "https://www.example.com/game"))`   
	Launch a [Mini-App](../4/webapps.md)
- `InlineKeyboardButton.WithLoginUrl("Login", new() { Url = "https://www.example.com/telegramAuth" }))`   
	Authenticate the Telegram user via a website _(Domain must be configured in [@BotFather])_
- `InlineKeyboardButton.WithCallbackGame("Launch game"))`   
	Launch an HTML game _(Game must be configured in [@BotFather])_
- `InlineKeyboardButton.WithPay("Pay 200 XTR"))`   
	Customize the Pay button caption _(only during a [SendInvoice call](../4/payments.md))_


[`ReplyKeyboardMarkup`]: https://core.telegram.org/bots/api/#replykeyboardmarkup
[`KeyboardButton`]: https://core.telegram.org/bots/api/#keyboardbutton
[Inline Keyboards]: https://core.telegram.org/bots/features#inline-keyboards
[callback button]: https://core.telegram.org/bots/2-0-intro#callback-buttons
[URL button]: https://core.telegram.org/bots/2-0-intro#url-buttons
[switch to inline button]: https://core.telegram.org/bots/2-0-intro#switch-to-inline-buttons
[`ReplyKeyboardRemove`]: https://core.telegram.org/bots/api#replykeyboardremove
[@BotFather]: https://t.me/botfather
