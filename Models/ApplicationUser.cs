using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AccessoriesWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
      
        public string? FirstName { get; set; }

      

        public string? LastName { get; set; }



        public string? Address { get; set; }



        public string? Country { get; set; }

 

        public string? State { get; set; }



        public int? Zip { get; set; } 

        public virtual List<Order> orders { get; set; }
        public virtual Cart cart { get; set; }
    }
}
