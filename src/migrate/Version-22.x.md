# Migration guide for version 22.x

If you're migrating from version 19.x, you might want to read our [migration doc for v21](Version-21.x.md) first.
There were a lot of interesting changes in versions v21.x.

## ⚠️ Breaking changes

We removed the `Async` suffix from our API method names, and renamed `SendTextMessageAsync` to `SendMessage`.  
This was done to match [official API documentation](https://core.telegram.org/bots/api#available-methods) and because all our TelegramBotClient methods are asynchronous _(no real need to differenciate between async or non-async methods)_

We also reordered a few optional parameters to move away the lesser used arguments later down the argument list (typically: `message_thread_id`/`entities`)
and move the more useful arguments up closer to the beginning (typically: `replyParameters`/`replyMarkup`)  
This should make your life simpler as you won't need to resort to [named arguments](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/named-and-optional-arguments#named-arguments) for the most commonly used method parameters.

Finally, we renamed property `Message.MessageId` as just `Message.Id`. _(but MessageId will remain supported)_

> [!NOTE]  
> **All the previous names are still supported at the moment**, so your code should just run fine with version 22.0, except you will get warnings about "Obsolete" code while compiling.

## 📝 How to adapt your code for these changes

In order to port your code easily and get rid of compiler warnings, I suggest you use these 4 **Find and Replace** operations in your IDE

So start by opening the Edit > **Find and Replace** > **Replace in Files** panel (Ctrl+Shift+H)  
Untick the checkboxes: **Match case** and **Match whole word**  
Tick the checkbox: **✓ Use regular expressions**  
Select the scope to look in: **Current project** or **Entire solution**

(In the following, we suppose your TelegramBotClient variable is named `bot`, please modify the expressions if necessary)

1. To remove explicit **null** arguments for messageThreadId/entities  
_Replace:_ `(bot\.Send\w+Async\b.*,) null,`  
 &nbsp;&nbsp;&nbsp;&nbsp;_With:_ `$1`  
🖱️Click on **Replace All**  
🖱️Click on **Replace All** again

2. To rename **SendTextMessageAsync** with **SendMessage**  
_Replace:_ `\.SendTextMessageAsync\b`  
 &nbsp;&nbsp;&nbsp;&nbsp;_With:_ `.SendMessage`  
🖱️Click on **Replace All**

3. To remove the **Async** suffixes:  
_Replace:_ `(bot\.\w+)Async\b`  
 &nbsp;&nbsp;&nbsp;&nbsp;_With:_ `$1`  
🖱️Click on **Replace All**

4. To rename the **MessageId** property:  
_Replace:_ `(message)\.MessageId\b`  
 &nbsp;&nbsp;&nbsp;&nbsp;_With:_ `$1.Id`  
🖱️Click on **Replace All**  
_(Depending on your variable naming convention, you might want to repeat that, replacing `(message)` with `(msg)` or something else)_

The remaining effort to make your code compile should now be much reduced.  
_(maybe a few more parameter reordering if you didn't use named parameters)_

**Addendum:** we also renamed method `MakeRequest`Async to `SendRequest`
_(if you use this [non-recommended](Version-21.x.md#request-structures) method)_

## What's new in version 22.0.0

- Support for [Bot API 7.11](https://core.telegram.org/bots/api#october-31-2024)
- Implicit conversions for single-field structures  
	For example classes containing just one string (like BotDescription, WebAppInfo or CopyTextButton) can be handled just as a string
- Implicit conversion from FileBase classes to InputFile  
	This means you can pass an object like a `Video` or `PhotoSize` directly as argument
	to a Send method and it will use its FileId
- Helper method `GetFileIdType`  
	It can tell you which type of object/media is referenced by a FileId string
- Huge rewrite of our serialization code to make it more performant and straightforward.
