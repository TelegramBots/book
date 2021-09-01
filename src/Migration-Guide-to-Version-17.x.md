# Migration guide for version 17.0

There are several major breaking changes in the v17:

- New exceptions handling
- Removal of API methods from `ITelegramBotClient` interface and moving them into extension methods in the same namespace (that shouldn't break anyones sources as long as they don't employ reflection or make their own interface implementations)
- Working with default enum values

These are the most user facing breaking changes you should be aware of during migration.

Let's dive deep on the migrations.

## 1. New exceptions handling

v17 brings a new base type for exceptions: `RequestException`. `ApiRequestException` which inherits from `RequestException` is thrown only in situations when an actual error response with the correct body is received from the Bot API. In other situations `RequestException` will be thrown instead containing actual exception as `InnerException` if there is one, e.g. serialization exceptions or connection exceptions.

If you used `ApiRequestException` and `HttpRequestException` to handle most exception now you need to replace `HttpRequestException` with `RequestException` and look for inner exception. All valid errors with JSON body from Telegram are now thrown as `ApiRequestException` including `429 Too Many Request`.

Since `5XX` exceptions don't usually include correct JSON body they are thrown as `RequestException` with `HttpRequestException` inside.

Look at the following exception for how to handle different kind of exceptions. You don't necesserely need to impement everything as you see, it's there only for an example.

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
            // Handle authorizarion exceptions (blocked users, unaccessible chats, etc)
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
    else if (exception.InnerException is JsonSerializationException serializarionException)
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

## 2. Removal of API methods from `ITelegramBotClient` interface

This change shouldn't affect most users, the methods are still there, but instead of being implementations of the interface they are now extension methods. It makes the interface leaner and easier to implement for custom clients and for decorators (e.g. rate limiters implemented as decorators). There isn't really a migration path for those who used these for some reason.

## 3. Working with default enum values

We changed how we work with enums. The most notable change is the default value: there is none, all our enums are now start with 1 (exception `UpdateType` and `MessageType` since they are not a part of the Bot API and we fully control these). 0  value is left unassinged for a purpose: if we encounter an unknown value in the response from the Bot API we assign 0 as the value.

So e.g. imaging if tommorrow Telegram adds another `MessageEntity` value. From now on all unknown values can be handled as in the `default` arm of `switch` statement.

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

Also some default enums values were removed, e.g. `ParseMode.Default` since we started using nullable types for every optional value and `ParseMode.Default` lost it's usefulness. If a message doesn't have any markup you'll receive `null` in places where `ParseMode` type was used or if you want to explicitly indicate an absence of markup pass `null` instead.

## 4. Other breaking changes

### InputMediaType

Property `Type` in `IInputMedia` was changed to an enum `InputMediaType` for easier discoverability. So if you relied string values like `photo`, `video`, `animation` and so on now you need to switch to using enums. As a result you'll get autocomplete in IDEs and more predictability of what types of input media there are.

### EncryptedPassportElementType

Property `Type` of `EncryptedPassportElement` was replace with an enum for the same reason with `EncryptedPassportElementType` enum

### ChatMember

As part of Bot API 5.3 implementation `ChatMember` type was split into a hierarchy of types with a discriminator field `Status`. If you need to access some data from the inheritors use pattern mathing or type casting, like so:

```csharp
ChatMember member = ... //;

if (chatMember is ChatMemberKicked kickedMember)
{
    // now you can access propertoes of a kicked chat member
    if (kickedMember.Until is not null)
    {
        // do something with the value of Until
    }
}
```

### ChatId

Fields `Identifier` and `Username` was made get-only properties. It shouldn't break most people's code as it's not a source breaking change. If you used reflection to find these fields you need to look for properties instead now.

### InlineQueryResultBase

Type `InlineQueryResultBase` was renamed to `InlineQueryResult` to be more close to the Bot API and so be more easily discoverable.

### Nullability

From now on all properties that are optional will use nullable types, e.g. `int?`, `string?`, because default values of such properties might be an actual values and isn't distinguishable from a lack of value. From now if a property is `null` you can be sure that it's value was not present in a response from the Bot API.
