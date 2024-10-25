using System;

namespace RSAHyundai.Authentication
{
    public interface ITokenHelper
    {
        (string token, DateTime expiration) GenerateJWT(Guid userId, string userEmail);
    }
}
