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
    public class MenuService: IMenuService
    {
        private readonly Context _context;
        private const string DateFormat = "yyyy-MM-dd";
        public MenuService(Context context)
        {
            _context = context;
        }

        #region Get

        public UpdateMenuViewModel GetEmptyMenu()
        {
            var model = new UpdateMenuViewModel
            {
                LunchDate = DateTime.Now.NextFriday().ToString(DateFormat),
                Editable = true,
                Sections = _context.MenuSections.Select(s => new MenuSectionViewModel()
                {
                    MenuSectionId = s.MenuSectionId,
                    Name = s.Name,
                    Number = s.Number,
                    Items = new List<MenuItemViewModel>() { new MenuItemViewModel() {
                        MenuId = 0,
                        MenuItemId = 0,
                        MenuSectionId = s.MenuSectionId,
                        Name = string.Empty,
                        Number =1
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
            template.LunchDate = DateTime.Now.NextFriday().ToString(DateFormat);
            return template;
        }

        public UpdateMenuViewModel GetLastMenu()
        {
            var lastMenu = _context.Menus.OrderByDescending(m => m.LunchDate).FirstOrDefault();
            if (lastMenu == null) return GetEmptyMenu();
            var model = new UpdateMenuViewModel()
            {
                LunchDate = lastMenu.LunchDate.ToString(DateFormat),
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

        public UpdateMenuViewModel GetFakeMenu()
        {

            var model = new UpdateMenuViewModel()
            {
                LunchDate = DateTime.Now.ToString(DateFormat),
                MenuId = 1,
                Price = 1.1,
                //Title = "Test",
                Sections = new List<MenuSectionViewModel>()
            };

            model.Sections.Add(new MenuSectionViewModel()
            {
                MenuId = 1,
                MenuSectionId = 1,
                Name = "Салаты",
                Number = 1,
                Items = new List<MenuItemViewModel>()
            });
            model.Sections.Add(new MenuSectionViewModel()
            {
                MenuId = 1,
                MenuSectionId = 2,
                Name = "Супы",
                Number = 2,
                Items = new List<MenuItemViewModel>()
            });
            model.Sections.Add(new MenuSectionViewModel()
            {
                MenuId = 1,
                MenuSectionId = 3,
                Name = "Горячее",
                Number = 3,
                Items = new List<MenuItemViewModel>()
            });
            model.Sections.Add(new MenuSectionViewModel()
            {
                MenuId = 1,
                MenuSectionId = 4,
                Name = "Гарнир",
                Number = 4,
                Items = new List<MenuItemViewModel>()
            });
            model.Sections.Add(new MenuSectionViewModel()
            {
                MenuId = 1,
                MenuSectionId = 5,
                Name = "Напитки",
                Number = 5,
                Items = new List<MenuItemViewModel>()
            });
            model.Sections[0].Items.Add(new MenuItemViewModel()
            {
                MenuSectionId = 1,
                Number = 1,
                Name = "Test",
                MenuItemId = 1
            });
            return model;
        }

        #endregion

        #region Update
        public UpdateMenuViewModel UpdateMenu(UpdateMenuViewModel model)
        {
            Menu menu;
            var lunchDate = model.LunchDate.ParseDate();
            if (model == null) return null;
            if (model.MenuId == 0)
            {
                menu = new Menu
                {
                    LunchDate = DateTime.Parse(model.LunchDate),
                    CreationDate = DateTime.Now,
                    Price = model.Price,
                    Active = true,
                    Editable = true
                };
                _context.Menus.Add(menu);
            }
            else
            {
                menu = _context.Menus.FirstOrDefault(l => l.MenuId == model.MenuId);
                if (menu == null)
                {
                    throw new Exception("Menu not found menuId = " + model.MenuId);
                }
                if (!menu.Editable)
                {
                    throw new Exception("Меню нельзя редактировать..");
                }
                menu.LunchDate = lunchDate;
                menu.Price = model.Price;
                menu.Editable = true;
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

        private int UpdateMenuSection(MenuSectionViewModel model)
        {
            MenuSection menuSection;
            if (model.MenuSectionId == 0)
            {
                menuSection = new MenuSection
                {
                    Name = model.Name,
                    Number = model.Number
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
