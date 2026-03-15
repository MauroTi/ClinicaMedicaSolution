namespace ClinicaMedica.Web.ViewModels.Consultas
{
    public class MedicoOptionViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty;

        public string NomeExibicao => $"{Nome} - {Especialidade}";
    }
}