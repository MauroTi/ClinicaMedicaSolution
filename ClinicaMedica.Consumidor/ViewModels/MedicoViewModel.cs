// ViewModels/MedicoViewModel.cs
namespace ClinicaMedica.Consumidor.ViewModels
{
    public class MedicoViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Especialidade { get; set; } = null!;
        public string Crm { get; set; } = null!;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public bool Ativo { get; set; }
    }
}