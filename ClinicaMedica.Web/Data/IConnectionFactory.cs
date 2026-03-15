using System.Data;

namespace ClinicaMedica.Web.Data.Interfaces
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}