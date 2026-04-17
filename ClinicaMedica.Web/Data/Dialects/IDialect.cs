namespace ClinicaMedica.Web.Data.Dialects
{
    public interface IDialect
    {
        string GetParameterPlaceholder(string name);
    }
}