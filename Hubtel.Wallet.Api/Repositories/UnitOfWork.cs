using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallet.Api.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public IUserRepository Users { get; private set; }

    public IWalletRepository Wallets { get; private set; }

    private readonly WalletDbContext _dbContext;

    private readonly ILogger _logger;

    public UnitOfWork(WalletDbContext dbContext, ILogger<UnitOfWork> logger)
    {
        _dbContext = dbContext;
        Wallets = new WalletRepository(dbContext, logger);
        Users = new UserRepository(dbContext, logger);
        _logger = logger;
    }
    public async Task<(bool, ApiError)> CompleteAsync()
    {
        try
        {
            var areChangesSaved = await _dbContext.SaveChangesAsync() > 0;

            return (areChangesSaved, ApiError.None);
        }
        catch(DbUpdateException ex)
        {
            _logger.LogError($"Error occurred while saving changes to the database:  \n{ex}");

            return (false, ApiError.Exception);
        }
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}
