using System.ComponentModel.DataAnnotations;

namespace AccessoriesWebsite.ViewModels
{
    public class ProductVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
 
        public string? ImageUrl { get; set; }
        [Required]
        public int InStock { get; set; }
        [Required]
        public int? categoryId { get; set; }
    }
}
