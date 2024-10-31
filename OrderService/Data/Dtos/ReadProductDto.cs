
namespace OrderService.Data.Dtos;

public class ReadProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string Description { get; set; }

    public float Price { get; set; }

    public int AvailableQuantity { get; set; }

    public int CategoryId { get; set; }

    public ReadCategoryDto Category { get; set; }
}