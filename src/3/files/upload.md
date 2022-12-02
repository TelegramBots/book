# Uploading files

First, read the [documentation on sending files](https://core.telegram.org/bots/api#sending-files).

## Upload local file

To upload local file open stream and call one of the file-sending functions:

```C#
{{#include ../../../Examples/3/Files.cs:upload-local-file}}
```

Be aware of limitation for this method - 10 MB max size for photos, 50 MB for other files.

## Upload file by file identifier

If the file is already stored somewhere on the Telegram servers, you don't need to reupload it: each file object has a `FileId` property. Simply pass this `FileId` as a parameter instead of uploading. There are no limits for files sent this way.

```C#
{{#include ../../../Examples/3/Files.cs:upload-by-file_id}}
```

## Upload by URL

Provide Telegram with an HTTP URL for the file to be sent. Telegram will download and send the file. 5 MB max size for photos and 20 MB max for other types of content.

```csharp
{{#include ../../../Examples/3/Files.cs:upload-by-url}}
```
