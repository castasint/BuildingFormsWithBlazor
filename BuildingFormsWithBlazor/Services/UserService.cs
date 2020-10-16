using BuildingFormsWithBlazor.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingFormsWithBlazor.Services
{
    public static class UserService
    {

        private static List<User> Users;
        static UserService()
        {

            Users = new List<User>();
            Users.Add(new User { FirstName = "John", LastName = "Doe", City = "London", Email = "johndoe@testemail.com", IsActive = true, DateOfBirth = new DateTime(1975, 01, 03) });
            Users.Add(new User { FirstName = "Alex", LastName = "Hales", City = "NewYork", Email = "alexhales@googlemail.com", IsActive = true, DateOfBirth = new DateTime(1955, 06, 11) });
            Users.Add(new User { FirstName = "Justin", LastName = "Li", City = "Shanghai", Email = "justinli@yahoo.com", IsActive = true, DateOfBirth = new DateTime(1983, 09, 04) });
            Users.Add(new User { FirstName = "Doug", LastName = "Lee", City = "Sydney", Email = "douglee@outlook.com", IsActive = false, DateOfBirth = new DateTime(2000, 07, 07) });
            Users.Add(new User { FirstName = "Rahul", LastName = "Mittal", City = "NewDelhi", Email = "rmittal@rediff.com", IsActive = true, DateOfBirth = new DateTime(1996, 11, 11) });

        }

        public static List<User> GetUsers()
        { return Users;
        }

        public static Boolean AddUser(User user)
        {
            Users.Add(user);
            return true;
        }
    }
}
