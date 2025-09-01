﻿#  Frequently Asked Questions

I recommend you read all of these as you will learn many interesting things. Or you can use Ctrl-F to search for a specific topic.

<!-- toc -->

### _1. Can you give me documentation/examples links?_
- Here is the [main documentation website](https://telegrambots.github.io/book/).
- You can find [more bot example projects](https://github.com/TelegramBots/Telegram.Bot.Examples) here
- Search the [official API documentation](https://core.telegram.org/bots/api) and [official bots FAQ](https://core.telegram.org/bots/faq).
- check tooltips in your IDE, or navigate with F12 on API methods and read/expand comments.

>If you're C# beginner, you should learn about [async programming](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/).

### _2. My update handler fails or stops executing at some point_
You likely have an exception somewhere. You should place a `try..catch` around your whole update handler.  
Also, you should learn to <ins>use a debugger</ins> and go step-by-step through your code to understand where and why an exception is raised. See next question.

### _3. Apparently my update handler gets a_ `NullReferenceException`
Not all updates are about an incoming Message, so `update.Message` could be null. (see also `update.Type`)  
Not all messages are text messages, `message.Text` could be null (see also `message.Type`). etc...  
So please <ins>use a debugger</ins> to check the content of your variables or structure fields and make sure your code can handle all cases.

### _4. How to add buttons under a message?_
Pass an [InlineKeyboardMarkup](2/reply-markup.md#inline-keyboards) into the `replyMarkup` parameter when sending the message. You will likely need to create a `List<List<InlineKeyboardButton>>` for rows&columns  
_See also next question._

### _5. How to handle a click on such inline buttons?_
For buttons with callback data, your update handler should handle `update.CallbackQuery`.
_(Remember that not all updates are about `update.Message`. See question #3)_  

Your code should answer to the query within 10 seconds, using `AnswerCallbackQuery` _(or else the button gets momentarily disabled)_

### _6. How to show a popup text to the user?_
It is only possible with inline callback button _(see above questions)_.  
Use `AnswerCallbackQuery` with some text, and pass parameter `showAlert: true` to display the text as an alert box instead of a short popup.

### _7. How to fill the input textbox of the user with some text?_
There is not a simple direct method for this, but here is what you can try:
- With a [Public username link](https://core.telegram.org/api/links#public-username-links): `t.me/username?text=Hello+World`  
  _(works only if target is a user/bot and not the current chat)_
- With a [Share link](https://core.telegram.org/api/links#share-links): `tg://msg_url?url=https://example.com&text=Hello+World`  
  _(user must select a target first)_
- With a [Bot deep link](https://core.telegram.org/api/links#bot-links): `t.me/botusername?start=param`  
  _(param is limited to base64 characters, bot will receive `/start param`)_
- With a [ReplyKeyboardMarkup](2/reply-markup.md#custom-reply-keyboards): buttons under the textbox to send pre-made texts
- With an [Inline Mode bot](3/inline.md) and `SwitchInlineQuery` inline buttons, you can make the user pre-type the name of your bot followed by some query

### _8. How to fetch previous messages?_
You can't with Bot API but it's possible with [WTelegramBot](https://www.nuget.org/packages/WTelegramBot#readme-body-tab).  
Normally, bots only get messages at the moment they are posted. You could archive them all in a database for later retrieval.

### _9. How to fetch a list of all users in chat?_
You can't with Bot API but it's possible with [WTelegramBot](https://www.nuget.org/packages/WTelegramBot#readme-body-tab).  
Normally, bots can only get the list of admins (`GetChatAdministrators`) or detail about one specific member (`GetChatMember`)  
Alternatively, you can keep track of users by observing new messages in a chat and saving user info into a database.

### _10. How to send a private message to some random user?_
You can't. Bots can only send private messages to users that have already initiated a private chat with your bot.

### _11. How to detect if a user blocked my bot?_
You would have received an `update.MyChatMember` with `NewChatMember.Status == ChatMemberStatus.Kicked`  
If you didn't record that info, you can try to `SendChatAction` and see if it raises an exception.

### _12. How to set a caption and ReplyMarkup to a media group (album)?_
Set the `media.Caption` (and `media.ParseMode`) on the first or last media

Media groups can't have Reply Markup unfortunately.

### _13. How to write a bot that make questions/answers with users?_
Either you can code a complex state machine workflow, saving where each user is currently in the discussion tree.  
Or you can just use [YourEasyBot](https://github.com/wiz0u/YourEasyBot) which makes sequential bots very simple to write... _(or one of the [other frameworks](https://github.com/TelegramBots/Telegram.Bot/wiki) available for Telegram.Bot)_

### _14. How to make font effects in message?_
Pass a `ParseMode.Html` _(or `ParseMode.MarkDownV2`)_ to argument `parseMode`. See [formatting options](https://core.telegram.org/bots/api#formatting-options).  
⚠️ I <ins>**highly recommend**</ins> you choose HTML formatting because MarkDownV2 has A LOT of annoyingly reserved characters and you will regret it later.

### _15. Where can I host my bot online for cheap/free?_
I would recommend you make an ASP.NET webhook bot and host it on some WebApp hosting service.  
For example, [Azure WebApp Service](https://azure.microsoft.com/products/app-service/web) has a [F1 Free plan](https://azure.microsoft.com/pricing/details/app-service/windows/) including 1 GB disk, 1 GB ram, 60 minutes of daily cumulated active CPU usage _(more than enough for most bots without heavy use)_. And publishing to Azure is very easy from VS.  
A credit-card is necessary but you shouldn't get charged if you stay within quotas.  
Other cloud providers might also offer similar services.

### _16. Is there some limitation/maximum about feature X?_
See <https://limits.tginfo.me> for a list of limitations.

### _17. How to populate the bot Menu button / commands list?_
You can either do this via [@BotFather](https://t.me/BotFather) _(static entries)_, or you can use `SetMyCommands` for more advanced settings  
⚠️ This menu can only be filled with bot commands, starting with a `/` and containing only latin characters `a-z_0-9`

### _18. How to receive `ChatMember` updates?_
You should specify all update types **including ChatMember** in `AllowedUpdates` array on `StartReceiving`:`ReceiverOptions` or `SetWebhook`  
[More details here](2/chats.md#detecting-new-group-members-and-changed-member-status)

### _19. How to get rid of past updates when I restart my bot?_
Pass true into `StartReceiving`:`ReceiverOptions`:`DropPendingUpdates` or `SetWebhook`:`dropPendingUpdates`
Alternatively, you can call `await bot.DropPendingUpdates()` before polling or using [`bot.OnUpdate`](3/updates/polling.md#by-setting-botonupdate-andor-botonmessage).

### _20. Difficulties to upload & send a file/media?_
- Make sure you `await` until the end of the send method before closing the file (a "`using`" clause would close the file on leaving the current { scope }
- If you just filled a `MemoryStream`, make sure to rewind it to the beginning with `ms.Position = 0;` before sending
- If you send a media group, make sure you specify different filenames on `InputFile.FromStream`

### _21. How to fetch all medias from an album/media group ?_
When receiving a media group, you need to collect the series of separate messages in a chat having the same `MediaGroupId` property.  
There is no way to know how many medias/messages the album is composed of.
[Read more here](2/send-msg/media-msg.md#receiving-a-media-group-album)

### _22. How to send a custom emoji❓_
⚠️ It costs about ~$5,000 !! 😱
- First you need to buy a reserved username on [Fragment](https://fragment.com/).
- Then you need to pay an [additional upgrade fee](https://fragment.com/about#assigning-collectible-usernames-to-telegram) of 1K TON to apply that username to your bot.  
- Finally, your bot can now post custom emojis using specific [HTML](https://core.telegram.org/bots/api#html-style) or [Markdown](https://core.telegram.org/bots/api#markdownv2-style) syntax (or entity).

To post to a specific group, there is an alternative solution:
- Have premium members boost your group to Level 4.
- Then you can assign a custom emoji pack to your group that your members AND bots can use freely in group messages.

### _23. How to upgrade my existing code? You keep breaking compatibility!_
A new lead developer (Wizou) is now in charge of the library and commits to reduce code-breaking changes in the future.  
Version 21.x of the library and later have been much improved to facilitate [migration from previous versions](migrate/Version-21.x.md) of the library, and include a lot of helpers/implicit operators to simplify your code.

### _24. Can I use several apps/instance to manage my bot?_
You can call API methods (like sending messages) from several instances in parallel  
**BUT** only one instance can call method GetUpdates (or else you will receive _Telegram API Error 409: Conflict: terminated by other getUpdates request_)

To process the updates in parallel, read [this section of our documentation](3/updates/#sequential-vs-parallel-updates).

### _25. How do I get the user id from a username?_

You can't with Bot API but it's possible with [WTelegramBot](https://www.nuget.org/packages/WTelegramBot#readme-body-tab).  
Alternatively, you could store in database the mapping of `UserId`<->`Username`.  
Remember that not every user has a username, and it can be changed.  
Also, your bot still won't be able to contact a user first.

### _26. How to receive messages from channels?_

Your bot has to be added as administrator of the channel.
You will then receive the messages as `update.ChannelPost` or `update.EditedChannelPost`.

### _27. How to send the same media multiple times_
The first time, you will send the media with a stream (upload). Next times, you will use its **FileId**:
```csharp
var msg = await bot.SendVideo(chatId, stream, ....);
var fileId = msg.Video.FileId

// next times:
await bot.SendVideo(chatId2, fileId, ...);
```
For photos, use `msg.Photo[^1].FileId`


### This FAQ doesn't have my question on it

Feel free to [join our Telegram group](https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA) and ask your question there
