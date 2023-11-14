namespace Hubtel.Wallet.Api.Contracts.Response;

public record ApiResponseRecord
{
    public int StatusCode { get; private set; }

    public object? Data { get; private set; }

    public object? Error { get; private set; }

    private ApiResponseRecord(string type, object? data, object? error = null)
    {
        StatusCode = GetStatusCode(type);
        Data = data; 
        Error = error;
    }

    public static ApiResponseRecord WithSuccess(string type, object data) => new(type, data);

    public static ApiResponseRecord WithFailure(string type, object error) => new(type, null, error);

    private int GetStatusCode(string type)
    {
        return type switch
        {
            "ok" => StatusCodes.Status200OK,
            "not found" => StatusCodes.Status404NotFound,
            "created" => StatusCodes.Status201Created,
            "bad request" => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}
