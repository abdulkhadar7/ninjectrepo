using NInject.Business.Logins;
using NInject.Data;
using NInject.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NInject.Services
{
    public class AccountService:IAccountService
    {
        private readonly UnitOfWork _unitOfWork;
        public AccountService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string AddNewUser(AddNewUserModel model)
        {
            string userId = string.Empty;
            string hashstring = null;
            string saltString = null;
            GenerateSaltedHash(model.Password, out hashstring, out saltString);
           
            
            using (var scope = new TransactionScope())
            {
                AspNetUsers user = new AspNetUsers
                {
                    Id = new Guid().ToString(),
                    AccessFailedCount = 0,
                    Email = model.Email,
                    EmailConfirmed = false,
                    LockoutEnabled = false,
                    LockoutEndDateUtc = null,
                    PasswordHash = hashstring,
                    PasswordSalt = saltString,
                    PhoneNumber = model.PhoneNumber,
                    PhoneNumberConfirmed = false,
                    SecurityStamp = null,
                    TwoFactorEnabled = false,
                    UserName = model.UserName
                };

                _unitOfWork.AspNetRepository.Insert(user);
                _unitOfWork.Save();
                scope.Complete();
                userId = user.Id;
            }
            return userId;
        }
        private void GenerateSaltedHash(string password,out string hash,out string salt)
        {
            var saltBytes = new byte[64];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            hash = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));
        }

        private bool VerifyPassword(string reqPassword,string storedHash,string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfcDerivedBytes = new Rfc2898DeriveBytes(reqPassword, saltBytes, 10000);
            bool status = Convert.ToBase64String(rfcDerivedBytes.GetBytes(256)) == storedHash;
            return status;
        }
    }
}
