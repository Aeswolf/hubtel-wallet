using Hubtel.Wallet.Api.Enums;
using System.Collections;

namespace Hubtel.Wallet.Api.Utilities;

public class Converter
{
    public static WalletAccountType ConvertToAccountType(string accountType)
    {
        if (accountType == "momo") return WalletAccountType.Momo;

        return WalletAccountType.Card;
    }

    public static WalletAccountSchemeType? ConvertToSchemeType(string accountScheme)
    {

        IDictionary<string, WalletAccountSchemeType> wordSchemePairs = new Dictionary<string, WalletAccountSchemeType>()
        {
            { "visa", WalletAccountSchemeType.Visa },
            { "master card", WalletAccountSchemeType.MasterCard },
            { "mtn", WalletAccountSchemeType.Mtn },
            { "vodafone", WalletAccountSchemeType.Vodafone },
            {  "airteltigo", WalletAccountSchemeType.AirtelTigo }
        };

        return wordSchemePairs[accountScheme.ToLower()];
    }
}
