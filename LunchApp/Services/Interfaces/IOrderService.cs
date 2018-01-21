using ViewModels.Order;
using ViewModels.User;
using ViewModels.UserLunch;

namespace Services.Interfaces
{
    public interface IOrderService
    {
        void AddUserLunchToOrder(UserLunchViewModel userLunch);
        OrderViewModel GetCurrentOrder();
        OrderViewModel UpdateOrder(OrderViewModel model);
        OrderViewModel SubmitOrder(OrderViewModel model, UserViewModel currentUser, string emailBody);
        bool IsOrderSubmitted(int orderId);
    }
}
