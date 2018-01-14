using ViewModels.Order;
using ViewModels.UserLunch;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        void AddUserLunchToOrder(UserLunchViewModel userLunch);
        OrderViewModel GetCurrentOrder();
        OrderViewModel UpdateOrder(OrderViewModel model);
    }
}
