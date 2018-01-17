using System;
using System.Linq;
using NUnit.Framework;
using Services;
using Services.Extensions;

namespace Tests.Menu
{
    public class GetMenuTests : BaseMenuTests
    {

        [Test]
        public void GetLastMenuAsTemplate_Test_WithNotEmpty()
        {
            var menuId = CreateFakeMenuInDatavase();
            var databaseMenu = TestContext.Menus.FirstOrDefault(x => x.MenuId == menuId);
            var menuService = new MenuService(TestContext);
            var testingItem = menuService.GetLastMenuAsTemplate();


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
