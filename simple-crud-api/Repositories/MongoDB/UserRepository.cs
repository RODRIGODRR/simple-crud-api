﻿using MongoDB.Driver;
using simple_crud_api.Config;
using simple_crud_api.Models;
using System.Collections.Generic;
using System.Linq;

namespace simple_crud_api.Repositories.MongoDB
{
    public interface IUserRepository
    {
        IList<User> GetAll();
        User GetById(string id);
        User Post(User obj);
        User Update(string id, User obj);
        bool Delete(string id);
    }
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public bool Delete(string id)
        {
            var result = _users.DeleteOne(e => e.Id == id).DeletedCount > 0;
            return result;
        }

        public IList<User> GetAll()
        {
            var result = _users.Find(e => true).ToList();
            return result;
        }

        public User GetById(string id)
        {
            var result = _users.Find(e => e.Id == id).FirstOrDefault();
            return result;
        }

        public User Post(User obj)
        {
            _users.InsertOne(obj);
            return obj;
        }

        public User Update(string id, User obj)
        {
            _users.ReplaceOne(e => e.Id == id, obj);
            return obj;
        }
    }
}