using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Services
{
    public class InsuredGroupService
    {
        private readonly IInsuredGroupQuery _insuredGroupQuery;
        private readonly IInsuredGroupWriteRepository _insuredGroupWriteRepository;

        public InsuredGroupService(IInsuredGroupQuery insuredGroupQuery, IInsuredGroupWriteRepository insuredGroupWriteRepository)
        {
            _insuredGroupQuery = insuredGroupQuery;
            _insuredGroupWriteRepository = insuredGroupWriteRepository;
        }

        public async Task<InsuredGroup?> GetById(Guid insuredGroupId, CancellationToken cancellationToken = default)
        {
            return await _insuredGroupQuery.GetById(insuredGroupId.ToString(), cancellationToken);
        }

        public async Task<List<InsuredGroup>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _insuredGroupQuery.GetAll();
        }

        public async Task<bool> Delete(Guid insuredGroupId, CancellationToken cancellationToken = default)
        {
            return await _insuredGroupWriteRepository.Delete(insuredGroupId.ToString(), cancellationToken);
        }

        public async Task<InsuredGroup> CreateOrUpdate(InsuredGroup insuredGroup, CancellationToken cancellationToken = default)
        {
            if (insuredGroup == null)
            {
                throw new ArgumentNullException(nameof(insuredGroup));
            }

            // Attempt to retrieve the existing InsuredGroup
            var existingInsuredGroup = await _insuredGroupQuery.GetById(insuredGroup.Id.ToString(), cancellationToken);

            if (existingInsuredGroup != null)
            {
                // Create a new InsuredGroup instance with updated values using the constructor
                var updatedInsuredGroup = new InsuredGroup(
                    planType: insuredGroup.PlanType,
                    numberOfMembers: insuredGroup.NumberOfMembers,
                    premiumPerMember: insuredGroup.PremiumPerMember
                )
                {
                    Id = existingInsuredGroup.Id  
                };
                await _insuredGroupWriteRepository.CreateOrUpdate(updatedInsuredGroup, cancellationToken);
            }
            else
            {
                // If the insured group doesn't exist, add it as a new entity
                await _insuredGroupWriteRepository.CreateOrUpdate(insuredGroup, cancellationToken);
            }

            // Return the updated or created insured group
            return insuredGroup;
        }

    }
}
