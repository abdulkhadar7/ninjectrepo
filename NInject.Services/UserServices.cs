using NInject.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NInject.Services
{
    public class UserServices : IUserService
    {
        private readonly UnitOfWork _unitOfWork;

        public UserServices(UnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }

        public int Authenticate(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.UserName == userName && u.PassKey == password);
            if (user != null && user.UserId > 0)
                return user.UserId;
            return 0;
        }
    }
}
