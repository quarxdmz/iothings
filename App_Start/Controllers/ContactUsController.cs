using api.Models;
using authorization.hs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace api.Controllers
{
   
    public class ContactUsController : ApiController
    {

        SV_db1Entities dbEntity = new SV_db1Entities();
        DateTime dt = DateTime.UtcNow;

        //bool haserror = false;
        //string errmsg = "", infomsg = "";

        [System.Web.Http.HttpPost]
        [Route("contact-us")]
        public async Task<IHttpActionResult> ContactUs()
        {
            
            string email = "", name = "", message = "";
            
            string root = HttpContext.Current.Server.MapPath("~/Temp");
            var provider = new MultipartFormDataStreamProvider(root);

            DateTime dtNow = DateTime.Now;
            string msg = "";

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {

                        switch (key)
                        {
                            //string email = "", password = "", user_type = "";
                            //string acck = "deftsoftapikey", device_type = "", device_token = "";
                            case "email":
                                custom.IsRequired(key, val, 1);
                                email = val;
                                break;
                            case "name":
                                custom.IsRequired(key, val, 1);
                                name = val;
                                break;
                            case "message":
                                custom.IsRequired(key, val, 1);
                                message = val;
                                break;
                            default:
                                msg = "Object reference not set to an instance of an object. Invalid parameter name: " + key;
                                return Json(new { data = new string[] { }, message = msg, success = false });
                        }
                    }
                }

                custom.IsRequired("email", email, 2);
                custom.IsRequired("name", name, 2);
                custom.IsRequired("message", message, 2);
                if (custom.haserror)
                {
                    return Json(new { data = new string[] { }, message = custom.errmsg, success = false });
                }
                if (!IsValidEmail(email))
                {
                    return Json(new { data = new string[] { }, message = "Invalid Email", success = false });
                }

                System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress("do-not-reply@healthsplash.com");//do-not-reply@healthsplash.com
                System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress("orlysbondoc@gmail.com");

                System.Net.Mail.MailMessage emailmessage = new System.Net.Mail.MailMessage(from, to);

                emailmessage.IsBodyHtml = true;
                emailmessage.Subject = "HealthSplash-Contact Us";
                emailmessage.Body = "Name: "+name+ "<br>" + "Email: "+email+"<br>Message: "+ message;
                emailmessage.Priority = System.Net.Mail.MailPriority.Normal;
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                {
                    //UseDefaultCredentials = false,
                    Port = 587, //465,
                    Host = "box995.bluehost.com",
                    EnableSsl = true,
                    Credentials = new NetworkCredential("staff@nationalcenterforpain.com", "Staff1@#$%"),
                    Timeout = 10000
                };
                client.Send(emailmessage);

                return Json(new { data = new string[] { }, message = "Email Sent", success = true });
            }
            catch (Exception ex)
            {
                msg = "The authorization header is either not valid or isn't Basic.";
                return Json(new { data = new string[] { }, message = ex.Message, success = false });
            }

        }
        public bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address == email)
                {
                    return true;
                }
                else
                {
                    return false;
                }
              
            }
            catch
            {
                return false;
            }
        }
        //public bool IsRequired(string key, string val, int i)
        //{
        //    if (i == 1)
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += key + " is required.\r\n ";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }

        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(val))
        //        {
        //            haserror = true;
        //            errmsg += " Missing parameter: " + key + ".\r\n ";
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }

        //}

    }
}