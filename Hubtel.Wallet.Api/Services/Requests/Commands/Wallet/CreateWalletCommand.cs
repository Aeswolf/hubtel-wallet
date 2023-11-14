using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Services.Common;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Requests.Commands.Wallet;

public record CreateWalletCommand(
    string OwnerPhoneNumber,
    string WalletName,
    string AccountNumber,
    WalletAccountType AccountType,
    WalletAccountSchemeType AccountScheme
) : IRequest<(WalletResponse?, ApiError)>;
