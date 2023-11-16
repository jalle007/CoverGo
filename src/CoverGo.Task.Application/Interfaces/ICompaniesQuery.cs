using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Interfaces;

public interface ICompaniesQuery
{
    public ValueTask<List<Company>> GetAll();
    public ValueTask<Company?> GetById(string id, CancellationToken cancellationToken = default);
}
