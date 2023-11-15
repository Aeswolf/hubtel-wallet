using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Services.Common;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Requests.Queries.User;

public sealed record GetUserQuery(string PhoneNumber) : IRequest<(UserResponse?, ApiError)>;