using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallet.Api.Models;

public class UserModel : BaseModel
{
    [Key]
    [StringLength(13, MinimumLength = 10, 
    ErrorMessage = "Phone number can have a maximum of 13 characters(comprising of a + sign and 12 digits) and a minimum of 10 digits")]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "First name must be at most 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Last name must be at most 50 characters")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "Password must at most 255 characters")]
    public string Password { get; set; } = string.Empty;

    public IReadOnlyCollection<WalletModel> OwnedWallets { get; set; }
}
