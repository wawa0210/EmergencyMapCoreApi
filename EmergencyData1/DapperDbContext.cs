using System.Data;

namespace EmergencyData
{
    public class DapperDbContext : IDapperDbContext
    {
        public DapperDbContext(IDbConnection connection)
        {
            InnerConnection = connection;
        }

        protected readonly IDbConnection InnerConnection;

        public virtual IDbConnection Connection
        {
            get
            {
                return InnerConnection;
            }
        }

        public void Dispose()
        {
            if (InnerConnection != null && InnerConnection.State != ConnectionState.Closed)
            {
                InnerConnection.Close();
                InnerConnection.Dispose();
            }
            
        }
    }
}
