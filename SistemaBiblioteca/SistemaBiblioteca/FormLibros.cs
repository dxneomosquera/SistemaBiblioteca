using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class FormLibros : Form
    {
        private int idLibroSeleccionado = 0;

        public FormLibros()
        {
            InitializeComponent();
            CargarAutores();
            CargarEditoriales();
            CargarLibros();
        }

        private void CargarAutores()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "SELECT IdAutor, Nombre + ' ' + Apellido AS NombreCompleto FROM Autores";
                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox1.DataSource = dt;
                    comboBox1.DisplayMember = "NombreCompleto";
                    comboBox1.ValueMember = "IdAutor";
                    comboBox1.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar autores: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarEditoriales()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "SELECT IdEditorial, Nombre FROM Editoriales";
                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    comboBox2.DataSource = dt;
                    comboBox2.DisplayMember = "Nombre";
                    comboBox2.ValueMember = "IdEditorial";
                    comboBox2.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar editoriales: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarLibros()
        {
            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = @"SELECT L.IdLibro, L.Codigo_ISBN, L.Titulo, L.Existencias, 
                                        A.Nombre + ' ' + A.Apellido AS Autor, 
                                        E.Nombre AS Editorial,
                                        L.IdAutor, L.IdEditorial
                                 FROM Libros L
                                 JOIN Autores A ON L.IdAutor = A.IdAutor
                                 JOIN Editoriales E ON L.IdEditorial = E.IdEditorial";

                using (SqlConnection con = conexionBD.AbrirConexion())
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;

                    if (dataGridView1.Columns["IdAutor"] != null) dataGridView1.Columns["IdAutor"].Visible = false;
                    if (dataGridView1.Columns["IdEditorial"] != null) dataGridView1.Columns["IdEditorial"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los libros: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            idLibroSeleccionado = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(textBox3.Text) || comboBox1.SelectedValue == null || comboBox2.SelectedValue == null)
            {
                MessageBox.Show("Todos los campos son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "INSERT INTO Libros (Codigo_ISBN, Titulo, Existencias, IdAutor, IdEditorial) VALUES (@Codigo_ISBN, @Titulo, @Existencias, @IdAutor, @IdEditorial)";

                using (SqlConnection con = conexionBD.AbrirConexion())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo_ISBN", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Titulo", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Existencias", Convert.ToInt32(textBox3.Text.Trim()));
                    cmd.Parameters.AddWithValue("@IdAutor", Convert.ToInt32(comboBox1.SelectedValue));
                    cmd.Parameters.AddWithValue("@IdEditorial", Convert.ToInt32(comboBox2.SelectedValue));
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Libro registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarLibros();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar. Verifique los datos o si el ISBN ya existe. Detalle: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un libro de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ConexionBD conexionBD = new ConexionBD();
                string query = "UPDATE Libros SET Codigo_ISBN = @Codigo_ISBN, Titulo = @Titulo, Existencias = @Existencias, IdAutor = @IdAutor, IdEditorial = @IdEditorial WHERE IdLibro = @IdLibro";

                using (SqlConnection con = conexionBD.AbrirConexion())
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Codigo_ISBN", textBox1.Text.Trim());
                    cmd.Parameters.AddWithValue("@Titulo", textBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@Existencias", Convert.ToInt32(textBox3.Text.Trim()));
                    cmd.Parameters.AddWithValue("@IdAutor", Convert.ToInt32(comboBox1.SelectedValue));
                    cmd.Parameters.AddWithValue("@IdEditorial", Convert.ToInt32(comboBox2.SelectedValue));
                    cmd.Parameters.AddWithValue("@IdLibro", idLibroSeleccionado);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Libro actualizado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarLibros();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idLibroSeleccionado == 0)
            {
                MessageBox.Show("Por favor, seleccione un libro para eliminarlo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show("¿Está seguro de que desea eliminar este libro?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion == DialogResult.Yes)
            {
                try
                {
                    ConexionBD conexionBD = new ConexionBD();
                    string query = "DELETE FROM Libros WHERE IdLibro = @IdLibro";

                    using (SqlConnection con = conexionBD.AbrirConexion())
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@IdLibro", idLibroSeleccionado);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Libro eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarLibros();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se puede eliminar. Es posible que este libro esté prestado. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                idLibroSeleccionado = Convert.ToInt32(fila.Cells["IdLibro"].Value);
                textBox1.Text = fila.Cells["Codigo_ISBN"].Value.ToString();
                textBox2.Text = fila.Cells["Titulo"].Value.ToString();
                textBox3.Text = fila.Cells["Existencias"].Value.ToString();
                comboBox1.SelectedValue = Convert.ToInt32(fila.Cells["IdAutor"].Value);
                comboBox2.SelectedValue = Convert.ToInt32(fila.Cells["IdEditorial"].Value);
            }
        }

        private void FormLibros_Load(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dataGridView1.Rows[e.RowIndex];
                idLibroSeleccionado = Convert.ToInt32(fila.Cells["IdLibro"].Value);
                textBox1.Text = fila.Cells["Codigo_ISBN"].Value.ToString();
                textBox2.Text = fila.Cells["Titulo"].Value.ToString();
                textBox3.Text = fila.Cells["Existencias"].Value.ToString();
                comboBox1.SelectedValue = Convert.ToInt32(fila.Cells["IdAutor"].Value);
                comboBox2.SelectedValue = Convert.ToInt32(fila.Cells["IdEditorial"].Value);
            }
        }
    }
}