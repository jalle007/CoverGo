using CoverGo.Task.Domain;

namespace CoverGo.Task.Application.Interfaces;

public interface ICompaniesWriteRepository
{
    public ValueTask<Company> CreateOrUpdate(Company company, CancellationToken cancellationToken = default);

    public ValueTask<bool> Delete(string id, CancellationToken cancellationToken = default);
}

