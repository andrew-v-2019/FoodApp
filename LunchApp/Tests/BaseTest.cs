using System.Configuration;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ViewModels;

namespace Tests
{

    public class BaseTest
    {
        public static void Main()
        {

        }

        protected Context TestContext;
        protected readonly string DateFormat = LocalizationStrings.DateFormat;

        [OneTimeSetUp]
        public void CreateTestData()
        {
            CreateDatabse();
        }

        [OneTimeTearDown]
        public void ClearTestDate()
        {
            ClearTestDatabase();
        }

        private void CreateDatabse()
        {
            var options = new DbContextOptionsBuilder();
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            options.UseSqlServer(connectionString);
            TestContext = new Context(options.Options);
            Context.Migrate(TestContext);
            Context.Seed(TestContext);
        }

        private void ClearTestDatabase()
        {
            TestContext.OrderUserLunches.RemoveRange(TestContext.OrderUserLunches);
            TestContext.Orders.RemoveRange(TestContext.Orders);
            TestContext.UserLunchItems.RemoveRange(TestContext.UserLunchItems);
            TestContext.UserLunches.RemoveRange(TestContext.UserLunches);
            TestContext.MenuItems.RemoveRange(TestContext.MenuItems);
            TestContext.Menus.RemoveRange(TestContext.Menus);
            TestContext.Users.RemoveRange(TestContext.Users);
            TestContext.SaveChanges();
        }
    }


}
