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
    public partial class FormAutores : Form
    {
        private int idAutorSeleccionado = 0;
        public FormAutores()
        {
            InitializeComponent();
            CargarAutores();
        }

        private void CargarAutores()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "SELECT IdAutor, Nombre, Apellido FROM Autores";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt; // Asigna los datos a la tabla visual
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Validar que los campos obligatorios no estén vacíos
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("El nombre y el apellido son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Instanciar la clase de conexión (POO)
                ConexionBD conexionBD = new ConexionBD();

                // 3. Crear la consulta SQL parametrizada para evitar inyección SQL
                string query = "INSERT INTO Autores (Nombre, Apellido) VALUES (@Nombre, @Apellido)";

                // 4. Abrir la conexión y ejecutar la consulta
                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        // Asignar los valores de las cajas de texto a los parámetros
                        cmd.Parameters.AddWithValue("@Nombre", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Apellido", textBox2.Text.Trim());

                        cmd.ExecuteNonQuery(); // Ejecuta el guardado en la base de datos
                    }
                }

                // 5. Presentar mensaje claro de éxito y limpiar las cajas de texto
                MessageBox.Show("Autor registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                CargarAutores();
            }
            catch (Exception ex)
            {
                // Manejo adecuado de excepciones mostrando un mensaje de error
                MessageBox.Show("Ocurrió un error al guardar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormAutores_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                idAutorSeleccionado = Convert.ToInt32(fila.Cells["IdAutor"].Value);
                textBox1.Text = fila.Cells["Nombre"].Value.ToString();
                textBox2.Text = fila.Cells["Apellido"].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idAutorSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un autor de la lista haciendo clic sobre él.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "UPDATE Autores SET Nombre = @Nombre, Apellido = @Apellido WHERE IdAutor = @IdAutor";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Apellido", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@IdAutor", idAutorSeleccionado);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Autor actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Clear();
                textBox2.Clear();
                idAutorSeleccionado = 0; // Reiniciamos el ID
                CargarAutores(); // Actualizamos la tabla visualmente
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idAutorSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un autor de la lista para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 1. Confirmación de seguridad antes de borrar
            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar este autor? Esta acción no se puede deshacer.", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    ConexionBD conexionBD = new ConexionBD();
                    string query = "DELETE FROM Autores WHERE IdAutor = @IdAutor";

                    using (SqlConnection con = conexionBD.AbrirConexion())
                    {
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@IdAutor", idAutorSeleccionado);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Autor eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox1.Clear();
                    textBox2.Clear();
                    idAutorSeleccionado = 0; // Reiniciamos la memoria del ID
                    CargarAutores(); // Refrescamos la tabla
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se puede eliminar. Es posible que este autor tenga libros registrados en el sistema. Error original: " + ex.Message, "Error de Integridad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
