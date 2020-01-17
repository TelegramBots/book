# Reply Markup

TODO: write documentation.

```c#
                await botClient.SendTextMessageAsync(
                    chatId: approveChatId,
                    text: $"Please approve post",
                    replyMarkup:
                     new InlineKeyboardMarkup(new InlineKeyboardButton[]
                     {
                         InlineKeyboardButton.WithCallbackData("+", "approve"),
                         InlineKeyboardButton.WithCallbackData("-", "decline"),
                     }));
```
