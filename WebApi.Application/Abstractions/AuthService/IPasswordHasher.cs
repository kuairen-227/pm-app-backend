namespace WebApi.Application.Abstractions.AuthService;

public interface IPasswordHashService
{
    string Hash(string password);
    bool Verify(string hashedPassword, string providedPassword);
}
