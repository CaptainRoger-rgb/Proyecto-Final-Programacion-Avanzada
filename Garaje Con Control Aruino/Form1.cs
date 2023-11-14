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

namespace Garaje_Con_Control_Aruino
{
    public partial class Form1 : Form
    {
        //Lo que se comenta es porque estara para otro codigo 
        /*public SerialPort serialPort
        {
            get;
        }*/
        //private SerialPort serialPort;

        public SerialPort ArduinoPort { get; }
        public Form1()
        {
            InitializeComponent();
            //serialPort = new SerialPort(); // Inicializa el objeto SerialPort en el constructor
            //ArduinoPort = new System.IO.Ports.SerialPort();
            //ArduinoPort = new System.IO.Ports.SerialPort();
            ArduinoPort = new SerialPort();
            ArduinoPort.PortName = "COM7"; //Checar en el equipo
            ArduinoPort.BaudRate = 9600;
            //ArduinoPort.Open();
            ArduinoPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            this.btnAbrir.Click += btnAbrir_Click;
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
        }

        //Otro Codigo
        /*private void EnviarComando(string comando)
        {
            try
            {
                if (serialPort.IsOpen)
                    serialPort.Close();

                serialPort.PortName = "COM3"; // Reemplaza con el nombre de tu puerto
                serialPort.BaudRate = 9600;
                serialPort.Open();

                // Enviar comando al Arduino
                serialPort.Write(comando);

                serialPort.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al enviar el comando: " + ex.Message);
            }
        }*/
    }
}
