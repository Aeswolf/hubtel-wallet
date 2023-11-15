using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Services.Common;
using Hubtel.Wallet.Api.Services.Requests.Queries.Wallet;
using MapsterMapper;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Handlers.Queries.Wallet;

public sealed class GetWalletsQueryHandler :
    IRequestHandler<GetWalletsQuery, (IReadOnlyCollection<WalletResponse>, ApiError)>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<GetWalletsQueryHandler> _logger;

    private readonly IMapper _mapper;

    public GetWalletsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetWalletsQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<(IReadOnlyCollection<WalletResponse>, ApiError)> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var (wallets, error) = await _unitOfWork.Wallets.GetAllAsync();

            if (error == ApiError.Exception) return (null!, error);

            var walletResponses = _mapper.Map<IReadOnlyCollection<WalletResponse>>(wallets);

            return (walletResponses, ApiError.None);
        }
        catch(ApplicationException ex)
        {
            _logger.LogError($"Error occurred while executing the get-wallets query handler: \n{ex}");

            return (null!, ApiError.Exception);
        }
    }
}
