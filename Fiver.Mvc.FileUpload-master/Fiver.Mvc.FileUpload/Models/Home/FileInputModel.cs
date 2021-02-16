using Microsoft.AspNetCore.Http;

namespace FilesDownloadAndDelete.Models.Home
{
    public class FileInputModel
    {
        public IFormFile FileToUpload { get; set; }
    }
}
