using System.Data;

namespace EmergencyData
{
    public interface IDbContextFactory
    {
        IDbConnection GetDbContext();
    }
}