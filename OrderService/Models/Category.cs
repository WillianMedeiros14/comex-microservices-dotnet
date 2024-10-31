using System.ComponentModel.DataAnnotations;

namespace OrderService.Models;

public class Category
{
    [Key]
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "O campo de nome é obrigatório.")]
    public string Name { get; set; }

    public virtual ICollection<Product> Products { get; set; }
}
