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

namespace FundooRepository.Repository
{
   public class UserRepository : IUserRepository
    {
        private readonly UserContext userContext;

        public UserRepository(UserContext userContext)
        {
            this.userContext = userContext;
        }
  
        public string Register(UserModel user)
        {
            try
            {
              
       
                //   string encryptedPassword = StringCipher.Encrypt(user.Password);
                //UserModel users = new UserModel()
                //    {
                //        FirstName = user.FirstName,
                //        LastName = user.LastName,
                //        Email = user.Email,
                //        Password = encryptedPassword

                //    };

                //    userContext.Users.Add(users);
                //    userContext.SaveChanges();
                
                var email = this.userContext.Users.Where(x => x.Email.Equals(user.Email)).FirstOrDefault();
                if (email == null)
                {
                    if (user != null)
                    {

                        this.userContext.Users.Add(user);
                        this.userContext.SaveChanges();
                        return "Registration Successful";
                    }
                    return "Registraion Unsuccessfull";
                }
                return "Email id is already present";
            }

            catch (ArgumentNullException ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Login(Login userData)
        {
            try
            {
                string message = string.Empty;
                //  var email = this.userContext.Users.Where(x => x.Email.Equals(userData.Email)).FirstOrDefault();
                // var pass = this.userContext.Users.Where(x => x.Password.Equals(userData.Password)).FirstOrDefault();
                var login = this.userContext.Users.Where(x => x.Email.Equals(userData.Email) && x.Password.Equals(userData.Password)).FirstOrDefault();
                if (login !=null)
                {
                    message = "login successful";
                }
                else
                {
                    message = "Login failed!! please enter enter both fields...";
                }

                return message;
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
                Console.WriteLine("jsjsjsj");
                 var verifyEmail = this.userContext.Users.Where(x => x.Email.Equals(email)).FirstOrDefault();
        
                 if (verifyEmail != null)
                 {
                     string url = string.Empty;
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
                 mail.From = new MailAddress("yc321998@gmail.com");
                 mail.To.Add(email);
                 mail.Subject = "Reset your password";
                 mail.Body = $"Click this link to reset your password\n{this.ReceiveMessage()}";
                 smtpServer.Port = 587;
                 smtpServer.Credentials = new NetworkCredential("yc321998@gmail.com", "Smartybhaijan1");
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
 
    }

}
