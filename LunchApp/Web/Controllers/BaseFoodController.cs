using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.User;
using Services.Extensions;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    public class BaseFoodController : Controller
    {
        private readonly IUserService _userService;

       
        public BaseFoodController(IUserService userService)
        {
            _userService = userService;
        }

        protected UserViewModel GetCurrentUser()
        {
            var ip = GetUserIp();
            var compName = DetermineCompName(ip);
            var user = _userService.GetByCompName(compName, ip);
            return user;

        }

        private string GetUserIp()
        {
            if (Request.IsLocal())
            {
                var ipAddress = Request.HttpContext.Connection.LocalIpAddress.ToString();
                return ipAddress;
            }
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return remoteIpAddress;
        }

        public static string DetermineCompName(string ip)
        {
            var myIp = IPAddress.Parse(ip);
            var getIpHost = Dns.GetHostEntry(myIp);
            var compName = getIpHost.HostName.Split('.').ToList();
            return compName.First();
        }
    }
}
