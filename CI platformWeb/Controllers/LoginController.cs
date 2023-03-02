using Microsoft.AspNetCore.Mvc;
using mainProjectsData.Data;
using mainProjectsData.Models;
using System.Net.Mail;
using System.Net;

namespace CI_platformWeb.Controllers
{
    public class LoginController : Controller
    {


        private readonly CIPlatformContext _db;

        public LoginController(CIPlatformContext db)
        {
            _db = db;
        }

        //--------------------------------------------------------------------- Login page ---------------------------------------------------------------------------------//

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User obj)
        {

            
            var LoginUser = _db.Users.Where(x => x.Email == obj.Email && x.Password == obj.Password).ToList().Count();
            if (LoginUser == 1)
            {
                return RedirectToAction("Index", "Home");
            }

            else if(LoginUser == 0)
            {
                TempData["UserNotFound"] = "User not found !!!";
                
            }
            
           

            return View();
        }
    //------------------------------------------------------------------------- Registration ---------------------------------------------------------------------------------//
        public IActionResult Registration()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Registration(User user)
        {
            var UserRegistration = _db.Users.Where(x => x.Email == user.Email).ToList().Count();
            string Confirmpassword = HttpContext.Request.Form["Confirmpassword"];


            if (UserRegistration == 1)
                {
                    TempData["Existant User"] = "User Already Exist, Please Try another email or phone";
                }

            else
            {
                if (Confirmpassword == null || Confirmpassword == user.Password)
                {
                    TempData["Confirmpass"] = "Confim Password Must be same as Password";
                }
                else
                {
                    _db.Users.Add(user);
                    _db.SaveChanges();
                    return RedirectToAction("Login", "Login");
                }

            }



            return RedirectToAction("Registration", "Login");
        }

    //--------------------------------------------------------------------------Forgot Password---------------------------------------------------------------------------------------//
        public IActionResult Forgotpassword()
        {
            return View();
        }        
        [HttpPost]
        public IActionResult Forgotpassword(User obj)
        {
            string Linkmail = obj.Email + DateTime.Now;
            string Token = Linkmail.GetHashCode().ToString();

            PasswordReset passwordReset = new PasswordReset();
            var lnkHref = "<a href='" + Url.Action("ResetPassword", "Login", new {Token}, "https") + "'>Reset Password</a>";
            //HTML Template for Send email
            string body = "<b>Please find the Password Reset Link. <br/><br/>" + lnkHref;
            body += "<br/><br/><strong>If haven't send this request Please Ignore the mail.</strong>";
            TempData["Token"] = Token;

            if (obj.Email == obj.Email)
            {
                var mail = HttpContext.Request.Form["Forget_mail"];

                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress("umeshchudasama06@gmail.com");
                    message.To.Add(new MailAddress(mail));
                    message.Subject = "Reseting Your Password!!";
                    message.IsBodyHtml = true; //to make message body as html
                    message.Body = body;
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com"; //for gmail host
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("umeshchudasama06@gmail.com", "habcdefgHIjk");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                    Console.WriteLine("Email sent ");


                    passwordReset.Email = mail;
                    passwordReset.Token = Token;
                    passwordReset.CreatedAt = DateTime.Now;
                    _db.PasswordResets.Add(passwordReset);
                    _db.SaveChanges();


                }
                catch (Exception ) {
                    
                }
                
            }



            return RedirectToAction("Login", "Login");
        }
//---------------------------------------------------------------------Reset password  -------------------------------------------------------------------------------//
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ResetPasswordPost()
        {
            var newPass = HttpContext.Request.Form["Password"];
            var url = HttpContext.Request.Headers.Referer.ToString();
            string Token = url.Split('=')[1];

            var ValidEmail = _db.PasswordResets.Where(x => x.Token == Token).OrderByDescending(p => p.CreatedAt).First();

            var ValidUser = _db.PasswordResets.Where(x => x.Token == Token && x.Flag == false && x.CreatedAt.AddHours(3) > DateTime.Now).Count();

            if (ValidUser > 0)
            {
                var UpdateUser = _db.Users.Where(x => x.Email == ValidEmail.Email).ToList().First();

                UpdateUser.Password = newPass;
                ValidEmail.Flag = true;
                UpdateUser.UpdatedAt = DateTime.Now;
                _db.Users.Update(UpdateUser);
                _db.PasswordResets.Update(ValidEmail);
                _db.SaveChanges();
               
            }
            else
            {
                TempData["TokenError"] = "Something Went Wrong Please try Again";
            }


            return RedirectToAction("Login", "Login");
           
            
            
        }
    }
}
