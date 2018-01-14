using System;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.UserLunch;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class UserLunchController : BaseFoodController
    {
        private readonly IUserLunchService _userLunchService;
        private readonly IUserService _userService;
        public UserLunchController(IUserService userService, IUserLunchService userLunchService) : base(userService)
        {
            _userLunchService = userLunchService;
            _userService = userService;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            var user = GetCurrentUser();
            var userLunchModel = _userLunchService.GetCurrentLunch(user.Id);
            if (userLunchModel == null) return new EmptyResult();
            userLunchModel.User = user;
            return Ok(userLunchModel);
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody] UserLunchViewModel model)
        {
            try
            {
                var user = _userService.UpdateUser(model.User);
                model.User = user;
                var refreshedModel = _userLunchService.UpdateUserLunch(model);
                return Ok(refreshedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }


    }
}
