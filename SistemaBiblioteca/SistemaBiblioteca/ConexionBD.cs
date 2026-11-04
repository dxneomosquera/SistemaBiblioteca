using System;
using System.Data.SqlClient;

namespace SistemaBiblioteca
{
    public class ConexionBD
    {
        // Ruta exacta a tu servidor local y base de datos
        private string cadenaConexion = "Server=(localdb)\\MSSQLLocalDB;Database=SistemaBiblioteca;Integrated Security=True;";

        // Método para abrir la conexión a SQL Server
        public SqlConnection AbrirConexion()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            conexion.Open();
            return conexion;
        }
    }
}