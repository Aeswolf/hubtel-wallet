namespace Hubtel.Wallet.Api.Services.Common;

public sealed record WalletResponse
(
    Guid WalletId,
    string WalletName,
    string AccountNumber,
    string AccountType,
   string AccountScheme,
   string Owner
);
