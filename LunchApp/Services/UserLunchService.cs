using System;
using System.Linq;
using ClassLibrary5.UserLunch;
using Data.Models;
using Services.Interfaces;
using ViewModels;
using ViewModels.UserLunch;

namespace Services
{
    public class UserLunchService : IUserLunchService
    {
        private readonly Context _context;
        private const string DateFormat = "yyyy-MM-dd";
        public UserLunchService(Context context)
        {
            _context = context;
        }


        #region update
        public UserLunchViewModel UpdateUserLunch(UserLunchViewModel model)
        {
            var userLunch = new UserLunch()
            {
                MenuId = model.MenuId,
                UserId = model.User.Id,
                UserLunchId = model.UserLunchId,
                Submitted = false,
                Editable = true
            };
            return null;
        }

        private UserLunch AddOrUpdate(UserLunch userLunch)
        {
            //var l = _context.UserLunches()
            return null;
        }
        #endregion



        #region Get

        private UserLunchViewModel GetCurrentLunch(Menu activeMenu, UserLunch lunch)
        {
            var model = new UserLunchViewModel()
            {
                Editable = lunch.Editable,
                UserLunchId = lunch.UserLunchId,
                MenuId = lunch.MenuId,
                LunchDate = activeMenu.LunchDate.ToString(DateFormat),
                Price = activeMenu.Price,
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
                    .ToList()
            };
            return model;
        }

        public UserLunchViewModel GetCurrentLunch(int userId)
        {
            var activeMenu = _context.Menus.FirstOrDefault(m => m.Active);
            if (activeMenu == null) return null;
            var lunch = _context.UserLunches.FirstOrDefault(l => l.UserId == userId && l.Menu.Active);
            if (lunch == null)
            {
                var newLunch = CreateNewLunch(activeMenu);
                return newLunch;
            }
            var model = GetCurrentLunch(activeMenu, lunch);
            return model;
        }

        private UserLunchViewModel CreateNewLunch(Menu activeMenu)
        {

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
                LunchDate = activeMenu.LunchDate.ToString(DateFormat),
                Editable = true,
            };
            return model;
        }
        #endregion


    }
}
