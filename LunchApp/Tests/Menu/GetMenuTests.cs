using System;
using System.Linq;
using Data.Models;
using NUnit.Framework;
using Services;
using Services.Extensions;

namespace Tests.Menu
{
    public class GetMenuTests : BaseMenuTests
    {

        [Test]
        public void GetLastMenuTest_Negative()
        {
            ClearTestDate();
            var menuService = new MenuService(TestContext);
            var res = menuService.GetLastMenu();
            Assert.IsNull(res);
        }

        [Test]
        public void GetActiveMenuForEditTest()
        {
            ClearTestDate();
            var testingMenuId = CreateFakeMenuInDatavase();
            var menuService = new MenuService(TestContext);
            var testingItem = menuService.GetActiveMenuForEdit();
            var databaseMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == testingMenuId);

            Assert.IsNotNull(databaseMenu);
            Assert.AreEqual(testingItem.Price, databaseMenu.Price);
            Assert.AreEqual(testingItem.LunchDate.ParseDate(), databaseMenu.LunchDate);
            var databaseSections = TestContext.MenuSections.ToList();
            Assert.AreEqual(testingItem.Sections.Count, databaseSections.Count);
            foreach (var sec in testingItem.Sections)
            {
                var databseSection = databaseSections.FirstOrDefault(s => s.MenuSectionId == sec.MenuSectionId);
                Assert.IsNotNull(databseSection);
                Assert.AreEqual(databseSection.Name, sec.Name);
                Assert.AreEqual(databseSection.Number, sec.Number);
                var databaseItems =
                    TestContext.MenuItems.Where(
                            i => i.MenuId == databaseMenu.MenuId && i.MenuSectionId == sec.MenuSectionId)
                        .Select(i => i).ToList();
                Assert.IsNotNull(databaseItems);
                Assert.AreEqual(databaseItems.Count, sec.Items.Count);

                foreach (var testingIten in sec.Items)
                {
                    Assert.IsNotNull(databaseItems.FirstOrDefault(x => x.Name.Equals(testingIten.Name)));
                    Assert.AreEqual(testingIten.MenuId, testingMenuId);
                    Assert.AreEqual(testingIten.MenuSectionId, sec.MenuSectionId);
                }
            }
        }


