namespace ClinicaMedica.Web.Core.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(string message) : base(message)
        {
            Errors = [];
        }

        public ValidationException(Dictionary<string, string[]> errors) 
            : base("Um ou mais erros de validação ocorreram.")
        {
            Errors = errors;
        }
    }
}