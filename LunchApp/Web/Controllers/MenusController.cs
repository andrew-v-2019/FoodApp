using System;
using System.IO;
using Converters;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels;
using ViewModels.Menu;
using Web.Validators;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class MenusController : BaseFoodController
    {

        private readonly IMenuService _menuService;
        private readonly MenuValidator _validator;

        public MenusController(IMenuService menuService, IUserService userService, MenuValidator validator) : base(userService)
        {
            _menuService = menuService;
            _validator = validator;
        }

        [HttpGet("last")]
        public IActionResult GetLast()
        {
            try
            {
                var model = _menuService.GetActiveMenuForEdit();
                return Ok(model);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("template")]
        public IActionResult GetTemplate()
        {
            var model = _menuService.GetLastMenuAsTemplate();
            return Ok(model);
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody] UpdateMenuViewModel model)
        {
            try
            {
                var results = _validator.Validate(model);
                if (!results.IsValid) throw new Exception(results.Errors[0].ErrorMessage);
                var savedModel = _menuService.UpdateMenu(model);
                return Ok(savedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("saveToDoc")]
        public IActionResult SaveToDoc([FromBody] UpdateMenuViewModel model)
        {
            try
            {
                var templateFile = new FileInfo(LocalizationStrings.PathToDocTemplate);
                var converter = new DocConverter();
                var str = converter.ConvertToDoc(model, templateFile);
                var result = new FileContentResult(System.IO.File.ReadAllBytes(str), "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    FileDownloadName = "myFile.docx"
                };
                return result;
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
