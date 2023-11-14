namespace Hubtel.Wallet.Api.Services.Common;

public record WalletResponse
{
    public Guid WalletId { get; set; }

    public string WalletName { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string AccountType { get; set; } = string.Empty;

    public string AccountScheme { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;
}

