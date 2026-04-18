using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

namespace ClinicaMedica.Web.Infrastructure.Database;

public static class DatabaseExceptionTranslator
{
    public static bool IsReferenceConstraintViolation(Exception exception)
    {
        return FindOracleException(exception)?.Number == 2292
            || FindMySqlException(exception)?.Number == 1451;
    }

    private static OracleException? FindOracleException(Exception? exception)
    {
        while (exception is not null)
        {
            if (exception is OracleException oracleException)
                return oracleException;

            exception = exception.InnerException;
        }

        return null;
    }

    private static MySqlException? FindMySqlException(Exception? exception)
    {
        while (exception is not null)
        {
            if (exception is MySqlException mySqlException)
                return mySqlException;

            exception = exception.InnerException;
        }

        return null;
    }
}
