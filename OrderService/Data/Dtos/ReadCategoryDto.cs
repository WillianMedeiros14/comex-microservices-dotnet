using System.ComponentModel.DataAnnotations;
namespace OrderService.Data.Dtos
{
    public class ReadCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}