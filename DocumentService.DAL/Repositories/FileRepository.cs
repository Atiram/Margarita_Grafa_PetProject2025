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

    public async Task<FileEntity> GetByIdAsync(string id)
    {
        var filter = Builders<FileEntity>.Filter.Eq(u => u.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<FileEntity>> GetAllAsync()
    {
        return await _mongoCollection.Find(Builders<FileEntity>.Filter.Empty).ToListAsync();
    }

    public async Task<FileEntity> CreateAsync(FileEntity fileEntity)
    {
        fileEntity.UploadedDate = DateTime.UtcNow;
        await _mongoCollection.InsertOneAsync(fileEntity);
        return fileEntity;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<FileEntity>.Filter.Eq(u => u.Id, id);
        var result = await _mongoCollection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }
}