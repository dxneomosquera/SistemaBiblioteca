using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SistemaBiblioteca
{
    public partial class FormEditoriales : Form
    {
        private int idEditorialSeleccionada = 0;
        public FormEditoriales()
        {
            InitializeComponent();
            CargarEditoriales();
        }
        private void CargarEditoriales()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "SELECT IdEditorial, Nombre, Pais FROM Editoriales";

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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void FormEditoriales_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar que los campos obligatorios no estén vacíos
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("El nombre y el país son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Instanciar conexión
                ConexionBD conexionBD = new ConexionBD();

                // 3. Crear la consulta SQL parametrizada
                string query = "INSERT INTO Editoriales (Nombre, Pais) VALUES (@Nombre, @Pais)";

                // 4. Ejecutar la inserción
                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pais", textBox2.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                // 5. Presentar mensaje de éxito, limpiar y recargar tabla
                MessageBox.Show("Editorial registrada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                CargarEditoriales(); // Actualiza la lista al instante
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                idEditorialSeleccionada = Convert.ToInt32(fila.Cells["IdEditorial"].Value);
                textBox1.Text = fila.Cells["Nombre"].Value.ToString();
                textBox2.Text = fila.Cells["Pais"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idEditorialSeleccionada == 0)
            {
                MessageBox.Show("Por favor, seleccione una editorial de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "UPDATE Editoriales SET Nombre = @Nombre, Pais = @Pais WHERE IdEditorial = @IdEditorial";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Pais", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@IdEditorial", idEditorialSeleccionada);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Editorial actualizada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                idEditorialSeleccionada = 0;
                CargarEditoriales();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idEditorialSeleccionada == 0)
            {
                MessageBox.Show("Por favor, seleccione una editorial para eliminarla.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar esta editorial?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    ConexionBD conexionBD = new ConexionBD();
                    string query = "DELETE FROM Editoriales WHERE IdEditorial = @IdEditorial";

                    using (SqlConnection con = conexionBD.AbrirConexion())
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@IdEditorial", idEditorialSeleccionada);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Editorial eliminada exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Clear();
                    textBox2.Clear();
                    idEditorialSeleccionada = 0;
                    CargarEditoriales();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se puede eliminar. Es posible que esta editorial tenga libros registrados. Error: " + ex.Message, "Error de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
