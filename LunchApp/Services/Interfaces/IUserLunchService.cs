using ViewModels.UserLunch;
namespace Services.Interfaces
{
    public interface IUserLunchService
    {
        UserLunchViewModel GetCurrentLunch(int userId);
    }
}
