# Migration guide for version 18.0

Most breaking changes in v18 come from new Bot API changes, such are:

1. Deprecation of `voice_chat*` related message properties in favour of `video_chat*` properties with the same
semantics and shape
2. A new animated sticker formats required differentiation for different sticker related requests, so static
`StickerSet` releated methods and requests were renamed using `Static` suffix
3. A new way of configuring the client

## 1. Removal of `VoiceChat*` properties in `Message` object

Telegram deprecated `voice_chat_*` properties in Bot API and replaced them with `video_chat_*` onces so we replaced
deprecated properties and corresponding `MessageType` enum members with new ones.

Following properties in `Message` object and enum members in `MessageType` enum were changed:

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

Also property `CanManageVoiceChats` in `ChatMemberAdministrator` and `PromoteChatMemberRequest` class was renamed to
`CanManageVideoChats`.

## 2. Renaming static sticker related methods and classes

Since Telegram added video stickers in one of the previous Bot API updates we needed a way to differentiate between
methods and classes for creating and senging static, animated and video stickers. We already used `Animated` and
`Video` suffix for methods related to animated and video stickers so we decided to do the same with static stickers:

- Classes `CreateNewStickerSetRequest` and `AddStickerToSetRequest` were renamed to `CreateNewStaticStickerSetRequest`
and `AddStaticStickerToSetRequest`
- Methods `AddStickerToSetAsync` and `CreateNewStickerSetAsync` where renamed to `AddStaticStickerToSetAsync` and
`CreateStaticNewStickerSetAsync`

## 3. A new way of configuration

From this version all client configuration related to Bot API is passed through `TelegramBotClientOptions` class.
You need to first instantiate an instance of it and pass it to the client

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

We left a constructor that accepts only the token and an instance of `HttpClient` since most people don't use custom
bot servers or test environments so it's more convenient to just pass the token.

```csharp
var client = new TelegramBotClient("<token>");
```


## Polling functionality in the core library

The latest biggest change wich is not a breaking one, but nevertheless worth a note: deprecation of
`Telegram.Bot.Extensions.Polling` package.

All the functionality from the packge was merged into the core library under namespace `Telegram.Bot.Polling`.

Name of the method `HandleErrorAsync` in `IUpdateHandler` interface was quite confusing from the beginning since a lot
of people assumed the can handle all errors in it, but in reality it's used only for handling errors during polling.
We decided to give it a more appropriate name: `HandlePollingErrorAsync`.
