using Hubtel.Wallet.Api.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hubtel.Wallet.Api.Models;

public sealed class WalletModel : BaseModel
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public WalletAccountType AccountType { get; set; }

    public WalletAccountSchemeType AccountScheme { get; set; }

    [ForeignKey("Users")]
    public string OwnerPhoneNumber { get; set; } = string.Empty;

    public UserModel Owner { get; set; }
}
