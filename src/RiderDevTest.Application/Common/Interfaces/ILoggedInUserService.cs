namespace RiderDevTest.Application.Common.Interfaces;

public interface ILoggedInUserService
{
    string? UserId { get; }
    string? UserName { get; }   
}