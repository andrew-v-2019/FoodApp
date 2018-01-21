using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Services.Interfaces;
using ViewModels;
using ViewModels.Menu;
using Services.Extensions;

namespace Services
{
    public class MenuService : IMenuService
    {
        private readonly Context _context;
        private readonly string _dateFormat = LocalizationStrings.DateFormat;
        public MenuService(Context context)
        {
            _context = context;
        }

        #region Get

        public UpdateMenuViewModel GetLastMenuAsTemplate()
        {
            var menu = GetLastMenu();
            var template = GetViewModel(menu);
            template.MenuId = 0;
            template.Editable = true;
            template.Sections.ForEach(s => s.Items.ForEach(m => { m.MenuItemId = 0; }));
            template.LunchDate = DateTime.Now.NextFriday().ToString(_dateFormat);
            return template;
        }

        public UpdateMenuViewModel GetActiveMenuForEdit()
        {
            var menu = GetActiveMenu();
            if (menu == null) return GetViewModel();
            var model = GetViewModel(menu);
            return model;
        }

        public Menu GetLastMenu()
        {
            var lastMenu = _context.Menus.OrderByDescending(m => m.LunchDate)
                .ThenByDescending(m => m.CreationDate)
                .FirstOrDefault();
            return lastMenu;
        }

        public bool MenuIsActive(int menuId)
        {
            var me = _context.Menus.FirstOrDefault(m => m.MenuId == menuId);
            return me != null && me.Active;
        }

        public Menu GetActiveMenu()
        {
            var activeMenu = _context.Menus.OrderByDescending(m => m.LunchDate)
                .ThenByDescending(m => m.CreationDate)
                .FirstOrDefault(m => m.Active);
            return activeMenu;
        }

        public bool CheckIfOrderForMenuSubmitted(int menuId)
        {
            var any = _context.Orders.Any(o => o.MenuId == menuId && o.Submitted);
            return any;
        }

        public bool CheckIfMenuIsEditable(int menuId)
        {
            var any = _context.UserLunches.Any(x => x.MenuId == menuId && x.Submitted);
            return !any;
        }

        private UpdateMenuViewModel GetViewModel(Menu menu = null)
        {
            var menuId = menu?.MenuId ?? 0;
            var price = menu != null ? menu.Price : 0;
            var editable = CheckIfMenuIsEditable(menuId);
            var date = menu?.LunchDate.ToString(_dateFormat) ?? DateTime.Now.NextFriday().ToString(_dateFormat);
            var model = new UpdateMenuViewModel()
            {
                LunchDate = date,
                MenuId = menuId,
                Price = price,
                Editable = editable,
                Sections = _context.MenuSections.Select(s => new MenuSectionViewModel()
                    {
                        MenuId = menuId,
                        Name = s.Name,
                        MenuSectionId = s.MenuSectionId,
                        Number = s.Number,
                        Items = menu != null
                            ? _context.MenuItems
                                .Where(i => i.MenuId == menuId && i.MenuSectionId == s.MenuSectionId)
                                .Select(
                                    i => new MenuItemViewModel()
                                    {
                                        MenuSectionId = s.MenuSectionId,
                                        Number = i.Number,
                                        Name = i.Name,
                                        MenuItemId = i.MenuItemId,
                                        MenuId = menuId
                                    })
                                .ToList()
                            : new List<MenuItemViewModel>()
                            {
                                new MenuItemViewModel()
                                {
                                    MenuId = 0,
                                    MenuItemId = 0,
                                    MenuSectionId = s.MenuSectionId,
                                    Name = string.Empty,
                                    Number = 1
                                }
                            }
                    })
                    .ToList()
            };
            return model;
        }

        public bool MenuForDateExisits(DateTime lunchDate, int menuId)
        {
            var anyForThisDate =
                _context.Menus.Any(m => m.LunchDate.Date == lunchDate && m.MenuId != menuId);
            return anyForThisDate;
        }

        #endregion

        #region Update

