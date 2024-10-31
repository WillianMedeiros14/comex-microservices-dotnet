using OrderService.Data.Dtos;


namespace OrderService.ItemServiceHttpClient
{
    public interface IStockServiceHttpClient
    {
        Task<List<ReadProductDto>> GetAllProducts(int skip = 0, int take = 50);
    }
}
