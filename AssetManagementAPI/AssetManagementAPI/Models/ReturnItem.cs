using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Models
{
    [Table("TB_T_ReturnItems")]
    public class ReturnItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int RequestItemId { get; set; }
        [Required]
        public string Penalty { get; set; }
        [Required]
        public string Notes { get; set; }
        public virtual RequestItem RequestItem { get; set; }
    }
}
