using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using System.Text;
using Exceptions;
using System.Linq;
using System.Net;

namespace Screen2Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {

                var myMessage = new SendGrid.SendGridMessage();
                myMessage.AddTo("sj003@jnetsolution.com.au");
                myMessage.From = new MailAddress("no_reply@screen2.com.au", "First Last");
                myMessage.Subject = "Sending with SendGrid is Fun screen2";
                myMessage.Text = "and easy to do anywhere, even with C#";

                // Create credentials, specifying your user name and password.
                var credentials = new NetworkCredential("stevenjiangnz", "password1234");

                var transportWeb = new SendGrid.Web("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");

                //transportWeb.DeliverAsync(myMessage).Wait();
                transportWeb.DeliverAsync(myMessage).Wait();

            }
            catch (InvalidApiRequestException ex)
            {
                var detalle = new StringBuilder();

                detalle.Append("ResponseStatusCode: " + ex.ResponseStatusCode + ".   ");
                for (int i = 0; i < ex.Errors.Count(); i++)
                {
                    detalle.Append(" -- Error #" + i.ToString() + " : " + ex.Errors[i]);
                }

                throw new ApplicationException(detalle.ToString(), ex);
            }

        }
    }
}
