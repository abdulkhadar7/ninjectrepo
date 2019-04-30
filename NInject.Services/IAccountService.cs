using NInject.Business.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NInject.Services
{
    public interface  IAccountService
    {
        string AddNewUser(AddNewUserModel model);
        bool verify(string password);
        LoginSuccessModel Login(LoginModel model);
    }
}
