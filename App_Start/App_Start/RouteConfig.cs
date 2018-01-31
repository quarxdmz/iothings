using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace api
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default2",
                url: "{controller}/{action}/{id}", //url: "{controller}/{action}/{id}",
                defaults: new { action = "Index", id = UrlParameter.Optional }
            );


            //added by orly



            //  routes.MapRoute(
            //    name: "Doctor",
            //    url: "{action}/{doc}", //url: "{controller}/{action}/{id}",
            //    defaults: new { action = "Doctor", doc = UrlParameter.Optional }
            //);

            //  routes.MapRoute(
            //  name: "User",
            //  url: "{action}/{name_first}", //url: "{controller}/{action}/{id}",
            //  defaults: new { action = "Post" }
            // );

        }

        //public static void Register(HttpConfiguration config)
        //{
        //    // Web API configuration and services
        //    config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

        //    // Web API routes
        //    config.MapHttpAttributeRoutes();

        //    config.Routes.MapHttpRoute(
        //        name: "DefaultApi",
        //        routeTemplate: "{controller}/{id}",
        //        defaults: new { id = RouteParameter.Optional }
        //    );

        //}
    }
}
