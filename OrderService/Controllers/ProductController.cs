using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;

using OrderService.Data;
using OrderService.Data.Dtos;
using System.Text.Json;

namespace OrderService.Controllers;

[ApiController]
[Route("api/order/[controller]")]
public class ProductController : ControllerBase
{
    private OrderContext _context;
    private IMapper _mapper;

    public ProductController(OrderContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    [HttpPost]
    public ActionResult ReceiveProductFromProductService(ReadProductDto dto)
    {
        string jsonString = JsonSerializer.Serialize(dto, new JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(jsonString);
        return Ok();
    }

    
    [HttpGet]
    public IEnumerable<ReadProductDto> GetAllProducts([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Products
            .OrderBy(p => p.Id)
            .Skip(skip)
            .Take(take)
            .ProjectTo<ReadProductDto>(_mapper.ConfigurationProvider)
            .ToList();
    }
}
