using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;

namespace api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "DefaultApi",
               routeTemplate: "{controller}/{id}",
               defaults: new { controller="Home", id = RouteParameter.Optional }
           );

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            // config.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "{controller}/{query}",
            //     defaults: new { controller="Home", action="Get",query = RouteParameter.Optional }
            // );

            // config.Routes.MapHttpRoute(
            //   name: "UserApi2",
            //   routeTemplate: "{controller}/{query}",
            //   defaults: new { controller = "User", action = "Post", query = RouteParameter.Optional }
            //);

            //  config.Routes.MapHttpRoute(
            //      name: "User_Get",
            //      routeTemplate: "{controller}/{query}",
            //      defaults: new { query = RouteParameter.Optional },
            //      constraints: new { httpMethod = new System.Web.Routing.HttpMethodConstraint("GET") }
            //  );

            //  config.Routes.MapHttpRoute(
            //      name: "User_Post",
            //      routeTemplate: "{controller}/{query}",
            //      defaults: new { query = RouteParameter.Optional },
            //      constraints: new { httpMethod = new System.Web.Routing.HttpMethodConstraint("POST") }
            //  );

            //var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            //config.Formatters.Insert(0, jsonpFormatter);
        }
    }
}
