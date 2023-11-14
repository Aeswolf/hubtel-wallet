using Hubtel.Wallet.Api.Enums;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Requests.Commands.User;

public record DeleteUserCommand(string PhoneNumber) : IRequest<(bool?, ApiError)>;
