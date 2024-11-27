using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Proyecto_final.Forms;
using System.Data.SqlClient;

namespace Proyecto_final
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();

        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);

        private void Inicio()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (username == "USERNAME" || string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Por favor, ingrese un nombre de usuario válido.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password == "PASSWORD" || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor, ingrese una contraseña válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validar credenciales en la base de datos
            bool credencialesValidas = ValidarCredenciales(username, password);

            if (!credencialesValidas)
            {
                MessageBox.Show("El nombre o la contraseña son incorrectos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

           
     
        

        Interfaz interfaz = new Interfaz(username);
            interfaz.Show();
            this.Hide(); // Ocultar el formulario actual
        }
 
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            if (txtUsername.Text == "USERNAME")
            {
                txtUsername.Text = "";
                txtUsername.ForeColor = Color.LightGray;
            }
        }

        private void txtUsername_Leave(object sender, EventArgs e)
        {
            if (txtUsername.Text == "")
            {
                txtUsername.Text = "USERNAME";
                txtUsername.ForeColor = Color.LightGray;
            }
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {

        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {

        }

        private void label2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_Enter_1(object sender, EventArgs e)
        {
            if (txtPassword.Text == "PASSWORD")
            {
                txtPassword.Text = "";
                txtPassword.ForeColor = Color.LightGray;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_Leave_1(object sender, EventArgs e)
        {
            if (txtPassword.Text == "")
            {
                txtPassword.Text = "PASSWORD";
                txtPassword.ForeColor = Color.LightGray;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }
        private bool ValidarCredenciales(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection("Server=WIN-H1P8F3D3IAS\\SQLEXPRESS;Database=db;Trusted_Connection=True;"))


            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Empleado WHERE Nombre = @username AND Contraseña = @password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int result = (int)cmd.ExecuteScalar();
                    return result > 0; // Si el resultado es mayor que 0, las credenciales son válidas
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            Inicio();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}