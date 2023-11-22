namespace Hubtel.Wallet.Api.Services.Common;

public sealed record UserResponse
(
    string PhoneNumber,
    string FirstName,
    string LastName,
    IReadOnlyCollection<WalletResponse> OwnedWallets
);
