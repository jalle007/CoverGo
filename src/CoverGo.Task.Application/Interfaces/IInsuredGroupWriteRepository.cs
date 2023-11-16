using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Interfaces;

public interface IInsuredGroupWriteRepository
{
    public ValueTask<InsuredGroup> CreateOrUpdate(InsuredGroup insuredGroup, CancellationToken cancellationToken = default);

    public ValueTask<bool> Delete(string id, CancellationToken cancellationToken = default);
}
