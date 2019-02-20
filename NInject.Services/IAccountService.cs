using NInject.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NInject.Services
{
    public interface IAccountService
    {
        IEnumerable<useraccountModel> GetAllUsers();
    }
}
