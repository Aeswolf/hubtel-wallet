using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Services.Common;
using Hubtel.Wallet.Api.Services.Requests.Queries.Wallet;
using MapsterMapper;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Handlers.Queries.Wallet;

public class GetWalletQueryHandler : IRequestHandler<GetWalletQuery, (WalletResponse?, ApiError)>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<GetWalletQueryHandler> _logger;

    private readonly IMapper _mapper;

    public GetWalletQueryHandler(IUnitOfWork unitOfWork, ILogger<GetWalletQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(WalletResponse?, ApiError)> Handle(GetWalletQuery request, CancellationToken cancellationToken)
    {
       try
       {
            var (wallet, error) = await _unitOfWork.Wallets.GetByIdAsync(request.WalletId);

            if (error == ApiError.Exception) return (null, error);

            if (wallet is null) return (null, ApiError.WalletNotFound);

            var walletResponse = _mapper.Map<WalletResponse>(wallet);

            return (walletResponse, ApiError.None);
       }
        catch(ApplicationException ex)
        {
            _logger.LogError($"Error occurred while executing the get-wallet query handler: \n{ex}");

            return (null, ApiError.Exception);
        }
    }
}
