using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Models
{
    [Table("TB_M_Accounts")]
    public class Account
    {
        [Key, Required(ErrorMessage = "Tidak boleh kosong")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong"), DataType(DataType.Password), MinLength(8, ErrorMessage = "Minimal 8 karakter")]
        public string Password { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
        [JsonIgnore]
        public virtual ICollection<RequestItem> RequestItems { get; set; }
    }
}
