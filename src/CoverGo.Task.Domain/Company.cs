namespace CoverGo.Task.Domain;

public class Company
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; }

    public Company(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}
