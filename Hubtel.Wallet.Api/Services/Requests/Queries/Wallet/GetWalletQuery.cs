using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Services.Common;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Requests.Queries.Wallet;

public sealed record GetWalletQuery(Guid WalletId) : IRequest<(WalletResponse?, ApiError)>;
