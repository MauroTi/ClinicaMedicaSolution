namespace ClinicaMedica.Web.Data.Dialects
{
    public class MySqlDialect : ISqlDialect
    {
        public string PrefixoParametro => "@";

        public string Limite(int quantidade)
            => $"LIMIT {quantidade}";
    }
}