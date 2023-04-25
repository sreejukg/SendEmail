using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;

namespace SendMailProject.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration Configuration;

        [BindProperty]
        public string Message { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            var emailFrom = Configuration["EmailSettings:FromName"];
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(Configuration["EmailSettings:FromName"], Configuration["EmailSettings:FromEmail"]));
            email.To.Add(new MailboxAddress(Configuration["EmailSettings:ToName"], Configuration["EmailSettings:ToEmail"]));
            email.Subject = Configuration["EmailSettings:Subject"];
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Message
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.office365.com", 587, false);
                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(Configuration["EmailSettings:Username"], Configuration["EmailSettings:Password"]);

                ViewData["ResponseMessage"] += client.Send(email);

                client.Disconnect(true);
            }
        }
    }
}