using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooModel
{
   public class UserCredentialModel
    {
        public object email;

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
