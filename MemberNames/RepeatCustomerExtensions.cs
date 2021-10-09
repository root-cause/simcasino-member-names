using Appearance.Agents;

namespace MemberNames
{
    public static class RepeatCustomerExtensions
    {
        public static void RandomizeName(this RepeatCustomer customer, Gender gender)
        {
            customer.Name = NameUtility.GetName(gender);
        }
    }
}
