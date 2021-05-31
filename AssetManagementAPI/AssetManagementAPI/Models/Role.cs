using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Models
{
    [Table("TB_M_Roles")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Maksimal 30 karakter")]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }
    }
}
