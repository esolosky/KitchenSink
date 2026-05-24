using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RiderDevTest.Application.Books;

namespace RiderDevTest.Application;

/// <summary>
/// Extension methods for mapping all Book feature endpoints to Minimal APIs.
/// </summary>
public static class BookEndpoints
{
    /// <summary>
    /// Maps all book-related endpoints under the /api route prefix.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <returns>The endpoint route builder for chaining.</returns>
    public static IEndpointRouteBuilder MapApplicationEndpoints(this IEndpointRouteBuilder app)
    {
        var apiGroup = app.MapGroup("/api");

        // Map book endpoints under /api/book
        apiGroup.MapGroup("/books")
            .WithTags("Books")
            .MapBookEndpoints();

        return app;
    }
}