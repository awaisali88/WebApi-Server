using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WebAPI_Model.GamesMDb;

namespace WebAPI_DataAccess.MongoDbContext
{
    public class GamesMDbContext : IGamesMDbContext
    {
        #region Constructor
        private readonly IMongoDatabase _db;

        public GamesMDbContext(IOptions<MongoDbOptions> options, IMongoClient client)
        {
            _db = client.GetDatabase(options.Value.Database);
        }
        #endregion

        #region Repositories
        public IMongoCollection<Game> Games => _db.GetCollection<Game>("Games");

        #endregion
    }
}
