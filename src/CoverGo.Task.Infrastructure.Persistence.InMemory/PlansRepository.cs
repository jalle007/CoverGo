using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory;

internal class PlansRepository : IPlansQuery, IPlansWriteRepository
{
    private static readonly List<Plan> _items = new()
    {
        new Plan("Basic" ),
        new Plan("Premium" ),
        new Plan("Gold" )
    };

    private readonly object _lock = new object();

    public ValueTask<Plan?> GetById(string id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var plan = _items.SingleOrDefault(p => p.Id.ToString() == id);
            return new ValueTask<Plan?>(plan);
        }
    }

    public ValueTask<List<Plan>> GetAll()
    {
        lock (_lock)
        {
            return new ValueTask<List<Plan>>(_items);
        }
    }

    public ValueTask<Plan> CreateOrUpdate(Plan plan, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var index = _items.FindIndex(p => p.Id == plan.Id);
            if (index != -1)
            {
                _items[index] = plan;
            }
            else
            {
                _items.Add(plan);
            }
            return new ValueTask<Plan>(plan);
        }
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
