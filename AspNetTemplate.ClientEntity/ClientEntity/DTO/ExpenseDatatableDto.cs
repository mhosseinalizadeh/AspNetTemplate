using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AspNetTemplate.ClientEntity.DTO
{
    public class ExpenseDatatableDto
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string Description { get; set; }

        public string UploadDate { get; set; }

        public string State { get; set; }

        public string StateDescription { get; set; }

        public string Link { get; set; }
    }
}
