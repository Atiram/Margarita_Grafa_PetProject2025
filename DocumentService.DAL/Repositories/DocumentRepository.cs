using DocumentService.DAL.Entities;
using DocumentService.DAL.MongoDb;
using DocumentService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DocumentService.DAL.Repositories;
public class DocumentRepository : IDocumentRepository
{
    private readonly MongoDbSettings _mongoSettings;
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<DocumentEntity> _mongoCollection;

    public DocumentRepository(IOptions<MongoDbSettings> mongoDbSettings, IMongoClient mongoClient)
    {
        _mongoSettings = mongoDbSettings.Value;
        _mongoClient = mongoClient;
        _mongoDatabase = _mongoClient.GetDatabase(_mongoSettings.DatabaseName);
        _mongoCollection = _mongoDatabase.GetCollection<DocumentEntity>(_mongoSettings.CollectionName);
    }

    public async Task<DocumentEntity> GetByIdAsync(string id)
    {
        var filter = Builders<DocumentEntity>.Filter.Eq(u => u.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<DocumentEntity>> GetAllAsync()
    {
        return await _mongoCollection.Find(Builders<DocumentEntity>.Filter.Empty).ToListAsync();
    }

    public async Task<DocumentEntity> CreateAsync(DocumentEntity documentEntity)
    {
        await _mongoCollection.InsertOneAsync(documentEntity);
        return documentEntity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<DocumentEntity>.Filter.Eq(u => u.Id, id);
        var result = await _mongoCollection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
}