        public UpdateMenuViewModel UpdateMenu(UpdateMenuViewModel model)
        {
            using (var tr = _context.Database.BeginTransaction())
            {
                try
                {
                    var lunchDate = model.LunchDate.ParseDate();
                    var name = string.Format(LocalizationStrings.MenuDefaultName,
                        lunchDate.ToString(LocalizationStrings.RusDateFormat));
                    var menu = _context.Menus.FirstOrDefault(l => l.MenuId == model.MenuId);
                    if (menu == null)
                    {
                        menu = new Menu
                        {
                            LunchDate = lunchDate,
                            CreationDate = DateTime.Now,
                            Price = model.Price,
                            Active = true,
                            Editable = true,
                            Name = name
                        };
                        _context.Menus.Add(menu);
                    }
                    else
                    {
                        if (!menu.Editable)
                        {
                            throw new Exception(LocalizationStrings.MenuIsLocked);
                        }
                        menu.LunchDate = lunchDate;
                        menu.Price = model.Price;
                        menu.Editable = true;
                        menu.Name = name;
                    }
                    _context.SaveChanges();
                    model.MenuId = menu.MenuId;
                    model = UpdateMenuSections(model);
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

        private UpdateMenuViewModel UpdateMenuSections(UpdateMenuViewModel model)
        {
            foreach (var section in model.Sections)
            {
                section.MenuId = model.MenuId;
                var id = UpdateMenuSection(section);
                section.MenuSectionId = id;
            }
            return model;
        }

        private void RemoveExceedMenuItems(MenuSectionViewModel model)
        {
            var vmIds = model.Items.Select(x => x.MenuItemId);
            var dbIds = _context.MenuItems.Where(x => x.MenuSectionId == model.MenuSectionId &&
                                                      x.MenuId == model.MenuId)
                .Select(x => x.MenuItemId);
            var idsToBurn = dbIds.Where(x => !vmIds.Contains(x));
            _context.MenuItems.RemoveRange(_context.MenuItems.Where(x => idsToBurn.Contains(x.MenuItemId)));
        }

        private int UpdateMenuSection(MenuSectionViewModel model)
        {
            MenuSection menuSection;
            if (model.MenuSectionId == 0)
            {
                menuSection = new MenuSection
                {
                    Name = model.Name,
                    Number = model.Number,
                };
                _context.MenuSections.Add(menuSection);
            }
            else
            {
                menuSection = _context.MenuSections.FirstOrDefault(s => s.MenuSectionId == model.MenuSectionId);
                if (menuSection == null) return 0;
                menuSection.Name = model.Name;
                menuSection.Number = model.Number;
            }
            _context.SaveChanges();
            model.MenuSectionId = menuSection.MenuSectionId;
            RemoveExceedMenuItems(model);
            foreach (var menuItem in model.Items)
            {
                menuItem.MenuSectionId = model.MenuSectionId;
                menuItem.MenuId = model.MenuId;
                var id = UpdateMenuItem(menuItem);
                menuItem.MenuItemId = id;
            }
            return model.MenuSectionId;
        }

        private int UpdateMenuItem(MenuItemViewModel model)
        {
            MenuItem menuItem;
            if (model.MenuItemId == 0)
            {
                menuItem = new MenuItem
                {
                    MenuSectionId = model.MenuSectionId,
                    Name = model.Name,
                    Number = model.Number,
                    MenuId = model.MenuId
                };
                _context.MenuItems.Add(menuItem);
            }
            else
            {
                menuItem = _context.MenuItems.FirstOrDefault(m => m.MenuItemId == model.MenuItemId);
                if (menuItem == null) return 0;
                menuItem.MenuSectionId = model.MenuSectionId;
                menuItem.Name = model.Name;
                menuItem.Number = model.Number;
            }
            _context.SaveChanges();
            return menuItem.MenuItemId;
        }

        public void DisableMenu(int menuId)
        {
            var menu = _context.Menus.FirstOrDefault(m => m.MenuId == menuId);
            if (menu == null) return;
            menu.Active = false;
            _context.SaveChanges();
        }

        #endregion
    }
}
