**This guide assumes prior knowledge of the C# programming language.**

Let's create a simple bot, that responds with "Hello" to any message it receives.

Operating such a bot is a two-step process:
1. Wait for a message from a user
2. Send a response

Step one is what we call `Getting updates`, and is more thoroughly described [here](Getting-Updates.md).

### Let's get coding

We will be working with a Console project.

First things first, reference the `Telegram.Bot` [NuGet package].

And add some using statements:
```csharp
using Telegram.Bot;             // TelegramBotClient
using Telegram.Bot.Args;        // MessageEventArgs
using Telegram.Bot.Types;       // Message
using Telegram.Bot.Types.Enums; // MessageType
```

Now let's create an instance of the `TelegramBotClient`, that we will be using.


```csharp
static readonly TelegramBotClient Bot = new TelegramBotClient("API Token");
```

The library provides a simple system for getting new updates as events.

To use it, subscribe to the `OnMessage` event and create the callback method.
We must also call `StartReceiving` to actually start the process.
```csharp
Bot.OnMessage += Bot_OnMessage;
Bot.StartReceiving();
```

This method will be called any time a user sends the bot a message (be it text, images, video ...).

The `MessageEventArgs` contain the `Message` object, that is important to us.
We only want to respond when the user sends us text, so we'll check that the message is of type `Text`.
```csharp
private static async void Bot_OnMessage(object sender, MessageEventArgs e)
{
    Message message = e.Message;

    if (message.Type == MessageType.Text)
    {

    }
}
```
Note the `async` modifier. All methods in `TelegramBotClient` are async.
To send a message we will use the `SendTextMessageAsync` method.

The method accepts two main parameters: chat and text.

`Chat` is the id of the chat, that you want to send the message to.
We will send it to the same chat we got the message from (`message.Chat`).

The text will, of course, be "Hello".
```csharp
await Bot.SendTextMessageAsync(message.Chat, "Hello");
```

### That is it!

The entire code should now look something like [this][Code gist].

Now all that is left is to start the bot up.

You can do it on your computer if you want, but you should also take a look at [other deployment options](Deploying-Your-Bot.md).


[NuGet package]: https://www.nuget.org/packages/Telegram.Bot/
[Code gist]: https://gist.github.com/MihaZupan/c5076b06c5b73cbc3b0f71cc6e6b4709