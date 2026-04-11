using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using SEKTY_ON.Models; 

namespace SEKTY_ON
{
    public partial class AddUserMovWindow : Window
    {
        public AddUserMovWindow()
        {
            InitializeComponent();
        }

        private async void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Por favor, llena ambos campos.");
                return;
            }


            var nuevoUsuario = new Usuario
            {
                Nombre = txtNombre.Text.Trim(),
                Contraseña = CalcularHash(txtPassword.Password), 
                Abilitado = true, 
                PuestoId = 1 
            };

            using (HttpClient client = new HttpClient())
            {
                try
                {
  
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Usuarios";

                    string json = JsonConvert.SerializeObject(nuevoUsuario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

 
                    var response = await client.PostAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Usuario guardado en la base de datos con éxito.");
                        this.DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("La base de datos rechazó el registro. Verifica el PuestoId.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error de conexión con la API: " + ex.Message);
                }
            }
        }


        private byte[] CalcularHash(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}