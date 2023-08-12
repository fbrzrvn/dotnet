namespace TestingTechniques;

public class ValueSamples
{
    public string FullName = "Lerelle Jones";

    public int Age = 21;

    public DateOnly DateOfBirth = new(2002, 6, 9);

    public User AppUser = new()
    {
        FullName = "Lerelle Jones",
        Age = 21,
        DateOfBirth = new(2002, 6, 9)
    };

    public IEnumerable<User> Users = new[]
    {
        new User()
        {
            FullName = "Lerelle Jones",
            Age = 21,
            DateOfBirth = new (2002, 6, 9)
        },
        new User()
        {
            FullName = "Tom Scott",
            Age = 37,
            DateOfBirth = new (1986, 6, 9)
        },
        new User()
        {
            FullName = "Steve Mould",
            Age = 43,
            DateOfBirth = new (1980, 10, 5)
        }
    };


    public IEnumerable<int> Numbers = new[] { 1, 5, 10, 15 };

    internal int InternalSecretNumber = 42;

    public event EventHandler ExampleEvent;

    public virtual void RaiseExampleEvent()
    {
        ExampleEvent(this, EventArgs.Empty);
    }
}