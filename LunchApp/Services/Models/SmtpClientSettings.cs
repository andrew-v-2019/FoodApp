
using System.Net.Mail;

namespace Services.Models
{
    public class SmtpClientSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public SmtpDeliveryMethod SmtpDeliveryMethod { get; set; }
        public bool EnableSsl { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
