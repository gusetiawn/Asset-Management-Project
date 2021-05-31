using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagementAPI.ViewModel
{
    public class RegisterVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GenderId { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }
}
