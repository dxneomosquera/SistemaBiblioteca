using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class FormUsuarios : Form
    {
        private int idUsuarioSeleccionado = 0;

        public FormUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "SELECT IdUsuario, Nombre, Apellido, Documento, Telefono, Correo FROM Usuarios";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación de campos obligatorios (ahora usando maskedTextBox)
                if (string.IsNullOrWhiteSpace(maskedTextBox1.Text) || string.IsNullOrWhiteSpace(maskedTextBox2.Text) || string.IsNullOrWhiteSpace(maskedTextBox3.Text))
                {
                    MessageBox.Show("Nombre, Apellido y Documento son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ConexionBD conexionBD = new ConexionBD();
                string query = "INSERT INTO Usuarios (Nombre, Apellido, Documento, Telefono, Correo) VALUES (@Nombre, @Apellido, @Documento, @Telefono, @Correo)";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", maskedTextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Apellido", maskedTextBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Documento", maskedTextBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@Telefono", maskedTextBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@Correo", maskedTextBox5.Text.Trim());
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuario registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar. Verifique que el documento no esté duplicado. Detalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormUsuarios_Load(object sender, EventArgs e)
        {
        }

        private void LimpiarCampos()
        {
            maskedTextBox1.Clear();
            maskedTextBox2.Clear();
            maskedTextBox3.Clear();
            maskedTextBox4.Clear();
            maskedTextBox5.Clear();
            idUsuarioSeleccionado = 0;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                idUsuarioSeleccionado = Convert.ToInt32(fila.Cells["IdUsuario"].Value);
                maskedTextBox1.Text = fila.Cells["Nombre"].Value.ToString();
                maskedTextBox2.Text = fila.Cells["Apellido"].Value.ToString();
                maskedTextBox3.Text = fila.Cells["Documento"].Value.ToString();
                maskedTextBox4.Text = fila.Cells["Telefono"].Value.ToString();
                maskedTextBox5.Text = fila.Cells["Correo"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Documento = @Documento, Telefono = @Telefono, Correo = @Correo WHERE IdUsuario = @IdUsuario";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", maskedTextBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Apellido", maskedTextBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Documento", maskedTextBox3.Text.Trim());
                        cmd.Parameters.AddWithValue("@Telefono", maskedTextBox4.Text.Trim());
                        cmd.Parameters.AddWithValue("@Correo", maskedTextBox5.Text.Trim());
                        cmd.Parameters.AddWithValue("@IdUsuario", idUsuarioSeleccionado);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuario actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idUsuarioSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un usuario para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar este usuario?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    ConexionBD conexionBD = new ConexionBD();
                    string query = "DELETE FROM Usuarios WHERE IdUsuario = @IdUsuario";

                    using (SqlConnection con = conexionBD.AbrirConexion())
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@IdUsuario", idUsuarioSeleccionado);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Usuario eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarUsuarios();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se puede eliminar. Es posible que este usuario tenga préstamos registrados. Error: " + ex.Message, "Error de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}