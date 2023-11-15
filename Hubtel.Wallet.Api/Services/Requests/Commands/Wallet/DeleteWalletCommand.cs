using Hubtel.Wallet.Api.Enums;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Requests.Commands.Wallet;

public sealed record DeleteWalletCommand(Guid WalletId) : IRequest<(bool?, ApiError)>;
