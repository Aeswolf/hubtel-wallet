using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Services.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Hubtel.Wallet.Api.Services.Requests.Commands.User;

public record CreateUserCommand(
     string PhoneNumber, 
     string FirstName,
     string LastName, 
     string Password
) : IRequest<(UserResponse?, ApiError)>;