# Long Polling

If you don't want to use our recommended **StartReceiving** helper ([see first example](../../1/example-bot.md)),
you can just call GetUpdateAsync with a timeout in a loop,
such that the call blocks for **up to** X seconds until there is an incoming update

Here is an example implementation:
```csharp
{{#include ../../../Examples/3/LongPolling.cs:long-polling}}
```