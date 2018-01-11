using System;
using System.Data;

namespace EmergencyData
{
    public interface IDapperDbContext : IDisposable
    {
        IDbConnection Connection { get; }
    }
}
