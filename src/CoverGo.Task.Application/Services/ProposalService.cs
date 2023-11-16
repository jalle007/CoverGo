using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Services
{
    public class ProposalService
    {
        private readonly IProposalsQuery _proposalsQuery;
        private readonly IProposalsWriteRepository _proposalsWriteRepository;

        public ProposalService(IProposalsQuery proposalsQuery, IProposalsWriteRepository proposalsWriteRepository)
        {
            _proposalsQuery = proposalsQuery;
            _proposalsWriteRepository = proposalsWriteRepository;
        }

        public async Task<Proposal> CreateOrUpdate(Proposal proposal, CancellationToken cancellationToken = default)
        {
            if (proposal == null)
            {
                throw new ArgumentNullException(nameof(proposal));
            }

            var existingProposal = await _proposalsQuery.GetById(proposal.Id.ToString(), cancellationToken);

            if (existingProposal != null)
            {
                var updatedProposal = new Proposal(new Company(proposal.ClientCompany.Name))
                {
                    Id = existingProposal.Id,
                    ClientCompany=existingProposal.ClientCompany,
                    InsuredGroups = proposal.InsuredGroups 
                };

                await _proposalsWriteRepository.CreateOrUpdate(updatedProposal, cancellationToken);
                return updatedProposal;
            }
            else
            {
                // If the proposal doesn't exist, add it as a new entity
                await _proposalsWriteRepository.CreateOrUpdate(proposal, cancellationToken);
                return proposal;
            }
        }

        public async Task<Proposal?> GetById(string proposalId, CancellationToken cancellationToken = default)
        {
            return await _proposalsQuery.GetById(proposalId, cancellationToken);
        }

        public async Task<List<Proposal>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _proposalsQuery.GetAll();
        }

        public async Task<bool> DeleteProposal(string proposalId, CancellationToken cancellationToken = default)
        {
            return await _proposalsWriteRepository.Delete(proposalId, cancellationToken);
        }

        public void ApplyDiscount(Proposal proposal, int discountPerMember)
        {
            foreach (var group in proposal.InsuredGroups)
            {
                var freeMembers = group.NumberOfMembers / discountPerMember;
                group.ApplyDiscount(freeMembers);
            }
        }
    
    }
}
