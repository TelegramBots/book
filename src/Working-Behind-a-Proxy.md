Note that some countries (namely Iran) are blocking http and socks proxies from connecting to Telegram.
If you live in such a country, consider using a VPN, Tor or moving the hosting to another country/cloud.

## HTTP Proxy

The `TelegramBotClient` accepts an `IWebProxy` as a parameter in the constructor.

To use an HTTP proxy, the `WebProxy` class if what you will want to use.

```csharp
using System.Net;

WebProxy httpProxy = new WebProxy(address, port)
{
    // Credentials if needed
    Credentials = new NetworkCredential("username", "password")
};

TelegramBotClient Bot = new TelegramBotClient("API Token", httpProxy);
```

## Socks5 Proxy

There is no direct support for socks proxies in the .NET Standard.

You can use an external NuGet package: [`HttpToSocks5Proxy`].

```csharp
using MihaZupan;

var proxy = new HttpToSocks5Proxy(Socks5ServerAddress, Socks5ServerPort);

// Or if you need credentials for your proxy server:
var proxy = new HttpToSocks5Proxy(Socks5ServerAddress, Socks5ServerPort, "username", "password");

// Allows you to use proxies that are only allowing connections to Telegram
// Needed for some proxies
proxy.ResolveHostnamesLocally = true;

TelegramBotClient Bot = new TelegramBotClient("API Token", proxy);
```
Example taken from [`HttpToSocks5Proxy`'s readme].

## Socks5 proxy over Tor

**Note: Do not use this in production as it has increased latency and abysmal bandwidth.**

*Using this method, you can avoid restrictions in some countries.
It might be useful to developers for testing, before pushing new versions to hosting providers.*

1. Install [Tor Browser]
2. Open the `torcc` file with a text editor (Found in `Tor Browser\Browser\TorBrowser\Data\Tor`)
3. Add the following lines:
```
EntryNodes {NL}
ExitNodes {NL}
StrictNodes 1
SocksPort 127.0.0.1:9050
```
4. Look at the [Socks5 proxy](#socks5-proxy) example above.
5. Start the Tor Browser

Usage:
```csharp
using MihaZupan;
Bot = new TelegramBotClient("API Token", new HttpToSocks5Proxy("127.0.0.1", 9050));
```

*Note that Tor has to be active at all times for the bot to work*


##### What do those lines in `torcc` do?
```
EntryNodes {NL}
ExitNodes {NL}
StrictNodes 1
```
These three lines make sure you use nodes from the Netherlands as much as possible to reduce latency.

`SocksPort 127.0.0.1:9050`

This line tells tor to listen on port 9050 for any socks connections.
You can change the port to anything you want (9050 is just the default), just make sure to use the same port in your code.


[`HttpToSocks5Proxy`]: https://www.nuget.org/packages/HttpToSocks5Proxy/
[`HttpToSocks5Proxy`'s readme]: https://github.com/MihaZupan/HttpToSocks5Proxy/blob/master/README.md#usage-with-telegrambot-library
[Tor Browser]: https://www.torproject.org/