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

namespace FundooRepository.Repository
{
    public class UserRepository : IUserRepository
    {
        private IConfiguration configuration;
        private readonly UserContext userContext;
      

        public UserRepository(UserContext userContext, IConfiguration configuration)
        {
            this.userContext = userContext;
            this.configuration = configuration;
        }

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



        public bool ForgotPassword(string email)
        {
            try
            {
                var verifyEmail = this.userContext.Users.Where(x => x.Email.Equals(email)).FirstOrDefault();

                if (verifyEmail != null)
                {
                    this.SendToMSMQ("www.gmail.com");
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
        public bool SendEmail(string email)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("Vishalr23singh@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Reset your password";
                mail.Body = $"Click this link to reset your password\n{this.ReceiveMessage()}";
                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential("Vishalr23singh@gmail.com", "9930315160");
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
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
        

      
        public string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(this.configuration["SecretKey:Key"]);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, username)
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


