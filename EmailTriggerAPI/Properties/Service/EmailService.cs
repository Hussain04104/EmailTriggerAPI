using EmailTriggerAPI.Properties.Models;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace EmailTriggerAPI.Properties.Service
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private static IConfiguration _config;
        private static ILogger<EmailService> _logger;


        public EmailService(IOptions<EmailSettings> emailSettings, IConfiguration config, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger;

        }
        private static string smtpServer;
        private static int smtpPort;
        private static string SenderEmail;
        private static string SenderPassword;


        public static void SendEmail(string to, string subject, string body, List<string>? attachments, string? type, Dictionary<string, string>? Datas)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                smtpServer = configuration["EmailSettings:SmtpServer"] ?? throw new Exception("Missing SMTP Server");
                smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? throw new Exception("Missing SMTP Port"));
                SenderEmail = configuration["EmailSettings:SenderEmail"] ?? throw new Exception("Missing Sender Email");
                SenderPassword = configuration["EmailSettings:SenderPassword"] ?? throw new Exception("Missing Sender Password");

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                    client.EnableSsl = true; // Try false for Office365
                    client.UseDefaultCredentials = false;

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(SenderEmail);
                        mail.To.Add(to);
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;

                        if (attachments != null)
                        {
                            foreach (var attachmentPath in attachments)
                            {
                                if (File.Exists(attachmentPath))
                                {
                                    mail.Attachments.Add(new Attachment(attachmentPath));
                                }
                            }
                        }

                        client.Send(mail);
                        _logger.LogInformation(" Email sent successfully!");
                    }
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($" SMTP Error: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" General Error: {ex.Message}");
            }
        }

        private static void AddMailAddresses(MailAddressCollection collection, string addresses)
        {
            if (!string.IsNullOrEmpty(addresses))
            {
                addresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries)
                         .ToList()
                         .ForEach(address => collection.Add(new MailAddress(address.Trim())));
            }
        }



    }

}

