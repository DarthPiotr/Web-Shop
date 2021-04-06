using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
//using EASendMail;

namespace WebShop
{
    public static class Hashing
    {
        private const int iterationCount = 10000;
        public static string Hash(string password)
        {
            // Create the salt value with a cryptographic PRNG
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //Create the Rfc2898DeriveBytes and get the hash value
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterationCount);
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the salt and password bytes for later use
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Turn the combined salt+hash into a string for storage
            return Convert.ToBase64String(hashBytes);
        }

        public static bool ComparePasswords(string password, string hashedPassword)
        {
            // Extract the bytes
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            // Get the salt
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Compute the hash on the password the user entered 
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            // Compare the results
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }
    public static class Mailing
    {
        public static void SendEmail(string address, string verificationString)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("myRealEmail@gmail.com", "myRealPassword")
            };

            MailMessage mail = new MailMessage
            {
                From = new MailAddress("myRealEmail@gmail.com"),
                Subject = "Aktywacja konta",
                Body = "<h1>Aktywacja konta</h1>" +
                "<p>Dziękujemy za logowanie w serwisie AliPhon Express." +
                "Aby aktywywać konto, proszę kliknąć w poniższy link.</p>" +
                "<p><a href=\"http://localhost:56734/User/UserInfo?ver=" + verificationString + "\">Link do aktywacji</a></p>",
                BodyEncoding = System.Text.Encoding.UTF8,
                SubjectEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true
            };
            mail.To.Add(new MailAddress(address));

            smtp.Send(mail);
        }
    }
}