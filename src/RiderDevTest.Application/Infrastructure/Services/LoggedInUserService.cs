using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RiderDevTest.Application.Common.Interfaces;

namespace RiderDevTest.Application.Infrastructure.Services;

public class LoggedInUserService(IHttpContextAccessor httpContextAccessor) : ILoggedInUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirst("preferred_username")?.Value;
}