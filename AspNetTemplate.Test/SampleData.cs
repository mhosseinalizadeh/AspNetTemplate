using AspNetTemplate.ClientEntity.DTO;
using AspNetTemplate.DomainEntity;
using System.Collections.Generic;
using System.Linq;

namespace AspNetTemplate.Test
{
    public class SampleData
    {
        public Role EmployeeRole = new Role() { Id = 1, Title = "Employee" };
        public Role TeamLeadRole = new Role() { Id = 2, Title = "Team Lead" };
        public Role financeRole = new Role() { Id = 3, Title = "Finance" };
        public LoginDto LoginDto = new LoginDto()
        {
            Email = "sam@test.com",
            Password = "123456"
        };
        public User SamUser = new User()
        {
            Id = 1,
            Email = "sam@test.com",
            FirstName = "Sam",
            LastName = "Doe",
            Password = "1234567",
            Roles = new List<Role>(){
                        new Role() { Id = 2, Title = "Team Lead" }
            }
        };
        public IEnumerable<User> AllUsers
        {
            get
            {
                return new List<User>() {
                    
                        SamUser,
                    new User(){
                        Id = 2,
                        Email = "john@test.com",
                        FirstName = "John",
                        LastName = "Doe",
                        Password = "john@test.com",
                        Roles = new List<Role>(){
                            EmployeeRole
                        }
                    }
                }.AsEnumerable();
            }
        }
    }
}
