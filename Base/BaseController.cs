using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.IO;
using Amazon.S3;
using System.Web.Configuration;
using Amazon.S3.Model;
using api.Models;

namespace api.Base
{
    public class BaseController : ApiController
    {
        public bool HAS_ERROR = false;
        public string ERR_MSG = "", INFO_MSG = "";

        /// <summary>
        /// To check if the request parameter is required or not.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public bool Is_Required(string key, string val, int i)
        {
            if (i == 1)
            {
                if (string.IsNullOrEmpty(val))
                {
                    HAS_ERROR = true;
                    ERR_MSG += key + " is required.\r\n";
                    return false;
                }
                return true;
            }
            else
            {
                if (string.IsNullOrEmpty(val))
                {
                    HAS_ERROR = true;
                    ERR_MSG += " Missing parameter: " + key + ".\r\n ";
                    return false;
                }
                return true;
            }

        }

        public static bool saveClaimDoctor_ext(string _attr_name, string _dname, string _value, long doc_id = 0)
        {
            // 01/04/2018: used by Doctor signup/login (HS-67)
            DateTime dt = DateTime.UtcNow;
            SV_db1Entities dbEntity = new SV_db1Entities();
            try
            {

                var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();
                if (d_ext == null) // add attr if does not exist yet
                {
                    d_ext = new DOCTOR_ext();
                    d_ext.rel_DOCTOR_id = doc_id;
                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_create = dt;
                    d_ext.create_by__USER_id = 0;
                    dbEntity.DOCTOR_ext.Add(d_ext);
                    dbEntity.SaveChanges();
                }
                else // update the record if attr already exist
                {
                    //DOCTOR_ext d_ext = new DOCTOR_ext();
                    //var d_ext = dbEntity.DOCTOR_ext.Where(a => a.attr_name == _attr_name && a.rel_DOCTOR_id == doc_id).FirstOrDefault();

                    d_ext.attr_name = _attr_name;
                    d_ext.dname = _dname;
                    d_ext.value = _value;
                    d_ext.dt_update = dt;
                    d_ext.update_by__USER_id = 0;
                    dbEntity.Entry(d_ext).State = System.Data.Entity.EntityState.Modified;
                    dbEntity.SaveChanges();

                }
            }
            catch (Exception ex) { }


            return true;
        }


        public string new_filename;
        public bool uploadpic(MultipartFileStreamProvider f, string id)
        { 
            try
            {
                string filename = "";
                string path = HttpContext.Current.Server.MapPath("~/Content/Temp/");
                Random rand = new Random((int)DateTime.Now.Ticks);
                int numIterations = 0;
                foreach (MultipartFileData file in f.FileData)
                {
                    numIterations = rand.Next(1, 99999);
                    string ext = file.Headers.ContentDisposition.FileName.Split('.')[1];
                    filename = numIterations + "-" + id + "." + ext.Replace("\"", "");
                    string new_path = path + filename.Replace("\"", "");
                    File.Move(file.LocalFileName, new_path);
                    try
                    {
                        AmazonS3Client S3Client = null;
                        AmazonS3Config S3Config = new AmazonS3Config()
                        {
                            ServiceURL = "http://s3-external-1.amazonaws.com"
                        };
                        string accessKey = WebConfigurationManager.AppSettings["AWSaccessKey"],
                               secretAccessKey = WebConfigurationManager.AppSettings["AWSsecretAccessKey"],
                               filePath = new_path,
                               newFileName = filename;
                        S3Client = new AmazonS3Client(accessKey, secretAccessKey, S3Config);
                        var s3PutObject = new PutObjectRequest()
                        {
                            FilePath = filePath,
                            BucketName = "hsrecs" + "/images",
                            CannedACL = S3CannedACL.PublicRead
                        };
                        if (!string.IsNullOrWhiteSpace(newFileName))
                        {
                            s3PutObject.Key = newFileName;
                        }
                        s3PutObject.Headers.Expires = new DateTime(2020, 1, 1);

                        PutObjectResponse s3PutResponse = S3Client.PutObject(s3PutObject);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }


                }
                new_filename = filename;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        private  static string mauthorize = null;
        public static bool authorize
        {
            // get{ return authorize; }
            get {
                HttpContext httpContext = HttpContext.Current;
                string authHeader = httpContext.Request.Headers["Authorization"];
                if (authHeader != null && authHeader.StartsWith("Basic"))
                {

                    api.Models.SV_db1Entities db = new api.Models.SV_db1Entities();


                    string encodedUserNamePassword = authHeader.Substring("Basic ".Length).Trim();
                    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                    string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUserNamePassword));

                    int sep = usernamePassword.IndexOf(':');
                    string username = usernamePassword.Substring(0, sep).ToLower();
                    string password = usernamePassword.Substring(sep + 1);

                    var app_user = db.app_authentication.Where(a => a.name == username);
                    if (app_user.Count() > 0)
                    {
                        if (app_user.FirstOrDefault().password == password)
                        {
                            //if (username == "deftsoft" && password == "deftsoftapikey")
                            //{
                            return true;

                            //}
                        }
                    }

                    return false;

                }

                return  false;

            }

        }
               
          
       
    }
}
