# FAQ

## How do I use an HTTP/Socks proxy?

Look at the wiki page: [Working Behind a Proxy](4/proxy.md).

## I got a '409' error. What do I do?

You are trying to receive updates multiple times at the same time. Either you are calling GetUpdates from two instances of the bot, or you are calling GetUpdates while a web hook is already set. That is not supported by the API, only receive on one instance.

## How do I get the user id from a username?

There is no way to do that with the API directly.
You could store a list of known usernames, mapped to ids.
This is *not* recommended, because usernames can be changed.

## How do I get updates in channels?

If you are using polling, you will have to subscribe to the `OnUpdate` event.
Check the `UpdateType` of the `Update`. If it is `UpdateType.ChannelPost` then the `Update.ChannelPost` property will be set.

## I have serialization issues or null values in `Update` object in my webhook. What do I do?

If you're using ASP.NET Core 3.0+ you need to configure System.Text.Json options to work correctly with Telegram.Bot

See [this page for instructions](Migration-Guide-to-Version-21.x.md#webhooks-with-systemtextjson).

## Is there a way to get a list of users in a group or a channel?

There's no API to get all users in a chat, there's only [`getChatMember`](https://core.telegram.org/bots/api#getchatmember) request to obtain a
[`ChatMember`](https://core.telegram.org/bots/api#chatmember) object knowing it's `user_id`.

You can keep track of users observing new messages in a chat and saving user info into a database.

## This FAQ doesn't have my question on it. Where can I get my torch and pitchfork?

Check the [`Bots FAQ by Telegram`] and if that doesn't pan out, feel free to let us know in the [public group chat].

[`Bots FAQ by Telegram`]: https://core.telegram.org/bots/faq
[Public Group Chat]: https://t.me/joinchat/B35YY0QbLfd034CFnvCtCA
