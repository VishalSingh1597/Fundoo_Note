using FundooModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Interface
{
    public interface IUserManager
    { 
        string Register(UserModel user);
         string Login(Login userData);
        bool ForgotPassword(string email);
    }
}
