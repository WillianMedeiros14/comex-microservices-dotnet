using System.Text.Json;
using AutoMapper;
using StockService.Data;
using StockService.Data.Dtos.RabbitMq;

namespace StockService.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {

        private ProductContext _context;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory, ProductContext context)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
            _context = context;
        }

        public void Processa(string mensagem)
        {
            using var scope = _scopeFactory.CreateScope();
            var updateProductQuantityInStockDtoList = JsonSerializer.Deserialize<IList<UpdateProductQuantityInStockDto>>(mensagem);

            foreach (var updateProductQuantityInStockDto in updateProductQuantityInStockDtoList)
            {
                Console.WriteLine("Chegou a mensagem");
                Console.WriteLine("Produto: " + updateProductQuantityInStockDto.ProductId);
                Console.WriteLine("Quantidade: " + updateProductQuantityInStockDto.Amount);
                Console.WriteLine("IdPedido: " + updateProductQuantityInStockDto.OrderId);

                var produto = _context.Products.SingleOrDefault(p => p.Id == updateProductQuantityInStockDto.ProductId);

                if (produto != null)
                {
                    produto.AvailableQuantity -= updateProductQuantityInStockDto.Amount;
                   
                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Produto não encontrado: " + updateProductQuantityInStockDto.ProductId);
                }
            }

            // if (!itemRepository.ExisteRestauranteExterno(restaurante.Id))
            // {
            //     itemRepository.CreateRestaurante(restaurante);
            //     itemRepository.SaveChanges();
            // }
        }
    }
}