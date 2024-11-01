using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data.Dtos.Product;
using OrderService.DTOs.Order;
using OrderService.ItemServiceHttpClient;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStockServiceHttpClient _stockServiceHttpClient;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IStockServiceHttpClient stockServiceHttpClient, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _stockServiceHttpClient = stockServiceHttpClient;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDTO>> CreateOrder(OrderCreateDTO orderCreateDto)
        {
            Order order = new Order
            {
                CreationDate = orderCreateDto.CreationDate,
                Status = orderCreateDto.Status,
                OrderItems = new List<OrderItem>()
            };


            foreach (var itemDto in orderCreateDto.OrderItems)
            {
                ReadProductDto product = await _stockServiceHttpClient.GetProductById(itemDto.ProductId);

                if (product == null)
                {
                    return NotFound($"Produto com ID {itemDto.ProductId} não encontrado.");
                }

                if (product.AvailableQuantity < itemDto.Amount)
                {
                    return BadRequest($"Estoque insuficiente para o produto {product.Name}.");
                }

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Amount = itemDto.Amount,
                    UnitPrice = product.Price
                });
            }


            order.Total = order.OrderItems.Sum(i => i.Total);

            await _orderRepository.CreateOrder(order);
            await _orderRepository.SaveChangesAsync();

            var orderReadDto = _mapper.Map<OrderReadDTO>(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderReadDto.Id }, orderReadDto);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderReadDTO>> GetOrderById(int id)
        {
            Order order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            var orderReadDto = _mapper.Map<OrderReadDTO>(order);
            return Ok(orderReadDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderReadDTO>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllOrders();

            if (orders == null || !orders.Any())
            {
                return NotFound("Nenhum pedido encontrado.");
            }

            var orderReadDtos = _mapper.Map<IEnumerable<OrderReadDTO>>(orders);

            return Ok(orderReadDtos);
        }
    }
}
