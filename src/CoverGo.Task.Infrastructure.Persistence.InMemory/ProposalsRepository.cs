using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory;

internal class ProposalsRepository : IProposalsQuery, IProposalsWriteRepository
{
    private static readonly List<Proposal> _items = new List<Proposal>
        {
          new Proposal(new Company( "ABC")),
          new Proposal(new Company("CoverGo")),
          new Proposal(new Company("Horns & Hooves"))
        };

    private readonly object _lock = new object();


    public ValueTask<Proposal?> GetById(string id, CancellationToken cancellationToken = default)
    {
        var proposal = _items.SingleOrDefault(p => p.Id.ToString() == id);
        if (proposal == null)
        {
            throw new KeyNotFoundException($"No proposal found with id {id}");
        }
        return new ValueTask<Proposal?>(proposal);
    }

    public ValueTask<List<Proposal>> GetAll()
    {
        return new ValueTask<List<Proposal>>(_items.ToList());
    }

    public ValueTask<Proposal> CreateOrUpdate(Proposal proposal, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var index = _items.FindIndex(p => p.Id == proposal.Id);
            if (index != -1)
            {
                _items[index] = proposal;
            }
            else
            {
                _items.Add(proposal);
            }
            Console.WriteLine($"Total items after operation: {_items.Count}");
            return new ValueTask<Proposal>(proposal);
        };
    }

    public ValueTask<bool> Delete(string id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            int removedCount = _items.RemoveAll(p => p.Id.ToString() == id);
            return new ValueTask<bool>(removedCount > 0);
        }
    }

}
