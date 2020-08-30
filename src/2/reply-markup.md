# Reply Markup

This chapter is not yet written.
Use reply markup like this:

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