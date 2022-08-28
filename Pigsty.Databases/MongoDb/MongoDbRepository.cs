using MongoDB.Driver;
using System.Linq.Expressions;

namespace Pigsty.Databases.MongoDb;

internal class MongoDbRepository<TElement, TId> : IMongoDbRepository<TElement, TId> where TElement : class, IIdentity<TId>
{
    private readonly IMongoDatabase _mongoDatabase;

    private IMongoCollection<TElement> _collection;
    private IMongoCollection<TElement> Collection => _collection ?? (_collection = _mongoDatabase.GetCollection<TElement>(CollectionName));
    
    public string CollectionName { get; set; }

    public MongoDbRepository(IMongoDatabase mongoDatabase) => _mongoDatabase = mongoDatabase;

    public Task<TElement> GetAsync(TId id) 
        => Collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();

    public async Task<IEnumerable<TElement>> GetAllAsync(Expression<Func<TElement, bool>> predicate)
        => Collection.AsQueryable().Where(predicate);

    public Task<IEnumerable<TElement>> GetAllAsync()
        => GetAllAsync((TElement element) => true);

    public Task AddAsync(TElement element) 
        => Collection.InsertOneAsync(element);
    public Task AddManyAsync(IEnumerable<TElement> elements) 
        => Collection.InsertManyAsync(elements);

    public Task<bool> IsExistingAsync(TId id)
        => Collection.Find(_ => true).AnyAsync();

    public Task DeleteAsync(TId id)
        => Collection.DeleteOneAsync(e => e.Id.Equals(id));

    public Task UpdateAsync(TElement element)
        => Collection.ReplaceOneAsync(e => e.Id.Equals(element.Id), element);
}