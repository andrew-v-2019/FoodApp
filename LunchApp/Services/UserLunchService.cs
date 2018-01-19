using System;
using System.Linq;
using ClassLibrary5.UserLunch;
using Data.Models;
using Services.Interfaces;
using ViewModels.UserLunch;
using System.Collections.Generic;
using ViewModels;

namespace Services
{
    public class UserLunchService : IUserLunchService
    {
        private readonly Context _context;
        private readonly string _dateFormat = LocalizationStrings.DateFormat;
        private readonly IMenuService _menuService;

        public UserLunchService(Context context, IMenuService menuService)
        {

            _context = context;
            _menuService = menuService;
        }

        #region update

        public void LockLunches(List<int> userLunchIds)
        {
            var lunches = _context.UserLunches.Where(l => userLunchIds.Contains(l.UserLunchId)).Select(l => l).ToList();
            foreach (var lunch in lunches)
            {
                lunch.Editable = false;
            }
            _context.SaveChanges();
        }

        public void AdjustUserLunchesWithList(List<UserLunchViewModel> model, int menuId)
        {
            var orderUserLunchIds = model.Select(l => l.UserLunchId).ToList();
            var lunchIdsToExclude = _context.UserLunches
                .Where(l => l.MenuId == menuId && !orderUserLunchIds.Contains(l.UserLunchId))
                .Select(l => l.UserLunchId)
                .ToList();
            var iunchItemsToExclude = _context.UserLunchItems
                .Where(i => lunchIdsToExclude.Contains(i.UserLunchId))
                .ToList();
            _context.OrderUserLunches.RemoveRange(
                _context.OrderUserLunches.Where(x => lunchIdsToExclude.Contains(x.UserLunchId)));
            _context.UserLunchItems.RemoveRange(iunchItemsToExclude);
            _context.UserLunches.RemoveRange(
                _context.UserLunches.Where(l => lunchIdsToExclude.Contains(l.UserLunchId)));
            _context.SaveChanges();
        }

