using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Services
{
    public class FileService: IFileService
    {
        private readonly IHostingEnvironment _hostingEnv;

        public FileService(IHostingEnvironment env)
        {
            _hostingEnv = env;
        }

        public string SaveFile(IFormFile file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fullPath = _hostingEnv.WebRootPath + $@"\1\{fileName}";
            using (var fs = File.Create(fullPath))
            {
                file.CopyTo(fs);
                fs.Flush();
                fs.Dispose();
            }
            return fullPath;
        }
    }
}
