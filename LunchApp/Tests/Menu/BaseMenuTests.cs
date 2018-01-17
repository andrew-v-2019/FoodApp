using System;
using System.Collections.Generic;
using System.Linq;
using Services;
using ViewModels;
using ViewModels.Menu;

namespace Tests.Menu
{
    public class BaseMenuTests : BaseTest
    {




        protected int CreateFakeMenuInDatavase()
        {
            var fakeViewModel = GetFakeMenu(0);
            var service = new MenuService(TestContext);
            var id = service.UpdateMenu(fakeViewModel).MenuId;
            return id;
        }

        protected static void MessFakeMenu(UpdateMenuViewModel model)
        {
            model.Price = model.Price + new Random().Next(1, 2000);
            foreach (var section in model.Sections)
            {
                foreach (var item in section.Items)
                {
                    item.Name = Guid.NewGuid().ToString();
                }
            }
        }

        protected UpdateMenuViewModel GetFakeMenu(int menuId)
        {
            var sec = TestContext.MenuSections.ToList();
            var model = new UpdateMenuViewModel
            {
                Editable = true,
                Price = 10000,
                MenuId = menuId,
                Sections = new List<MenuSectionViewModel>(),
                LunchDate = DateTime.Now.AddYears(1100).ToString(DateFormat)
            };
            foreach (var dbSection in sec)
            {
                var section = new MenuSectionViewModel()
                {
                    Number = dbSection.Number,
                    Name = dbSection.Name,
                    MenuSectionId = dbSection.MenuSectionId,
                    MenuId = menuId,
                    Items = new List<MenuItemViewModel>()
                };
                for (var i = 0; i < 100; i++)
                {
                    var item = new MenuItemViewModel()
                    {
                        MenuId = menuId,
                        Number = i,
                        Name = Guid.NewGuid().ToString(),
                        MenuSectionId = section.MenuSectionId,
                    };

                    section.Items.Add(item);
                }
                model.Sections.Add(section);
            }
            return model;
        }
    }
}
