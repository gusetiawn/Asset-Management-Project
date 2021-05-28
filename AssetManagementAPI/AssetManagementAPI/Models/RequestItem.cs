using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.Models
{
    [Table("TB_T_RequestItems")]
    public class RequestItem
    {
        [Key]
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Tidak boleh kosong"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime EndDate { get; set; }
        [Required]
        public string Notes { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Account Account { get; set; }
        public virtual Item Item { get; set; }
        public virtual ReturnItem ReturnItem { get; set; }
        public virtual Status Status { get; set; }
    }
}
