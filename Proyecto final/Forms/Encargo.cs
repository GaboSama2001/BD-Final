using Proyecto_final.Forms;
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
    public partial class Encargo : Form
    {
        Database db = new Database();
        private int idVendedor;

        public Encargo(int usuario)
        {
            InitializeComponent();
            ConfigurarCarrito();
            CargarProductos();
            this.idVendedor = usuario;
        }

 

        private List<DataRow> carrito = new List<DataRow>();
        private decimal total = 0; // Para calcular el total

        private void AgregarProductoAlCarrito()
        {
            if (dataGridViewProductos.SelectedRows.Count > 0)
            {
                // Obtener datos del producto seleccionado
                var row = dataGridViewProductos.SelectedRows[0].DataBoundItem as DataRowView;
                if (row != null)
                {
                    int codigo = (int)row["Codigo"];
                    string nombre = row["Nombre"].ToString();
                    decimal precio = (decimal)row["Precio_Unit"];
                    int cantidad;

                    // Verificar que la cantidad sea válida
                    if (!int.TryParse(txtCantidad.Text, out cantidad) || cantidad <= 0)
                    {
                        MessageBox.Show("Ingrese una cantidad válida.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Calcular subtotal
                    decimal subtotal = precio * cantidad;

                    // Añadir al carrito
                    dataGridViewCarrito.Rows.Add(codigo, nombre, precio, cantidad, subtotal);

                    // Actualizar el total
                    total += subtotal;
                    txtTotal.Text = total.ToString();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un producto para agregar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EliminarProductoDelCarrito()
        {
            if (dataGridViewCarrito.SelectedRows.Count > 0)
            {
                // Obtener fila seleccionada
                var row = dataGridViewCarrito.SelectedRows[0];
                decimal subtotal = (decimal)row.Cells["Subtotal"].Value;

                // Restar subtotal al total
                total -= subtotal;
                txtTotal.Text = total.ToString();

                // Eliminar fila del carrito
                dataGridViewCarrito.Rows.Remove(row);
            }
            else
            {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void ConfigurarCarrito()
        {
            // Asegúrate de limpiar las columnas si ya existen
            dataGridViewCarrito.Columns.Clear();

            // Agregar columnas al DataGridView del carrito
          
            dataGridViewCarrito.Columns.Add("Nombre", "Producto");
            dataGridViewCarrito.Columns.Add("Precio", "Precio Unitario");
            dataGridViewCarrito.Columns.Add("Cantidad", "Cantidad");
            dataGridViewCarrito.Columns.Add("Subtotal", "Subtotal");

            // Formato para las columnas de precios
            dataGridViewCarrito.Columns["Precio"].DefaultCellStyle.Format = "C$ #,##0.00"; // Formato moneda
            dataGridViewCarrito.Columns["Subtotal"].DefaultCellStyle.Format = "C$ #,##0.00"; // Formato moneda

            // Evitar que el usuario edite directamente las celdas del carrito
            dataGridViewCarrito.AllowUserToAddRows = false; // No permitir filas nuevas manualmente
            dataGridViewCarrito.ReadOnly = true; // Solo lectura
            dataGridViewCarrito.SelectionMode = DataGridViewSelectionMode.FullRowSelect; // Selección por filas completas
        }
        private void CargarProductos(string filtro = "")
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT Codigo, Nombre, Tipo, Precio_Unit FROM ProductoServicio";

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
            }
        }
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text;
            CargarProductos(filtro);
        }

        private void dataGridViewProductos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Recibos_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dbDataSet1.Cliente' Puede moverla o quitarla según sea necesario.
            this.clienteTableAdapter1.Fill(this.dbDataSet1.Cliente);
            // TODO: esta línea de código carga datos en la tabla 'dbDataSet1.Cliente' Puede moverla o quitarla según sea necesario.
            this.clienteTableAdapter1.Fill(this.dbDataSet1.Cliente);
            // TODO: esta línea de código carga datos en la tabla 'dbDataSet1.Cliente' Puede moverla o quitarla según sea necesario.
            this.clienteTableAdapter1.Fill(this.dbDataSet1.Cliente);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            AgregarProductoAlCarrito();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LimpiarFormulario()
        {
            // Limpia el DataGridView del carrito si es necesario
            if (dataGridViewCarrito.SelectedRows.Count > 0)
            {
                dataGridViewCarrito.ClearSelection();
            }

            // Limpia los campos del recibo
            txtCantidad.Text = string.Empty;
            txtTotal.Text = "0.00"; // Reinicia el total

            // Limpia cualquier campo extra relacionado con el cliente o el producto
            cbCliente.SelectedIndex = -1; // Sin selección de cliente
        }
        private void btnFacturar_Click(object sender, EventArgs e)
        {
            // Verificar si se seleccionó un cliente
            if (cbCliente.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un cliente antes de facturar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si hay productos en el carrito
            if (dataGridViewCarrito.Rows.Count == 0)
            {
                MessageBox.Show("El carrito está vacío. Agregue productos antes de facturar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Obtener el cliente seleccionado
            var cliente = (cbCliente.SelectedItem as DataRowView)["ID"].ToString(); // ID del cliente
            if (!decimal.TryParse(txtTotal.Text, out decimal total))
            {
                MessageBox.Show("El total no es válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Conexión a la base de datos
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction()) // Usar transacción para mayor seguridad
                {
                    try
                    {
                        // Insertar encabezado del recibo
                        string queryRecibo = @"
                    INSERT INTO Recibo (Comprador, Vendedor, FechaHora, Monto)
                    OUTPUT INSERTED.Numero
                    VALUES (@Comprador, @Vendedor, @FechaHora, @Monto)";

                        SqlCommand cmdRecibo = new SqlCommand(queryRecibo, conn, trans);
                        cmdRecibo.Parameters.AddWithValue("@Comprador", cliente);
                        cmdRecibo.Parameters.AddWithValue("@Vendedor", idVendedor); // Agregar el ID del vendedor
                        cmdRecibo.Parameters.AddWithValue("@FechaHora", DateTime.Now);
                        cmdRecibo.Parameters.AddWithValue("@Monto", total);

                        // Obtener el Número del recibo generado automáticamente
                        int numeroRecibo = (int)cmdRecibo.ExecuteScalar();

                        // Insertar detalles del recibo
                        foreach (DataGridViewRow row in dataGridViewCarrito.Rows)
                        {
                            if (row.Cells["Codigo"].Value == null || row.Cells["Cantidad"].Value == null || row.Cells["Subtotal"].Value == null)
                            {
                                throw new Exception("El carrito contiene datos inválidos.");
                            }

                            int codigoProducto = Convert.ToInt32(row.Cells["Codigo"].Value);
                            int cantidad = Convert.ToInt32(row.Cells["Cantidad"].Value);
                            decimal subtotal = Convert.ToDecimal(row.Cells["Subtotal"].Value);

                            string queryDetalle = @"
                        INSERT INTO DetalleRecibo (NumeroRecibo, CodigoProducto, CantidadComprada, Total)
                        VALUES (@NumeroRecibo, @CodigoProducto, @CantidadComprada, @Total)";

                            SqlCommand cmdDetalle = new SqlCommand(queryDetalle, conn, trans);
                            cmdDetalle.Parameters.AddWithValue("@NumeroRecibo", numeroRecibo);
                            cmdDetalle.Parameters.AddWithValue("@CodigoProducto", codigoProducto);
                            cmdDetalle.Parameters.AddWithValue("@CantidadComprada", cantidad);
                            cmdDetalle.Parameters.AddWithValue("@Total", subtotal);

                            cmdDetalle.ExecuteNonQuery();
                        }

                        // Confirmar la transacción
                        trans.Commit();
                        MessageBox.Show($"Factura generada con éxito. Número de recibo: {numeroRecibo}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Limpiar carrito y total
                        dataGridViewCarrito.Rows.Clear();
                        txtTotal.Text = "0.00";
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback(); // Deshacer cambios en caso de error
                        MessageBox.Show("Error al generar la factura: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            // Limpiar el formulario después de la facturación
            LimpiarFormulario();
        }


        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            EliminarProductoDelCarrito();

        }

        private void dataGridViewCarrito_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ConfigurarCarrito();
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
