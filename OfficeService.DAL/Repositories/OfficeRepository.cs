using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficeService.DAL.Entities;
using OfficeService.DAL.MongoDb;
using OfficeService.DAL.Repositories.Interfaces;

namespace OfficeService.DAL.Repositories;
public class OfficeRepository : IOfficeRepository
{
    private readonly IMongoCollection<OfficeEntity> _mongoCollection;

    public OfficeRepository(IOptions<MongoDbSettings> mongoDbSettings, IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _mongoCollection = mongoDatabase.GetCollection<OfficeEntity>(mongoDbSettings.Value.CollectionName);
    }

    public async Task<OfficeEntity> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<OfficeEntity>.Filter.Eq(u => u.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<OfficeEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _mongoCollection.Find(Builders<OfficeEntity>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public async Task<OfficeEntity> CreateAsync(OfficeEntity officeEntity, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(officeEntity, new InsertOneOptions(), cancellationToken);
        return officeEntity;
    }

    public async Task<OfficeEntity?> UpdateAsync(OfficeEntity officeEntity, CancellationToken cancellationToken)
    {
        var filter = Builders<OfficeEntity>.Filter.Eq(o => o.Id, officeEntity.Id);
        var updateDefinition = Builders<OfficeEntity>.Update
            .Set(o => o.City, officeEntity.City)
            .Set(o => o.Street, officeEntity.Street)
            .Set(o => o.HouseNumber, officeEntity.HouseNumber)
            .Set(o => o.OfficeNumber, officeEntity.OfficeNumber)
            .Set(o => o.RegistryPhoneNumber, officeEntity.RegistryPhoneNumber)
            .Set(o => o.Status, officeEntity.Status)
            .Set(o => o.UpdatedAt, officeEntity.UpdatedAt);

        var result = await _mongoCollection.UpdateOneAsync(filter, updateDefinition, null, cancellationToken);

        return result.MatchedCount != 0 ? officeEntity : null;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<OfficeEntity>.Filter.Eq(u => u.Id, id);
        var result = await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }

    public async Task<List<string>> GetAllCitiesAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<OfficeEntity>.Filter.Empty;

        var cities = await _mongoCollection.Distinct(
            o => o.City,
            filter,
            null,
            cancellationToken)
        .ToListAsync(cancellationToken);

        return cities;
    }

    public async Task<List<CityStreetPair>> GetAllCitiesAndStreetsAsync(CancellationToken cancellationToken)
    {
        var filter = Builders<OfficeEntity>.Filter.Empty;

        var cityStreetPairs = await _mongoCollection.Find(filter)
                                                    .Project(o => new { o.City, o.Street })
                                                    .ToListAsync(cancellationToken);

        var uniqueCityStreetPairs = cityStreetPairs.Select(p => new CityStreetPair { City = p.City, Street = p.Street })
                                                   .Distinct()
                                                   .ToList();

        return uniqueCityStreetPairs;
    }
}
