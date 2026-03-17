// ViewModels/PacienteViewModel.cs
namespace ClinicaMedica.Consumidor.ViewModels
{
    public class PacienteViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool Ativo { get; set; }
    }
}
