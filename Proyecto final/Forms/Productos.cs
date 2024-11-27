using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_final
{
    public partial class Productos : Form
    {
        Database db = new Database();
        public Productos()
        {
            InitializeComponent();
            CargarProductos();
        }

        private void Encargos_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dbDataSet.ProductoServicio' Puede moverla o quitarla según sea necesario.
            this.productoServicioTableAdapter.Fill(this.dbDataSet.ProductoServicio);

        }



        private void comboBoxClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        private void dataGridViewProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
   
        }
        private void CargarProductos(string filtro = "")
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT Codigo, Nombre, Tipo, Precio_Unit, Estado FROM ProductoServicio";  // Agregar el campo Estado

                // Agregar filtro de búsqueda si aplica
                if (!string.IsNullOrEmpty(filtro))
                {
                    query += " WHERE Nombre LIKE @Filtro OR CAST(Codigo AS NVARCHAR) LIKE @Filtro";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(filtro))
                {
                    cmd.Parameters.AddWithValue("@Filtro", $"%{filtro}%");
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridViewProductos.DataSource = dt;

                // Recorrer las filas y cambiar el color si está deshabilitado
                foreach (DataGridViewRow row in dataGridViewProductos.Rows)
                {
                    bool estado = Convert.ToBoolean(row.Cells["Estado"].Value);
                    if (estado == false)  // Si el estado es 0 (deshabilitado)
                    {
                        row.DefaultCellStyle.BackColor = Color.Gray;   // Cambiar color de fondo
                        row.DefaultCellStyle.ForeColor = Color.White;  // Cambiar color de texto
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;  // Fondo normal
                        row.DefaultCellStyle.ForeColor = Color.Black;  // Texto normal
                    }
                }
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtNombreProducto_TextChanged(object sender, EventArgs e)
        {

        }

        private void Actualizar_Click(object sender, EventArgs e)
        {

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text;
            CargarProductos(filtro);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO ProductoServicio (Codigo,Nombre, Tipo, Precio_Unit) VALUES (@Codigo ,@Nombre, @Tipo, @Precio)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Codigo", Convert.ToInt32(txtCodigo.Text));
                cmd.Parameters.AddWithValue("@Nombre", txtNombreProducto.Text);
                cmd.Parameters.AddWithValue("@Tipo", cbTipo.Text);
                cmd.Parameters.AddWithValue("@Precio", Convert.ToDecimal(txtPrecioProducto.Text));

                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Producto/Servicio agregado correctamente.");
                    CargarProductos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE ProductoServicio SET Nombre = @Nombre, Tipo = @Tipo, Precio_Unit = @Precio WHERE Codigo = @Codigo";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    // Parámetros
                    cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text);
                    cmd.Parameters.AddWithValue("@Nombre", txtNombreProducto.Text);
                    cmd.Parameters.AddWithValue("@Tipo", cbTipo.Text);
                    cmd.Parameters.AddWithValue("@Precio", Convert.ToDecimal(txtPrecioProducto.Text));

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Producto actualizado correctamente.");
                            CargarProductos(); // Recargar los productos en el DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el producto con el código especificado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al actualizar el producto: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para actualizar.");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este producto?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection conn = db.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM ProductoServicio WHERE Codigo = @Codigo";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        // Parámetro
                        cmd.Parameters.AddWithValue("@Codigo", txtCodigo.Text);

                        try
                        {
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Producto eliminado correctamente.");
                                CargarProductos(); // Recargar los productos en el DataGridView
                            }
                            else
                            {
                                MessageBox.Show("No se encontró el producto con el código especificado.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para eliminar.");
            }
        }
        private void DeshabilitarProducto(int codigoProducto)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                string query = "UPDATE ProductoServicio SET Estado = 0 WHERE Codigo = @Codigo";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Codigo", codigoProducto);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (dataGridViewProductos.SelectedRows.Count > 0)
            {
                // Obtener el código del producto seleccionado
                int codigoProducto = Convert.ToInt32(dataGridViewProductos.SelectedRows[0].Cells["Codigo"].Value);

                // Confirmar la acción
                var confirmResult = MessageBox.Show("¿Estás seguro de deshabilitar este producto?", "Confirmar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Si el usuario confirma, proceder con la deshabilitación
                    using (SqlConnection conn = db.GetConnection())
                    {
                        conn.Open();

                        string query = "UPDATE ProductoServicio SET Estado = 0 WHERE Codigo = @Codigo";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Codigo", codigoProducto);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Producto deshabilitado correctamente.");

                            // Actualizar la vista de productos
                            CargarProductos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al deshabilitar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (confirmResult == DialogResult.No)
                {
                    // Si el usuario selecciona "No", se puede realizar alguna acción si es necesario
                    MessageBox.Show("La acción de deshabilitar el producto fue cancelada.", "Acción cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Si el usuario elige "Cancelar", no hace nada y no se ejecuta ningún código adicional.
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para deshabilitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            MessageBox.Show("Seleccione un producto para deshabilitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
       

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (dataGridViewProductos.SelectedRows.Count > 0)
            {
                // Obtener el código del producto seleccionado
                int codigoProducto = Convert.ToInt32(dataGridViewProductos.SelectedRows[0].Cells["Codigo"].Value);

                // Confirmar la acción
                var confirmResult = MessageBox.Show("¿Estás seguro de habilitar este producto?", "Confirmar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    // Si el usuario confirma, proceder con la habilitación
                    using (SqlConnection conn = db.GetConnection())
                    {
                        conn.Open();

                        string query = "UPDATE ProductoServicio SET Estado = 1 WHERE Codigo = @Codigo";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@Codigo", codigoProducto);

                        try
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Producto habilitado correctamente.");

                            // Actualizar la vista de productos
                            CargarProductos();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al habilitar el producto: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else if (confirmResult == DialogResult.No)
                {
                    // Si el usuario selecciona "No", se puede realizar alguna acción si es necesario
                    MessageBox.Show("La acción de habilitar el producto fue cancelada.", "Acción cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                // Si el usuario elige "Cancelar", no hace nada y no se ejecuta ningún código adicional.
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un producto para habilitar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
