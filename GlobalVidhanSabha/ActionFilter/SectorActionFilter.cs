using System;
using System.Linq;
using System.Security.AccessControl;
using System.Web.Mvc;
using System.Web.Routing;
using VishanSabha.Services.SectorService;
using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;




namespace VishanSabha.ActionFilter
{
    public class SectorActionFilter : ActionFilterAttribute
    {
        #region Nazif

        private readonly SectorService service;
        private readonly ObjectCache _cache;

        public SectorActionFilter()
        {
            service = DependencyResolver.Current.GetService<SectorService>();
            _cache = System.Runtime.Caching.MemoryCache.Default;
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
             
                var controller = filterContext.Controller as Controller;

            if (controller == null) return;

            var Session = filterContext.HttpContext.Session;
            string contact = Session["Contact"]?.ToString();

            if (string.IsNullOrEmpty(contact))
            {
                controller.TempData["Error"] = "Session expired. Please log in again.";
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Auth", action = "Login" }));
                return;
            }

            int sectorId;
            var routeValues = filterContext.ActionParameters;

            if (routeValues.ContainsKey("sectorIdStr") && routeValues["sectorIdStr"] != null)
            {
                sectorId = Convert.ToInt32(routeValues["sectorIdStr"]);
            }
            else
            {
                sectorId = Convert.ToInt32(service.GetsectorInchargeId(contact));
                routeValues["sectorIdStr"] = sectorId;
            }

             var booths = service.GetBoothsBySectorId(sectorId, null);
             controller.ViewData["BoothList"] = booths;


            controller.ViewData["BoothLisst"] = booths;
            string BoothLisstKey = "BoothLisst";
            if (!_cache.Contains(BoothLisstKey))
            {
                var BoothLisst = service.GetBoothsBySectorId(sectorId, null);
                _cache.Add(BoothLisstKey, BoothLisst, DateTimeOffset.Now.AddHours(6));
            }
            controller.ViewData["BoothLisst"] = _cache.Get(BoothLisstKey);


            string casteListKey = "CasteList";
            if (!_cache.Contains(casteListKey))
            {
                var casteList = service.GetAllSubCaste();
                _cache.Add(casteListKey, casteList, DateTimeOffset.Now.AddHours(6));
            }
            controller.ViewData["CasteList"] = _cache.Get(casteListKey);
        }
        }
        #endregion
    }
