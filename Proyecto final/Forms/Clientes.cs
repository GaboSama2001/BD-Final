using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_final.Forms
{
    public partial class Clientes : Form
    {
        Database db = new Database();
        public Clientes()
        {
            InitializeComponent();
            CargarClientes();
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int lparam);




        private void btnAgregar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Cliente (Cedula, Nombre, Celular) VALUES (@Cedula, @Nombre, @Celular)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Cedula", txtCedula.Text);
                cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@Celular", txtCelular.Text);

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cliente agregado correctamente.");
                    CargarClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void CargarClientes()
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT ID, Cedula, Nombre, Celular FROM Cliente";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewClientes.DataSource = dt;
            }
        }

        private void Clientes_Load(object sender, EventArgs e)
        {

        }

        private void Actualizar_Click(object sender, EventArgs e)
        {
            if (dataGridViewClientes.SelectedRows.Count > 0)
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Cliente SET Cedula = @Cedula, Nombre = @Nombre, Celular = @Celular WHERE ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Cedula", txtCedula.Text);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Celular", txtCelular.Text);
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridViewClientes.SelectedRows[0].Cells[0].Value));

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente actualizado correctamente.");
                        CargarClientes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para editar.");
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridViewClientes.SelectedRows.Count > 0)
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "DELETE FROM Cliente WHERE ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(dataGridViewClientes.SelectedRows[0].Cells[0].Value));

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Cliente eliminado correctamente.");
                        CargarClientes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Seleccione un cliente para eliminar.");
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            Productos encargos = new Productos();
            this.Hide();    
            encargos.ShowDialog();
        }

  

        private void dataGridViewClientes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSlide_Click(object sender, EventArgs e)
        {

        }

        private void panelLateral_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panlContenedor_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
       
      
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

