using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JR.FinancialManager.Core.Models.Dto
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResultDto
    {
        public string Token { get; set; }
        
        public string Roles { get; set; }
    }
}