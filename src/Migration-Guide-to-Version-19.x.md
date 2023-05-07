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

We removed old hierarchy of `InputFile` related classes such as `InputOnlineFile`, `InputTelegramFile`, `InputFileStream`, etc., and also removed all implicit casts to them. From now on you should explicitly specify one of file types: `InputFileStream` for `Stream` content, `InputFileUrl` for URL and `InputFileId` if you want to use existing `file_id`. For convenience the base `InputType` class has factory methods to create the correct types:

- `InputFile.FromStream(Stream stream, string? fileName = default)` for streams
- `InputFile.FromString(string urlOrFileId)` for URLs or file ids
- `InputFile.FromUri(Uri url)` - for URLs as strings
- `InputFile.FromUri(string url)` - for URLs as `URI`s
- `InputFile.FromFileId(string fileId)` - for file ids

The migration scheme looks like that:

| Previous method | New method |
| - | - |
| `new InputTelegramType(string)` | `InputFile.FromId(string)`, `InputFile.FromString(string)` |
| `new InputTelegramType(Stream, string?)` | `InputFile.FromStream(Stream, string?)` |
| `new InputFileStream(Stream)` | `InputFile.FromStream(Stream)` |
| `new InputOnlineFile(string)` | `InputFile.FromId(string)`, `InputFile.FromString(string)`, `InputFile.FromString(string)`, `InputFile.FromUrl(string)`, `InputFile.FromUrl(Uri)` |
| `new InputOnlineFile(Stream, string?)` | `InputFile.FromStream(Stream, string?)` |
| raw `Stream` | `InputFile.FromStream(Stream)` |
| raw `string` | `InputFile.FromString(string)` |
| raw `URI` | `InputFile.FromUrl(URI)` |

## ChatId implicit conversion

Implicit conversion from `ChatId` to `string` was removed due to complaints and problems it caused. The migration path is to explicitly call `ChatId.ToString()` method.

## Stickers

- All methods and types with animated, static and video sticker distinction were removed and replaced with a single set of sticker related methods per new Bot API updates: `AddAnimatedStickerToSetAsync`, `AddStaticStickerToSetAsync`, `AddVideoStickerToSetAsync`, etc. Remove the words `Static`, `Animated` and `Video` from sticker related methods in your code
- Associated emojies and masks were moved to a separate type `InputSticker`, use them there instead, consult the official Bot API docs for a more detailed information

## .NET Core 3.1 removed as a separate target framework

Since .NET Core 3.1 LTS status is not officialy supported anymore we changed the target to `netstandard2.0` and `net6.0` instead. If you're using .NET Core 3.1 or .NET 5 runtimes you need to use build for `netstandard2.0` instead. If you relied on `IAsyncEnumerable` implementation of poller you need to move to .NET 6 instead.

## Other changes

- `Message.Type` returns `MessageType.Animation` when the message contains an `Animation`, use `MessageType.Animation` instead of `MessageType.Document` to check if the message contains an animation
- Property `CanSendMediaMessages` was removed from the types `ChatMemberRestricted` and `ChatPermissions` and replaced with more granular permissions, use them instead
- Removed method `GetChatMembersCountAsync`, use `GetChatMemberCountAsync`
- Removed method `KickChatMemberAsync`, use `BanChatMemberAsync`
- Properties and types `VoiceChatEnded`, `VoiceChatParticipantsInvited`,
`VoiceChatScheduled`, `VoiceChatStarted` removed, use methods and types which start with `Video*` instead
- All propties with the word `Thumb` in them were renamed to contain the word `Thumbnail` per new Bot API updates
- A new type `InlineQueryResultsButton` is used instead of `SwitchPmText` and `SwitchPmParameter` properties, consult the official Bot API docs
