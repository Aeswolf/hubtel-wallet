using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Services.Common;
using Hubtel.Wallet.Api.Services.Requests.Commands.User;
using Hubtel.Wallet.Api.Models;
using MediatR;
using MapsterMapper;

namespace Hubtel.Wallet.Api.Services.Handlers.Commands.User;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, (UserResponse?, ApiError)>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<CreateUserCommandHandler> _logger;

    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateUserCommandHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(UserResponse?, ApiError)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (user, error) = await _unitOfWork.Users.GetByPhoneNumberAsync(request.PhoneNumber);

            if (error == ApiError.Exception) return (null, error);

            if (user is not null) return (null, ApiError.UserExists);

            user = _mapper.Map<UserModel>(request);

            (user, error) = await _unitOfWork.Users.CreateAsync(user!);

            if (error == ApiError.Exception) return (null, error);

            var (wasChangesSaved, saveException) = await _unitOfWork.CompleteAsync();

            if (saveException == ApiError.Exception) return (null, saveException);

            if (!wasChangesSaved) return (null, ApiError.SavesFailure);

            var userResponse = _mapper.Map<UserResponse>(user!);

            return (userResponse, ApiError.None);
        }
        catch(ApplicationException ex)
        {
            _logger.LogError($"Error occurred while executing the create user command handler: \n{ex}");

            return (null, ApiError.Exception);
        }
    }
}
