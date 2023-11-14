using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallet.Api.Contracts.Requests.User;

public class CreateUserContract
{
    [Required(ErrorMessage = "User Phone number is required")]
    [StringLength(13, MinimumLength = 10,
   ErrorMessage = "Phone number can have a maximum of 13 characters(comprising of a + sign and 12 digits) and a minimum of 10 digits")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])", ErrorMessage = "First name must comprise of only letters")]
    [StringLength(50, ErrorMessage = "First name must be at most 50 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name must be at most 50 characters")]
    [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])", ErrorMessage = "First name must comprise of only letters")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is  required")]
    [StringLength(255, ErrorMessage = "First name must at most 255 characters")]
    [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?!\\s)(?=.*[#$%&*!?]).{8,255}$")]
    public string Password { get; set; } = string.Empty;
}
