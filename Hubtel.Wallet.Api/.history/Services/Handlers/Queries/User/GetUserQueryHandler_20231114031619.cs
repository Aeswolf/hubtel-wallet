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

    public Task<(UserResponse?, ApiError)> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        try
        {

        }
        catch(ApplicationException ex)
        {

        }
    }
}