namespace ClinicaMedica.Web.Data.Dialects
{
    public interface ISqlDialect
    {
        string PrefixoParametro { get; }
        string Limite(int quantidade);
    }
}