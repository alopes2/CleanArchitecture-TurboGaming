namespace TurboGaming.Core.Repositories;

public interface IRepository<TDomain> 
    where TDomain : class
{
    Task<TDomain> GetByIdAsync(string id);

    Task<IEnumerable<TDomain>> GetAllAsync();

    Task AddAsync(TDomain domainModel);

    void Remove(TDomain domainModel);
}