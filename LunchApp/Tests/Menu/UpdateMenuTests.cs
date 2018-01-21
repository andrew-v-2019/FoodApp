using System;
using System.Linq;
using NUnit.Framework;
using Services;
using Services.Extensions;
using ViewModels;


namespace Tests.Menu
{
    [TestFixture]
    public class UpdateMenuTests : BaseMenuTests
    {
        [Test]
        public void UpdateMenu_WithNewItems_Test()
        {
            var fakeFrontEndMenuOld = GetFakeMenu(0);
            var menuService = new MenuService(TestContext);
            fakeFrontEndMenuOld.LunchDate = DateTime.Now.AddDays(1).ToString(DateFormat);
            var updateResult = menuService.UpdateMenu(fakeFrontEndMenuOld);
            var newMenuId = updateResult.MenuId;

            var fakeFrontEndMenu = GetFakeMenu(newMenuId);
            fakeFrontEndMenu.LunchDate = DateTime.Now.AddDays(1).ToString(DateFormat);
            menuService.UpdateMenu(fakeFrontEndMenu);

            var count = TestContext.Menus.Count(m => m.MenuId == newMenuId);
            Assert.IsTrue(count == 1);

            var dbMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == newMenuId);
            Assert.IsNotNull(dbMenu);

            var lunchDate = fakeFrontEndMenu.LunchDate.ParseDate();
            Assert.AreEqual(dbMenu.Name,
                string.Format(LocalizationStrings.MenuDefaultName,
                    lunchDate.ToString(LocalizationStrings.RusDateFormat)));
            Assert.IsTrue(dbMenu.Active);
            Assert.IsTrue(dbMenu.Editable);
            Assert.AreEqual(dbMenu.LunchDate, lunchDate);
            Assert.AreEqual(dbMenu.Price, fakeFrontEndMenuOld.Price);
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
        public void UpdateMenu_WithUpdatedItemNames_Test()
        {
            var fakeFrontEndMenu = GetFakeMenu(0);
            var menuService = new MenuService(TestContext);
            fakeFrontEndMenu.LunchDate = DateTime.Now.ToString(DateFormat);
            var updateResult = menuService.UpdateMenu(fakeFrontEndMenu);
            var newMenuId = updateResult.MenuId;

            MessFakeMenu(fakeFrontEndMenu);
            menuService.UpdateMenu(fakeFrontEndMenu);

            var count = TestContext.Menus.Count(m => m.MenuId == newMenuId);
            Assert.IsTrue(count == 1);

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
    }
}
