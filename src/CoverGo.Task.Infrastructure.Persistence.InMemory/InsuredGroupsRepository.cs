using CoverGo.Task.Application.Interfaces;
using CoverGo.Task.Domain;

namespace CoverGo.Task.Infrastructure.Persistence.InMemory;

internal class InsuredGroupsRepository : IInsuredGroupQuery, IInsuredGroupWriteRepository
{
    private static readonly List<InsuredGroup> _items = new List<InsuredGroup>
    {
        new InsuredGroup(PlanType.Base, 100, 1000m),
        new InsuredGroup(PlanType.Premium, 50, 2000m)
    };

    private readonly object _lock = new object();

    public ValueTask<InsuredGroup?> GetById(string id, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var insuredGroup = _items.SingleOrDefault(ig => ig.Id.ToString() == id);
            return new ValueTask<InsuredGroup?>(insuredGroup);
        }
    }

    public ValueTask<List<InsuredGroup>> GetAll()
    {
        lock (_lock)
        {
            return new ValueTask<List<InsuredGroup>>(_items);
        }
    }

    public ValueTask<InsuredGroup> CreateOrUpdate(InsuredGroup insuredGroup, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            var index = _items.FindIndex(ig => ig.Id == insuredGroup.Id);
            if (index != -1)
            {
                _items[index] = insuredGroup;
            }
            else
            {
                _items.Add(new InsuredGroup(insuredGroup.PlanType, insuredGroup.NumberOfMembers, insuredGroup.PremiumPerMember){});
            }
            Console.WriteLine($"Total insured groups after operation: {_items.Count}");
            return new ValueTask<InsuredGroup>(insuredGroup);
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
