using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Models;
using Hubtel.Wallet.Api.Services.Common;
using Hubtel.Wallet.Api.Services.Requests.Commands.Wallet;
using Hubtel.Wallet.Api.Utilities;
using MapsterMapper;
using MediatR;

namespace Hubtel.Wallet.Api.Services.Handlers.Commands.Wallet;

public sealed class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, (WalletResponse?, ApiError)>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ILogger<CreateWalletCommandHandler> _logger;

    private readonly IMapper _mapper;

    public CreateWalletCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateWalletCommandHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(WalletResponse?, ApiError)> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (user, error) = await _unitOfWork.Users.GetByPhoneNumberAsync(request.OwnerPhoneNumber);

            if (error == ApiError.Exception) return (null, error);

            if (user == null) return (null, ApiError.NotFound);

            var (existingWallet, existingWalletError) = await _unitOfWork.Wallets.GetByAccountNumberAsync(request.AccountNumber);

            if (existingWalletError == ApiError.Exception) return (null, existingWalletError);

            if (existingWallet is not null) return (null, ApiError.Duplication);

            var (ownerWalletCount, countError) = await _unitOfWork.Wallets.GetOwnerWalletsCountAsync(request.OwnerPhoneNumber);

            if (countError == ApiError.Exception) return (null, countError);

            if (ownerWalletCount == 5) return (null, ApiError.WalletMaximumLimit);

            var wallet = _mapper.Map<WalletModel>(request);

            (wallet, error) = await _unitOfWork.Wallets.CreateAsync(wallet!);

            if (error == ApiError.Exception) return (null, error);

            if (wallet?.AccountType == WalletAccountType.Card)
            {
                wallet.AccountNumber = CardNumberShortener.Shorten(wallet.AccountNumber);
            }

            var (wereChangesSaved, saveException) = await _unitOfWork.CompleteAsync();

            if (saveException == ApiError.Exception) return (null, saveException);

            if (!wereChangesSaved) return (null, ApiError.SavesFailure);

            var walletResponse = _mapper.Map<WalletResponse>(wallet!);

            return (walletResponse, ApiError.None);
        }
        catch (ApplicationException ex)
        {
            _logger.LogError($"Error occurred while executing the create-wallet command handler: \n{ex}");

            return (null, ApiError.Exception);
        }
    }
}
