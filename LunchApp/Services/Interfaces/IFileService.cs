using Microsoft.AspNetCore.Http;

namespace Services
{
    public interface IFileService
    {
        string SaveFile(IFormFile file);
    }
}
