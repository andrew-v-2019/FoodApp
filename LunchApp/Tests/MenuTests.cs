using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using NUnit.Framework;
using Services;
using Services.Extensions;
using ViewModels;
using ViewModels.Menu;

namespace Tests
{
    [TestFixture]
    public class MenuTests : BaseTest
    {
        [Test]
        public void GetEmptyMenuTest()
        {
            var sec = TestContext.MenuSections.ToList();
            var menuService = new MenuService(TestContext);
            var emptyMenu = menuService.GetEmptyMenu();
            Assert.IsNull(emptyMenu.Price);
            Assert.IsTrue(emptyMenu.Editable);
            Assert.AreEqual(emptyMenu.Sections.Count, sec.Count);
            var nextFr = DateTime.Now.NextFriday().ToString(DateFormat);
            Assert.AreEqual(emptyMenu.LunchDate, nextFr);

            foreach (var testingSection in emptyMenu.Sections)
            {
                var dbSection = sec.FirstOrDefault(s => s.Name.Equals(testingSection.Name));
                Assert.IsNotNull(dbSection);
                Assert.AreEqual(dbSection.Number, testingSection.Number);
            }
        }


        [Test]
        public void UpdateMenuTest()
        {
            var fakeFrontEndMenu = GetFakeMenu(0);
            var menuService = new MenuService(TestContext);
            var updateResult = menuService.UpdateMenu(fakeFrontEndMenu);
            var newMenuId = updateResult.MenuId;

            MessFakeMenu(fakeFrontEndMenu);
            menuService.UpdateMenu(fakeFrontEndMenu);

            var dbMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == newMenuId);
            Assert.IsNotNull(dbMenu);

            var lunchDate = fakeFrontEndMenu.LunchDate.ParseDate();
            Assert.AreEqual(dbMenu.Name,
                string.Format(LocalizationStrings.MenuDefaultName,
                    lunchDate.ToString(LocalizationStrings.RusDateFormat)));
            Assert.IsTrue(dbMenu.Active);
            Assert.IsTrue(dbMenu.Editable);
            Assert.AreEqual(dbMenu.LunchDate, lunchDate);
            Assert.AreEqual(dbMenu.Price, fakeFrontEndMenu.Price);
            Assert.AreEqual(dbMenu.MenuId, newMenuId);
            Assert.AreEqual(dbMenu.CreationDate.Date, DateTime.Now.Date);

            var dbMenuItems = TestContext.MenuItems.Where(m => m.MenuId == newMenuId);
            Assert.IsNotNull(dbMenuItems);

            foreach (var fakeMenuSection in fakeFrontEndMenu.Sections)
            {
                var dbMenuSectionItems = dbMenuItems.Where(x => x.MenuSectionId == fakeMenuSection.MenuSectionId)
                    .Select(x => x).ToList();
                Assert.AreEqual(dbMenuSectionItems.Count, fakeMenuSection.Items.Count);
                foreach (var fakeMenuSectionItem in fakeMenuSection.Items)
                {
                    var dbMenuSectionItem =
                        dbMenuSectionItems.FirstOrDefault(x => x.Name.Equals(fakeMenuSectionItem.Name));
                    Assert.IsNotNull(dbMenuSectionItem);
                    Assert.AreEqual(dbMenuSectionItem.MenuSectionId, fakeMenuSectionItem.MenuSectionId);
                    Assert.AreEqual(dbMenuSectionItem.MenuId, newMenuId);
                    Assert.AreEqual(dbMenuSectionItem.Number, fakeMenuSectionItem.Number);
                }
            }
        }


        [Test]
        public void CreateMenuTest()
        {
            const int menuId = 0;
            var fakeFrontEndMenu = GetFakeMenu(menuId);
            var menuService = new MenuService(TestContext);
            var updateResult = menuService.UpdateMenu(fakeFrontEndMenu);
            var newMenuId = updateResult.MenuId;
            Assert.IsTrue(newMenuId > 0);

            var dbMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == newMenuId);
            Assert.IsNotNull(dbMenu);


            var lunchDate = fakeFrontEndMenu.LunchDate.ParseDate();
            Assert.AreEqual(dbMenu.Name,
                string.Format(LocalizationStrings.MenuDefaultName,
                    lunchDate.ToString(LocalizationStrings.RusDateFormat)));
            Assert.IsTrue(dbMenu.Active);
            Assert.IsTrue(dbMenu.Editable);
            Assert.AreEqual(dbMenu.LunchDate, lunchDate);
            Assert.AreEqual(dbMenu.Price, fakeFrontEndMenu.Price);
            Assert.AreEqual(dbMenu.MenuId, newMenuId);
            Assert.AreEqual(dbMenu.CreationDate.Date, DateTime.Now.Date);

            var dbMenuItems = TestContext.MenuItems.Where(m => m.MenuId == newMenuId);
            Assert.IsNotNull(dbMenuItems);

            foreach (var fakeMenuSection in fakeFrontEndMenu.Sections)
            {
                var dbMenuSectionItems = dbMenuItems.Where(x => x.MenuSectionId == fakeMenuSection.MenuSectionId)
                    .Select(x => x).ToList();
                Assert.AreEqual(dbMenuSectionItems.Count, fakeMenuSection.Items.Count);
                foreach (var fakeMenuSectionItem in fakeMenuSection.Items)
                {
                    var dbMenuSectionItem =
                        dbMenuSectionItems.FirstOrDefault(x => x.Name.Equals(fakeMenuSectionItem.Name));
                    Assert.IsNotNull(dbMenuSectionItem);
                    Assert.AreEqual(dbMenuSectionItem.MenuSectionId, fakeMenuSectionItem.MenuSectionId);
                    Assert.AreEqual(dbMenuSectionItem.MenuId, newMenuId);
                    Assert.AreEqual(dbMenuSectionItem.Number, fakeMenuSectionItem.Number);
                }
            }
        }


        private void MessFakeMenu(UpdateMenuViewModel model)
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


        private UpdateMenuViewModel GetFakeMenu(int menuId)
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
