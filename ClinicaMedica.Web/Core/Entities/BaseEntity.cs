namespace ClinicaMedica.Web.Core.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime DataCadastro { get; protected set; } = DateTime.Now;
    }
}