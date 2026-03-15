using System;
using System.Data;
using ClinicaMedica.Web.Data;
using ClinicaMedica.Web.Daos.Interfaces;

namespace ClinicaMedica.Web.Daos
{
    public class DashboardDao : IDashboardDao
    {
        private readonly DbConnectionFactory _dbConnectionFactory;

        public DashboardDao(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public int ObterTotalMedicos()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            db.Open();
            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM medicos WHERE Ativo = 1;";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int ObterTotalPacientes()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            db.Open();
            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM pacientes;";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int ObterTotalConsultas()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            db.Open();
            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM consultas;";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public int ConsultasAgendadas()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            db.Open();
            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT COUNT(*) FROM consultas;";
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public decimal ObterReceitaTotal()
        {
            using IDbConnection db = _dbConnectionFactory.CreateConnection();
            db.Open();
            using var cmd = db.CreateCommand();
            cmd.CommandText = "SELECT IFNULL(SUM(Valor),0) FROM consultas WHERE Status='Confirmada';";
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
    }
}