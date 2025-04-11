# List of helpers in the library

Our library is mainly a bridge between your application and Telegram Bot API.  
However, to simplify your life, we also provide a set of additional helpers methods and services, listed below.

> For more advanced features, you can look at [high-level bot frameworks](https://github.com/TelegramBots/Telegram.Bot/wiki) constructed around our library

## Message and format helpers

- `message.MessageLink()`: Returns the <a href="t.me">t.me/...</a> link to this message, or `null` if the message was not in a Supergroup or Channel
- `message.IsServiceMessage`: Detect service messages vs content messages
- `message.ToMarkdown()` to convert the message to Markdown format
- `message.ToHtml()` to convert the message to HTML format
- `Markdown.Escape()` to escape reserved Markdown characters in a string
- `HtmlText.Escape()` to escape reserved HTML characters in a string
- `HtmlText.ToPlain()` to convert HTML string to plain text _(removing tags)_
- `HtmlText.PlainText()` to get the number of characters of plain text from HTML
- `HtmlText.Truncate()` to truncate HTML string to a number of plain-text characters _(while still preserving the formatting)_

## Easier information
- `chat.ToString()` & `user.ToString()` to easily print/log information about the chat/user
- `ChatMember` has properties `IsAdmin` and `IsInChat` to simplify testing if the user is an admin or currently inside the chat

## Updates
- `bot.DropPendingUpdates()` to clear the pending updates queue
- `Update.AllTypes` is a constant array containing all `UpdateType`s, usable with `GetUpdates`/`SetWebhook`
- `bot.OnMessage` / `bot.OnUpdate` events to easily subscribe to messages or updates
_(these automatically start a background task to poll for updates)_

## Telegram files
- `bot.GetInfoAndDownloadFile` to get file information and download it in a single call
- `TelegramBotClient.GetFileIdType(fileId)` to determine the type of object stored behind a FileId _(photo, video, etc)_

## Simplified constructors and implicit conversions
We've added easier ways to construct various instances from other types, especially when passing arguments to methods:
- `ChatPermissions(bool)` and `ChatAdministratorRights(bool)` constructors to set all Can* fields to the specified value
- `ReactionType` from an emoji (string) or a customEmojiId (long)
- `ReplyParameters` from a messageId (int), or a `Message` class, so you can pass these directly for the `replyParameters:` argument
- `LinkPreviewOptions` from a `bool` where `true` means to disable the preview
- `LabeledPrice` from tuple `(string label, long amount)`
- `BotCommand` from tuple `(string command, string description)`
- `BotCommandScope` has several static methods to construct scopes
- `InputFile` from a fileId or URL (string) or a `Stream` or a received media file

Examples:
```csharp
await bot.RestrictChatMember(chatId, userId, new ChatPermissions(true)); // unmute
await bot.SetMessageReaction(msg.Chat, msg.Id, ["👍"]);
await bot.SendMessage(msg.Chat, "Visit t.me/tgbots_dotnet", replyParameters: msg, linkPreviewOptions: true);
await bot.SendInvoice(chatId, "Product", "Description", "ProductID", "XTR", [("Price", 500)]);
await Bot.SetMyCommands([("/start", "Start the bot"), ("/privacy", "Privacy policy")], BotCommandScope.AllPrivateChats());
await bot.SendPhoto(msg.Chat, "https://picsum.photos/310/200.jpg");
await bot.SendVideo(msg.Chat, msg.Video, "Sending your video back");
```

## Reply Markup

Keyboards can be easily constructed by passing directly the following type of objects for the `replyMarkup:` parameter:

| Type | Meaning |
|------|---------|
| `string` | single keyboard text button |
| `string[]` | keyboard text buttons on one row |
| `string[][]` | multiple keyboard text buttons |
| `KeyboardButton` | single keyboard button |
| `KeyboardButton[]` | multiple keyboard buttons on one row |
| `KeyboardButton[][]` or<br/>`IEnumerable<KeyboardButton>[]` | multiple keyboard buttons |
| `InlineKeyboardButton` | single inline button |
| `InlineKeyboardButton[]` | inline buttons on 1 row |
| `InlineKeyboardButton[][]` or<br/> `IEnumerable<InlineKeyboardButton>[]` | multiple inline buttons |

Additionally, `InlineKeyboardButton` can be implicitly constructed from a tuple `(string text, string callbackOrUrl)` for Callback or Url buttons
```csharp
await bot.SendMessage(msg.Chat, "Visit our website", replyMarkup: InlineKeyboardButton.WithUrl("Click here", "https://telegrambots.github.io/book/"));
await bot.SendMessage(botOwnerId, $"Annoying user: {msg.From}", replyMarkup: new InlineKeyboardButton[]
    { ("Ban him", $"BAN {msg.From.Id}"), ("Mute him", $"MUTE {msg.From.Id}") });
await bot.SendMessage(msg.Chat, "Keyboard buttons:", replyMarkup: new string[] { "MENU", "INFO", "LANGUAGE" });
```
### Constructing dynamically
`ReplyKeyboardMarkup` & `InlineKeyboardMarkup` have methods to help you construct keyboards dynamically:
```csharp
var replyMarkup = new InlineKeyboardMarkup()
    .AddButton(InlineKeyboardButton.WithUrl("Link to Repository", "https://github.com/TelegramBots/Telegram.Bot"))
    .AddNewRow().AddButton("callback").AddButton("caption", "data")
    .AddNewRow("with", "three", "buttons")
    .AddNewRow().AddButtons("A", "B", InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("switch"));
```

And you can use `new ReplyKeyboardMarkup(true)` to resize the reply keyboard.

## Mini-App and Login widget validation
- `AuthHelpers.ParseValidateData` should be used to confirm the authenticity of data received along Telegram's Mini-Apps or the Login Widget javascript requests
