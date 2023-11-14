namespace Hubtel.Wallet.Api.Contracts.Requests.Wallet;

public class CreateWalletContract
{
    public string OwnerPhoneNumber { get; set; } = string.Empty;

    public string WalletName { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public string AccountType { get; set; } = string.Empty;

    public string AccountScheme { get; set; } = string.Empty;
}
