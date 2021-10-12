using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FundooModel
{
    class ForgetPassword
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Url { get; set; }
    }

}

