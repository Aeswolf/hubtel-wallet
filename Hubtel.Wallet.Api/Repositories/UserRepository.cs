using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Hubtel.Wallet.Api.Repositories;

public class UserRepository : GenericRepository<UserModel>, IUserRepository
{
    public UserRepository(DbContext dbContext, ILogger logger)
        : base(dbContext, logger)
    {
    }

    public async Task<(UserModel?, ApiError)> GetByPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            var user = await _dbSet.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber
                        && !user.IsDeleted);

            return (user, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving a user " +
                $"by specified phone number from database  : \n{ex}");

            return (null, ApiError.Exception);
        }
    }

    public async Task<(UserModel?, ApiError)> GetWithWalletsAsync(string phoneNumber)
    {
        try
        {
            var user = await _dbSet.Include(user => user.OwnedWallets)
                                .FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber
                                && !user.IsDeleted);

            if (user is not null)
            {
                user.OwnedWallets = user.OwnedWallets.Where(wallet => !wallet.IsDeleted).ToList();
            }

            return (user, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving a user " +
                $"with all the user's wallets from database  : \n{ex}");

            return (null, ApiError.Exception);
        }
    }
}
