using Telegram.Bot;
using Telegram.Bot.Types;

namespace Examples.Chapter3;

internal class Files
{
    public readonly ITelegramBotClient bot = new TelegramBotClient("YOUR_BOT_TOKEN");
    public readonly ChatId chatId = 12345;
    public readonly Update update = new();

    private async Task GetFile()
    {
        if (update.Message is not { Photo: { } }) {
            return;
        }
// ANCHOR: get-file
var fileId = update.Message.Photo.Last().FileId;
var fileInfo = await bot.GetFile(fileId);
var filePath = fileInfo.FilePath;
// ANCHOR_END: get-file

    if (filePath is null)
    {
        return;
    }

    await DownloadFile();
    await GetInfoAndDownloadFile();

    async Task DownloadFile()
    {
// ANCHOR: download-file
const string destinationFilePath = "../downloaded.file";

await using Stream fileStream = File.Create(destinationFilePath);
await bot.DownloadFile(filePath, fileStream);
// ANCHOR_END: download-file
    }

    async Task GetInfoAndDownloadFile()
    {
// ANCHOR: get-and-download-file
const string destinationFilePath = "../downloaded.file";

await using Stream fileStream = File.Create(destinationFilePath);
var file = await bot.GetInfoAndDownloadFile(fileId, fileStream);
// ANCHOR_END: get-and-download-file
    }
    }

    private async Task UploadLocalFile()
    {
// ANCHOR: upload-local-file
await using Stream stream = File.OpenRead("../hamlet.pdf");
var message = await bot.SendDocument(chatId, document: InputFile.FromStream(stream, "hamlet.pdf"),
    caption: "The Tragedy of Hamlet,\nPrince of Denmark");
// ANCHOR_END: upload-local-file
    }

    private async Task UploadByFileId()
    {
        if (update.Message is not { Photo: { } })
        {
            return;
        }

// ANCHOR: upload-by-file_id
var fileId = update.Message.Photo.Last().FileId;
var message = await bot.SendPhoto(chatId, fileId);
// ANCHOR_END: upload-by-file_id
    }

    private async Task UploadByURL()
    {
// ANCHOR: upload-by-url
var message = await bot.SendPhoto(chatId, "https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg");
// ANCHOR_END: upload-by-url
    }
}
