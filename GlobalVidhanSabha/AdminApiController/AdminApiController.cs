using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;
using Newtonsoft.Json;
using VishanSabha.Models;
using VishanSabha.Services;
using VishanSabha.Services.Auth;
using VishanSabha.Services.SectorService;

namespace VishanSabha.AdminApiController
{
    public class AdminApiController : ApiController
    {
        private readonly AdminServices _adminService;
        private readonly AuthService _authService;
        private readonly BoothService _boothService;
        private readonly SectorService _sectorService;

        public AdminApiController()
        {
            _adminService = new AdminServices();
            _authService = new AuthService();
            _boothService = new BoothService();
            _sectorService = new SectorService();
        }

        [HttpPost]
        [Route("api/admin/addmandal")]
        public IHttpActionResult AddMandal()
        {
        
            var request = HttpContext.Current.Request;

            if (request == null || string.IsNullOrWhiteSpace(request.Form.Get("Name")))
                return Ok(new { status = false, StatusCode = 200, Message = "Invalid mandal data." });

            Mandal mandal = new Mandal();
            string idStr = request.Form.Get("id");
            mandal.Id = !string.IsNullOrWhiteSpace(idStr) && int.TryParse(idStr, out int parsedId) ? parsedId : 0;

            mandal.Name = request.Form.Get("Name");

            bool isAdded = _adminService.AddMandal(mandal,70);

            return isAdded
                ? Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    Message = mandal.Id > 0 ? "Mandal Updated successfully" : "Mandal added Successfully!!"
                })
                : Ok(new { status = false, StatusCode = 404, Message = "Some error occurred !!" });
        }

        [HttpGet]
        [Route("api/admin/GetName")]
        public IHttpActionResult GetName()
        {
            return Ok(new
            {
                status = true,
                message = "Hello"
            });
        }

        //[HttpGet]
        //[Route("api/admin/GetAllMandal")]
        //public IHttpActionResult GetAllMandal()
        //{
        //    var data = _adminService.GetAllMandal();

        //    if (data != null && data.Any())
        //    {
        //        return Json(new
        //        {
        //            data = data,
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            message = "No data found"
        //        });
        //    }
        //}
        [HttpGet]
        [Route("api/admin/DeleteMandalById")]
        public IHttpActionResult DeleteMandalById(int id)
        {

            var res = _adminService.DeleteById_Mandal(id);
            if (res)
            {
                return Json(new
                {
                    status = true,
                    message = "Mandal Deleted Successfully!!"
                });
            }
            else
            {
                return Json(new
                {
                    status = false,
                    message = "Some Error Occured!!"
                });
            }

        }
        [HttpGet]
        [Route("api/admin/GetMandalById")]
        public IHttpActionResult GetMandalById(int id)
        {
            var data = _adminService.GetById_Mandal(id);
            if (data != null)
            {
                return Json(new
                {
                    data = data
                });
            }
            else
            {
                return Json(new
                {
                    message = "Data Not Found !!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetSectorListwithBoothCountByMandalId")]
        public IHttpActionResult GetsectorListwithBoothCount(int MandalId)
        {
            var sectorList = _adminService.GetAllSectorWithBoothCountByMandalId(MandalId);
            if (sectorList != null && sectorList.Any())
            {
                return Ok(new
                {
                    status = true,
                    SectorListWithBoothCount = sectorList
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetBoothBySectorId")]
        public IHttpActionResult GetBoothBysectorId(int sectorId)
        {
            var res = _adminService.GetAll_BoothForListBySectorId(sectorId);
            if (res != null && res.Any())
            {
                return Ok(new
                {
                    status = true,
                    BoothListBySectorId = res
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/SelectMandalSectorBoothcount")]
        public IHttpActionResult selectMandalSectorboothCount()
        {
            var data = _adminService.selectAllSectorBoothCount();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    MandalBySectorboothcount = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });

            }
        }

        [HttpGet]
        [Route("api/admin/SectorListByBoothCount")]
        public IHttpActionResult SectorListByBoothCount()
        {
            var data = _adminService.GetAllSectorWithBoothCount();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    SectorListwithBoothCount = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });

            }
        }
        [HttpGet]
        [Route("api/admin/GetBoothListByMandalId")]
        public IHttpActionResult GetBoothListByMandalId(int mandalId)
        {
            var res = _adminService.GetAll_BoothForListByMandalId(mandalId);
            if (res != null && res.Any())
            {
                return Ok(new
                {
                    status = true,
                    BoothListByMandalId = res
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

        //Sector 
        //[HttpPost]
        //[Route("api/admin/AddSector")]
        //public IHttpActionResult AddSector()
        //{
        //    var request = HttpContext.Current.Request;
        //    if (request == null)
        //        return Ok("All Fields are required!!");
        //    var SectorInchargeName = request.Form.Get("SectorInchargeName");
        //    Sector sector = new Sector();
        //    sector.Id = string.IsNullOrWhiteSpace(request.Form.Get("Id")) ? 0 : Convert.ToInt32(request.Form.Get("Id"));
        //    sector.Mandal_Id = string.IsNullOrWhiteSpace(request.Form.Get("Mandal_Id")) ? 0 : Convert.ToInt32(request.Form.Get("Mandal_Id"));
        //    //sector.SectorNo = request.Form.Get("SectorNo");
        //    sector.SectorName = request.Form.Get("SectorName");
        //    sector.SectorInchargeName = Convert.ToInt32(SectorInchargeName);
        //        sector.Village = request.Form["VillageId"];

        //    if (sector.SectorInchargeName == 1)
        //    {
        //        sector.InchargeName = request.Form.Get("InchargeName");
        //        sector.FatherName = request.Form.Get("FatherName");
        //        sector.Age = string.IsNullOrWhiteSpace(request.Form.Get("Age")) ? 0 : Convert.ToInt32(request.Form.Get("Age"));
        //        sector.CasteId = request.Form.Get("CasteId");
        //        sector.subcaste = request.Form.Get("SubCaste");
        //        sector.Address = request.Form.Get("Address");
        //        sector.Education = request.Form.Get("Education");
        //        sector.PhoneNumber = request.Form.Get("PhoneNumber");
        //        HttpPostedFile file = request.Files["ProfileImage"];
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            string folderPath = HttpContext.Current.Server.MapPath("~/UploadedImages/Sector/");
        //            if (!Directory.Exists(folderPath))
        //            {
        //                Directory.CreateDirectory(folderPath);
        //            }
        //            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //            string fullPath = Path.Combine(folderPath, fileName);
        //            file.SaveAs(fullPath);
        //            sector.ProfileImage = "/UploadedImages/Sector/" + fileName;
        //        }
        //    }

        //    var res = _adminService.AddSector(sector);

        //    if (res)
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            StatusCode = 200,
        //            message = sector.Id > 0 ? "Sector Updated Successfully!!" : "Sector Added Successfully!!"
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            StatusCode = 404,
        //            message = "Some Error Occured!!"
        //        });
        //    }
        //}
        //[HttpGet]
        //[Route("api/admin/GetAllSector")]
        //public IHttpActionResult GetAllSector(int? limit = null, int? page = null)
        //{
        //    var data = _adminService.GetAll_Sector(limit, page);
        //    if (data != null && data.Any())
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            StatusCode = 200,
        //            data = data
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status = false,
        //            StatusCode = 404,
        //            message = "No Data Found!!"
        //        });
        //    }
        //}

        [HttpGet]
        [Route("api/admin/DeleteSectorById")]
        public IHttpActionResult DeleteSectorById(int id)
        {
            var res = _adminService.DeleteById_Sector(id);
            if (res)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Sector Deleted Successfully !!"
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    StatusCode = 404,
                    message = "Some Error Occuerd!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetSectorById")]
        public IHttpActionResult GetSectorById(int id)
        {
            var data = _adminService.GetById_Sector(id);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,

                    data = data
                });
            }
            else
            {
                return Ok(new
                {
                    statuscode = 404,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/SectorDetailsForReport")]
        public IHttpActionResult SectorDetailsForReports([FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            var SectorData = _adminService.GetAll_SectorDetails(filter);
            if (SectorData != null && SectorData.Any())
            {
                return Ok(new
                {
                    status = true,
                    SectorListForreports = SectorData
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetBoothListForReport")]
        public IHttpActionResult GetBoothListForReport([FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            var BoothData = _adminService.GetBoothReport(filter);
            if (BoothData != null && BoothData.Any())
            {
                return Ok(new
                {
                    status = true,
                    BoothReportData = BoothData
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/MandalCount")]
        public IHttpActionResult MandalCount()
        {
            int mandal = _adminService.TotalMandalCount();
            if (mandal > 0)
            {
                return Ok(new
                {
                    status = true,
                    MandalCount = mandal
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

        //[Route("api/admin/GetAllBooth")]
        //public IHttpActionResult GetAll_Booth(int? limit = null, int? page = null)
        //{
        //    var data = _adminService.GetAll_BoothForTable(limit,page);
        //    if (data != null && data.Any())
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            StatusCode = 200,
        //            data = data
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status = false,
        //            StatusCode = 404,
        //            message = "No Data Found!!"
        //        });
        //    }
        //}
        [HttpGet]
        [Route("api/Admin/DeleteBoothById")]
        public IHttpActionResult DeleteBoothById(int id)
        {
            var res = _adminService.DeleteById_Booth(id);
            if (res)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Booth Deleted Successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    StatusCode = 404,
                    message = "Some Error Occcured!!"
                });
            }

        }

        [HttpGet]
        [Route("api/admin/GetBoothById")]
        public IHttpActionResult GetBoothById(int id)
        {
            var data = _adminService.GetById_Booth(id);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    data = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllCaste")]
        public IHttpActionResult GetAllCaste()
        {
            var data = _adminService.GetAllCategory();
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    data = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    StatusCode = 404,
                    message = "No Data Found !!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetAllSubcaste")]
        public IHttpActionResult GetAllSubCaste()
        {
            var data = _adminService.GetAllSubCaste();
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    data = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    StatusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetSubCasteByCasteId")]
        public IHttpActionResult GetSubCasteByCasteId(int casteId)
        {
            var data = _adminService.GetByCasteId_SubCaste(casteId);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    data = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    StatusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }
     
        //[Route("api/auth/login")]
        //public IHttpActionResult Login(string contact, string password)
        //{
        //    if (string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(password))
        //    {
        //        return BadRequest("Invalid request.");
        //    }

        //    var user = _authService.ValidateUser(contact, password);

        //    if (user != null)
        //    {
        //        //HttpContext.Current.Session["Contact"] = user.Contact;
        //        int userid = _authService.GetBoothInchargeId(contact);


        //        var response = new LoginResponse
        //        {

        //            Success = true,
        //            Message = "Login successful",
        //            UserId = userid,
        //            Contact = user.Contact,
        //            Role = user.Role
        //        };

        //        return Ok(response);
        //    }

        //    return Ok(new LoginResponse
        //    {
        //        Success = false,
        //        Message = "Invalid contact or password"
        //    });
        //}

        [HttpPost]
        [Route("api/auth/login")]
        public IHttpActionResult Login(string contact, string password)
        {
            if (string.IsNullOrEmpty(contact) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Invalid request. Please provide both contact and password.");
            }

            var user = _authService.ValidateUser(contact, password);

            if (user != null)
            {
             
                int userId = _authService.GetBoothInchargeId(contact);

               
                string redirectAction = "";
                string redirectController = "";

                switch (user.Role)
                {
                    case "Admin":
                        redirectAction = "Dashboard";
                        redirectController = "Admin";
                        break;
                    case "SectorIncharge":
                        redirectAction = "SectorDashboard";
                        redirectController = "Sector";
                        break;
                    case "BoothIncharge":
                        redirectAction = "BoothDashboard";
                        redirectController = "Booth";
                        break;
                    default:
                        redirectAction = "Dashboard";
                        redirectController = "Admin";
                        break;
                }

                var response = new
                {
                    Success = true,
                    Message = $"Welcome back, {user.Contact}!",
                    UserId = userId,
                    Contact = user.Contact,
                    Role = user.Role,
                    RedirectUrl = $"/{redirectController}/{redirectAction}"
                };

                return Ok(response);
            }

            // If login failed
            var errorResponse = new
            {
                Success = false,
                Message = "Invalid contact or password!"
            };

            return Ok(errorResponse);
        }



        //Sector Sanyojak Pannel

        [HttpGet]
        [Route("api/admin/GetSectorByMandalId")]
        public IHttpActionResult GetSectorByMandalId(int mandalId, int? limit = null, int? page = null)
        {
            var sectorList = _adminService.GetSectorByMandalId(mandalId.ToString(), limit, page);
            if (sectorList != null && sectorList.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    sectorDataByMandalId = sectorList
                });

            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }
     

        //sector Pannal end




        //Add New Voters
        [HttpPost]
        [Route("api/admin/addNewVoter")]
        public IHttpActionResult AddNewVoter()
        {
            var form = HttpContext.Current.Request;
            if (form == null)
                return Ok("All Fields are required!!");
            NewVoters data = new NewVoters();
            string idStr = form.Form.Get("Id");
            data.Id = !string.IsNullOrWhiteSpace(idStr) && int.TryParse(idStr, out int parsedId) ? parsedId : 0;
            data.Booth_Id = Convert.ToInt32(form.Form.Get("Booth_Id"));
            data.Name = form.Form.Get("Name").ToString();
            data.FatherName = form.Form.Get("FatherName").ToString();
            data.MobileNumber = form.Form.Get("MobileNumber").ToString();
            data.CasteId = Convert.ToInt32(form.Form.Get("CasteId"));
            data.SubCasteId = Convert.ToInt32(form.Form.Get("SubCasteId"));
            data.DOB = form.Form.Get("DOB").AsDateTime();
            data.totalage = form.Form.Get("totalage").ToString();
            data.VillageListId = form.Form.Get("VillageListId").ToString();
            //data.Education = form.Form.Get("Education").ToString();

            var res = _adminService.AddNewVoters(data);
            if (res)
            {
                return Ok(new
                {
                    status = true,
                    messsage = data.Id > 0 ? "Data Updated Successfully!!" : "Data Added Successfully!!"

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    messsage = data.Id > 0 ? "Some error occured!!" : "Some Error Occured!!"

                });
            }
        }
        //[HttpGet]
        //[Route("api/admin/GetAllNewVoters")]
        //public IHttpActionResult GetAllNewVoter([FromUri] FilterModel filter, int? limit = null, int? page=null)
        //{
        //    filter = filter ?? new FilterModel();
        //    var data = _adminService.GetNewVoters(filter, limit, page);
        //    if (data != null && data.Any())
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            NewVoters = data
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status = false,
        //            message = "No Data Found!!"
        //        });
        //    }
        //}
        [HttpGet]
        [Route("api/admin/DeleteNewVotersById")]
        public IHttpActionResult DeleteNewVotersById(int id)
        {
            var res = _adminService.deleteNewVoter(id);
            if (res)
            {
                return Ok(new
                {
                    status = true,
                    message = "Data Deleted Successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "Some Error Occured!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetNewVotersById")]
        public IHttpActionResult GetNewVotersById(int id)
        {
            var res = _adminService.getVoterById(id);
            if (res != null)
            {
                return Ok(new
                {
                    status = true,
                    NewVoter = res,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

        //New Voter End

        //Admin Dashboard
        [HttpGet]
        [Route("api/admin/SectorCount")]
        public IHttpActionResult SectorCount()
        {
            int secCount = _adminService.SectorCount();
            if (secCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    SectorCount = secCount,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }


        [HttpGet]
        [Route("api/admin/BoothCount")]
        public IHttpActionResult BoothCount()
        {
            int boothCount = _adminService.BoothCount();
            if (boothCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    BoothCount = boothCount,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/PannaPramukhCount")]
        public IHttpActionResult PannaPramukhCount()
        {
            int PannaCount = _adminService.GetTotalPannaPramukhCount();
            if (PannaCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    PannaCount = PannaCount,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/TotalPravasiVoterCount")]
        public IHttpActionResult PravasiVoterCount()
        {
            int PravasiCount = _adminService.GetTotalPravasi();
            if (PravasiCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    PravasiVoterCount = PravasiCount,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/TotalNewVotersCount")]
        public IHttpActionResult NewVotersCount()
        {
            int newvoterCount = _adminService.GetTotalVoters();
            if (newvoterCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    NewVoterCount = newvoterCount,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/TotalDoubleVoterCount")]
        public IHttpActionResult DoubleVoterCount()
        {
            int DoubleVoterCount = _adminService.GetTotalDoubleVoter();
            if (DoubleVoterCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    DoubleVoterCount = DoubleVoterCount,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/TotalEffectivePersonCount")]
        public IHttpActionResult EffectivePersonCount()
        {
            int effectivepersonCount = _adminService.GetTotalEffectivePerson();
            if (effectivepersonCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    EffectivePersonCount = effectivepersonCount,

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    statusCode = 404,
                    message = "No Data Found!!"
                });
            }
        }



        //end

        //Panna Pramukh
        [HttpPost]
        [Route("api/admin/addPannaPramukh")]
        public IHttpActionResult AddPannaPramukh()
        {
            var form = HttpContext.Current.Request;
            if (form == null)
                return Ok(new { status = false, message = "Invalid request." });

            PannaPramukh data = new PannaPramukh();

            string idStr = form.Form.Get("PannaPramukh_Id");
            data.PannaPramukh_Id = !string.IsNullOrWhiteSpace(idStr) && int.TryParse(idStr, out int parsedId) ? parsedId : 0;
            data.Booth_Id = Convert.ToInt32(form.Form.Get("Booth_Id") ?? "0");
            data.Pannapramukh = form.Form.Get("Pannapramukh") ?? "";
            data.PannaNumber = form.Form.Get("PannaNumber") ?? "";
            //data.PannaNumTo = form.Form.Get("PannaNumTo") ?? "";
            data.Category = form.Form.Get("Category") ?? "";
            data.Cast = form.Form.Get("Cast") ?? "";
            data.VoterNumber = form.Form.Get("VoterNumber") ?? "";
            //data.AadharNumber = form.Form.Get("AadharNumber") ?? "";
            data.Address = form.Form.Get("Address") ?? "";
            data.Mobile = form.Form.Get("Mobile") ?? "";
            data.VillageListId = form.Form.Get("VillageListId") ?? "";
            HttpPostedFile file = form.Files["ProfileImage"];


            if (file != null && file.ContentLength > 0)
            {
                string folderPath = HttpContext.Current.Server.MapPath("~/UploadedImages/Sector/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fullPath = Path.Combine(folderPath, fileName);
                file.SaveAs(fullPath);
                data.ProfileImageUrl = "/UploadedImages/Sector/" + fileName;
            }

            var res = _adminService.InsertPannaPramukh(data);

            if (res)
            {
                return Ok(new
                {
                    status = true,
                    message = data.PannaPramukh_Id > 0 ? "Data Updated Successfully!!" : "Data Added Successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = data.PannaPramukh_Id > 0 ? "Error while updating!!" : "Error while adding!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetallPannaPramukhList")]
        public IHttpActionResult GetAllPannaPramukh(int? limit = null, int? page = null)
        {
            var PannaList = _adminService.GetAllPannaPramukhList(limit, page);
            if (PannaList != null && PannaList.Any())
            {
                return Ok(new
                {
                    status = true,
                    PannaPramukhList = PannaList
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/DashboardCounts")]
        public IHttpActionResult DashboardTotalcount()
        {
            var data = _adminService.DashboardTotalCount();
            if (data != null && data.Count > 0)
            {
                return Ok(new
                {
                    status = true,
                    DaschboardCount = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }


        [HttpGet]
        [Route("api/admin/DeletePannaPramukhById")]
        public IHttpActionResult DeletePannaById(int pannaId)
        {
            var res = _adminService.DeleteById_PannaPramukh(pannaId);
            if (res)
            {
                return Ok(new
                {
                    status = true,
                    message = "Data deleted successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "Some error occured!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetPannaPramukhById")]
        public IHttpActionResult GetPannaPramukhById(int pannaId)
        {
            var PannaList = _adminService.GetAll_PannaPramukhById(pannaId);
            if (PannaList != null && PannaList.Any())
            {
                return Ok(new
                {
                    status = true,
                    PannaPramukhList = PannaList
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpPost]
        [Route("api/admin/UpdatePannaPramukh")]
        public IHttpActionResult UpdatePannaPramukh()
        {
            var form = HttpContext.Current.Request;
            if (form == null)
                return Ok(new { status = false, message = "Invalid request." });

            PannaPramukh data = new PannaPramukh();

            //string idStr = form.Form.Get("PannaPramukh_Id");
            //data.PannaPramukh_Id = !string.IsNullOrWhiteSpace(idStr) && int.TryParse(idStr, out int parsedId) ? parsedId : 0;
            data.PannaPramukh_Id = Convert.ToInt32(form.Form.Get("PannaPramukh_Id"));
            data.Booth_Id = Convert.ToInt32(form.Form.Get("Booth_Id") ?? "0");
            data.Pannapramukh = form.Form.Get("Pannapramukh") ?? "";
            data.PannaNumber = form.Form.Get("PannaNumber") ?? "";
            data.PannaNumTo = form.Form.Get("PannaNumTo") ?? "";
            data.Category = form.Form.Get("Category") ?? "";
            data.Cast = form.Form.Get("Cast") ?? "";
            data.VoterNumber = form.Form.Get("VoterNumber") ?? "";
            //data.AadharNumber = form.Form.Get("AadharNumber") ?? "";
            data.Address = form.Form.Get("Address") ?? "";
            data.Mobile = form.Form.Get("Mobile") ?? "";
            data.VillageListId = form.Form.Get("VillageListId") ?? "";
            HttpPostedFile file = form.Files["ProfileImage"];


            if (file != null && file.ContentLength > 0)
            {
                string folderPath = HttpContext.Current.Server.MapPath("~/UploadedImages/Sector/");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string fullPath = Path.Combine(folderPath, fileName);
                file.SaveAs(fullPath);
                data.ProfileImageUrl = "/UploadedImages/Sector/" + fileName;
            }

            var res = _adminService.UpdatePannaPramukh(data);

            if (res)
            {
                return Ok(new
                {
                    status = true,
                    message = "Data Updated Successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "Error while updating!!"
                });
            }
        }

        #region EffectivePerson Nazif
        [HttpGet]
        [Route("api/admin/GetAllEffectivePerson")]
        public IHttpActionResult GetAll(int? limit = null, int? page=null)
        {
            var data = _adminService.GetAllEffectivePersonsforapi(limit,page);

            return Ok(new
            {
                status = data != null && data.Any(),
                message = data != null && data.Any() ? "Data fetched successfully." : "No data found.",
                data
            });
        }
        [HttpGet]
        [Route("api/admin/EffectivepersonById")]
        public IHttpActionResult EffectivepersonById(int id)
        {
            var person = _adminService.GetEffectivePersonById(id);
            if (person != null)
                return Ok(person);
            return NotFound();
        }
        //end
        [HttpPost]
        [Route("api/admin/AddEffectiveperson")]
        public IHttpActionResult AddEffectiveperson()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                // Read form fields
                var model = new EffectivePerson();

                int.TryParse(httpRequest.Form["effectivePersonId"], out int effectivePersonId);
                model.effectivePersonId = effectivePersonId;

                model.Name = httpRequest.Form["Name"];
                model.Designation = httpRequest.Form["Designation"];
                model.Cast = httpRequest.Form["Cast"];
                model.Category = httpRequest.Form["Category"];

                if (int.TryParse(httpRequest.Form["Booth_Id"], out int boothId))
                    model.Booth_Id = boothId;

                model.VillageListId = httpRequest.Form["VillageListId"];
                model.Mobile = httpRequest.Form["Mobile"];
                model.Description = httpRequest.Form["Description"];

                model.Status = httpRequest.Form["Status"] == "true" || httpRequest.Form["Status"] == "1";

                bool isSaved = model.effectivePersonId > 0
                    ? _adminService.UpdateEffectivePerson(model)
                    : _adminService.InsertEffectivePerson(model);

                if (isSaved)
                {
                    return Ok(new
                    {
                        success = true,
                        message = model.effectivePersonId > 0
                            ? "Effective Person updated successfully."
                            : "Effective Person added successfully."
                    });
                }

                return BadRequest("Failed to save Effective Person.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [System.Web.Http.HttpDelete]
        [Route("api/admin/DeleteEffectiveperson")]
        public IHttpActionResult DeleteEffectiveperson(int id)
        {
            try
            {
                bool isDeleted = _adminService.DeleteEffectivePerson(id);
                return Ok(new
                {
                    status = isDeleted,
                    message = isDeleted ? "Deleted successfully." : "Deletion failed."
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion


        //[HttpGet]
        //[Route("api/admin/GetAllDoubleVoters")]
        //public IHttpActionResult GetDoubleVoters(int? limit = null,int? page=null)
        //{
        //    var data = _adminService.GetAllDoubleVoters(limit,page);
        //    if (data != null)
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            DoubleVoters = data,
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status = false,
        //            Message = "No Data Found!!",
        //        });
        //    }
        //}


        [HttpGet]
        [Route("api/admin/GetAllDoubleVotersForList")]
        public IHttpActionResult GetDoubleVotersforList([FromUri] FilterModel filter,int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _adminService.GetDoubleVoterReport(filter,limit, page);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    DoubleVoters = data,
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    Message = "No Data Found!!",
                });
            }
        }


        //[HttpGet]
        //[Route("api/admin/GetAllPravasiVoter")]
        //public IHttpActionResult GetPravasiVoter([FromUri] FilterModel filter, int? limit = null, int? page = null)
        //{
        //    filter = filter ?? new FilterModel();
        //    var data = _adminService.GetAllPravasiVoterData(filter, limit, page);
        //    if (data != null)
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            PravasiVoters = data,
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status = false,
        //            Message = "No Data Found!!",
        //        });
        //    }
        //}




        #region  BoothVoterDesc Nazif
        //[HttpGet]
        //[Route("api/admin/GetAllBoothVoterDesc")]
        //public IHttpActionResult GetAllBoothVoterDesc(int? limit = null, int? page=null)
        //{
        //    var data = _adminService.getBoothVoterDes(limit,page);
        //    if (data != null)
        //    {
        //        return Content(HttpStatusCode.OK, new
        //        {
        //            status = true,
        //            message = "Data fetched successfully.",
        //            BoothVotersDesc = data
        //        });
        //    }
        //    else
        //    {
        //        return Content(HttpStatusCode.NotFound, new
        //        {
        //            status = false,
        //            message = "No data found.",

        //        });
        //    }
        //}
        [HttpGet]
        [Route("api/admin/getBoothVoterDesById")]
        public IHttpActionResult GetBoothVoterDesById(int Id)
        {

            var result = _adminService.getBoohDesById(Id);

            if (result != null && result.Id > 0)
            {
                return Content(HttpStatusCode.OK, new { status = true, data = result });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new { status = false, message = "No record found." });
            }
        }
        [HttpPost]
        [Route("api/admin/addOrUpdateBoothVoterDes")]
        public IHttpActionResult AddOrUpdateBoothVoterDes()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                if (form == null)
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = false,
                        message = "Form data is missing."
                    });
                }

                // Parsing and validating form fields
                int.TryParse(form["Id"], out int id);
                int.TryParse(form["TotalVoters"], out int totalVoters);
                int.TryParse(form["TotalMan"], out int totalMan);
                int.TryParse(form["TotalWoman"], out int totalWoman);
                int.TryParse(form["TotalOther"], out int totalOther);
                int.TryParse(form["BoothId"], out int boothId);
                string VillageListId = form["VillageListId"]?.ToString() ?? "";

                BoothVotersDes model = new BoothVotersDes
                {
                    Id = id,
                    BoothName = form["BoothName"] ?? "",
                    BoothNumber = form["BoothNumber"] ?? "",
                    TotalVoters = totalVoters,
                    TotalMan = totalMan,
                    TotalWoman = totalWoman,
                    TotalOther = totalOther,
                    BoothId = boothId,
                    VillageListId= VillageListId
                };

                bool result = _adminService.addOrUpdateBoothVoterDes(model);

                if (result)
                {
                    return Content(HttpStatusCode.OK, new
                    {
                        status = true,
                        message = model.Id > 0 ? "Updated successfully." : "Added successfully."
                    });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new
                    {
                        status = false,
                        message = model.Id > 0 ? "Failed to update." : "Failed to add."
                    });
                }
            }
            catch (Exception ex)
            {
                // Optional: Log the error
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An error occurred.",
                    error = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("api/admin/deleteBoothVoterDesc")]
        public IHttpActionResult deleteBoothVoterDesc(int id)
        {
            var result = _adminService.DeleteBoothVoterDes(id);
            if (result)
            {
                return Content(HttpStatusCode.OK, new
                {
                    status = true,
                    message = "Deleted successfully."
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "No record Found"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/getinchargebyboothId")]
        public IHttpActionResult GetBoothIncharge(int boothId)
        {
            // Call your service to get the incharge name
            string inchargeName = _adminService.GetBoothInchargeNameByBoothId(boothId);

            if (!string.IsNullOrEmpty(inchargeName))
            {
                return Ok(new { success = true, Incharge = inchargeName });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new { success = false, message = "Booth Incharge not found" });
            }
        }

        [HttpGet]
        [Route("api/admin/getAllBoothSamitiIncharges")]
        public IHttpActionResult GetAllBoothSamitiIncharges(
int? limit = null, int? page = null)
        {
            try
            {
                var result = _adminService.GetAllBoothSamitiIncharges(limit,page);

                if (result != null && result.Count > 0)
                {
                    return Content(HttpStatusCode.OK, new
                    {
                        status = true,
                        message = "Booth Samiti Incharges fetched successfully.",
                        data = result
                    });
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "No Booth Samiti Incharges found.",
                        data = new List<BoothSamiti>()
                    });
                }
            }
            catch (Exception ex)
            {
                // Optional: log the exception
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An error occurred while fetching data.",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("api/admin/insertBoothIncharge")]
        public IHttpActionResult InsertBoothIncharge()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                if (form == null || string.IsNullOrEmpty(form["BoothId"]) || string.IsNullOrEmpty(form["BoothIncharge"]))
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = false,
                        message = "BoothId and Incharge_Name are required."
                    });
                }

                if (!int.TryParse(form["BoothId"], out int boothId))
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = false,
                        message = "Invalid BoothId."
                    });
                }

                string inchargeName = form["BoothIncharge"];

                BoothSamiti model = new BoothSamiti
                {
                    BoothId = boothId,
                    BoothIncharge = inchargeName
                };

                bool isInserted = _adminService.InsertBoothIncharge(model);

                if (isInserted)
                {
                    return Content(HttpStatusCode.OK, new
                    {
                        status = true,
                        message = "Booth Inchargeinserted successfully."
                    });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new
                    {
                        status = false,
                        message = "Failed to insert Booth Incharge."
                    });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("api/admin/getBoothSamitiByBoothId")]
        public IHttpActionResult GetBoothSamitiByBoothId(int boothId)
        {
            try
            {
                if (boothId <= 0)
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = false,
                        message = "Invalid BoothId."
                    });
                }

                var result = _adminService.GetBoothSamitiByBoothId(boothId);

                if (result != null && result.Count > 0)
                {
                    return Content(HttpStatusCode.OK, new
                    {
                        status = true,
                        message = "Booth Samiti members fetched successfully.",
                        data = result
                    });
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "No Booth Samiti members found for the given BoothId.",
                        data = new List<BoothSamiti>()
                    });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An error occurred while fetching data.",
                    error = ex.Message
                });
            }
        }
        [HttpPost]
        [Route("api/admin/insertBoothSamiti")]
        public IHttpActionResult InsertBoothSamiti()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                if (form == null || string.IsNullOrWhiteSpace(form["BoothId"]))
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = false,
                        message = "BoothId  required."
                    });
                }

                BoothSamiti model = new BoothSamiti
                {
                    BoothId = int.TryParse(form["BoothId"], out var boothId) ? boothId : 0,
                    DesignationId = int.TryParse(form["DesignationId"], out var desVal) ? desVal : 0,

                    Name = form["Name"],
                    Cast = form["Cast"],
                    Category = form["Category"],
                    Age = int.TryParse(form["Age"], out var ageVal) ? (int?)ageVal : null,
                    Mobile = form["Mobile"],
                    Occupation = form["Occupation"]
                };


                bool isInserted = _adminService.InsertBoothSamiti(model);

                if (isInserted)
                {
                    return Content(HttpStatusCode.OK, new
                    {
                        status = true,
                        message = "Booth Samiti member inserted successfully."
                    });
                }
                else
                {
                    return Content(HttpStatusCode.InternalServerError, new
                    {
                        status = false,
                        message = "Failed to insert Booth Samiti member."
                    });
                }
            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "SQL error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        [Route("api/admin/updateBoothSamiti")]
        public IHttpActionResult UpdateBoothSamiti()
        {
            try
            {
                var form = HttpContext.Current.Request.Form;

                // Required fields validation
                if (form == null || string.IsNullOrWhiteSpace(form["BoothSamiti_Id"]))
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = false,
                        message = "BoothSamiti_Id required."
                    });
                }

                // Parse and bind values
                BoothSamiti model = new BoothSamiti
                {
                    BoothSamiti_Id = int.TryParse(form["BoothSamiti_Id"], out var samitiId) ? samitiId : 0,
                    //BoothId = int.TryParse(form["BoothId"], out var boothId) ? boothId : 0,
                    DesignationId = int.TryParse(form["DesignationId"],out var desId)? desId:0,
                   
                    Name = form["Name"],
                    Cast = form["Cast"],
                    Category = form["Category"],
                    Age = !string.IsNullOrWhiteSpace(form["Age"]) && int.TryParse(form["Age"], out var ageVal) ? (int?)ageVal : null,
                    Mobile = form["Mobile"],
                    Occupation = form["Occupation"],
                    //BoothIncharge = form["BoothIncharge"]
                };

                // Double-check IDs
                if (model.BoothSamiti_Id <= 0)
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = false,
                        message = "Invalid BoothSamiti_Id or BoothId."
                    });
                }

                // Call service
                bool updated = _adminService.UpdateBoothSamiti(model);

                if (updated)
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Booth Samiti record updated successfully."
                    });
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "Update failed. Record may not exist or nothing was changed."
                    });
                }
            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "A database error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
            }
        }


        [HttpGet]

        [Route("api/admin/getBoothSamitiBySamitiId")]
        public IHttpActionResult GetBoothSamitiById(int BoothSamiti_Id)
        {
            if (BoothSamiti_Id <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = "Invalid BoothSamiti_Id."
                });
            }

            var record = _adminService.GetAllBoothSamiti()
                                      .FirstOrDefault(b => b.BoothSamiti_Id == BoothSamiti_Id);

            if (record == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    success = false,
                    message = "Record not found."
                });
            }

            return Ok(new
            {
                success = true,
                data = record
            });
        }
        [HttpGet]
        [Route("api/admin/deleteBoothSamiti")]
        public IHttpActionResult SoftDeleteBoothSamiti(int BoothSamiti_Id)
        {
            if (BoothSamiti_Id <= 0)
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    status = false,
                    message = "Invalid BoothSamiti_Id."
                });
            }

            try
            {
                bool isDeleted = _adminService.SoftDeleteBoothSamiti(BoothSamiti_Id);

                if (isDeleted)
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Booth Samiti member deleted successfully"
                    });
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "Record not found or already deleted."
                    });
                }
            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "Database error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
            }
        }
        #endregion


        #region VillagesByIds Nazif
        [HttpGet]
        [Route("api/admin/villageBySectorId")]
        public IHttpActionResult GetVillageBySectorId(string sectorId)
        {

            if (sectorId == null)
            {
                return BadRequest("Invalid SectorId.");
            }

            try
            {
                var data = _adminService.GetVillageBySectorId(sectorId); // Your data access method

                if (data == null || !data.Any())
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "No villages found for the given Sector ID."
                    });
                }

                return Ok(new
                {
                    status = true,
                    message = "Villages retrieved successfully.",
                    data = data
                });
            }
            catch (SqlException ex)
            {
                return InternalServerError(new Exception("Database error occurred.", ex));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        //[HttpGet]
        //[Route("api/admin/getVillagesByMandalId")]
        //public IHttpActionResult GetVillagesByMandalId(int mandalId)
        //{
        //    if (mandalId <= 0)
        //    {
        //        return Content(HttpStatusCode.BadRequest, new
        //        {
        //            status = false,
        //            message = "Invalid Mandal ID."
        //        });
        //    }

        //    try
        //    {
        //        List<VillageList> villages = _adminService.getVillageByMandalId(mandalId); // Your method here

        //        if (villages == null || villages.Count == 0)
        //        {
        //            return Content(HttpStatusCode.NotFound, new
        //            {
        //                status = false,
        //                message = "No villages found for the given Mandal ID."
        //            });
        //        }

        //        return Ok(new
        //        {
        //            status = true,
        //            message = "Villages retrieved successfully.",
        //            data = villages
        //        });
        //    }
        //    catch (SqlException ex)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, new
        //        {
        //            status = false,
        //            message = "Database error occurred.",
        //            error = ex.Message
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, new
        //        {
        //            status = false,
        //            message = "An unexpected error occurred.",
        //            error = ex.Message
        //        });
        //    }
        //}
        //[HttpPost]
        //[Route("api/admin/AddBooths")]
        //public IHttpActionResult AddBooths()
        //{
        //    try
        //    {
        //        var httpRequest = HttpContext.Current.Request;

        //        if (httpRequest == null || httpRequest.Form.Count == 0)
        //        {
        //            return Content(HttpStatusCode.BadRequest, new { status = false, message = "Invalid request. Please send form data." });
        //        }

        //        // Build Booth object from form fields
        //        Booth booth = new Booth
        //        {
        //            Mandal_Id = Convert.ToInt32(httpRequest.Form["Mandal_Id"]),
        //            Sector_Id = Convert.ToInt32(httpRequest.Form["Sector_Id"]),
        //            BoothNumber = httpRequest.Form["BoothNumber"],
        //            BoothName = httpRequest.Form["BoothName"],
        //            BoothInchargeName = Convert.ToInt32(httpRequest.Form["BoothInchargeName"]),
        //            InchargeName = httpRequest.Form["Incharge_Name"],
        //            FatherName = httpRequest.Form["FatherName"],
        //            Age = Convert.ToInt32(httpRequest.Form["Age"]),
        //            CasteId = Convert.ToInt32(httpRequest.Form["CasteId"]),
        //            subcaste = httpRequest.Form["SubCaste"],
        //            Address = httpRequest.Form["Address"],
        //            Education = httpRequest.Form["Education"],
        //            PhoneNumber = httpRequest.Form["PhoneNumber"],
        //            boothLocation = httpRequest.Form["boothLocation"],
        //            Village = httpRequest.Form["VillageId"],
        //            Aanshik = httpRequest.Form["Aanshik"],
        //            Booth_Id = string.IsNullOrWhiteSpace(httpRequest.Form["Booth_Id"])
        //                        ? 0
        //                        : Convert.ToInt32(httpRequest.Form["Booth_Id"])
        //        };

        //        //// ✅ Handle VillageNames (comma-separated -> List<string>)
        //        //if (!string.IsNullOrWhiteSpace(httpRequest.Form["VillageNames"]))
        //        //{
        //        //    booth.VillageNames = httpRequest.Form["VillageNames"]
        //        //        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //        //        .Select(v => v.Trim())
        //        //        .ToList();
        //        //}
        //        //else
        //        //{
        //        //    booth.VillageNames = new List<string>();
        //        //}

        //        //// ✅ Handle VillageIds (comma-separated -> List<string>)
        //        //if (!string.IsNullOrWhiteSpace(httpRequest.Form["VillageId"]))
        //        //{
        //        //    booth.villageId = httpRequest.Form["VillageId"]
        //        //        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //        //        .Select(v => v.Trim())
        //        //        .ToList();
        //        //}
        //        //else
        //        //{
        //        //    booth.villageId = new List<string>();
        //        //}

        //        // ✅ Handle profile image upload and save to disk
        //        if (httpRequest.Files.Count > 0)
        //        {
        //            var file = httpRequest.Files[0];

        //            if (file != null && file.ContentLength > 0)
        //            {
        //                string folderPath = HttpContext.Current.Server.MapPath("~/UploadsImages/");
        //                if (!Directory.Exists(folderPath))
        //                {
        //                    Directory.CreateDirectory(folderPath);
        //                }

        //                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //                string fullPath = Path.Combine(folderPath, fileName);

        //                file.SaveAs(fullPath);

        //                // Save relative path
        //                booth.ProfileImage = "/UploadsImages/" + fileName;
        //            }
        //        }

        //        // ✅ Save to DB through service
        //        bool result = _adminService.AddBooth(booth);

        //        if (result)
        //        {
        //            return Ok(new { status = true, message = booth.Booth_Id>0?"Booth Updated successfully":"Booth Inserted successfully" });
        //        }
        //        else
        //        {
        //            return Content(HttpStatusCode.InternalServerError, new { status = false, message = "Failed to save booth" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, new
        //        {
        //            status = false,
        //            message = "An error occurred while processing the request.",
        //            error = ex.Message
        //        });
        //    }
        //}


        #endregion


        #region PravasiVoters Nazif
        [HttpPost]
        [Route("api/admin/addPravasiVoter")]
        public IHttpActionResult AddPravasiVoter()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                int idValue = 0;
                int.TryParse(httpRequest.Form["id"], out idValue);

                PravasiVoter data = new PravasiVoter
                {
                    id = idValue,
                    boothno = Convert.ToInt32(httpRequest.Form["boothno"]),
                    name = httpRequest.Form["name"],
                    Category = Convert.ToInt32(httpRequest.Form["Category"]),
                    Caste = Convert.ToInt32(httpRequest.Form["Caste"]),
                    mobile = httpRequest.Form["mobile"],
                    currentAddress = httpRequest.Form["currentAddress"],
                    Occupation =httpRequest.Form["Occupation"],
                    VillageListId = httpRequest.Form["VillageListId"]
                };
                bool success = _adminService.AddPravasiVoters(data);

                if (success)
                {
                    string message = data.id > 0 ? "Pravasi voter updated successfully." : "Pravasi voter added successfully.";
                    return Content(HttpStatusCode.OK, new
                    {
                        status = 200,
                        message = message
                    });
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, new
                    {
                        status = 400,
                        message = "Failed to save pravasi voter."
                    });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = 404,
                    message = "An error occurred.",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("api/admin/deletePravasiVoter")]
        public IHttpActionResult DeletePravasiVoter(int id)
        {
            try
            {
                bool success = _adminService.DeletePravasiVoter(id);

                if (success)
                {
                    return Content(HttpStatusCode.OK, new
                    {
                        status = 200,
                        message = $"Pravasi voter  deleted successfully."
                    });
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = 404,
                        message = $"Some error occured."
                    });
                }
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = 404,
                    message = "An error occurred while deleting the voter.",
                    error = ex.Message
                });
            }
        }

        #endregion


        #region Reports Nazif
        [HttpGet]
        [Route("api/admin/combinedReport")]
        public IHttpActionResult CombinedReport([FromUri] FilterModel filter, int? limit = null, int? page = null)
        {

            filter = filter ?? new FilterModel();
            var result = _adminService.GetCombinedReport(filter,limit,page);
            try
            {

                if (result == null)
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "Data Not Found",

                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Data Reterived Successfully",
                        data = result

                    });
                }
            }

            catch (SqlException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "Database error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
            }

        }

        [HttpGet]
        [Route("api/admin/pravasiVoterReport")]

        public IHttpActionResult PravasiVoterReport([FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            var result = _adminService.GetPravasiVoterReport(filter);
            try
            {

                if (result == null)
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "Not Data Found"

                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Data Retrieved successfully",
                        data = result
                    });
                }

            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "Database error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
            }
        }


        [HttpGet]
        [Route("api/admin/sectorReportWithBooth")]

        public IHttpActionResult SectorReportWithBooth([FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            var sectors = _adminService.GetAll_SectorDetails(filter);
            if (sectors == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "No Sector Available"
                });

            }

            var enrichedSectors = sectors.Select(sector => new
            {
                Sector = sector,
                Booths = _adminService.GetBoothslistbySectorId(sector.Id)
            }).ToList();

            try
            {
                if (enrichedSectors == null)
                {
                    return Content(HttpStatusCode.NotFound, new
                    {
                        status = false,
                        message = "No Sector Available"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = true,
                        message = "SectorReport retreived succesfully",
                        data = enrichedSectors

                    });
                }
            }
            catch (SqlException ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "Database error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new
                {
                    status = false,
                    message = "An unexpected error occurred.",
                    error = ex.Message
                });
            }

        }

        [HttpGet]
        [Route("api/admin/boothReport")]
        public IHttpActionResult BoothRport([FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var result = _adminService.GetBoothReport(filter,limit,page);
            if (result == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "Data Not Found"
                });
            }
            else
            {
                return Ok(new
                {
                    status = true,
                    message = "Data retrieve successfully",
                    data = result
                });
            }
        }
        [HttpGet]
        [Route("api/admin/doubleVoterReport")]
        public IHttpActionResult DoubleVoterReport([FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter??new FilterModel();
            var result = _adminService.GetDoubleVoterReport(filter,limit,page);
            if (result == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "Data Not Found"
                });
            }
            else
            {
                return Ok(new
                {
                    status = true,
                    message = "Data retrieve successfully",
                    data = result
                });
            }
        }

        [HttpGet]
        [Route("api/admin/seniorCitizenReport")]
        public IHttpActionResult SeniorCitizenReport([FromUri] FilterModel filter,
int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var result = _adminService.GetSeniorCitizenReport(filter, limit, page);
            if (result == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "Data Not Found"
                });
            }
            else
            {
                return Ok(new
                {
                    status = true,
                    message = "Data retrieve successfully",
                    data = result
                });
            }
        }
        [HttpGet]
        [Route("api/admin/handicapedReport")]
        public IHttpActionResult HandicapedReport([FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            var result = _adminService.GetHandicapReport(filter);
            if (result == null)
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "Data Not Found"
                });
            }
            else
            {
                return Ok(new
                {
                    status = true,
                    message = "Data retrieve successfully",
                    data = result
                });
            }
        }


        #endregion

        #region block nazif
        [HttpGet]
        [Route("api/admin/getAllBlocks")]
        public IHttpActionResult GetAllBlocks([FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();

            var data = _adminService.GetAllBlocks(filter,limit,page);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    message = "Successfully retrieve data",
                    Blocks = data,
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    Message = "No Data Found!!",
                });
            }
        }
        [HttpGet]
        [Route("api/admin/getBlockById")]
        public IHttpActionResult GetBlockById(int id)
        {
            var data = _adminService.GetBlockById(id);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    message = "Successfully retrieve data",
                    Blocks = data,
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    Message = "No Data Found!!",
                });
            }
        }
        [HttpGet]
        [Route("api/admin/deleteBlockById")]
        public IHttpActionResult DeleteBlockById(int id)
        {
            bool data = _adminService.DeleteBlock(id);
            if (data)
            {
                return Ok(new
                {
                    status = true,
                    message = "Successfully delete data",
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    Message = "No Data Found!!",
                });
            }
        }
        [HttpPost]
        [Route("api/admin/addOrUpdateBlock")]
        public async Task<IHttpActionResult> AddOrUpdateBlock()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                // Read Form fields
                int.TryParse(httpRequest.Form["Block_Id"], out int blockId);

                VishanSabha.Models.Block model = new VishanSabha.Models.Block
                {
                    Block_Id = blockId,
                    BlockName = httpRequest.Form["BlockName"],
                    InchargeName = httpRequest.Form["InchargeName"],
                    Contact = httpRequest.Form["Contact"],
                    Address = httpRequest.Form["Address"],
                    Occupation = httpRequest.Form["Occupation"],
                    party = string.IsNullOrEmpty(httpRequest.Form["party"]) ? (int?)null : int.Parse(httpRequest.Form["party"]),
                    Category = string.IsNullOrEmpty(httpRequest.Form["Category"]) ? (int?)null : int.Parse(httpRequest.Form["Category"]),
                    Caste = string.IsNullOrEmpty(httpRequest.Form["Caste"]) ? (int?)null : int.Parse(httpRequest.Form["Caste"]),
                };

                // Fetch existing block for update
                string oldImagePath = null;
                if (model.Block_Id > 0)
                {
                    var existing = _adminService.GetBlockById(model.Block_Id);
                    if (existing == null)
                        return Json(new { success = false, message = "Block not found." });

                    oldImagePath = existing.ProfileImage;
                }

                // Handle Profile Image
                var postedFile = httpRequest.Files["ProfileImage"];
                if (postedFile != null && postedFile.ContentLength > 0)
                {
                    // Delete old image
                    if (!string.IsNullOrEmpty(oldImagePath))
                    {
                        var oldFullPath = HttpContext.Current.Server.MapPath(oldImagePath);
                        if (File.Exists(oldFullPath))
                            File.Delete(oldFullPath);
                    }

                    string uploadFolder = HttpContext.Current.Server.MapPath("~/UploadedImages/");
                    if (!Directory.Exists(uploadFolder))
                        Directory.CreateDirectory(uploadFolder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(postedFile.FileName);
                    string filePath = Path.Combine(uploadFolder, fileName);
                    postedFile.SaveAs(filePath);

                    model.ProfileImage = "/UploadedImages/" + fileName;
                }
                else
                {
                    model.ProfileImage = oldImagePath; // retain existing image
                }

                // Save to DB
                int resultId = _adminService.SaveBlock(model);

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


        #endregion

        #region Bdc nazif
        [HttpGet]
        [Route("api/admin/getAllBdc")]
        public IHttpActionResult GetAllBdc(int? limit = null, int? page = null)
        {
            var data = _adminService.GetBDC(limit,page);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    message = "Successfully retrieve data",
                    Blocks = data,
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    Message = "No Data Found!!",
                });
            }
        }
        [HttpGet]
        [Route("api/admin/deleteBdcById")]
        public IHttpActionResult DeleteBdcById(int id)
        {
            bool data = _adminService.DeleteBDC(id);
            if (data)
            {
                return Ok(new
                {
                    status = true,
                    message = "Successfully delete data",
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    Message = "No Data Found!!",
                });
            }
        }
        #endregion


        #region Activity 

        [HttpPost]
        [Route("api/admin/addActivity")]
        public IHttpActionResult AddActivity()
        {
            try
            {
                var form = HttpContext.Current.Request;


                if (form == null ||
                    string.IsNullOrEmpty(form["Title"]) ||
                    string.IsNullOrEmpty(form["Description"]) ||
                    string.IsNullOrEmpty(form["ActivityDate"]))
                {
                    return Ok(new
                    {
                        status = false,
                        Message = "All Fields are required!!"
                    });
                }


                Activities model = new Activities();

                model.ActivityId = string.IsNullOrEmpty(form["ActivityId"]) ? 0 : Convert.ToInt32(form["ActivityId"]);
                model.Title = form["Title"];
                model.Description = form["Description"];
                model.ActivityDate = Convert.ToDateTime(form["ActivityDate"]);
                model.VideoUrl = form["VideoUrl"];



                List<string> currentImages = new List<string>();
                if (model.ActivityId > 0)
                {
                    var existing = _adminService.GetActivityById(model.ActivityId);
                    if (existing == null)
                        return Ok(new { status = false, Message = "Activity not found." });

                    currentImages = existing.ImagePaths ?? new List<string>();

                    var imagesToDeleteJson = form["ImagesToDeleteJson"];
                    if (!string.IsNullOrEmpty(imagesToDeleteJson))
                    {
                        var toDelete = JsonConvert.DeserializeObject<List<string>>(imagesToDeleteJson);
                        foreach (var img in toDelete)
                        {
                            string fullPath = HttpContext.Current.Server.MapPath(img);
                            if (System.IO.File.Exists(fullPath))
                                System.IO.File.Delete(fullPath);

                            currentImages.Remove(img);
                        }
                    }
                }



                var videoFile = HttpContext.Current.Request.Files["VideoFile"];
                if (videoFile != null && videoFile.ContentLength > 0)
                {
                    const int maxVideoSizeBytes = 200 * 1024 * 1024; // 200 MB
                    if (videoFile.ContentLength > maxVideoSizeBytes)
                    {
                        return Ok(new { status = false, Message = "Video size should not exceed 200 MB." });
                    }

                    string videoFolder = HttpContext.Current.Server.MapPath("~/UploadedVideos/");
                    if (!Directory.Exists(videoFolder))
                        Directory.CreateDirectory(videoFolder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(videoFile.FileName);
                    string videoPath = Path.Combine(videoFolder, fileName);
                    videoFile.SaveAs(videoPath);


                    model.VideoUrl = "/UploadedVideos/" + fileName;
                }



                List<string> allowedImageExtensions = new List<string> { ".jpeg", ".jpg", ".png" };
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    string uploadPath = HttpContext.Current.Server.MapPath("~/UploadedImages/");
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);

                    for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
                    {
                        var img = HttpContext.Current.Request.Files[i];


                        if (img == videoFile) continue;

                        if (img != null && img.ContentLength > 0)
                        {
                            string ext = Path.GetExtension(img.FileName).ToLower();
                            if (!allowedImageExtensions.Contains(ext))
                            {
                                return Ok(new
                                {
                                    status = false,
                                    Message = $"Only JPEG, JPG, and PNG images are allowed. Invalid file: {img.FileName}"
                                });
                            }

                            string fileName = Guid.NewGuid() + ext;
                            string filePath = Path.Combine(uploadPath, fileName);
                            img.SaveAs(filePath);

                            currentImages.Add("/UploadedImages/" + fileName);
                        }
                    }
                }

                model.ImagePaths = currentImages;
                model.Status = true;

                bool result = _adminService.AddOrUpdateActivity(model);

                return Ok(new
                {
                    status = result,
                    Message = model.ActivityId > 0 ? "Activity updated successfully." : "Activity added successfully."
                });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, Message = "Error: " + ex.Message });
            }
        }
      [HttpGet]
