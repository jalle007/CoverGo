using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Interfaces;

public interface IProposalsWriteRepository
{
    public ValueTask<Proposal> CreateOrUpdate(Proposal Proposal, CancellationToken cancellationToken = default);

    public ValueTask<bool> Delete(string id, CancellationToken cancellationToken = default);
}
