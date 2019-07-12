using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AspNetTemplate.ClientEntity.DTO
{
    public class ExpenseUploadDto
    {

        public string Description { get; set; }

        [Required(ErrorMessage = "An expanse photo is required.")]
        public IFormFile ExpensePhoto { get; set; }
        public int UserId { get; set; }
    }
}
