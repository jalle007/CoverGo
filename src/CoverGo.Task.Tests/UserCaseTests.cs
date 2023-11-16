using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Application.Services;
using CoverGo.Task.Domain;
using Moq;

namespace CoverGo.Task.Tests
{
    public class UserCaseTests
    {
        private Mock<IProposalsQuery> _mockProposalsQuery;
        private Mock<IProposalsWriteRepository> _mockProposalsWriteRepository;
        private ProposalService _proposalService;

        public UserCaseTests()
        {
            // Arrange
            _mockProposalsQuery = new Mock<IProposalsQuery>();
            _mockProposalsWriteRepository = new Mock<IProposalsWriteRepository>();
            _proposalService = new ProposalService(_mockProposalsQuery.Object, _mockProposalsWriteRepository.Object);
        }

        /// <summary>
        /// User_Case_1
        /// As an insurance company admin
        /// I want to create proposal for the client company
        /// So that I can send it to client later for approval
        /// There are 3 clients in a system: ABC, CoverGo, Horns & Hooves
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async ValueTask User_Case_1_CreateProposal_ForNewClient_ShouldCreateProposal()
        {
            // Arrange
            var clientCompany = new Company("ABC"); // Assuming Company is the client company entity
            var expectedProposal = new Proposal(clientCompany);

            _mockProposalsQuery
                .Setup(query => query.GetById(expectedProposal.Id.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Proposal?)null);

            _mockProposalsWriteRepository
                .Setup(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProposal);

            // Act
            var createdProposal = await _proposalService.CreateOrUpdate(expectedProposal, CancellationToken.None);

            // Assert
            Assert.NotNull(createdProposal);
            Assert.Equal(clientCompany.Name, createdProposal.ClientCompany.Name);
            _mockProposalsWriteRepository.Verify(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// User_Case_2
        /// As an insurance company admin
        /// I want to add insured group to proposal with N employees of specific plan
        /// So that I can send it to client later
        /// One proposal can have multiple insured groups.Insured group has an amount of members and their plan.
        /// There are two plans available: Base for 500$ and Premium for 750
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async ValueTask User_Case_2_AddInsuredGroupToProposal_ShouldAddGroup()
        {
            // Arrange
            var clientCompany = new Company("CoverGo");
            var proposalId = Guid.NewGuid();
            var proposal = new Proposal(clientCompany) { Id = proposalId };
            var insuredGroup = new InsuredGroup(PlanType.Base, 10, 5000m); // Assuming 10 members with Base plan at $500 each

            _mockProposalsQuery
                .Setup(query => query.GetById(proposalId.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(proposal);

            _mockProposalsWriteRepository
                .Setup(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()))
                .Callback<Proposal, CancellationToken>((p, ct) => proposal.AddInsuredGroup(insuredGroup))
                .ReturnsAsync(proposal);

            // Act
            proposal.AddInsuredGroup(insuredGroup);
            var updatedProposal = await _proposalService.CreateOrUpdate(proposal, CancellationToken.None);

            // Assert
            Assert.Contains(insuredGroup, updatedProposal.InsuredGroups);
            Assert.Equal(1, updatedProposal.InsuredGroups.Count); // Assuming this is the first group added
            Assert.Equal(5000m, updatedProposal.TotalPremium);
            _mockProposalsWriteRepository.Verify(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// User_Case_3
        /// As an insurance company admin
        /// I want to see the total premium of a proposal
        /// So that I can understand how much the client company will pay us
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void User_Case_3_CalculateTotalPremium_ShouldReturnCorrectSum()
        {
            // Arrange
            var company = new Company("ABC");
            var proposal = new Proposal(company);

            var insuredGroupBase = new InsuredGroup(PlanType.Base, 10, 500m); // 10 members, $500 each
            var insuredGroupPremium = new InsuredGroup(PlanType.Premium, 5, 750m); // 5 members, $750 each

            // Act
            proposal.AddInsuredGroup(insuredGroupBase);
            proposal.AddInsuredGroup(insuredGroupPremium);

            var expectedTotalPremium = (10 * 500m) + (5 * 750m);
            var actualTotalPremium = proposal.TotalPremium;

            // Assert
            Assert.Equal(expectedTotalPremium, actualTotalPremium);
        }

        /// <summary>
        /// User_Case_3
        /// As an insurance company admin I want to create a discount so that for every 
        /// X members in a proposal with a selected plan - 1 member’s insurance is free
        /// </summary>
        /// <returns></returns>
        [Fact]
        public void User_Case_4_ApplyDiscount_PerXMembers_OneMemberFree()
        {
            // Arrange
            var company = new Company("ABC");
            var proposal = new Proposal(company);
            var basePlanPremium = 500m;
            var premiumPlanPremium = 750m;

            // Adding groups to the proposal
            proposal.AddInsuredGroup(new InsuredGroup(PlanType.Base, 10, basePlanPremium)); // 10 members with base plan
            proposal.AddInsuredGroup(new InsuredGroup(PlanType.Premium, 20, premiumPlanPremium)); // 20 members with premium plan

            var discountPerMember = 5; // For every 5 members, one is free

            // Expected discount calculations
            var expectedBaseDiscount = (10 / discountPerMember) * basePlanPremium; // 2 members free for base plan
            var expectedPremiumDiscount = (20 / discountPerMember) * premiumPlanPremium; // 4 members free for premium plan

            var expectedTotalPremium = (10 * basePlanPremium - expectedBaseDiscount) + (20 * premiumPlanPremium - expectedPremiumDiscount);

            // Act
            _proposalService.ApplyDiscount(proposal, discountPerMember);

            // Assert
            Assert.Equal(expectedTotalPremium, proposal.InsuredGroups.Sum(group => group.TotalPremium));
        }
    }

}

