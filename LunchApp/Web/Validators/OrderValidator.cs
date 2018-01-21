using FluentValidation;
using Services.Interfaces;
using ViewModels;
using ViewModels.Order;

namespace Web.Validators
{
    public class OrderValidator : AbstractValidator<OrderViewModel>
    {
        private readonly IMenuService _menuService;
        private readonly IOrderService _orderService;

        public OrderValidator(IMenuService menuService, IOrderService orderService)
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;
            _menuService = menuService;
            _orderService = orderService;
            RuleFor(x => x).Must(BeForActiveMenu).WithMessage(LocalizationStrings.MenuIsLocked);
            RuleFor(x => x).Must(OrderHasntBeenSubmitted).WithMessage(LocalizationStrings.MenuIsLocked);
        }

        private bool BeForActiveMenu(OrderViewModel model)
        {
            var r = _menuService.MenuIsActive(model.MenuId);
            return r;
        }

        private bool OrderHasntBeenSubmitted(OrderViewModel model)
        {
            var r = _orderService.IsOrderSubmitted(model.OrderId);
            return !r;
        }
    }
}
