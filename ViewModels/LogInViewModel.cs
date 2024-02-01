using System.ComponentModel.DataAnnotations;

namespace AccessoriesWebsite.ViewModels
{
    public class LogInViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string UserName { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersisted { get; set; }
    }
}
