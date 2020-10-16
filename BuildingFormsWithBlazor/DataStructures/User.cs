using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingFormsWithBlazor.DataStructures
{
    public class User
    {
    
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Boolean IsActive { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
    }
}
