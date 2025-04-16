//using Microsoft.AspNetCore.Mvc;
//using EmailTriggerAPI.Service;

//namespace EmailTriggerAPI.Controllers;

//[ApiController]
//[Route("emailjob")]
//public class EmailJobController : ControllerBase
//{
//    private readonly EmailService _emailService;

//    public EmailJobController(EmailService emailService)
//    {
//        _emailService = emailService;
//    }

//    [HttpPost("trigger")]
//    public async Task<IActionResult> Trigger()
//    {

//        string to = "haarif512@gmail.com";
//        string cc = "mohamedaliyar2002@gmail.com";
//        string subject = "Daily Automation Report";
        

//        _emailService.SendEmail(to, cc, null, subject, null, null, "reports", null);
//        return Ok("Email sent");
//    }
//}


using Microsoft.AspNetCore.Mvc;
using EmailTriggerAPI.Service;
using System.Threading.Tasks;
using System.Linq;

namespace EmailTriggerAPI.Controllers
{
    [ApiController]
    [Route("emailjob")]
    public class EmailJobController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;
        private readonly EmailService _emailService;

        public EmailJobController(AppointmentService appointmentService, EmailService emailService)
        {
            _appointmentService = appointmentService;
            _emailService = emailService;
        }

        [HttpPost("trigger")]
        public IActionResult Trigger()
        {
            try
            {
                var templates = _appointmentService.GetDynamicTemplateDetails();

                foreach (var template in templates.Where(t => t.Status == 1))
                {
                    var emailBody = _appointmentService.GetEmailBodyFromTemplate(template.EmailBodyHTMLPath);

                   _emailService.SendEmail(
    to: template.ToEmail,
    cc: template.CCEmail,
    bcc: template.BCCEmail,
    subject: template.Subject,
    body: emailBody,
    attachments: null,
    type: template.TemplateKey,
    Datas: null // you can replace this with actual key-value data if needed
);

                }

                return Ok("All active email templates processed and sent.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
