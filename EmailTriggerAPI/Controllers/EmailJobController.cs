using Microsoft.AspNetCore.Mvc;
using EmailTriggerAPI.Service;

namespace EmailTriggerAPI.Controllers;

[ApiController]
[Route("emailjob")]
public class EmailJobController : ControllerBase
{
    private readonly EmailService _emailService;

    public EmailJobController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("trigger")]
    public async Task<IActionResult> Trigger()
    {

        string to = "haarif512@gmail.com";
        string cc = "mohamedaliyar2002@gmail.com";
        string subject = "Daily Automation Report";
        

        EmailService.SendEmail(to, cc, null, subject, null, null, "reports", null);
        return Ok("Email sent");
    }
}
