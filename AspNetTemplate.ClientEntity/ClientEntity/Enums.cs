using System.ComponentModel;

namespace AspNetTemplate.ClientEntity
{

    public class Enums
    {
        public enum NotifyType
        {
            AddExpense,
            DeclineExpense,
            ApproveExpense
        }

        public enum State
        {
            [Description("Added")]
            Added,
            [Description("Approved")]
            Approved,
            [Description("Declined")]
            Declined
        }

        public enum Role {
            [Description("Employee")]
            Employee = 1,
            [Description("TeamLead")]
            TeamLead = 2,
            [Description("Finance")]
            Finance = 3,

        }
    }
}
