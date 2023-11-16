using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory;

internal class CompaniesRepository : ICompaniesQuery, ICompaniesWriteRepository
{
    private static readonly List<Company> _companies = new List<Company>
    {
        new Company ( "ABC" ),
        new Company ( "CoverGo" ),
        new Company ( "Horns & Hooves" ),
     
    };

    private readonly object _lock = new object();

    public ValueTask<Company?> GetById(string id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var company = _companies.SingleOrDefault(c => c.Id.ToString() == id);
            return new ValueTask<Company?>(company);
        }
    }

    public ValueTask<List<Company>> GetAll()
    {
        lock (_lock)
        {
            return new ValueTask<List<Company>>(_companies);
        }
    }

    public ValueTask<Company> CreateOrUpdate(Company company, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var index = _companies.FindIndex(c => c.Id == company.Id);
            if (index != -1)
            {
                _companies[index] = company;
            }
            else
            {
                _companies.Add(company);
            }
            Console.WriteLine($"Total companies after operation: {_companies.Count}");
            return new ValueTask<Company>(company);
        }
    }

    public ValueTask<bool> Delete(string id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            int removedCount = _companies.RemoveAll(p => p.Id.ToString() == id);
            return new ValueTask<bool>(removedCount > 0);
        }
    }
}
