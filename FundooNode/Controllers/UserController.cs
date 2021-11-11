// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------


namespace FundooNode.Controllers
{
    using FundooManager.Interface;
    using FundooModel;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Text;
    using Microsoft.Extensions.Configuration;
    using Microsoft.AspNetCore.DataProtection;
    using StackExchange.Redis;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// UserController class
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    public class UserController : ControllerBase
    {
        /// <summary>
        /// The manager
        /// </summary>
        private readonly IUserManager manager;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="logger">The logger.</param>
        public UserController(IUserManager manager, ILogger<UserController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        /// <summary>
        /// Forgot the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>email id is exists or not</returns>
        [HttpGet]
        [Route("api/forgotpassword")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                this.logger.LogInformation("Forgot Password method called!!!");
                var result = this.manager.ForgotPassword(email);
                if (result)
                {
                    this.logger.LogInformation($"{email} Got mail to Reset Password");
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Check Your Mail" });
                }
                else
                {
                    this.logger.LogInformation("Error ! Email Id Not found to Reset Password");
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Error !!Email Id Not found Or Incorrect" });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning("Some Error Occured while Changing Password");
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Registers the specified user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>successfully user register or not</returns>
        //[HttpPost]
        //[Route("api/register")]
        //public async Task<IActionResult> Register([FromBody] UserModel user)
        //{
        //    try
        //    {
        //        const string SessionName = "_Username";
        //        const string SessionEmail = "_Email";
        //        this.logger.LogInformation("Register method called!!!");
        //        string resultMessage = await this.manager.Register(user);
        //        if (resultMessage.Equals("Registration Successful"))
        //        {
        //            HttpContext.Session.SetString(SessionName, user.FirstName);
        //            HttpContext.Session.SetString(SessionEmail, user.Email);
        //            this.logger.LogInformation($"{user.FirstName} Registerd Succesfully");
        //            return this.Ok(new ResponseModel<string>() { Status = true, Message = resultMessage });
        //        }
        //        else
        //        {
        //            this.logger.LogInformation("Registration Failed");
        //            return this.BadRequest(new ResponseModel<string>() { Status = false, Message = resultMessage });
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        this.logger.LogWarning("Some Error Occured while Registration");
        //        return this.NotFound(new ResponseModel<string>() { Status = true, Message = ex.Message });
        //    }
        //}
        [HttpPost]
        [Route("api/register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            try
            {
                this.logger.LogInformation("Register method called!!!");
                string resultMessage = await this.manager.Register(user);
                if (resultMessage.Equals("Registration Successfull"))
                {
                    this.logger.LogInformation($"{user.FirstName} Registerd Succesfully");
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = resultMessage });
                }
                else
                {
                    this.logger.LogInformation("Registration Failed");
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = resultMessage });
                }

            }
            catch (Exception ex)
            {
                this.logger.LogWarning("Some Error Occured while Registration");
                return this.NotFound(new ResponseModel<string>() { Status = true, Message = ex.Message });
            }
        }

        /// <summary>
        /// Logins the specified user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>user logged in or not</returns>
        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody] Login userData)
        {
            try
            {
                this.logger.LogInformation("Login method called!!!");
                var result = this.manager.Login(userData);

                if (result.Message.Equals("Login Success"))
                {
                    //ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                    //IDatabase database = connectionMultiplexer.GetDatabase();
                    //result.Data.Password = null;
                    this.logger.LogInformation($"{userData.Email} Login Succesfully");
                    string Token = this.manager.GenerateToken(userData.Email);
                    return this.Ok(new { Status = true, Message = result.Message, Data = result.Data, Token });
                }
                this.logger.LogInformation("Login Failed");
                return this.BadRequest(new ResponseModel<UserModel>() { Status = true, Message = result.Message });
            }
            catch (Exception ex)
            {
                this.logger.LogWarning("Some Error Occured while Login");
                return this.NotFound(new { Status = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>password update or not</returns>
        [HttpPut]
        [Route("api/resetpassword")]
        public IActionResult ResetPassword([FromBody] UserCredentialModel userData)
        {
            try
            {
                this.logger.LogInformation("Reset Password method called!!!");
                bool result = this.manager.ResetPassword(userData);
                if (result)
                {
                    this.logger.LogInformation($"{userData.Email} Reset Password Succesfully");
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Password Succesfully Updated" });
                }
                else
                {
                    this.logger.LogInformation("Error ! Email Id Not found to Reset Password");
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Some Error Ocuured!!Try again" });
                }
            }
            catch (Exception ex)
            {
                this.logger.LogWarning("Some Error Occured while Resting Password");
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }
    }
}