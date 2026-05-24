using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RiderDevTest.Application.Common.Mappings;
using RiderDevTest.Application.Common.Models;
using RiderDevTest.Application.Infrastructure.Persistence;

namespace RiderDevTest.Application.Books;

public static class GetAllBooks
{
    public record Query(
        Guid? Id,
        string? Title,
        string? Author,
        int PageNumber,
        int PageSize) : IRequest<PaginatedList<BookDto>>;

    internal static class Endpoint
    {
        public static async Task<IResult> Handle(
            ISender mediator,
            Guid? id = null,
            string? title = null,
            string? author = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var query = new Query(
                id,
                title,
                author,
                pageNumber,
                pageSize);

            var result = await mediator.Send(query);

            return Results.Ok(result);
        }
    }
    
    internal sealed class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(v => v.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be at least 1");

            RuleFor(v => v.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be at least 1")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must not exceed 100");

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.Author))
                .WithMessage("Either Title or Author must be provided.");
            
            RuleFor(v => v)
                .Must(v => IsValidAuthor(v.Author))
                .When(v => !string.IsNullOrWhiteSpace(v.Author))
                .WithMessage("Author must not contain special characters.")
                .WithName("Author");
        }

        private bool IsValidAuthor(string? author)
        {
            return author != null && Regex.IsMatch(
                author,
                @"^[a-zA-Z0-9\s'"".,;:!?&()\-]+$"
            );
        }
    }
    
    internal sealed class Handler(ApplicationDbContext context)
        : IRequestHandler<Query, PaginatedList<BookDto>>
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<PaginatedList<BookDto>> Handle(
            Query request,
            CancellationToken cancellationToken)
        {
            var query = _context.Books
                .Include(a => a.Author)
                .Include(a => a.Publisher)
                .AsNoTracking()
                .AsQueryable();
            var authorText = request.Author?.ToLower() ?? string.Empty;
            var titleText = request.Title?.ToLower() ?? string.Empty;
            query = query.Where(a => 
                (string.IsNullOrEmpty(authorText) || a.Author.SearchName.Contains(authorText))
                || (string.IsNullOrEmpty(titleText) || a.Title.Contains(titleText)));

            query = query.OrderBy(a => a.Title);

            var paginatedResult = await query
                .Select(a => new BookDto(
                    a.Id,
                    a.Title,
                    a.Author.FormattedName(),
                    a.PageCount,
                    a.PublishDate,
                    a.Created,
                    a.LastModified
                    ))
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return paginatedResult;
        }
    }
}