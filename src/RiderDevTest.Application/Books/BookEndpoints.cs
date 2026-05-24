using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RiderDevTest.Application.Common;
using RiderDevTest.Application.Common.Models;

namespace RiderDevTest.Application.Books;

public static class BookEndpoints
{
    /// <summary>
    /// Maps all book-related endpoints.
    /// </summary>
    /// <param name="group">The route group builder.</param>
    /// <returns>The route group builder for chaining.</returns>
    internal static RouteGroupBuilder MapBookEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllBooks.Endpoint.Handle)
            .WithName("GetAllBooks")
            .Produces<PaginatedList<BookDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<GetAllBooks.Query>>();
        return group;
    }
}
