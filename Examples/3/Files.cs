using Telegram.Bot;
using Telegram.Bot.Types;

namespace Examples.Chapter3;

internal class Files
{
    private readonly ITelegramBotClient botClient = new TelegramBotClient("{YOUR_ACCESS_TOKEN_HERE}");
    private readonly CancellationToken cancellationToken = new().Token;
    private readonly ChatId chatId = 12345;
    private readonly Update update = new();

    private async Task GetFile()
    {
        if (update.Message is not { Photo: { } }) {
            return;
        }
// ANCHOR: get-file
var fileId = update.Message.Photo.Last().FileId;
var fileInfo = await botClient.GetFileAsync(fileId);
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

await using Stream fileStream = System.IO.File.OpenWrite(destinationFilePath);
await botClient.DownloadFileAsync(
    filePath: filePath,
    destination: fileStream,
    cancellationToken: cancellationToken);
// ANCHOR_END: download-file
    }

    async Task GetInfoAndDownloadFile()
    {
// ANCHOR: get-and-download-file
const string destinationFilePath = "../downloaded.file";

await using Stream fileStream = System.IO.File.OpenWrite(destinationFilePath);
var file = await botClient.GetInfoAndDownloadFileAsync(
    fileId: fileId,
    destination: fileStream,
    cancellationToken: cancellationToken);
// ANCHOR_END: get-and-download-file
    }
    }

    private async Task UploadLocalFile()
    {
// ANCHOR: upload-local-file
await using Stream stream = System.IO.File.OpenRead("../hamlet.pdf");
Message message = await botClient.SendDocumentAsync(
    chatId: chatId,
    document: InputFile.FromStream(stream: stream, fileName: "hamlet.pdf"),
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
Message message = await botClient.SendPhotoAsync(
    chatId: chatId,
    photo: InputFile.FromFileId(fileId));
// ANCHOR_END: upload-by-file_id
    }

    private async Task UploadByURL()
    {
// ANCHOR: upload-by-url
Message message = await botClient.SendPhotoAsync(
    chatId: chatId,
    photo: InputFile.FromUri("https://cdn.pixabay.com/photo/2017/04/11/21/34/giraffe-2222908_640.jpg"));
// ANCHOR_END: upload-by-url
    }
}
