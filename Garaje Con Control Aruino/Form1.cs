using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient; //Referencia para trabajar en lOCALHOST en Xamp

namespace Garaje_Con_Control_Aruino
{
    public partial class Form1 : Form
    {        
        public SerialPort ArduinoPort { get; }

        string conexionSql = "Server=localhost;Port=3306;Database=SERVOMOTOR;Uid=root;Pwd=;";
        public Form1()
        {
            InitializeComponent();            
            ArduinoPort = new SerialPort();
            ArduinoPort.PortName = "COM6"; //Checar en el equipo
            ArduinoPort.BaudRate = 9600;
            //ArduinoPort.Open();
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            //this.btnAbrir.Click += btnAbrir_Click;
            //this.Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Abre el puerto serie
                ArduinoPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el puerto serie: " + ex.Message);
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            string dato = ArduinoPort.ReadLine();
            // Actualiza el estado de la puerta en el Label
            lblEstadoPuerta.Text = dato;
        }

        private void InsertarRegistro(string abrir)
        {
            using (MySqlConnection conection = new MySqlConnection(conexionSql))
            {
                conection.Open();

                string insertQuery = "INSERT INTO REGISTROS (ABRIR)" + "VALUES (@ABRIR)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@ABRIR", abrir);
                    command.ExecuteNonQuery();
                }
                conection.Close();
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                ArduinoPort.Open();
                // Envía el comando para abrir la puerta al dispositivo
                ArduinoPort.Write("A"); // Envia el comando 'A' para abrir la puerta
                //serialPort.Open();
                lblEstadoPuerta.Text = "Abierta";
                ArduinoPort.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el comando: " + ex.Message);
            }
            string abrir = btnAbrir.Text;
            InsertarRegistro(abrir);
        }

        private void InsertarRegistro2(string cerrar)
        {
            using (MySqlConnection conection = new MySqlConnection(conexionSql))
            {
                conection.Open();

                string insertQuery = "INSERT INTO REGISTROS (CERRAR)" + "VALUES (@CERRAR)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@CERRAR", cerrar);
                    command.ExecuteNonQuery();
                }
                conection.Close();
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            try
            {
                ArduinoPort.Open();
                // Envía el comando para cerrar la puerta al dispositivo
                ArduinoPort.Write("C"); // Envia el comando 'C' para cerrar la puerta
                lblEstadoPuerta.Text = "Cerrada";
                ArduinoPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el comando: " + ex.Message);
            }

            string cerrar = btnCerrar.Text;
            InsertarRegistro2(cerrar);
        }
        private void InsertarRegistro3(string nombre, string fecha)
        {
            using (MySqlConnection conection = new MySqlConnection(conexionSql))
            {
                conection.Open();

                string insertQuery = "INSERT INTO REGISTROS (NOMBRE, FECHA)" + "VALUES (@NOMBRE, @FECHA)";

                using (MySqlCommand command = new MySqlCommand(insertQuery, conection))
                {
                    command.Parameters.AddWithValue("@NOMBRE", nombre);
                    command.Parameters.AddWithValue("@FECHA", fecha);
                    command.ExecuteNonQuery();
                }
                conection.Close();
            }
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            //Obtener los datos en los TexBox 
            string nombres = txtNombre.Text;
            string fecha = txtFecha.Text;
            InsertarRegistro3(nombres, fecha);
            MessageBox.Show("Datos ingresados Correctamente");
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            txtFecha.Clear();
            txtNombre.Clear();
        }
    }
}
