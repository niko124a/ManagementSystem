using Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess.Repositories;

public class ApiAuthRepository
{
    private readonly AutoMechanicManagementSystemDbContext _dbContext;
    public ApiAuthRepository(AutoMechanicManagementSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ApiAuth> GetLatestTokenAsync()
    {
        return await _dbContext.ApiAuth
            .OrderByDescending(t => t.CreatedDate)
            .FirstOrDefaultAsync();
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return (await _dbContext.SaveChangesAsync() >= 1);
    }
}