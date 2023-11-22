using Hubtel.Wallet.Api.Enums;
using System.Text.RegularExpressions;

namespace Hubtel.Wallet.Api.Utilities;

public sealed class Validator
{
    public static bool ValidatePhoneNumber(string phoneNumber)
    {
        return ApiRegex.IsAValidAirtelTigoNumber(phoneNumber) || ApiRegex.IsAValidMtnNumber(phoneNumber)
                    || ApiRegex.IsAValidVodafoneNumber(phoneNumber);
    }

    public static bool ValidateAccountNumber(string accountNumber)
    {
        return ApiRegex.IsAValidVisaAccountNumber(accountNumber)
                        || ApiRegex.IsAValidMasterCardAccountNumber(accountNumber);
    }


    public static bool ValidateAccountNumberWithSchemeAndType(string accountNumber, string scheme, string type)
    {
        return (type == "momo") ? ValidatePhoneNumberWithScheme(accountNumber, scheme)
                : ValidateAccountNumberWithScheme(accountNumber, scheme);
    }


    private static bool ValidatePhoneNumberWithScheme(string phoneNumber, string scheme)
    {
        return scheme.ToLower() == "mtn" ? ApiRegex.IsAValidMtnNumber(phoneNumber)
                : scheme.ToLower() == "vodafone" ? ApiRegex.IsAValidVodafoneNumber(phoneNumber)
                : ApiRegex.IsAValidAirtelTigoNumber(phoneNumber);
    }

    private static bool ValidateAccountNumberWithScheme(string accountNumber, string scheme)
    {
        return scheme.ToLower() == "visa" ? ApiRegex.IsAValidVisaAccountNumber(accountNumber)
                    : ApiRegex.IsAValidMasterCardAccountNumber(accountNumber);
    }

}
