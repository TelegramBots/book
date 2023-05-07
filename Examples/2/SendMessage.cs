using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Examples.Chapter2;

internal class SendMessage
{
    private readonly ITelegramBotClient botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");
    private readonly CancellationToken cancellationToken = new CancellationTokenSource().Token;
    private readonly ChatId chatId = 12345;
    private readonly Update update = new ();

    private async Task SendTextMessage()
    {
// ANCHOR: text-message
Message message = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Hello, World!",
    cancellationToken: cancellationToken);
// ANCHOR_END: text-message
    }

    private async Task SendStickerMessage()
    {
// ANCHOR: sticker-message
Message message = await botClient.SendStickerAsync(
    chatId: chatId,
    sticker: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/sticker-dali.webp"),
    cancellationToken: cancellationToken);
// ANCHOR_END: sticker-message
    }

    private async Task SendVideoMessage()
    {
// ANCHOR: video-message
Message message = await botClient.SendVideoAsync(
    chatId: chatId,
    video: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/video-bulb.mp4"),
    cancellationToken: cancellationToken);
// ANCHOR_END: video-message
    }

    private async Task SendMediaGroup()
    {
// ANCHOR: send-media-group
Message[] messages = await botClient.SendMediaGroupAsync(
    chatId: chatId,
    media: new IAlbumInputMedia[]
    {
        new InputMediaPhoto(
            InputFile.FromUri("https://cdn.pixabay.com/photo/2017/06/20/19/22/fuchs-2424369_640.jpg")),
        new InputMediaPhoto(
            InputFile.FromUri("https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg")),
    },
    cancellationToken: cancellationToken);
// ANCHOR_END: send-media-group
    }

    private async Task SendAudio()
    {
// ANCHOR: send-audio
Message message = await botClient.SendAudioAsync(
    chatId: chatId,
    audio: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/audio-guitar.mp3"),
    /*
    performer: "Joel Thomas Hunger",
    title: "Fun Guitar and Ukulele",
    duration: 91, // in seconds
    */
    cancellationToken: cancellationToken);
// ANCHOR_END: send-audio
    }

    private async Task SendVoice()
    {
// ANCHOR: send-voice
await using Stream stream = System.IO.File.OpenRead("/path/to/voice-nfl_commentary.ogg");
Message message = await botClient.SendVoiceAsync(
    chatId: chatId,
    voice: InputFile.FromStream(stream),
    duration: 36,
    cancellationToken: cancellationToken);
// ANCHOR_END: send-voice
    }

    private async Task SendDocument()
    {
// ANCHOR: send-document
Message message = await botClient.SendDocumentAsync(
    chatId: chatId,
    document: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg"),
    caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
    parseMode: ParseMode.Html,
    cancellationToken: cancellationToken);
// ANCHOR_END: send-document
    }

    private async Task SendAnimation()
    {
// ANCHOR: send-animation
Message message = await botClient.SendAnimationAsync(
    chatId: chatId,
    animation: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/video-waves.mp4"),
    caption: "Waves",
    cancellationToken: cancellationToken);
// ANCHOR_END: send-animation
    }

    private async Task SendPoll()
    {
// ANCHOR: send-poll
Message pollMessage = await botClient.SendPollAsync(
    chatId: "@channel_name",
    question: "Did you ever hear the tragedy of Darth Plagueis The Wise?",
    options: new[]
    {
        "Yes for the hundredth time!",
        "No, who`s that?"
    },
    cancellationToken: cancellationToken);
// ANCHOR_END: send-poll

// ANCHOR: stop-poll
Poll poll = await botClient.StopPollAsync(
    chatId: pollMessage.Chat.Id,
    messageId: pollMessage.MessageId,
    cancellationToken: cancellationToken);
// ANCHOR_END: stop-poll
    }

    private async Task SendContact()
    {
// ANCHOR: send-contact
Message message = await botClient.SendContactAsync(
    chatId: chatId,
    phoneNumber: "+1234567890",
    firstName: "Han",
    lastName: "Solo",
    cancellationToken: cancellationToken);
// ANCHOR_END: send-contact
    }

    private async Task SendvCard()
    {
// ANCHOR: send-vCard
Message message = await botClient.SendContactAsync(
    chatId: chatId,
    phoneNumber: "+1234567890",
    firstName: "Han",
    vCard: "BEGIN:VCARD\n" +
           "VERSION:3.0\n" +
           "N:Solo;Han\n" +
           "ORG:Scruffy-looking nerf herder\n" +
           "TEL;TYPE=voice,work,pref:+1234567890\n" +
           "EMAIL:hansolo@mfalcon.com\n" +
           "END:VCARD",
    cancellationToken: cancellationToken);
// ANCHOR_END: send-vCard
    }

    private async Task SendVenue()
    {
// ANCHOR: send-venue
Message message = await botClient.SendVenueAsync(
    chatId: chatId,
    latitude: 50.0840172f,
    longitude: 14.418288f,
    title: "Man Hanging out",
    address: "Husova, 110 00 Staré Město, Czechia",
    cancellationToken: cancellationToken);
// ANCHOR_END: send-venue
    }

    private async Task SendLocation()
    {
// ANCHOR: send-location
Message message = await botClient.SendLocationAsync(
    chatId: chatId,
    latitude: 33.747252f,
    longitude: -112.633853f,
    cancellationToken: cancellationToken);
// ANCHOR_END: send-location
    }

    private async Task SendPhoto()
    {
// ANCHOR: send-photo
Message message = await botClient.SendPhotoAsync(
    chatId: chatId,
    photo: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg"),
    caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
    parseMode: ParseMode.Html,
    cancellationToken: cancellationToken);
// ANCHOR_END: send-photo
    }

    private async Task SendSticker()
    {
// ANCHOR: send-sticker
Message message1 = await botClient.SendStickerAsync(
    chatId: chatId,
    sticker: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/sticker-fred.webp"),
    cancellationToken: cancellationToken);

Message message2 = await botClient.SendStickerAsync(
    chatId: chatId,
    sticker: InputFile.FromFileId(message1.Sticker!.FileId),
    cancellationToken: cancellationToken);
// ANCHOR_END: send-sticker
    }

    private async Task SendText()
    {
    if (update.Message is null)
        return;

// ANCHOR: send-text
Message message = await botClient.SendTextMessageAsync(
    chatId: chatId,
    text: "Trying *all the parameters* of `sendMessage` method",
    parseMode: ParseMode.MarkdownV2,
    disableNotification: true,
    replyToMessageId: update.Message.MessageId,
    replyMarkup: new InlineKeyboardMarkup(
        InlineKeyboardButton.WithUrl(
            text: "Check sendMessage method",
            url: "https://core.telegram.org/bots/api#sendmessage")),
    cancellationToken: cancellationToken);
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
    $"{message.From.FirstName} sent message {message.MessageId} " +
    $"to chat {message.Chat.Id} at {message.Date}. " +
    $"It is a reply to message {message.ReplyToMessage.MessageId} " +
    $"and has {message.Entities.Length} message entities.");
// ANCHOR_END: message-contents
    }

    private async Task SendVideo()
    {
// ANCHOR: send-video
Message message = await botClient.SendVideoAsync(
    chatId: chatId,
    video: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/docs/video-countdown.mp4"),
    thumbnail: InputFile.FromUri("https://raw.githubusercontent.com/TelegramBots/book/master/src/2/docs/thumb-clock.jpg"),
    supportsStreaming: true,
    cancellationToken: cancellationToken);
// ANCHOR_END: send-video
    }

    private async Task SendVideoNote()
    {
// ANCHOR: send-video-note
await using Stream stream = System.IO.File.OpenRead("/path/to/video-waves.mp4");

Message message = await botClient.SendVideoNoteAsync(
    chatId: chatId,
    videoNote: InputFile.FromStream(stream),
    duration: 47,
    length: 360, // value of width/height
    cancellationToken: cancellationToken);
// ANCHOR_END: send-video-note
    }
}
