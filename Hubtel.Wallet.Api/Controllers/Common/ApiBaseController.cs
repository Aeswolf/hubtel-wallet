using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hubtel.Wallet.Api.Controllers.Common;

[ApiController]
public class ApiBaseController : ControllerBase
{
    protected readonly IMapper _mapper;

    protected readonly ISender _sender;

    protected readonly ILogger<ApiBaseController> _logger;

    public ApiBaseController(ILogger<ApiBaseController> logger, ISender sender, IMapper mapper)
    {
        _logger = logger;
        _sender = sender;
        _mapper = mapper;
    }
}
