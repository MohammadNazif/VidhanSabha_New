using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VishanSabha.Services.Auth;
using VishanSabha.Services.SectorService;
using VishanSabha.Services;
using System.Threading.Tasks;
using VishanSabha.Models;

namespace VishanSabha.AdminApiController
{
    public class BoothApiController : ApiController
    {

        private readonly BoothService _boothService;


        public BoothApiController()
        {

            _boothService = new BoothService();

        }

        //Booth InchargePannel api
        [HttpGet]
        [Route("api/booth/GetallPannaListByBoothIncId")]
        public IHttpActionResult GetallPannaListByBoothIncId(int boothIncId,[FromUri] FilterModel filter,int? limit=null, int? page=null)
        {
            filter = filter ?? new FilterModel();
            var pannapramukhData = _boothService.GetallPannaListByBoothIncId(boothIncId, filter,limit,page);
            if (pannapramukhData != null && pannapramukhData.Any())
            {
                return Ok(new
                {
                    StatusCode = 200,
                    status = true,
                    data = pannapramukhData
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
        [Route("api/booth/GetTotalPannaCountByBoothIncId")]
        public IHttpActionResult GetTotalPannaCountByBoothIncId(int boothIncId)
        {
            int TotalPanna = _boothService.GetPannaCountByBoothIncId(boothIncId);
            if (TotalPanna > 0)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    status = true,
                    PannaPramukhcount = TotalPanna
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
        [Route("api/booth/GetBoothVoterCountByBoothIncId")]
        public IHttpActionResult GetBoothVoterCountByBoothIncId(int boothIncId)
        {
            int BoothVoter = _boothService.GetBoothVoterCountByBoothIncId(boothIncId);
            if (BoothVoter > 0)
            {
                return Ok(new
                {
                    StatusCode = 200,
                    status = true,
                    BoothVoterCount = BoothVoter
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
        [Route("api/booth/GetBoothVoterListByBoothIncId")]
        public IHttpActionResult GetBoothVoterListByBoothIncId(int boothIncId,int? limit=null,int? page=null)
        {
            var BoothVoterList = _boothService.getBoothVoterDesByBoothIncId(boothIncId,limit,page);
            if (BoothVoterList != null && BoothVoterList.Any())
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    BoothVoterDesData = BoothVoterList
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
        [Route("api/booth/GetBoothsamitiCountByBoothIncId")]
        public IHttpActionResult BoothsamitiCountByBoothIncId(int boothIncId)
        {
            int BoothSamiti = _boothService.GetBoothSamitiCountByBoothIncId(boothIncId);
            if (BoothSamiti > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    BoothSamitiCount = BoothSamiti
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
        [Route("api/booth/GetBoothSamitiListByIncId")]
        public IHttpActionResult BoothSamitiListByIncId(int boothIncId)
        {
            var BoothSamiti = _boothService.GetAllBoothSamitiByIncId(boothIncId);
            if (BoothSamiti != null && BoothSamiti.Any())
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    BoothSamitiData = BoothSamiti
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
        //booth Inchargepannel end



     

        [HttpGet]
        [Route("api/admin/BoothDashboardCountByBoothIncId")]
        public IHttpActionResult BoothDashboardCount(int boothIncId)
        {
            var data = _boothService.BoothDashboardCount(boothIncId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    BoothDashboardcount = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    StatusCode = 404,
                    message = "Some error occured!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetAllBoothSamitiByIncId")]
        public IHttpActionResult GetAllBoothSamitiByIncId(int boothIncId, int? limit = null, int? page = null)
        {
            var data = _boothService.GetAllBoothSamitiByIncId(boothIncId,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    BoothSamitiList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    StatusCode = 404,
                    message = "Some error occured!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/CasteListForBoothVoterDesByBoothIncId")]
        public IHttpActionResult CasteListByBoothVoterDes(int BoothIncId, int boothVoterDesId)
        {
            var data = _boothService.getCastVoterDesByBoothIncId(BoothIncId, boothVoterDesId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data Found Successfully!!",
                    CasteListForBoothVoterDes = data
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
        [Route("api/admin/getallsahmatlist")]
        public IHttpActionResult getallsahmatlist(int boothIncId,[FromUri]FilterModel filter,int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _boothService.getallsahmatlist(boothIncId, filter, limit, page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data Get Successfully!!",
                    SahmatDataList = data
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
        [Route("api/admin/GetAllAsahmatListByBoothIncId")]
        public IHttpActionResult GetAllAsahmatListByBoothIncId(int BoothIncId, [FromUri] FilterModel filter,
int? limit = null, int? page = null)
        {
            var data = _boothService.getallAsahmatlist(BoothIncId, filter, limit, page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    AsahmatList = data
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
        [Route("api/admin/GetAllTotalPravasiListByBoothIncId")]
        public IHttpActionResult GetAllTotalPravasiList(int BoothIncId,[FromUri] FilterModel filter,
int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _boothService.GetAllTotalPravasiList(BoothIncId, filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    PravasiList = data
                });
            }
            else
            {
                return Ok(new
                {
                    status = true,
                    message = "No data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/getalldoubleVoterByBoothIncId")]
        public IHttpActionResult getalldoubleVoterByBoothIncId(int BoothIncId,

int? limit = null, int? page = null)
        {
            var data = _boothService.getalldoubleVoterByBoothIncId(BoothIncId,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    DoubleVoterList = data
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
        [Route("api/admin/GetBoothInchargeProfileByBoothIncId")]
        public IHttpActionResult GetBoothInchargeProfileByBoothIncId(int BoothIncId)
        {
            var data = _boothService.GetBoothInchargeProfileByBoothIncId(BoothIncId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    BoothInchargeProfile = data
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
        [Route("api/admin/GetAllPrabhavsaliByBoothIncId")]
        public IHttpActionResult GetAllPrabhavsaliByBoothIncId(int BoothIncId, [FromUri] FilterModel filter,

int? limit = null, int? page = null)
        {
            var data = _boothService.GetAllPrabhavsaliByBoothIncId(BoothIncId, filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get found!!",
                    PrabhavshaliList = data
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
        [Route("api/booth/GetAllNewvoterByBoothIncId")]
        public IHttpActionResult GetAllNewvoterByBoothIncId(int BoothIncId,[FromUri] FilterModel filter,

int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _boothService.GetAllNewvoterByBoothIncId(BoothIncId, filter, limit, page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    NewVotersList = data
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
        [Route("api/admin/GetAllSeniorCitizenByBoothIncId")]
        public IHttpActionResult GetAllSeniorCitizenByBoothIncId(int BoothIncId, [FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            var data = _boothService.GetAllSeniorCitizenByBoothIncId(BoothIncId, filter, limit, page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SeniorCitizenList = data
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
        [Route("api/booth/GetAllHandiCapByBoothIncId")]
        public IHttpActionResult GetAllHandiCapByBoothIncId(int BoothIncId, [FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _boothService.GetAllHandiCapByBoothIncId(BoothIncId, filter, limit, page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    HandicapList = data
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
        [Route("api/admin/GetSocialMediaPostByBoothId")]
        public IHttpActionResult GetSocialMediaPostByBoothId(int BoothId,
int? limit = null, int? page = null)
        {
            var data = _boothService.GetSocialMediaPostByBoothId(BoothId,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SocialMediaPostForBoothPannel = data
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

        #region Booth excel pdf report

        [HttpGet]
        [Route("api/Booth/BoothDesiabledExcelPdfReport")]
        public IHttpActionResult BoothDesiabledExportReport(string format,int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.GetAllHandiCapByBoothIncId(BoothIncId, filter);
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
            "S.N.", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Category", "Caste", "Address","Current Address", "Contact",
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.Name ?? "",
                item.Data.Category ?? "",
                item.Data.Caste ?? "",
                item.Data.Address ?? "",
                item.Data.Mobile ?? "",
            };
            var generator = new ReportGenerator<(int Index, SeniorCitizenList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth ViklaangVoter Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothViklaanVoterReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth ViklaangVoter Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothViklaanVoterReport.xlsx";
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


        [HttpGet]
        [Route("api/Booth/BoothVoterDesExportReport")]
        public IHttpActionResult BoothVoterDesExportReport(string format,int BoothIncId)
        {
           
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.getBoothVoterDesByBoothIncId(BoothIncId);
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
            "S.N.", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Total Voters", "Total Man","Total Woman" , "Total Other"
          };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BoothVotersDes Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.TotalVoters.ToString(),
                item.Data.TotalMan.ToString() ,
                item.Data.TotalWoman.ToString(),
                item.Data.TotalOther.ToString(),

            };
            var generator = new ReportGenerator<(int Index, BoothVotersDes Data)>();

            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth BoothVoterDescription Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothVoterDescriptionReport.pdf";

            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth BoothVoterDescription", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothVoterDescription.xlsx";
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
        [HttpGet]
        [Route("api/Booth/PannaPramukhExportReport")]
        public IHttpActionResult PannaPramukhExportReport(string format,int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.GetallPannaListByBoothIncId(BoothIncId,filter);
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
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Panna No.","Panna Pramukh", "Category ","Caste" , "Voter Id","Village" , "Address","Mobile"
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PannaPramukh Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.PannaNumber.ToString(),
                item.Data.Pannapramukh ?? "" ,
                item.Data.Categoryname ?? "",
                item.Data.SubCasteName?? "",
                item.Data.VoterNumber?? "",
                item.Data.village?? "",
                item.Data.Address?? "",
                item.Data.Mobile?? "",

            };

            var generator = new ReportGenerator<(int Index, PannaPramukh Data)>();

            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth PannaPramukh Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothPannaPramukhReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth PannaPramukh Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothPannaPramukhReport.xlsx";
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
        [HttpGet]
        [Route("api/Booth/NewVoterExportReport")]
        public IHttpActionResult NewVoterExportReport(string format,int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.GetAllNewvoterByBoothIncId(BoothIncId, filter);
            if(reportData==null ||!reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report"
                });
            }

            var headers = new string[]
          {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name", "Father Name ","Category" , "Caste","Contact" , "DOB","Total Age"
          };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, NewVoterList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNo ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.Village ?? "",
                item.Data.name ?? "" ,
                item.Data.fathername ?? "",
                item.Data.Category?? "",
                item.Data.Caste?? "",
                item.Data.Mobile?? "",
                @DateTime.Parse(item.Data.dob).ToString("yyyy-MM-dd"),

                item.Data.totalAge?? "",

            };

            var generator = new ReportGenerator<(int Index, NewVoterList Data)>();

            byte[] FileBytes;
            string ContentType;
            string fileName;
            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth NewVoter Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothNewVoterReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth NewVoter Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothNewVoterReport.xlsx";
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
        [HttpGet]
        [Route("api/Booth/BoothPravasiExportReport")]
        public IHttpActionResult BoothPravasiExportReport(string format,int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.GetAllTotalPravasiList(BoothIncId,filter);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report"
                });
            }
            var headers = new string[]
           {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Pravasi Name","Category", "Caste ","Contact" , "Current Address","Occupation" ,
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PravasiVoter Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.name ?? "",
                item.Data.CategoryName ?? "",
                item.Data.CasteName?? "",
                item.Data.mobile?? "",
                item.Data.currentAddress?? "",
                item.Data.Occupation?? "",

            };

            var generator = new ReportGenerator<(int Index, PravasiVoter Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth PravasiVoter Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothPravasiVoterReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth PravasiVoter Report", mapFunc);
                ContentType= "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothPravasiVoterReport.xlsx";
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

        [HttpGet]
        [Route("api/Booth/BoothSahamatExportReport")]
        public IHttpActionResult BoothSahamatExportReport(string format,int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.getallsahmatlist(BoothIncId,filter);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report"
                });
            }
            var headers = new string[]
           {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name", "age ","Contact","Party","Occupation" ,
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedUnSatisfied Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.boothNo.ToString(),
                item.Data.MandalName ?? "",
                item.Data.sectorName ?? "",
                item.Data.BoothName ?? "",
               (item.Data.VillageNames != null && item.Data.VillageNames.Any()
                ? string.Join(", ", item.Data.VillageNames)
                : "No villages listed"),

                item.Data.name ?? "",
                item.Data.age.ToString(),
                item.Data.mobile?? "",
                item.Data.party?? "",
                item.Data.Occupation?? "",

            };

            var generator = new ReportGenerator<(int Index, SatisfiedUnSatisfied Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;
            if (format == "pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth SahmatVoter Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothSahmatVoterReport.pdf";
            }
            else if (format == "excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth SahmatVoter Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothSahmatVoterReport.xlsx";
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
        [Route("api/Booth/BoothAsahamatExportReport")]
        public IHttpActionResult BoothAsahamatExportReport(string format, int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.getallAsahmatlist(BoothIncId, filter);
            if (reportData == null || !reportData.Any())
            {
                return Ok(new
                {
                    status = false,
                    message = "No data to generate report"
                });
            }
            var headers = new string[]
             {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name", "age ","Contact","Party","Reason","Occupation" ,
             };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedUnSatisfied Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.boothNo.ToString(),
                item.Data.MandalName ?? "",
                item.Data.sectorName ?? "",
                item.Data.BoothName ?? "",
               (item.Data.VillageNames != null && item.Data.VillageNames.Any()
                ? string.Join(", ", item.Data.VillageNames)
                : "No villages listed"),

                item.Data.name ?? "",
                item.Data.age.ToString(),
                item.Data.mobile?? "",
                item.Data.party?? "",
                item.Data.reason?? "",
                item.Data.Occupation?? "",
            };

            var generator = new ReportGenerator<(int Index, SatisfiedUnSatisfied Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;
            if (format == "pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth AsahmatVoter Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothAsahmatVoterReport.pdf";
            }
            else if (format == "excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth AsahmatVoter Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothAsahmatVoterReport.xlsx";
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
        [Route("api/Booth/BoothDoubleVotersExportReport")]
        public IHttpActionResult BoothDoubleVotersExportReport(string format,int BoothIncId)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.getalldoubleVoterByBoothIncId(BoothIncId);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report"
                });
            }
            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Father Name","Voter Id","Current Address","Previous Address" ,"Reason",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, doubleVoter Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber.ToString(),
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
               (item.Data.VillageNames != null && item.Data.VillageNames.Any()
                ? string.Join(", ", item.Data.VillageNames)
                : "No villages listed"),

                item.Data.name ?? "",
                item.Data.fathername ?? "",
                item.Data.voterno?? "",
                item.Data.currentAddress?? "",
                item.Data.pastAddress?? "",
                item.Data.reason?? "",

            };

            var generator = new ReportGenerator<(int Index, doubleVoter Data)>();

            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth DoubleVoter Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothDoubleVoterReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth DoubleVoter Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothDoubleVoterReport.xlsx";
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
                FileName=fileName
            };
            return ResponseMessage(result);
        }
        [HttpGet]
        [Route("api/Booth/BoothSeniorCitizenExportReport")]
        public IHttpActionResult BoothSeniorCitizenExportReport(string format, int BoothIncId,[FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.GetAllSeniorCitizenByBoothIncId(BoothIncId, filter);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report"
                });
            }

            var headers = new string[]
           {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Category","Caste ","Address ","Mobile" ,
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno.ToString(),
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
               (item.Data.Village != null && item.Data.Village.Any()
                ? string.Join(", ", item.Data.Village)
                : "No villages listed"),

                item.Data.Name ?? "",
                item.Data.Category ?? "",
                item.Data.Caste?? "",
                item.Data.Address?? "",
                item.Data.Mobile?? "",

            };

            var generator = new ReportGenerator<(int Index, SeniorCitizenList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth VarishthNagrik Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothVarishthNagrikReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth VarishthNagrik Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothVarishthNagrikReport.xlsx";
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
        [HttpGet]
        [Route("api/Booth/BoothSamithiExportReport")]
        public IHttpActionResult BoothSamithiExportReport(string format,int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.GetAllBoothSamitiByBoothIncId(BoothIncId, filter);
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
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Designation","Category","Caste","Age ","Contact ","Occupation" ,
          };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BoothSamiti Data), List<string>> mapFunc = item => new List<string>
            {
               item.Index.ToString(),
                item.Data.BoothNumber.ToString(),
                item.Data.MandalName ?? "",
                item.Data.sectorName ?? "",
                item.Data.BoothName ?? "",
               (item.Data.village != null && item.Data.village.Any()
                ? string.Join(", ", item.Data.village)
                : "No villages listed"),
                 item.Data.Name ?? "",
                 item.Data.Designation ?? "",

                item.Data.CategoryName ?? "",
                item.Data.SubCasteName ?? "",
                 item.Data.Age.ToString(),
                item.Data.Mobile?? "",
                item.Data.Occupation ?? "",

            };
            var generator = new ReportGenerator<(int Index, BoothSamiti Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth BoothSamiti Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothSamitiReportForBooth.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth BoothSamiti Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothSamitiReportForBooth.xlsx";
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
            result.Content.Headers.ContentType=new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return ResponseMessage(result);
        }
        [HttpGet]
        [Route("api/Booth/BoothPrabhavsaliExportReport")]
        public IHttpActionResult BoothPrabhavsaliExportReport(string format,int BoothIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _boothService.GetAllPrabhavsaliByBoothIncId(BoothIncId,filter);
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
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Name","Category","Caste","Contact ","Description","Designation" ,
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, EffectivePersonList Data), List<string>> mapFunc = item => new List<string>
            {
               item.Index.ToString(),
                item.Data.BoothNo.ToString(),
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
               //(item.Data.village != null && item.Data.village.Any()
               // ? string.Join(", ", item.Data.village)
               // : "No villages listed"),
               //  item.Data.Name ?? "",
               //  item.Data.Designation ?? "",
                item.Data.Name ?? "",
                item.Data.Category ?? "",
                item.Data.Caste ?? "",
                item.Data.Mobile?? "",
                item.Data.Description ?? "",
                item.Data.Designation ?? "",
            };

            var generator = new ReportGenerator<(int Index, EffectivePersonList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Booth Prabhavshali Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "BoothPrabhavshaliReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Booth Prabhavshali Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "BoothPrabhavshaliReport.xlsx";
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

    }

}
