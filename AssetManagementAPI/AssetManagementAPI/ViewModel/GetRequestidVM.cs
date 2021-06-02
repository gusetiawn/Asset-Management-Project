using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.ViewModel
{
    public class GetRequestIdVM
    {
        public int Id { get; set; }
        [Required]
        public int ItemId { get; set; }
        [Required]
        public string AccountId { get; set; }
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
        [Required]
        public int StatusId { get; set; }
        [Required, MaxLength(30, ErrorMessage = "Maksimal 30 karakter")]
        public string NameItem { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string NameStatus { get; set; }
    }
}