[Route("api/admin/GetCasteByBoothVoterDesId")]
public IHttpActionResult CastByBoothVoterDesId(int BoothVoterDesId)
{
    var data = _adminService.GetCastVotersDesById(BoothVoterDesId);

    if (data != null && data.Any())
    {
        // flatten into a single array
        var flatList = data.SelectMany(x => x.CastName.Select((cast, index) => new
        {
            Id = x.Id,
            VoterDes = x.VoterDes,
            CastName = cast,
            Number = x.Number.ElementAtOrDefault(index),
            castNameId = x.castNameId.ElementAtOrDefault(index)
        })).ToList();

        return Ok(new
        {
            status = true,
            message = "Caste List By Booth Voter Des. Id Found Successfully",
            CastListByBoothVoterDes = flatList
        });
    }
    else
    {
        return Ok(new
        {
            StatusCode = 404,
            status = false,
            message = "No Data Found!!"
        });
    }
}

        [HttpGet]
        [Route("api/admin/GetAllActivity")]
        public IHttpActionResult GetAllActivity(int? limit = null, int? page = null)
        {
            var data = _adminService.GetAllActivities(limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Activity All Data Found!!",
                    ActivityData = data
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = 404,
                    status = false,
                    Message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/DeleteActivityById")]
        public IHttpActionResult DeleteActivityById(int ActivityId)
        {
            var res = _adminService.DeleteActivity(ActivityId);
            if (res)
            {
                return Ok(new
                {
                    status = true,
                    message = "Activity Deleted Successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = 404,
                    status = false,
                    Message = "Some Error Occured!!"
                });
            }
        }

        #endregion


        #region excelPdfReport api

        [HttpGet]
        [Route("api/admin/combinedmandalReportForExcelPdf")]
        public IHttpActionResult CombinedMandalExportReport(string format, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = _adminService.GetCombinedReport(filter);

            if (reportData == null || !reportData.Any())
                return Content(HttpStatusCode.NotFound, new { Status = 200, Message = "No data to generate report." });

            var headers = new string[]
            {
            "Booth No.", "Mandal", "Polling Station", "Sector Name", "Sector Sanyojak", "Booth Adhyaksh",
            "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education"
            };

            Func<Booth, List<string>> mapFunc = booth => new List<string>
        {
            booth.BoothNumber?.ToString() ?? "",
            booth.MandalName ?? "",
            booth.BoothName ?? "",
            booth.SectorName ?? "",
            $"{booth.SectorIncName} {booth.SectorIncPhone}".Trim(),
            booth.InchargeName ?? "",
            booth.PhoneNumber ?? "",
            string.Join(", ", booth.VillageNames ?? new List<string>()),
            booth.FatherName ?? "",
            booth.Age.ToString(),
            booth.SubCasteName ?? "",
            booth.Address ?? "",
            booth.Education ?? ""
        };

            var generator = new ReportGenerator<Booth>();

            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format.ToLower() == "pdf")
            {
                fileBytes = generator.ExportToPdf(reportData.ToList(), headers, "Combined Mandal Reports", mapFunc);
                contentType = "application/pdf";
                fileName = "CombinedMandalExportReport.pdf";
            }
            else if (format.ToLower() == "excel")
            {
                fileBytes = generator.ExportToExcel(reportData.ToList(), headers, "Combined Mandal Reports", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "CombinedMandalExportReport.xlsx";
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new { Status = 404, Message = "Invalid format. Allowed values are pdf/excel." });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return ResponseMessage(result);
        }

        [HttpGet]
        [Route("api/export/CombinedSectorReport")]
        public IHttpActionResult CombinedSectorExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var sectors = _adminService.GetAll_SectorDetails(filter);
            var combinedList = new List<CombinedSectorBoothReport>();

            if (sectors == null || !sectors.Any())
                return Content(HttpStatusCode.NotFound, new { Status = 200, Message = "No data to generate report." });
            foreach (var sector in sectors)
            {
                var booths = _adminService.GetBoothslistbySectorId(sector.Id);

                foreach (var booth in booths)
                {
                    combinedList.Add(new CombinedSectorBoothReport
                    {
                        // Sector data
                        MandalName = sector.MandalName,
                        SectorName = sector.SectorName,
                        InchargeName = sector.InchargeName,
                        SectorPhoneNumber = sector.PhoneNumber,
                        SectorVillageNames = sector.VillageNames,
                        SectorFatherName = sector.FatherName,
                        SectorAge = sector.Age,
                        SectorCaste = sector.subcaste,
                        SectorAddress = sector.Address,
                        SectorEducation = sector.Education,
                        SectorProfileImage = sector.ProfileImage,

                        // Booth data
                        BoothNumber = booth.BoothNo,
                        BoothName = booth.BoothName,
                        BoothIncharge = booth.InchargeName,
                        BoothPhone = booth.Mobile,
                        BoothVillageNames = booth.VillageNames,
                        BoothFatherName = booth.FatherName,
                        BoothAge = booth.Age,
                        BoothCaste = booth.SubCasteName,
                        BoothAddress = booth.Address,
                        BoothEducation = booth.Education,
                        BoothProfileImage = booth.ProfileImage
                    });
                }
            }

            if (!combinedList.Any())
                return NotFound();

            var headers = new string[]
            {
            "S.N.", "Mandal", "Sector", "Sector Sanyojak", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education",
            "Booth No", "Polling Station", "Booth Adhyaksh", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education"
            };

            var indexedData = combinedList.Select((data, index) => (Index: index + 1, Data: data)).ToList();

            Func<(int Index, CombinedSectorBoothReport Data), List<string>> mapFunc = item => new List<string>
        {
            item.Index.ToString(),
            item.Data.MandalName ?? "",
            item.Data.SectorName ?? "",
            item.Data.InchargeName ?? "",
            item.Data.SectorPhoneNumber ?? "",
            string.Join(", ", item.Data.SectorVillageNames ?? new List<string>()),
            item.Data.SectorFatherName ?? "",
            item.Data.SectorAge?.ToString() ?? "",
            item.Data.SectorCaste ?? "",
            item.Data.SectorAddress ?? "",
            item.Data.SectorEducation ?? "",

            item.Data.BoothNumber ?? "",
            item.Data.BoothName ?? "",
            item.Data.BoothIncharge ?? "",
            item.Data.BoothPhone ?? "",
            string.Join(", ", item.Data.BoothVillageNames ?? new List<string>()),
            item.Data.BoothFatherName ?? "",
            item.Data.BoothAge?.ToString() ?? "",
            item.Data.BoothCaste ?? "",
            item.Data.BoothAddress ?? "",
            item.Data.BoothEducation ?? ""
        };

            var generator = new ReportGenerator<(int Index, CombinedSectorBoothReport Data)>();

            if (format.ToLower() == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Combined Sector Reports", mapFunc);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                    {
                        Headers =
                    {
                        ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf"),
                        ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "CombinedSectorExportReport.pdf"
                        }
                    }
                    }
                });
            }
            else if (format.ToLower() == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Combined Sector Reports", mapFunc);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                    {
                        Headers =
                    {
                        ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
                        ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = "CombinedSectorExportReport.xlsx"
                        }
                    }
                    }
                });
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new { Status = 404, Message = "Invalid format. Allowed values are pdf/excel." });
            }
        }





        [HttpGet]
        [Route("api/admin/MandalExportReport")]
        public IHttpActionResult MandalExportReport(string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = _adminService.GetMandalReport();

            if (reportData == null || !reportData.Any())
                return Content(HttpStatusCode.NotFound, new { Status = 200, Message = "No data to generate report." });

            var headers = new string[]
            {
        "S.N", "Mandal Name", "Total Sectors", "Total Booths", "Total Voters",
        "Senior Count", "Disabled Count", "Double Votes",
        "Pravasi Count", "Effective Person"
            };

            var indexedData = reportData.Select((data, index) => (Index: index + 1, Data: data)).ToList();

            Func<(int Index, Mandal Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.Name ?? "",
        item.Data.TotalSectors.ToString(),
        item.Data.TotalBooths.ToString(),
        item.Data.TotalVotes.ToString(),
        item.Data.SeniorCount.ToString(),
        item.Data.DisabledCount.ToString(),
        item.Data.DoubleVotes.ToString(),
        item.Data.PravasiCount.ToString(),
        item.Data.EffectivePerson.ToString()
    };

            var generator = new ReportGenerator<(int Index, Mandal Data)>();

            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == "pdf")
            {
                fileBytes = generator.ExportToPdf(indexedData, headers, "Mandal Report", mapFunc);
                contentType = "application/pdf";
                fileName = "MandalReport.pdf";
            }
            else if (format == "excel")
            {
                fileBytes = generator.ExportToExcel(indexedData, headers, "Mandal Report", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "MandalReport.xlsx";
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new { Status = 404, Message = "Invalid format. Allowed values are pdf/excel." });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return ResponseMessage(result);
        }

        [HttpGet]
        [Route("api/admin/PravasiVoterExportReport")]
        public IHttpActionResult PravasiVoterExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetPravasiVoterReport(filter);

            if (reportData == null || !reportData.Any())
                return Content(HttpStatusCode.NotFound, new { Status = 200, Message = "No data to generate report." });

            var headers = new string[]
            {
        "S.No", "Booth No.", "Booth Adhyaksh", "Pravasi Name", "Category",
        "Caste", "Contact", "Occupation", "Current Address", "Sector", "Sector Incharge"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PravasiVoter Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber ?? "",
        item.Data.BoothIncharge ?? "",
        item.Data.name ?? "",
        item.Data.CategoryName ?? "",
        item.Data.CasteName ?? "",
        item.Data.mobile ?? "",
        item.Data.Occupation ?? "",
        item.Data.currentAddress ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""
    };

            var generator = new ReportGenerator<(int Index, PravasiVoter Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Pravasi Voter Report", mapFunc);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                };
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "PravasiVoterReport.pdf"
                };
                return ResponseMessage(result);
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Pravasi Voter Report", mapFunc);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                };
                result.Content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "PravasiVoterReport.xlsx"
                };
                return ResponseMessage(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new { Status = 404, Message = "Invalid format. Allowed values are pdf/excel." });
            }
        }

        [HttpGet]
        [Route("api/admin/HandicapExportReport")]
        public IHttpActionResult HandicapExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetHandicapReport(filter);

            if (reportData == null || !reportData.Any())
                return Content(HttpStatusCode.NotFound, new { Status = 200, Message = "No data to generate report." });

            // Define column headers
            var headers = new string[]
          {
        "S.N.", "Booth No.", "Booth Adhyaksh", "Village", "Name", "Contact", "Handicap", "Caste", "Address", "Sector", "Sector Incharge"
          };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorOrDisabled Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNo.ToString(),
        item.Data.BoothIncharge ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.Name ?? "",
        item.Data.Mobile ?? "",
        item.Data.SeniorOrDisabledType ?? "",
        item.Data.SubCasteName ?? "",
        item.Data.Address ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""
    };
            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, SeniorOrDisabled Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if (format == "pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Handicap Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "HandicapReport.pdf";

            }
            else if (format == "excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Handicap Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "HandicapReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "Invalid format"
                });
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(FileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName

            };
            return ResponseMessage(result);

        }


        [HttpGet]
        [Route("api/admin/SeniorCitizenExportReport")]
        public IHttpActionResult SeniorCitizenExportReport(string format, [FromUri] FilterModel filter,
int? limit = null, int? page = null)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetSeniorCitizenReport(filter,limit,page);

            if (reportData == null || !reportData.Any())
                return Content(HttpStatusCode.NotFound, new { Status = 200, Message = "No data to generate report." });

            // Define column headers
            var headers = new string[]
            {
        "S.N.","Booth No.","Booth Adhyaksh","Village"," Name","Contact","SeniorCitizen","Caste", "Address","Sector","Sector Sanyojak",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, SeniorOrDisabled Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNo.ToString(),
        item.Data.BoothIncharge ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.Name ?? "",

        item.Data.Contact ?? "",
        item.Data.SeniorOrDisabledType .ToString(),
        item.Data.CasteName ?? "",
        item.Data.Address ?? "",

        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? "",

    };
            var generator = new ReportGenerator<(int Index, SeniorOrDisabled Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Varishth Nagrik Reports", mapFunc);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "SeniorCitizenReport.pdf"
                    }
                }
                    },
                    ReasonPhrase = "PDF generated successfully"
                });
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Senior Citizen Report", mapFunc);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "SeniorCitizenReport.xlsx"
                    }
                }
                    },
                    ReasonPhrase = "Excel generated successfully"
                });
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new { Status = 404, Message = "Invalid format. Allowed values are pdf/excel." });
            }
        }


        [HttpGet]
        [Route("api/admin/EffectivePersonExportReport")]
        public IHttpActionResult EffectivePersonExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetEffectivePersonReport(filter);

            if (reportData == null || !reportData.Any())
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    Status = "Error",
                    Message = "No data to generate report."
                });
            }

            // Define column headers
            var headers = new string[]
            {
        "S.N.", "Booth No.", "Booth Adhyaksh", "Village","Designation", "Name", "Contact","Category", "Caste", "Description", "Sector", "Sector Incharge"
            };

            // Create indexed data for serial numbers
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, EffectivePerson Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber.ToString() ?? "",
        item.Data.BoothIncharge ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.Designation ?? "",
        item.Data.Name ?? "",
        item.Data.Mobile ?? "",
        item.Data.CategoryName ?? "",
        item.Data.Castename ?? "",
        item.Data.Description ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""
    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, EffectivePerson Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Effective Person Report", mapFunc);

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "EffectivePersongExportReport.pdf"
                    }
                }
                    },
                    ReasonPhrase = "Report generated successfully"
                });
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Effective Person Report", mapFunc);

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "EffectivePersongExportReport.xlsx"
                    }
                }
                    },
                    ReasonPhrase = "Report generated successfully"
                });
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    Status = "Error",
                    Message = "Invalid format. Only 'pdf' or 'excel' allowed."
                });
            }
        }


        [HttpGet]
        [Route("api/admin/SectorExportReport")]
        public IHttpActionResult SectorExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetAll_SectorDetails(filter);

            if (reportData == null || !reportData.Any())
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    Status = "Error",
                    Message = "No data to generate report."
                });
            }

            // Define column headers
            var headers = new string[]
            {
        "S.N.", "Sector", "Sector Sanyojak", "Contact","Caste", "Village",
        "Total Booths","Total Voters", "Senior Citizen", "Handicap",
        "Double Votes","Pravasi"
            };

            // Create indexed data for serial numbers
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, Sector Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.SectorName ?? "",
        item.Data.InchargeName ?? "",
        item.Data.PhoneNumber ?? "",
        item.Data.SubCasteName ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.TotalBooth.ToString(),
        item.Data.TotalVotes.ToString(),
        item.Data.SeniorCitizen.ToString(),
        item.Data.Handicap.ToString(),
        item.Data.DoubleVotes.ToString(),
        item.Data.Pravasi.ToString()
    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, Sector Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Sector Report", mapFunc);

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "SectorReport.pdf"
                    }
                }
                    },
                    ReasonPhrase = "Sector PDF report generated successfully"
                });
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Sector Report", mapFunc);

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "SectorReport.xlsx"
                    }
                }
                    },
                    ReasonPhrase = "Sector Excel report generated successfully"
                });
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    Status = "Error",
                    Message = "Invalid format. Only 'pdf' or 'excel' allowed."
                });
            }
        }

        [HttpGet]
        [Route("api/admin/BoothExportReport")]
        public IHttpActionResult BoothExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetBoothReport(filter);

            if (reportData == null || !reportData.Any())
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    Status = "Error",
                    Message = "No data to generate report."
                });
            }

            // Define column headers
            var headers = new string[]
            {
        "Booth No.", "Polling Station", "Booth Adhyaksh", "Contact",
        "Village", "Caste", "Total Voters", "Senior Citizen",
        "Handicap", "Double Votes","Pravasi"
            };

            // Mapping function
            Func<Booth, List<string>> mapFunc = item => new List<string>
    {
        item.BoothNo.ToString(),
        item.BoothName ?? "",
        item.InchargeName ?? "",
        item.Mobile ?? "",
        string.Join(", ", item.VillageNames ?? new List<string>()),
        item.castname ?? "",
        item.TotalVotes.ToString(),
        item.SeniorCitizen.ToString(),
        item.Handicap.ToString(),
        item.DoubleVotes.ToString(),
        item.Pravasi.ToString()
    };

            var generator = new ReportGenerator<Booth>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(reportData.ToList(), headers, "Booth Report", mapFunc);

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "BoothReport.pdf"
                    }
                }
                    },
                    ReasonPhrase = "Booth PDF report generated successfully"
                });
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(reportData.ToList(), headers, "Booth Report", mapFunc);

                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "BoothReport.xlsx"
                    }
                }
                    },
                    ReasonPhrase = "Booth Excel report generated successfully"
                });
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    Status = "Error",
                    Message = "Invalid format. Only 'pdf' or 'excel' allowed."
                });
            }
        }
        [HttpGet]
        [Route("api/admin/DoubleVoterExportReport")]
        public IHttpActionResult DoubleVoterExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetDoubleVoterReport(filter);

            if (reportData == null || !reportData.Any())
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = "error",
                    message = "No data to generate report."
                });
            }

            var headers = new string[]
            {
        "S.N.", "Booth No.", "Booth Adhyaksh", "Vilage", "Name", "Father Name",
        "Voter Id", "Current Address", "Previous Address", "Reason", "Sector", "Sector Sanyojak"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, doubleVoter Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber.ToString() ?? "",
        item.Data.BoothIncharge ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.name ?? "",
        item.Data.fathername ?? "",
        item.Data.voterno?.ToString() ?? "",
        item.Data.currentAddress ?? "",
        item.Data.pastAddress ?? "",
        item.Data.reason ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""
    };

            var generator = new ReportGenerator<(int Index, doubleVoter Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Double Voter Report", mapFunc);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "DoubleVoterReport.pdf"
                    }
                }
                    },
                    ReasonPhrase = "Report generated successfully"
                });
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Double Voter Report", mapFunc);
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                    {
                        Headers =
                {
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"),
                    ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = "DoubleVoterReport.xlsx"
                    }
                }
                    },
                    ReasonPhrase = "Report generated successfully"
                });
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    status = "error",
                    message = "Invalid format. Use 'pdf' or 'excel'."
                });
            }
        }

        [HttpGet]
        [Route("api/admin/NewVoteraExportReport")]
        public IHttpActionResult NewVoteraExportReport(string format, [FromUri] FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            filter = filter ?? new FilterModel();
            var reportData = _adminService.GetNewVoterReport(filter);

            if (reportData == null || !reportData.Any())
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    Status = "Error",
                    Message = "No data to generate report."
                });
            }

            // Define column headers
            var headers = new string[]
            {
        "S.N.","Booth No.","Booth Adhyaksh","Vilage","Voter Name","Father Name","Contact","Category","Caste","DOB","Age Till(1-jan-2027)","Education","Sector","Sector Sanyojak",
            };

            // Create indexed data for serial numbers
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Mapping function
            Func<(int Index, NewVoters Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNo.ToString(),
        item.Data.BoothIncharge ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.VoterName ?? "",
        item.Data.FatherName ?? "",
        item.Data.MobileNumber?.ToString() ?? "",
        item.Data.Category ?? "",
        item.Data.caste ?? "",
        item.Data.dateofbirth.ToString("dd-MM-yyyy"),
        item.Data.totalage ?? "",
        item.Data.Education ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""
    };

            var generator = new ReportGenerator<(int Index, NewVoters Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "New Voters Report", mapFunc);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(pdfBytes)
                };
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "NewVotersReport.pdf"
                };
                return ResponseMessage(result);
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "New Voters Report", mapFunc);
                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(excelBytes)
                };
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "NewVotersReport.xlsx"
                };
                return ResponseMessage(result);
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    Status = "Error",
                    Message = "Invalid format. Allowed formats are pdf or excel."
                });
            }
        }

        #endregion

        [HttpPost]
        [Route("api/admin/GetLocationForLogin")]
        public async Task<IHttpActionResult> GetLocationForLogin()
        {
            try
            {
                // Form-data read
                var request = HttpContext.Current.Request;

                string latitude = request.Form["latitude"];
                string longitude = request.Form["longitude"];
                string userIdStr = request.Form["userId"];

                if (string.IsNullOrEmpty(latitude) ||
                    string.IsNullOrEmpty(longitude) ||
                    string.IsNullOrEmpty(userIdStr) ||
                    !int.TryParse(userIdStr, out int userId) ||
                    userId <= 0)
                {
                    return BadRequest("Latitude, Longitude and UserId are required.");
                }

                // Call service
                var location = await _authService.GetLocation(latitude, longitude, userId);

                return Ok(new
                {
                    Success = true,
                    Message = "Location saved successfully.",
                    Location = location
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("api/admin/getAllLocation")]
        public IHttpActionResult getAllLocation()
        {
            var data = _authService.getAllLocation();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    messAGE = "Get All Location!!",
                    Location = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    messAGE = "No Data Found!!",

                });
            }
        }
        [HttpGet]
        [Route("api/admin/getAllNewVoterReport")]
        public IHttpActionResult getAllNewVoter([FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _adminService.GetNewVoterReport(filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    messAGE = "Get All New Voters!!",
                    NewVoters = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    messAGE = "No Data Found!!",

                });
            }
        }




        #region AdminDashboardList api
        [HttpGet]
        [Route("api/admin/GetSahmatList")]
        public IHttpActionResult GetSahmatList( int? limit = null, int? page = null)
        {
            var allData = _adminService.getAllSahmatData(limit,page);
            //var satisfiedOnly = allData.Where(x => x.sahmatAsahmat == type).ToList();

            if (allData != null && allData.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get Successfully!!",
                    SahmatVoterList = allData
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetAsahmatList")]
        public IHttpActionResult GetAsahmatList(int type=2 ,int? limit = null, int? page = null)
        {
            try
            {
                // Get all Sahmat/Asahmat data
                var allData = _adminService.getAllASahmatData(limit, page);

                // Filter only Asahmat data (case-insensitive check)
                //var asahmatList = allData.Where(x => x.sahmatAsahmat == type).ToList();

                if (allData != null && allData.Any())
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Data fetched successfully!",
                        AsahmatVoterList = allData
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        message = "No data found!"
                    });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllPradhanList")]
        public IHttpActionResult GetAllEffectivePersons(string type = "Pradhan")
        {
            var model = _adminService.GetAllEffectivePersons();

            var data = model.Where(x => x.Designationdata == type).ToList();

            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    PradhanList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllBlocksList")]
        public IHttpActionResult GetAllBlocksList([FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();

            var data = _adminService.GetAllBlocks(filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    AllBlockList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetAlldoctorList")]
        public IHttpActionResult GetAllDoctorList(int? limit = null, int? page = null)
        {
            var data = _adminService.GetAllDoctorList(limit,page);

            //var data = model.Where(x => x.Designationdata == type).ToList();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    AllDoctorList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data Found!!"
                });
            }

        }

        [HttpGet]
        [Route("api/admin/GetAllAdvocateList")]
        public IHttpActionResult GetAllAdvocateList( int? limit = null, int? page = null)
        {
            var data = _adminService.GetAllAdvocateList(limit,page);

            //var data = model?
            //    .Where(x => x.DesignationId == type)
            //    .ToList();

            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    AllAdvocateList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data Found!!"
                });
            }
        }


        [HttpGet]
        [Route("api/admin/GetAllBDCList")]
        public IHttpActionResult GetAllBDCList(string type = "BDC Member")
        {
            var model = _adminService.GetAllEffectivePersons();

            var data = model.Where(x => x.Designationdata == type).ToList();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    AllBDCMemberList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data Found!!"
                });
            }

        }
        [HttpGet]
        [Route("api/admin/GetAllGovtEmpList")]
        public IHttpActionResult GetAllGovtEmpList( int? limit = null, int? page = null)
        {
            var data = _adminService.GetAllGovtEmpList(limit,page);

            //var data = model.Where(x => x.Designationdata == type).ToList();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    GovtEmpList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
       
        [HttpGet]
        [Route("api/admin/GetAllDisabledList")]
        public IHttpActionResult GetAllDisabledList( int? limit = null, int? page = null)
        {
            var seniorordisabled = _adminService.GetAllDisabledList(limit,page);

            //var Disabled = seniorordisabled.Where(x => x.SeniorOrDisabledType == type).ToList();
            if (seniorordisabled != null)
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    DisabledList = seniorordisabled
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllSeniorCitizenList")]
        public IHttpActionResult GetAllSeniorList( int? limit = null, int? page = null)
        {
            var seniorordisabled = _adminService.GetAllSeniorList(limit,page);

            //var Disabled = seniorordisabled.Where(x => x.SeniorOrDisabledType == type).ToList();
            if (seniorordisabled != null)
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SeniorList = seniorordisabled
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data Found!!"
                });
            }
        }

        [HttpPost]
        [Route("api/admin/AddOrUpdateBDC")]
        public IHttpActionResult AddOrUpdateBDC()
        {
            try
            {
                var request = HttpContext.Current.Request;
                if (request == null)
                    return Ok(new { status = false, StatusCode = 400, message = "Invalid request!" });

                // Create model object
                BDC bdc = new BDC();

                // Map form values
                bdc.BDC_Id = string.IsNullOrWhiteSpace(request.Form.Get("BDC_Id")) ? 0 : Convert.ToInt32(request.Form.Get("BDC_Id"));
                bdc.Block_Id = string.IsNullOrWhiteSpace(request.Form.Get("Block_Id")) ? 0 : Convert.ToInt32(request.Form.Get("Block_Id"));

                bdc.Name = request.Form.Get("Name");
                bdc.villageId = string.IsNullOrWhiteSpace(request.Form.Get("villageId")) ? null :request.Form.Get("villageId").ToString();


                bdc.Category = string.IsNullOrWhiteSpace(request.Form.Get("Category")) ? (int?)null : Convert.ToInt32(request.Form.Get("Category"));


                bdc.Caste = string.IsNullOrWhiteSpace(request.Form.Get("Caste")) ? (int?)null : Convert.ToInt32(request.Form.Get("Caste"));
                bdc.party = int.TryParse(request.Form.Get("party"), out int caste) ? caste : 0;



                bdc.Age = string.IsNullOrWhiteSpace(request.Form.Get("Age")) ? 0 : Convert.ToInt32(request.Form.Get("Age"));
                bdc.Contact = request.Form.Get("Contact");
                bdc.Education = request.Form.Get("Education");
                bdc.WardNumber = string.IsNullOrWhiteSpace(request.Form.Get("WardNumber")) ? 0 : Convert.ToInt32(request.Form.Get("WardNumber"));

                // Handle Profile Image upload
                HttpPostedFile file = request.Files["ProfileImage"];
                if (file != null && file.ContentLength > 0)
                {
                    string folderPath = HttpContext.Current.Server.MapPath("~/UploadedImages/BDC/");
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(folderPath, fileName);

                    file.SaveAs(filePath);
                    bdc.ProfileImage = "/UploadedImages/BDC/" + fileName;
                }
                else
                {

                    if (bdc.BDC_Id > 0)
                    {
                        var existing = _adminService.GetBDC(bdc.BDC_Id).FirstOrDefault();
                        if (existing != null)
                            bdc.ProfileImage = existing.ProfileImage;
                    }
                }


                _adminService.SaveBDC(bdc);


                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = bdc.BDC_Id > 0 ? "BDC  updated successfully!" : "BDC  added successfully!"
                });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, StatusCode = 500, message = "Error: " + ex.Message });
            }
        }
        [HttpGet]
        [Route("api/admin/GetBDCById")]
        public IHttpActionResult GetBDCById(int BDCId)
        {
            var data = _adminService.GetBDC(BDCId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data Get Successfully!!",
                    BDCById = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }


        [HttpGet]
        [Route("api/admin/GetBlockCount")]
        public IHttpActionResult GetBlockCount()
        {
            int count = _adminService.GetAllBlocksCounts();
            if (count != 0 && count > 0)
            {
                return Ok(new
                {
                    status = true,
                    message = "Count get successfully!!",
                    BlockCount = count
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }

        }
        [HttpGet]
        [Route("api/admin/GetBDCCount")]
        public IHttpActionResult GetBDCCount()
        {
            int count = _adminService.GetBDCCount();
            if (count != 0 && count > 0)
            {
                return Ok(new
                {
                    status = true,
                    message = "BDC Count get successfully!!",
                    BDCCount = count
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetDesignationsForBoothSamiti")]
        public IHttpActionResult GetDesignations()
        {
            var data = _adminService.GetDesignations();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    DesignationList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/getOccupation")]
        public IHttpActionResult getOccupation()
        {
            var data = _adminService.getOccupation();
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    occupationList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }
 
        //[HttpGet]
        //[Route("api/admin/GetEffectiveDesignations")]
        //public IHttpActionResult GetEffectiveDesignations()
        //{
        //    var data = _adminService.GetEffectiveDesignations();
        //    if (data != null && data.Any())
        //    {
        //        return Ok(new
        //        {
        //            status = true,
        //            message = "Data get successfully!!",
        //            EffectivePersonDesignation = data
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status = false,
        //            message = "No data found!!"
        //        });
        //    }
        //}
        [HttpGet]
        [Route("api/admin/GetMandalReport")]
        public IHttpActionResult GetMandalReport(int? limit = null, int? page = null)
        {
            var data = _adminService.GetMandalReport(limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    MandalReport = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetSectorsDetailsByMandalId")]
        public IHttpActionResult GetSectorsByMandalId(string mandalId)
        {
            var data = _adminService.GetSectorsByMandalId(mandalId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SectorDetailForMandalReport = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetBoothslistbySectorId")]
        public IHttpActionResult GetBoothslistbySectorId(int sectorId)
        {
            var data = _adminService.GetBoothslistbySectorId(sectorId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    BoothDetailsForMandalReport = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    Message = "No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetSecInchargeProfileBySecId")]
        public IHttpActionResult GetSecInchargeProfileBySecId(int sectorId)
        {
            var data = _adminService.GetSecInchargeBySecId(sectorId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SectorInchargeProfile = data

                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetBoothInchargeProfileByBoothId")]
        public IHttpActionResult GetBoothInchargeByBoothId(int BoothId)
        {
            var data = _adminService.GetBoothInchargeByBoothId(BoothId);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Data get successfully!!",
                    BoothInchargeProfile=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }


        #endregion
        [HttpPost]
        [Route("api/admin/AddCastVoterByBoothVoterDesId")]
        public IHttpActionResult AddCastVoter()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

           
                string voterDesStr = httpRequest.Form["VoterDes"];

          
                var numbers = httpRequest.Form.GetValues("Number")?.Select(int.Parse).ToList() ?? new List<int>();
                var castNameIds = httpRequest.Form.GetValues("castNameId")?.ToList() ?? new List<string>();

                int voterDes = 0;
                int.TryParse(voterDesStr, out voterDes);

                
                CastVotersDes model = new CastVotersDes
                {
                    VoterDes = voterDes,
                    Number = numbers,
                    CastName = castNameIds
                };

                bool isSaved = _adminService.AddCastVotersDes(model);

                return Ok(new
                {
                    status = isSaved,
                    StatusCode = isSaved ? 200 : 400,
                    message = isSaved ? "Caste voters added successfully!" : "Voter limit exceeded or failed to add caste voters!"
                });

            }
            catch (Exception ex)
            {
                return Ok(new { status = false, StatusCode = 500, message = "Error: " + ex.Message });
            }
        }


        [HttpGet]
        [Route("api/admin/DeleteCastVoterById")]
        public IHttpActionResult DeleteCastVoterbyId(int casteVoterId)
        {
            var res = _adminService.DeleteCastVoter(casteVoterId);
            if(res)
            {
                return Ok(new
                {
                    status=true,
                    message="Caste Voter Deleted Successfully!!"
                });
            }else
            {
                return Ok(new
                {
                    status=false,
                    message="Some error occured!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetCastListById")]
        public IHttpActionResult GetCastListById(int CastVoterId)
        {
            var data = _adminService.GetCastListById(CastVoterId);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Caste voter data get successfully!!",
                    CastVoterDataById=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="some error occured!!"
                });
            }
        }

        [HttpPost]
        [Route("api/admin/UpdateCasteVoterDesById")]
        public IHttpActionResult UpdateCasteVoterDes(int id, int castId, int number)
        {
            var res = _adminService.UpdateCastVoter(id, castId, number);
            if(res)
            {
                return Ok(new
                {
                    status=true,
                    message="Caste voter updated successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Some error occured!!"
                });
            }
        }
        [HttpPost]
        [Route("api/admin/SeniorOrDisabledAddandUpdate")]
        public IHttpActionResult SeniorOrDisabledAddandUpdate()
        {
            try
            {
                var form = HttpContext.Current.Request;
                var data = new SeniorOrDisabled();

                // Parse values safely
                data.Id = int.TryParse(form.Form["Id"], out int id) ? id : 0;
                data.boothId = int.TryParse(form.Form["boothId"], out int booth) ? booth : 0;
                data.SeniorOrDisableddata = int.TryParse(form.Form["SeniorOrDisableddata"], out int type) ? type : 0;
                data.VillageListId = form.Form["VillageListId"] ?? string.Empty;  // fixed key: was "name" earlier

                data.Name = form.Form["Name"] ?? string.Empty;
                data.Mobile = form.Form["Mobile"] ?? string.Empty;
                data.Address = form.Form["Address"] ?? string.Empty;  // fixed key: was "name" earlier

                data.Caste = int.TryParse(form.Form["Caste"], out int caste) ? caste : 0;
                data.Category = int.TryParse(form.Form["Category"], out int category) ? category : 0;

                bool result;
                string message;

                // Insert or Update
                if (data.Id > 0)
                {
                    result = _adminService.UpdateSeniorOrDisabled(data);
                    message = result ? "Record updated successfully." : "Failed to update record.";
                }
                else
                {
                    result = _adminService.InsertSeniorOrDisabled(data);
                    message = result ? "Record inserted successfully." : "Failed to insert record.";
                }

                return Json(new { success = result, message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllSeniorOrDisabled")]
        public IHttpActionResult GetSeniorOrDisabled(int? limit = null, int? page = null)
        {
     
            var data = _adminService.GetSeniorOrDisabledforapi(limit,page);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    SeniorOrDisabledAllData=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetAllSeniorOrDisabledById")]
        public IHttpActionResult GetSeniorOrDisabledById(int id)
        {
            
            var data = _adminService.GetSeniorOrDisabledforapi().FirstOrDefault(s => s.Id == id); 
            if (data != null )
            {
                return Ok(new
                {
                    status = true,
                    SeniorOrDisabledAllData = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/DeleteSeniorOrDisabledByid")]
        public IHttpActionResult DeleteSeniorOrDisabled(int Id)
        {
            var res = _adminService.DeleteSeniorOrDisabled(Id);
            if (res)
            {
                return Ok(new
                {
                    status=true,
                    message="Senior and disabled deleted successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="some error occured!!"
                });
            }
        }

        #region socialMediaPost

        [HttpPost]
        [Route("api/admin/SocialMediaPostInsert")]
        public IHttpActionResult InsertPost()
        {
            try
            {
                var request = HttpContext.Current.Request;
                //int postId = 0;
                //string postIdStr = request.Form["postId"];

                //if (!string.IsNullOrEmpty(postIdStr))
                //{
                //    int.TryParse(postIdStr, out postId);
                //}
                string title = request.Form["Title"];
                string description = request.Form["Description"];


                string boothIds = request.Form["BoothIds"];
                string sectorIds = request.Form["SectorIds"];
                string platforms = request.Form["Platform"];


                string postUrl = null;
                if (request.Files.Count > 0)
                {
                    var file = request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string uniqueFilename = Guid.NewGuid().ToString() + Path.GetExtension(filename);
                        string uploadFolder = HttpContext.Current.Server.MapPath("~/UploadsImages");
                        string fullpath = Path.Combine(uploadFolder, uniqueFilename);

                        file.SaveAs(fullpath);
                        postUrl = "/UploadsImages/" + uniqueFilename;
                    }
                }

                int res = _adminService.InsertPost(
                    new SocialMediaPost { title = title, Description = description, PostUrl = postUrl },
                    boothIds,
                    sectorIds,
                    platforms
                );




                if(res>0)
                {
                    return Ok(new
                    {
                        success = true,
                        message =   "Post inserted successfully.",

                    });
                }
                else
                {
                    return Ok(new
                    {
                        status=false,
                        message="Some error occured!!"
                    });
                }
                
               
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpPost]
        [Route("api/admin/SocialMediaPostUpdate")]
        public IHttpActionResult SocialMediaPostUpdate()
        {
            try
            {
                var request = HttpContext.Current.Request;
                int postId = 0;
                string postIdStr = request.Form["postId"];

                if (!string.IsNullOrEmpty(postIdStr))
                {
                    int.TryParse(postIdStr, out postId);
                }
                string title = request.Form["Title"];
                string description = request.Form["Description"];
                string boothIds = request.Form["BoothIds"];
                string sectorIds = request.Form["SectorIds"];
                string platforms = request.Form["Platform"];
           

                string postUrl = null;
                if (request.Files.Count > 0)
                {
                    var file = request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        string filename = Path.GetFileName(file.FileName);
                        string uniqueFilename = Guid.NewGuid().ToString() + Path.GetExtension(filename);
                        string uploadFolder = HttpContext.Current.Server.MapPath("~/UploadsImages");
                        string fullpath = Path.Combine(uploadFolder, uniqueFilename);

                        file.SaveAs(fullpath);
                        postUrl = "/UploadsImages/" + uniqueFilename;
                    }
                }

                var postModel = new SocialMediaPost
                {
                    PostId = postId,
                    title = title,
                    Description = description,
                    PostUrl = postUrl,
                    Platform = !string.IsNullOrEmpty(platforms) ? platforms.Split(','): new string[] { }

                  
                };

                var res = _adminService.UpdatePost(postModel, boothIds, sectorIds);

                if (res)
                {
                    return Ok(new
                    {
                        success = true,
                        message = "Post Updated successfully!!"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        message = "Some error occurred!!"
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllSocialMediaPost")]
        public IHttpActionResult GetAllSocialMedia(int? limit = null, int? page = null)
        {
            var data = _adminService.GetAllPosts(limit,page);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Social media get successfully!!",
                    SocialMediaPostData=data

                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetSocialMediaPostDetailById")]
        public IHttpActionResult GetSocialMediaPostDetailById(int PostId)
        {
            var data = _adminService.GetSocialMediaPostDetailById(PostId);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Social Media Post Detail get successfully!!",
                    SocialMediaDetailsData=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetSocialMediaPostById")]
        public IHttpActionResult GetSocialMediaPostById(int PostId)
        {
            var data = _adminService.GetPostById(PostId);
            if(data!=null)
            {
                return Ok(new
                {
                    status=true,
                    message="Social Media Post Data Get Successfully!!",
                    SocialMediaPostById=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/DeleteSocialMediaPostById")]
        public IHttpActionResult DeletePost(int PostId)
        {
            var res = _adminService.DeletePost(PostId);
            if(res)
            {
                return Ok(new
                {
                    status=true,
                    message="Social Media Post Deleted Successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }

        #endregion
        [HttpGet]
        [Route("api/admin/GetAllBDCMemberList")]
        public IHttpActionResult BDCMemberLists(
    [FromUri] FilterModel filter,
    int? type = 4,
    int? pageNumber = null,
    int? pageSize = null)


        {
            filter = filter ?? new FilterModel();
            var data = _adminService.GetEffectivePersonsPaged(filter, type, pageSize, pageNumber);

            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "BDC Member List retrieved successfully.",
                    BDCMemberList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No BDC members found for the given criteria."
                });
            }
        }


        #region ListPdfExcelApi
        //[HttpGet]
        //[Route("api/admin/PravasiVoterListExportReport")]
        //public IHttpActionResult PravasiVoterListExportReport(string format, [FromUri] FilterModel filter)
        //{
        //    filter = filter ?? new FilterModel();
        //    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        //    var reportData = _adminService.GetAllPravasiVoterData(filter);
        //    if(reportData==null || !reportData.Any())
        //    {
        //        return Ok(new
        //        {
        //            status=false,
        //            message="No data to generate report!!"
        //        });
        //    }
        //    // Define column headers
        //    var headers = new string[]
        //    {
        //"S.N.","Booth No."," Name","Category","Caste", "Contact","Current Address","Occupation",
        //    };

        //    // Create indexed data for serial numberS
        //    var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

        //    // Define mapping function that includes the S.N.
        //    Func<(int Index, PravasiVoter Data), List<string>> mapFunc = item => new List<string>
        //    {
        //        item.Index.ToString(),
        //        item.Data.boothno.ToString(),
        //        item.Data.name ?? "",
        //        item.Data.CategoryName ?? "",
        //        item.Data.CasteName .ToString(),
        //        item.Data.mobile ?? "",
        //        item.Data.currentAddress ?? "",
        //        item.Data.Occupation ?? "",
        //    };
        //    var generator = new ReportGenerator<(int Index, PravasiVoter Data)>();
        //    byte[] FileBytes;
        //    string ContentType;
        //    string fileName;

        //    if(format=="pdf")
        //    {
        //        FileBytes = generator.ExportToPdf(indexedData, headers, "PravasiVoter Report", mapFunc);
        //        ContentType = "application/pdf";
        //        fileName = "PravasiVoterReport.pdf";
        //    }
        //    else if(format=="excel")
        //    {
        //        FileBytes = generator.ExportToExcel(indexedData, headers, "PravasiVoter Report", mapFunc);
        //        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        fileName = "PravasiVoterReport.xlsx";
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status=false,
        //            message="Invalid format"
        //        });
        //    }
        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(FileBytes)
        //    };
        //    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
        //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = fileName
        //    };
        //    return ResponseMessage(result);
        //}

        //[HttpGet]
        //[Route("api/admin/GetNewVotersListExcelPdfReport")]
        //public IHttpActionResult GetNewVotersListPdfExcelReport(string format, [FromUri] FilterModel filter,int? limit = null,int? page=null)
        //{
        //    filter = filter ?? new FilterModel();
        //    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        //    var reportData = _adminService.GetNewVoters(filter,limit,page);
        //    if(reportData==null || !reportData.Any())
        //    {
        //        return Ok(new
        //        {
        //            status=false,
        //            message="No data to generate "
        //        });
        //    }

        //    // Define column headers
        //    var headers = new string[]
        //    {
        //"S.N.","Booth No.","Village"," Name","Father Name","Contact","Category","Caste", "DOB","Age Till (1-Jan-2027)",
        //    };

        //    // Create indexed data for serial numberS
        //    var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

        //    // Define mapping function that includes the S.N.
        //    Func<(int Index, NewVoters Data), List<string>> mapFunc = item => new List<string>
        //    {
        //        item.Index.ToString(),
        //        item.Data.BoothNumber.ToString(),
        //       string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        //        item.Data.Name ?? "",
        //        item.Data.FatherName .ToString(),
        //        item.Data.MobileNumber ?? "",
        //        item.Data.CategoryName ?? "",
        //        item.Data.caste ?? "",
        //        item.Data.DOB != null ? item.Data.DOB.Value.ToString("dd/MM/yyyy") : "",
        //        item.Data.totalage ?? "",

        //    };

        //    var generator = new ReportGenerator<(int Index, NewVoters Data)>();
        //    byte[] fileBytes;
        //    string ContentType;
        //    string fileName;
        //    if(format=="pdf")
        //    {
        //        fileBytes = generator.ExportToPdf(indexedData, headers, "NewVoters Report", mapFunc);
        //        ContentType = "application/pdf";
        //        fileName = "NewVotersReport.pdf";
        //    }
        //    else if(format=="excel")
        //    {
        //        fileBytes = generator.ExportToExcel(indexedData, headers, "NewVoters Report", mapFunc);
        //        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        fileName = "NewVotersReport.xlsx";
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status=false,
        //            message="Invalid format"
        //        });
        //    }
        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(fileBytes)
        //    };

        //    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
        //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = fileName
        //    };
        //    return ResponseMessage(result);
        //}

        [HttpGet]
        [Route("api/admin/BlockPramukhListExcelPdfReport")]
        public IHttpActionResult BlockPramukhListExcelPdfReport(string format, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _adminService.GetAllBlocks(filter);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report"
                });
            }
            // Define column headers
            var headers = new string[]
            {
        "S.N.","Booth Name.","Block Pramukh"," Contact","Caste","Address","Occupation",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, Block Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BlockName.ToString(),

                item.Data.InchargeName ?? "",
                item.Data.Contact .ToString(),
                item.Data.CasteName ?? "",
                item.Data.Address ?? "",
                item.Data.Occupation ?? "",
                //item.Data.ProfileImage ?? "",


            };

            var generator = new ReportGenerator<(int Index, Block Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Block Pramukh Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BlockPramukhReport.pdf";

            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Block Pramukh Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BlockPramukhReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format"
                });
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(FileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName

            };
            return ResponseMessage(result);

        }


        //[HttpGet]
        //[Route("api/admin/SahmatAsahmatExcelPdfList")]
        //public IHttpActionResult SahmatAsahmatExcelPdfList(int type, string format, [FromUri] FilterModel filter,int? limit=null,int? page=null)
        //{
        //    filter = filter ?? new FilterModel();
        //    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        //    var reportData = _adminService.GetAllDataSatisfiedUnsatisfied(type, filter,limit,page);
        //    if (reportData == null || !reportData.Any())
        //    {
        //        return Ok(new
        //        {
        //            status = false,
        //            message = "No data to generate report!!"
        //        });
        //    }

        //    var headers = new string[]
        //   {
        //"S.N.", "Booth No.", "Village", "Type","Name","Age", "Contact", "Party","Reason","Occupation"
        //   };

        //    var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

        //    Func<(int Index, SatisfiedUnSatisfied Data), List<string>> mapFunc = item => new List<string>
        //      {
        //        item.Index.ToString(),
        //        item.Data.boothNo.ToString()?? "",
        //        item.Data.village ?? "",
        //        item.Data.sahmatAsahmatName ?? "",
        //        item.Data.name ?? "",
        //        item.Data.age.ToString() ,
        //        item.Data.mobile ?? "",
        //        item.Data.party ?? "",
        //        item.Data.reason ?? "",
        //          item.Data.Occupation ?? "",

        //        };

        //    var generator = new ReportGenerator<(int Index, SatisfiedUnSatisfied Data)>();
        //    var Type = _adminService.getSahmatAsahmatType();
        //    var SahmatAsahmatType = Type
        //    .Where(x => x.Id == type)
        //    .Select(x => x.type).FirstOrDefault();

        //    byte[] FileBytes;
        //    string ContentType;
        //    string fileName;

        //    if(format=="pdf")
        //    {
        //        FileBytes = generator.ExportToPdf(indexedData, headers, $"{SahmatAsahmatType} List Report", mapFunc);
        //        ContentType = "application/pdf";
        //        fileName = $"{SahmatAsahmatType}Report.pdf";
        //    }
        //    else if(format=="excel")
        //    {
        //        FileBytes = generator.ExportToExcel(indexedData, headers, $"{SahmatAsahmatType} List Report", mapFunc);
        //        ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        fileName = $"{SahmatAsahmatType}Report.xlsx";
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status=false,
        //            message="Invalid format"
        //        });
        //    }
        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ByteArrayContent(FileBytes)
        //    };
        //    result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
        //    result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = fileName
        //    };
        //    return ResponseMessage(result);



        //}

        [HttpGet]
        [Route("api/admin/PrabhavshaliExcelPdfReport")]
        public IHttpActionResult PrabhavshaliExcelPdfReport(string type,string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _adminService.GetAllEffectivePersons();
            var filteredData = reportData;
            if(filteredData==null || !filteredData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report"
                });
            }
            // Headers
            var headers = new string[]
            {
        "S.N.", "Booth No.", "Village", "Type","Name","caste", "Contact", "Description",
            };

            var indexedData = filteredData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, EffectivePerson Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber.ToString()?? "",
        item.Data.village ?? "",
        item.Data.Designationdata ?? "",
        item.Data.Name ?? "",
        item.Data.Castename.ToString() ,
        item.Data.Mobile ?? "",
        item.Data.Description ?? "",

    };

            var generator = new ReportGenerator<(int Index, EffectivePerson Data)>();
            var Data = reportData
            .Where(x => x.Designation == type)
            .Select(x => x.Designationdata).FirstOrDefault();


            byte[] FileBytes;
            string ContentType;
            string fileName;

         

            if (format == "pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, $"{Data} List Report", mapFunc);
                ContentType = "application/pdf";
                fileName = $"{Data}Report.pdf";
            }
            else if (format == "excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, $"{Data}List Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"{Data}Report.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "Invalid format"
                });
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(FileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return ResponseMessage(result);

        }

        [HttpGet]
        [Route("api/admin/SeniorOrDisabledexportReport")]
        public IHttpActionResult SeniorDisabledExportReport(string type, string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var allData = _adminService.GetAllSeniorOrDisabled();
            var filteredData = allData.Where(x => x.SeniorOrDisabledStatus == type).ToList();
            var filteredTypes = filteredData
                .Select(x => x.SeniorOrDisabledType)
                .Distinct()
                .ToList();

            if (filteredData == null || !filteredData.Any())
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found for export"
                });
            }

            // Headers
            var headers = new string[]
            {
        "S.N.", "Booth No.", "Village", "Type","Name", "Contact", "Category","Caste","Address"
            };

            // Index data
            var indexedData = filteredData
                .Select((item, index) => (Index: index + 1, Data: item))
                .ToList();

            // Map function
            Func<(int Index, SeniorOrDisabled Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber.ToString() ?? "",
        item.Data.village ?? "",
        item.Data.SeniorOrDisabledStatus ?? "",
        item.Data.Name ?? "",
        item.Data.Mobile ?? "",
        item.Data.CategoryName ?? "",
        item.Data.SubCasteName ?? "",
        item.Data.Address ?? ""
    };

            var generator = new ReportGenerator<(int Index, SeniorOrDisabled Data)>();

            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format?.ToLower() == "pdf")
            {
                fileBytes = generator.ExportToPdf(indexedData, headers, $"{string.Join(", ", filteredTypes)} List Report", mapFunc);
                contentType = "application/pdf";
                fileName = $"{type}_Report.pdf";
            }
            else if (format?.ToLower() == "excel")
            {
                fileBytes = generator.ExportToExcel(indexedData, headers, $"{string.Join(", ", filteredTypes)} List Report", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"{type}_Report.xlsx";
            }
            else
            {
                return BadRequest("Invalid format");
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return ResponseMessage(response);
        }

        [HttpGet]
        [Route("api/admin/InfluencerExcelPdfList")]
        public IHttpActionResult InfluencerExcelPdfList(string format, [FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _adminService.GetInfluencers(filter, limit,page);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report!!"
                });
            }

            var headers = new string[]
             {
            "S.N.","Booth No.","Designation"," Name","Caste","Contact","Description",
             };
            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, Influencer Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber?.ToString() ?? "",
                item.Data.EffectiveDesignationdata ?? "",
                item.Data.PersonName?.ToString() ?? "",
                item.Data.SubCasteName ?? "",
                (item.Data.Mobile ?? 0L).ToString(),
                item.Data.Description?.ToString() ?? ""
                // item.Data.ProfileImage ?? ""
            };
            var generator = new ReportGenerator<(int Index, Influencer Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Influencer List Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "InfluencerListReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Influencer List Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "InfluencerListReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format"
                });
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(FileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return ResponseMessage(result);
        }
        #endregion


        #region SahmatAshamat api

        [HttpPost]
        [Route("api/admin/AddSahmatAsahmat")]
        public IHttpActionResult InsertOrUpdateSahmatAsahmat()
        {
            try
            {
                var form = HttpContext.Current.Request;
                //if (form == null || form.Form.Count == 0)
                //{
                //    return Ok(new { status = false, message = "All fields are required!!" });
                //}

                SatisfiedUnSatisfied data = new SatisfiedUnSatisfied();

                // Safe parsing for int fields
                string idStr = form.Form.Get("id");
                data.id = int.TryParse(idStr, out int parsedId) ? parsedId : 0;

                data.boothNo = int.TryParse(form.Form.Get("boothNo"), out int booth) ? booth : 0;
                data.sahmatAsahmat = int.TryParse(form.Form.Get("sahmatAsahmat"), out int type) ? type : 0;
                data.age = int.TryParse(form.Form.Get("age"), out int age) ? age : 0;
                data.Occupations = int.TryParse(form.Form.Get("Occupations"), out int occ) ? occ : 0;

                data.village = form.Form.Get("village") ?? "";
                data.name = form.Form.Get("name") ?? "";
                data.mobile = form.Form.Get("mobile") ?? "";
                data.party = form.Form.Get("party")?? "";
                data.village = form.Form.Get("village") ?? "";
                data.reason = form.Form.Get("reason") ?? "";

                var res = _adminService.AddSatisfiedUnsatisfied(data);

                if (res)
                {
                    return Ok(new
                    {
                        status = true,
                        message = data.id > 0 ? "Data Updated Successfully!!" : "Data Added Successfully!!"
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        message = data.id > 0 ? "Update failed, please try again!!" : "Insert failed, please try again!!"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = false,
                    message = "Something went wrong!!",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllParty")]
        public IHttpActionResult Getallparty()
        {
            var data = _adminService.Getallparty();
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Get party data",
                    PartyData=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=true,
                    message="No Data found"
                });
            }

        }

        [HttpGet]
        [Route("api/admin/GetAllDataSatisfiedUnsatisfied")]
        public IHttpActionResult GetAllDataSatisfiedUnsatisfied(int? limit=null,int? page=null)
        {  
            try
            {

                // Call service with correct order of params
                var data = _adminService.GetAllDataSatisfiedUnsatisfiedforapi(limit,page);

                if (data != null && data.Any())
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Get Satisfied/Unsatisfied Data",
                        data = data
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        message = "No Data Found"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = false,
                    message = "Something went wrong",
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("api/admin/deleteSatisfiedUnsatisfied")]
        public IHttpActionResult deleteSatisfiedUnsatisfied(int id)
        {
            var res = _adminService.deleteSatisfiedUnsatisfied(id);
            if(res)
            {
                return Ok(new
                {
                    status=true,
                    message="Data deleted successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Some error occured!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/getSahmatAsahmatType")]
        public IHttpActionResult getSahmatAsahmatType()
        {
            var res = _adminService.getSahmatAsahmatType();
            if (res!=null && res.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Sahmat & Asahmat Type data!!",
                    SahmatAsahmat=res
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetAllSatisfiedUnsatisfiedDataById")]
        public IHttpActionResult GetAllSatisfiedUnsatisfiedDataById(int id)
        {
            var res = _adminService.GetAllSatisfiedUnsatisfiedDataById(id);
            if (res!=null )
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SahmatAsahmatByid=res
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/VillageListByBoothId")]
        public IHttpActionResult VillageListByBoothId(int BoothId)
        {
            var data = _adminService.VillageListByBoothId(BoothId.ToString());
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Village List By Booth Id",
                    VilageLis=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found"
                });
            }
        }


        #endregion

        #region doublevoter

        [HttpPost]
        [Route("api/admin/addDoubleVoter")]
        public IHttpActionResult AddDoubleVoter()
        {
            var form = HttpContext.Current.Request;

            doubleVoter data = new doubleVoter();

            // Safe parsing for int fields
            string idStr = form.Form.Get("id");
            data.id = int.TryParse(idStr, out int parsedId) ? parsedId : 0;

            data.BoothNumber = int.TryParse(form.Form.Get("BoothNumber"), out int booth) ? booth : 0;
            data.name = form.Form.Get("name") ?? "";
            data.fathername = form.Form.Get("fathername") ?? "";
            data.voterno = form.Form.Get("voterno") ?? "";

            data.pastAddress = form.Form.Get("pastAddress") ?? "";
            data.currAddress = form.Form.Get("currAddress") ?? "";
            data.description = form.Form.Get("description") ?? "";
            data.VillageListId = form.Form.Get("VillageListId") ?? "";

            var res = _adminService.AddDoubleVoters(data);
            if (res)
            {
                return Ok(new
                {
                    status=true,
                    message=data.id>0?"Data updated successfully":"Data inserted successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    sttaus=false,
                    message=data.id>0?"Updation failed,please try again!!":"Insertion failed,please try again!!"
                });
            }

           
           

        }

        [HttpGet]
        [Route("api/admin/DeleteDoubleVoterById")]
        public IHttpActionResult DeleteDoubleVoter(int id)
        {
            var res = _adminService.deleteDoubleVoter(id);
            if (res)
            {
                return Ok(new
                {
                    status=true,
                    message="Data deleted successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="some error occured!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/getDoubleVoterById")]
        public IHttpActionResult getDoubleVoterById(int id)
        {
            var res = _adminService.getDoubleVoterById(id);
            if (res != null)
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SahmatAsahmatByid = res
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No data found!!"
                });
            }
        }


        #endregion


        #region Influencer 

        [HttpGet]
        [Route("api/admin/GetEffectiveDesignations")]
        public IHttpActionResult GetEffectiveDesignation()
        {
            var data = _adminService.GetEffectiveDesignations();
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="EffectivePersonDesignation get successfully!!",
                    EffectiveDesignation=data

                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetEffectivePersonByDesId")]
        public IHttpActionResult GetEffectivePersons([FromUri] FilterModel filter, int designationId)
        {
            try
            {
                filter = filter ?? new FilterModel();

                var data = _adminService.GetEffectivePersonsPaged(filter, designationId, null, null);

                if (data != null && data.Any())
                {
                    return Ok(new
                    {
                        status = true,
                        message = "Effective persons retrieved successfully!",
                        EffectivePerson = data
                    });
                }
                else
                {
                    return Ok(new
                    {
                        status = false,
                        message = "No data found"
                    });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("api/admin/InsertInfluencer")]
        public IHttpActionResult InsertInfluencer()
        {
            var form = HttpContext.Current.Request;
            Influencer data = new Influencer();

            string Id = form.Form["Id"];
            data.Id = string.IsNullOrEmpty(Id) ? 0 : Convert.ToInt32(Id);
            data.IsEffective = Convert.ToBoolean(form.Form["IsEffective"]);

            if (data.IsEffective)
            {
                data.Designation = Convert.ToInt32(form.Form["Designation"]);
                data.PersonId = Convert.ToInt32(form.Form["PersonId"]);
            }
            else
            {
                data.BoothId = Convert.ToInt32(form.Form["BoothId"]);
                data.PersonName = form.Form["PersonName"];
                data.Category = Convert.ToInt32(form.Form["Category"]);
                data.Caste = Convert.ToInt32(form.Form["Caste"]);
                data.Mobile = Convert.ToInt64(form.Form["Mobile"]);
                data.Description = form.Form["Description"];
                data.VillageListId = form.Form["VillageListId"];
            }

            var res = _adminService.InsertInfluencer(data);

            return Ok(new
            {
                status = res,
                message = res
                    ? (data.Id > 0 ? "Data Updated successfully!!" : "Data Inserted successfully!!")
                    : (data.Id > 0 ? "Updation failed...please try again!!" : "Insertion failed...please try again!!")
            });
        }
        [HttpGet]
        [Route("api/admin/DeleteInfluencerPerson")]
        public IHttpActionResult DeleteInfluencerPerson(int id)
        {
            var res = _adminService.DeleteInfluencerPerson(id);
            if(res)
            {
                return Ok(new
                {
                    status=true,
                    message="Influencer deleted successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="some error occured!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetInfluencerById")]
        public IHttpActionResult GetInfluencerById(int id)
        {
            var data = _adminService.GetInfluencerById(id);
            if(data!=null)
            {
                return Ok(new
                {
                    status=true,
                    message="Influencer data get successfully!!",
                    InfluencerDatabyId=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });

            }
        }
        [HttpGet]
        [Route("api/admin/GetInfluencers")]
        public IHttpActionResult GetInfluencers([FromUri] FilterModel filter,int? limit=null,int? page=null)
        {
            filter = filter ?? new FilterModel();
            var data = _adminService.GetInfluencers(filter,limit,page);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Influencers data get successfully!!",
                    Influencers=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }
        #endregion


        #region village

        [HttpGet]
        [Route("api/admin/GetAllVillage")]
        public IHttpActionResult GetAllVillage()
        {
            var data = _adminService.GetAllVillage();
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="All Village List!!",
                    VillageList=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }

        #endregion


        [HttpGet]
        [Route("api/admin/GetEffectivePersonReport")]
        public IHttpActionResult GetEffectivePersonReport([FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            var data = _adminService.GetEffectivePersonReport(filter);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Effective person report list!!",
                    EffectivePersonReport=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }

        }
        //[HttpGet]
        //[Route("api/admin/GetAllPravasiVoterData")]
        //public IHttpActionResult GetAllPravasiVoterData([FromUri] FilterModel filter, int? limit = null, int? page = null)
        //{
        //    filter = filter ?? new FilterModel();
        //    var data = _adminService.GetAllPravasiVoterData(filter, limit, page);
        //    if(data!=null && data.Any())
        //    {
        //        return Ok(new
        //        {
        //            status=true,
        //            message="PravasiVoterList Data!!",
        //            PravasiVoterList=data
        //        });
        //    }
        //    else
        //    {
        //        return Ok(new
        //        {
        //            status=false,
        //            message="No data found!!"
        //        });
        //    }
        //}
        [HttpGet]
        [Route("api/admin/influencerlist")]
        public IHttpActionResult influencerlist([FromUri] FilterModel filter,
int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _adminService.GetInfluencers(filter,limit,page);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Influencer List Data!!",
                    InfluencerList=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No data found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/admin/GetCombinedSectorReports")]
        public IHttpActionResult GetCombinedSectorReports( [FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            try
            {
                var sectors = _adminService.GetAll_SectorDetails(filter,limit,page);
                var combinedList = new List<object>();

                //foreach (var sector in sectors)
                //{
                //    var booths = _adminService.GetBoothslistbySectorId(sector.Id,limit,page);

                //    if (booths != null && booths.Any())
                //    {
                //        foreach (var b in booths)
                //        {
                //            var record = new
                //            {
                //                // --- Sector Data ---
                //                SectorId = sector.Id,
                //                MandalName = sector.MandalName,
                //                SectorName = sector.SectorName,
                //                SectorIncharge = sector.InchargeName,
                //                SectorPhone = sector.PhoneNumber,
                //                SectorFatherName = sector.FatherName,
                //                SectorAge = sector.Age,
                //                SectorSubCaste = sector.subcaste,
                //                SectorAddress = sector.Address,
                //                SectorProfileImage = sector.ProfileImage,
                //                SectorVillages = sector.VillageNames != null ? string.Join(", ", sector.VillageNames) : "",

                //                // --- Booth Data ---
                //                BoothId = b.BoothId,
                //                BoothNo = b.BoothNo,
                //                BoothName = b.BoothName,
                //                BoothIncharge = b.InchargeName,
                //                BoothMobile = b.Mobile,
                //                BoothFatherName = b.FatherName,
                //                BoothAge = b.Age,
                //                BoothSubCaste = b.SubCasteName,
                //                BoothAddress = b.Address,
                //                BoothEducation = b.Education,
                //                BoothProfileImage = b.ProfileImage
                //            };

                //            combinedList.Add(record);
                //        }
                //    }
                //    else
                //    {
                //        // Add sector even if no booth found
                //        var record = new
                //        {
                //            SectorId = sector.Id,
                //            MandalName = sector.MandalName,
                //            SectorName = sector.SectorName,
                //            SectorIncharge = sector.InchargeName,
                //            SectorPhone = sector.PhoneNumber,
                //            SectorFatherName = sector.FatherName,
                //            SectorAge = sector.Age,
                //            SectorSubCaste = sector.subcaste,
                //            SectorAddress = sector.Address,
                //            SectorProfileImage = sector.ProfileImage,
                //            SectorVillages = sector.VillageNames != null ? string.Join(", ", sector.VillageNames) : "",

                //            BoothId = (int?)null,
                //            BoothNo = "",
                //            BoothName = "",
                //            BoothIncharge = "",
                //            BoothMobile = "",
                //            BoothFatherName = "",
                //            BoothAge = (int?)null,
                //            BoothSubCaste = "",
                //            BoothAddress = "",
                //            BoothEducation = "",
                //            BoothProfileImage = ""
                //        };

                //        combinedList.Add(record);
                //    }
                //}

                return Ok(new
                {
                    status = true,
                    message = "Combined Sector + Booth Report fetched successfully.",
                    SectorCombinedReport = sectors
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    status = false,
                    message = "Error while fetching combined report.",
                    error = ex.Message
                });
            }
        }
        [HttpGet]
        [Route("api/admin/CombinedSectorReportExcelPdf")]
        public IHttpActionResult CombinedSectorExportExcelPdfReport(string format, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var sectors = _adminService.GetAll_SectorDetails(filter);
            var combinedList = new List<CombinedSectorBoothReport>();

            foreach (var sector in sectors)
            {
                var booths = _adminService.GetBoothslistbySectorId(sector.Id);

                foreach (var booth in booths)
                {
                    combinedList.Add(new CombinedSectorBoothReport
                    {
                        // Sector data
                        MandalName = sector.MandalName,
                        SectorName = sector.SectorName,
                        InchargeName = sector.InchargeName,
                        SectorPhoneNumber = sector.PhoneNumber,
                        SectorVillageNames = sector.VillageNames,
                        SectorFatherName = sector.FatherName,
                        SectorAge = sector.Age,
                        SectorCaste = sector.subcaste,
                        SectorAddress = sector.Address,
                        SectorEducation = sector.Education,
                        SectorProfileImage = sector.ProfileImage,

                        // Booth data
                        BoothNumber = booth.BoothNo,
                        BoothName = booth.BoothName,
                        BoothIncharge = booth.InchargeName,
                        BoothPhone = booth.Mobile,
                        BoothVillageNames = booth.VillageNames,
                        BoothFatherName = booth.FatherName,
                        BoothAge = booth.Age,
                        BoothCaste = booth.SubCasteName,
                        BoothAddress = booth.Address,
                        BoothEducation = booth.Education,
                        BoothProfileImage = booth.ProfileImage
                    });
                }
            }

            if (combinedList == null || !combinedList.Any())
                return Content(HttpStatusCode.NotFound, new { Status = 200, Message = "No data to generate report." });

            var headers = new string[]
            {
        "S.N.", "Mandal", "Sector", "Sector Sanyojak", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education",
        "Booth No", "Polling Station", "Booth Adhyaksh", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education"
            };

            var indexedData = combinedList.Select((data, index) => (Index: index + 1, Data: data)).ToList();

            Func<(int Index, CombinedSectorBoothReport Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.MandalName ?? "",
        item.Data.SectorName ?? "",
        item.Data.InchargeName ?? "",
        item.Data.SectorPhoneNumber ?? "",
        string.Join(", ", item.Data.SectorVillageNames ?? new List<string>()),
        item.Data.SectorFatherName ?? "",
        item.Data.SectorAge?.ToString() ?? "",
        item.Data.SectorCaste ?? "",
        item.Data.SectorAddress ?? "",
        item.Data.SectorEducation ?? "",

        item.Data.BoothNumber ?? "",
        item.Data.BoothName ?? "",
        item.Data.BoothIncharge ?? "",
        item.Data.BoothPhone ?? "",
        string.Join(", ", item.Data.BoothVillageNames ?? new List<string>()),
        item.Data.BoothFatherName ?? "",
        item.Data.BoothAge?.ToString() ?? "",
        item.Data.BoothCaste ?? "",
        item.Data.BoothAddress ?? "",
        item.Data.BoothEducation ?? ""
    };

            var generator = new ReportGenerator<(int Index, CombinedSectorBoothReport Data)>();

            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format?.ToLower() == "pdf")
            {
                fileBytes = generator.ExportToPdf(indexedData, headers, "Combined Sector Reports", mapFunc);
                contentType = "application/pdf";
                fileName = "CombinedSectorExportReport.pdf";
            }
            else if (format?.ToLower() == "excel")
            {
                fileBytes = generator.ExportToExcel(indexedData, headers, "Combined Sector Reports", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "CombinedSectorExportReport.xlsx";
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, new { Status = 404, Message = "Invalid format. Allowed values are pdf/excel." });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return ResponseMessage(result);
        }
        [HttpGet]
        [Route("api/admin/GetAllPradhanListNew")]
        public IHttpActionResult GetAllPradhanList(int? limit = null, int? page = null)
        {
            var data = _adminService.GetAllPradhan(limit,page);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    Message = "Pradhan List ",
                    PradhanList = data
                });
            }
            else
            {
              
                    return Ok(new
                    {
                        status = false,
                        Message = "No data Found ",
                 
                    });
                
            }

        }
        [HttpGet]
        [Route("api/admin/GetAllBDCListNew")]
        public IHttpActionResult GetAllBDCList(int? limit = null, int? page = null)
        {
            var data = _adminService.GetBDC(limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    Message = "BDC List ",
                    BDCList = data
                });
            }
            else
            {

                return Ok(new
                {
                    status = false,
                    Message = "No data Found ",

                });

            }

        }
        [HttpGet]
        [Route("api/admin/GetBoothList")]
        public IHttpActionResult GetAllBoothList([FromUri] FilterModel filter,int? limit=null,int? page=null)
        {
            filter = filter ?? new FilterModel();
            var data = _adminService.GetBoothList(filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    Message = "BDC List ",
                    BoothList = data
                });
            }
            else
            {

                return Ok(new
                {
                    status = false,
                    Message = "No data Found ",

                });

            }

        }

        [HttpGet]
        [Route("api/admin/PradhanListExportReport")] 
        public IHttpActionResult PradhanListExportReport(string format)
        {
           
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _adminService.GetAllPradhan();
            if (reportData == null || !reportData.Any())
            {
                return Ok(new
                {
                    status = false,
                    message = "No data to generate report!!"
                });
            }
            // Define column headers
            var headers = new string[]
            {
        "S.N.","Pradhan Name"," Village","Gender", "Contact",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, Pradhan Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Name.ToString(),
                item.Data.Village_Name ?? "",
                item.Data.GenderName ?? "",
                item.Data.Contact .ToString(),
            
            };
            var generator = new ReportGenerator<(int Index, Pradhan Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if (format == "pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Pradhan Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "PradhanReport.pdf";
            }
            else if (format == "excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Pradhan Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "PradhanReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "Invalid format"
                });
            }
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(FileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return ResponseMessage(result);
        }



        [HttpGet]
        [Route("api/admin/BDCListExportReport")]
        public IHttpActionResult BDCListExportReport(string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            if (_adminService == null)
            {
                return Ok(new { status = false, message = "_adminService not initialized!" });
            }

            var reportData = _adminService.GetBDC();
            if (reportData == null || !reportData.Any())
            {
                return Ok(new { status = false, message = "No data to generate report!!" });
            }

            var headers = new[]
            {
        "S.N.","Name","Village","Contact","Age","Caste","Education","Block","Party","Profile"
    };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BDC Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data?.Name ?? "",
        item.Data?.Village ?? "",
        item.Data?.Contact ?? "",
        item.Data?.Age.ToString() ?? "",
        item.Data?.CasteName ?? "",
        item.Data?.Education ?? "",
        item.Data?.BlockName ?? "",
        item.Data?.partyName ?? "",
        item.Data?.ProfileImage ?? ""
    };

            var generator = new ReportGenerator<(int Index, BDC Data)>();
            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == "pdf")
            {
                fileBytes = generator.ExportToPdf(indexedData, headers, "BDC Report", mapFunc);
                contentType = "application/pdf";
                fileName = "BDCReport.pdf";
            }
            else if (format == "excel")
            {
                fileBytes = generator.ExportToExcel(indexedData, headers, "BDC Report", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BDCReport.xlsx";
            }
            else
            {
                return Ok(new { status = false, message = "Invalid format" });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return ResponseMessage(result);
        }


        [HttpGet]
        [Route("api/admin/BoothListExportReport")]
        public IHttpActionResult BoothListExportReport(string format, [FromUri] FilterModel filter,int? limit=null,int? page=null)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            if (_adminService == null)
            {
                return Ok(new { status = false, message = "_adminService not initialized!" });
            }

            var reportData = _adminService.GetBoothList(filter,limit,page);
            if (reportData == null || !reportData.Any())
            {
                return Ok(new { status = false, message = "No data to generate report!!" });
            }

            var headers = new[]
            {
        "S.N.","Mandal Name","Sector Name","Booth No.","Polling Station","Booth Adhayksh","Contact","Vllage","Caste"
    };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, Booth Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data?.MandalName.ToString() ?? "",
        item.Data?.SectorName ?? "",
        item.Data?.BoothNumber ?? "",
        item.Data?.BoothName ?? "",
        item.Data?.InchargeName ?? "",
        item.Data?.PhoneNumber ?? "",
        item.Data?.Village ?? "",
        item.Data?.castname ?? "",
   
    };

            var generator = new ReportGenerator<(int Index, Booth Data)>();
            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == "pdf")
            {
                fileBytes = generator.ExportToPdf(indexedData, headers, "BoothList ", mapFunc);
                contentType = "application/pdf";
                fileName = "BoothList.pdf";
            }
            else if (format == "excel")
            {
                fileBytes = generator.ExportToExcel(indexedData, headers, "BoothList ", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothList.xlsx";
            }
            else
            {
                return Ok(new { status = false, message = "Invalid format" });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return ResponseMessage(result);
        }

        [HttpPost]
        [Route("api/admin/AddAndUpdatePradhan")]
        public IHttpActionResult AddAndUpdatePradhan()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                // Initialize model
                var model = new Pradhan();

                // Parse numeric fields safely
                int.TryParse(httpRequest.Form["PradhanId"], out int pradhanId);
                model.Id = pradhanId;

                int.TryParse(httpRequest.Form["Designation"], out int designationId);
                model.Designation = designationId;

                int.TryParse(httpRequest.Form["Gender"], out int gender);
                model.Gender = gender;

                //int.TryParse(httpRequest.Form["Mandal_Id"], out int mandalId);
                //model.Mandal_Id = mandalId;

                // Parse text fields
                model.Name = httpRequest.Form["Name"];
                model.villageId = httpRequest.Form["villageId"];
                model.Contact = httpRequest.Form["Contact"];

                // Optional: Validate required fields
                if (string.IsNullOrEmpty(model.Name) || string.IsNullOrEmpty(model.Contact))
                {
                    return BadRequest("Name and Contact are required fields.");
                }

                // Save to database (your service handles insert/update logic)
                bool isSaved = _adminService.InsertPradhan(model);

                if (isSaved)
                {
                    return Ok(new
                    {
                        success = true,
                        message = model.Id > 0
                            ? "Pradhan updated successfully."
                            : "Pradhan added successfully."
                    });
                }

                return Ok(new
                {
                    success = false,
                    message = "Failed to save Pradhan details. Please try again."
                });
            }
            catch (Exception ex)
            {
                // Return full error for debugging (optional: log exception internally)
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("api/admin/GetallPradhan")]
        public IHttpActionResult GetAllPradhan(int? limit = null, int? page = null)
        {
            var data = _adminService.GetAllPradhan(limit,page);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Pradhan Data",
                    PradhanData=data
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="No Data found"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/deletePradhanById")]
        public IHttpActionResult deletePradhanbyId(int id)
        {
            bool res = _adminService.DeletePradhan(id);
            if(res)
            {
                return Ok(new
                {
                    status=true,
                    message="Pradhan deleted successfully!!"
                });
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Some error occured!!"
                });
            }

        }
        [HttpGet]
        [Route("api/admin/GetAllPradhanById")]
        public IHttpActionResult GetPradhanById(int id)
        {
            var data = _adminService.GetAllPradhanById(id);
            if(data!=null && data.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="Pradhan Data by Id",
                    PradhanData=data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }

    }
}
