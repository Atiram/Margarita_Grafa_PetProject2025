﻿using DocumentService.DAL.Entities;
using DocumentService.DAL.MongoDb;
using DocumentService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DocumentService.DAL.Repositories;
public class FileRepository : IFileRepository
{
    private readonly IMongoCollection<FileEntity> _mongoCollection;

    public FileRepository(IOptions<MongoDbSettings> mongoDbSettings, IMongoClient mongoClient)
    {
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _mongoCollection = mongoDatabase.GetCollection<FileEntity>(mongoDbSettings.Value.CollectionName);
    }

    public async Task<FileEntity> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<FileEntity>.Filter.Eq(u => u.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<FileEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _mongoCollection.Find(Builders<FileEntity>.Filter.Empty).ToListAsync(cancellationToken);
    }

    public async Task<FileEntity> CreateAsync(FileEntity fileEntity, CancellationToken cancellationToken = default)
    {
        fileEntity.UploadedDate = DateTime.UtcNow;
        await _mongoCollection.InsertOneAsync(fileEntity, new InsertOneOptions(), cancellationToken);
        return fileEntity;
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<FileEntity>.Filter.Eq(u => u.Id, id);
        var result = await _mongoCollection.DeleteOneAsync(filter, cancellationToken);
        return result.DeletedCount > 0;
    }
}