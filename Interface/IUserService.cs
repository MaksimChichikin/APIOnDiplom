using MyProApiDiplom.Models;

namespace MyProApiDiplom.Interface
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string FullName, string Email, string Password);
    }
    public interface IRegistration {
        Task<User> RegistrationAsync(string FullName, string Email, string Password);
    }
}
