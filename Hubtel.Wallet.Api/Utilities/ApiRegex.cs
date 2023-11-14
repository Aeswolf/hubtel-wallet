using System.Text.RegularExpressions;

namespace Hubtel.Wallet.Api.Utilities;

public class ApiRegex
{
    public static bool IsAValidMtnNumber(string phoneNumber)
    {
        var mtnNumberPattern = "^((?:\\+233)|0)(24|25|54|55|59)\\d{7}$";

        return Regex.IsMatch(phoneNumber, mtnNumberPattern);
    }

    public static bool IsAValidVodafoneNumber(string phoneNumber)
    {
        var vodafoneNumberPattern = "^((?:\\+233)|0)(20|50)\\d{7}$";

        return Regex.IsMatch(phoneNumber, vodafoneNumberPattern);
    }

    public static bool IsAValidAirtelTigoNumber(string phoneNumber)
    {
        var airtelTigoNumberPattern = "^((?:\\+233)|0)(27|26|57)\\d{7}$";

        return Regex.IsMatch(phoneNumber, airtelTigoNumberPattern);
    }

    public static bool IsAValidVisaAccountNumber(string accountNumber)
    {
        var visaPattern = "^4[0-9]{15}$";

        return Regex.IsMatch(accountNumber, visaPattern);
    }

    public static bool IsAValidMasterCardAccountNumber(string accountNumber)
    {
        var masterCardPattern = "^5[1-5][0-9]{14}$";

        return Regex.IsMatch(accountNumber, masterCardPattern);
    }


}
