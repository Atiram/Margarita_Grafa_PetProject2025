using AuthenticationService.DAL.Entities;
using AuthenticationService.DAL.MongoDb;
using AuthenticationService.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AuthenticationService.DAL.Repositories;
public class UserRepository : IUserRepository
{
    private readonly MongoDbSettings _mongoSettings;
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _mongoDatabase;
    private readonly IMongoCollection<UserEntity> _mongoCollection;

    public UserRepository(IOptions<MongoDbSettings> mongoDbSettings, IMongoClient mongoClient)
    {
        _mongoSettings = mongoDbSettings.Value;
        _mongoClient = mongoClient;
        _mongoDatabase = _mongoClient.GetDatabase(_mongoSettings.DatabaseName);
        _mongoCollection = _mongoDatabase.GetCollection<UserEntity>(_mongoSettings.CollectionName);
    }

    public async Task<UserEntity> GetByIdAsync(string id)
    {
        var filter = Builders<UserEntity>.Filter.Eq(u => u.Id, id);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<List<UserEntity>> GetAllAsync()
    {
        return await _mongoCollection.Find(Builders<UserEntity>.Filter.Empty).ToListAsync();
    }

    public async Task<UserEntity> CreateAsync(UserEntity user)
    {
        await _mongoCollection.InsertOneAsync(user);
        return user;
    }

    public async Task<bool> UpdateAsync(string id, UserEntity user)
    {
        var filter = Builders<UserEntity>.Filter.Eq(u => u.Id, id);
        var update = Builders<UserEntity>.Update
            .Set(u => u.Username, user.Username)
            .Set(u => u.Role, user.Role);

        var result = await _mongoCollection.UpdateOneAsync(filter, update);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var filter = Builders<UserEntity>.Filter.Eq(u => u.Id, id);
        var result = await _mongoCollection.DeleteOneAsync(filter);
        return result.DeletedCount > 0;
    }

    public async Task<UserEntity> GetUserByUsernameAsync(string username)
    {
        var filter = Builders<UserEntity>.Filter.Eq(u => u.Username, username);
        return await _mongoCollection.Find(filter).FirstOrDefaultAsync();
    }



    //public IMongoCollection<MyDocument> GetCollection<MyDocument>(string collectionName)
    //{
    //    IMongoCollection<MyDocument> mongoCollection = _mongoDatabase.GetCollection<MyDocument>(collectionName);
    //    return mongoCollection;
    //}
    //public async Task<List<UserEntity>> GetUser()
    //{
    //    //List<UserEntity> people = new List<UserEntity> {
    //    //new UserEntity { Username="tom", Password="12345", Role = "admin" },
    //    //new UserEntity { Username="bob", Password="12345", Role = "user" }
    //    //};

    //    var collection = _mongoDatabase.GetCollection<UserEntity>(_mongoSettings.CollectionName);
    //    var filter = Builders<UserEntity>.Filter.Eq("docName", "tom");
    //    var documents = collection.Find(filter).ToList();

    //    var people = await _mongoCollection.Find(Builders<UserEntity>.Filter.Empty).ToListAsync();

    //    return people;
    //}

    //public UserEntity Create()
    //{
    //    var document = new UserEntity
    //    {
    //        Username = "john",
    //        Password = "12345",
    //        Role = "admin"
    //    };
    //    _mongoCollection.InsertOne(document);
    //    return document;
    //}
}
