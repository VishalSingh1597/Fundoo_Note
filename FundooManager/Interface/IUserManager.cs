using FundooModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooManager.Interface
{
    public interface IUserManager
    { 
        Task<string> Register(UserModel user);
      
        bool ForgotPassword(string email);
        bool ResetPassword(UserCredentialModel userData);
       ResponseModel<UserModel> Login(Login userData);
        string GenerateToken(string email);
    }
}
