using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class EmailLogic
    {
        public static void SendEmail(List<string> to, string subject, string body)
        {
            MailAddress fromMailAddress = new MailAddress(GlobalConfig.AppKeyLookup("senderEmail"), GlobalConfig.AppKeyLookup("senderDisplayName"));

            MailMessage mail = new MailMessage();
            
            to.ForEach(x => mail.To.Add(x));
            mail.From = fromMailAddress;
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            
            client.Send(mail);

        
        
        }

    }
}
