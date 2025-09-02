
# Migration guide for version 22.x

If you're migrating from version 19.x, you might want to read our [migration doc for v21](Version-21.x.md) first.
There were lots of interesting changes in versions v21.x.

We are back on [Nuget.org](https://www.nuget.org/packages/Telegram.Bot/#versions-body-tab)!  
_And our old nuget feed nuget.voids.site is no more, so please remove this line from your nuget.config file, or from your IDE settings (Package sources)_

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

## What's new in version 22.0

- Support for [Bot API 7.11](https://core.telegram.org/bots/api#october-31-2024)
- Implicit conversions for single-field structures  
	For example classes containing just one string (like BotDescription, WebAppInfo or CopyTextButton) can be handled just as a string
- Implicit conversion from FileBase classes to InputFile  
	This means you can pass an object like a `Video` or `PhotoSize` directly as argument
	to a Send method and it will use its FileId
- Helper method `GetFileIdType`  
	It can tell you which type of object/media is referenced by a FileId string
- Huge rewrite of our serialization code to make it more performant and straightforward.
- Updated System.Text.Json due to vulnerability [CVE-2024-43485](https://github.com/advisories/GHSA-8g4q-xg66-9fp4)

## What's new in version 22.1

- Support for [Bot API 8.0](https://core.telegram.org/bots/api-changelog#november-17-2024)
- new helper `message.MessageLink()` to get the <u>t.me</u> link to that message (Supergroup and Channel only)
- `ToHtml`/`ToMarkdown`: support for ExpandableBlockquote _(v22.1.1)_
- fix savePreparedInlineMessage request _(v22.1.2)_
- `TransactionPartnerUser.Gift` type was corrected in Bot API _(v22.1.3)_

## What's new in version 22.2

- Support for [Bot API 8.1](https://core.telegram.org/bots/api-changelog#december-4-2024)
- Support for [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot) / [Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-build-tools-and-aot) / [Trimming](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained)  
  _(this is still experimental and we would enjoy your feedback if you try to use the library in such contexts)_

## What's new in version 22.3

- Support for [Bot API 8.2](https://core.telegram.org/bots/api#january-1-2025)
- Renamed class `Telegram.Bot.Types.File` as `TGFile`  
  _(the name `File` was annoyingly conflicting with the often-used `System.IO.File` class)_
- Added property `Token` to `TelegramBotClient`  
  _(you can use it to `ParseValidateData` MiniApp requests, authenticate WebHook updates, ...)_

## What's new in version 22.4
- Support for [Bot API 8.3](https://core.telegram.org/bots/api#february-12-2025)
- Added helpers `ChatMember.IsInChat` and `ChatMember.IsAdmin`
- Added constructor `ChatPermissions(bool)` to set all fields to **true** or **false**
- Fix a potential issue in `.ToHtml()` escaping `&` in URLs
- Renamed helper classes `Emoji` to `DiceEmoji` and `KnownReactionTypeEmoji` to `ReactionEmoji`
- Changed `IReplyMarkup` to abstract class `ReplyMarkup` so you can directly pass [text, button or arrays of buttons](../3/inline.html#reply-markup) for the `replyMarkup:` parameter
- `BotCommand` can also now be implicitly constructed from a `(string command, string description)` tuple or new explicit constructor _(v22.4.3)_  
  Example: `await Bot.SetMyCommands([("/start", "Start the bot"), ("/privacy", "Privacy policy")]);`
- Added helper `DownloadFile(TGFile file, ...)` _(v22.4.4)_

## What's new in version 22.5
- Support for [Bot API 9.0](https://core.telegram.org/bots/api#april-11-2025)
- Removed the `*Async`-suffixed versions of the methods that were marked [Obsolete] since v22.0.  
  → Follow the steps at the top of this page to adapt your existing code if you haven't already.
- `Services.ConfigureTelegramBot*` is **no longer necessary** to make your Webhook bot compatible with Telegram updates!  
  → We've made the library structures compatible with the default JsonCamelCaseNamingPolicy used by ASP.NET Core projects.  
  In some rare cases _(like [Native AOT](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot) / [Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/webassembly-build-tools-and-aot) / [Trimming](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-self-contained))_, this call might still be necessary.
- Removed dependency on ASP.NET Core (Microsoft.AspNetCore.App) from the main library, by moving the `ConfigureTelegramBotMvc()` method to a separate package [Telegram.Bot.AspNetCore](https://github.com/TelegramBots/Telegram.Bot.AspNetCore)  
  → That dependency caused some issues for users deploying to Docker / Android / WebAssembly  
  → As explained above, you shouldn't need this method anymore in most cases, but if you do, you can add the [nuget package Telegram.Bot.AspNetCore](https://www.nuget.org/packages/Telegram.Bot.AspNetCore) to your project
- Merged the Telegram.Bot.Extensions.Passport package into the main library  
  → It was old and unmaintained, but now its methods and structures supporting [Telegram Passport](https://core.telegram.org/passport#personaldetails) are up-to-date and available directly in Telegram.Bot
- New `HtmlText` helpers method: `ToPlain`, `PlainLength` and `Truncate` to get
  the plain text only from HTML text _(remove the &lt;tags&gt;)_, the number of plain text characters, or truncate the HTML to a number of plain text characters _(useful to comply with the 4096/1024 message limit)_
- Added constructor `ChatAdministratorRights(bool)` to set all fields to **true** or **false** (except IsAnonymous)
- Added helper property `message.IsServiceMessage` to detect service messages vs content messages
- Added helper property `ChatMemberRestricted.IsMuted` to detect if restriction is muting the user _(v22.5.1)_

Builds & Releases:
- Dropped Azure DevOps builds in favor of GitHub Actions  
  → Our [alternative nuget feed](https://dev.azure.com/tgbots/Telegram.Bot/_artifacts/feed/release) will no longer be updated since we are [back on Nuget.org](https://www.nuget.org/packages/Telegram.Bot/#versions-body-tab)
- "Prerelease" versions are now pushed to Nuget.org as the library is developed, for people who want to test upcoming features early  
  → You can provide feedback on these versions and help us detect problems before the next stable release
- Now including **Release Notes** as part of Nuget packages  
  → You can see them on [nuget.org package page](https://www.nuget.org/packages/Telegram.Bot/#releasenotes-body-tab), or in your IDE's Package Manager, on the Package Detail pane where you select a version.

2 small breaking changes:
- Merged `AnswerShippingQuery` method overloads. You'll need to add `errorMessage:` in your code to pass that argument and indicate failure.
- The `Poll.Type` is now an enum `PollType` instead of `string`

## What's new in version 22.6
- Support for [Bot API 9.1](https://core.telegram.org/bots/api#july-3-2025)
- `ReplyParameters(Message)` supports null
- Fix `MessageLink()` for non-topics group
- `TransferGift`: fix type for newOwnerChatId
- Sealed some internal types for performance
- faster `IsServiceMessage`

## What's new in version 22.7
- Support for [Bot API 9.2](https://core.telegram.org/bots/api-changelog#august-15-2025)
- Added `ChatMember.ExpireDate` helper (= generic `UntilDate`)
- implicit `InputFile(string)` `InputFileId(string)` now allows `null` string (and return null)
- **New all-in-one helper method `SendHtml`** (which supports `<img> <video> <file> <preview> <keyboard>` tags)  
👉 see [SendHtml documentation](../2/send-msg/send-html.md) for more details
