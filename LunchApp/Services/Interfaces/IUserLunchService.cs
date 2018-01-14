using System.Collections.Generic;
using ViewModels.UserLunch;
namespace Services.Interfaces
{
    public interface IUserLunchService
    {
        UserLunchViewModel GetCurrentLunch(int userId);
        UserLunchViewModel UpdateUserLunch(UserLunchViewModel model);
        List<UserLunchViewModel> GetCurrentLunches();
        void AdjustUserLunchesWithList(List<UserLunchViewModel> model, int menuId);
        void LockLunches(List<int> userLunchIds);
    }
}
