using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Examples.Chapter2;

internal class SendMessage
{
    public readonly ITelegramBotClient bot = new TelegramBotClient("YOUR_BOT_TOKEN");
    public readonly ChatId chatId = 12345;
    public readonly Update update = new ();

    private async Task SendTextMessage()
    {
// ANCHOR: text-message
await bot.SendMessage(chatId, "Hello, World!");
// ANCHOR_END: text-message
    }

    private async Task SendStickerMessage()
    {
// ANCHOR: sticker-message
await bot.SendSticker(chatId, "https://telegrambots.github.io/book/docs/sticker-dali.webp");
// ANCHOR_END: sticker-message
    }

    private async Task SendVideoMessage()
    {
// ANCHOR: video-message
await bot.SendVideo(chatId, "https://telegrambots.github.io/book/docs/video-hawk.mp4");
// ANCHOR_END: video-message
    }

    private async Task SendMediaGroup()
    {
// ANCHOR: send-media-group
var messages = await bot.SendMediaGroup(chatId, new IAlbumInputMedia[]
    {
        new InputMediaPhoto("https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg"),
        new InputMediaPhoto("https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg"),
    });
// ANCHOR_END: send-media-group
    }

    private async Task SendAudio()
    {
// ANCHOR: send-audio
var message = await bot.SendAudio(chatId, "https://telegrambots.github.io/book/docs/audio-guitar.mp3"
    //  , performer: "Joel Thomas Hunger", title: "Fun Guitar and Ukulele", duration: 91    // optional
    );
// ANCHOR_END: send-audio
    }

    private async Task SendVoice()
    {
// ANCHOR: send-voice
await using Stream stream = File.OpenRead("/path/to/voice-nfl_commentary.ogg");
var message = await bot.SendVoice(chatId, stream, duration: 36);
// ANCHOR_END: send-voice
    }

    private async Task SendDocument()
    {
// ANCHOR: send-document
await bot.SendDocument(chatId, "https://telegrambots.github.io/book/docs/photo-ara.jpg",
    "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>", ParseMode.Html);
// ANCHOR_END: send-document
    }

    private async Task SendAnimation()
    {
// ANCHOR: send-animation
await bot.SendAnimation(chatId, "https://telegrambots.github.io/book/docs/video-waves.mp4", "Waves");
// ANCHOR_END: send-animation
    }

    private async Task SendPoll()
    {
// ANCHOR: send-poll

var pollMessage = await bot.SendPoll("@channel_name",
    "Did you ever hear the tragedy of Darth Plagueis The Wise?",
    new InputPollOption[]
    {
        "Yes for the hundredth time!",
        "No, who`s that?"
    });
// ANCHOR_END: send-poll

// ANCHOR: stop-poll
Poll poll = await bot.StopPoll(pollMessage.Chat, pollMessage.Id);
// ANCHOR_END: stop-poll
    }

    private async Task SendContact()
    {
// ANCHOR: send-contact
await bot.SendContact(chatId, phoneNumber: "+1234567890", firstName: "Han", lastName: "Solo");
// ANCHOR_END: send-contact
    }

    private async Task SendvCard()
    {
// ANCHOR: send-vCard
await bot.SendContact(chatId, phoneNumber: "+1234567890", firstName: "Han",
    vcard: "BEGIN:VCARD\n" +
           "VERSION:3.0\n" +
           "N:Solo;Han\n" +
           "ORG:Scruffy-looking nerf herder\n" +
           "TEL;TYPE=voice,work,pref:+1234567890\n" +
           "EMAIL:hansolo@mfalcon.com\n" +
           "END:VCARD");
// ANCHOR_END: send-vCard
    }

    private async Task SendVenue()
    {
// ANCHOR: send-venue
await bot.SendVenue(chatId, latitude: 50.0840172f, longitude: 14.418288f,
    title: "Man Hanging out", address: "Husova, 110 00 Staré Město, Czechia");
// ANCHOR_END: send-venue
    }

    private async Task SendLocation()
    {
// ANCHOR: send-location
await bot.SendLocation(chatId, latitude: 33.747252f, longitude: -112.633853f);
// ANCHOR_END: send-location
    }

    private async Task SendPhoto()
    {
// ANCHOR: send-photo
var message = await bot.SendPhoto(chatId, "https://telegrambots.github.io/book/docs/photo-ara.jpg",
    "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>", ParseMode.Html);
// ANCHOR_END: send-photo
    }

    private async Task SendSticker()
    {
// ANCHOR: send-sticker
var message1 = await bot.SendSticker(chatId, "https://telegrambots.github.io/book/docs/sticker-fred.webp");

var message2 = await bot.SendSticker(chatId, message1.Sticker!.FileId);
// ANCHOR_END: send-sticker
    }

    private async Task SendText()
    {
    if (update.Message is null)
        return;

// ANCHOR: send-text
var message = await bot.SendMessage(chatId, "Trying <b>all the parameters</b> of <code>sendMessage</code> method",
    ParseMode.Html,
    protectContent: true,
    replyParameters: update.Message.Id,
    replyMarkup: new InlineKeyboardMarkup(
        InlineKeyboardButton.WithUrl("Check sendMessage method", "https://core.telegram.org/bots/api#sendmessage")));
// ANCHOR_END: send-text

    if (message is not {From: { },
        ReplyToMessage: { },
        Entities: { },
    })
    {
        return;
    }

// ANCHOR: message-contents
Console.WriteLine(
    $"{message.From.FirstName} sent message {message.Id} " +
    $"to chat {message.Chat.Id} at {message.Date}. " +
    $"It is a reply to message {message.ReplyToMessage.Id} " +
    $"and has {message.Entities.Length} message entities.");
// ANCHOR_END: message-contents
    }

    private async Task SendVideo()
    {
// ANCHOR: send-video
await bot.SendVideo(chatId, "https://telegrambots.github.io/book/docs/video-countdown.mp4",
    thumbnail: "https://telegrambots.github.io/book/2/docs/thumb-clock.jpg", supportsStreaming: true);
// ANCHOR_END: send-video
    }

    private async Task SendVideoNote()
    {
// ANCHOR: send-video-note
await using Stream stream = File.OpenRead("/path/to/video-waves.mp4");

await bot.SendVideoNote(chatId, stream,
    duration: 47, length: 360); // value of width/height
// ANCHOR_END: send-video-note
    }
}
