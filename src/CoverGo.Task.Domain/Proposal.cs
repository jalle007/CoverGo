namespace CoverGo.Task.Domain
{
    public class Proposal
    {
        public Guid Id { get;  set; }
        public Company ClientCompany { get; init; }  
        public List<InsuredGroup> InsuredGroups { get;  set; }
        public decimal TotalPremium => InsuredGroups.Sum(group => group.TotalPremium);

        public Proposal(Company clientCompany)
        {
            Id = Guid.NewGuid();
            ClientCompany = clientCompany ?? throw new ArgumentNullException(nameof(clientCompany));
            InsuredGroups = new List<InsuredGroup>();
        }

        public void AddInsuredGroup(InsuredGroup group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            InsuredGroups.Add(group);
        }

    }
}
