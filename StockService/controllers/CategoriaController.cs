using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StockService.Data;
using StockService.Data.Dtos;
using StockService.Models;

namespace StockService.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private ProductContext _context;
    private IMapper _mapper;

    public CategoryController(ProductContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona uma categoria ao banco de dados
    /// </summary>
    /// <param name="categoryDto">Objeto com os campos necessários para criação de uma categoria</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateCategory([FromBody] CreateCategoryDto categoryDto)
    {
        Category category = _mapper.Map<Category>(categoryDto);
        _context.Categories.Add(category);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }

    /// <summary>
    /// Buscar todas as categorias do banco de dados
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso encontre as categorias</response>
    [HttpGet]
    public IEnumerable<ReadCategoryDto> GetAllCategories([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        return _context.Categories
            .OrderBy(c => c.Id)
            .Skip(skip)
            .Take(take)
            .ProjectTo<ReadCategoryDto>(_mapper.ConfigurationProvider)
            .ToList();
    }

    /// <summary>
    /// Buscar uma categoria por id do banco de dados
    /// </summary>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso encontre a categoria</response>
    /// <response code="404">Caso a categoria não seja encontrada</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadCategoryDto))]
    public IActionResult GetCategoryById(int id)
    {
        var category = _context.Categories.FirstOrDefault(category => category.Id == id);
        if (category == null) return NotFound();
        var categoryDto = _mapper.Map<ReadCategoryDto>(category);
        return Ok(categoryDto);
    }

    /// <summary>
    /// Atualizar uma categoria por id do banco de dados
    /// </summary>
    /// <param name="id">O ID da categoria a ser atualizada</param>
    /// <param name="categoryDto">Objeto com os campos necessários para atualização de uma categoria</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso atualização seja feita com sucesso</response>
    /// <response code="404">Caso a categoria não seja encontrada</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateCategory(int id, [FromBody] UpdateCategoryDto categoryDto)
    {
        var category = _context.Categories.FirstOrDefault(categoria => categoria.Id == id);
        if (category == null) return NotFound();
        _mapper.Map(categoryDto, category);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deletar uma categoria por id do banco de dados
    /// </summary>
    /// <param name="id">O ID da categoria a ser deletada</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso deleção seja feita com sucesso</response>
    /// <response code="404">Caso a categoria não seja encontrada</response>
    /// <response code="400">Caso a categoria não possa ser deletada porque está em uso</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.FirstOrDefault(category => category.Id == id);
        if (category == null) return NotFound();

        try
        {
            _context.Remove(category);
            _context.SaveChanges();
            return NoContent();
        }
        catch (DbUpdateException ex)
        {

            if (ex.InnerException != null && ex.InnerException.Message.Contains("FK_"))
            {
                return BadRequest("Não é possível deletar a categoria porque ela está em uso.");
            }
            throw;
        }
    }

}
