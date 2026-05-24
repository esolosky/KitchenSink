namespace RiderDevTest.Application.Domain;

public class Book : BaseEntity
{
    public Guid AuthorId { get; private set; }
    public Guid PublisherId { get; private set; }
    public string Title { get; private set; }
    public int PageCount { get; private set; }
    public DateTime PublishDate { get; private set; }
    public Author Author { get; private set; } = null!;
    public Publisher Publisher { get; private set; } = null!;

    public Book(string title, Guid authorId, Guid publisherId)
    {
        Title = title;
        AuthorId = authorId;
        PublisherId = publisherId;
    }
}