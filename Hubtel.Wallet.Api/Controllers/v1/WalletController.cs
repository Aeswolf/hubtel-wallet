using Hubtel.Wallet.Api.Contracts.Requests.Wallet;
using Hubtel.Wallet.Api.Contracts.Response;
using Hubtel.Wallet.Api.Services.Requests.Commands.Wallet;
using Hubtel.Wallet.Api.Utilities;
using MapsterMapper;
using MediatR;
using Hubtel.Wallet.Api.Enums;
using Microsoft.AspNetCore.Mvc;
using Hubtel.Wallet.Api.Services.Requests.Queries.Wallet;
using Hubtel.Wallet.Api.Controllers.v1.Common;

namespace Hubtel.Wallet.Api.Controllers.v1;

[Route("api/v1/wallets")]
public class WalletController : ApiBaseController
{
    public WalletController(ILogger<ApiBaseController> logger, ISender sender, IMapper mapper) : base(logger, sender, mapper)
    {
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateWalletAsync(CreateWalletContract request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseRecord.WithFailure("bad request", ModelState));
        }

        if (!Validator.ValidatePhoneNumber(request.OwnerPhoneNumber))
        {
            return BadRequest(ApiResponseRecord.WithFailure("bad request", "Invalid phone number"));
        }

        if (!Validator.ValidateAccountNumberWithSchemeAndType(request.AccountNumber, request.AccountScheme, request.AccountType))
        {
            return BadRequest(ApiResponseRecord.WithFailure("bad request", $"Account number { request.AccountNumber } does not conform to scheme or type"));
        }

        try
        {
            var createWalletCommand = _mapper.Map<CreateWalletCommand>(request);

            var (walletResponse, error) = await _sender.Send(createWalletCommand);

            if (error is ApiError.Exception)
            {
                _logger.LogError($"Error occurred while trying to create a new wallet");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseRecord.WithFailure("internal server error", "Failed to create a new wallet"));
            }

            if (error is ApiError.NotFound)
            {
                return NotFound(ApiResponseRecord.WithFailure("bad request", "User not found"));
            }

            if (error is ApiError.Duplication)
            {
                return BadRequest(ApiResponseRecord.WithFailure("bad request", "Wallet already in use"));
            }

            if (error is ApiError.WalletMaximumLimit)
            {
                return BadRequest(ApiResponseRecord.WithFailure("bad request", "Specified user can not create more than 5 wallets"));
            }

            if (error is ApiError.InvalidOwner)
            {
                return BadRequest(ApiResponseRecord.WithFailure("bad request", $"Specified user do not own {request.AccountNumber}"));
            }

            if (error is ApiError.SaveFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseRecord.WithFailure("internal server error", "Failed to save new wallet details"));
            }

            return CreatedAtAction("GetWallet",
                new { walletId = walletResponse.WalletId },
                ApiResponseRecord.WithSuccess("created", walletResponse));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError($"Error occurred while trying to create a new wallet: \n{ex}");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponseRecord.WithFailure("internal server error", "Failed to create a new wallet"));
        }
    }

    [HttpGet("{walletId:guid}", Name = "GetWallet")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWalletAsync(Guid walletId)
    {
        try
        {
            var (walletResponse, error) = await _sender.Send(new GetWalletQuery(walletId));

            if (error is ApiError.Exception)
            {
                _logger.LogError($"Error occurred while trying to retrieve wallet with specified id");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseRecord.WithFailure("internal server error", "Failed to retrieve wallet with specified id"));
            }

            if (error is ApiError.NotFound)
            {
                return NotFound(ApiResponseRecord.WithFailure("not found", "Wallet with specified id was not found"));
            }

            return Ok(ApiResponseRecord.WithSuccess("ok", walletResponse));

        }
        catch (ApplicationException ex)
        {
            _logger.LogError($"Error occurred while trying to retrieve wallet with specified id: \n{ex}");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponseRecord.WithFailure("internal server error", "Failed to retrieve wallet with specified id"));
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWalletsAsync()
    {
        try
        {
            var (walletResponses, error) = await _sender.Send(new GetWalletsQuery());

            if (error is ApiError.Exception)
            {
                _logger.LogError($"Error occurred while trying to retrieve all wallets");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseRecord.WithFailure("internal server error", "Failed to retrieve all wallets"));
            }

            return Ok(ApiResponseRecord.WithSuccess("ok", walletResponses));
        }
        catch (ApplicationException ex)
        {
            _logger.LogError($"Error occurred while trying to retrieve all wallets: \n{ex}");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponseRecord.WithFailure("internal server error", "Failed to retrieve all wallets"));
        }
    }

    [HttpDelete("{walletId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveWalletAsync(Guid walletId)
    {
        try
        {
            var (isWalletDeleted, error) = await _sender.Send(new DeleteWalletCommand(walletId));

            if (error is ApiError.Exception)
            {
                _logger.LogError($"Error occurred while trying to remove wallet with specified id");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseRecord.WithFailure("internal server error", "Failed to remove wallet with specified id"));
            }

            return NoContent();
        }
        catch (ApplicationException ex)
        {
            _logger.LogError($"Error occurred while trying to remove wallet with a specified id: \n{ex}");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponseRecord.WithFailure("internal server error", "Failed to remove wallet with a specified id"));
        }
    }
}
