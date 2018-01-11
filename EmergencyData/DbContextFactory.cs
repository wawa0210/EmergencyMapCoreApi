using System;
using System.Data;

namespace EmergencyData
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly string _connectionString = string.Empty;

        public DbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        private IDbConnection _dbContext;

        private IDbConnection DbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    var t = typeof(IDbConnection);
                    this._dbContext =
                        (IDbConnection)Activator.CreateInstance(t, this._connectionString);
                }
                return _dbContext;
            }
        }

        public IDbConnection GetDbContext()
        {
            return DbContext;
        }
    }
}