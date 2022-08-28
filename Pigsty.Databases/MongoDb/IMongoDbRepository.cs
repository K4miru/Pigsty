using System.Linq.Expressions;

namespace Pigsty.Databases.MongoDb;

public interface IMongoDbRepository<TElement, TId> where TElement : class, IIdentity<TId>
{
    string CollectionName { get; set; }
    Task<bool> IsExistingAsync(TId id);
    Task<TElement> GetAsync(TId id);
    Task<IEnumerable<TElement>> GetAllAsync(Expression<Func<TElement, bool>> predicate);
    Task<IEnumerable<TElement>> GetAllAsync();
    Task DeleteAsync(TId id);
    Task UpdateAsync(TElement element);
    Task AddAsync(TElement element);
    Task AddManyAsync(IEnumerable<TElement> elements);
}