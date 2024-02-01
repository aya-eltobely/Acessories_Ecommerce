using System.ComponentModel.DataAnnotations;

namespace AccessoriesWebsite.Areas.Admin.ViewModels
{
    public class AddCatVM
    {
        [Required]
        public string Name { get; set; }
    }
}
