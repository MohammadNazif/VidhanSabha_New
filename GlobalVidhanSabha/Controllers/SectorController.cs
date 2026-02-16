using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows.Media.Media3D;
using VishanSabha.ActionFilter;
using VishanSabha.Models;
using VishanSabha.Services;
using VishanSabha.Services.SectorService;
using static QuestPDF.Helpers.Colors;

namespace VishanSabha.Controllers
{
    [System.Web.Http.Authorize(Roles = "SectorIncharge")]
    [SectorActionFilter]
    public class SectorController : Controller
    {
        SectorService service = new SectorService();

        [System.Web.Http.Authorize(Roles = "SectorIncharge")]
        public ActionResult SectorDashboard(int? Id)
        {
          
            string contact = Session["Contact"].ToString();
            var sectorIdStr = service.GetsectorInchargeId(contact);

            int sectorId = Id ?? Convert.ToInt32(sectorIdStr);

            ViewBag.BoothCount = service.GetBoothCountBySectorId(sectorId);

            ViewBag.pannaCount = service.GetPannaCountBySectorId(sectorId);
            ViewBag.BoothVoterCount = service.GetBoothVoterDesCountBySectorId(sectorId);
            ViewBag.boothSamithiCount = service.GetboothSamithiBySectorId(sectorId);
            ViewBag.PravasiCount = service.GetPravasiCountBySecIncId(sectorId);
            ViewBag.NewvoterCount = service.GetNewVotersCountBySecIncId(sectorId);
            ViewBag.EffectivePersonCount = service.GetEffectivePersonCountBySecIncId(sectorId);
            ViewBag.DoubleVoteCount = service.GetDoubleVoteCountBySecIncId(sectorId);
            ViewBag.SatisfiedCount = service.GetSatisfiedCountBySecIncId(sectorId);
            ViewBag.UnSatisfiedCount = service.GetUnSatisfiedCountBySecIncId(sectorId);
            ViewBag.SeniorCount = service.GetSeniorCountBySecIncId(sectorId);
            ViewBag.Handicaped = service.GetHandicapedCount(sectorId);
            ViewBag.ActivityCount = service.GetAllActivity();
            ViewData["AllowAccess"] = service.getAllowAccessDataBySecIncId(sectorId);

            return View(model: sectorIdStr);
        }
        public ActionResult BoothListBySector(int? sectorIdStr,FilterModel filter, int? limit = null, int? page = null)
        {
            try
            {
                ViewData["BoothList"] = service.GetBoothsBySectorId( sectorIdStr,filter,limit,page);

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error retrieving booth list: " + ex.Message;
                return View("Error");
            }
        }
      public ActionResult PannaListBySector(int? sectorIdStr,FilterModel filter, int? limit = null, int? page = null)
        {
        
            try
            {
                int sectorId = sectorIdStr.Value;

                List<Models.PannaPramukh> pannaList = service.GetPannaBySectorId(sectorId,filter,limit,page);
                ViewData["PannaList"] = pannaList;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error retrieving panna list: " + ex.Message;
                return View("Error");
            }
        }

        public ActionResult BoothVoterdesListBySector(int? sectorIdStr,FilterModel filter, int? limit = null, int? page = null)
        {
            try
            {  
               int sectorId = sectorIdStr.Value;
 
                List<BoothVotersDes> BoothVoterDesList = service.GetBoothVoterdesBySectorId(sectorId,filter,limit,page);

                if (BoothVoterDesList == null || BoothVoterDesList.Count == 0)
                {
                    ViewBag.Message = "No booth records found.";
                }
                ViewData["BoothVoterList"] = BoothVoterDesList;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error retrieving booth voter list: " + ex.Message;
                return View("Error");
            }
        }

        #region manisha
        public ActionResult GetBoothSamitiBySecIncId( int? sectorIdStr, FilterModel filter,int? limit=null,int? page =null)
        {
            int sectorId = sectorIdStr.Value;

            ViewData["BoothSamitiData"] = service.GetAllBoothSamitiBySectorIncId(sectorId,filter);
            ViewData["designationList"] = service.GetDesignations();
            ViewData["occupationList"] = service.getOccupation();
            return View();
        }
        public ActionResult ActivityList()
        {
            ViewData["ActivityData"] = service.GetAllActivityForSectorInc();
            return View();
        }
        public ActionResult PravasiList(int? sectorIdStr, FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["occupationList"] = service.getOccupation();
            ViewData["PravasiVoter"] = service.GetAllPravasiBySecIncId(sectorId, filter,limit,page);
            return View();
        }
        public ActionResult NewVoters(int? sectorIdStr, FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["Newvoter"] = service.GetAllNewvoterBySecIncId(sectorId,filter,limit,page);
            return View();
        }
        public ActionResult EffectivePerson(int? sectorIdStr, FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["designationList"] = service.GetDesignations();
            ViewData["EffecPersonList"] = service.GetAllEffectivePersonBySecIncId(sectorId,filter,limit,page);
            return View();
        }

        public ActionResult DoubleVoter(int? sectorIdStr, FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["DoubleVoteList"] = service.GetAllDoublevoteBySecIncId(sectorId, filter,limit,page);
            return View();
        }
        public ActionResult SatisfiedPersonList(int? sectorIdStr,FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["SatisfiedPersonList"] = service.GetAllSatisfiedBySecIncId(sectorId, filter, limit, page);
            return View();
        }


        public ActionResult UnSatisfiedPersonList(int? sectorIdStr, FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["UnSatisfiedPersonList"] = service.GetAllUnSatisfiedBySecIncId(sectorId, filter,limit,page);
            return View();
        }

        public ActionResult SeniorCitizen(int? sectorIdStr, FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["SeniorCitizenList"] = service.GetAllSeniorCitizenBySecIncId(sectorId, filter,limit,page);
            return View();
        }

        public ActionResult Handicaped(int? sectorIdStr, FilterModel filter, int? limit = null, int? page = null)
        {
            int sectorId = sectorIdStr.Value;
            ViewData["SeniorHandicapedList"] = service.GetAllHandiCapBySecIncId(sectorId,filter,limit,page);
            return View();
        }

     
        #endregion
    }
}