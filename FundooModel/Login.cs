
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------


namespace FundooModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    /// <summary>
    /// Login class
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Enter your email")]

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
        [Required]

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

    }
}
