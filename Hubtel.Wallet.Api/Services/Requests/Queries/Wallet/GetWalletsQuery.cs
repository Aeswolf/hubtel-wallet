using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Services.Common;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Requests.Queries.Wallet;

public record GetWalletsQuery() : IRequest<(IReadOnlyCollection<WalletResponse>, ApiError)>;
