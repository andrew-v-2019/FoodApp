using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private UpdateMenuViewModel GetEmptyMenu()
        {
            var model = new UpdateMenuViewModel
            {
                LunchDate = DateTime.Now.NextFriday().ToString(_dateFormat),
                Editable = true,
                Sections = _context.MenuSections.Select(s => new MenuSectionViewModel()
                {
                    MenuSectionId = s.MenuSectionId,
                    Name = s.Name,
                    Number = s.Number,
                    Items = new List<MenuItemViewModel>()
                    {
                        new MenuItemViewModel()
                        {
                            MenuId = 0,
                            MenuItemId = 0,
                            MenuSectionId = s.MenuSectionId,
                            Name = string.Empty,
                            Number = 1
                        }
                    },

                }).ToList()
            };
            return model;
        }

        public UpdateMenuViewModel GetLastMenuAsTemplate()
        {
            var template = GetLastMenu();
            template.MenuId = 0;
            template.Sections.ForEach(s => s.Items.ForEach(m => { m.MenuItemId = 0; }));
            template.Editable = true;
            template.LunchDate = DateTime.Now.NextFriday().ToString(_dateFormat);
            return template;
        }

        public Menu GetActiveMenu()
        {
            var lastMenu = _context.Menus.OrderByDescending(m => m.LunchDate)
                .ThenByDescending(m => m.CreationDate)
                .FirstOrDefault(m => m.Active);
            return lastMenu;
        }

        public UpdateMenuViewModel GetLastMenu()
        {
            var lastMenu = GetActiveMenu();
            if (lastMenu == null) return GetEmptyMenu();
            var model = new UpdateMenuViewModel()
            {
                LunchDate = lastMenu.LunchDate.ToString(_dateFormat),
                MenuId = lastMenu.MenuId,
                Price = lastMenu.Price,
                Editable = lastMenu.Editable,
                Sections = _context.MenuSections.Select(s => new MenuSectionViewModel()
                {
                    MenuId = lastMenu.MenuId,
                    Name = s.Name,
                    MenuSectionId = s.MenuSectionId,
                    Number = s.Number,
                    Items = _context.MenuItems
                        .Where(i => i.MenuId == lastMenu.MenuId && i.MenuSectionId == s.MenuSectionId).Select(
                            i => new MenuItemViewModel()
                            {
                                MenuSectionId = s.MenuSectionId,
                                Number = i.Number,
                                Name = i.Name,
                                MenuItemId = i.MenuItemId
                            }).ToList()
                }).ToList()
            };
            return model;
        }

        #endregion

        #region Update

        public UpdateMenuViewModel UpdateMenu(UpdateMenuViewModel model)
        {
            var lunchDate = model.LunchDate.ParseDate();
            var anyForThisDate = _context.Menus.Any(m => m.LunchDate.Date == lunchDate && m.MenuId != model.MenuId);
            if (anyForThisDate)
            {
                throw new Exception(LocalizationStrings.MenuForThisDateExists);
            }
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
            return model;
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

        #endregion
    }
}
