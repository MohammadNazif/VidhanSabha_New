using System;
using System.Web.Http;
using System.Web.Mvc;
using VishanSabha.Models;
using VishanSabha.Services;

namespace VishanSabha.Controllers
{
    [System.Web.Http.Authorize(Roles = "BoothIncharge")]
    public class BoothController : Controller
    {
        BoothService boothService = new BoothService();
        AdminServices service = new AdminServices();

        // GET: Booth
        public ActionResult BoothDashboard()
        {
            string contact = Session["Contact"].ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            ViewBag.TotalPanna = boothService.GetPannaCountByBoothIncId(boothInchargeId);
            ViewBag.TotalBoothVoter = boothService.GetBoothVoterCountByBoothIncId(boothInchargeId);
            ViewBag.TotalBoothSamiti = boothService.GetBoothSamitiCountByBoothIncId(boothInchargeId);
            ViewBag.TotalPravasi = boothService.GetTotalPravasiCount(boothInchargeId);
            //ViewBag.TotalNewVoters = boothService.GetNewVotersCountByIncharge(boothInchargeId);
            ViewBag.TotalActivities = boothService.GetAdminActivitiesCount();
            ViewBag.TotalSahmat = boothService.GetSahmatCountByIncharge(boothInchargeId);
            ViewBag.TotalAaSahmat = boothService.GetAsahamatCountByIncharge(boothInchargeId);
            ViewBag.DoubleVoter = boothService.GetTotalDoubleVoterCount(boothInchargeId);
            ViewBag.BoothSamithi = boothService.GetTotalBoothSamithiCount(boothInchargeId);
            ViewBag.PrabhavsaliCount = boothService.GetTotalPrabhavsaliCount(boothInchargeId);
            ViewBag.NewVotersCount = boothService.GetTotalNewVotersCount(boothInchargeId);

            ViewBag.GetTotalSeniorCitizen = boothService.GetTotalSeniorCitizenCount(boothInchargeId);
            ViewBag.CountDisabled = boothService.CountDisabledByBoothIncId(boothInchargeId);
            ViewBag.CountActivity = boothService.GetCountSocialMediaByBoothId(boothInchargeId);
            


            return View();
        }
        public ActionResult PannaPramukhList(FilterModel filter, int? limit = null, int? page = null)
        {
            string contact = Session["Contact"].ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            ViewData["PannaList"] = boothService.GetallPannaListByBoothIncId(boothInchargeId, filter,limit,page);
            ViewData["CasteList"] = service.GetAllSubCaste();
            return View();
        }
        public ActionResult BoothVoter(int? limit = null, int? page = null)
        {
            string contact = Session["Contact"].ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            ViewData["BoothVoterList "] = boothService.getBoothVoterDesByBoothIncId(boothInchargeId,limit,page);
            return View();
        }
        public ActionResult BoothSamiti()
        {
            string contact = Session["Contact"].ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            ViewData["BoothSamitiList"] = boothService.GetAllBoothSamitiByIncId(boothInchargeId);
            return View();
        }

        #region Nazif
        public ActionResult TotalPravasi(FilterModel filter, int? limit = null, int? page = null)
        {
            string contact = Session["Contact"].ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            ViewData["TotalPravasiList"] = boothService.GetAllTotalPravasiList(boothInchargeId, filter,limit,page);
            ViewData["CasteList"] = service.GetAllSubCaste();
            return View();
        }

        public ActionResult NewVotersList()
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var newVotersList = boothService.GetNewVotersListByIncharge(boothInchargeId);
            ViewData["NewVotersList"] = newVotersList;
            return View();
        }

        public ActionResult AdminActivities(int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            ViewData["AdminActivities"] = service.GetAllActivities(limit,page);
            return View();
        }

        #endregion

        //public ActionResult GetBoothInchargeId()
        //{
        //    string contact = Session["Contact"].ToString();
        //    var ContactNo = boothService.GetBoothInchargeId(contact);
        //    return Json(new
        //    {
        //        data = ContactNo
        //    });

        //}

        #region deep

        public ActionResult AllSahamat(FilterModel filter,

int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getsahmatList = boothService.getallsahmatlist(boothInchargeId, filter,limit,page);
            ViewData["AllsahmatList"] = getsahmatList;

            ViewData["occupationList"] = service.getOccupation();
            return View();
        }

        public ActionResult AllAsahamat(FilterModel filter, int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getAsahmatList = boothService.getallAsahmatlist(boothInchargeId,filter,limit,page);
            ViewData["AllAsahmatList"] = getAsahmatList;

            ViewData["occupationList"] = service.getOccupation();
   
            return View();
        }

        public ActionResult AllDoubleVotes(int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getDoubleList = boothService.getalldoubleVoterByBoothIncId(boothInchargeId,limit,page);
            ViewData["getDoubleList"] = getDoubleList;
            return View();
        }
        public ActionResult AllBoothSamithi(FilterModel filter, int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getBoothSamithi = boothService.GetAllBoothSamitiByBoothIncId(boothInchargeId, filter,limit,page);
            ViewData["getBoothSamithi"] = getBoothSamithi;

            ViewData["CasteList"] = service.GetAllSubCaste();
            ViewData["designationList"] = service.GetDesignations();

            ViewData["occupationList"] = service.getOccupation();
            return View();
        }

        public ActionResult AlLPrabhavsaliVyakti(FilterModel filter,
int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getPrabhavsali = boothService.GetAllPrabhavsaliByBoothIncId(boothInchargeId, filter,limit,page);
            ViewData["getAllPrabhavsali"] = getPrabhavsali;

            ViewData["CasteList"] = service.GetAllSubCaste();
            ViewData["designationList"] = service.GetEffectiveDesignations();
            return View();
        }

        public ActionResult AllNewVoters(FilterModel filter, int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getNewvoter = boothService.GetAllNewvoterByBoothIncId(boothInchargeId, filter,limit,page);
            ViewData["getAllNewvoter"] = getNewvoter;

            ViewData["CasteList"] = service.GetAllSubCaste();
            return View();
        }

        public ActionResult AllSeniorCitizen(FilterModel filter, int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getSeniorCitizen = boothService.GetAllSeniorCitizenByBoothIncId(boothInchargeId, filter,limit,page);
            ViewData["getAllSeniorCitizen"] = getSeniorCitizen;


            ViewData["CasteList"] = service.GetAllSubCaste();
            return View();
        }

        public ActionResult AllHandiCap(FilterModel filter, int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getAllHandiCap = boothService.GetAllHandiCapByBoothIncId(boothInchargeId, filter,limit,page);
            ViewData["CasteList"] = service.GetAllSubCaste();
            ViewData["getAllHandiCap"] = getAllHandiCap;

         
            return View();
        }

        public ActionResult AllSocialMedia(
int? limit = null, int? page = null)
        {
            string contact = Session["Contact"]?.ToString();
            int boothInchargeId = boothService.GetBoothInchargeId(contact);
            var getSocialMedia = boothService.GetSocialMediaPostByBoothId(boothInchargeId,limit,page);
            ViewData["getSocialMedia"] = getSocialMedia;
            return View();
        }

        [System.Web.Http.HttpPost]
        public ActionResult SaveFacebookPostStatus([FromBody] FacebookPostStatusModel model)
        {
            string contact= Session["Contact"]?.ToString();
            //int boothId = Convert.ToInt32(contact);
           int BoothId = boothService.GetBoothInchargeId(contact);
            model.UserId = BoothId;
            try
            {
               
                boothService.SaveFacebookPostStatus(model.UserId, model.PostId, model.Status);

                return Json(new { success = true, message = "Post status saved successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

    }
}