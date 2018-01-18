using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Data.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options) : base(options)
        {

        }

        public Context() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer("Data Source=.\\SQLEXPRESS;Integrated Security=True; Initial Catalog=food");
        }


        public DbSet<User> Users { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        public DbSet<MenuSection> MenuSections { get; set; }

        public DbSet<UserLunch> UserLunches { get; set; }

        public DbSet<UserLunchItem> UserLunchItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderUserLunch> OrderUserLunches { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelbuilder);
        }

        public static void Seed(Context context)
        {
            var menuSections = new List<MenuSection>
            {
                new MenuSection() {Name = "Салаты", Number = 1},
                new MenuSection() {Name = "Супы", Number = 2},
                new MenuSection() {Name = "Горячее ", Number = 3},
                new MenuSection() {Name = "Гарнир", Number = 4},
                new MenuSection() {Name = "Напитки", Number = 5}
            };
            foreach (var s in menuSections)
            {
                if (!context.MenuSections.Any(x => x.Name.Equals(s.Name)))
                {
                    context.Add(s);
                }
            }
            var configs = GetDefaultSettings();
            foreach (var сonfig in configs)
            {
                var configuration = new Configuration()
                {
                    Value = сonfig.Value,
                    Key = config.Key
                };
                context.Configurations.Add(configuration);
            }
            context.SaveChanges();
        }

        private static Dictionary<string, string> GetDefaultSettings()
        {
            var settings = new Dictionary<string, string>
            {
                ["Smtp.Host"] = "smtp.yandex.ru",
                ["Smtp.Port"] = "587",
                ["Smtp.EnableSsl"] = "True",
                ["Smtp.Password"] = "Kludgekludge1",
                ["Smtp.UserName"] = "vlasovandrei87@yandex.ru",
                ["OurEmail"] = "vlasovandrei87@yandex.ru",
                ["SupplierEmails"] = "andrew@sparkequation.com",
                ["DeveloperEmails"] = "vlasovandrei87@yandex.ru",
            };
            return settings;
        }

        public static void Migrate(Context context)
        {
            context.Database.Migrate();
        }
    }
}
