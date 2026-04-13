namespace ClinicaMedica.Web.Core.Entities
{
    public class Paciente : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;

        // Construtor padrão
        public Paciente() { }

        // Construtor com parâmetros
        public Paciente(string nome, string cpf, DateTime dataNascimento, string genero,
            string telefone, string email, string endereco, string cidade, string estado,
            string cep, bool ativo = true)
        {
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Genero = genero;
            Telefone = telefone;
            Email = email;
            Endereco = endereco;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            Ativo = ativo;
        }

        public void Atualizar(string nome, string cpf, DateTime dataNascimento, string genero,
            string telefone, string email, string endereco, string cidade, string estado,
            string cep, bool ativo)
        {
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Genero = genero;
            Telefone = telefone;
            Email = email;
            Endereco = endereco;
            Cidade = cidade;
            Estado = estado;
            Cep = cep;
            Ativo = ativo;
        }

        public int CalcularIdade() => DateTime.Now.Year - DataNascimento.Year;
    }
}