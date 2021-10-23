using FundooModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FundooRepository.Interface
{
    public interface IUserRepository
    {

        Task<string> Register(UserModel user);
        ResponseModel<UserModel> Login(Login userData);
       bool ForgotPassword(string email);
        bool ResetPassword(UserCredentialModel userData);
        
        string GenerateToken(string email);
    }
}
