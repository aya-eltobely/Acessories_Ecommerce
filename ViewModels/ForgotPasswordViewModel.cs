﻿using System.ComponentModel.DataAnnotations;

namespace AccessoriesWebsite.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
