using RiderDevTest.Application.Domain;

namespace RiderDevTest.Application.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed Healthcare data
        if (!context.Books.Any())
        {
            context.Books.AddRange(
                new Book("Kafka On The Shore", new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111")),
                new Book("Grapes Of Wrath", new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111")),
                new Book("Ship Of Magic", new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111")));
            
            context.Authors.AddRange(
                new Author("Haruki", "Murakami", null),
                new Author("John", "Steinbeck", null),
                new Author("Robin", "Hobb", null));
            
            context.Publishers.AddRange(
                new Publisher("Knopf", "", "1745 Broadway", null, null, "Manhattan", "NY", "US"),
                new Publisher("Knopf", "", "1745 Broadway", null, null, "Manhattan", "NY", "US"),
                new Publisher("Knopf", "", "1745 Broadway", null, null, "Manhattan", "NY", "US"));

            await context.SaveChangesAsync();
        }
    }
}