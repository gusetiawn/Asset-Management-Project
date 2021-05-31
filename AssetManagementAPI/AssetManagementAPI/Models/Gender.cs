using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Models
{
    [Table("TB_M_Genders")]
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong")]
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}
