using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Interfaces;

public interface IUserRepository : IGenericRepository<UserModel>
{
    Task<(UserModel?, ApiError)> GetByPhoneNumberAsync(string phoneNumber);

    Task<(IReadOnlyCollection<UserModel>, ApiError)> GetAllWithWalletsAsync();

    Task<(UserModel?, ApiError)> GetWithWalletsAsync(string phoneNumber);

    Task<(bool, ApiError)> RemoveByPhoneNumberAsync(string phoneNumber);

 }
