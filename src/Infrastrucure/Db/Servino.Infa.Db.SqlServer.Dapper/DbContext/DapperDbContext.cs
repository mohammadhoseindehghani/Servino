using System.Data;
using Microsoft.Data.SqlClient;
using Servino.Presentation.RazorPagesUI.Configurations;

namespace Servino.Infa.Db.SqlServer.Dapper.DbContext;

public class DapperDbContext(string connectionString, SiteSettings siteSettings)
{
    public IDbConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}