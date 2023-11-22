using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Services.Common;
using Hubtel.Wallet.Api.Services.Requests.Commands.Wallet;
using MapsterMapper;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Handlers.Commands.Wallet;

public sealed class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, (bool?, ApiError)>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<DeleteWalletCommandHandler> _logger;

    private readonly IMapper _mapper;

    public DeleteWalletCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteWalletCommandHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(bool?, ApiError)> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (isWalletDeleted, error) = await _unitOfWork.Wallets.RemoveAsync(request.WalletId);

            if (error is ApiError.Exception) return (null, error);

            if (!isWalletDeleted) return (isWalletDeleted, ApiError.None);

            error = await _unitOfWork.SaveToDbAsync();

            if (error is not ApiError.None) return (null, error);

            return (isWalletDeleted, ApiError.None);
        }
        catch(ApplicationException ex)
        {
            _logger.LogError($"Error occurred while executing the delete-wallet command handler: \n{ex}");

            return (null, ApiError.Exception);
        }
    }
}
