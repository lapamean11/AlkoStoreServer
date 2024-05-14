using AlkoStoreServer.Data;
using System;

namespace AlkoStoreServer.Base
{
    public class BaseRepository
    {
        public readonly AppDbContext _dbContext; // protected

        public BaseRepository(AppDbContext context)
        {
            _dbContext = context;
        }
    }
}
