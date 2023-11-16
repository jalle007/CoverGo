using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Interfaces;

public interface IPlansWriteRepository
{
    public ValueTask<Plan> CreateOrUpdate(Plan Plan, CancellationToken cancellationToken = default);

    public ValueTask<bool> Delete(string id, CancellationToken cancellationToken = default);
}
