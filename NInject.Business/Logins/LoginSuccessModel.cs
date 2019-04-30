using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NInject.Business.Logins
{
    public class LoginSuccessModel
    {
        public string userId { get; set; }
        public string userName { get; set; }
        public List<string> Rolenames { get; set; }
    }
}
