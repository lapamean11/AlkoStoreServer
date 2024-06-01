using AlkoStoreServer.Data;
using Newtonsoft.Json;
using System;

namespace AlkoStoreServer.Base
{
    public class BaseRepository
    {
        protected readonly AppDbContext _dbContext; // protected

        public BaseRepository(AppDbContext context)
        {
            _dbContext = context;
        }

        public static string SerializeToJson<T>(T data)
        { 
            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings 
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }
    }
}