        public UserLunchViewModel UpdateUserLunch(UserLunchViewModel model)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    if (_menuService.CheckIfOrderForMenuSubmitted(model.MenuId))
                    {
                        throw new Exception(LocalizationStrings.OrderHasBeenSent);
                    }
                    var newLunch = false;
                    var userLunch = _context.UserLunches.FirstOrDefault(x => x.UserLunchId == model.UserLunchId);
                    if (userLunch == null)
                    {
                        newLunch = true;
                        userLunch = new UserLunch();
                    }
                    userLunch.MenuId = model.MenuId;
                    userLunch.UserId = model.User.Id;
                    userLunch.UserLunchId = model.UserLunchId;
                    userLunch.Submitted = true;
                    userLunch.Editable = true;
                    userLunch.SubmitionDate = DateTime.Now;
                    if (newLunch)
                    {
                        userLunch.CreationDate = DateTime.Now;
                        _context.UserLunches.Add(userLunch);
                        _context.SaveChanges();
                    }
                    var lunchId = userLunch.UserLunchId;
                    model.UserLunchId = lunchId;
                    UpdateLunchItems(model);
                    var menu = _context.Menus.FirstOrDefault(m => m.MenuId == model.MenuId);
                    if (menu == null) return model;
                    menu.Editable = false;
                    _context.SaveChanges();
                    tr.Commit();
                    return model;
                }
                catch (Exception e)
                {
                    tr.Rollback();
                    throw new Exception(e.Message, e.InnerException);
                }
            }
        }

        private UserLunchViewModel UpdateLunchItems(UserLunchViewModel model)
        {
            foreach (var sec in model.Sections)
            {
                foreach (var item in sec.Items)
                {
                    UpdateUserLunchItem(item, model.UserLunchId);
                }
            }
            return model;
        }

        private UserLunchItemViewModel UpdateUserLunchItem(UserLunchItemViewModel model, int userLunchId)
        {
            if (!model.Checked) return model;
            var newItem = false;
            var dbItem =
                _context.UserLunchItems.FirstOrDefault(
                    x => x.MenuItemId == model.MenuItemId && x.UserLunchId == userLunchId);
            if (dbItem == null)
            {
                newItem = true;
                dbItem = new UserLunchItem();
            }
            dbItem.UserLunchId = userLunchId;
            dbItem.MenuItemId = model.MenuItemId;
            if (!newItem) return model;
            dbItem.Date = DateTime.Now;
            _context.UserLunchItems.Add(dbItem);
            _context.SaveChanges();
            return model;
        }

        #endregion



        #region Get




        private UserLunchViewModel GetCurrentLunch(Menu activeMenu, UserLunch lunch)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == lunch.UserId);
            var userName = user != null ? user.Name : string.Empty;
            var model = new UserLunchViewModel()
            {
                Editable = !_menuService.CheckIfOrderForMenuSubmitted(lunch.MenuId),
                UserLunchId = lunch.UserLunchId,
                MenuId = lunch.MenuId,
                LunchDate = activeMenu.LunchDate.ToString(_dateFormat),
                Price = activeMenu.Price,
                User = new ViewModels.User.UserViewModel()
                {
                    Id = lunch.UserId,
                    Name = userName
                },
                Sections = _context.MenuSections.Select(s => new UserLunchSectionViewModel()
                {
                    MenuSectionId = s.MenuSectionId,
                    MenuId = activeMenu.MenuId,
                    Name = s.Name,
                    Number = s.Number,
                    Items = _context.MenuItems
                            .Where(i => i.MenuSectionId == s.MenuSectionId && i.MenuId == activeMenu.MenuId)
                            .Select(i => new UserLunchItemViewModel()
                            {
                                MenuSectionId = i.MenuSectionId,
                                Name = i.Name,
                                MenuItemId = i.MenuItemId,
                                Number = i.Number,
                                Checked =
                                    _context.UserLunchItems.Any(x => x.MenuItemId == i.MenuItemId)
                            })
                            .OrderBy(i => i.Number)
                            .ToList()
                })
                    .OrderBy(s => s.Number)
                    .ToList(),
                SelectedForOrder = true
            };
            return model;
        }

        public UserLunchViewModel GetCurrentLunch(int userId)
        {
            var menu = _menuService.GetActiveMenu() ?? _menuService.GetLastMenu();
            if (menu == null) throw new Exception(LocalizationStrings.ActiveMenuDoesntNotExist);
            var lunch = _context.UserLunches.FirstOrDefault(l => l.UserId == userId && l.MenuId == menu.MenuId);
            if (lunch == null)
            {
                var newLunch = CreateNewLunch(menu);
                return newLunch;
            }
            var model = GetCurrentLunch(menu, lunch);
            return model;
        }

        public List<UserLunchViewModel> GetCurrentLunches()
        {
            var menu = _menuService.GetActiveMenu() ?? _menuService.GetLastMenu();
            if (menu == null) throw new Exception(LocalizationStrings.ActiveMenuDoesntNotExist);
            var lunches = _context.UserLunches.Where(l => l.MenuId == menu.MenuId).Select(l => l).ToList();
            var viewModels = lunches.Select(l => GetCurrentLunch(menu, l)).ToList();
            return viewModels;
        }

        private UserLunchViewModel CreateNewLunch(Menu activeMenu)
        {
            if (_menuService.CheckIfOrderForMenuSubmitted(activeMenu.MenuId))
            {
                throw new Exception(LocalizationStrings.OrderHasBeenSent);
            }
            var sections = _context.MenuSections.Select(s => new UserLunchSectionViewModel()
            {
                MenuSectionId = s.MenuSectionId,
                MenuId = activeMenu.MenuId,
                Name = s.Name,
                Number = s.Number,
                Items = _context.MenuItems
                        .Where(i => i.MenuSectionId == s.MenuSectionId && i.MenuId == activeMenu.MenuId)
                        .Select(i => new UserLunchItemViewModel()
                        {
                            MenuSectionId = i.MenuSectionId,
                            Name = i.Name,
                            MenuItemId = i.MenuItemId,
                            Number = i.Number,
                            Checked = false
                        })
                        .OrderBy(i => i.Number)
                        .ToList()
            })
                .OrderBy(s => s.Number)
                .ToList();
            var model = new UserLunchViewModel()
            {
                MenuId = activeMenu.MenuId,
                Sections = sections,
                Price = activeMenu.Price,
                LunchDate = activeMenu.LunchDate.ToString(_dateFormat),
                Editable = !_menuService.CheckIfOrderForMenuSubmitted(activeMenu.MenuId)
            };
            return model;
        }

        #endregion


    }
}
