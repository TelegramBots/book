# Business Bot Features

[![Bot Business Mode](https://img.shields.io/badge/Bot_API_Doc-Business_Mode_-blue.svg?style=flat-square)](https://core.telegram.org/bots/features#bots-for-business)

Several [business features](https://telegram.org/blog/telegram-business) have been added for premium users to Telegram.

In particular, premium users can now select a bot to act as a [**chatbot on their behalf**](https://telegram.org/blog/telegram-business#chatbots-for-business),
in order to manage/reply to messages from other users _(typically, their business customers)_.  

## BotFather configuration

First, the bot owner need to talk to [@BotFather](https://t.me/BotFather) and go to the **Bot Settings** to enable **Business Mode**

In the following sections, we will refer to the premium user using your chatbot as "*the business owner*".

<video autoplay loop controls muted poster="https://telegram.org/file/400780400238/1/x875tPT245w.58064/1b426d3eda0a923c03" style="max-width: 600px;" title="" alt="Chatbots for Business">
  <source src="https://telegram.org/file/400780400792/3/Y8CWkKZOVHM.3771962.mp4/044a6d7645581d8bf6" type="video/mp4">
</video>

## BusinessConnection update

Once your chatbot is configured, the business owner has to go to their account settings, under **Telegram Business > Chatbots** and type your bot username.

At this point, your bot will receive an `update.BusinessConnection` which contains:
- a unique id _(you may want to store this)_
- details on the User (business owner)
- IsEnabled _(false if the business connection got cancelled)_
- CanReply _(if the bot can act on behalf of that user in chats that were active in the last 24 hours)_

You can retrieve these info again later using `GetBusinessConnectionAsync(id)`

## BusinessMessage updates

From now on, your bot will receive updates regarding private messages between users (customers) and the business owner:
- `update.BusinessMessage`: a customer sent a new message to the business owner
- `update.EditedBusinessMessage`: a customer modified one of its message sent to the business owner
- `update.DeletedBusinessMessages`: a customer deleted some messages in private with the business owner

In these messages/updates, the field `BusinessConnectionId` will tell you which BusinessConnection this applies to
_(useful for context if your chatbot is used by several business owners)_

## Acting on behalf of the business owner

If the business owner enabled "**Reply to message**" during the initial business connection,
your bot can reply or do some other actions on behalf of their user account.

To do so, you can call many Bot API methods with the optional _businessConnectionId:_ parameter.

This way your bot can send/edit/pin messages, send chat actions (like _"typing"_), manage polls/live location, as if you were the business owner user.

Some notes about messages sent on behalf of the business owner:
- They will NOT be maked with your bot name from the customer point of view
- They will be marked with your bot name in the business owner private chat (a banner also appears on top of the chat)
- These features are limited to private chats initiated by customers talking to the business owner.
