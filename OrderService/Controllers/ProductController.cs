using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;

using OrderService.Data;
using OrderService.Data.Dtos;
using System.Text.Json;
using OrderService.ItemServiceHttpClient;

namespace OrderService.Controllers;

[ApiController]
[Route("api/order/[controller]")]
public class ProductController : ControllerBase
{
    private OrderContext _context;
    private IMapper _mapper;
    private readonly IStockServiceHttpClient _stockServiceHttpClient;


    public ProductController(OrderContext context, IMapper mapper, IStockServiceHttpClient stockServiceHttpClient = null)
    {
        _context = context;
        _mapper = mapper;
        _stockServiceHttpClient = stockServiceHttpClient;
    }

    [HttpPost]
    public ActionResult ReceiveProductFromProductService(ReadProductDto dto)
    {
        string jsonString = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(jsonString);
        return Ok();
    }


    [HttpGet]
    public async Task<IActionResult> GetAvailableProductsAsync([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var products = await _stockServiceHttpClient.GetAllProducts(skip, take);
        return Ok(products);
    }
}
