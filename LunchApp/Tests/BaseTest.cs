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

        public BaseTest()
        {
            CreateDatabse();
        }

        [OneTimeSetUp]
        public void CreateDatabse()
        {
            var options = new DbContextOptionsBuilder();
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            options.UseSqlServer(connectionString);
            TestContext = new Context(options.Options);
            Context.Migrate(TestContext);
            Context.Seed(TestContext);
        }
    }
}
