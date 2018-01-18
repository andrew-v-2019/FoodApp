using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Services.Interfaces;
using Services.Models;

namespace Services
{
    public class ConfigurationsProvider : IConfigurationsProvider
    {
        private readonly Context _context;

        public ConfigurationsProvider(Context context)
        {
            _context = context;
        }

        public List<string> GetToEmails()
        {
            var val = GetValue("SupplierEmails");
            var vals = val.Split(',').ToList();
            return vals;
        }

        public List<string> GetDevEmails()
        {
            var val = GetValue("DeveloperEmails");
            var vals = val.Split(',').ToList();
            return vals;
        }

        public string GetFromEmail()
        {
            var val = GetValue("OurEmail");
            return val;
        }

        private string GetValue(string key, string defaultVal = "")
        {
            var val = _context.Configurations.FirstOrDefault(c => c.Key.ToLower().Contains(key.ToLower()));
            return val != null ? val.Value : defaultVal;
        }

        public SmtpClientSettings GetSmtpSettings()
        {
            var settings = new SmtpClientSettings
            {
                Host = GetValue("Smtp.Host", "smtp.yandex.ru"),
                Port = Convert.ToInt32(GetValue("Smtp.Port", "587")),
                EnableSsl = Convert.ToBoolean(GetValue("Smtp.EnableSsl", "True")),
                Password = GetValue("Smtp.Password", "Kludgekludge1"),
                UserName = GetValue("Smtp.UserName", "vlasovandrei87@yandex.ru")
            };
            return settings;
        }
    }
}
