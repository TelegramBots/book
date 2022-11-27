using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;

// ANCHOR: usings
// using System.Net;
// using System.Net.Http;
// ANCHOR_END: usings

namespace Examples.Chapter2;

internal class Proxy
{
    private async Task ClientWithHttpProxy()
    {
// ANCHOR: http-proxy-client
WebProxy webProxy = new (Host: "https://example.org", Port: 8080)
{
    // Credentials if needed:
    Credentials = new NetworkCredential("USERNAME", "PASSWORD")
};
HttpClient httpClient = new (
    new HttpClientHandler { Proxy = webProxy, UseProxy = true, }
);

var botClient = new TelegramBotClient("YOUR_API_TOKEN", httpClient);
// ANCHOR_END: http-proxy-client
    }

    private async Task ClientWithSocksProxy()
    {
// ANCHOR: socks-proxy-client
WebProxy proxy = new ("socks5://127.0.0.1:9050")
{
    Credentials = new NetworkCredential("USERNAME", "PASSWORD")
};
HttpClient httpClient = new (
    new SocketsHttpHandler { Proxy = proxy, UseProxy = true, }
);

var botClient = new TelegramBotClient("YOUR_API_TOKEN", httpClient);
// ANCHOR_END: socks-proxy-client
    }

    private async Task TorProxy()
    {
// ANCHOR: tor-proxy-client
WebProxy proxy = new ("socks5://127.0.0.1:9050");

HttpClient httpClient = new (
    new SocketsHttpHandler { Proxy = proxy, UseProxy = true }
);

var botClient = new TelegramBotClient("YOUR_API_TOKEN", httpClient);
// ANCHOR_END: tor-proxy-client
    }
}
