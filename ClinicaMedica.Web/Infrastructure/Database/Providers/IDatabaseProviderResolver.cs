namespace ClinicaMedica.Web.Infrastructure.Database.Providers
{
    public interface IDatabaseProviderResolver
    {
        DatabaseProvider GetProvider();
    }
}