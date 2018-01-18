using System;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.Order;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : BaseFoodController
    {
        private readonly IOrderService _orderService;
        private readonly IConfigurationsProvider _configurationsProvider;

        public OrdersController(IUserService userService, IOrderService orderService, IConfigurationsProvider configurationsProvider) : base(userService)
        {
            _orderService = orderService;
            _configurationsProvider = configurationsProvider;
        }


        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var model = _orderService.GetCurrentOrder();
                return Ok(model);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("")]
        public IActionResult Update([FromBody] OrderViewModel model)
        {
            try
            {
                var savedModel = _orderService.UpdateOrder(model);
                return Ok(savedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost("submit")]
        public IActionResult Submit([FromBody] OrderViewModel model)
        {
            try
            {
                var user = GetCurrentUser();
                var savedModel = _orderService.SubmitOrder(model, user);
                return Ok(savedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
