using Hubtel.Wallet.Api.Enums;

namespace Hubtel.Wallet.Api.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }

    IWalletRepository Wallets { get; }

    Task<(bool, ApiError)> CompleteAsync();
}
