using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;

namespace Hubtel.Wallet.Api.Services.Common;

public static class SaveChanges
{
    public static async Task<ApiError> SaveToDbAsync(this IUnitOfWork unitOfWork)
    {
        var (areChangesSaved, error) = await unitOfWork.CompleteAsync();

        if (error is ApiError.Exception) return error;

        if (!areChangesSaved) return ApiError.SaveFailure;

        return ApiError.None;
    }
}
