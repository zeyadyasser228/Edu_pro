using System.Threading.Tasks;
using EduPro.Models;

namespace EduPro.Services
{
    public interface IUserService // functions as Prototype
    {
        // b4of aly da5l 
        Task<UserModel> AuthenticateAsync(string email, string password);

        // get user mn al database bl email 
        Task<UserModel> GetUserByEmailAsync(string email);

        // signup 
        Task<UserModel> CreateUserAsync(UserModel user);

        // b4of al email da fel database wla l2a 
        Task<bool> EmailExistsAsync(string email);

        // update last login 
        Task UpdateLastLoginAsync(int userId);

        // rabdon and code get 
        Task<string> GeneratePasswordResetCodeAsync(string email);
        // verify code sent 
        Task<bool> VerifyResetCodeAsync(string email, string code);
        // rest
        Task ResetPasswordAsync(string email, string newPassword);
    }
}