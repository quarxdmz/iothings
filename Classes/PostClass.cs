using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using static api.Classes.PostClass;

namespace api.Classes
{
    public class PostClass
    {
        public string APIUrl
        {
            get;
            private set;
        }
        public string endpoint
        {
            get;
            private set;
        }
        public string APIkey
        {
            get;
            private set;
        }

        public void APIProxy(string Url)
        {
            this.APIUrl = Url;
        }

        public void APIendpoint(string endpoint)
        {
            this.endpoint = endpoint;
        }
        public void API_key(string APIkey)
        {
            this.APIkey = APIkey;
        }

        public string sendRequest(string email, string fname,string lname,string status)
        {

            merge_file mf = new merge_file
            {
                FNAME = fname,
                LNAME = lname
            };  
            try
            {

                JObject jo = new JObject();
                IRestRequest request = new RestRequest(endpoint, Method.POST);
                request.JsonSerializer = new RestSharp.Serializers.NetwonsoftJsonSerializer();
                request.RequestFormat = DataFormat.Json;
                RestClient client = new RestClient(APIUrl);
                client.Authenticator = new HttpBasicAuthenticator(null, APIkey);
                jo.Add("content-type", JToken.FromObject("application/json"));
                jo.Add("email_address", JToken.FromObject(email));
                jo.Add("status", JToken.FromObject(status));
                jo.Add("merge_fields", JToken.FromObject(mf));
                request.AddBody(jo);

                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public string updateRequest(string email, string status)
        {

            members members = new members
            {
                email_address = email,
                status = status
            };

            data ob = new data
            {
               ms = new members
               {
                   email_address = email,
                   status = status
               },
                update_existing = true
            };
           

            try
            {
                JObject jo = new JObject();
                IRestRequest request = new RestRequest(endpoint, Method.PATCH);
                request.JsonSerializer = new RestSharp.Serializers.NetwonsoftJsonSerializer();
                request.RequestFormat = DataFormat.Json;
                RestClient client = new RestClient(APIUrl);
                client.Authenticator = new HttpBasicAuthenticator(null, APIkey);
                jo.Add("content-type", JToken.FromObject("application/json"));
                jo.Add("email_address", JToken.FromObject(email));
                jo.Add("status", JToken.FromObject(status));
                request.AddBody(jo);
                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public class merge_file
        {
            public string FNAME {get;set;}
            public string LNAME { get; set; }
        }

        public class members
        {
            public string email_address { get; set; }
            public string status { get; set; }
        }

        public class data
        { 
            public bool update_existing { get; set; }
            public PostClass.members ms { get; internal set; }
        }
    }

  
}