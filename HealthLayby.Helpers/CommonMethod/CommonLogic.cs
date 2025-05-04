using HealthLayby.Helpers.Constant;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;

namespace HealthLayby.Helpers.CommonMethod
{
    /// <summary>
    /// Common Logic
    /// </summary>
    public static class CommonLogic
    {
        #region Encrption/Decryption functions

        /// <summary>
        /// Encrypts the specified string text.
        /// </summary>
        /// <param name="strText">The string text.</param>
        /// <returns>
        /// Convert string to base64 string
        /// </returns>
        public static string Encrypt(string strText)
        {
            byte[] byKey = { };
            byte[] IV =
            {
                18,52,86,120,144,171,205,239
            };
            byKey = System.Text.Encoding.UTF8.GetBytes("HealthLB");

#pragma warning disable SYSLIB0021 // Type or member is obsolete
            var des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // Type or member is obsolete

            byte[] inputByteArray = System.Text.Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string encryptedString = Convert.ToBase64String(ms.ToArray());
            encryptedString = encryptedString.Replace("/", "*");
            encryptedString = encryptedString.Replace("\\", "#");
            encryptedString = encryptedString.Replace("&", "$");
            encryptedString = encryptedString.Replace("+", "_");
            return encryptedString;
        }

        /// <summary>
        /// Decrypts the specified string to decrypt.
        /// </summary>
        /// <param name="stringToDecrypt">The string to decrypt.</param>
        /// <returns>
        /// Convert base64 string to string
        /// </returns>
        public static string Decrypt(string stringToDecrypt)
        {
            stringToDecrypt = stringToDecrypt.Replace("*", "/");
            stringToDecrypt = stringToDecrypt.Replace("#", "\\");
            stringToDecrypt = stringToDecrypt.Replace("$", "&");
            stringToDecrypt = stringToDecrypt.Replace("_", "+");

            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            byte[] byKey = { };
            byte[] IV =
            {
            18,52,86,120,144,171,205,239
            };
            byKey = System.Text.Encoding.UTF8.GetBytes("HealthLB");

#pragma warning disable SYSLIB0021 // Type or member is obsolete
            var des = new DESCryptoServiceProvider();
#pragma warning restore SYSLIB0021 // Type or member is obsolete

            inputByteArray = Convert.FromBase64String(stringToDecrypt);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        #endregion

        /// <summary>
        /// Gets the unique identifier string.
        /// </summary>
        /// <returns></returns>
        public static string GetGUIDString()
        {
            string GuidString = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return GuidString.Replace("/", "").Replace("\\", "").Replace("&", "").Replace("+", "");
        }

        /// <summary>
        /// Sends the email.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="toEmail">To email.</param>
        /// <param name="attachedfile">The attachedfile.</param>
        /// <returns></returns>
        public static string SendEmail(string subject, string body, string toEmail, string? attachedfile = null)
        {
            try
            {
                string clientEmail = AppSetting.ClientEmail;
                string clientEmailPassword = AppSetting.ClientEmailPassword;
                bool isEnableSSL = AppSetting.IsSSLEnabled;

                MailMessage mail = new MailMessage(clientEmail, toEmail);
                SmtpClient client = new SmtpClient();

                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(clientEmail, clientEmailPassword);
                client.TargetName = "STARTTLS/smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = isEnableSSL;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = "smtp.gmail.com";

                if (!string.IsNullOrEmpty(attachedfile))
                {
                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(attachedfile);
                    mail.Attachments.Add(attachment);
                }

                mail.Subject = MessageConstant.ProjectName + " - " + subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                client.SendMailAsync(mail).Wait();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;
        }
    }
}
