using Data.Models;
using Services.Interfaces;
using System;
using System.Linq;
using ViewModels;
using ViewModels.Order;
using ViewModels.User;
using ViewModels.UserLunch;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly Context _context;
        private readonly IMenuService _menuService;
        private readonly IUserLunchService _userLunchService;
        private readonly IEmailService _emailService;

        public OrderService(Context context, IMenuService menuService, IUserLunchService userLunchService, IEmailService emailService)
        {
            _context = context;
            _menuService = menuService;
            _userLunchService = userLunchService;
            _emailService = emailService;
        }

        private Order GetOrCreateOrder(int menuId)
        {
            var any = _context.Orders.FirstOrDefault(o => o.MenuId == menuId);
            if (any != null) return any;
            {
                var menu = _context.Menus.FirstOrDefault(o => o.MenuId == menuId);
                if (menu == null) throw new Exception(LocalizationStrings.ActiveMenuDoesntNotExist);
                any = new Order()
                {
                    CreationDate = DateTime.Now,
                    MenuId = menuId,
                    Submitted = false,
                    SubmitionDate = null,
                    OrderName = menu.Name
                };
                _context.Orders.Add(any);
                _context.SaveChanges();
            }
            return any;
        }

        public void AddUserLunchToOrder(UserLunchViewModel userLunch)
        {
            var order = GetOrCreateOrder(userLunch.MenuId);
            var userLunchOrder = _context.OrderUserLunches.FirstOrDefault(u => u.UserLunchId == userLunch.UserLunchId && u.OrderId == order.OrderId);
            if (userLunchOrder != null) return;
            userLunchOrder = new OrderUserLunch()
            {
                OrderId = order.OrderId,
                UserLunchId = userLunch.UserLunchId
            };
            _context.OrderUserLunches.Add(userLunchOrder);
            _context.SaveChanges();
        }

        public OrderViewModel GetCurrentOrder()
        {
            var menu = _menuService.GetActiveMenu() ?? _menuService.GetLastMenu();
            if (menu == null) throw new Exception(LocalizationStrings.ActiveMenuDoesntNotExist);
            var order = _context.Orders.OrderByDescending(x => x.CreationDate)
                .FirstOrDefault(o => o.MenuId == menu.MenuId);
            var model = new OrderViewModel { UserLunches = _userLunchService.GetCurrentLunches() };
            if (order == null)
            {
                model.MenuId = menu.MenuId;
                model.OrderId = 0;
                model.OrderName = menu.Name;
                model.Submitted = false;
            }
            else
            {
                model.Submitted = order.Submitted;
                model.OrderId = order.OrderId;
                model.OrderName = order.OrderName;
                model.SubmitionDate = order.SubmitionDate?.ToString(LocalizationStrings.RusDateFormat) ?? string.Empty;
                model.MenuId = order.MenuId;
            }
            return model;
        }

        public OrderViewModel UpdateOrder(OrderViewModel model)
        {
            var order = GetOrCreateOrder(model.MenuId);
            order.OrderName = string.IsNullOrWhiteSpace(model.OrderName) ? order.OrderName : model.OrderName;
            _context.SaveChanges();
            var selectedLunches = model.UserLunches.Where(l => l.SelectedForOrder).ToList();
            _userLunchService.AdjustUserLunchesWithList(selectedLunches, model.MenuId);
            model.UserLunches = selectedLunches;
            return model;
        }

        private bool SendOrder(OrderViewModel model)
        {
            
        }

        public OrderViewModel SubmitOrder(OrderViewModel model, UserViewModel currentUser)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    UpdateOrder(model);
                    var order = GetOrCreateOrder(model.MenuId);
                    order.SubmitionDate = DateTime.Now;

                    order.Submitted = true;
                    order.SubmittedByUserId = currentUser.Id;
                    var idsToLock = model.UserLunches.Select(l => l.UserLunchId).ToList();
                    _userLunchService.LockLunches(idsToLock);

                    //ToDo: Impelemet email stuff

                    _context.SaveChanges();
                    _menuService.DisableMenu(model.MenuId);
                    model.Submitted = true;
                    model.SubmitionDate = order.SubmitionDate.Value.ToString(LocalizationStrings.RusDateFormat);
                    tr.Commit();
                    return model;
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
