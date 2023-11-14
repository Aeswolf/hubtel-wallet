namespace Hubtel.Wallet.Api.Models;

public abstract class BaseModel
{
    public DateTime CreatedAt { get; set; }

    public DateTime DeletedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
