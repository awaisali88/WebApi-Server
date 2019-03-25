using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;
using WebAPI_Model.GamesMDb;

namespace WebAPI_DataAccess.MongoDbContext
{
    public interface IGamesMDbContext
    {
        IMongoCollection<Game> Games { get; }
    }
}
