# Dealing with chats

All messages in Telegram are sent/received on a specific chat.  
The `chat.Type` can be one of 4 types:

- `ChatType.Private`:  
  A private discussion with a user. The `chat.Id` is the same as the `user.Id` (positive number)
- `ChatType.Group`:  
  A private chat group with less than 200 users
- `ChatType.Supergroup`:  
  An advanced chat group, capable of being public, supporting more than 200 users, with specific user/admin rights
- `ChatType.Channel`:  
  A broadcast type of publishing feed (only admins can write to it)

Additional notes:
- For groups/channels, the `chat.Id` is a negative number, and the `chat.Title` will be filled.
- For <u>public</u> groups/channels, the `chat.Username` will be filled.
- For private chat with a user, the `chat.FirstName` will be filled, and optionally, the `chat.LastName` and `chat.Username` if the user has one.

## Calling chat methods

All methods for dealing with chats _(like sending messages, etc..)_ take a `ChatId` parameter.

For this parameter, you can pass directly a	`long` _(the chat or user ID)_,
or when sending to a public group/channel, you can pass a `"@chatname"` string

### Getting full info about a chat (`GetChat`)

Once a bot has joined a group/channel or has started receiving messages from a user, it can use method `GetChat` to get detailed info about that chat/user.

There are lots of information returned depending on the type of chat, and most are optional and may be unavailable.  
Here are a few interesting ones:
* For private chat with a User:
	- Birthdate
	- Personal channel
	- [Business](../4/business.md) information
	- Bio
* For groups/channels:
	- Description
	- default Permissions _(non-administrator access rights)_
	- Linked ChatId _(the associated channel/discussion group for this chat)_
	- IsForum _(This chat group has topics. There is no way to retrieve the list of topics)_
* Common information for all chats:
	- Photo _(use `GetInfoAndDownloadFile` and the `photo.BigFileId` to download it)_
	- Active Usernames _(premium user & public chats can have multiple usernames)_
	- Available reactions in this chat
	- Pinned Message _(the most recent one)_


## Receiving chat messages

See chapter [Getting Updates](../3/updates/README.md) for how to receive updates & messages.

For groups or private chats, you would receive an update of type `UpdateType.Message` (which means only the field `update.Message` will be set)

For channel messages, you would receive an update with field `update.ChannelPost`.

For [business](../4/business) messages, you would receive an update with field `update.BusinessMessage`.

If someone modifies an existing message, you would receive an update with one of the fields `update.Edited*`

Note: if you use the `bot.OnMessage` event, this is simplified and you can just check the UpdateType argument.

> [!IMPORTANT]  
> By default, for privacy reasons, bots in groups receive only messages that are targeted at them (reply to their messages, inline messages, or targeted `/commands@botname` with the bot username suffix)  
> If you want your bot to receive ALL messages in the group, you can either make it admin, or <u>disable</u> the **Bot Settings** : [**Group Privacy** mode](https://core.telegram.org/bots/features#privacy-mode) in [@BotFather](https://t.me/botfather)

## Migration to Supergroup

When you create a private chat group in Telegram, it is usually a `ChatType.Group`.

If you change settings (like admin rights or making it public), or if members reach 200,
the Group may be migrated into a Supergroup.

In such case, the Supergroup is like a separate chat with a different ID. 
The old Group will have a service message `MigrateToChatId` with the new supergroup ID.
The new Supergroup will have a service message `MigrateFromChatId` with the old group ID.

## Managing new members in a group

Bots can't directly add members into a group/channel.  
To invite users to join a group/channel, you can send to the users the public link `https://t.me/chatusername` (if chat has a username), or invite links:

### Invite links

Invite links are typically of the form `https://t.me/+1234567890aAbBcCdDeEfF` and allow users clicking on them to join the chat.

You can send those links as a text message or as an `InlineKeyboardButton.WithUrl(...)`.

If your bot is administrator on a private (or public) group/channel, it can:
- read the (fixed) primary link of the chat:
```csharp
var chatFullInfo = await bot.GetChat(chatId); // you should call this only once
Console.WriteLine(chatFullInfo.InviteLink);
```
- create new invite links on demand
```csharp
var link = await bot.CreateChatInviteLink(chatId, "name/reason", ...);
Console.WriteLine(link.InviteLink);
```

See also [some other methods for managing invite links](https://core.telegram.org/bots/api#exportchatinvitelink).

### Detecting new group members and changed member status

Note: Bots can't detect new <u>channel</u> members

The simpler approach to detecting new members joining a group is to handle service messages of type `MessageType.NewChatMembers`: the field `message.NewChatMembers` will contain an array of the new User details.  
Same for a user leaving the chat, with the `message.LeftChatMember` service message.

However, under various circumstances (bigger groups, hidden member lists, etc..), these service messages may not be sent out.  

The more complex (and more reliable) approach is instead to handle updates of type `UpdateType.ChatMember`:

* First you need to enable this specific update type among the `allowedUpdates` parameter when calling `GetUpdates`, `SetWebhook` or `StartReceiving`+`ReceiverOptions`.
* Typically, you would pass `Update.AllTypes` as the allowedUpdates parameter.
* After that, you will receive an `update.ChatMember` structure for each user changing status with their old & their new status
* The `OldChatMember`/`NewChatMember` status fields can be one of the derived `ChatMember*` class: `Owner`/`Creator`, `Administrator`, `Member`, `Restricted`, `Left`, `Banned`/`Kicked`)
