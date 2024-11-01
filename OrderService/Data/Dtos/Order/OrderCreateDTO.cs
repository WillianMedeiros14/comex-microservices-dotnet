using OrderService.Enums;

namespace OrderService.DTOs.Order
{
    public class OrderCreateDTO
    {
        public OrderStatus Status { get; set; } = OrderStatus.Pendente;
        public ICollection<OrderItemCreateDTO> OrderItems { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
