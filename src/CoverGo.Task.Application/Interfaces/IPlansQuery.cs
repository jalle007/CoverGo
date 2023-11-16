using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Interfaces;

public interface IPlansQuery
{
    public ValueTask<List<Plan>> GetAll();
    public ValueTask<Plan?> GetById(string id, CancellationToken cancellationToken = default);

}
