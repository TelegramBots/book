# Migration guide for version 18.0

Most breaking changes in v18 come from new Bot API changes, such as:

1. In [Bot API 6.0](https://core.telegram.org/bots/api#april-16-2022) `voice_chat*` related message properties were
deprecated in favour of `video_chat*` with the same semantics and shape.
2. With introduction of video stickers in [Bot API 5.7](https://core.telegram.org/bots/api#january-31-2022) we needed a way
to separate methods for different sticker types. So static .WEBP `*StickerSet*` methods and requests were given a `Static` prefix.
3. Removed `untilDate` parameter from `TelegramBotClientExtensions.BanChatSenderChatAsync` method and `UntilDate` property from `BanChatSenderChatRequest` class.
4. As of the next update some users will be able to upload up to 4GB files, so we changed `FileBase.FileSize` type to `long?`.
5. A new way of configuring the client.
6. `ApiRequestEventArgs` contains full request data.

Complete list of changes is available in [CHANGELOG](https://github.com/TelegramBots/Telegram.Bot/blob/master/CHANGELOG.md)

## 1. Removal of `VoiceChat*` properties in `Message` object

Telegram renamed `voice_chat_*` properties in the `Message` class and with `video_chat_*` onces so we replaced
corresponding `MessageType` enum members with the new ones.

Following properties in `Message` class and corresponding enum members in `MessageType` enum were changed:

```diff
-VoiceChatScheduled
-VoiceChatStarted
-VoiceChatEnded
-VoiceChatParticipantsInvited
+VideoChatScheduled
+VideoChatStarted
+VideoChatEnded
+VideoChatParticipantsInvited
```

Also property `CanManageVoiceChats` in `ChatMemberAdministrator` and `PromoteChatMemberRequest` classes was renamed to
`CanManageVideoChats`.

## 2. Renaming static sticker methods and classes

With introduction of video stickers in [Bot API 5.7](https://core.telegram.org/bots/api#january-31-2022) we needed a way
to separate methods for different sticker types. We already used `Animated` and `Video` suffix for methods related to animated
and video stickers so we decided to do the same for the static stickers:

- Classes `CreateNewStickerSetRequest` and `AddStickerToSetRequest` were renamed to `CreateNewStaticStickerSetRequest`
and `AddStaticStickerToSetRequest`.
- Methods `CreateNewStickerSetAsync` and `AddStickerToSetAsync` where renamed to
`CreateStaticNewStickerSetAsync` and `AddStaticStickerToSetAsync`.

## 3. Removal of `untilDate` parameter from `BanChatSenderChatAsync` method and `UntilDate` property from `BanChatSenderChatRequest` class

The `untilDate` parameter from `TelegramBotClientExtensions.BanChatSenderChatAsync` method and `UntilDate` property from `BanChatSenderChatRequest` class were removed from the Bot API.

## 4. Lifting of the FileSize limit

As of the next update some users will be able to upload up to 4GB files, so we changed `FileBase.FileSize` type to `long?` to accommodate this change.

## 5. A new way of client configuration

Starting with this release client configuration parameters should be passed through `TelegramBotClientOptions` class.
You need to create an instance of `TelegramBotClientOptions` and pass it to the client:

```csharp
using Telegram.Bot;

var options = new TelegramBotClientOptions(
    token: "<token>"

    // pass an optional baseUrl if you want to use a custom bot server
    baseUrl: "https://custombotserverdomain.com",

    // pass an optional flag `true` if you want to use test environment
    useTestEnvironment = true
);

var client = new TelegramBotClient(options);
```

If you don't know about test environment you can read more about it in the official
[documentation](https://core.telegram.org/bots/webapps#using-bots-in-the-test-environment).

If you don't need extra configuration options you can still use the constructor that accepts a token and an instance of `HttpClient`:

```csharp
var client = new TelegramBotClient("<token>");
```

## 6. Polling functionality in the core library

The latest biggest change which is not a breaking one, but nevertheless worth a note: deprecation of
`Telegram.Bot.Extensions.Polling` package.

All the functionality from the package was merged into the core library under namespace `Telegram.Bot.Polling`.

Name of the method `HandleErrorAsync` in `IUpdateHandler` interface was quite confusing from the beginning since a lot
of people assumed they can handle all errors in it, but in reality it's used only for handling errors during polling.
We decided to give it a more appropriate name: `HandlePollingErrorAsync`.
