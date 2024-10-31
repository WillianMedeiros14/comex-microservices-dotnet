using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using StockService.Data;
using StockService.Data.Dtos;
using StockService.Models;
using StockService.ItemServiceHttpClient;
using StockService.RabbitMqClient;

namespace StockService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private ProductContext _context;
    private IMapper _mapper;

    private IOrderServiceHttpClient _orderServiceHttpClient;
    private IRabbitMqClient _rabbitMqClient;

    public ProductController(ProductContext context, IMapper mapper, IOrderServiceHttpClient orderServiceHttpClient, IRabbitMqClient rabbitMqClient)
    {
        _context = context;
        _mapper = mapper;
        _orderServiceHttpClient = orderServiceHttpClient;
        _rabbitMqClient = rabbitMqClient;
    }

    /// <summary>
    /// Adiciona um produto ao banco de dados
    /// </summary>
    /// <param name="productDto">Objeto com os campos necessários para criação de um produto</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    /// <response code="404">Caso a categoria não seja encontrada</response>

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddProduct([FromBody] CreateProductDto productDto)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == productDto.CategoryId);
        if (category == null)
        {
            return NotFound("Categoria não encontrada.");
        }

        Product product = _mapper.Map<Product>(productDto);
        _context.Products.Add(product);
        _context.SaveChanges();

        var productResponseDto = _mapper.Map<ProductResponseDto>(product);
        var readProductDto = _mapper.Map<ReadProductDto>(product);

        _orderServiceHttpClient.SendProductToOrderService(readProductDto);

        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, productResponseDto);
    }

    /// <summary>
    /// Buscar todos os produtos do banco de dados
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso encontre os produtos</response>

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


    /// <summary>
    /// Buscar um produto por id do banco de dados
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso encontre os produtos</response>
    /// <response code="404">Caso o produto não seja encontrado</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadProductDto))]
    public IActionResult GetProductById(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound();
        var productDto = _mapper.Map<ReadProductDto>(product);
        return Ok(productDto);
    }

    /// <summary>
    /// Atualizar um produto por id do banco de dados
    /// </summary>
    /// <param name="id">O ID do produto a ser atualizado</param>
    /// <param name="productDto">Objeto com os campos necessários para atualização de um produto</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso atualização seja feita com sucesso</response>
    /// <response code="404">Caso o produto não seja encontrado</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateProduct(int id, [FromBody] UpdateProductDto productDto)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound();
        _mapper.Map(productDto, product);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletar um produto por id do banco de dados
    /// </summary>
    /// <param name="id">O ID do produto a ser deletado</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso deleção seja feita com sucesso</response>
    /// <response code="404">Caso o produto não seja encontrado</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(product => product.Id == id);
        if (product == null) return NotFound();
        _context.Remove(product);
        _context.SaveChanges();
        return NoContent();
    }

}
