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
    }
}
