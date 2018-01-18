using System.Collections.Generic;
using Services.Models;

namespace Services.Interfaces
{
    public interface IConfigurationsProvider
    {
        SmtpClientSettings GetSmtpSettings();
        string GetFromEmail();
        List<string> GetToEmails();
    }
}
