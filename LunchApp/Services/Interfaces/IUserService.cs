
using ViewModels.User;

namespace Services.Interfaces
{
    public interface IUserService
    {
        UserViewModel UpdateUser(UserViewModel model);
        UserViewModel GetByCompName(string compName, string ip);
    }
}
