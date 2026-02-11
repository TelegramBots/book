using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

#pragma warning disable IDE0079
#pragma warning disable CA1861

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
var sent = await bot.SendMessage(chatId, "Choose a response",
                                 replyMarkup: new[] { "Help me", "Call me ☎️" });
// ANCHOR_END: single-row
    }

    private async Task MultipleRowMarkup()
    {
// ANCHOR: multiple-row
var sent = await bot.SendMessage(chatId, "Choose a response", replyMarkup: new string[][]
{
    ["Help me"],
    ["Call me ☎️", "Write me ✉️"]
});
// ANCHOR_END: multiple-row
    }

    private async Task RequestInfo()
    {
// ANCHOR: request-info
var sent = await bot.SendMessage(chatId, "Who or Where are you?", replyMarkup: new KeyboardButton[]
{
    KeyboardButton.WithRequestLocation("Share Location"),
    KeyboardButton.WithRequestContact("Share Contact")
});
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
var sent = await bot.SendMessage(chatId, "A message with an inline keyboard markup",
    replyMarkup: new InlineKeyboardButton[][]
    {
        [("1.1", "11"), ("1.2", "12")], // two buttons on first row
        [("2.1", "21"), ("2.2", "22")]  // two buttons on second row
    });
// ANCHOR_END: callback-buttons
    }

    private async Task UrlButtons()
    {
// ANCHOR: url-buttons
var sent = await bot.SendMessage(chatId, "A message with an inline keyboard markup",
        replyMarkup: new InlineKeyboardButton("Repository Link", "https://github.com/TelegramBots/Telegram.Bot"));
// ANCHOR_END: url-buttons
    }

    private async Task SwitchToInline()
    {
// ANCHOR: switch-to-inline
var sent = await bot.SendMessage(chatId, "A message with an inline keyboard markup",
    replyMarkup: new InlineKeyboardButton[]
    {
        InlineKeyboardButton.WithSwitchInlineQuery("switch_inline_query"),
        InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("switch_inline_query_current_chat")
    });
// ANCHOR_END: switch-to-inline
    }

    private async Task ButtonsStyle()
    {
// ANCHOR: buttons-style
var sent = await bot.SendMessage(chatId, "A button with style and custom emoji",
            replyMarkup: new InlineKeyboardButton("Styled button", "CallbackData or Url")
                { Style = KeyboardButtonStyle.Primary, IconCustomEmojiId = "5373141891321699086" });
// ANCHOR_END: buttons-style
    }
}
