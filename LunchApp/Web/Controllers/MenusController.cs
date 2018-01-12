using System;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.Menu;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class MenusController : Controller
    {

        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("empty")]
        public IActionResult GetEmpty()
        {
            var model = _menuService.GetEmptyMenu();
            //var model = _menuService.GetFakeMenu();
            return Ok(model);
        }


        [HttpGet("last")]
        public IActionResult GetLast()
        {
            try
            {
                var model = _menuService.GetLastMenu();
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
                var savedModel = _menuService.UpdateMenu(model);
                return Ok(savedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }


    }
}
