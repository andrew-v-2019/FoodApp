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
        private readonly IOrderService _orderService;
        public UserLunchController(IUserService userService, IUserLunchService userLunchService, IOrderService orderService) : base(userService)
        {
            _userLunchService = userLunchService;
            _userService = userService;
            _orderService = orderService;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            try
            {
                var user = GetCurrentUser();
                var userLunchModel = _userLunchService.GetCurrentLunch(user.Id);
                userLunchModel.User = user;
                return Ok(userLunchModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody] UserLunchViewModel model)
        {
            try
            {
                var user = _userService.UpdateUser(model.User);
                model.User = user;
                var refreshedModel = _userLunchService.UpdateUserLunch(model);
                _orderService.AddUserLunchToOrder(refreshedModel);
                return Ok(refreshedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }


    }
}
