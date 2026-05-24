using Microsoft.AspNetCore.Mvc;
using RiderDevTest.Application.Books;
using RiderDevTest.Application.Common.Models;

namespace RiderDevTest.Controllers;

[ApiController]
[Route("[controller]")]
public class BookController : ControllerBase
{
    [HttpGet(Name = "GetBook")]
    public PaginatedList<BookDto> Get()
    {
        return null;
    }
}