using Hubtel.Wallet.Api.Enums;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Requests.Commands.Wallet;

public record DeleteWalletCommand(Guid WalletId) : IRequest<(bool?, ApiError)>;

