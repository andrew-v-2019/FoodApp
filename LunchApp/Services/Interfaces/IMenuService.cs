using ViewModels;
using ViewModels.Menu;

namespace Services.Interfaces
{
    public interface IMenuService
    {
        UpdateMenuViewModel UpdateMenu(UpdateMenuViewModel model);
        UpdateMenuViewModel GetEmptyMenu();
        UpdateMenuViewModel GetLastMenu();
        UpdateMenuViewModel GetFakeMenu();
        UpdateMenuViewModel GetLastMenuAsTemplate();
    }
}
