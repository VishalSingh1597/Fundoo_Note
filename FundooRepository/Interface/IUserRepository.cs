// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------

using FundooModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

    namespace FundooRepository.Interface
    {
    using FundooModel;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// IUserRepository interface
    /// </summary>
    public interface IUserRepository
        {

        /// <summary>
        /// Registers the specified user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>user register or not</returns>
        Task<string> Register(UserModel user);

        /// <summary>
        /// Logins the specified user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>login successful or not</returns>
        ResponseModel<UserModel> Login(Login userData);

        /// <summary>
        /// Forgot the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>mail to user email</returns>
        bool ForgotPassword(string email);

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>true if password updated </returns>
        bool ResetPassword(UserCredentialModel userData);

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>generate token</returns>

        string GenerateToken(string email);
        }
    }

