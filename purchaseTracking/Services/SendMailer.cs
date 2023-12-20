using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace purchaseTracking.Services
{
    /*
     * CAMBIOS 20 DE DICIEMBRE 2021 
     * AGREGAR NUMERO DE OV Y NOMBRE DE SN EN LA SOLICITUD Y MENSAJE DE NOTIFICACION
    */
    public interface IsendMailer
    {
        void sendMail(string from, string to, string subject, string path, string empleado, string actividad, string path_file);
        
    }
    public interface IsendNotification
    {
        void sendNotification(string from, string to, string subject, string ejecutivo, string solicitud, string cuerpo, string orden_venta, string sn, string path);
    }

    public class SendNotification : IsendNotification
    {
        public void sendNotification(string from, string to, string subject, string ejecutivo, string solicitud, string cuerpo, string orden_venta, string sn, string path)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Notificaciones eTALENT", from));
            email.To.AddRange(address(to));
            email.Cc.AddRange(address(from));
            email.Subject = subject;
            var mensaje = new BodyBuilder();
            string mytemplate = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText("C:\\Template\\response_notification_a.html"))
            {
                mytemplate = reader.ReadToEnd();
            }
            mytemplate = mytemplate.Replace("{ejecutivo}", ejecutivo);
            mytemplate = mytemplate.Replace("{solicitud}", solicitud);
            mytemplate = mytemplate.Replace("{cuerpo}", cuerpo);
            mensaje.HtmlBody = mytemplate;
            if (path != "")
            {
                mensaje.Attachments.Add(path);
            }
           
            email.Body = mensaje.ToMessageBody();
            email.Headers.Add("Disposition-Notification-To", "ticket@isertec.com");
            var smtp = new SmtpClient();
            smtp.Connect(Mailer.Server, Mailer.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(Mailer.SenderEmail, Mailer.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public InternetAddressList address(string direcciones)
        {
            InternetAddressList temp = new InternetAddressList();
            string[] address_ = direcciones.Split(' ', ',');
            foreach (string aux in address_)
            {
                temp.Add(MailboxAddress.Parse(aux));
            }
            return temp;
        }

        public InternetAddressList copyAddress(string direcciones)
        {
            InternetAddressList temp = new InternetAddressList();
            string[] address_ = direcciones.Split(' ', ',');
            foreach (string aux in address_)
            {
                temp.Add(MailboxAddress.Parse(aux));
            }
            return temp;
        }
    }
    public class SendMailer : IsendMailer
    {
        // path from controller
        public void sendMail(string from, string to, string subject, string path, string empleado, string actividad, string path_file)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Solicitud eTALENT", from));
            email.To.Add(new MailboxAddress("", from));
            email.Cc.AddRange(address(to));
            //crea libro electronico de direcciones para la copia del correo
            email.Cc.AddRange(copyAddress("nomina@isertec.com,ticket@isertec.com"));
            email.Subject = subject;
            var mensaje = new BodyBuilder();
            string mytemplate = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText("C:\\Template\\etalent_template.html"))
            {
                mytemplate = reader.ReadToEnd();
            }
            mytemplate = mytemplate.Replace("{empleado}", empleado);
            mytemplate = mytemplate.Replace("{actividad}", actividad);
            mytemplate = mytemplate.Replace("{link-confirm}", "https://marcaje.isertec.com/Account/listInvoice/" + actividad);
            
            mensaje.HtmlBody = mytemplate;
            if (path_file != "")
            {
                mensaje.Attachments.Add(path_file);
            }
            email.Body = mensaje.ToMessageBody();
            email.Headers.Add("Disposition-Notification-To", "ticket@isertec.com");
            var smtp = new SmtpClient();
            smtp.Connect(Mailer.Server, Mailer.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(Mailer.SenderEmail, Mailer.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        //Salida de correo unicamente de OT de emergencia de Energia y Aplicaciones
      
      
        public InternetAddressList address(string direcciones)
        {
            InternetAddressList temp = new InternetAddressList();
            string[] address_ = direcciones.Split(' ', ',');
            foreach (string aux in address_)
            {
                temp.Add(MailboxAddress.Parse(aux));
            }
            return temp;
        }

        public InternetAddressList copyAddress(string direcciones)
        {
            InternetAddressList temp = new InternetAddressList();
            string[] address_ = direcciones.Split(' ', ',');
            foreach (string aux in address_)
            {
                temp.Add(MailboxAddress.Parse(aux));
            }
            return temp;
        }
    }
}