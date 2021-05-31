using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Models
{
    [Table("TB_M_Items")]
    public class Item
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(30, ErrorMessage = "Maksimal 30 karakter")]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual ICollection<RequestItem> RequestItems { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
    }
}
