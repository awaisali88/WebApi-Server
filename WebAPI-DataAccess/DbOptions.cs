using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace WebAPI_DataAccess
{
    public class WebApiDbOptions : DbOptions
    {
    }

    public class NorthwindDbOptions : DbOptions
    {
    }

    public class MongoDbOptions
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
