using Hubtel.Wallet.Api.Contracts.Requests.User;
using Hubtel.Wallet.Api.Contracts.Response;
using Hubtel.Wallet.Api.Controllers.Common;
using Hubtel.Wallet.Api.Services.Requests.Commands.User;
using Hubtel.Wallet.Api.Utilities;
using Hubtel.Wallet.Api.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Wallet.Api.Controllers;

[Route("api/users")]
public class UserController : ApiBaseController
{
    public UserController(ILogger<UserController> logger, ISender sender, IMapper mapper)
        : base(logger, sender, mapper) { }


    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(CreateUserContract request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ApiResponseRecord.WithFailure("bad request", ModelState));
        }

        if (!Validator.ValidatePhoneNumber(request.PhoneNumber))
        {
            return BadRequest(ApiResponseRecord.WithFailure("bad request", "Invalid phone number"));
        }

        try
        {
            var createUserCommand = _mapper.Map<CreateUserCommand>(request);

            var (userResponse, error) = await _sender.Send(createUserCommand);

            if (error is ApiError.Exception)
            {
                _logger.LogError($"Error occurred while trying to create a new user");

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseRecord.WithFailure("internal server error", "Failed to create a new user"));
            }

            if (error is ApiError.UserExists)
            {
                return BadRequest(ApiResponseRecord.WithFailure("bad request", "Phone number is already in use"));
            }

            if (error is ApiError.SavesFailure)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ApiResponseRecord.WithFailure("internal server error", "Failed to save new user details"));
            }

            return CreatedAtAction("GetUser",
                new { phoneNumber = userResponse.PhoneNumber },
                ApiResponseRecord.WithSuccess("created", userResponse));
        }
        catch(ApplicationException ex)
        {
            _logger.LogError($"Error occurred while trying to create a new user: \n{ex}");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponseRecord.WithFailure("internal server error", "Failed to create a new user"));
        }
    }

    [HttpGet("{phoneNumber}", Name="GetUser")]
    public async Task<IActionResult> GetUserAsync(string phoneNumber)
    {
        try
        {

        }
        catch(ApplicationException ex)
        {
            _logger.LogError($"Error occurred while trying to retrieve user by phone number: \n{ex}");

            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponseRecord.WithFailure("internal server error", "Failed to retrieve user by phone number"));
        }
    }
}
