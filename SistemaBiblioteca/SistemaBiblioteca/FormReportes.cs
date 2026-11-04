using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class FormReportes : Form
    {
        public FormReportes()
        {
            InitializeComponent();
            CargarReporteActivos();
        }

        private void CargarReporteActivos()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();

                string query = @"SELECT P.IdPrestamo AS [ID Préstamo], 
                                        U.Nombre + ' ' + U.Apellido AS [Usuario], 
                                        L.Titulo AS [Libro Prestado], 
                                        P.FechaPrestamo AS [Fecha de Préstamo], 
                                        P.FechaDevolucion AS [Fecha Límite], 
                                        P.Estado
                                 FROM Prestamos P
                                 JOIN Usuarios U ON P.IdUsuario = U.IdUsuario
                                 JOIN Libros L ON P.IdLibro = L.IdLibro
                                 WHERE P.Estado = 'Activo'";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el reporte: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormReportes_Load(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}