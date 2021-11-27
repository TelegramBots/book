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
// using Telegram.Bot.Types.ReplyMarkups;

ReplyKeyboardMarkup replyKeyboardMarkup = new(new []
    {
        new KeyboardButton[] { "Help me", "Call me ☎️" },
    })
    {
        ResizeKeyboard = true
    };

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Choose a response",
    replyMarkup: replyKeyboardMarkup,
    cancellationToken: cancellationToken);
```

> We specify `ResizeKeyboard = true` here to resize the keyboard vertically for optimal fit (e.g., make the keyboard smaller if there are just two rows of buttons).

### Multi-row keyboard markup

A [`ReplyKeyboardMarkup`] with two rows of buttons:

```c#
// using Telegram.Bot.Types.ReplyMarkups;

ReplyKeyboardMarkup replyKeyboardMarkup = new(new []
    {
        new KeyboardButton[] { "One", "Two" },
        new KeyboardButton[] { "Three", "Four" },
    })
    {
        ResizeKeyboard = true
    };

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Choose a response",
    replyMarkup: replyKeyboardMarkup,
    cancellationToken: cancellationToken);
```

### Request information

[`ReplyKeyboardMarkup`] containing buttons for contact and location requests using helper methods `KeyboardButton.WithRequestLocation` and `KeyboardButton.WithRequestContact`:

```c#
// using Telegram.Bot.Types.ReplyMarkups;

ReplyKeyboardMarkup replyKeyboardMarkup = new(new []
    {
        KeyboardButton.WithRequestLocation("Share Location"),
        KeyboardButton.WithRequestContact("Share Contact"),
    });

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Who or Where are you?",
    replyMarkup: replyKeyboardMarkup,
    cancellationToken: cancellationToken);
```

### Remove keyboard

To remove keyboard you have to send an instance of [`ReplyKeyboardRemove`] object:

```c#
// using Telegram.Bot.Types.ReplyMarkups;

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Removing keyboard",
    replyMarkup: new ReplyKeyboardRemove(),
    cancellationToken: cancellationToken);
```

## Inline keyboards

There are times when you'd prefer to do things without sending any messages to the chat. For example, when your user is changing settings or flipping through search results. In such cases you can use [Inline Keyboards] that are integrated directly into the messages they belong to.

Unlike custom reply keyboards, pressing buttons on inline keyboards doesn't result in messages sent to the chat. Instead, inline keyboards support buttons that work behind the scenes: [callback buttons](#callback-buttons), [URL buttons](#url-buttons) and [switch to inline buttons](#switch-to-inline-buttons).

### Callback buttons

When a user presses a [callback button], no messages are sent to the chat. Instead, your bot simply receives the relevant query. Upon receiving the query, your bot can display some result in a notification at the top of the chat screen or in an alert. In this example we use `InlineKeyboardButton.WithCallbackData` helper method to create a button with a text and callback data.

```c#
// using Telegram.Bot.Types.ReplyMarkups;

InlineKeyboardMarkup inlineKeyboard = new(new []
    {
        // first row
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "1.1", callbackData: "11"),
            InlineKeyboardButton.WithCallbackData(text: "1.2", callbackData: "12"),
        },
        // second row
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "2.1", callbackData: "21"),
            InlineKeyboardButton.WithCallbackData(text: "2.2", callbackData: "22"),
        },
    });

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "A message with an inline keyboard markup",
    replyMarkup: inlineKeyboard,
    cancellationToken: cancellationToken);
```

### URL buttons

Buttons of this type have a small arrow icon to help the user understand that tapping on a [URL button] will open an external link. In this example we use `InlineKeyboardButton.WithUrl` helper method to create a button with a text and url.

```c#
// using Telegram.Bot.Types.ReplyMarkups;

InlineKeyboardMarkup inlineKeyboard = new(new []
    {
        InlineKeyboardButton.WithUrl(
            text: "Link to the Repository",
            url: "https://github.com/TelegramBots/Telegram.Bot"
        )
    }
);

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "A message with an inline keyboard markup",
    replyMarkup: inlineKeyboard,
    cancellationToken: cancellationToken);
```

### Switch to Inline buttons

Pressing a [switch to inline button] prompts the user to select a chat, opens it and inserts the bot's username into the input field. You can also pass a query that will be inserted along with the username – this way your users will immediately get some inline results they can share. In this example we use `InlineKeyboardButton.WithSwitchInlineQuery` and `InlineKeyboardButton.WithSwitchInlineQueryCurrentChat` helper methods to create buttons which will insert the bot's username in the chat's input field.

```c#
// using Telegram.Bot.Types.ReplyMarkups;

InlineKeyboardMarkup inlineKeyboard = new(new[]
    {
        InlineKeyboardButton.WithSwitchInlineQuery("switch_inline_query"),
        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("switch_inline_query_current_chat"),
    }
);

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "A message with an inline keyboard markup",
    replyMarkup: inlineKeyboard,
    cancellationToken: cancellationToken);
```

[special keyboard]: https://core.telegram.org/bots#keyboards
[`ReplyKeyboardMarkup`]: https://core.telegram.org/bots/api/#replykeyboardmarkup
[`KeyboardButton`]: https://core.telegram.org/bots/api/#keyboardbutton
[Inline Keyboards]: https://core.telegram.org/bots#inline-keyboards-and-on-the-fly-updating
[callback button]: https://core.telegram.org/bots/2-0-intro#callback-buttons
[URL button]: https://core.telegram.org/bots/2-0-intro#url-buttons
[switch to inline button]: https://core.telegram.org/bots/2-0-intro#switch-to-inline-buttons
[`ReplyKeyboardRemove`]: https://core.telegram.org/bots/api#replykeyboardremove