        [Test]
        public void GetLastMenuTest_Positive()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var menuService = new MenuService(TestContext);
            var res = menuService.GetLastMenu();
            Assert.IsNotNull(res);
            var dbMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == testingMenuId);
            Assert.IsNotNull(dbMenu);
            Assert.AreEqual(dbMenu.MenuId, res.MenuId);
            Assert.AreEqual(dbMenu.Active, res.Active);
            Assert.AreEqual(dbMenu.LunchDate, res.LunchDate);
            Assert.AreEqual(dbMenu.Price, res.Price);
            Assert.AreEqual(dbMenu.CreationDate, res.CreationDate);
            Assert.AreEqual(dbMenu.Editable, res.Editable);
            Assert.AreEqual(dbMenu.Name, res.Name);
        }


        [Test]
        public void MenuIsActiveTest_Negative()
        {
            var menuService = new MenuService(TestContext);
            var res = menuService.MenuIsActive(1212121);
            Assert.IsFalse(res);

            var testingMenuId = CreateFakeMenuInDatavase();
            var testingMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == testingMenuId);
            Assert.IsNotNull(testingMenu);
            testingMenu.Active = false;
            TestContext.SaveChanges();

            var res2 = menuService.MenuIsActive(testingMenuId);
            Assert.IsFalse(res2);
        }


        [Test]
        public void MenuIsActiveTest_Positive()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var menuService = new MenuService(TestContext);
            var res = menuService.MenuIsActive(testingMenuId);
            Assert.IsTrue(res);
        }


        [Test]
        public void GetActiveMenu_TestNegative()
        {
            ClearTestDate();
            var menuService = new MenuService(TestContext);
            var res = menuService.GetActiveMenu();
            Assert.IsNull(res);
        }


        [Test]
        public void GetActiveMenu_TestPositive()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var menuService = new MenuService(TestContext);
            var res = menuService.GetActiveMenu();
            Assert.IsNotNull(res);
        }


        [Test]
        public void CheckIfOrderForMenuSubmittedTest_Negative()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var menuService = new MenuService(TestContext);
            var res = menuService.CheckIfOrderForMenuSubmitted(testingMenuId);
            Assert.IsFalse(res);
        }


        [Test]
        public void CheckIfOrderForMenuSubmittedTest_Positive()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var or = new Order()
            {
                MenuId = testingMenuId,
                Submitted = true,
            };
            TestContext.Orders.Add(or);
            TestContext.SaveChanges();
            var menuService = new MenuService(TestContext);
            var res = menuService.CheckIfOrderForMenuSubmitted(testingMenuId);
            Assert.IsTrue(res);
        }


        [Test]
        public void CheckIfMenuIsEditableTest_Positive()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var menuService = new MenuService(TestContext);
            var res = menuService.CheckIfMenuIsEditable(testingMenuId);
            Assert.IsTrue(res);
        }


        [Test]
        public void CheckIfMenuIsEditableTest_Negative()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var user = new User()
            {
                Name = "Test user"
            };
            TestContext.Users.Add(user);
            TestContext.SaveChanges();
            var userLunch = new UserLunch()
            {
                MenuId = testingMenuId,
                Submitted = true,
                UserId = user.Id
            };
            TestContext.UserLunches.Add(userLunch);
            TestContext.SaveChanges();
            var menuService = new MenuService(TestContext);
            var res = menuService.CheckIfMenuIsEditable(testingMenuId);
            Assert.IsFalse(res);
        }


        [Test]
        public void IsMenuForDateExist_TestPositive()
        {
            var testingDate = DateTime.Now.AddYears(1000).Date;

            var testingMenuId = CreateFakeMenuInDatavase();
            var testingMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == testingMenuId);
            Assert.IsNotNull(testingMenu);
          
            testingMenu.LunchDate = testingDate;
            TestContext.SaveChanges();

            var testingMenuId2 = CreateFakeMenuInDatavase();
            var testingMenu2 = TestContext.Menus.FirstOrDefault(m => m.MenuId == testingMenuId2);
            Assert.IsNotNull(testingMenu2);
            testingMenu2.LunchDate = testingDate;
            TestContext.SaveChanges();

            var menuService = new MenuService(TestContext);
            var r = menuService.MenuForDateExisits(testingDate, testingMenuId);
            Assert.IsTrue(r);
        }


        [Test]
        public void IsMenuForDateExist_TestNegative()
        {
            var testingMenuId = CreateFakeMenuInDatavase();
            var testingMenu = TestContext.Menus.FirstOrDefault(m => m.MenuId == testingMenuId);
            Assert.IsNotNull(testingMenu);
            var testingDate = DateTime.Now.AddYears(1000);
            testingMenu.LunchDate = testingDate;
            TestContext.SaveChanges();
            var menuService = new MenuService(TestContext);
            var r = menuService.MenuForDateExisits(testingDate, testingMenuId);
            Assert.IsFalse(r);
        }



        [Test]
        public void GetLastMenuAsTemplate_Test_WithNotEmpty()
        {
            var menuId = CreateFakeMenuInDatavase();
            var databaseMenu = TestContext.Menus.FirstOrDefault(x => x.MenuId == menuId);
            var menuService = new MenuService(TestContext);
            var testingItem = menuService.GetLastMenuAsTemplate();
            Assert.AreEqual(testingItem.Editable, true);
            var nextFrStr = DateTime.Now.NextFriday().ToString(DateFormat);
            Assert.IsNotNull(databaseMenu);
            Assert.AreEqual(testingItem.Price, databaseMenu.Price);
            Assert.AreEqual(testingItem.LunchDate, nextFrStr);
            var databaseSections = TestContext.MenuSections.ToList();
            Assert.AreEqual(testingItem.Sections.Count, databaseSections.Count);
            foreach (var sec in testingItem.Sections)
            {
                var databseSection = databaseSections.FirstOrDefault(s => s.MenuSectionId == sec.MenuSectionId);
                Assert.IsNotNull(databseSection);
                Assert.AreEqual(databseSection.Name, sec.Name);
                Assert.AreEqual(databseSection.Number, sec.Number);
                var databaseItems =
                    TestContext.MenuItems.Where(
                            i => i.MenuId == databaseMenu.MenuId && i.MenuSectionId == sec.MenuSectionId)
                        .Select(i => i).ToList();
                Assert.IsNotNull(databaseItems);
                Assert.AreEqual(databaseItems.Count, sec.Items.Count);

                foreach (var testingIten in sec.Items)
                {
                    Assert.IsNotNull(databaseItems.FirstOrDefault(x => x.Name.Equals(testingIten.Name)));
                    Assert.AreEqual(testingIten.MenuId, menuId);
                    Assert.AreEqual(testingIten.MenuItemId, 0);
                    Assert.AreEqual(testingIten.MenuSectionId, sec.MenuSectionId);
                }
            }
        }

        [Test]
        public void GetLastMenuAsTemplate_Test_WithEmpty()
        {
            ClearTestDate();
            var nextFrStr = DateTime.Now.NextFriday().ToString(DateFormat);
            var menuService = new MenuService(TestContext);
            var testingEmptyMenu = menuService.GetLastMenuAsTemplate();
            Assert.IsNotNull(testingEmptyMenu);
            Assert.AreEqual(testingEmptyMenu.MenuId, 0);
            Assert.IsTrue(testingEmptyMenu.Editable);
            Assert.AreEqual(testingEmptyMenu.LunchDate, nextFrStr);
            Assert.AreEqual(testingEmptyMenu.Price, 0);
            Assert.AreEqual(testingEmptyMenu.Sections.Count, 5);
            foreach (var sec in testingEmptyMenu.Sections)
            {
                Assert.AreEqual(sec.Items.Count, 1);
                Assert.IsEmpty(sec.Items[0].Name);
                Assert.AreEqual(sec.Items[0].Number, 1);
                Assert.AreEqual(sec.Items[0].MenuSectionId, sec.MenuSectionId);
                Assert.AreEqual(sec.Items[0].MenuId, 0);
                Assert.AreEqual(sec.Items[0].MenuItemId, 0);
            }
        }
    }
}
