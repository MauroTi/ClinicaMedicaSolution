namespace ClinicaMedica.Consumidor.Infrastructure.Database;

public static class DatabaseSession
{
    public const string SessionKey = "database";
    public const string MySql = "mysql";
    public const string Oracle = "oracle";

    public static string Normalize(string? database)
    {
        return string.Equals(database, Oracle, StringComparison.OrdinalIgnoreCase)
            ? Oracle
            : MySql;
    }
}
