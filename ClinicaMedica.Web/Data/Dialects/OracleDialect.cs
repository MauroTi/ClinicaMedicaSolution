namespace ClinicaMedica.Web.Data.Dialects
{
    public class OracleDialect : ISqlDialect
    {
        public string PrefixoParametro => ":";

        public string Limite(int quantidade)
        {
            return $"FETCH FIRST {quantidade} ROWS ONLY";
        }
    }
}