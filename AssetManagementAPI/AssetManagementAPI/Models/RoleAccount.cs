using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Models
{
    [Table("TB_T_RoleAccounts")]
    public class RoleAccount
    {
        public string AccountId { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Account Account { get; set; }
    }
}
