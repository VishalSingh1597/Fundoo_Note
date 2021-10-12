using FundooManager.Interface;
using FundooModel;
using FundooRepository.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace FundooManager.Manager
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repository;
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }
        public string Register (UserModel user)
        {
            try
            {
                return this.repository.Register(user);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Login(Login userData)
        {

            try
            {
                return this.repository.Login(userData);
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
                return this.repository.ForgotPassword(email);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
