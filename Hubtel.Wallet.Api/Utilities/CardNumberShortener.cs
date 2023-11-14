namespace Hubtel.Wallet.Api.Utilities;

public class CardNumberShortener
{
    public static string Shorten(string cardNumber) => cardNumber.Substring(0, 6);
}
