using System.Data;

namespace ClinicaMedica.Web.Data
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}