using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Services.Common;
using Hubtel.Wallet.Api.Services.Requests.Queries.User;
using MapsterMapper;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Handlers.Queries.User;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, (UserResponse?, ApiError)>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<GetUserQueryHandler> _logger;

    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(UserResponse?, ApiError)> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var (user, error) = await _unitOfWork.Users.GetWithWalletsAsync(request.PhoneNumber);

            if (error == ApiError.Exception) return (null, error);

            if (user is null) return (null, ApiError.UserNotFound);

            var userResponse = _mapper.Map<UserResponse>(user);

            return (userResponse, ApiError.None);
        }
        catch (ApplicationException ex)
        {
            _logger.LogError($"Error occurred while executing the get-user query handler: \n{ex}");

            return (null, ApiError.Exception);
        }
    }
}