# Migration guide for version 17.0

There are several breaking changes in v17:

- New exceptions handling logic
- Removal of update and message events
- Removal of API methods from `ITelegramBotClient` interface and moving them into extension methods in the same namespace (that shouldn't break anyone's sources as long as they don't employ reflection or make their own interface implementations)
- Working with default enum values

These are the most user facing breaking changes you should be aware of during migration.

Let's dive deep on the migrations.

## New exceptions handling logic

v17 brings a new base type for exceptions: `RequestException`. `ApiRequestException`  inherits from `RequestException` and is thrown only when an actual error response with the correct body is received from the Bot API. In other situations `RequestException` will be thrown instead containing actual exception as `InnerException` if there is one, e.g. serialization or connection-related exceptions.

If you used `ApiRequestException` and `HttpRequestException` to handle most exception now you have to replace `HttpRequestException` with `RequestException` and look for the inner exception. All valid errors with JSON body from Telegram are now thrown as `ApiRequestException` including `429: Too Many Requests`.

Since `5XX` responses don't usually include correct JSON body they are thrown as `RequestException` with `HttpRequestException` inside.

Look at the following example on how to handle different kinds of exceptions. You might not need to implement everything as you see, it's there only for demonstration purposes.

```csharp
try
{
    await bot.SendTextMessageAsync(chatId, "Hello");
}
catch (ApiRequestException exception)
{
    switch (exception.StatusCode)
    {
        case 400:
            // Handle incorrect requests exceptions
            break;
        case 401:
            // Handle incorrect bot token exception (revoked tokens)
            break;
        case 403:
            // Handle authorization exceptions (blocked users, unaccessible chats, etc)
            break;
        case 429:
            // Handle rate limiting exception
            break;
        default:
            // Handle other errors with valid json body: it includes status code and description of the error
            break;
    }
}
catch (RequestException exception)
{
    if (exception.InnerException is HttpRequestException httpRequestException)
    {
        // Handle connection exceptions or 5XX exceptions from the Bot API
    }
    else if (exception.InnerException is JsonSerializationException serializationException)
    {
        // Handle serialization exception when a request or a response can't be serialized for some reason
    }
    else
    {
        // Handle all other exceptions
    }
}
catch (OperationCancelledException exception)
{
    // Handle cancellation exception, e.g. when CancellationToken is cancelled
}
```

## Removal of events

In v17 we removed events and introduced a new way of getting updates with [Telegram.Bot.Extensions.Polling] package. You can find an example in [First Chat Bot](./1/example-bot.md) article.

## Removal of API methods from `ITelegramBotClient` interface

This change shouldn't affect most users, the methods are still there, but instead of being implementations of the interface they are now extension methods. It makes the interface leaner and easier to implement for custom clients and for decorators (e.g. rate limiters implemented as decorators). There isn't really a migration path for those who used these for some reason.

## Working with default enum values

We changed how we work with enums. The most notable change is the default value: there is none, all our enums are now start with 1 (exception `UpdateType` and `MessageType` since they are not a part of the Bot API and we fully control these). `0` value is left unassigned for a purpose: if we encounter an unknown value in the response from the Bot API we assign `0` as its value.

Let's imagine that Telegram adds new `MessageEntity` value. From now on all unknown values can be handled in the `default` case of `switch` statement.

```csharp
MessageEntity entity = message.Entities.First();

switch (entity.Type)
{
    case MessageEntityType.Username:
        // ...
        break;
    case MessageEntityType.Command:
        // ...
        break;
    default:
        // All unknown values will go there
        break;
}
```

Also some default enums values were removed, e.g. `ParseMode.Default` since we started using nullable types for every optional value and `ParseMode.Default` lost it's use. If a message doesn't have any markup you'll receive `null` in places where `ParseMode` type was used or if you want to explicitly indicate an absence of markup pass `null` instead.

## Other breaking changes

### Constructor accepting IWebProxy

We removed constructor accepting `IWebProxy`. Now you have to configure HttpClient yourself to use proxy. You can find examples in [Working Behind a Proxy](./4/proxy.md#http-proxy) article.

### InputMediaType

Property `Type` in `IInputMedia` was changed to an enum `InputMediaType` for easier discoverability. So if you relied string values like `photo`, `video`, `animation` and so on now you need to switch to using enums. As a result you'll get autocomplete in IDEs and more predictability of what types of input media there are.

### EncryptedPassportElementType

Property `Type` of `EncryptedPassportElement` was replace with an enum for the same reason with `EncryptedPassportElementType` enum.

### ChatMember

As part of Bot API 5.3 implementation `ChatMember` type was split into a hierarchy of types with a discriminator field `Status`. If you need to access some data from the derived class you should use pattern matching or type casting like this:

```csharp
ChatMember member = ... //;

if (chatMember is ChatMemberKicked kickedMember)
{
    // now you can access properties of a kicked chat member
    if (kickedMember.Until is not null)
    {
        // do something with the value of Until
    }
}
```

### ChatId

Fields `Identifier` and `Username` are now get-only properties. It shouldn't break most people's code as it's not a source breaking change. If you used reflection to find these fields you should to look for properties now.

### InlineQueryResultBase

Type `InlineQueryResultBase` is renamed to `InlineQueryResult` to match Bot API type hierarchy.

### Nullability

From now on all properties that are optional will use nullable types, e.g. `int?`, `string?`, because default values of such properties might be an actual values and isn't distinguishable from a lack of value. From now if a property is `null` you can be sure that it's value was not present in a response from the Bot API.

[Telegram.Bot.Extensions.Polling]:https://github.com/TelegramBots/Telegram.Bot.Extensions.Polling

### ReplyKeyboardMarkup

Since `ResizeKeyboard` and `OneTimeKeyboard` are optional, we removed them from `ReplyKeyboardMarkup` constructor. You have to use [object initialization syntax](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/how-to-initialize-objects-by-using-an-object-initializer) to configure these properties:

```csharp
var replyKeyboardMarkup = new ReplyKeyboardMarkup(
    new KeyboardButton[][]
    {
        new KeyboardButton[] { "1.1", "1.2" },
        new KeyboardButton[] { "2.1", "2.2" },
    })
    {
        ResizeKeyboard = true
    };
```
