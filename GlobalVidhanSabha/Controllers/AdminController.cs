using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.EMMA;
using Newtonsoft.Json;
using VishanSabha.ActionFilter;
using VishanSabha.Models;

using VishanSabha.Services;

namespace VishanSabha.Controllers
{
    [Authorize(Roles = "MLA")]
    [AdminActionFilter]
    public class AdminController : Controller
    {
        AdminServices service = new AdminServices();
        BoothService booth = new BoothService();

        public ActionResult Dashboard()
        {
            ViewBag.SectorCount = service.SectorCount();
            ViewBag.BoothCount = service.BoothCount();
            ViewBag.PannaCount = service.GetTotalPannaPramukhCount();
            ViewBag.PravasiCount = service.GetTotalPravasi();
            ViewBag.Voterscount = service.GetTotalVoters();
            ViewBag.DoubleVotercount = service.GetTotalDoubleVoter();
            ViewBag.EffectivePerson = service.GetTotalEffectivePerson();
            ViewBag.Block = service.GetAllBlocksCounts();
            ViewBag.Bdc = service.GetBDCCount();
            ViewBag.sahamat = service.getSahamatCount();
            ViewBag.Asahamat = service.getAsahamatCount();
            ViewBag.Activity = booth.GetAdminActivitiesCount();
            ViewBag.Influencer = service.GetInfluencerCount();
            return View();
        }

        [HttpGet]
        public ActionResult Mandal()
        {
            int VidhansabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            List<Mandal> getmandal = service.GetAllMandalByVidhan(VidhansabhaId);
            return View(getmandal);
        }

