namespace RiderDevTest.Application.Domain;

public class Author : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Suffix { get; set; }
    public string SearchName { get; set; }

    public string FormattedName()
    {
        return FirstName + " " + LastName + (string.IsNullOrEmpty(Suffix) ? "" : ", " + Suffix);
    }

    public Author(string firstName, string lastName, string? suffix)
    {
        FirstName = firstName;
        LastName = lastName;
        Suffix = suffix;
        SearchName = BuildSearchName();
    }

    private string BuildSearchName()
    {
        var searchName = $"{FirstName.ToLower()} {LastName.ToLower()}";
        if (!string.IsNullOrEmpty(Suffix))
        {
            searchName = $"{searchName}, {Suffix.ToLower()}";
        }
        return searchName;
    }
}