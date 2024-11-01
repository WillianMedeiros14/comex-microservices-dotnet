using System.Text.Json;
using AutoMapper;
using StockService.Data.Dtos.RabbitMq;

namespace StockService.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;

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
            }

            // if (!itemRepository.ExisteRestauranteExterno(restaurante.Id))
            // {
            //     itemRepository.CreateRestaurante(restaurante);
            //     itemRepository.SaveChanges();
            // }
        }
    }
}