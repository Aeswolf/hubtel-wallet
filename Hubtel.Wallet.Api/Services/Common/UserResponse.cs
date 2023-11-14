namespace Hubtel.Wallet.Api.Services.Common;

public record UserResponse
{
    public string PhoneNumber { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public IReadOnlyCollection<WalletResponse> OwnedWallets { get; set; }
}
