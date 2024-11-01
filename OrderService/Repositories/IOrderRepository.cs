
using OrderService.Models;

namespace OrderService.Repository
{
    public interface IOrderRepository
    {
        Task CreateOrder(Order order);
        Task SaveChangesAsync();
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrderById(int id);
    }
}
