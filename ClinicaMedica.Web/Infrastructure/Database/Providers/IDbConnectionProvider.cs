using System.Data;

namespace ClinicaMedica.Web.Infrastructure.Database.Providers
{
    public interface IDbConnectionProvider
    {
        DatabaseProvider Provider { get; }
        IDbConnection CreateConnection(string connectionString);
        string GetConnectionStringName();
    }
}