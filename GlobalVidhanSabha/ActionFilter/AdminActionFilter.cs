using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using VishanSabha.Models;
using VishanSabha.Services;



namespace VishanSabha.ActionFilter
{
    #region Nazif
    public class AdminActionFilter : ActionFilterAttribute
    {
        private readonly AdminServices service;

        public AdminActionFilter()
        {
            service = DependencyResolver.Current.GetService<AdminServices>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;
            int VidhanId = 0;
            if (session != null)
            {
                var VidhanSabhaId = session["VidhanSabhaId"];   // get session value
                VidhanId = Convert.ToInt32(VidhanSabhaId);
                // Example check
                if (VidhanSabhaId == null)
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
            } 

            var cache = MemoryCache.Default;
            var controller = filterContext.Controller as Controller;

            if (controller == null) return;

            var sahmatTypes = service.getSahmatAsahmatType() ?? new List<SahmatAsahmatType>();
            controller.ViewBag.SahmatTypes = sahmatTypes;


            var prabhavsali = cache["Prabhavsali"] as List<EffectiveDesignation> ?? service.GetEffectiveDesignations();
            cache.Set("Prabhavsali", prabhavsali, DateTimeOffset.Now.AddMinutes(60));
            controller.ViewBag.Prabhavsali = prabhavsali;

            controller.ViewBag.allowedIds = new[] {  8, 9, 10};


            var seniOrrordisabled = service.GetSeniorDisabledtype() ?? new List<SeniorDisabled>();
            controller.ViewBag.seniOrrordisabled = seniOrrordisabled;


            var Booths = cache["Booths"] as List<Booth> ?? service.GetAll_Booth(VidhanId);
            cache.Set("Booths", Booths, DateTimeOffset.Now.AddMinutes(60));
            controller.ViewBag.BoothList = Booths;
            controller.ViewData["BoothList"] = Booths;


            var VillageList = cache["VillageList"] as List<VillageList> ?? service.GetAllVillage();
            cache.Set("VillageList", VillageList, DateTimeOffset.Now.AddMinutes(60));
            controller.ViewData["VillageList"] = VillageList;


            var GetAllSubCaste = cache["GetAllSubCaste"] as List<SubCaste> ?? service.GetAllSubCaste();
            cache.Set("GetAllSubCaste", GetAllSubCaste, DateTimeOffset.Now.AddMinutes(60));
            controller.ViewData["GetAllSubCaste"] = GetAllSubCaste;
            controller.ViewData["CasteList"] = GetAllSubCaste;


            var GetDesignations = cache["GetDesignations"] as List<Designation> ?? service.GetDesignations();
            cache.Set("GetDesignations", GetDesignations, DateTimeOffset.Now.AddMinutes(60));
            controller.ViewData["GetDesignations"] = GetDesignations;


            var GetAllCategory = cache["GetAllCategory"] as List<Caste> ?? service.GetAllCategory();
            cache.Set("GetAllCategory", GetAllCategory, DateTimeOffset.Now.AddMinutes(60));
            controller.ViewData["CategoryList"] = GetAllCategory;
        }
    }
    #endregion
}