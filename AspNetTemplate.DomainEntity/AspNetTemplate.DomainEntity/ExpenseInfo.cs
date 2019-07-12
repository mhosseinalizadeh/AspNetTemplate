using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.DomainEntity
{
    public class ExpenseInfo
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public int OwnerId { get; set; }
        public ExpenseState State { get; set; }
        public string Description { get; set; }
        public string StateDescription { get; set; }
        public DateTime UploadDate { get; set; }
 
        public User Owner { get; set; }
    }

    public enum ExpenseState {
        UnApproved,
        Approved
    }
}
