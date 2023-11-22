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
            var error = await CheckOwnerExistenceAsync(request.OwnerPhoneNumber);

            if (error is not ApiError.None) return (null, error);

            if (request.AccountType is WalletAccountType.Momo)
            {
                error = await CheckMomoValidalityAsync(request.AccountNumber, request.OwnerPhoneNumber);

                if (error is not ApiError.None) return (null, error);
            }

            error = await CheckForWalletDuplication(request.AccountNumber);

            if (error is not ApiError.None) return (null, error);

            error = await CheckUserWalletLimitAsync(request.OwnerPhoneNumber);

            if (error is not ApiError.None) return (null, error);

            var wallet = _mapper.Map<WalletModel>(request);

            (wallet, error) = await CreateWalletAsync(wallet!);

            if (error is not ApiError.None) return (null, error);

            error = await _unitOfWork.SaveToDbAsync();

            if (error == ApiError.Exception) return (null, error);

            var walletResponse = _mapper.Map<WalletResponse>(wallet!);

            return (walletResponse, ApiError.None);
        }
        catch (ApplicationException ex)
        {
            _logger.LogError($"Error occurred while executing the create-wallet command handler: \n{ex}");

            return (null, ApiError.Exception);
        }
    }

    private async Task<ApiError> CheckOwnerExistenceAsync(string phoneNumber)
    {
        var (user, error) = await _unitOfWork.Users.GetByPhoneNumberAsync(phoneNumber);

        if (error == ApiError.Exception) return error;

        if (user == null) return ApiError.NotFound;

        return ApiError.None;
    }

    private async Task<ApiError> CheckForWalletDuplication(string accountNumber)
    {
        var (wallet, error) = await _unitOfWork.Wallets.GetByAccountNumberAsync(accountNumber);

        if (error is ApiError.Exception) return error;

        if (wallet is not null) return ApiError.Duplication;

        return ApiError.None;
    }

    private async Task<ApiError> CheckUserWalletLimitAsync(string phoneNumber)
    {
        var (walletCount, error) = await _unitOfWork.Wallets.GetOwnerWalletsCountAsync(phoneNumber);

        if (error is ApiError.Exception) return error;

        if (walletCount == 5) return ApiError.WalletMaximumLimit;

        return ApiError.None;
    }

    private async Task<(WalletModel?, ApiError)> CreateWalletAsync(WalletModel? wallet)
    {
        var (createdWallet, error) = await _unitOfWork.Wallets.CreateAsync(wallet!);

        if (error is ApiError.Exception) return (null, error);

        if (createdWallet?.AccountType == WalletAccountType.Card)
        {
            createdWallet.AccountNumber = CardNumberShortener.Shorten(wallet!.AccountNumber);
        }

        return (createdWallet, ApiError.None);
    }

    private async Task<ApiError> CheckMomoValidalityAsync(string phoneNumber, string ownerPhoneNumber)
    {
        var (user, error) = await _unitOfWork.Users.GetByPhoneNumberAsync(phoneNumber);

        if (error is ApiError.Exception) return error;

        if (user is not null && phoneNumber != ownerPhoneNumber) return ApiError.InvalidOwner;

        return ApiError.None;
    }
}
