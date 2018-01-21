using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using RazorEngine;
using RazorEngine.Templating;
using Services.Interfaces;
using ViewModels;
using ViewModels.Order;
using Web.Validators;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : BaseFoodController
    {
        private readonly IOrderService _orderService;
        private readonly OrderValidator _validator;

        public OrdersController(IUserService userService, IOrderService orderService,OrderValidator validator) : base(userService)
        {
            _orderService = orderService;
            _validator = validator;
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
                var results = _validator.Validate(model);
                if (!results.IsValid) throw new Exception(results.Errors[0].ErrorMessage);
                var savedModel = _orderService.UpdateOrder(model);
                return Ok(savedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        private static string ParseOrderEmail(OrderViewModel model)
        {
            var templateFile = new FileInfo(LocalizationStrings.PathToOrderEmailTemplate);
            var templateText = System.IO.File.ReadAllText(templateFile.FullName);
            var renderedText = Engine.Razor.RunCompile(templateText, string.Empty, typeof(OrderViewModel), model);
            return renderedText;
        }

        [HttpPost("submit")]
        public IActionResult Submit([FromBody] OrderViewModel model)
        {
            try
            {
                var results = _validator.Validate(model);
                if (!results.IsValid) throw new Exception(results.Errors[0].ErrorMessage);
                var parsedOrderEmail = ParseOrderEmail(model);
                var user = GetCurrentUser();
                var savedModel = _orderService.SubmitOrder(model, user, parsedOrderEmail);
                return Ok(savedModel);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
