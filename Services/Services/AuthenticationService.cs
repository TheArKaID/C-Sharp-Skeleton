using Microsoft.EntityFrameworkCore;
using RSAHyundai.Authentication;
using RSAHyundai.Data;
using RSAHyundai.DTOs.Authentication;
using RSAHyundai.Encryption;
using RSAHyundai.Interfaces;
using System;
using System.Threading.Tasks;

namespace RSAHyundai.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _pwHasher;
        private readonly ITokenHelper _tokenHelper;
        public AuthService(AppDbContext context, IPasswordHasher pwhasher, ITokenHelper tokenHelper)
        {
            _context = context;
            _pwHasher = pwhasher;
            _tokenHelper = tokenHelper;
        }

        public async Task<(string token, DateTime expiration)> CreateJWT(CredentialsDTO credentials)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(c => c.Email == credentials.Email);
                if (user != null)
                {
                    var (verified, needsUpgrade) = _pwHasher.Check(user.Password, credentials.Password);
                    if (verified)
                    {
                        if (needsUpgrade)
                        {
                            user.Password = _pwHasher.Hash(credentials.Password);
                            await _context.SaveChangesAsync();
                        }

                        return _tokenHelper.GenerateJWT(user.Id, user.Email);
                    }
                }

                return (null, DateTime.MinValue);
            }
            catch
            {
                throw;
            }
        }
    }
}
