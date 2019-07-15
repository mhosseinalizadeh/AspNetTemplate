using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AspNetTemplate.ClientEntity.Enums;

namespace AspNetTemplate.ClientEntity.DTO
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please choose a password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please choose a role.")]
        public Role Role { get; set; }
    }
}
