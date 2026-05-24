namespace RiderDevTest.Application.Domain;

public class Publisher : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string StreetAddr1 { get; set; }
    public string? StreetAddr2 { get; set; }
    public string? StreetAddr3 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }

    public Publisher(string name, string email, string streetAddr1, string? streetAddr2, string? streetAddr3, string city,
        string state, string country)
    {
        Name = name;
        Email = email;
        StreetAddr1 = streetAddr1;
        StreetAddr2 = streetAddr2;
        StreetAddr3 = streetAddr3;
        City = city;
        State = state;
        Country = country;
    }
}