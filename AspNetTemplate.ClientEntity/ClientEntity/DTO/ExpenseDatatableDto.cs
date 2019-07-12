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

        [SearchableString]
        [Sortable]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [SearchableString]
        [Sortable]
        [DisplayName("Description")]
        public string Description { get; set; }

        [SearchableDateTime]
        [Sortable(Default = true)]
        [DisplayName("Start Date")]
        public DateTime UploadDate { get; set; }

        [Sortable]
        [DisplayName("State")]
        public string State { get; set; }

        [SearchableString]
        [DisplayName("State Description")]
        public string StateDescription { get; set; }

        [DisplayName("File")]
        public string File { get; set; }
    }
}
