using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Interfaces;

public interface IGenericRepository<TModel> where TModel : BaseModel
{
    Task<(TModel?, ApiError)> CreateAsync(TModel model);

    Task<(IReadOnlyCollection<TModel>, ApiError)> GetAllAsync();

    Task<(bool, ApiError)> RemoveAllAsync();
}
