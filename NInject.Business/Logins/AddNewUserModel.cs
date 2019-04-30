using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NInject.Business.Logins
{
    public  class AddNewUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }        
        public string UserName { get; set; }
        public string PasswordSalt { get; set; }
    }
}
