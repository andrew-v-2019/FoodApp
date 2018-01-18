using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IEmailService
    {
        bool SendEmial(string from, List<string> to, string subject, string body);
    }
}
