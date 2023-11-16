using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Interfaces;

public interface IInsuredGroupQuery
{
    public ValueTask<List<InsuredGroup>> GetAll();

    public ValueTask<InsuredGroup?> GetById(string id, CancellationToken cancellationToken = default);
}
 