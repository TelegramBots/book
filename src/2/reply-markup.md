# Reply Markup

[![reply markup tests](https://img.shields.io/badge/Examples-Reply_Markup-green.svg?style=flat-square)](https://github.com/TelegramBots/Telegram.Bot/blob/master/test/Telegram.Bot.Tests.Integ/ReplyMarkup/ReplyMarkupTests.cs)

Telegram provides two types of reply markup: [Custom keyboards](#custom-keyboards) and [Inline keyboards](#inline-keyboards).

## Custom keyboards

Whenever your bot sends a message, it can pass along a [special keyboard] with predefined reply options. Regular keyboards are represented by [`ReplyKeyboardMarkup`] object. You can request a contact or location information from the user with [`KeyboardButton`] or send a poll. Regular button will send predefined text to the chat.

Keyboard is an array of button rows, each represented by an array of [`KeyboardButton`] objects. [`KeyboardButton`] supports text and emoji.

By default, custom keyboards are displayed until a new keyboard is sent by a bot.

### Single-row keyboard markup

A [`ReplyKeyboardMarkup`] with two buttons in a single row:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:single-row}}
```

> We specify `ResizeKeyboard = true` here to resize the keyboard vertically for optimal fit (e.g., make the keyboard smaller if there are just two rows of buttons).

### Multi-row keyboard markup

A [`ReplyKeyboardMarkup`] with two rows of buttons:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:multiple-row}}
```

You can use `new List<List<KeyboardButton>>` instead of `KeyboardButton[][]` if you prefer to build the list dynamically.

### Request information

[`ReplyKeyboardMarkup`] containing buttons for contact and location requests using helper methods `KeyboardButton.WithRequestLocation` and `KeyboardButton.WithRequestContact`:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:request-info}}
```

### Remove keyboard

To remove keyboard you have to send an instance of [`ReplyKeyboardRemove`] object:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:remove-keyboard}}
```

## Inline keyboards

There are times when you'd prefer to do things without sending any messages to the chat. For example, when your user is changing settings or flipping through search results. In such cases you can use [Inline Keyboards] that are integrated directly into the messages they belong to.

Unlike custom reply keyboards, pressing buttons on inline keyboards doesn't result in messages sent to the chat. Instead, inline keyboards support buttons that work behind the scenes: [callback buttons](#callback-buttons), [URL buttons](#url-buttons) and [switch to inline buttons](#switch-to-inline-buttons).

### Callback buttons

When a user presses a [callback button], no messages are sent to the chat, and your bot simply receives an `update.CallbackQuery` instead.
Upon receiving this, your bot should answer to that query within 10 seconds, using `AnswerCallbackQueryAsync` _(or else the button gets momentarily disabled)_

In this example we use `InlineKeyboardButton.WithCallbackData` helper method to create a button with a text and callback data:

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:callback-buttons}}
```

You can use `new List<List<InlineKeyboardButton>>` instead of `InlineKeyboardButton[][]` if you prefer to build the list dynamically.

### URL buttons

Buttons of this type have a small arrow icon to help the user understand that tapping on a [URL button] will open an external link. In this example we use `InlineKeyboardButton.WithUrl` helper method to create a button with a text and url.

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:url-buttons}}
```

### Switch to Inline buttons

Pressing a [switch to inline button] prompts the user to select a chat, opens it and inserts the bot's username into the input field. You can also pass a query that will be inserted along with the username – this way your users will immediately get some inline results they can share. In this example we use `InlineKeyboardButton.WithSwitchInlineQuery` and `InlineKeyboardButton.WithSwitchInlineQueryCurrentChat` helper methods to create buttons which will insert the bot's username in the chat's input field.

```c#
{{#include ../../Examples/2/ReplyMarkup.cs:usings}}

{{#include ../../Examples/2/ReplyMarkup.cs:switch-to-inline}}
```

[special keyboard]: https://core.telegram.org/bots#keyboards
[`ReplyKeyboardMarkup`]: https://core.telegram.org/bots/api/#replykeyboardmarkup
[`KeyboardButton`]: https://core.telegram.org/bots/api/#keyboardbutton
[Inline Keyboards]: https://core.telegram.org/bots#inline-keyboards-and-on-the-fly-updating
[callback button]: https://core.telegram.org/bots/2-0-intro#callback-buttons
[URL button]: https://core.telegram.org/bots/2-0-intro#url-buttons
[switch to inline button]: https://core.telegram.org/bots/2-0-intro#switch-to-inline-buttons
[`ReplyKeyboardRemove`]: https://core.telegram.org/bots/api#replykeyboardremove
