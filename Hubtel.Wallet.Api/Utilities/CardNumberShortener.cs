namespace Hubtel.Wallet.Api.Utilities;

public sealed class CardNumberShortener
{
    public static string Shorten(string cardNumber) => cardNumber.Substring(0, 6);
}
