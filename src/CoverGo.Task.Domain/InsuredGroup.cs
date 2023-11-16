namespace CoverGo.Task.Domain
{
    public enum PlanType
    {
        Base,
        Premium
    }

    public class InsuredGroup
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public PlanType PlanType { get; init; }
        public int NumberOfMembers { get; private set; }
        public decimal PremiumPerMember { get; private set; }
        public decimal TotalPremium { get; private set; }

        public InsuredGroup(PlanType planType, int numberOfMembers, decimal premiumPerMember)
        {
            PlanType = planType;
            NumberOfMembers = numberOfMembers;
            PremiumPerMember = premiumPerMember;
            TotalPremium = NumberOfMembers * PremiumPerMember;
        }

        public void ApplyDiscount(int freeMembers)
        {
            var discount = freeMembers * PremiumPerMember;
            TotalPremium -= discount;
        }
    }

}
