namespace CoverGo.Task.Domain;

public class Plan
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; }

    public Plan(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
