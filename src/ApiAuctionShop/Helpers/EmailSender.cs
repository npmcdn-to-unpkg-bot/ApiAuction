using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAuctionShop.Helpers
{
    public static class EmailSender
    {
        public static Task SendEmailAsync(string email, string subject, string message)
        {
            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new System.Net.Mail.MailAddress("sagos30@gmail.com", "Patryk");
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;
            var credentials = new System.Net.NetworkCredential(
           "Patryk321",
            "patryk321");
            var transportWeb = new SendGrid.Web(credentials);
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }

    }
}
