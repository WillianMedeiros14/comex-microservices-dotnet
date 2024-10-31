using StockService.Data.Dtos;

namespace StockService.RabbitMqClient
{
    public interface IRabbitMqClient
    {
        void PublishProduct(ReadProductDto readProductDto);
    }
}