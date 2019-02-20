using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NInject.Business;
using NInject.Data;
using NInject.Data.UnitOfWork;
using AutoMapper;

namespace NInject.Services
{
    public class AccountService : IAccountService
    {
        private readonly UnitOfWork _unitOfWork;
        public AccountService(UnitOfWork unitofWork)
        {
            _unitOfWork = unitofWork;
        }
        public IEnumerable<useraccountModel> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll().ToList();
            if(users.Any())
            {
                var config = new MapperConfiguration(cfg =>
                  {
                      cfg.CreateMap<useraccount, useraccountModel>();
                  });
                IMapper mapper = config.CreateMapper();
                var usermodel = mapper.Map<List<useraccount>, List<useraccountModel>>(users);
                return usermodel;
            }
            return null;
        }
    }
}
