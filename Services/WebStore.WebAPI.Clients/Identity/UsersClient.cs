using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;
using WebStore.Interfaces.Identity;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Identity
{
    public class UsersClient : BaseClient, IUserClient
    {
        public UsersClient(HttpClient client) : base(client, WebAPIAddrsses.Identity.Users)
        {
        }

        public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            
        }

        public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            
        }

        public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            
        }

        public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public void Dispose()
        {
            
        }

        public Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            
        }

        public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            
        }

        public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            
        }

        public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            
        }

        public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            
        }

        public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            
        }

        public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken)
        {
            
        }

        public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            
        }

        public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken)
        {
            
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            
        }

        public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken)
        {
            
        }

        public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken)
        {
            
        }

        public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken)
        {
            
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            
        }

        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            
        }
    }
}
