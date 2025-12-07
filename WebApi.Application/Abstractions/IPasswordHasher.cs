namespace WebApi.Application.Abstractions;

public interface IPasswordHashService
{
    string Hash(string password);
    bool Verify(string hashedPassword, string providedPassword);
}