        [HttpPost]
        public JsonResult AddMandal(Mandal model)
        {
            try
            {
                int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
                string action = model.Id > 0 ? "Update" : "Insert";
                bool result = service.AddMandal(model, VidhanSabhaId);

                if (result)
                {
                    return Json(new
                    {
                        success = true,
                        message = action == "Insert" ? "Mandal created successfully!" : "Mandal updated successfully!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Mandal already exists or operation failed."
                    });
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing your request."
                });
            }
        }
        //this action bind 
        [HttpGet]
        public JsonResult GetMandals()
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var mandals = service.GetAllMandal(VidhanSabhaId); // List<Mandal>
            return Json(mandals, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteMandal(int Id)
        {
            try
            {
                bool result = service.DeleteById_Mandal(Id);

                if (result)
                {
                    return Json(new { success = true, message = "Mandal deleted successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete mandal." });
                }
            }
            catch
            {
                return Json(new { success = false, message = "An error occurred while deleting mandal." });
            }
        }
        public JsonResult EditMandal(int Id)
        {
            Mandal mandal = service.GetById_Mandal(Id);
            return Json(mandal, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Sector(int? limit = null,int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            List<Sector> sector_list = service.GetAll_Sector(VidhanSabhaId);
            //ViewData["CategoryList"] = service.GetAllCategory(); move to action filter

            return View(sector_list);
        }
        [HttpGet]
        public JsonResult GetSubcastesByCategory(int categoryId)
        {
            var subCastes = service.GeSubcasteByCategoryId(categoryId);
            return Json(subCastes, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Getsector(int mandalId)
        {
            var sectors = service.GetSectorByMandalId(mandalId.ToString())
                .Select(s => new { s.Id, s.SectorName })
                .ToList();

            return Json(sectors, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult addSectors(Sector model, HttpPostedFileBase Image)
        {
            try
            {
                string filepath = "";
                string filename = "";
                int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
                model.Username = model.SectorName.Substring(0, 4) + model.PhoneNumber.Substring(model.PhoneNumber.Length - 4);
                model.Password = Guid.NewGuid().ToString().Substring(0, 8);



                if (Image != null && Image.ContentLength > 0)
                {

                    filename = Guid.NewGuid() + DateTime.Now.ToString("ddMMMyyyyHHmmss") + Path.GetExtension(Image.FileName);
                    filepath = "/UploadsImages/" + filename;


                    model.ProfileImage = filepath;
                }


                bool isUpdate = model.Id > 0;
                bool result = service.AddSector(model,VidhanSabhaId);

                if (result)
                {
                    if (Image != null && Image.ContentLength > 0)
                    {
                        Image.SaveAs(Path.Combine(Server.MapPath("~/UploadsImages/"), filename));
                    }
                    string msg = isUpdate ? "Sector updated successfully" : "Sector added successfully";
                    return Json(new { success = true, message = msg, status = isUpdate ? "update" : "insert" });
                }
                else
                {
                    return Json(new { success = false, message = "Operation failed" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteSector(int Id)
        {
            bool res = service.DeleteById_Sector(Id);

            if (res)
            {
                return Json(new { success = true, message = "Deleted successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete Sector." });
            }
        }

        [HttpGet]
        public JsonResult EditSector(int Id)
        {
            var data = service.GetById_Sector(Id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

       

        //bOOTH CONTROLLER
        [HttpGet]
        public ActionResult Booth(int? page = null,int ? limit = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            List<Booth> booth_list = service.GetAll_BoothForTable(VidhanSabhaId);
            //ViewData["CategoryList"] = serviceGetAllCaste();
            return View(booth_list);
        }

        [HttpPost]
        public JsonResult addbooth(Booth booth, HttpPostedFileBase ProfileImage)
        {
            try
            {
                string filename = "";
                string filepath = "";
                int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
                booth.Username = booth.BoothName.Substring(0, 4) + booth.PhoneNumber.Substring(booth.PhoneNumber.Length - 4);
                booth.Password = Guid.NewGuid().ToString().Substring(0, 8);



                if (ProfileImage != null && ProfileImage.ContentLength > 0)
                {

                    filename = Guid.NewGuid() + DateTime.Now.ToString("dddMMMyyyy") + Path.GetFileName(ProfileImage.FileName);


                    filepath = "/UploadsImages/" + filename;


                    string serverPath = Server.MapPath(filepath);

                    string dir = Path.GetDirectoryName(serverPath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    // Save file in folder
                    ProfileImage.SaveAs(serverPath);

                    // Save DB me
                    booth.ProfileImage = filepath;
                }

                // Save data + path in DB
                bool result = service.AddBooth(booth,VidhanSabhaId);

                string message = booth.Booth_Id > 0 ? "Booth updated successfully." : "Booth created successfully.";

                return Json(new { success = result, message = message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteBooth(int id)
        {
            bool result = service.DeleteById_Booth(id);

            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete booth." });
            }
        }

        #region manisha
        public ActionResult SectorList()
        {
            ViewData["SectorList"] = service.GetAllSectorWithBoothCount();
            return View();
        }
        public ActionResult BoothList()
        {
            ViewData["BoothList"] = service.GetAll_BoothForList();
            return View();
        }
        public ActionResult GetBoothsBySector(int sectorId)
        {
            var data = service.GetBoothBySectorId(sectorId.ToString());

            return Json(data, JsonRequestBehavior.AllowGet);

        }

        //today add
        [HttpGet]
        public JsonResult GetById_Booth(int Id)
        {
            var booth = service.GetById_Booth(Id); // This is your existing DB fetch
            return Json(booth, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSectorsByMandal(int mandalId)
        {
            var sectors = service.GetSectorByMandalId(mandalId.ToString())
                .Select(s => new { s.Id, s.SectorName })
                .ToList();

            return Json(sectors, JsonRequestBehavior.AllowGet);
        }

        //Report Booth

        [HttpGet]
        public ActionResult SectorReports(FilterModel filter, int? limit = null, int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            List<Sector> sector_list = service.GetAll_SectorDetails(filter,limit,page);
            //ViewData["MandalList"] = service.GetAllMandal();
            ViewData["SectorList"] = service.GetAll_Sector(VidhanSabhaId);
            return View(sector_list);
        }

        [HttpGet]
        public ActionResult BoothReports(FilterModel filter, int? limit = null, int? page = null)
        {
           
            var boothReports = service.GetBoothReport(filter,limit,page);
            ViewData["VillageList1"] = !string.IsNullOrEmpty(filter.boothIds)
             ? (object)service.GetVillageListByBoothId(filter.boothIds)
             : (object)service.GetAllVillage();
            return View(boothReports);

        }



        [HttpGet]
        public ActionResult MandalReports(int? limit = null, int? page = null)
        {
            var summaryList = service.GetMandalReport(limit,page);
            return View(summaryList);
        }

        [HttpGet]
        public ActionResult SectorDetailsByMandalIdList(string mandalId)
        {
            var sectorList = service.GetSectorsByMandalId(mandalId);
            return PartialView("SectorDetails", sectorList);
        }

        [HttpGet]
        public ActionResult BoothListBySectorId(int sectorId,int? limit=null,int? page=null)
        {
            var boothList = service.GetBoothslistbySectorId(sectorId, limit, page);
            return PartialView("BoothListSectorId", boothList);
        }


        public ActionResult PravasiReports(FilterModel filter)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["SectorList"] = service.GetAll_Sector(VidhanSabhaId);
            ViewData["Occupation"] = service.getOccupation();
            List<PravasiVoter> list = service.GetPravasiVoterReport(filter);
            return View(list);

        }

        public ActionResult DoubleVoterReports(FilterModel filter, int? limit = null, int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["SectorList"] = service.GetAll_Sector(VidhanSabhaId);
          

            List<doubleVoter> list = service.GetDoubleVoterReport(filter,limit,page);
            ViewData["BoothListtt"] = !string.IsNullOrEmpty(filter.sectorIds)
  ? (object)service.GetBoothBySectorId(filter.sectorIds)
  : (object)service.GetAll_Booth(VidhanSabhaId);

            ViewData["VillageList11"] = !string.IsNullOrEmpty(filter.boothIds) ? (object)service.GetVillageListByBoothId(filter.boothIds) : (object)service.GetAllVillage();
            return View(list);

        }
        public ActionResult NewVotersReports(FilterModel filter)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["SectorList"] = service.GetAll_Sector(VidhanSabhaId);
            List<NewVoters> list = service.GetNewVoterReport(filter);
            return View(list);
        }

        public ActionResult SeniorCitizenReports(FilterModel filter, int? limit, int? page)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["SectorList"] = service.GetAll_Sector(VidhanSabhaId);
            List<SeniorOrDisabled> list = service.GetSeniorCitizenReport(filter,limit,page);
            return View(list);

        }

        public ActionResult HandicapReports(FilterModel filter)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["SectorList"] = service.GetAll_Sector(VidhanSabhaId);
            List<SeniorOrDisabled> list = service.GetHandicapReport(filter);
            return View(list);

        }

        public ActionResult EffectivePersonReports(FilterModel filter)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["SectorList"] = service.GetAll_Sector(VidhanSabhaId);
            List<EffectivePerson> list = service.GetEffectivePersonReport(filter);
            return View(list);
        }


        public ActionResult GetInchargeDetailBySecId(int sectorid)
        {
            var data = service.GetSecInchargeBySecId(sectorid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetBoothsInchargeByBoothId(int boothid)
        {
            var data = service.GetBoothInchargeByBoothId(boothid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetInchargeDetailByBoothId(int boothid)
        {
            var data = service.GetBoothInchargeByBoothId(boothid);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadExcelForBooth()
        {
            return View();
        }
        [HttpPost]
        public JsonResult UploadExcelForBooth(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                return Json(new { success = false, message = "Please select a file." });

            try
            {
                bool result = service.ProcessBoothExcel(file.InputStream);
                return Json(new { success = result, message = result ? "Excel uploaded successfully!" : "Failed to process Excel." });

            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }

        }


        public ActionResult DoubleVote(int? id,int? limit,int?page)
        {
            doubleVoter data = new doubleVoter();
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["DoubleVoterList"] = service.GetAllDoubleVoters(VidhanSabhaId,limit, page);
            //ViewData["BoothList"] = service.GetAll_Booth();


            if (id.HasValue)
            {
                data = service.getDoubleVoterById(id.Value);
            }

            return View(data);
        }



        [HttpPost]
        public ActionResult AddDoubleVoter(doubleVoter data)
        {
            try
            {
                var result = service.AddDoubleVoters(data);
                if (result)
                {
                    return Json(new
                    {
                        status = true,
                        message = (data.id != 0 && data.id > 0) ? "Updated Successfully!!" : "Inserted Successfully!!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = (data.id != 0 && data.id > 0) ? "Update Failed!!" : "Insert Failed!!"
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = "Exception: " + ex.Message
                });
            }
        }


        [HttpPost]
        public ActionResult DeleteDoubleVoter(int id)
        {
            var res = service.deleteDoubleVoter(id);
            if (res)
            {
                return Json(new
                {
                    status = true,
                    message = "Record deleted successfully!!"
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Some error occured!!"
                });
            }
        }

        public ActionResult PravasiVoter(int? id, FilterModel filter, int? limit = null, int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            PravasiVoter data = new PravasiVoter();
            //ViewData["CategoryList"] = service.GetAllCategory();
            ViewData["AllPravasiVoter"] = service.GetAllPravasiVoterData(VidhanSabhaId,filter, limit, page);
            //ViewData["BoothList"] = service.GetAll_Booth();
            ViewData["Occupation"] = service.getOccupation();

            if (id.HasValue)
            {
                data = service.getPravasiDataById(id.Value);
            }
            return View(data);
        }


        public ActionResult GetSubcasteByCategory(int id)
        {
            var result = service.GeSubcasteByCategoryId(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddPravasiVoter(PravasiVoter data)
        {
            try
            {
                var result = service.AddPravasiVoters(data);
                if (result)
                {
                    return Json(new
                    {
                        status = true,
                        message = (data.id != 0 && data.id > 0) ? "Updated Successfully!!" : "Inserted Successfully!!"
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = false,
                        message = (data.id != 0 && data.id > 0) ? "Update Failed!!" : "Insert Failed!!"
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new
                {
                    status = false,
                    message = "Exception: " + ex.Message
                });
            }
        }
        public ActionResult DeletePravasiVoter(int id)
        {
            var res = service.DeletePravasiVoter(id);
            if (res)
            {
                return Json(new
                {
                    status = true,
                    message = "Record deleted Successfully!!"
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Some error occured!!"
                });
            }
        }

        #endregion
        //Nazif 
        [HttpGet]
        public ActionResult PannaPramukh()
        {
            ViewBag.CastList = service.GetAllSubCaste().ToList();
            return View();
        }

        [HttpGet]
        public JsonResult GetAllPannaPramukh(string[] boothNumbers, int? limit = null, int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var pannaList = service.GetAll_PannaPramukhforfilter(VidhanSabhaId,boothNumbers, limit, page);
            return Json(new { success = true, data = pannaList }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult PannaPramukh(PannaPramukh model, HttpPostedFileBase ProfileImage)
        {
            try
            {
                string folderPath = Server.MapPath("~/UploadedImages/ProfileImages");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Handle file upload
                if (ProfileImage != null && ProfileImage.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    string filePath = Path.Combine(folderPath, fileName);
                    ProfileImage.SaveAs(filePath);

                    // Delete old image if updating
                    if (model.PannaPramukh_Id > 0)
                    {
                        var existing = service.GetAll_PannaPramukh()
                                              .FirstOrDefault(p => p.PannaPramukh_Id == model.PannaPramukh_Id);

                        if (existing != null && !string.IsNullOrEmpty(existing.ProfileImageUrl))
                        {
                            string oldPath = Server.MapPath(existing.ProfileImageUrl);
                            if (System.IO.File.Exists(oldPath))
                                System.IO.File.Delete(oldPath);
                        }
                    }

                    // Save new image path
                    model.ProfileImageUrl = "/UploadedImages/ProfileImages/" + fileName;
                }
                else
                {
                    // Keep existing image if no new file uploaded during update
                    if (model.PannaPramukh_Id > 0)
                    {
                        var existing = service.GetAll_PannaPramukh()
                                              .FirstOrDefault(p => p.PannaPramukh_Id == model.PannaPramukh_Id);

                        if (existing != null)
                        {
                            model.ProfileImageUrl = existing.ProfileImageUrl;
                        }
                    }
                }

                // Insert or Update logic
                bool isSuccess = model.PannaPramukh_Id > 0
                    ? service.UpdatePannaPramukh(model)
                    : service.InsertPannaPramukh(model);

                if (isSuccess)
                {
                    return Json(new { success = true, message = model.PannaPramukh_Id > 0 ? "Updated successfully." : "Added successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save data." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        [HttpPost]
        public ActionResult DeletePannaPramukh(int id)
        {
            try
            {
                bool result = service.DeleteById_PannaPramukh(id);

                return Json(result
                    ? new { success = true, message = "Panna Pramukh deleted successfully." }
                    : new { success = false, message = "Failed to delete Panna Pramukh." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        //Samiti - Nazif
        [HttpGet]
        public ActionResult BoothSamiti(int? limit = null, int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var boothSamitiList = service.GetAllBoothSamitiIncharges(limit,page);
            // Pass it via ViewBag or ViewData
            ViewBag.SamitiList = service.GetAllBoothSamitiIncharges();
            ViewBag.BoothList = service.GetAll_Booth(VidhanSabhaId);
            ViewBag.CastList = service.GetAllSubCaste().ToList();
            //ViewData["CategoryList"] = service.GetAllCategory();
            return View(boothSamitiList);
        }

        [HttpGet]
        public JsonResult GetAvailableDesignations(int boothId)
        {
            var allDesignations = service.GetDesignations();
            var usedDesignationIds = service.GetAllBoothSamiti()
                   .Where(x => x.BoothId == boothId && x.DesignationId != 7) // assuming 7 = 'Sadasya'
                   .Select(x => x.DesignationId)
                   .ToList();

            var result = allDesignations.Select(d => new
            {
                id = d.Id,
                name = d.DesigName,
                disabled = (d.Id != 6 && usedDesignationIds.Contains(d.Id))
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SaveBoothIncharge(BoothSamiti model)
        {
            try
            {
                bool isSaved = service.InsertBoothIncharge(model); // Call your service
                if (isSaved)
                {
                    return Json(new { success = true, message = "Booth Inchargesaved successfully." });
                }


                else
                {
                    return Json(new { success = false, message = "Failed to save Booth Incharge." });
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Booth Already exist" });
            }
        }

        [HttpGet]
        public JsonResult IsDesignationUnique(string designation, int boothId, int? excludeId)
        {
            var exists = service.GetAllBoothSamiti().Any(x =>
                x.BoothId == boothId &&
                x.Designation == designation &&
                (excludeId == null || x.BoothSamiti_Id != excludeId));

            return Json(!exists, JsonRequestBehavior.AllowGet); // Return true if unique
        }

        [HttpGet]
        public JsonResult GetAllBoothSamiti(int boothId)
        {
            try
            {
                var data = service.GetBoothSamitiByBoothId(boothId); // Replace with actual service
                return Json(new
                {
                    success = data != null && data.Any(),
                    data = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult GetBoothSamitiByid(int id)
        {
            var record = service.GetAllBoothSamiti().FirstOrDefault(b => b.BoothSamiti_Id == id);

            if (record == null)
                return Json(new { success = false, message = "Record not found" }, JsonRequestBehavior.AllowGet);

            // return your DTO or map properties to send minimal info to client
            return Json(new { success = true, data = record }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveBoothSamiti(BoothSamiti model)
        {
            try
            {
                bool isSaved;

                if (model.BoothSamiti_Id > 0)
                {
                    // Update existing record
                    isSaved = service.UpdateBoothSamiti(model);
                }
                else
                {
                    // Insert new record
                    isSaved = service.InsertBoothSamiti(model);
                }

                if (isSaved)
                {
                    return Json(new
                    {
                        success = true,
                        message = model.BoothSamiti_Id > 0 ? "Booth Samiti updated successfully." : "Booth Samiti added successfully."
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save Booth Samiti." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetBoothIncharge(int boothId)
        {
            // Assume you have a service method that returns BoothInchargeby BoothId
            string inchargeName = service.GetBoothInchargeNameByBoothId(boothId);

            if (inchargeName != null)
                return Json(new { success = true, incharge = inchargeName }, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, message = "Booth Inchargenot found" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteBoothSamiti(int id)
        {
            try
            {
                bool isDeleted = service.SoftDeleteBoothSamiti(id);

                if (isDeleted)
                {
                    return Json(new { success = true, message = "Record deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Record not found or could not be deleted." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
        //EffectivePerson-Nazif
        [HttpGet]
        public ActionResult EffectivePerson(int? limit = null, int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var model = service.GetAllEffectivePersons(limit,page);
            ViewBag.BoothList = service.GetAll_Booth(VidhanSabhaId);

            var designationList = service.GetEffectiveDesignations();
            ViewData["DesignationList"] = designationList;

            //ViewBag.SelectedBooth = boothId; // 👈 for dropdown selected
            //ViewData["CategoryList"] = service.GetAllCategory();

            return View(model);
        }

        [HttpPost]
        public JsonResult EffectivePerson(EffectivePerson model)
        {
            try
            {
                bool isSaved;

                if (model.effectivePersonId > 0)
                {
                    isSaved = service.UpdateEffectivePerson(model);
                }
                else
                {
                    isSaved = service.InsertEffectivePerson(model);
                }

                if (isSaved)
                {
                    return Json(new
                    {
                        success = true,
                        message = model.effectivePersonId > 0
                            ? "Effective Person updated successfully."
                            : "Effective Person added successfully."
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save Effective Person." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetEffectivePersonById(int id)
        {
            var model = service.GetEffectivePersonById(id);
            if (model != null)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteEffectivePerson(int id)
        {
            try
            {
                bool isDeleted = service.DeleteEffectivePerson(id); // soft delete: sets status = 0
                return Json(new
                {
                    success = isDeleted,
                    message = isDeleted ? "Effective Person deleted successfully." : "Deletion failed."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public ActionResult SeniorOrDisabled(FilterModel filter,int? type, int? limit, int? page)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhansabhaId"]);
            var seniorordisabled = service.GetSeniorOrDisabled(VidhanSabhaId, filter,type,limit,page);

            ViewBag.CastList = service.GetAllSubCaste().ToList();
            //ViewData["CategoryList"] = service.GetAllCategory();
            //ViewData["BoothList"] = service.GetAll_Booth();
            ViewData["GetSeniorDisabledtype"] = service.GetSeniorDisabledtype();

            return View(seniorordisabled);
        }


        [HttpPost]
        public JsonResult SeniorOrDisabled(List<SeniorOrDisabled> model)
        {
            if (model == null || !model.Any())
                return Json(new { success = false, message = "No data received." });

            foreach (var item in model)
            {
                if (item.Id > 0)
                {
                    bool updated = service.UpdateSeniorOrDisabled(item);
                    if (!updated)
                        return Json(new { success = false, message = $"Failed to update record with Id {item.Id}" });
                }
                else
                {
                    bool inserted = service.InsertSeniorOrDisabled(item);
                    if (!inserted)
                        return Json(new { success = false, message = $"Failed to insert record with Name {item.Name}" });
                }
            }

            return Json(new { success = true, message ="Operation completed successfully." });
        }

        [HttpGet]
        public JsonResult GetSeniorOrDisabledById(int id, FilterModel filter,
       int? type,
       int? pageNumber=null,
       int? pageSize=null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var record = service.GetSeniorOrDisabled(VidhanSabhaId,
       filter,
       type,
      pageNumber,
      pageSize).FirstOrDefault(s => s.Id == id);

            if (record == null)
                return Json(new { success = false, message = "Record not found" }, JsonRequestBehavior.AllowGet);



            return Json(new { success = true, data = record }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteSeniorOrDisabled(int id)
        {
            try
            {
                var result = service.DeleteSeniorOrDisabled(id);
                if (result)
                {
                    return Json(new { success = true, message = "Record deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to delete record." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        public JsonResult EditSeniorOrDisabled(SeniorOrDisabled model)
        {
            if (model == null || model.Id == 0)
            {
                return Json(new { success = false, message = "Invalid data." });
            }

            try
            {
                bool isUpdated = service.UpdateSeniorOrDisabled(model);
                if (!isUpdated)
                {
                    return Json(new { success = false, message = "Failed to update the record." });
                }

                return Json(new { success = true, message = "Record updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        //AddNewBoothVoters - deep singh
        public ActionResult NewVoters(FilterModel filter,int? limit,int? page)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var votersList = service.GetNewVoters(VidhanSabhaId,filter, limit,page);
            return View(votersList);
        }





        [HttpPost]
        public JsonResult deleteVoters(int Id)
        {
            bool res = service.deleteNewVoter(Id);

            if (res)
            {
                return Json(new { success = true, message = "Deleted successfully!" });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete Sector." });
            }
        }


        [HttpPost]
        public JsonResult AddNewVoter(NewVoters voter)
        {
            try
            {
                var result = service.AddNewVoters(voter);

                bool isUpdate = voter.Id > 0;
                if (result)
                {
                    string msg = isUpdate ? "Voter updated successfully" : "New Voter added successfully";
                    return Json(new { success = true, message = msg, status = isUpdate ? "update" : "AddNewVoters" });
                }
                else
                {
                    return Json(new { success = false, message = "Operation failed" });
                }
            }
            catch (Exception ex)
            {
                // ✅ Exact error client ko bhej do (debug ke liye)
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpGet]
        public JsonResult GetVoterbyId(int Id)
        {
            var result = service.getVoterById(Id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //complete new NewVoters controller -- deep singh

        //App_Start BoothVotersDescription controller -- deep singh

        public ActionResult BoothVotersDescription(int? limit, int? page)
        {
            int VishanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["BoothList"] = service.GetBoothDropdown(VishanSabhaId);
            //var filterBoothId = string.IsNullOrEmpty(boothId) ? "all" : boothId;

            var result = service.getBoothVoterDes(VishanSabhaId,limit, page);
            //ViewBag.SelectedBooth = filterBoothId;
            //ViewData["CasteList"] = service.GetAllSubCaste();

            return View(result);
        }





        [HttpGet]
        public JsonResult GetCastVotersById(int voterDesId)
        {
            var data = service.GetCastVotersDesById(voterDesId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BoothVotersDescription(BoothVotersDes model)
        {
            bool isSaved = service.addOrUpdateBoothVoterDes(model);

            if (isSaved)
            {
                string message = model.Id > 0
                    ? "Booth Voter Data Updated Successfully!"
                    : "Booth Voter Data Saved Successfully!";

                return Json(new { success = true, message = message });
            }
            else
            {
                return Json(new { success = false, message = "Something went wrong while saving!" });
            }
        }


        [HttpPost]
        public JsonResult DeleteBoothVoterDes(int id)
        {
            try
            {
                bool isDeleted = service.DeleteBoothVoterDes(id); // your ADO.NET method call

                if (isDeleted)
                    return Json(new { success = true, message = "Deleted successfully!" });
                else
                    return Json(new { success = false, message = "Deletion failed!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetBoothVoterById(int id)
        {
            var boothData = service.getBoohDesById(id); // call to your method
            return Json(boothData, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult AddCastVoter(CastVotersDes model)
        {
            bool isSaved = service.AddCastVotersDes(model);

            if (isSaved)
            {
                return Json(new { success = true, message = "Caste voter added successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to add caste voter." });
            }
        }

        [HttpPost]
        public JsonResult DeleteCastVoter(int id)
        {
            try
            {
                bool result = service.DeleteCastVoter(id);
                if (result)
                {
                    return Json(new { success = true, message = "Caste deleted successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Could not delete caste." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetCastById(int castVoterId)
        {
            var data = service.GetCastListById(castVoterId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCastVoter(int castVoterId, int castId, int number)
        {
            try
            {
                bool isUpdated = service.UpdateCastVoter(castVoterId, castId, number);

                if (!isUpdated)
                    return Json(new { success = false, message = "Update failed." });

                // Return the same parameters received
                return Json(new
                {
                    success = true,
                    CastVoterId = castVoterId,
                    CastNameId = castId,
                    Number = number,
                    //castNameText= castNameText
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        #region Activity and SocialMediaPost


        //
        [HttpGet]
        public ActionResult Activity()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllActivities(int? limit = null, int? page = null)
        {
            try
            {
                var data = service.GetAllActivities(limit,page);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ✅ Get Activity by ID (used in Edit)
        [HttpGet]
        public JsonResult GetActivityById(int id)
        {
            try
            {
                var model = service.GetActivityById(id);
                if (model != null)
                    return Json(model, JsonRequestBehavior.AllowGet);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // ✅ Save (Insert or Update)

        [HttpPost]
        public JsonResult SaveActivity(Activities model, List<HttpPostedFileBase> Images, HttpPostedFileBase VideoFile, string ImagesToDeleteJson = null)
        {
            try
            {
                List<string> currentImages = new List<string>();

                // Existing activity if updating
                if (model.ActivityId > 0)
                {
                    var existing = service.GetActivityById(model.ActivityId);
                    if (existing == null)
                        return Json(new { success = false, message = "Activity not found." });

                    currentImages = existing.ImagePaths ?? new List<string>();

                    // Delete requested images
                    var toDelete = string.IsNullOrEmpty(ImagesToDeleteJson)
                        ? new List<string>()
                        : JsonConvert.DeserializeObject<List<string>>(ImagesToDeleteJson);

                    foreach (var img in toDelete)
                    {
                        string fullPath = Server.MapPath(img);
                        if (System.IO.File.Exists(fullPath))
                            System.IO.File.Delete(fullPath);

                        currentImages.Remove(img);
                    }
                }

                // Validate and upload video
                if (VideoFile != null && VideoFile.ContentLength > 0)
                {
                    const int maxVideoSizeBytes = 200 * 1024 * 1024; // 200 MB in bytes

                    if (VideoFile.ContentLength > maxVideoSizeBytes)
                    {
                        return Json(new { success = false, message = "Video size should not exceed 200 MB." });
                    }

                    string videoFolder = Server.MapPath("~/UploadedVideos/");
                    if (!Directory.Exists(videoFolder))
                        Directory.CreateDirectory(videoFolder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(VideoFile.FileName);
                    string videoPath = Path.Combine(videoFolder, fileName);
                    VideoFile.SaveAs(videoPath);

                    model.VideoUrl = "/UploadedVideos/" + fileName; // Save relative path
                }

                // Validate and upload new images
                if (Images != null && Images.Count > 0)
                {
                    string[] allowedImageExtensions = { ".jpeg", ".jpg", ".png" };

                    string uploadPath = Server.MapPath($"~/UploadedImages/");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    foreach (var img in Images)
                    {
                        if (img != null && img.ContentLength > 0)
                        {
                            string ext = Path.GetExtension(img.FileName).ToLower();

                            if (!allowedImageExtensions.Contains(ext))
                            {
                                return Json(new { success = false, message = $"Only JPEG, JPG, and PNG image formats are allowed. Invalid file: {img.FileName}" });
                            }

                            string fileName = Guid.NewGuid() + ext;
                            string filePath = Path.Combine(uploadPath, fileName);
                            img.SaveAs(filePath);

                            string relativePath = $"/UploadedImages/" + fileName;
                            currentImages.Add(relativePath);
                        }
                    }
                }

                model.ImagePaths = currentImages;
                model.Status = true;

                bool result = service.AddOrUpdateActivity(model);

                if (model.ActivityId > 0)
                    return Json(new { success = result, message = "Activity updated successfully." });
                else
                    return Json(new { success = result, message = "Activity added successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        // ✅ Soft Delete
        [HttpPost]
        public JsonResult DeleteActivities(int id)
        {
            try
            {
                var result = service.DeleteActivity(id);
                return Json(new
                {
                    success = result,
                    message = result ? "Deleted successfully." : "Delete failed."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult DeleteImage(string imageUrl, int activityId)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl) || activityId <= 0)
                    return Json(new { success = false, message = "Invalid parameters." });

                // Get the activity
                var activity = service.GetActivityById(activityId);
                if (activity == null)
                    return Json(new { success = false, message = "Activity not found." });

                // Remove image from activity's image list
                var images = activity.ImagePaths ?? new List<string>();
                if (!images.Contains(imageUrl))
                    return Json(new { success = false, message = "Image not found in activity." });

                images.Remove(imageUrl);

                // Delete physical file
                var fullPath = Server.MapPath(imageUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                // Update the activity with new image list
                activity.ImagePaths = images;
                service.AddOrUpdateActivity(activity);  // Save changes

                return Json(new { success = true, message = "Image deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        [HttpGet]
        public ActionResult SocialMediaPost(int? Id, int? limit = null, int? page = null)
        {
            SocialMediaPost data = new SocialMediaPost();

            if (Id.HasValue)
            {
                data = service.GetPostById(Id.Value);
            }
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["PoatData"] = service.GetAllPosts(limit,page);
            ViewBag.BoothList = service.GetAll_Booth(VidhanSabhaId).ToList();
            ViewBag.SectorList = service.GetAll_Sector(VidhanSabhaId).ToList();
            return View(data);
        }
        [HttpPost]
        public JsonResult InsertPost(SocialMediaPost model, string boothIds, string sectorIds)
        {
            try
            {
                if (model == null)
                    return Json(new { success = false, message = "Invalid post data." });

                if (model.post != null && model.post.ContentLength > 0)
                {
                    string filename = Path.GetFileName(model.post.FileName);
                    string uniqueFilename = Guid.NewGuid().ToString() + Path.GetExtension(filename);
                    string uploadFolder = Server.MapPath("/UploadsImages");
                    string fullpath = Path.Combine(uploadFolder, uniqueFilename);
                    model.post.SaveAs(fullpath);

                    model.PostUrl = "/UploadsImages/" + uniqueFilename;
                }


                string platformCsv = (model.Platform != null) ? string.Join(",", model.Platform) : null;

                int postId = service.InsertPost(model, boothIds, sectorIds, platformCsv);
                return Json(new { success = true, message = model.PostId > 0 ? "Post Updated successfully!!" : "Post inserted successfully.", postId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdatePost(SocialMediaPost model, string boothIds, string sectorIds)
        {
            try
            {
                if (model == null || model.PostId <= 0)
                    return Json(new { success = false, message = "Invalid post data." });

                bool updated = service.UpdatePost(model, boothIds, sectorIds);

                if (!updated)
                    return Json(new { success = false, message = "Post not found or update failed." });

                return Json(new { success = true, message = "Post updated successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        [HttpPost]
        public JsonResult DeletePost(int id)
        {
            try
            {
                if (id <= 0)
                    return Json(new { success = false, message = "Invalid PostId." });

                bool deleted = service.DeletePost(id);

                if (!deleted)
                    return Json(new { success = false, message = "Delete failed or Post not found." });

                return Json(new { success = true, message = "Post deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetPostById(int id)
        {
            SocialMediaPost post = service.GetPostById(id);

            if (post == null)
            {
                return Json(new { success = false, message = "Post not found" });
            }

            // Return post object as JSON
            return Json(new { success = true, data = post }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AccessControl()
        {
            return View();
        }
        #endregion


        public ActionResult AllowAccess()
        {
            return View();
        }
        public ActionResult GetDropdownData(string type)
        {
            var data = service.GetDataForAllowAccess(type);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertAuthorization(string Type, string sectorboothId, string AllowedPermissions)
        {
            try
            {

                service.AddAllowAccess(Type, sectorboothId, AllowedPermissions);

                return Json(new { success = true, message = "Authorization inserted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult GetVillageListByMandalId(int mandalId)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var data = service.getVillageByMandalId(mandalId, VidhanSabhaId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetVillageListBySectorlId(string sectorId)
        {
            var data = service.GetVillageBySectorId(sectorId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CombinedReports(FilterModel filter,int? limit = null, int? page = null)

        {
            var reportData = service.GetCombinedReport(filter,limit,page);
            //ViewData["MandalList"] = service.GetAllMandal();
            //ViewData["SectorList"] = service.GetAll_Sector();
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            ViewData["SectorList"] = !string.IsNullOrEmpty(filter.mandalIds)
  ? (object)service.GetSectorsByMandalId(filter.mandalIds)
  : (object)service.GetAll_Sector(VidhanSabhaId);

            ViewData["VillageList1"]= !string.IsNullOrEmpty(filter.sectorIds)
  ? (object)service.GetVillageBySectorId(filter.sectorIds)
  : (object)service.GetAllVillage();

            ViewData["CombinedReport"] = reportData;
            return View();
        }


        public ActionResult CombinedSectorReports(FilterModel filter, int? limit = null, int? page = null)
        {
           
            var sectors = service.GetAll_SectorDetails(filter,limit,page);

            //var sectorBoothsDict = new Dictionary<int, List<Booth>>();
            //foreach (var sector in sectors)
            //{
            //    var booths = service.GetBoothslistbySectorId(sector.Id).Select(d => filter.boothIds.Split(',').Equals(d.BoothId)).ToList();

            //    sectorBoothsDict[sector.Id] = booths; // ✅ Keep it separate, no sector.Booths
            //}

            var sectorBoothsDict = new Dictionary<int, List<Booth>>();

            // Safely split boothIds (can be null or empty) — use char[] overload
            var boothIds = (filter?.boothIds ?? "")
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(id => {
                    return int.TryParse(id.Trim(), out var val) ? (int?)val : null;
                })
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();

            foreach (var sector in sectors ?? Enumerable.Empty<Sector>())
            {
                var booths = service.GetBoothslistbySectorId(sector.Id) ?? new List<Booth>();

                var filteredBooths = (boothIds == null || boothIds.Count == 0)
                    ? booths.ToList()
                    : booths.Where(d => boothIds.Contains(d.BoothId)).ToList();

                sectorBoothsDict[sector.Id] = filteredBooths;
            }

            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            //ViewData["MandalList"] = service.GetAllMandal();
            ViewData["SectorList"] = !string.IsNullOrEmpty(filter.mandalIds)
  ? (object)service.GetSectorsByMandalId(filter.mandalIds)
  : (object)service.GetAll_Sector(VidhanSabhaId);


            ViewData["SectorListUseToCimbinedReport"] = sectors;
            ViewData["BoothDataBySector"] = sectorBoothsDict;

            ViewData["BoothListtt"] = !string.IsNullOrEmpty(filter.sectorIds)
     ? (object)service.GetBoothBySectorId(filter.sectorIds)
     : (object)service.GetAll_Booth(VidhanSabhaId);


            ViewData["VillageList11"] = !string.IsNullOrEmpty(filter.boothIds) ? (object)service.GetVillageListByBoothId(filter.boothIds) : (object)service.GetAllVillage();

            return View();
        }

        public ActionResult SatisfiedandUnsatisfied(int? id,FilterModel filter,
        int? type,int? limit, int? page)
        {
            SatisfiedUnSatisfied data = new SatisfiedUnSatisfied();
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            if (id.HasValue)
            {
                data = service.GetAllSatisfiedUnsatisfiedDataById(id.Value);
            }
           
                var allData = service.GetAllDataSatisfiedUnsatisfied(VidhanSabhaId,type, filter,limit,page);
                ViewData["SatisfiedUnsatisfiedAlldata"] = allData;
            
            ViewData["Party"] = service.Getallparty();
          
            ViewData["BoothNumber"] = service.GetBoothIdAndNumber(VidhanSabhaId);
            var SahmatAsahmatType = service.getSahmatAsahmatType();
            ViewData["Occupation"] = service.getOccupation();
            ViewData["SahmatAsahmatType"] = SahmatAsahmatType;
            return View(data);
        }
        public ActionResult GetVillageListByBoothId(int boothId)
        {
            var data = service.VillageListByBoothId(boothId.ToString());
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult addSatisfiedUnSatisfied(SatisfiedUnSatisfied data)
        {
            var res = service.AddSatisfiedUnsatisfied(data);
            if (res)
            {
                return Json(new
                {
                    status = true,
                    message = data.id > 0 ? "Data Updated Successfully" : "Data Added Successfully!!"
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = data.id > 0 ? "Some error occured!!" : "Some Error Occured!!"
                });
            }
        }
        public ActionResult DeleteSatisfiedUnsatisfied(int id)
        {
            var res = service.deleteSatisfiedUnsatisfied(id);
            if (res)
            {
                return Json(new
                {
                    success = true,
                    message = "Booth deleted successfully!"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Unable to delete booth. Please try again."
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult BoothDetailsByMandalId(int mandalId)
        {
            var boothList = service.GetBoothsByMandalId(mandalId);
            return PartialView("BoothDetails", boothList);
        }
        public ActionResult GetPostDetailsById(int id)
        {
            var data = service.GetSocialMediaPostDetailById(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region Block Nazif

        public ActionResult Block(FilterModel filter, int? limit = null, int? page = null)
        {
            var blocks = service.GetAllBlocks(filter,limit,page);
            ViewData["Party"] = service.Getallparty();
           
            return View(blocks);
        }

        [HttpPost]
        public JsonResult AddBlock(Block model, HttpPostedFileBase ProfileImage = null)
        {
            try
            {
                // Load existing block if updating
                string oldImagePath = null;

                if (model.Block_Id > 0)
                {
                    var existing = service.GetBlockById(model.Block_Id); // Fetch from DB
                    if (existing == null)
                        return Json(new { success = false, message = "Block not found." });
                    else
                    {

                        oldImagePath = existing.ProfileImage;

                    }

                }


                // Handle new image upload
                if (ProfileImage != null && ProfileImage.ContentLength > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(oldImagePath))
                    {
                        string fullOldPath = Server.MapPath(oldImagePath);
                        if (System.IO.File.Exists(fullOldPath))
                        {
                            System.IO.File.Delete(fullOldPath);
                        }
                    }

                    // Upload new image
                    string uploadFolder = Server.MapPath("~/UploadedImages/");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ProfileImage.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);
                    ProfileImage.SaveAs(filePath);

                    model.ProfileImage = "/UploadedImages/" + fileName; // Save relative path to DB
                }
                else
                {
                    // Keep existing image path if no new image is uploaded
                    model.ProfileImage = oldImagePath;
                }

                // Save or update block
                int resultId = service.SaveBlock(model);

                if (resultId > 0)
                {
                    string message = model.Block_Id > 0 ? "Block updated successfully." : "Block added successfully.";
                    return Json(new { success = true, blockId = resultId, message });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save block." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetBlockById(int id)
        {
            try
            {
                var block = service.GetBlockById(id);
                if (block != null)
                {
                    return Json(new
                    {
                        success = true,
                        data = block
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, message = "Block not found." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteBlock(int id)
        {
            try
            {
                var block = service.GetBlockById(id);
                if (block == null)
                    return Json(new { success = false, message = "Block not found." });

                // Delete profile image if exists
                if (!string.IsNullOrEmpty(block.ProfileImage))
                {
                    string fullPath = Server.MapPath(block.ProfileImage);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }

                // Soft delete (or hard delete based on your DB design)
                bool deleted = service.DeleteBlock(id); // Implement this in your service

                return Json(new { success = deleted, message = deleted ? "Block deleted successfully." : "Failed to delete block." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        //public JsonResult AddBLock()
        //{
        //    var block = service.add
        //}
        #endregion


        #region Bdc Nazif

        public ActionResult Bdc(FilterModel filter, int? limit = null, int? page = null)
        {
            var bdclist = service.GetBDC(limit,page);
            ViewData["BlockList"] = service.GetAllBlocks(filter);
            ViewData["Party"] = service.Getallparty();
            return View(bdclist);
        }

        [HttpPost]
        public JsonResult AddOrUpdateBDC(BDC model, HttpPostedFileBase ProfileImage = null)
        {
            try
            {
                string oldImagePath = null;

                if (model.BDC_Id > 0)
                {
                    var existing = service.GetBDC(model.BDC_Id).FirstOrDefault();
                    if (existing == null)
                        return Json(new { success = false, message = "BDC profile not found." });

                    oldImagePath = existing.ProfileImage;
                }

                // Handle file upload
                if (ProfileImage != null && ProfileImage.ContentLength > 0)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(oldImagePath))
                    {
                        string fullOldPath = Server.MapPath(oldImagePath);
                        if (System.IO.File.Exists(fullOldPath))
                            System.IO.File.Delete(fullOldPath);
                    }

                    string uploadFolder = Server.MapPath("~/UploadedImages/");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ProfileImage.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);
                    ProfileImage.SaveAs(filePath);

                    model.ProfileImage = "/UploadedImages/" + fileName;
                }
                else
                {
                    model.ProfileImage = oldImagePath;
                }

                // Save or update
                service.SaveBDC(model);

                string msg = model.BDC_Id > 0 ? "BDC profile updated successfully." : "BDC profile added successfully.";
                return Json(new { success = true, message = msg });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult GetBDCById(int id)
        {
            try
            {
                var bdc = service.GetBDC(id).FirstOrDefault();
                if (bdc == null)
                    return Json(new { success = false, message = "BDC profile not found." }, JsonRequestBehavior.AllowGet);

                return Json(new { success = true, data = bdc }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult DeleteBDC(int id)
        {
            try
            {
                var bdc = service.GetBDC(id).FirstOrDefault();
                if (bdc == null)
                    return Json(new { success = false, message = "BDC profile not found." });

                // Delete profile image if exists
                if (!string.IsNullOrEmpty(bdc.ProfileImage))
                {
                    string fullPath = Server.MapPath(bdc.ProfileImage);
                    if (System.IO.File.Exists(fullPath))
                        System.IO.File.Delete(fullPath);
                }

                // Soft delete (or hard delete in your service)
                service.DeleteBDC(id);

                return Json(new { success = true, message = "BDC profile deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        #endregion



        #region Lists Nazif

        public ActionResult SahmatAhsamat(int? type, FilterModel filter, int? limit, int? page)
        {
            ViewData["Occupation"] = service.getOccupation();
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            // Fetch filtered data directly from service
            var allData = service.GetAllDataSatisfiedUnsatisfied(VidhanSabhaId,type, filter,limit,page );
            ViewBag.Type = type;

            ViewData["SatisfiedUnsatisfiedAlldata"] = allData;

            ViewData["VillageList11"] = !string.IsNullOrEmpty(filter.boothIds) ? (object)service.GetVillageListByBoothId(filter.boothIds) : (object)service.GetAllVillage();

            return View();
        }

        [HttpGet]
        public ActionResult Prabhavsali( FilterModel filter,int? type,int? pageNumber = 1,int? pageSize = 10, int? limit = null, int? page = null)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var data = service.GetEffectivePersonsPaged(filter, type,pageNumber,pageSize,limit,page );

            ViewData["VillageList11"] = !string.IsNullOrEmpty(filter.boothIds) ? (object)service.GetVillageListByBoothId(filter.boothIds) : (object)service.GetAllVillage();

            ViewBag.effectivrType = type;
            return View(data);
        }

        [HttpGet]
        public ActionResult VarishthNaagarikViklaang(FilterModel filter,int? type,  int? limit = null, int? page = null)
        {
            //if (pageNumber < 1) pageNumber = 1;
            //if (pageSize < 1) pageSize = 10;
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var data = service.GetSeniorOrDisabled(VidhanSabhaId, filter, type, limit,page );
            ViewBag.Type = type;

            ViewData["VillageList11"] = !string.IsNullOrEmpty(filter.boothIds) ? (object)service.GetVillageListByBoothId(filter.boothIds) : (object)service.GetAllVillage();
            return View(data);
        }

        public ActionResult BlockPramukh(FilterModel filter, int? limit = null, int? page = null)
        {
            var blocks = service.GetAllBlocks(filter,limit,page);
            ViewData["Occupation"] = service.getOccupation();

            return View(blocks);
        }

        public ActionResult VoterPravasi(FilterModel filter, int? limit = null, int? page = null)
        {
            int VidhansabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            List<PravasiVoter> list = service.GetAllPravasiVoterData(VidhansabhaId,filter, limit,page);

            ViewData["AllPravasiVoter"] = list;
            ViewData["Occupation"] = service.getOccupation();
            PravasiVoter data = new PravasiVoter();
            return View(data);
        }
        public ActionResult VotersNew(FilterModel filter, int? limit = null, int? page = null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            List<NewVoters> votersList = service.GetNewVoters(VidhanSabhaId, filter, limit,page);
            ViewData["VillageList11"] = !string.IsNullOrEmpty(filter.boothIds) ? (object)service.GetVillageListByBoothId(filter.boothIds) : (object)service.GetAllVillage();
        
            return View(votersList);
        }
        #endregion

        #region Influencer Management 
        public ActionResult Influencer(FilterModel filter,int? limit, int? page)
        {
            ViewData["DesignationList"] = service.GetEffectiveDesignations();
            ViewData["InfluencerList"] = service.GetInfluencers(filter,limit,page);
           
            return View();
        }
        [HttpGet]
        public JsonResult GetPersonsByDesignation(int desgId,FilterModel filter)
        {
            var data = service.GetEffectivePersonsPaged(filter, desgId,null,null);
            return Json(new
            {
                status = data != null ? true : false,
                data = data
            },JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddInfluencer(Influencer model)
        {
            try
            {
                bool result = service.InsertInfluencer(model);

                return Json(new
                {
                    success = result,
                    message = result
                        ? (model.Id.HasValue && model.Id > 0 ? "Updated successfully." : "Inserted successfully.")
                        : "Operation failed."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public JsonResult DeleteInfluencerPerson(int id)
        {
            try
            {
                bool isDeleted = service.DeleteInfluencerPerson(id);
                return Json(new
                {
                    success = isDeleted,
                    message = isDeleted ? "Influencer Person deleted successfully." : "Deletion failed."
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetInfluencerById(int id)
        {
            var influencer = service.GetInfluencerById(id);

            if (influencer == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(influencer, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InfluencerList(FilterModel filter,
int? limit = null, int? page = null)
        {
            ViewData["influencerList"] = service.GetInfluencers(filter,limit,page);
            ViewData["Occupation"] = service.GetEffectiveDesignations();
            return View();
        }

        public ActionResult GetVillagesByBoothId(int BoothId)
        {
            var villages = service.GetVillageListByBoothId(BoothId.ToString());
            return Json(villages, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BoothListForList(FilterModel filter,int? limit=null, int? page=null)
        {
            ViewData["BoothList"] = service.GetBoothList(filter,limit,page);

            var sectors = service.GetAll_SectorDetails(filter);
            var sectorBoothsDict = new Dictionary<int, List<Booth>>();

            foreach (var sector in sectors)
            {
                var booths = service.GetBoothslistbySectorId(sector.Id);
                sectorBoothsDict[sector.Id] = booths; // ✅ Keep it separate, no sector.Booths
            }
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            //ViewData["MandalList"] = service.GetAllMandal();
            ViewData["SectorList"] =!string.IsNullOrEmpty(filter.mandalIds)? service.GetSectorByMandalId(filter.mandalIds) : service.GetAll_Sector(VidhanSabhaId);
            ViewData["SectorListUseToCimbinedReport"] = sectors;
            ViewData["BoothDataBySector"] = sectorBoothsDict;

            return View();
        }
        #endregion

        [HttpGet]
        public ActionResult Pradhan(int? limit = null, int? page = null)
        {
            var model = service.GetAllPradhan(limit,page);           

            var designationList = service.GetEffectiveDesignations();
            ViewData["DesignationList"] = designationList;           

            //var villageList = service.GetAllVillages();
            //ViewData["VillageList"] = villageList;
            return View(model);
        }
        [HttpPost]
        public JsonResult SavePradhan(Pradhan model)
        {
            try
            {
                bool isSaved;

                    isSaved = service.InsertPradhan(model);
                

                if (isSaved)
                {
                    return Json(new
                    {
                        success = true,
                        message = model.Id > 0
                            ? "Pradhan updated successfully."
                            : "Pradhan added successfully."
                    });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to save Pradhan." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public ActionResult DeletePradhan(int id)
        {
            bool result = service.DeletePradhan(id);

            if (result)
            {
                return Json(new { success = true, message = "Record deleted successfully." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete record." }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPradhanById(int id)
        {
            var result = service.GetAllPradhanById(id);
            return Json(result, JsonRequestBehavior.AllowGet);
            
           
        }


        public ActionResult PradhanList(int? limit = null, int? page = null)
        {

            ViewData["PradhanList"] = service.GetAllPradhan(limit,page);
            return View();
        }

        public ActionResult BDCList(int? limit = null, int? page = null)
        {
            ViewData["BDCList"] = service.GetBDC(limit,page);
            return View();
        }
    }
}


