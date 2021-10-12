using FundooManager.Interface;
using FundooModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundooNode.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserManager manager;

        public UserController(IUserManager manager)
        {
            this.manager = manager;
        }

        [HttpPost]
        [Route("api/forgotpassword")]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
            
                var result = this.manager.ForgotPassword(email);
                if (result)
                {
             
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = "Check Your Mail" });
                }
                else
                {
                  
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = "Error !!Email Id Not found Or Incorrect" });
                }
            }
            catch (Exception ex)
            {
               
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }


        [HttpPost]
        [Route("api/register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            try
            {
                string resultMessage = this.manager.Register(user);
                if (resultMessage.Equals("Registration Successful"))
                {
                    return this.Ok(new ResponseModel<string>() { Status = true, Message = resultMessage });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = resultMessage });
                }

            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = true, Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/login")]
        public IActionResult Login([FromBody] Login userData)
        {
            try
            {
                string result = this.manager.Login(userData);
                if (result.Equals("Login Success"))
                { 
                     return this.Ok(new { Status = true, Message = result });
                }
                else
                {
                    return this.BadRequest(new ResponseModel<string>() { Status = false, Message = result });
                }
            }
            catch (Exception ex)
            {
                return this.NotFound(new ResponseModel<string>() { Status = false, Message = ex.Message });
            }
        }

    }
}
