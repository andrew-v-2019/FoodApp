using Converters;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {

        private readonly Services.IFileService _fileService;
        private readonly IMenuService _menuService;

        public FilesController(Services.IFileService fileService, IMenuService menuService)
        {
            _fileService = fileService;
            _menuService = menuService;
        }

        [HttpPost]
        public void Upload()
        {
            
            if (Request.Form.Files.Count <= 0) return;
            var file = Request.Form.Files[0];
            var fullPath = _fileService.SaveFile(file);
            var p = new DocParser();
            var viewModel= p.Parse(fullPath);
            viewModel = _menuService.UpdateMenu(viewModel);
        }

        

    }
}
