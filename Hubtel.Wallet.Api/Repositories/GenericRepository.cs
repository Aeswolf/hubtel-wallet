using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Interfaces;
using Hubtel.Wallet.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Hubtel.Wallet.Api.Repositories;

public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : BaseModel
{
    protected readonly DbSet<TModel> _dbSet;

    protected readonly ILogger _logger;

    public GenericRepository(DbContext dbContext, ILogger logger)
    {
        _dbSet = dbContext.Set<TModel>();
        _logger = logger;
    }

    public async Task<(TModel?, ApiError)> CreateAsync(TModel model)
    {
        try
        {
            await _dbSet.AddAsync(model);

            return (model, ApiError.None);
        }
        catch(DbException ex)
        {
            _logger.LogError($"Error occurred while creating a new instance of {model.GetType()} : \n{ex}");

            return (null, ApiError.Exception);
        }
    }

    public async Task<(IReadOnlyCollection<TModel>, ApiError)> GetAllAsync()
    {
        try
        {
            var instances = await _dbSet.Where(instance => !instance.IsDeleted).AsNoTracking()
                                        .AsSplitQuery().ToListAsync();

            return (instances, ApiError.None);
        }
        catch(DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving instances from database  : \n{ex}");

            return (null!, ApiError.Exception);
        }
    }

    public async Task<(bool, ApiError)> RemoveAllAsync()
    {
        try
        {
            var instances = await _dbSet.Where(instance => instance.IsDeleted).ToListAsync();

            _dbSet.RemoveRange(instances);

            return (true, ApiError.None);
        }
        catch (DbException ex)
        {
            _logger.LogError($"Error occurred while retrieving instances from database  : \n{ex}");

            return (false, ApiError.Exception);
        }
    }
}
