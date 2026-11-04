namespace SistemaBiblioteca
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormPrestamos formPrestamos = new FormPrestamos();
            formPrestamos.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormAutores formAutores = new FormAutores();
            formAutores.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormEditoriales formEditoriales = new FormEditoriales();
            formEditoriales.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FormUsuarios formUsuarios = new FormUsuarios();
            formUsuarios.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormLibros formLibros = new FormLibros();
            formLibros.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormReportes formReportes = new FormReportes();
            formReportes.Show();
        }
    }
}
