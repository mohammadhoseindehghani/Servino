using System.Data;
using Microsoft.Data.SqlClient;

namespace Servino.Infa.Db.SqlServer.Dapper.DbContext;

public class DapperDbContext(string connectionString)
{
    public IDbConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}