using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ANCHOR: usings
// using Telegram.Bot.Types.ReplyMarkups;
// ANCHOR_END: usings

namespace BookExamples.Chapter2;

internal class ReplyMarkup
{
    public readonly ITelegramBotClient botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");
    public readonly ChatId chatId = 12345;

    private async Task SingleRowMarkup()
    {
// ANCHOR: single-row
var buttons = new KeyboardButton[]
{
    "Help me", "Call me ☎️",
};

var sent = await botClient.SendTextMessageAsync(chatId, "Choose a response",
    replyMarkup: new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true });
// ANCHOR_END: single-row
    }

    private async Task MultipleRowMarkup()
    {
// ANCHOR: multiple-row
var buttons = new KeyboardButton[][]
{
    new KeyboardButton[] { "Help me" },
    new KeyboardButton[] { "Call me ☎️", "Write me ✉️" },
};

var sent = await botClient.SendTextMessageAsync(chatId, "Choose a response",
    replyMarkup: new ReplyKeyboardMarkup(buttons) { ResizeKeyboard = true });
// ANCHOR_END: multiple-row
    }

    private async Task RequestInfo()
    {
// ANCHOR: request-info
var buttons = new[]
{
    KeyboardButton.WithRequestLocation("Share Location"),
    KeyboardButton.WithRequestContact("Share Contact"),
};

var sent = await botClient.SendTextMessageAsync(chatId, "Who or Where are you?",
    replyMarkup: new ReplyKeyboardMarkup(buttons));
// ANCHOR_END: request-info
    }

    private async Task RemoveKeyboard()
    {
// ANCHOR: remove-keyboard
var sent = await botClient.SendTextMessageAsync(chatId, "Removing keyboard",
    replyMarkup: new ReplyKeyboardRemove());
// ANCHOR_END: remove-keyboard
    }

    private async Task CallbackButtons()
    {
// ANCHOR: callback-buttons
var buttons = new InlineKeyboardButton[][]
{
    new[] // first row
    {
        InlineKeyboardButton.WithCallbackData("1.1", "11"),
        InlineKeyboardButton.WithCallbackData("1.2", "12"),
    },
    new[] // second row
    {
        InlineKeyboardButton.WithCallbackData("2.1", "21"),
        InlineKeyboardButton.WithCallbackData("2.2", "22"),
    },
};

var sent = await botClient.SendTextMessageAsync(chatId, "A message with an inline keyboard markup",
    replyMarkup: new InlineKeyboardMarkup(buttons));
// ANCHOR_END: callback-buttons
    }

    private async Task UrlButtons()
    {
// ANCHOR: url-buttons
var buttons = new[]
{
    InlineKeyboardButton.WithUrl("Repositoy Link", "https://github.com/TelegramBots/Telegram.Bot")
};

var sent = await botClient.SendTextMessageAsync(chatId, "A message with an inline keyboard markup",
    replyMarkup: new InlineKeyboardMarkup(buttons));
// ANCHOR_END: url-buttons
    }

    private async Task SwitchToInline()
    {
// ANCHOR: switch-to-inline
var buttons = new[]
{
    InlineKeyboardButton.WithSwitchInlineQuery("switch_inline_query"),
    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("switch_inline_query_current_chat"),
};

var sent = await botClient.SendTextMessageAsync(chatId, "A message with an inline keyboard markup",
    replyMarkup: new InlineKeyboardMarkup(buttons));
// ANCHOR_END: switch-to-inline
    }
}
