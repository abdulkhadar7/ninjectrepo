using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NInject.Data
{
    [Table("useraccount")]
    public class useraccount
    {
        [Key]
        public long UserID { get; set; }

        public long client_group_id { get; set; }

        public long PeopleID { get; set; }

        public string ProfileName { get; set; }

        public byte[] UserName { get; set; }

        public byte[] Password { get; set; }

        public int? InvalidAttempts { get; set; }

        public bool? IsActive { get; set; }

        public bool? Locked { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? LastSuccessfullLogin { get; set; }

        public DateTime? LastTry { get; set; }

        public DateTime? LastResetOn { get; set; }

        public DateTime? LastLockoutOn { get; set; }

        public string ResetReferenceID { get; set; }

        public DateTime? ResetRequestOn { get; set; }

        public int? ResetCount { get; set; }

        public int? NoOfLockOuts { get; set; }

        public byte[] Email { get; set; }

        public byte[] Phone { get; set; }

        public string FacebookID { get; set; }

        public string GoogleID { get; set; }

        public bool? FacebookActive { get; set; }

        public bool? GoogleActive { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string RequestToken { get; set; }

        public DateTime? LastAccessedOn { get; set; }

        public bool? IsGuest { get; set; }

        public byte[] FirstName { get; set; }

        public byte[] LastName { get; set; }

        public string master_client_code { get; set; }

        public string temp_password { get; set; }

        public byte[] temp_password_bin { get; set; }

        public string AccountNo { get; set; }

        public byte[] MHOUserName { get; set; }

    }

}
