using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VishanSabha.Services.SectorService;
using VishanSabha.Models;




namespace VishanSabha.AdminApiController
{
    public class SectorApiController : ApiController
    {

   
        private readonly SectorService _sectorService;
     

        public SectorApiController()
        {
        
            _sectorService = new SectorService();
      
        }

        [HttpGet]
        [Route("api/admin/SectorDashboardCount")]
        public IHttpActionResult SectorDashboardCount(int SecIncId)
        {
            var data = _sectorService.SectorDashboardCount(SecIncId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    SectorDashboardCount = data
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
        [Route("api/admin/GetSectorInchargeProfileBySecIncId")]
        public IHttpActionResult GetSectorInchargeProfileById(int sectorId)
        {
            var data = _sectorService.GetSectorIncProfile(sectorId);
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
                    message = "No Data Found!!"
                });
            }
        }

        [HttpGet]
        [Route("api/sector/GetBoothListBySectorIncId")]
        public IHttpActionResult GetBoothListBySectorIncId(int sectorIncId,[FromUri] FilterModel filter,
int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var BoothList = _sectorService.GetBoothsBySectorId(sectorIncId, filter,limit,page);
            if (BoothList != null && BoothList.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    SectorBoothList = BoothList
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
        [Route("api/admin/GetPannaListBySectorId")]
        public IHttpActionResult GetPannaListBySectorId(int secIncid,[FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetPannaBySectorId(secIncid, filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    PannaList = data
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
        [Route("api/admin/GetAllSatisfiedListBySecIncId")]
        public IHttpActionResult GetAllSatisfiedBySecIncId(int SecIncId, [FromUri] FilterModel filter,int? limit=null,int? page=null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllSatisfiedBySecIncId(SecIncId,filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    SatisfiedList = data
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
        [Route("api/admin/GetAllUnSatisfiedBySecIncId")]
        public IHttpActionResult GetAllUnSatisfiedBySecIncId(int SecIncId, [FromUri]   FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllUnSatisfiedBySecIncId(SecIncId, filter, limit, page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    UnSatisfiedList = data
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
        [Route("api/sector/GetAllPravasiBySecIncId")]
        public IHttpActionResult GetAllPravasiBySecIncId(int SecIncId,[FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllPravasiBySecIncId(SecIncId,filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    PravasiList = data
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
        [Route("api/admin/GetAllNewvoterBySecIncId")]
        public IHttpActionResult GetAllNewvoterBySecIncId(int SecIncId,[FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllNewvoterBySecIncId(SecIncId, filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    PravasiList = data
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
        [Route("api/sector/GetAllEffectivePersonBySecIncId")]
        public IHttpActionResult GetAllEffectivePersonBySecIncId(int SecIncId,[FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllEffectivePersonBySecIncId(SecIncId,filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    EffectivePersonList = data
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
        [Route("api/admin/GetAllDoublevoteBySecIncId")]
        public IHttpActionResult GetAllDoublevoteBySecIncId(int SecIncId,[FromUri] FilterModel filter,int? limit=null,int? page=null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllDoublevoteBySecIncId(SecIncId, filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    DoubleVoterList = data
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
        [Route("api/admin/GetAllSeniorCitizenBySecIncId")]
        public IHttpActionResult GetAllSeniorCitizenBySecIncId(int SecIncId,[FromUri]  FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllSeniorCitizenBySecIncId(SecIncId, filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    SeniorCitizenList = data
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
        [Route("api/admin/GetAllHandiCapBySecIncId")]
        public IHttpActionResult GetAllHandiCapBySecIncId(int SecIncId, [FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllHandiCapBySecIncId(SecIncId,filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    HandiCapList = data
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
        [Route("api/admin/GetAllActivityForSectorIncid")]
        public IHttpActionResult GetAllActivityForSectorIncid()
        {
            int count = _sectorService.GetAllActivity();
            if (count != 0 && count > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = "Count Fetched!!",
                    ActivityCount = count

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
        [Route("api/admin/GetAllActivityListForSectorInc")]
        public IHttpActionResult GetAllActivityListForSectorInc(int? limit = null, int? page = null)
        {
            var count = _sectorService.GetAllActivityForSectorInc(limit,page);
            if (count != null && count.Any())
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = "Activity Data Fetched!!",
                    ActivityList = count

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
        [Route("api/admin/BoothVoterDesBySecIncId")]
        public IHttpActionResult BoothVoterDesBySecIncId(int SecIncId)
        {
            var data = _sectorService.BoothVoterDesBySecIncId(SecIncId);
            if (data != 0 && data > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    message = "Booth Voter Des Count Fetched!!",
                    BoothVoterDesCount = data
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
        [Route("api/admin/GetAllBoothSamitiBySectorIncId")]
        public IHttpActionResult GetAllBoothSamitiBySectorIncId(int SecIncId,[FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetAllBoothSamitiBySectorIncId(SecIncId, filter,limit,page);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    BoothSamiti = data
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetAllHandiCapBySecIncIdCount")]
        public IHttpActionResult GetAllHandiCapBySecIncIdCount(int SecIncId)
        {
            var data = _sectorService.GetHandicapedCount(SecIncId);
            if (data != 0)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Count Fetched!!",
                    HandicapedCount = data
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/admin/GetAllBoothVoterDescbySectId")]
        public IHttpActionResult GetAllBoothVoterDescbySectId(int SecIncId,[FromUri] FilterModel filter, int? limit = null, int? page = null)
        {
            filter = filter ?? new FilterModel();
            var data = _sectorService.GetBoothVoterdesBySectorId(SecIncId,filter,limit,page);
            if (data != null)
            {
                return Ok(new
                {
                    status = true,
                    StatusCode = 200,
                    message = "Data Fetched!!",
                    BoothVoterDesc = data
                });
            }
            else
            {
                return Content(HttpStatusCode.NotFound, new
                {
                    status = false,
                    message = "No Data Found!!"
                });
            }
        }
        [HttpGet]
        [Route("api/sector/GetBoothCountBySectorId")]
        public IHttpActionResult GetBoothCountBySectorId(int sectorIncid)
        {
            int Boothcount = _sectorService.GetBoothCountBySectorId(sectorIncid);
            if (Boothcount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    SectorBoothCount = Boothcount
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
        [Route("api/sector/GetPannaCountBySectorId")]
        public IHttpActionResult GetPannaCountBySectorId(int sectorIncid)
        {
            int PannaCount = _sectorService.GetPannaCountBySectorId(sectorIncid);
            if (PannaCount > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    SectorPannaCount = PannaCount
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
        [Route("api/admin/GetBoothVoterDesCountBySectorIncId")]
        public IHttpActionResult GetBoothVoterDesCountBySectorIncId(int sectorIncId )
        {
            int BoothVoterDes = _sectorService.GetBoothVoterDesCountBySectorId(sectorIncId);
            if (BoothVoterDes > 0)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    BoothVoterDesCount = BoothVoterDes,

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
        [Route("api/admin/GetPannaListBySectorIncId")]
        public IHttpActionResult GetPannaListBySectorIncId(int sectorIncId ,[FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            var PannaList = _sectorService.GetPannaBySectorId(sectorIncId , filter);
            if (PannaList != null)
            {
                return Ok(new
                {
                    status = true,
                    statusCode = 200,
                    PannaPramukhList = PannaList,

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
        [Route("api/admin/GetSocialMediaPostBySectorId")]
        public IHttpActionResult GetSocialMediaPostBySectorId(int sectorId)
        {
            var data = _sectorService.GetSocialMediaPostBySectorId(sectorId);
            if (data != null && data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "Data get successfully!!",
                    SocialMediaPostForSectorPannel = data
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

        #region excel pdf report
        [HttpGet]
        [Route("api/sector/DisabledExportPdfReportForSecPnl")]
        public IHttpActionResult sectorDesiabledExportReport(string format, int SecIncId,[FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllHandiCapBySecIncId(SecIncId,filter);
            if (reportData == null || !reportData.Any())
            {
                return Ok(new
                {
                    status = false,
                    message = "No data to generate report!!"
                });
            }

            var headers = new string[]
            {
        "S.No", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Category", "Caste", "Address","Contact"
            };

            var indexData = reportData.Select((item, index) => (index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.Boothno ?? "",
        item.Data.MandalName ?? "",
        item.Data.SectorName ?? "",
        item.Data.BoothName ?? "",
        item.Data.Village ?? "",
        item.Data.Name ?? "",
        item.Data.Category ?? "",
        item.Data.Caste ?? "",
        item.Data.Address ?? "",
        item.Data.Mobile ?? "",
    };

            var generator = new ReportGenerator<(int Index, SeniorCitizenList Data)>();

            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == "pdf")
            {
                fileBytes = generator.ExportToPdf(indexData, headers, "Sector Vikalaag Report", mapFunc);
                contentType = "application/pdf";
                fileName = "SectorVikalaag.pdf";
            }
            else if (format == "excel")
            {
                fileBytes = generator.ExportToExcel(indexData, headers, "Sector Vikalaag Report", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorVikalaag.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    messsage = "Invalid format!!"
                });
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
        [Route("api/sector/BoothExcelPdfReportForSector")]
        public IHttpActionResult BoothExcelPdfReport(string format, int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetBoothsBySectorId(SecIncId,filter);
            if (reportData == null || !reportData.Any())
            {
                return Ok(new
                {
                    status = false,
                    message = "No data to generate report!!"
                });
            }
            var headers = new string[]
             {
            "S.No", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Booth Adhyaksh","Father Name", "Caste","Contact","Education",
             };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, Booth Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.Village ?? "",
                item.Data.InchargeName ?? "",
                item.Data.FatherName ?? "",
                item.Data.castname ?? "",
                item.Data.Mobile ?? "",
                item.Data.Education ?? "",
            };
            var generator = new ReportGenerator<(int Index, Booth Data)>();

            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == "pdf")
            {
                fileBytes = generator.ExportToPdf(indexedData, headers, "Sector Booth Report", mapFunc);
                contentType = "application/pdf";
                fileName = "SectorBooth.pdf";
            }
            else if (format == "excel")
            {
                fileBytes = generator.ExportToExcel(indexedData, headers, "Sector Booth Report", mapFunc);
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorBooth.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status = false,
                    messsage = "Invalid format!!"
                });
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
        [Route("api/sector/BoothSamitiReportForSector")]
        public IHttpActionResult BoothSamitiReport(string format,int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();

            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllBoothSamitiBySectorIncId(SecIncId, filter);
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
            "S.No",  "Mandal Name", "Sector Name","Booth No.", "Polling Station","Booth Adhyaksh","Designation", "Category","Caste","Age","Contact","Occupation",
          };

            var indexData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BoothSamiti Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.MandalName ?? "",
                item.Data.sectorName ?? "",
                 item.Data.BoothNumber ?? "",
                item.Data.BoothName ?? "",
                item.Data.BoothIncharge ?? "",
                item.Data.Designation ?? "",
                item.Data.CategoryName ?? "",
                item.Data.Cast ?? "",
                item.Data.Age.ToString(),
                item.Data.Mobile ?? "",
                item.Data.Occupation ?? "",
            };

            var generator = new ReportGenerator<(int Index, BoothSamiti Data)>();

            byte[] fileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                fileBytes = generator.ExportToPdf(indexData, headers, "Sector BoothSamiti Report ", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorBoothSamiti.pdf";
            }
            else if(format=="excel")
            {
                fileBytes = generator.ExportToExcel(indexData, headers, "Sector BoothSamiti Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorBoothSamiti.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid Format!!"
                });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(fileBytes)
            };
            result.Content.Headers.ContentType =new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return ResponseMessage(result);

        }

        [HttpGet]
        [Route("api/sector/PravasiReportForSector")]
        public IHttpActionResult PravasiReportForSector(string format,int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllPravasiBySecIncId(SecIncId, filter);

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
            "S.No",  "Mandal Name", "Sector Name","Booth No.", "Polling Station","Pravasi Name","Category","Caste","Contact","Current Add.","Occupation",
              };

            var indexData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PravasiList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                 item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                 item.Data.BoothNo ?? "",
                item.Data.BoothName ?? "",
                //item.Data.BoothIncharge ?? "",
                item.Data.name ?? "",
                item.Data.Category ?? "",
                item.Data.Caste ?? "",
                //item.Data.Age.ToString(),
                item.Data.Mobile ?? "",
                item.Data.CurrAddress ?? "",
                item.Data.Occupation ?? "",
            };
            var generator = new ReportGenerator<(int Index, PravasiList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexData, headers, "Secot PravasiList Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorPravasiReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexData, headers, "Sector PravasiList Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorPravasiReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid Format!!"
                });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(FileBytes)
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attacchment")
            {
                FileName=fileName
            };
            return ResponseMessage(result);

        }

        [HttpGet]
        [Route("api/sector/NewVoterExcelPdfReportForSector")]
        public IHttpActionResult NewVoterReportForSector(string format, int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllNewvoterBySecIncId(SecIncId, filter);
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
            "S.No",  "Mandal Name", "Sector Name","Booth No.", "Polling Station","Name","Father Name","Category","Caste","Contact","Education","DOB","Age(Till 1 Jan 2027)"
         };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, NewVoterList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                 item.Data.BoothNo ?? "",
                item.Data.BoothName ?? "",
                //item.Data.BoothIncharge ?? "",
                item.Data.name ?? "",
                 item.Data.fathername ?? "",
                item.Data.Category ?? "",
                item.Data.Caste ?? "",
                //item.Data.Age.ToString(),
                item.Data.Mobile ?? "",
                item.Data.education ?? "",
                item.Data.dob ?? "",
                 item.Data.totalAge ?? "",
            };

            var generator = new ReportGenerator<(int Index, NewVoterList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Sector New Voter Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorNewVoter.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Sector New Voter Report", mapFunc);
                ContentType= "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorNewVoterReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format!!"
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
        [Route("api/sector/PrabhavshaliExcelPdfReport")]
        public IHttpActionResult PrabhavshaliReport(string format,int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License =  QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllEffectivePersonBySecIncId(SecIncId, filter);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to genearte report!!"
                });
            }

            var headers = new string[]
           {
            "S.No",  "Mandal Name", "Sector Name","Booth No.", "Polling Station","Name","Category","Caste","Contact","Description","Designation",
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, EffectivePersonList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                 item.Data.BoothNo ?? "",
                item.Data.BoothName ?? "",
                //item.Data.BoothIncharge ?? "",
                item.Data.Name ?? "",
                 //item.Data.fathername ?? "",
                item.Data.Category ?? "",
                item.Data.Caste ?? "",
                //item.Data.Age.ToString(),
                item.Data.Mobile ?? "",
                item.Data.Description ?? "",
                item.Data.Designation ?? "",

            };
            var generator = new ReportGenerator<(int Index, EffectivePersonList Data)>();
            byte[] Filebytes;
            string ContentType;
            string filename;
            if(format=="pdf")
            {
                Filebytes = generator.ExportToPdf(indexedData, headers, "Sector Prabhavshali List Report", mapFunc);
                ContentType = "application/pdf";
                filename = "SectorPrabhavshaliReport.pdf";
            }
            else if(format=="excel")
            {
                Filebytes = generator.ExportToExcel(indexedData, headers, "Sector Prabhavshali List Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                filename = "SectorPrabhavshhaliReport.xlsx";

            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format!!"
                });
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(Filebytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            return ResponseMessage(result);


        }
        [HttpGet]
        [Route("api/sector/DoubleVoterExcelPdfReport")]
        public IHttpActionResult DoubleVoterReportExcelPdf(string format, int SecIncId, [FromUri] FilterModel filterModel)
        {
            filterModel = filterModel ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllDoublevoteBySecIncId(SecIncId, filterModel);
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
            "S.No",  "Mandal Name", "Sector Name","Booth No.", "Polling Station","Village","Name","Father Name","Voter Id","Current Address","Previous Address","Reason"
          };

            var indexData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, DoubleVoteList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                 item.Data.BoothNo ?? "",
                item.Data.BoothName ?? "",
                item.Data.Village ?? "",
                item.Data.Name ?? "",
                item.Data.FatherName ?? "",
                  item.Data.voterid ?? "",
                item.Data.currentAdd ?? "",
                item.Data.preAdd ?? "",
                //item.Data.Age.ToString(),
                item.Data.reason ?? "",


            };

            var generator = new ReportGenerator<(int Index, DoubleVoteList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexData, headers, "Sector DoubleVoteList Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorDoubleVoteReport.pdf";
            }
            else if (format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexData, headers, "Sector DoubleVoteList Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorDoubleVoteReport.xlsx";

            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid Format!!"
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
        [Route("api/sector/SahmatExcelPdfReportForSector")]
        public IHttpActionResult SahamtListExcelReport(string format,int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllSatisfiedBySecIncId(SecIncId, filter);
            if(reportData==null || !reportData.Any())
            {
                return Ok(new
                {
                    status=true,
                    message="No data to generate report!! "
                });
            }
            var headers = new string[]
           {
            "S.No", "Mandal Name", "Sector Name","Booth No.", "Polling Station","Village","Name","Age","Contact","Party"
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.mandalname ?? "",
                item.Data.sectorname ?? "",
                 item.Data.boothno ?? "",
                item.Data.boothname ?? "",
                item.Data.village ?? "",
                item.Data.name ?? "",
                item.Data.age.ToString(),
                
                //item.Data.Age.ToString(),
                item.Data.mobile ?? "",
                 item.Data.party ?? "",


            };

            var generator = new ReportGenerator<(int Index, SatisfiedList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Sector SahamatVoteList Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorSahmatVoteReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Sector SahmantVoteList Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorDoubleVoteReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format!!"
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
        [Route("api/sector/AsahmatExcelPdfReportForSector")]
        public IHttpActionResult AsahmatExcelPdfReport(string format,int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData=_sectorService.GetAllUnSatisfiedBySecIncId(SecIncId,filter);
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
            "S.No", "Mandal Name", "Sector Name","Booth No.", "Polling Station","Village","Name","Age","Contact","Party","Reason"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.mandalname ?? "",
                item.Data.sectorname ?? "",
                 item.Data.boothno ?? "",
                item.Data.boothname ?? "",
                item.Data.village ?? "",
                item.Data.name ?? "",
                item.Data.age.ToString(),
                
                //item.Data.Age.ToString(),
                item.Data.mobile ?? "",
                 item.Data.party ?? "",
                 item.Data.reason ?? ""


            };

            var generator = new ReportGenerator<(int Index, SatisfiedList Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if (format == "pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Sector AsahmatList Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorAsahmatList.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Sector AsahmatList Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorAsahmatList.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format!!"
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
        [Route("api/sector/SeniorCitizenExcelPdfReportForSector")]
        public IHttpActionResult SeniorCitizenReport(string format, int SecIncId, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetAllSeniorCitizenBySecIncId(SecIncId,filter);
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
            "S.No", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Category", "Caste", "Address","Contact",
           };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.Village ?? "",
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
                FileBytes = generator.ExportToPdf(indexedData, headers, "Sector VarishthNagrikList Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorVarishthNagrikReport.pdf";

            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Sector VarishthNagrik Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorVarishthNagrikReport.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format!!"
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
        [Route("api/sector/BoothVoterExcelPdfReportForSector")]
        public IHttpActionResult BoothVoterExcelPdfReport(string format,int SecIncId, [FromUri] FilterModel filter )
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var reportData = _sectorService.GetBoothVoterdesBySectorId(SecIncId, filter);
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
            "S.No",  "Mandal Name", "Sector Name", "Polling Station","Booth No.", "Total Voters","Total Male","Total Female", "Total Others",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BoothVotersDes Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.BoothNumber ?? "",
                item.Data.TotalVoters.ToString(),
                item.Data.TotalMan.ToString(),
                item.Data.TotalWoman.ToString(),
                item.Data.TotalOther.ToString(),

            };
            var generator = new ReportGenerator<(int Index, BoothVotersDes Data)>();
            byte[] fileBytes;
            string ContentType;
            string filename;
            if(format=="pdf")
            {
                fileBytes = generator.ExportToPdf(indexedData, headers, "Sector BoothVoterDescription Report", mapFunc);
                ContentType = "application/pdf";
                filename = "SectorBoothVoterDescription.pdf";
            }
            else if(format=="excel")
            {
                fileBytes = generator.ExportToExcel(indexedData, headers, "Sector BoothVoterDescription Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                filename = "SectorBoothVoterDescription.xlsx";
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
                Content = new ByteArrayContent(fileBytes)
            };
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = filename
            };
            return ResponseMessage(result);

        }

        [HttpGet]
        [Route("api/sector/PannaPramukhExcelPdfBySectorId")]
        public IHttpActionResult PannaPramukhExcelPdfBySectorId(int SectorId,string format, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var data = _sectorService.GetPannaBySectorId(SectorId, filter);
            if(data==null || !data.Any())
            {
                return Ok(new
                {
                    status=false,
                    message="No data to generate report!!"
                });
            }
            var headers = new string[]
           {
            "S.No", "Mandal", "Sector Name", "Panna Pramukh", "Panna Number", "Village","Contact","Voter Id", "Caste","Address","Image",
           };

            var indexedData = data.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PannaPramukh Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.Pannapramukh ?? "",
                item.Data.PannaNumber ?? "",
                item.Data.village ?? "",
                item.Data.Mobile ?? "",
                item.Data.VoterNumber ?? "",
                item.Data.Castename ?? "",
                item.Data.Address ?? "",
                item.Data.ProfileImageUrl ?? "",
            };

            var generator = new ReportGenerator<(int Index, PannaPramukh Data)>();
            byte[] FileBytes;
            string ContentType;
            string fileName;

            if(format=="pdf")
            {
                FileBytes = generator.ExportToPdf(indexedData, headers, "Sector PannaPramukh Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorPannaPramukh.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Sector PannaPramukh Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorPannaPramukh.xlsx";
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format!!"
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
        [Route("api/sector/DisabledExcelPdfReportbySectorId")]
        public IHttpActionResult DisabledExcelPdfReportbySectorId(int SectorId, string format, [FromUri] FilterModel filter)
        {
            filter = filter ?? new FilterModel();
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var data = _sectorService.GetAllHandiCapBySecIncId(SectorId, filter);
            if (data == null || !data.Any())
            {
                return Ok(new
                {
                    status = true,
                    message = "No data to generate report!!"
                });
            }
            var headers = new string[]
          {
            "S.No", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Category", "Caste", "Address","Contact",
          };

            var indexedData = data.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.Village ?? "",
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
                FileBytes = generator.ExportToPdf(indexedData, headers, "Sector Viklaang Report", mapFunc);
                ContentType = "application/pdf";
                fileName = "SectorViklaangReport.pdf";
            }
            else if(format=="excel")
            {
                FileBytes = generator.ExportToExcel(indexedData, headers, "Sector Viklaang Report", mapFunc);
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = "SectorViklaangReport.xlsx";
                 
            }
            else
            {
                return Ok(new
                {
                    status=false,
                    message="Invalid format!!"
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
