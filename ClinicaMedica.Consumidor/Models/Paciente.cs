// Models/Paciente.cs
namespace ClinicaMedica.Consumidor.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}