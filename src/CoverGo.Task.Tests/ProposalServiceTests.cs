using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Application.Services;
using CoverGo.Task.Domain;
using Moq;

namespace CoverGo.Task.Tests
{
    public class ProposalServiceTests
    {
        private readonly Mock<IProposalsQuery> _mockProposalsQuery;
        private readonly Mock<IProposalsWriteRepository> _mockProposalsWriteRepository;
        private readonly ProposalService _proposalService;

        public ProposalServiceTests()
        {
            // Arrange
            _mockProposalsQuery = new Mock<IProposalsQuery>();
            _mockProposalsWriteRepository = new Mock<IProposalsWriteRepository>();
            _proposalService = new ProposalService(_mockProposalsQuery.Object, _mockProposalsWriteRepository.Object);
        }


        [Fact]
        public async ValueTask  CreateOrUpdateProposal_ShouldCreateNewProposal_WhenProposalDoesNotExist()
        {
            // Arrange
            var proposalId = Guid.NewGuid().ToString();
            var newProposal = new Proposal(new Company("New Company"));

            _mockProposalsQuery
                .Setup(query => query.GetById(proposalId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Proposal?)null);  // Correctly using ValueTask<Proposal?>

            _mockProposalsWriteRepository
                .Setup(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(newProposal);

            // Act
            var result = await _proposalService.CreateOrUpdate(newProposal, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Company", result.ClientCompany.Name);
            _mockProposalsWriteRepository.Verify(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async ValueTask CreateOrUpdateProposal_ShouldUpdateExistingProposal_WhenProposalExists()
        {
            // Arrange
            var existingProposal = new Proposal(new Company("Existing Company")) { Id = Guid.NewGuid() };

            _mockProposalsQuery
                .Setup(query => query.GetById(existingProposal.Id.ToString(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProposal);

            _mockProposalsWriteRepository
                .Setup(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProposal);

            // Act
            var result = await _proposalService.CreateOrUpdate(existingProposal, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Existing Company", result.ClientCompany.Name);
            _mockProposalsWriteRepository.Verify(repo => repo.CreateOrUpdate(It.IsAny<Proposal>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async ValueTask GetById_ShouldReturnProposal_WhenProposalExists()
        {
            // Arrange
            var proposalId = Guid.NewGuid().ToString();
            var expectedProposal = new Proposal(new Company("Some Company")) { Id = Guid.Parse(proposalId) };

            _mockProposalsQuery
                .Setup(query => query.GetById(proposalId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedProposal);

            // Act
            var result = await _proposalService.GetById(proposalId, default);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedProposal.Id, result.Id);
        }

    }
}
