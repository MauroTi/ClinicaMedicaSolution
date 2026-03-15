namespace ClinicaMedica.Web.Helpers
{
    public static class StatusConsultaHelper
    {
        private static readonly string[] StatusValidos =
        {
            "Agendada",
            "Realizada",
            "Cancelada"
        };

        public static string NormalizarStatus(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "Agendada";

            var valor = status.Trim().ToLower();

            return valor switch
            {
                "agendada" => "Agendada",
                "realizada" => "Realizada",
                "cancelada" => "Cancelada",
                _ => "Agendada"
            };
        }

        public static bool StatusEhValido(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return false;

            var normalizado = NormalizarStatus(status);
            return StatusValidos.Contains(normalizado);
        }
    }
}