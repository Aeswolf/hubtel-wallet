using Hubtel.Wallet.Api.Enums;
using System.Text.RegularExpressions;

namespace Hubtel.Wallet.Api.Utilities;

public class Validator
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

    public static bool ValidateWalletAccountNumber(string accountNumber, string accountType)
    {
        return (accountType == "momo") ? ValidatePhoneNumber(accountNumber)
                        : ValidateAccountNumber(accountNumber);
    }

    public static bool ValidateAccountSchemeAndAccountNumberMatch(string accountNumber, string scheme)
    {
        IDictionary<string, bool> accountNumberSchemePairs = new Dictionary<string, bool>()
        {
            { "visa", ApiRegex.IsAValidVisaAccountNumber(accountNumber) },
            { "master card", ApiRegex.IsAValidMasterCardAccountNumber(accountNumber) },
            { "mtn", ApiRegex.IsAValidMtnNumber(accountNumber) },
            { "vodafone", ApiRegex.IsAValidVodafoneNumber(accountNumber) },
            {  "airteltigo", ApiRegex.IsAValidAirtelTigoNumber(accountNumber) }
        };

        return accountNumberSchemePairs[scheme.ToLower()];
    }


}
