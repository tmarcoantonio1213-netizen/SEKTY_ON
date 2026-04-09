using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text;
using SEKTY_ON.Models;
using System.Windows.Input;
namespace SEKTY_ON
{
    public partial class RecoveryWindow : Window
    {
        public RecoveryWindow()
        {
            InitializeComponent();
        }

        private async void btnValidar_Click(object sender, RoutedEventArgs e)
        {
            string correoUsuario = txtCorreoRecuperar.Text.Trim();

            if (string.IsNullOrWhiteSpace(correoUsuario))
            {
                MessageBox.Show("Por favor, ingresa tu correo electrónico.");
                return;
            }

            this.Cursor = Cursors.Wait;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://8jr3q3p7-7060.usw3.devtunnels.ms/api/Responsables";
                    var response = await client.GetStringAsync(url);
                    var responsables = JsonConvert.DeserializeObject<List<Responsable>>(response);

                    var usuario = responsables.FirstOrDefault(r => r.Correo.Equals(correoUsuario, StringComparison.OrdinalIgnoreCase));

                    if (usuario != null)
                    {

                        string nuevaPasswordPlana = GenerarPasswordAleatoria(8);

                        using (var sha256 = System.Security.Cryptography.SHA256.Create())
                        {
                            usuario.Contraseña = sha256.ComputeHash(Encoding.UTF8.GetBytes(nuevaPasswordPlana));
                        }


                        var json = JsonConvert.SerializeObject(usuario);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        var putResponse = await client.PutAsync($"{url}/{usuario.Id}", content);

                        if (putResponse.IsSuccessStatusCode)
                        {

                            bool enviado = await Task.Run(() => EnviarEmail(usuario.Correo, nuevaPasswordPlana, usuario.Nombre));

                            if (enviado)
                            {
                                MessageBox.Show($"¡Éxito! Se ha enviado una nueva contraseña temporal a {correoUsuario}", "SEKTY-ON");
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Contraseña actualizada en sistema, pero no se pudo enviar el correo. Contacta a soporte.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error al actualizar la base de datos.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El correo no está registrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de red o servidor: " + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private string GenerarPasswordAleatoria(int longitud)
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
            Random res = new Random();
            return new string(Enumerable.Repeat(caracteres, longitud)
              .Select(s => s[res.Next(s.Length)]).ToArray());
        }

        private bool EnviarEmail(string destinatario, string password, string nombre)
        {
            try
            {

                string correoEmisor = "christianalder003@gmail.com";
                string tokenGoogle = "mqwdvenoadzkdgxk";

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(correoEmisor, "Soporte SEKTY-ON");
                mail.To.Add(destinatario);
                mail.Subject = "Nueva Contraseña de Acceso - SEKTY-ON";
                mail.IsBodyHtml = true;
                mail.Body = $@"
                    <div style='font-family: Arial; padding: 20px; border: 1px solid #660636; border-radius: 10px;'>
                        <h2 style='color: #660636;'>Hola {nombre},</h2>
                        <p>Has solicitado restablecer tu contraseña.</p>
                        <p>Tu nueva clave temporal es:</p>
                        <div style='background-color: #F0F0F0; padding: 10px; text-align: center; font-size: 20px; font-weight: bold;'>
                            {password}
                        </div>
                        <p>Por seguridad, cámbiala en cuanto inicies sesión.</p>
                        <br>
                        <p>Saludos,<br>Equipo SEKTY-ON</p>
                    </div>";

                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(correoEmisor, tokenGoogle),
                    EnableSsl = true
                };

                smtp.Send(mail);
                return true;
            }
            catch { return false; }
        }

        private void btnRegresar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}