using RSAHyundai.DTOs.Authentication;
using System;
using System.Threading.Tasks;

namespace RSAHyundai.Interfaces
{
    public interface IAuthService
    {
        Task<(string token, DateTime expiration)> CreateJWT(CredentialsDTO credentials);
    }
}
