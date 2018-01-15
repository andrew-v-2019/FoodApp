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
        public OrderService(Context context, IMenuService menuService, IUserLunchService userLunchService)
        {
            _context = context;
            _menuService = menuService;
            _userLunchService = userLunchService;
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
            var order = _context.Orders.OrderByDescending(x => x.CreationDate).FirstOrDefault(o => o.Submitted == false);
            var model = new OrderViewModel { UserLunches = _userLunchService.GetCurrentLunches() };
            if (order == null)
            {
                var lastMenu = _menuService.GetActiveMenu();
                if (lastMenu == null) throw new Exception(LocalizationStrings.ActiveMenuDoesntNotExist);
                model.MenuId = lastMenu.MenuId;
                model.OrderId = 0;
                model.OrderName = lastMenu.Name;
                model.Submitted = false;
                return model;
            }
            model.Submitted = order.Submitted;
            model.OrderId = order.OrderId;
            model.OrderName = order.OrderName;
            model.MenuId = order.MenuId;
            return model;
        }

        public OrderViewModel UpdateOrder(OrderViewModel model)
        {
            var order = GetOrCreateOrder(model.MenuId);
            order.OrderName = order.OrderName;
            _context.SaveChanges();
            _userLunchService.AdjustUserLunchesWithList(model.UserLunches, model.MenuId);
            return model;
        }

        public OrderViewModel SubmitOrder(OrderViewModel model, UserViewModel currentUser)
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
            return model;
        }
    }
}
