// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRepository.cs" company="BridgeLabs">
//   Copyright © 2021 Company="BridgeLabs"
// </copyright>
// <creator name="Vishal"/>
// ----------------------------------------------------------------------------------------------------------

namespace FundooRepository.Repository
{
    using FundooModel;
    using FundooRepository.Context;
    using FundooRepository.Interface;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Net.Mail;
    using Experimental.System.Messaging;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.Extensions.Configuration;
    using System.Threading.Tasks;
    using StackExchange.Redis;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// user repository class
    /// </summary>
    /// <seealso cref="FundooRepository.Interface.FundooRepository.Interface;" />

    public class UserRepository : IUserRepository
    {

        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration configuration;
        /// <summary>
        /// The user context
        /// </summary>
        private readonly UserContext userContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="configuration">The configuration.</param>

        public UserRepository(UserContext userContext, IConfiguration configuration)
        {
            this.userContext = userContext;
            this.configuration = configuration;
        }
        /// <summary>
        /// Encrypts the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>encrypted data</returns>

        public static string EncryptPassword(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error Password Encryption" + ex.Message);
            }
        }

        /// <summary>
        /// Registers the specified user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>registration is successful or not</returns>
        public async Task<string> Register(UserModel user)
        {
            try
            {
                var verifyEmail = this.userContext.Users.Where(x => x.Email.Equals(user.Email)).FirstOrDefault();
                if (verifyEmail == null)
                {
                    if (user != null)
                    {
                        user.Password = EncryptPassword(user.Password);
                        this.userContext.Users.Add(user);
                        await userContext.SaveChangesAsync();
                        return "Registration Successfull";
                    }

                    return "Registraion Unsuccessfull";
                }

                return "Email ID already exists";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //public async Task<string> Register(UserModel user)
        //{
        //    try
        //    {
        //        User newUser = new User()
        //        {
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            Email = user.Email,
        //            Password = EncryptPassword(user.Password)
        //        };

        //        if (user != null)
        //        {
        //            user.Password = EncryptPassword(user.Password);
        //            userContext.Users.Add(user);
        //            await userContext.SaveChangesAsync();
        //            return "Registration Successfull";
        //        }
        //        return "Registraion Unsuccessfull";
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Logins the specified user data.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>user email and password correct or wrong</returns> 
        public ResponseModel<UserModel> Login(Login userData)
        {

            try
            {
                string message = string.Empty;


                var emailCheck = this.userContext.Users.Where(x => x.Email.Equals(userData.Email)).FirstOrDefault();
                if (emailCheck != null)
                {
                    string encodedPassword = EncryptPassword(userData.Password);
                    var pass = this.userContext.Users.Where(x => x.Password.Equals(encodedPassword) && x.Email == userData.Email).FirstOrDefault();
                    if (pass != null)
                    {
                        //ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.Connect("127.0.0.1:6379");
                        //IDatabase database = connectionMultiplexer.GetDatabase();
                        //database.StringSet(key: "FirstName", emailCheck.FirstName);
                        //database.StringSet(key: "LastName", emailCheck.LastName);
                        //HttpContext.Session.SetString("mysession", "mysessionValue");
                        //HttpContext.Session.SetString("userId", user.UserId.ToString());
                        return new ResponseModel<UserModel>
                        {
                            Status = true,
                            Message = "Login Success",
                            Data = emailCheck,


                        };
                    }
                    return new ResponseModel<UserModel>
                    {
                        Status = false,
                        Message = "Invalid Password"
                    };
                }
                return new ResponseModel<UserModel>
                {
                    Status = false,
                    Message = "Email Does not Exist"
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Forgot the password.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>email id exists or not</returns>

        public bool ForgotPassword(string email)
        {
            try
            {
                var verifyEmail = this.userContext.Users.Where(x => x.Email.Equals(email)).FirstOrDefault();
                if (verifyEmail != null)
                {
                    string url = string.Empty;
                    this.SendToMSMQ("wwww.passwordreset.com");
                    bool result = this.SendEmail(email);
                    return result;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Sends to MSMQ.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>return true if message exists</returns>
        public bool SendToMSMQ(string url)
        {
            MessageQueue msqueue;

            try
            {
                if (MessageQueue.Exists(@".\Private$\MyQueue"))
                {
                    msqueue = new MessageQueue(@".\Private$\MyQueue");
                }
                else
                {
                    msqueue = MessageQueue.Create(@".\Private$\MyQueue");
                }

                Message message = new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = url;
                msqueue.Label = "url Link";
                msqueue.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>mail to client</returns>
        public bool SendEmail(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("vishalr23singh@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Reset your password";
                mail.Body = $"Click this link to reset your password\n{this.ReceiveMessage()}";
                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("vishalr23singh@gmail.com", "9930315160");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Receives the message.
        /// </summary>
        /// <returns>url to reset password</returns>
        public string ReceiveMessage()
        {
            try
            {
                var receiveQueue = new MessageQueue(@".\Private$\MyQueue");
                var receiveMsg = receiveQueue.Receive();
                receiveMsg.Formatter = new BinaryMessageFormatter();
                string linkToSend = receiveMsg.Body.ToString();
                return linkToSend;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns>password updated or not</returns>
        public bool ResetPassword(UserCredentialModel userData)
        {
            try
            {
                var userDetails = this.userContext.Users.Where(x => x.Email.Equals(userData.Email)).FirstOrDefault();
                if (userDetails != null)
                {
                    userDetails.Password = EncryptPassword(userData.Password);
                    this.userContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>generated token</returns>
        public string GenerateToken(string email)
        {
            byte[] key = Encoding.UTF8.GetBytes(this.configuration["SecretKey:Key"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, email)
            }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(token);
        }

    }

}


