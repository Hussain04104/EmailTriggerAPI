using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using EmailTriggerAPI.Models;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace EmailTriggerAPI.Service
{
    public class EmailService
    {
            private static string SmtpServer;
            private static int smtpPort;
            private static string SenderEmail;
            private static string SenderPassword;
            private static string ConnectionString;


        private readonly IConfiguration _configuration;
        private readonly AppointmentService _appointmentService;

        public EmailService(IConfiguration configuration, AppointmentService appointmentService)
        {
            _configuration = configuration;
            _appointmentService = appointmentService;
        }
public void SendEmail(
    string to,
    string? cc,
    string? bcc,
    string? subject,
    string? body,
    List<string>? attachments,
    string? type,
    Dictionary<string, string>? Datas)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

            SmtpServer = configuration["EmailSettings:SmtpServer"] ?? throw new Exception("Missing SMTP Server");
            smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"] ?? throw new Exception("Missing SMTP Port"));
            SenderEmail = configuration["EmailSettings:SenderEmail"] ?? throw new Exception("Missing Sender Email");
            SenderPassword = configuration["EmailSettings:SenderPassword"] ?? throw new Exception("Missing Sender Password");
            //ConnectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Missing DB Connection");
            
            //if (type != null)
            //    {
            //    //string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Appointments.html");
            //    //string templatePath = configuration.GetValue<string>("MailTemplate:Path").Replace("filePath", type);
            //    //body = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "Template not found.";
            //    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Appointments.html");
            //    body = File.ReadAllText(templatePath);
            //    //foreach (var pair in Datas)
            //    //    {
            //    //        body = body.Replace($"{{{pair.Key}}}", pair.Value);
            //    //    }
            //    }
            //string emailBody = _appointmentService.GetAppointmentEmailBody();

            if (!string.IsNullOrWhiteSpace(type))
            {
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Appointments.html");
                body = File.Exists(templatePath) ? File.ReadAllText(templatePath) : "Template not found.";

                if (Datas != null)
                {
                    foreach (var pair in Datas)
                    {
                        body = body.Replace($"{{{pair.Key}}}", pair.Value);
                    }
                }
            }
            //else
            //{
            //    body = _appointmentService.GetAppointmentEmailBody();
            //}
            using (SmtpClient client = new SmtpClient(SmtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(SenderEmail, SenderPassword);
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(SenderEmail, configuration["EmailSettings:SenderEmail"]);
                        AddMailAddresses(mail.To, to);
                        if (!string.IsNullOrEmpty(cc))
                        {
                            AddMailAddresses(mail.CC, cc);
                        }
                        if (!string.IsNullOrEmpty(bcc))
                        {
                            AddMailAddresses(mail.Bcc, bcc);
                        }
                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;
                        mail.BodyEncoding = Encoding.UTF8;
                        if (attachments != null)
                        {
                            attachments.ForEach(attachmentPath =>
                            {
                                if (File.Exists(attachmentPath))
                                {
                                    mail.Attachments.Add(new Attachment(attachmentPath));
                                }
                            });
                        }

                        client.Send(mail);
                    }
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

