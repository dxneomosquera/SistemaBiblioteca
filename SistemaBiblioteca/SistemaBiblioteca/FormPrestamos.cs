using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class FormPrestamos : Form
    {
        private int idPrestamoSeleccionado = 0;

        public FormPrestamos()
        {
            InitializeComponent();
            CargarUsuarios();
            CargarLibros();
            CargarEstados();
            CargarPrestamos();
        }

        private void CargarUsuarios()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "SELECT IdUsuario, Nombre + ' ' + Apellido AS NombreCompleto FROM Usuarios";
                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "NombreCompleto";
                    comboBox1.ValueMember = "IdUsuario";
                    comboBox1.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar usuarios: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarLibros()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "SELECT IdLibro, Titulo FROM Libros";
                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "Titulo";
                    comboBox2.ValueMember = "IdLibro";
                    comboBox2.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar libros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEstados()
        {
            comboBox3.Items.Clear();
            comboBox3.Items.Add("Activo");
            comboBox3.Items.Add("Devuelto");
            comboBox3.SelectedIndex = -1;
        }

        private void CargarPrestamos()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = @"SELECT P.IdPrestamo, 
                                        U.Nombre + ' ' + U.Apellido AS Usuario, 
                                        L.Titulo AS Libro, 
                                        P.FechaPrestamo, P.FechaDevolucion, P.Estado,
                                        P.IdUsuario, P.IdLibro
                                 FROM Prestamos P
                                 JOIN Usuarios U ON P.IdUsuario = U.IdUsuario
                                 JOIN Libros L ON P.IdLibro = L.IdLibro";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns["IdUsuario"] != null) dataGridView1.Columns["IdUsuario"].Visible = false;
                    if (dataGridView1.Columns["IdLibro"] != null) dataGridView1.Columns["IdLibro"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los préstamos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            idPrestamoSeleccionado = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null || comboBox2.SelectedValue == null || comboBox3.SelectedItem == null)
            {
                MessageBox.Show("Por favor, seleccione el usuario, el libro y el estado.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "INSERT INTO Prestamos (IdUsuario, IdLibro, FechaPrestamo, FechaDevolucion, Estado) VALUES (@IdUsuario, @IdLibro, @FechaPrestamo, @FechaDevolucion, @Estado)";

                using (SqlConnection con = conexionBD.AbrirConexion())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", Convert.ToInt32(comboBox1.SelectedValue));
                    cmd.Parameters.AddWithValue("@IdLibro", Convert.ToInt32(comboBox2.SelectedValue));
                    cmd.Parameters.AddWithValue("@FechaPrestamo", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@FechaDevolucion", dateTimePicker2.Value.Date);
                    cmd.Parameters.AddWithValue("@Estado", comboBox3.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Préstamo registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarPrestamos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idPrestamoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un préstamo de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "UPDATE Prestamos SET IdUsuario = @IdUsuario, IdLibro = @IdLibro, FechaPrestamo = @FechaPrestamo, FechaDevolucion = @FechaDevolucion, Estado = @Estado WHERE IdPrestamo = @IdPrestamo";

                using (SqlConnection con = conexionBD.AbrirConexion())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdUsuario", Convert.ToInt32(comboBox1.SelectedValue));
                    cmd.Parameters.AddWithValue("@IdLibro", Convert.ToInt32(comboBox2.SelectedValue));
                    cmd.Parameters.AddWithValue("@FechaPrestamo", dateTimePicker1.Value.Date);
                    cmd.Parameters.AddWithValue("@FechaDevolucion", dateTimePicker2.Value.Date);
                    cmd.Parameters.AddWithValue("@Estado", comboBox3.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamoSeleccionado);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Préstamo actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarPrestamos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idPrestamoSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un préstamo para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar este préstamo?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    ConexionBD conexionBD = new ConexionBD();
                    string query = "DELETE FROM Prestamos WHERE IdPrestamo = @IdPrestamo";

                    using (SqlConnection con = conexionBD.AbrirConexion())
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@IdPrestamo", idPrestamoSeleccionado);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Préstamo eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarPrestamos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                idPrestamoSeleccionado = Convert.ToInt32(fila.Cells["IdPrestamo"].Value);
                comboBox1.SelectedValue = Convert.ToInt32(fila.Cells["IdUsuario"].Value);
                comboBox2.SelectedValue = Convert.ToInt32(fila.Cells["IdLibro"].Value);

                if (fila.Cells["FechaPrestamo"].Value != DBNull.Value)
                    dateTimePicker1.Value = Convert.ToDateTime(fila.Cells["FechaPrestamo"].Value);

                if (fila.Cells["FechaDevolucion"].Value != DBNull.Value)
                    dateTimePicker2.Value = Convert.ToDateTime(fila.Cells["FechaDevolucion"].Value);

                comboBox3.SelectedItem = fila.Cells["Estado"].Value.ToString();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }
    }
}