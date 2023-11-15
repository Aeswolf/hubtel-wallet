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

    public async Task<(IReadOnlyCollection<UserModel>, ApiError)> GetAllWithWalletsAsync()
    {
        try
        {
            var instances = await _dbSet.Where(instance => !instance.IsDeleted)
                            .Include(instance => instance.OwnedWallets)
                            .Where(instance => instance.OwnedWallets.Any(wallet => !wallet.IsDeleted))
                            .AsNoTracking().AsSplitQuery().ToListAsync();

            return (instances, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving all users " +
                $"with all their wallets from database  : \n{ex}");

            return (null!, ApiError.Exception);
        }
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

    public async Task<(bool, ApiError)> RemoveByPhoneNumberAsync(string phoneNumber)
    {
        try
        {
            var user = await _dbSet.FirstOrDefaultAsync(user => user.PhoneNumber == phoneNumber
                                                                                    && !user.IsDeleted);

            if (user is null) return (false, ApiError.None);

            _dbSet.Remove(user);

            return (true, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving a user with from database  : \n{ex}");

            return (false, ApiError.Exception);
        }
    }
}
