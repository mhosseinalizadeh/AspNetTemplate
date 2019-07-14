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
    }
}
