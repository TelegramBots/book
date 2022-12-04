# Migration guide for version 19.0

## Topics in Groups

New topics functionality allow bots interact with users in topic specified by `messageThreadId` parameter.

We try to keep our Bot API implementation as close to [Telegram Bot API](https://core.telegram.org/bots/api#forumtopicclosed) as possible. This means, that the new `messageThreadId` now the first optional parameter for a variety of methods.

Consider to use named parameters to avoid confusion with changed parameter order.

```diff
-Message message = await BotClient.SendTextMessageAsync(
-    _fixture.SupergroupChat.Id,
-    "Please click on *Notify* button.",
-    cancellationToken);
+Message message = await BotClient.SendTextMessageAsync(
+    chatId: _fixture.SupergroupChat.Id,
+    text: "Please click on *Notify* button.",
+    messageThreadId: threadId,
+    cancellationToken: cancellationToken);
```

## New InputFile Hierarchy

Old `InputMedia*` class hierarchy poorly reflected actual file-related APIs.

We removed `InputOnlineFile` class and implicit casts to it. From now on you should explicitly specify one of file types: `InputFile` for `Stream` content, `InputFileUrl` for URL and `InputFileId` if you want to use existing `file_id`.

## ChatId implicit conversion

## ContainsMasks

Added the field sticker_type to the class StickerSet, describing the type of stickers in the set.
The field contains_masks has been removed from the documentation of the class StickerSet. The field is still returned in the object for backward compatibility, but new bots should use the field sticker_type instead.
Added the parameter sticker_type to the method createNewStickerSet.
The parameter contains_masks has been removed from the documentation of the method createNewStickerSet. The parameter will still work for backward compatibility, but new bots should use the parameter sticker_type instead.
