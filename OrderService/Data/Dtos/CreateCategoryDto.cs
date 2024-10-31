using System.ComponentModel.DataAnnotations;
namespace OrderService.Data.Dtos
{
    public class CreateCategoryDto
    {
        [Required(ErrorMessage = "O campo de nome é obrigatório.")]
        public string Name { get; set; }
    }
}