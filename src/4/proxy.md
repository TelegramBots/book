# Working Behind a Proxy

`TelegramBotClient` allows you to use a proxy for Bot API connections. This guide covers using three different proxy solutions.

- [HTTP Proxy](#http-proxy)
- [SOCKS5 Proxy](#socks5-proxy)
- [SOCKS5 Proxy over Tor (Testing Only)](#socks5-proxy-over-tor)

![Telegram Network](docs/tg-network.gif)

> If you are in a country, such as Iran, where HTTP and SOCKS proxy connections to Telegram servers are blocked, consider using a VPN, using Tor Network, or hosting your bot in other jurisdictions.

## HTTP Proxy

You can configure `HttpClient` with `WebProxy` and pass it to the Bot client.

```csharp
// using System.Net;
// using System.Net.Http;

var webProxy = new WebProxy(Host: "https://example.org", Port: 8080) {
    // Credentials if needed:
    Credentials = new NetworkCredential("USERNAME", "PASSWORD")
};
var httpClient = new HttpClient(
    new HttpClientHandler { Proxy = webProxy, UseProxy = true }
);

var botClient = new TelegramBotClient("YOUR_API_TOKEN", httpClient);
```

## SOCKS5 Proxy

Unfortunately, there is no built-in support for socks proxies in the .NET Standard libraries.
You can use an external NuGet package: [`HttpToSocks5Proxy`] provided by one of our team members.

```csharp
// using MihaZupan;

var proxy = new HttpToSocks5Proxy(Socks5ServerAddress, Socks5ServerPort);

// Or if you need credentials for your proxy server:
var proxy = new HttpToSocks5Proxy(
    Socks5ServerAddress, Socks5ServerPort, "USERNAME", "PASSWORD"
);

// Allows you to use proxies that are only allowing connections to Telegram
// Needed for some proxies
proxy.ResolveHostnamesLocally = true;

var httpClient = new HttpClient(
    new HttpClientHandler { Proxy = proxy, UseProxy = true }
);

botClient = new TelegramBotClient("YOUR_API_TOKEN", httpClient);
```

## SOCKS5 Proxy over Tor

**Warning: Use for Testing only!**

> Do not use this method in a production environment as it has high network latency and poor bandwidth.

Using Tor, a developer can avoid network restrictions while debugging and testing the code
before a production release.

1. Install [Tor Browser]
2. Open the `torcc` file with a text editor (Found in `Tor Browser\Browser\TorBrowser\Data\Tor`)
3. Add the following lines: (configurations are described below)

    ```text
    EntryNodes {NL}
    ExitNodes {NL}
    StrictNodes 1
    SocksPort 127.0.0.1:9050
    ```

4. Look at the [Socks5 proxy](#socks5-proxy) example above.
5. Start the Tor Browser

Usage:

```csharp
// using MihaZupan;

var proxy = new HttpToSocks5Proxy("127.0.0.1", 9050);

var httpClient = new HttpClient(
    new HttpClientHandler { Proxy = proxy, UseProxy = true }
);

botClient = new TelegramBotClient("YOUR_API_TOKEN", httpClient);
```

> Note that Tor has to be active at all times for the bot to work.

### Configurations in `torcc`

```text
EntryNodes {NL}
ExitNodes {NL}
StrictNodes 1
```

These three lines make sure you use nodes from the Netherlands as much as possible to reduce latency.

`SocksPort 127.0.0.1:9050`

This line tells tor to listen on port 9050 for any socks connections.
You can change the port to anything you want (9050 is just the default), only make sure to use the same port in your code.

[`HttpToSocks5Proxy`]: https://www.nuget.org/packages/HttpToSocks5Proxy/
[Tor Browser]: https://www.torproject.org/
