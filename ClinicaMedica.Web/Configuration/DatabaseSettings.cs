using System;

namespace ClinicaMedica.Web.Configuration
{
    public class DatabaseSettings
    {
        public string Provider { get; set; } = "MySql";
        public string ConnectionStringName { get; set; } = "DefaultConnection";
    }
}