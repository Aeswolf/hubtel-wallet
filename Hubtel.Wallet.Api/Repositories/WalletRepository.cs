using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Hubtel.Wallet.Api.Repositories;

public sealed class WalletRepository : GenericRepository<WalletModel>, IWalletRepository
{
    public WalletRepository(DbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
    }

    public async Task<(WalletModel?, ApiError)> GetByIdAsync(Guid id)
    {
        try
        {
            var wallet = await _dbSet.FirstOrDefaultAsync(wallet => wallet.Id == id
                                                                    && !wallet.IsDeleted);

            return (wallet, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving a wallet " +
                $"with a specified id from database  : \n{ex}");

            return (null, ApiError.Exception);
        }
    }

    public async Task<(WalletModel?, ApiError)> GetByAccountNumberAsync(string accountNumber)
    {
        try
        {
            var wallet = await _dbSet.FirstOrDefaultAsync(wallet => wallet.AccountNumber == accountNumber && !wallet.IsDeleted);

            return (wallet, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving a wallet " +
                $"with a specified name and owner from database  : \n{ex}");

            return (null, ApiError.Exception);
        }
    }

    public async Task<(IReadOnlyCollection<WalletModel>, ApiError)> GetByOwnerAsync(string phoneNumber)
    {
        try
        {
            var wallets = await _dbSet.Where(wallet => wallet.OwnerPhoneNumber == phoneNumber
                        && !wallet.IsDeleted).AsNoTracking().AsSplitQuery().ToListAsync();

            return (wallets, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving all wallets " +
                $"by owner's phone number from database  : \n{ex}");

            return (null!, ApiError.Exception);
        }
    }

    public async Task<(int?, ApiError)> GetOwnerWalletsCountAsync(string phoneNumber)
    {
        try
        {
            var walletCount = await _dbSet.Where(wallet => wallet.OwnerPhoneNumber == phoneNumber && !wallet.IsDeleted)
                                        .CountAsync();

            return (walletCount, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while counting the number of wallets belonging to a user: \n{ex}");

            return (null, ApiError.Exception);
        }
    }

    public async Task<(bool, ApiError)> RemoveAsync(Guid id)
    {
        try
        {
            var wallet = await _dbSet.FirstOrDefaultAsync(wallet => wallet.Id == id && !wallet.IsDeleted);

            if (wallet is null) return (false, ApiError.None);

            wallet.IsDeleted = true;
            wallet.DeletedAt = DateTime.UtcNow;

            return (true, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while removing" +
                $" a wallet with a specified id from database  : \n{ex}");

            return (false, ApiError.Exception);
        }
    }
}
