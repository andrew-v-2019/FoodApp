using System;
using Data.Models;
using ViewModels.Menu;

namespace Services.Interfaces
{
    public interface IMenuService
    {
        UpdateMenuViewModel UpdateMenu(UpdateMenuViewModel model);
        UpdateMenuViewModel GetActiveMenuForEdit();
        UpdateMenuViewModel GetLastMenuAsTemplate();
        Menu GetActiveMenu();
        void DisableMenu(int menuId);
        Menu GetLastMenu();
        bool CheckIfOrderForMenuSubmitted(int menuId);
        bool MenuForDateExisits(DateTime lunchDate, int menuId);
        bool CheckIfMenuIsEditable(int menuId);
        bool MenuIsActive(int menuId);
    }
}
