using VishanSabha.Models;

namespace VishanSabha.Services.Auth
{
    public interface IAuthService
    {
        Login ValidateUser(string contact, string password);
        Role GetUserByContact(string contact);
    }
}
