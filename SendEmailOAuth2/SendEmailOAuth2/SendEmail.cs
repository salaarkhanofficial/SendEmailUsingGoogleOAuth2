using System;
using System.Threading;
using System.Threading.Tasks;
using EASendMail;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
namespace SendEmailOAuth2
{
    public class SendEmail
    {
        string clientId = "761106473078-m8u0nblbm0ecetn4jhniohda62c926pb.apps.googleusercontent.com";
        string clientSecret = "t3KZWvDuaj0TvNzVR-p-Zw6o";
        //Refresh OAuth2 Token
        async Task RefreshOAuth2TokenAsync(string clientID, string clientSecret)
        {
            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                {
                    ClientId = clientID,
                    ClientSecret = clientSecret
                },
                    new[] { "email", "profile", "https://mail.google.com/" },
                    "user",
                    CancellationToken.None
            );

            var jwtPayload = GoogleJsonWebSignature.ValidateAsync(credential.Token.IdToken).Result;
            var username = jwtPayload.Email;
            var strd = credential.Token.AccessToken;
        }

        void SendMailWithXOAUTH2(string userEmail, string accessToken, string recieverEmail, string Sbjct, string Body)
        {
            try
            {
                // Gmail SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");

                // enable SSL connection
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                // Using 587 port, you can also use 465 port
                oServer.Port = 587;

                // use Gmail SMTP OAUTH 2.0 authentication
                oServer.AuthType = SmtpAuthType.XOAUTH2;
                // set user authentication
                oServer.User = userEmail;
                // use access token as password
                oServer.Password = accessToken;

                SmtpMail oMail = new SmtpMail("TryIt");
                // Your gmail email address
                oMail.From = userEmail;
                oMail.To = recieverEmail;

                oMail.Subject = Sbjct;
                oMail.TextBody = Body;

                Console.WriteLine("start to send email using OAUTH 2.0 ...");

                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);

                Console.WriteLine("The email has been submitted to server successfully!");
            }
            catch (Exception ep)
            {
                Console.WriteLine("Exception: {0}", ep.Message);
            }
        }
    }
}
