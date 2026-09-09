namespace SomeLibrary;

public class Person
{
	private int _age;

	public int Age
	{
		get { return _age; }
		set
		{
			if (value >= 0 && value < 130)
			{
				_age = value;
			}
		}
	}
	public string? FirstName { get; set; }
    public string? LastName { get; set; }

	public void Introduce()
	{
        Console.WriteLine($"Hallo, ik ben {FirstName} {LastName} ({Age} jaar oud)");
	}
}
