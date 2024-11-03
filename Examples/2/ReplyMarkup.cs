using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// ANCHOR: usings
// using Telegram.Bot.Types.ReplyMarkups;
// ANCHOR_END: usings

namespace BookExamples.Chapter2;

internal class ReplyMarkup
{
    public readonly ITelegramBotClient bot = new TelegramBotClient("YOUR_BOT_TOKEN");
    public readonly ChatId chatId = 12345;

    private async Task SingleRowMarkup()
    {
// ANCHOR: single-row
var replyMarkup = new ReplyKeyboardMarkup(true)
    .AddButtons("Help me", "Call me ☎️");

var sent = await bot.SendMessage(chatId, "Choose a response", replyMarkup: replyMarkup);
// ANCHOR_END: single-row
    }

    private async Task MultipleRowMarkup()
    {
// ANCHOR: multiple-row
var replyMarkup = new ReplyKeyboardMarkup(true)
    .AddButton("Help me")
    .AddNewRow("Call me ☎️", "Write me ✉️");

var sent = await bot.SendMessage(chatId, "Choose a response", replyMarkup: replyMarkup);
// ANCHOR_END: multiple-row
    }

    private async Task RequestInfo()
    {
// ANCHOR: request-info
var replyMarkup = new ReplyKeyboardMarkup()
    .AddButton(KeyboardButton.WithRequestLocation("Share Location"))
    .AddButton(KeyboardButton.WithRequestContact("Share Contact"));

var sent = await bot.SendMessage(chatId, "Who or Where are you?", replyMarkup: replyMarkup);
// ANCHOR_END: request-info
    }

    private async Task RemoveKeyboard()
    {
// ANCHOR: remove-keyboard
await bot.SendMessage(chatId, "Removing keyboard", replyMarkup: new ReplyKeyboardRemove());
// ANCHOR_END: remove-keyboard
    }

    private async Task CallbackButtons()
    {
// ANCHOR: callback-buttons
var inlineMarkup = new InlineKeyboardMarkup()
    .AddButton("1.1", "11") // first row, first button
    .AddButton("1.2", "12") // first row, second button
    .AddNewRow()
    .AddButton("2.1", "21") // second row, first button
    .AddButton("2.2", "22");// second row, second button

var sent = await bot.SendMessage(chatId, "A message with an inline keyboard markup",
    replyMarkup: inlineMarkup);
// ANCHOR_END: callback-buttons
    }

    private async Task UrlButtons()
    {
// ANCHOR: url-buttons
var inlineMarkup = new InlineKeyboardMarkup()
    .AddButton(InlineKeyboardButton.WithUrl("Repository Link", "https://github.com/TelegramBots/Telegram.Bot"));

var sent = await bot.SendMessage(chatId, "A message with an inline keyboard markup",
    replyMarkup: inlineMarkup);
// ANCHOR_END: url-buttons
    }

    private async Task SwitchToInline()
    {
// ANCHOR: switch-to-inline
var inlineMarkup = new InlineKeyboardMarkup()
    .AddButton(InlineKeyboardButton.WithSwitchInlineQuery("switch_inline_query"))
    .AddButton(InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("switch_inline_query_current_chat"));

var sent = await bot.SendMessage(chatId, "A message with an inline keyboard markup",
    replyMarkup: inlineMarkup);
// ANCHOR_END: switch-to-inline
    }
}
