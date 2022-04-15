using System.Linq.Expressions;
using MongoDB.Driver;

namespace SamsWebsite.Common.MongoDB
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _dbCollection;
        private FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase mongoDatabase, string collectionName)
        {
            _dbCollection = mongoDatabase.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).ToListAsync();

        }

        public async Task<T> GetAsync(Guid id)
        {
            FilterDefinition<T> filter = _filterBuilder.Eq(e => e.Id, id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> CreateAsync(T entityToCreate)
        {
            if (entityToCreate == null)
            {
                throw new ArgumentNullException();
            }

            await _dbCollection.InsertOneAsync(entityToCreate);

            return entityToCreate;
        }

        public async Task<T> UpdateAsync(T entityToUpdate) 
        {
            if (entityToUpdate == null)
            {
                throw new ArgumentNullException();
            }

            FilterDefinition<T> filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entityToUpdate.Id);
            await _dbCollection.ReplaceOneAsync(filter, entityToUpdate);

            return entityToUpdate;
        }

        public async Task RemoveAsync(Guid id) {
            FilterDefinition<T> filter = _filterBuilder.Eq(entity => entity.Id, id);
            await _dbCollection.DeleteOneAsync(filter);
        }
    }
}