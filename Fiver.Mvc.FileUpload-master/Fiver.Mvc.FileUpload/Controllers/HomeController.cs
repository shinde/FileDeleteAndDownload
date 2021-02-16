using FilesDownloadAndDelete.Models.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace FilesDownloadAndDelete.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileProvider fileProvider; 
        public HomeController(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Files");
        }   
        public IActionResult Files()
        {
            var model = new FilesViewModel();
            foreach (var item in this.fileProvider.GetDirectoryContents(""))
            {
                model.Files.Add(
                    new FileDetails { Name = item.Name, Path = item.PhysicalPath });
            }            
            return View(model);
        }     
       

        [HttpPost]
        public async Task<FileResult> Download(List<string> files)
        {     
                     
            using (var memoryStream = new MemoryStream())
            {
                foreach (var item in files)
                {
                    var path = Path.Combine(
                               Directory.GetCurrentDirectory(),
                               "wwwroot", item);
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            ziparchive.CreateEntryFromFile(path, item);
                        }
                    }
                }
                return  File(memoryStream.ToArray(), "application/zip", "Attachments.zip");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(List<string> files)
        {          
            foreach(var item in files)
            {
                var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot", item.ToString());
                var fileInfo = new FileInfo(path);
                fileInfo.Delete();
            } 
            
            return RedirectToAction("Files");

        }      

    }
}
