using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using ProjectFinalHorus.Context;
using ProjectFinalHorus.Infrastructure.Concrete;
using ProjectFinalHorus.ModalLogin;
using ProjectFinalHorus.ModalLogin.Abstract;
using ProjectFinalHorus.ModalLogin.Models;

namespace ProjectFinalHorus.Controllers
{
    public class ModalLoginController : Controller, ILoginController<LoginUser, ResetPasswordViewModel>
    {
        // TODO : EF DatabaseContext Sample
        private SampleDatabaseContext db = new SampleDatabaseContext();
        private MailHelper mailer = new MailHelper();

        // TODO : Sample Index Page - You can remove this.
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SignIn(string login_username, string login_password, bool login_rememberme)
        {
            ModalLoginJsonResult result = new ModalLoginJsonResult();

            login_username = login_username?.Trim();
            login_password = login_password?.Trim();

            if (string.IsNullOrEmpty(login_username) || string.IsNullOrEmpty(login_password))
            {
                result.HasError = true;
                result.Result = "Username and password can not be empty.";
            }
            else
            {
                // AsNoTracking : This should be used for example if you want to load entity only to read data and you don't plan to modify them.

                LoginUser user = db.LoginUsers.AsNoTracking().FirstOrDefault(x => x.Username == login_username && x.Password == login_password);

                if (user != null)
                {
                    result.HasError = false;
                    result.Result = "Login successfully.";

                    user.Password = string.Empty;   // Session is not include pass for security.

                    // Set login to session
                    Session["login"] = user;
                }
                else
                {
                    result.HasError = true;
                    result.Result = "Username or password is wrong.";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LostPassword(string lost_email)
        {
            ModalLoginJsonResult result = new ModalLoginJsonResult();

            lost_email = lost_email?.Trim();

            if (string.IsNullOrEmpty(lost_email))
            {
                result.HasError = true;
                result.Result = "E-Mail address can not be empty.";
            }
            else
            {
                // TODO : KMB Modal Login - Lost Password
                LoginUser user = db.LoginUsers.FirstOrDefault(x => x.Email == lost_email);

                if (user != null)
                {
                    //
                    // TODO : Send password with e-mail.
                    // Reads mail settings from AppSettings into web.config file.
                    //

                    #region Sends password to user mail address.
                    // Sends password to user mail address.
                    //bool sent = mailer.SendMail($"<b>Your password :</b> {user.Password}",
                    //user.Email, "Your missed password", true);

                    //if (sent == false)
                    //{
                    //    result.HasError = true;
                    //    result.Result = "Password has not been sent.";
                    //}
                    //else
                    //{
                    //    result.HasError = false;
                    //    result.Result = "Password has been sent.";
                    //}
                    #endregion


                    #region Sends password reset link to user mail address.
                    // Sends password reset link to user mail address.
                    user.LostPasswordToken = Guid.NewGuid();

                    if (db.SaveChanges() > 0)
                    {
                        bool sent = mailer.SendMail(
                            $"<b>Your reset password link :</b> <a href='http://{Request.Url.Authority}/ModalLogin/ResetPassword/{user.LostPasswordToken}' target='_blank'>Reset Password</a>",
                            user.Email, "Reset Password", true);

                        if (sent == false)
                        {
                            result.HasError = true;
                            result.Result = "Reset Password link has not been sent.";
                        }
                        else
                        {
                            result.HasError = false;
                            result.Result = "Reset Password link has been sent.";
                        }
                    }
                    else
                    {
                        result.HasError = true;
                        result.Result = "Error occured.";
                    }

                    #endregion


                }
                else
                {
                    result.HasError = true;
                    result.Result = "E-Mail address not found.";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SignOut()
        {
            Session.Clear();

            // TODO : Redirect Url after SignOut
            return RedirectToAction("Index");
        }

        public ActionResult UserProfile()
        {
            if (Session["login"] == null)
                return RedirectToAction("Index");

            LoginUser user = Session["login"] as LoginUser;

            return View(user);
        }

        public ActionResult EditProfile()
        {
            if (Session["login"] == null)
                return RedirectToAction("Index");

            LoginUser user = Session["login"] as LoginUser;

            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(LoginUser user, HttpPostedFileBase ProfileImage)
        {
            LoginUser usr = db.LoginUsers.Find(user.Id);

            if (user.Username != usr.Username)
            {
                // if username is using then not acceptable.
                LoginUser chk = db.LoginUsers.AsNoTracking().FirstOrDefault(x => x.Username == user.Username);

                if (chk != null)
                {
                    ModelState.AddModelError("Username", "User name is not valid.");
                    ModelState.Remove("Password");

                    return View(user);
                }
            }

            if (user.Email != usr.Email)
            {
                // if email is using then not acceptable.
                LoginUser chk = db.LoginUsers.AsNoTracking().FirstOrDefault(x => x.Email == user.Email);

                if (chk != null)
                {
                    ModelState.AddModelError("Email", "E-Mail address is not valid.");
                    ModelState.Remove("Password");

                    return View(user);
                }
            }

            if (usr != null)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string extension = ProfileImage.ContentType.Replace("image/", "");

                    ProfileImage.SaveAs(Server.MapPath($"~/images/user_{user.Id}.{extension}"));
                    usr.ProfileImageFileName = $"user_{user.Id}.{extension}";
                }

                usr.Username = user.Username;
                usr.Name = user.Name;
                usr.Surname = user.Surname;
                usr.Password = user.Password ?? usr.Password;
                usr.Email = user.Email;

                if (db.SaveChanges() > 0)
                {
                    usr.Password = string.Empty;    // Session is not include pass for security.
                    Session["login"] = usr;

                    return RedirectToAction("UserProfile");
                }
            }

            ModelState.Remove("Password");

            return View(user);
        }

        public ActionResult DeleteProfile()
        {
            if (Session["login"] == null)
                return RedirectToAction("Index");

            LoginUser user = Session["login"] as LoginUser;

            db.LoginUsers.Remove(db.LoginUsers.Find(user.Id));

            if (db.SaveChanges() > 0)
            {
                Session.Clear();
                return RedirectToAction("Index");
            }

            return RedirectToAction("UserProfile");
        }

        public ActionResult ResetPassword(Guid? id)
        {
            return View(new ResetPasswordViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(Guid? id, ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            LoginUser user = db.LoginUsers.FirstOrDefault(x => x.LostPasswordToken == id);

            if (user == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (model.Password == model.PasswordRepeat)
            {
                user.Password = model.Password;

                if (db.SaveChanges() > 0)
                {
                    // if saving is success, we are updating reset password date and reset pasword token. Because token mustn't use again.
                    user.LastResetPasswordDate = DateTime.Now;
                    user.LostPasswordToken = null;
                    db.SaveChanges();
                }
            }
            else
            {
                ModelState.AddModelError(nameof(model.PasswordRepeat), "�ifre ile �ifre tekrar uyu�muyor.");
                return View(model);
            }

            // TODO : Redirect Url after Reset Passowd
            return RedirectToAction("Index", "ModalLogin");
        }


#if DEBUG
        [HttpPost]
        public ActionResult Index(string servername, string databasename, string userid, string password, string iswinauthentication)
        {
            if (string.IsNullOrEmpty(servername) || string.IsNullOrEmpty(databasename) ||
                string.IsNullOrWhiteSpace(servername) || string.IsNullOrWhiteSpace(databasename))
            {
                ViewBag.ResultStyle = "danger";
                ViewBag.Result = "Error : Server name or database name must not be empty.";

                return View();
            }

            string connStr = "Server=" + servername + ";Database=" + databasename + "; ";

            if (iswinauthentication != null && iswinauthentication == "on")
            {
                connStr += "Integrated Security=true;";
            }
            else
            {
                connStr += "User Id=" + userid + ";Password=" + password + ";";
            }

            try
            {
                Configuration conf = WebConfigurationManager.OpenWebConfiguration("~");
                conf.ConnectionStrings.ConnectionStrings["SampleDatabaseContext"].ConnectionString = connStr;
                conf.Save();

                ViewBag.ResultStyle = "success";
                ViewBag.Result = "ConnectionString(KmbContext) saved to web.config with successfully..<br><b>First request can be a few slowly. Becase CodeFirst create database in your SQL Server. After Log-in you :) if you can write correct username and pass.(sample username is below)</b>";
            }
            catch (Exception ex)
            {
                ViewBag.ResultStyle = "danger";
                ViewBag.Result = "Error : <b>" + ex.Message + "</b>";
            }

            return View();
        }
#endif

    }
}