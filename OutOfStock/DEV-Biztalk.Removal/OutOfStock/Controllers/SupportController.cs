using System.Web.Mvc;
using System.Net.Mail;
using OutOfStock.Models;


namespace OutOfStock.Controllers
{
    public class SupportController : Controller
    {


        public ActionResult Index()
        {
            var contactUs = new ViewModelContactUs();

            contactUs.userName = OOSUser.GetUserName();
            return View("Index", contactUs);
        }



        public JsonResult SendEmail(string name, string details)
        {
            string emailBody = "";

            emailBody = string.Format("User: {1}{0} Details: {2}{0}", "\n", name, details);

            MailAddress from = new MailAddress("OOSGlobalSupport@wholefoods.com", "Global Scanbacks Support");
            MailAddress to = new MailAddress("stacie.christensen@wholefoods.com", "Global Scanbacks Support");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Out of Stock Support Email - Website";
            message.Body = emailBody;
            SmtpClient client = new SmtpClient("smtp.wholefoods.com");
            client.Send(message);
            string data = "ok";
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}