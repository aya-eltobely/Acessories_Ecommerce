using System.ComponentModel.DataAnnotations;

namespace AccessoriesWebsite.ViewModels
{
    public class UserDataOrderVM
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]

        public string LastName { get; set; }


        //[Required]
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }


        [Required]
        public string Address { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]

        public string Country { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$")]

        public string State { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$")]

        public int Zip { get; set; }
    }
}
