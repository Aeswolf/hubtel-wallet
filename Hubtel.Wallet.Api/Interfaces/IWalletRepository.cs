using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Interfaces;

public interface IWalletRepository : IGenericRepository<WalletModel>
{
    Task<(WalletModel?, ApiError)> GetByIdAsync(Guid id);

    Task<(IReadOnlyCollection<WalletModel>, ApiError)> GetByOwnerAsync(string phoneNumber);

    Task<(bool, ApiError)> RemoveAsync(Guid id);

    Task<(int?, ApiError)> GetOwnerWalletsCountAsync(string phoneNumber);

    Task<(WalletModel?, ApiError)> GetByAccountNumberAsync(string accountNumber);
}
