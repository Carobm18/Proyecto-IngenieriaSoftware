using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;

namespace SIERRHH.Models
{
    public class Email
    {
        public void EnviarPassword(Correo user, string passwTemp)
        {
            //se crea una instancia del objeto email
            MailMessage email = new MailMessage();

            //Asunto
            email.Subject = "Recuperar Contraseña";

            //Destinatarios
            email.To.Add(new MailAddress("sierrhh2024@outlook.com"));
            email.To.Add(new MailAddress(user.correo));

            //emisor del correo 
            email.From = new MailAddress("sierrhh2024@outlook.com");

            //se consutruye la vista html para el body del email
            string html = "Bienvenidos a nuestra plataforma SIERRHH ";
            html += "<br> A continuacion le brindamos le brindamos una contraseña temporal";
            html += "<br><b>Contraseña temporal: </b>" + passwTemp;
            html += "<br><b>Utilice esta contraseña para poder restablecer tu contraseña </b>";
            html += "<br><b>No responda este correo porque fue generado de forma automatica ";
            html += "por la plataforma SIERRHH</b>";

            //se indica el contenido esen html
            email.IsBodyHtml = true;

            //se indica la prioridad recomendacion debe ser prioridad normal 
            email.Priority = MailPriority.Normal;

            //se instancia la vista del html para el campo del body del email
            AlternateView view = AlternateView.CreateAlternateViewFromString(html,
                Encoding.UTF8, MediaTypeNames.Text.Html);

            //se agrega la vista html al cuerpo del email 
            email.AlternateViews.Add(view);

            //configuracion del protocolo de comunicacion smtp 
            SmtpClient smtp = new SmtpClient();

            //servidor de correo a implementar 
            smtp.Host = "smtp-mail.outlook.com";

            //puerto de comunicacion 
            smtp.Port = 587;

            //se indica si el buton utiliza seguridad tipo SSL 
            smtp.EnableSsl = true;

            //se indica si el buzon utiliza credenciales por default
            smtp.UseDefaultCredentials = false;

            //se asignan los datos para los credenciales 
            smtp.Credentials = new NetworkCredential("sierrhh2024@outlook.com", "SIERRHHLCJHAV@");

            //metodo para enviar el email
            smtp.Send(email);

            email.Dispose();
            smtp.Dispose();
        }

        public void EnviarCorreoConcurso(String correo, String nombreCompleto, String puesto)
        {
            // Se crea una instancia del objeto email
            MailMessage email = new MailMessage();

            // Asunto
            email.Subject = "¡Felicitaciones! Ha ganado el concurso para el puesto";

            // Destinatarios
            email.To.Add(new MailAddress("sierrhh2024@outlook.com"));
            email.To.Add(new MailAddress(correo));

            // Emisor del correo 
            email.From = new MailAddress("sierrhh2024@outlook.com");

            // Se construye la vista HTML para el body del email
            string html = "<h1 style='color:#1E90FF;'>¡Felicidades!</h1>";
            html += "<p>Estimado/a <b>" + nombreCompleto + "</b>,</p>";
            html += "<p>Nos complace informarle que ha sido seleccionado/a como el/la ganador/a del concurso para el puesto en nuestra empresa.</p>";
            html += "<p>Queremos agradecerle por su participación y el esfuerzo demostrado durante el proceso de selección.</p>";
            html += "<p>A continuación, le brindamos el nombre del puesto:</p>";
            html += "<ul>";
            html += "<li><b>Puesto:</b> " + puesto + "</li>";
            html += "</ul>";
            html += "<p>Por favor, contactese con recursos humanos al numero 8888-8888 o ir a la oficina para mas información.</p>";
            html += "<p>¡Esperamos verle pronto!</p>";
            html += "<p style='color:#1E90FF;'><b>Saludos cordiales,</b></p>";
            html += "<p>Equipo de Recursos Humanos</p>";
            html += "<p><i>Nota: No responda a este correo ya que fue generado automáticamente por nuestra plataforma SIERRHH.</i></p>";

            // Se indica que el contenido es HTML
            email.IsBodyHtml = true;

            // Se indica la prioridad recomendación debe ser prioridad normal 
            email.Priority = MailPriority.Normal;

            // Se instancia la vista del HTML para el campo del body del email
            AlternateView view = AlternateView.CreateAlternateViewFromString(html,
                Encoding.UTF8, MediaTypeNames.Text.Html);

            // Se agrega la vista HTML al cuerpo del email 
            email.AlternateViews.Add(view);

            // Configuración del protocolo de comunicación SMTP 
            SmtpClient smtp = new SmtpClient();

            // Servidor de correo a implementar 
            smtp.Host = "smtp-mail.outlook.com";

            // Puerto de comunicación 
            smtp.Port = 587;

            // Se indica si el buzón utiliza seguridad tipo SSL 
            smtp.EnableSsl = true;

            // Se indica si el buzón utiliza credenciales por default
            smtp.UseDefaultCredentials = false;

            // Se asignan los datos para las credenciales 
            smtp.Credentials = new NetworkCredential("sierrhh2024@outlook.com", "SIERRHHLCJHAV@");

            // Método para enviar el email
            smtp.Send(email);

            email.Dispose();
            smtp.Dispose();
        }



    }
}
