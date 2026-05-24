namespace RiderDevTest.Application.Books;

public record BookDto(
    Guid Id,
    string Title,
    string Author,
    int PageCount,
    DateTime PublishDate,
    DateTime Created,
    DateTime? LastModified);