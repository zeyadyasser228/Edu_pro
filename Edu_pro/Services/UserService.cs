    using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EduPro.Data;
using EduPro.Models;
using Microsoft.EntityFrameworkCore;

namespace EduPro.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context; // Entity Framawork 
        private readonly ILogger<UserService> _logger; // Logger For messages and save Errrors
        // Constructor 
        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }
        // Check Authenicated or not 
        public async Task<UserModel> AuthenticateAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            // Check Database Found or Not 
            // Back for Data of the user
             var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
          
            // user exist or no 
            if (user == null)
                return null;

            if (!user.IsActive)
                return null;

            // Add a null check for the password
            if (user.Password == null)
            {
                _logger.LogWarning($"User {email} has a NULL password in the database");
                return null;
            }

            if (!VerifyPasswordHash(password, user.Password))
                return null;

            return user;
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
        // signup 
        public async Task<UserModel> CreateUserAsync(UserModel user)
        {
            if (await EmailExistsAsync(user.Email))
                throw new Exception("Email is already registered");

            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
        // Encrypt to Password 
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            string computedHash = HashPassword(password);
            return computedHash.Equals(storedHash);
        }
        public async Task<string> GeneratePasswordResetCodeAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                    return null;

                Random random = new Random();
                string verificationCode = random.Next(100000, 999999).ToString(); // 10 to 99 

                user.VerificationCode = verificationCode;
                user.VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(5);

                await _context.SaveChangesAsync();

                return verificationCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error generating verification code for email: {email}");
                throw;
            }
        }
        public async Task<bool> VerifyResetCodeAsync(string email, string code)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {email}");
                    return false;
                }

                _logger.LogInformation($"Comparing codes - Stored: {user.VerificationCode}, Provided: {code}");
                _logger.LogInformation($"Expiry time: {user.VerificationCodeExpiry}, Current time: {DateTime.UtcNow}");

                bool isValid = user.VerificationCode == code &&
                              user.VerificationCodeExpiry.HasValue &&
                              user.VerificationCodeExpiry.Value > DateTime.UtcNow;

                _logger.LogInformation($"Code validation result: {isValid}");

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error verifying code for email: {email}");
                throw;
            }
        }
        public async Task ResetPasswordAsync(string email, string newPassword)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    throw new ArgumentException($"User not found for email: {email}");
                }

                user.Password = HashPassword(newPassword);

                user.VerificationCode = "";
                user.VerificationCodeExpiry = null;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Password reset successful for user: {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error resetting password for email: {email}");
                throw;
            }
        }

      

    }
}