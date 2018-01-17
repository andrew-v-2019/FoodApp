using Data.Models;
using ViewModels.Menu;

namespace Services.Interfaces
{
    public interface IMenuService
    {
        UpdateMenuViewModel UpdateMenu(UpdateMenuViewModel model);
        UpdateMenuViewModel GetLastMenu();
        UpdateMenuViewModel GetLastMenuAsTemplate();
        Menu GetActiveMenu();
    }
}
