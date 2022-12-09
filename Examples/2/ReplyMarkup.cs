using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ANCHOR: usings
// using Telegram.Bot.Types.ReplyMarkups;
// ANCHOR_END: usings

namespace BookExamples.Chapter2;

internal class ReplyMarkup
{
    private readonly ITelegramBotClient botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");
    private readonly ChatId chatId = 12345;
    private readonly CancellationToken cancellationToken = new CancellationTokenSource().Token;

    private async Task SingleRowMarkup()
    {
// ANCHOR: single-row
ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
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
// ANCHOR_END: single-row
    }

    private async Task MultipleRowMarkup()
    {
// ANCHOR: multiple-row
ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { "Help me" },
    new KeyboardButton[] { "Call me ☎️" },
})
{
    ResizeKeyboard = true
};

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Choose a response",
    replyMarkup: replyKeyboardMarkup,
    cancellationToken: cancellationToken);
// ANCHOR_END: multiple-row
    }

    private async Task RequestInfo()
    {
// ANCHOR: request-info
ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    KeyboardButton.WithRequestLocation("Share Location"),
    KeyboardButton.WithRequestContact("Share Contact"),
});

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Who or Where are you?",
    replyMarkup: replyKeyboardMarkup,
    cancellationToken: cancellationToken);
// ANCHOR_END: request-info
    }

    private async Task RemoveKeyboard()
    {
// ANCHOR: remove-keyboard
Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Removing keyboard",
    replyMarkup: new ReplyKeyboardRemove(),
    cancellationToken: cancellationToken);
// ANCHOR_END: remove-keyboard
    }

    private async Task CallbackButtons()
    {
// ANCHOR: callback-buttons
InlineKeyboardMarkup inlineKeyboard = new(new[]
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
// ANCHOR_END: callback-buttons
    }

    private async Task UrlButtons()
    {
// ANCHOR: url-buttons
InlineKeyboardMarkup inlineKeyboard = new(new[]
{
    InlineKeyboardButton.WithUrl(
        text: "Link to the Repository",
        url: "https://github.com/TelegramBots/Telegram.Bot")
});

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "A message with an inline keyboard markup",
    replyMarkup: inlineKeyboard,
    cancellationToken: cancellationToken);
// ANCHOR_END: url-buttons
    }

    private async Task SwitchToInline()
    {
// ANCHOR: switch-to-inline
InlineKeyboardMarkup inlineKeyboard = new(new[]
{
    InlineKeyboardButton.WithSwitchInlineQuery(
        text: "switch_inline_query"),
    InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(
        text: "switch_inline_query_current_chat"),
});

Message sentMessage = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "A message with an inline keyboard markup",
    replyMarkup: inlineKeyboard,
    cancellationToken: cancellationToken);
// ANCHOR_END: switch-to-inline
    }
}
