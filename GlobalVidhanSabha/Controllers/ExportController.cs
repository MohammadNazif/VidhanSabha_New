using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuestPDF.Helpers;
using VishanSabha.Models;
using VishanSabha.Services;
using VishanSabha.Services.SectorService;

namespace VishanSabha.Controllers
{
    public class ExportController : Controller
    {
        AdminServices service = new AdminServices();
        BoothService Boothservice = new BoothService();
        SectorService sectorservice = new SectorService();

        #region Admin Reports

        //Combined Mandal Report 
        public ActionResult CombinedMandalExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetCombinedReport(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
        "Booth No.", "Mandal", "Polling Station", "Sector Name", "Sector Sanyojak", "Booth Adhyaksh",
        "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education"
            };

            Func<Booth, List<string>> mapFunc = booth => new List<string>
    {
        booth.BoothNumber?.ToString() ?? "",
        //booth.PollingStationBoothName?.ToString() ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(reportData.ToList(), headers, "Combined Mandal Reports", mapFunc);
                return File(pdfBytes, "application/pdf", "CombinedMandalExportReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(reportData.ToList(), headers, "Combined Mandal Reports", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CombinedMandalExportReport.xlsx");
            }

            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }


        public ActionResult CombinedSectorExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            // ✅ Get all sector + booth combined data in one call
            var combinedList = service.GetAll_SectorDetails(filter);

            if (combinedList == null || !combinedList.Any())
                return HttpNotFound("No combined data found.");

            // ✅ Define headers for export
            var headers = new string[]
            {
        "S.N.", "Mandal", "Sector", "Sector Sanyojak", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education",
        "Booth No", "Polling Station", "Booth Adhyaksh", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education"
            };

            // ✅ Create indexed data list for export
            var indexedData = combinedList.Select((data, index) => (Index: index + 1, Data: data)).ToList();

            // ✅ Map data to export columns
            Func<(int Index, Sector Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.MandalName ?? "",
        item.Data.SectorName ?? "",
        item.Data.InchargeName ?? "",
        item.Data.PhoneNumber ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.FatherName ?? "",
        item.Data.Age.HasValue ? item.Data.Age.ToString() : "",
        item.Data.subcaste ?? "",
        item.Data.Address ?? "",
        item.Data.Education ?? "",

        item.Data.BoothNumber ?? "",
        item.Data.BoothName ?? "",
        item.Data.BoothIncharge ?? "",
        item.Data.BoothPhone ?? "",
        string.Join(", ", item.Data.BoothVillageNames ?? new List<string>()),
        item.Data.BoothFatherName ?? "",
        item.Data.BoothAge > 0 ? item.Data.BoothAge.ToString() : "",
        item.Data.BoothCaste ?? "",
        item.Data.BoothAddress ?? "",
        item.Data.BoothEducation ?? ""
    };

            // ✅ Use common report generator
            var generator = new ReportGenerator<(int Index, Sector Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Combined Sector Reports", mapFunc);
                return File(pdfBytes, "application/pdf", "CombinedSectorExportReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Combined Sector Reports", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CombinedSectorExportReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }


        //public ActionResult CombinedSectorExportReport(string format, FilterModel filter)
        //{
        //    QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

        //    var sectors = service.GetAll_SectorDetails(filter);
        //    var combinedList = new List<CombinedSectorBoothReport>();

        //    foreach (var sector in sectors)
        //    {
        //        var booths = service.GetBoothslistbySectorId(sector.Id);

        //        // Get only the first booth (if any)
        //        var booth = booths?.FirstOrDefault();

        //        combinedList.Add(new CombinedSectorBoothReport
        //        {
        //            // Sector data
        //            MandalName = sector.MandalName,
        //            SectorName = sector.SectorName,
        //            PollingStationBoothName = sector.PollingStationBoothName,
        //            InchargeName = sector.InchargeName,
        //            SectorPhoneNumber = sector.PhoneNumber,
        //            SectorVillageNames = sector.VillageNames,
        //            SectorFatherName = sector.FatherName,
        //            SectorAge = sector.Age,
        //            SectorCaste = sector.subcaste,
        //            SectorAddress = sector.Address,
        //            SectorEducation = sector.Education,
        //            SectorProfileImage = sector.ProfileImage,

        //            // Booth data (only first booth or blank if none)
        //            BoothNumber = booth?.BoothNo,
        //            BoothName = booth?.BoothName,
        //            BoothIncharge = booth?.InchargeName,
        //            BoothPhone = booth?.Mobile,
        //            BoothVillageNames = booth?.VillageNames,
        //            BoothFatherName = booth?.FatherName,
        //            BoothAge = booth?.Age,
        //            BoothCaste = booth?.SubCasteName,
        //            BoothAddress = booth?.Address,
        //            BoothEducation = booth?.Education,
        //            BoothProfileImage = booth?.ProfileImage
        //        });
        //    }


        //    if (!combinedList.Any())
        //        return HttpNotFound("No combined data found.");

        //    var headers = new string[]
        //    {
        //    "S.N.", "Mandal", "Sector", "Sector Sanyojak", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education",
        //    "Booth No","Booth Name", "Polling Station", "Booth Adhyaksh", "Contact", "Village", "Father Name", "Age", "Caste", "Address", "Education"
        //    };

        //    var indexedData = combinedList.Select((data, index) => (Index: index + 1, Data: data)).ToList();

        //    Func<(int Index, CombinedSectorBoothReport Data), List<string>> mapFunc = item => new List<string>
        //{
        //    item.Index.ToString(),
        //    item.Data.MandalName ?? "",
        //    item.Data.SectorName ?? "",
        //    item.Data.InchargeName ?? "",
        //    item.Data.SectorPhoneNumber ?? "",
        //    string.Join(", ", item.Data.SectorVillageNames ?? new List<string>()),
        //    item.Data.SectorFatherName ?? "",
        //    item.Data.SectorAge?.ToString() ?? "",
        //    item.Data.SectorCaste ?? "",
        //    item.Data.SectorAddress ?? "",
        //    item.Data.SectorEducation ?? "",

        //    item.Data.BoothNumber ?? "",
        //    item.Data.PollingStationBoothName ?? "",
        //    item.Data.BoothName ?? "",
        //    item.Data.BoothIncharge ?? "",
        //    item.Data.BoothPhone ?? "",
        //    string.Join(", ", item.Data.BoothVillageNames ?? new List<string>()),
        //    item.Data.BoothFatherName ?? "",
        //    item.Data.BoothAge?.ToString() ?? "",
        //    item.Data.BoothCaste ?? "",
        //    item.Data.BoothAddress ?? "",
        //    item.Data.BoothEducation ?? ""
        //};

        //    var generator = new ReportGenerator<(int Index, CombinedSectorBoothReport Data)>();

        //    if (format == "pdf")
        //    {
        //        var pdfBytes = generator.ExportToPdf(indexedData, headers, "Combined Sector Reports", mapFunc);
        //        return File(pdfBytes, "application/pdf", "CombinedSectorExportReport.pdf");
        //    }
        //    else if (format == "excel")
        //    {
        //        var excelBytes = generator.ExportToExcel(indexedData, headers, "Combined Sector Reports", mapFunc);
        //        return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CombinedSectorExportReport.xlsx");
        //    }
        //    else
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
        //    }
        //}

        public ActionResult PrabhavsaliExportList( string format, FilterModel filter,
       int? type,
       int? pageNumber,
       int? pageSize)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var allData = service.GetEffectivePersonsPaged(filter,
                type,
            pageNumber,
                pageSize);
            var filteredData = allData;
           


            if (filteredData == null || !filteredData.Any())
                return HttpNotFound("No data found for export.");

            // Headers
            var headers = new string[]
            {
        "S.N.", "Booth No.","Sector","Sector Sanyojak", "Village", "Type","Name","Caste", "Contact", "Description",
            };

            var indexedData = filteredData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, EffectivePerson Data), List<string>> mapFunc = item => new List<string>
{
    item.Index.ToString(),
    item.Data?.BoothNumber.ToString() ?? "",
    item.Data?.SectorName.ToString() ?? "",
    item.Data?.SectorIncharge.ToString() ?? "",
    //item.Data?.PollingStationBoothName ?? "",
    item.Data?.village ?? "",
    item.Data?.Designationdata ?? "",
    item.Data?.Name ?? "",
    item.Data?.Castename?.ToString() ?? "",
    item.Data?.Mobile ?? "",
    item.Data?.Description ?? ""
};


            var Data = filteredData
      .Where(x => x.Designation == type.ToString())
      .Select(x => x.Designationdata)
      .FirstOrDefault();
            var generator = new ReportGenerator<(int Index, EffectivePerson Data)>();
            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, $"{Data} List", mapFunc);
                return File(pdfBytes, "application/pdf", $"{Data}_List.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, $"{Data} List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{Data}_List.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        //mandalreport

        public ActionResult MandalExportReport(string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetMandalReport();

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define headers (including S.No)
            var headers = new string[]
            {
        "S.N", "Mandal Name", "Total Sectors", "Total Booths", "Total Voters",
        "Senior Count", "Disabled Count", "Double Votes",
        "Pravasi Count", "Effective Person"
            };

            // Add index
            var indexedData = reportData.Select((data, index) => (Index: index + 1, Data: data)).ToList();

            // Map data to string list for report
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

            // Use correct generic type
            var generator = new ReportGenerator<(int Index, Mandal Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Mandal Report", mapFunc);
                return File(pdfBytes, "application/pdf", "MandalReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Mandal Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MandalReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }


        //PravasiVoter report

        public ActionResult PravasiVoterExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetPravasiVoterReport(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers (include S.No.)
            var headers = new string[]
            {
        "S.No", "Booth No.","Booth Name", "Booth Adhyaksh", "Pravasi Name", "Category", "Caste", "Contact", "Occupation", "Current Address", "Sector", "Sector Incharge"
            };

            // Add indexing to data
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function with S.No.
            Func<(int Index, PravasiVoter Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber ?? "",
        item.Data.PollingStationBoothName ?? "",
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

            // Use generic ReportGenerator
            var generator = new ReportGenerator<(int Index, PravasiVoter Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Pravasi Voter Report", mapFunc);
                return File(pdfBytes, "application/pdf", "PravasiVoterReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Pravasi Voter Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PravasiVoterReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }
        public ActionResult HandicapExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetHandicapReport(filter);

            if (reportData == null || !reportData.Any())
            {
                // Data nahi mila, error message TempData me bheje view ke liye
                TempData["ErrorMessage"] = "No data found to generate report.";
                return RedirectToAction("HandicapReports"); // Redirect to report listing page ya jaha aap alert dikhana chahte hain
            }

            // Headers, map function waqera aapka jo hai wahi rahega
            var headers = new string[]
            {
        "S.N.", "Booth No.","Booth Name", "Booth Adhyaksh", "Village", "Name", "Contact", "Handicap", "Caste", "Address", "Sector", "Sector Incharge"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorOrDisabled Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNo.ToString(),
        item.Data.PollingStationBoothName.ToString()??"",
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

            var generator = new ReportGenerator<(int Index, SeniorOrDisabled Data)>();
            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Handicap Report", mapFunc);

                // Add header to force download
                Response.AppendHeader("Content-Disposition", "attachment; filename=HandicapReport.pdf");

                // Return file with binary content type
                return File(pdfBytes, "application/octet-stream");
            }


            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Handicap Report", mapFunc);

                TempData["SuccessMessage"] = "Excel report generated successfully.";

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HandicapReport.xlsx");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid format requested.";
                return RedirectToAction("HandicapReports");
            }
        }


        //EffectivePersongExportReport pdf excel
        public ActionResult EffectivePersongExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetEffectivePersonReport(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
        "S.N.", "Booth No.","Booth Name", "Booth Adhyaksh", "Village","Designation", "Name", "Contact","Category", "Caste", "Description", "Sector", "Sector Incharge"
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, EffectivePerson Data), List<string>> mapFunc = item => new List<string>
    {
       item.Index.ToString(),
        item.Data.BoothNumber.ToString(),
        item.Data.PollingStationBoothName.ToString()??"",
        item.Data.BoothIncharge ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.Designation ?? "",
        item.Data.Name ?? "",
        item.Data.Mobile ?? "",
        item.Data.Category ?? "",
        item.Data.Castename ?? "",
        item.Data.Description ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""
    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, EffectivePerson Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Effective Persong Report", mapFunc);
                return File(pdfBytes, "application/pdf", "EffectivePersongExportReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Handicap Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EffectivePersongExportReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        //SectorExportReport PDF EXCEL

        public ActionResult SectorExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetAll_SectorDetails(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
        "S.N.", "Sector",  "Sector Sanyojak", "Contact","Caste", "Village", "Total Booths","Total Voters", "Senior Citizen", "Handicap", "Double Votes","Pravasi",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, Sector Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.SectorName.ToString(),
        item.Data.InchargeName ?? "",
        item.Data.PhoneNumber ?? "",
        item.Data.SubCasteName ?? "",
        string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.TotalBooth .ToString(),
        item.Data.TotalVotes .ToString(),
        item.Data.SeniorCitizen .ToString(),
        item.Data.Handicap.ToString(),
        item.Data.DoubleVotes.ToString(),
        item.Data.Pravasi.ToString()

    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, Sector Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Sector Report", mapFunc);
                return File(pdfBytes, "application/pdf", "SectorReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Handicap Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SectorReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        //BoothExportReport PDF EXCEL

        public ActionResult BoothExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetBoothReport(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
            "Booth No.", "Polling Station", "Booth Adhyaksh", "Contact", "Village","Caste", "Total Voters", "Senior Citizen", "Handicap", "Double Votes","Pravasi",
            };

            // Create indexed data for serial numberS
            Func<Booth, List<string>> mapFunc = item => new List<string>
    {

        item.BoothNo.ToString(),
        //item.PollingStationBoothName.ToString()??"",
        item.BoothName ?? "",
        item.InchargeName ?? "",
        item.Mobile ?? "",
        item.Village ?? "",
        //string.Join(", ", item.VillageNames ?? new List<string>()),
        item.castname ?? "",
        //item.SectorName ?? "",
        //  item.SectorIncName ?? "",
        item.TotalVotes .ToString(),
        item.SeniorCitizen .ToString(),
        item.Handicap.ToString(),
        item.DoubleVotes.ToString(),
        item.Pravasi.ToString()

    };

            var generator = new ReportGenerator<Booth>();


            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(reportData.ToList(), headers, "Booth Reports", mapFunc);
                return File(pdfBytes, "application/pdf", "BoothReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(reportData.ToList(), headers, "Booth Reports", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoothReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        //DoubleVoterExportReport


        public ActionResult DoubleVoterExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetDoubleVoterReport(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");


            var headers = new string[]
            {
        "S.N.", "Booth No.", "Booth Adhyaksh","Vilage", "Name","Father Name", "Voter Id","Current Address", "Previous Address", "Reason", "Sector","Sector Sanyojak",
            };


            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();


            Func<(int Index, doubleVoter Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber.ToString(),
        //item.Data.PollingStationBoothName.ToString()??"",
        item.Data.BoothIncharge ?? "",
         string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.name ?? "",
        item.Data.fathername ?? "",
        item.Data.voterno .ToString(),
        item.Data.currentAddress ?? "",
        item.Data.pastAddress ?? "",
        item.Data.reason ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""

    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, doubleVoter Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Sector Report", mapFunc);
                return File(pdfBytes, "application/pdf", "DoubleVoterReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Double Voter Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DoubleVoterReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        //NewVoteraExportReport

        public ActionResult NewVoteraExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetNewVoterReport(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
        "S.N.","Booth No.","Booth Name","Booth Adhyaksh","Vilage","Voter Name","Father Name","Contact","Category","Caste", "DOB","Age Till(1-jan-2027)","Education","Sector","Sector Sanyojak",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, NewVoters Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNo.ToString(),
        item.Data.PollingStationBoothName.ToString()??"",
        item.Data.BoothIncharge ?? "",
         string.Join(", ", item.Data.VillageNames ?? new List<string>()),
        item.Data.VoterName ?? "",
        item.Data.FatherName ?? "",
        item.Data.MobileNumber .ToString(),
        item.Data.Category ?? "",
        item.Data.caste ?? "",
        item.Data.dateofbirth.ToString("dd-MM-yyyy") ?? "",
        item.Data.totalage ?? "",
        item.Data.Education ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? ""

    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, NewVoters Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "New Voters Report", mapFunc);
                return File(pdfBytes, "application/pdf", "NewVotersReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Handicap Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NewVotersReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        //SeniorCitizenExportReport

        public ActionResult SeniorCitizenExportReport(string format,FilterModel filter,int? limit,int? page)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetSeniorCitizenReport(filter,limit,page);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
        "S.N.","Booth No.","Booth Name","Booth Adhyaksh","Village"," Name","Contact","SeniorCitizen","Caste", "Address","Sector","Sector Sanyojak",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, SeniorOrDisabled Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNo.ToString(),
        item.Data.PollingStationBoothName.ToString()??"",
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

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, SeniorOrDisabled Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Varishth Nagrik Reports", mapFunc);
                return File(pdfBytes, "application/pdf", "SeniorCitizenReport.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Varishth Nagrik Reports", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", " SeniorCitizenReport.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        #region List excel pdf

        //List (PannaPramukh/EXCEL)
        public ActionResult PravasiVoterListExportReport(string format, FilterModel filter)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetAllPravasiVoterData(VidhanSabhaId,filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
        "S.N.","Sector Name","Sector Sanyojak","Booth No.","Village"," Name","Category","Caste", "Contact","Current Address","Occupation",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, PravasiVoter Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
          item.Data.SectorName??"",
        item.Data.SectorIncharge??"",
        item.Data.BoothNumber.ToString(),
        item.Data.VillageListName??"",
      
        //item.Data.PollingStationBoothName.ToString()??"",
        item.Data.name ?? "",
        item.Data.CategoryName ?? "",
        item.Data.CasteName .ToString(),
        item.Data.mobile ?? "",
        item.Data.currentAddress ?? "",
        item.Data.Occupation ?? "",
    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, PravasiVoter Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Pravasi Voter List", mapFunc);
                return File(pdfBytes, "application/pdf", "PravasiVoterList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Pravasi Voter List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", " PravasiVoterList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult NewVoterListExportReport(string format, FilterModel filter,int? limit,int? page)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetNewVoters(VidhanSabhaId, filter,limit,page);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
        "S.N.","Booth No.","Village","Sector Name","Sector Sanyojak"," Name","Father Name","Contact","Category","Caste", "DOB","Age Till (1-Jan-2027)",
            };

            // Create indexed data for serial numberS
            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, NewVoters Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber.ToString(),
        item.Data.Village.ToString(),
        item.Data.SectorName.ToString(),
        item.Data.SectorIncharge.ToString(),
        //item.Data.village,

        item.Data.Name ?? "",
        item.Data.FatherName .ToString(),
        item.Data.MobileNumber ?? "",
        item.Data.CategoryName ?? "",
        item.Data.caste ?? "",
        item.Data.DOB != null ? item.Data.DOB.Value.ToString("dd/MM/yyyy") : "",
        item.Data.totalage ?? "",

    };

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, NewVoters Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Pravasi Voter List", mapFunc);
                return File(pdfBytes, "application/pdf", "PravasiVoterList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Pravasi Voter List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", " PravasiVoterList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

    //    public ActionResult SatisfiedUnsatisfiedListExport(string type, string format)
    //    {
    //        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

    //        var allData = service.getAllDataSatisfiedUnsatisfied();
    //        var filteredData = allData.Where(x => x.sahmatAsahmatName == type).Select(x => x.name).FirstOrDefault();

    //        if (filteredData == null || !filteredData.Any())
    //            return HttpNotFound("No data found for export.");

    //        var headers = new string[]
    //        {
    //    "S.N.", "Booth No", "Village", "Type", "Name", "Age", "Contact", "Party", "Reason", "Occupation"
    //        };

    //        var indexedData = filteredData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

    //        Func<(int Index, SatisfiedUnSatisfied Data), List<string>> mapFunc = item => new List<string>
    //{
    //    item.Index.ToString(),
    //    item.Data.boothNo.ToString() ?? "",
    //    item.Data.village ?? "",
    //    item.Data.sahmatAsahmatName ?? "",
    //    item.Data.name ?? "",
    //    item.Data.age.ToString(),
    //    item.Data.mobile ?? "",
    //    item.Data.party ?? "",
    //    item.Data.reason ?? "" ,
    //    item.Data.Occupation ?? ""
    //};

    //        var generator = new ReportGenerator<(int Index, SatisfiedUnSatisfied Data)>();

    //        if (format == "pdf")
    //        {
    //            var pdfBytes = generator.ExportToPdf(indexedData, headers, $"{filteredData} Voter List", mapFunc);
    //            return File(pdfBytes, "application/pdf", $"{type}_VoterList.pdf");
    //        }
    //        else if (format == "excel")
    //        {
    //            var excelBytes = generator.ExportToExcel(indexedData, headers, $"{filteredData} Voter List", mapFunc);
    //            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{type}_VoterList.xlsx");
    //        }
    //        else
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
    //        }
    //    }


        public ActionResult BlockPramukhExportList(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var reportData = service.GetAllBlocks(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            // Define column headers
            var headers = new string[]
            {
        "S.N.","Block Name.","Block Pramukh"," Contact","Caste","Address","Occupation",
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

            // Create report generator for tuple
            var generator = new ReportGenerator<(int Index, Block Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "BlockPramukh List", mapFunc);
                return File(pdfBytes, "application/pdf", "BlockPramukhList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "BlockPramukh List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", " BlockPramukhList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }


        public ActionResult InfluencerListExcelPdf(string format, FilterModel filter,
int? limit = null, int? page = null)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            var data = service.GetInfluencers(filter,limit,page);
            if (data == null || !data.Any())
                return HttpNotFound("No data to generate report");

            var headers = new string[]
              {
            "S.N.","Booth No.","Designation"," Name","Caste","Contact","Description",
              };
            // Create indexed data for serial numberS
            var indexedData = data.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            // Define mapping function that includes the S.N.
            Func<(int Index, Influencer Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber?.ToString() ?? "",
                //item.Data.PollingStationBoothName?.ToString() ?? "",
                item.Data.EffectiveDesignationdata ?? "",
                item.Data.PersonName?.ToString() ?? "",
                item.Data.SubCasteName ?? "",
                (item.Data.Mobile ?? 0L).ToString(),
                item.Data.Description?.ToString() ?? ""
                // item.Data.ProfileImage ?? ""
            };
            var generator = new ReportGenerator<(int Index, Influencer Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Influencer List", mapFunc);
                return File(pdfBytes, "application/pdf", "InfluencerList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Influencer List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", " InfluencerList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult ExportSeniorOrDisabledList( string format, FilterModel filter, int? type, int? limit = null, int? page = null)
        {
         
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            var allData = service.GetSeniorOrDisabled(VidhanSabhaId, filter, type,limit,page);
            // if SeniorOrDisabledStatus is int but type comes as string
       //     var filteredData = allData
       //.Where(x => x.SeniorOrDisableddata != null && x.SeniorOrDisabledStatus.Trim() == type.ToString().Trim())
       //.ToList();

       //     var filteredType = filteredData.Select(x => x.SeniorOrDisabledType).FirstOrDefault();

       //     ;


            if (allData == null || !allData.Any())
                return HttpNotFound("No data found for export.");

            // Headers
            var headers = new string[]
            {
        "S.N.", "Booth No.","Village","Sector Name","Sector Sanyonjak", "Type","Name", "Contact", "Category","Caste","Adress"
            };

            var indexedData = allData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorOrDisabled Data), List<string>> mapFunc = item => new List<string>
    {
        item.Index.ToString(),
        item.Data.BoothNumber.ToString()?? "",
        //item.Data.PollingStationBoothName.ToString()?? "",
        item.Data.village ?? "",
        item.Data.SectorName ?? "",
        item.Data.SectorIncharge ?? "",
        item.Data.SeniorOrDisabledStatus ?? "",
        item.Data.Name ?? "",
        item.Data.Mobile ?? "",
        item.Data.CategoryName ?? "",
        item.Data.SubCasteName ?? "",
        item.Data.Address ?? ""
    };

            var generator = new ReportGenerator<(int Index, SeniorOrDisabled Data)>();

            var reportTitle = $"{(type == 1 ? "Senior" : "Disabled")} List Report";

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, reportTitle, mapFunc);
                var fileName = $"{reportTitle}.pdf";
                return File(pdfBytes, "application/pdf", fileName);
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, reportTitle, mapFunc);
                var fileName = $"{reportTitle}.xlsx";
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }


        public ActionResult SahmatAhsamatdList(int? type, string format, FilterModel filter, int? limit=null, int? page=null)
        {
            int VidhanSabhaId = Convert.ToInt32(Session["VidhanSabhaId"]);
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            var allData = service.GetAllDataSatisfiedUnsatisfied(VidhanSabhaId,type, filter,limit,page );



            if (allData == null || !allData.Any())
                return HttpNotFound("No data found for export.");

            // Headers
            var headers = new string[]
            {
        "S.N.", "Booth No.","Village", "Type","Name","Age", "Contact", "Party","Reason","Occupation"
            };

            var indexedData = allData.Select((item, index) => (Index: index + 1, Data: item)).ToList();
            Func<(int Index, SatisfiedUnSatisfied Data), List<string>> mapFunc = item => new List<string>
{
    item.Index.ToString(),
    item.Data?.boothNo.ToString() ?? "",
    //item.Data?.PollingStationBoothName ?? "",
    item.Data?.village ?? "",
    item.Data?.sahmatAsahmatName ?? "",
    item.Data?.name ?? "",
    item.Data?.age.ToString() ?? "",
    item.Data?.mobile ?? "",
    item.Data?.party ?? "",
    item.Data?.reason ?? "",
    item.Data?.Occupation ?? ""
};



            var generator = new ReportGenerator<(int Index, SatisfiedUnSatisfied Data)>();
            var Type = service.getSahmatAsahmatType();

            var filteredTypes = allData
      .Where(x => x.sahmatAsahmat == type)
      .Select(x => x.sahmatAsahmatName).FirstOrDefault();


            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, $"{filteredTypes} List Report", mapFunc);
                return File(pdfBytes, "application/pdf", $"{filteredTypes}List.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, $"{filteredTypes}List Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{filteredTypes}List.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }



  

        #endregion
        #endregion

        #region Booth Reports
        public ActionResult BoothDesiabledExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.GetAllHandiCapByBoothIncId(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Category", "Caste", "Address","Current Address", "Contact",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Booth Vikalaag Report", mapFunc);
                return File(pdfBytes, "BoothVikalaag/pdf", "BoothVikalaag.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Booth Vikalaag Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoothVikalaag.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothVoterDesExportReport(string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.getBoothVoterDesByBoothIncId(boothInchargeId);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Total Voters", "Total Man","Total Woman" , "Total Other"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BoothVotersDes Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                //item.Data.PollingStationBoothName ?? "",
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothName ?? "",
                item.Data.TotalVoters.ToString(),
                item.Data.TotalMan.ToString() ,
                item.Data.TotalWoman.ToString(),
                item.Data.TotalOther.ToString(),
             
            };

            var generator = new ReportGenerator<(int Index, BoothVotersDes Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, " BoothVoter Description Report", mapFunc);
                return File(pdfBytes, "BoothVikalaag/pdf", "BoothVoterDescription.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "BoothVoter Description Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoothVoterDescription.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult PannaPramukhExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.GetallPannaListByBoothIncId(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Panna No.","Panna Pramukh", "Category ","Caste" , "Voter Id","Village" , "Address","Mobile"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PannaPramukh Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Panna Pramukh Report", mapFunc);
                return File(pdfBytes, "BoothVikalaag/pdf", "PannaPramukh.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Panna Pramukh Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PannaPramukh.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothNewVoterExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.GetAllNewvoterByBoothIncId(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name", "Father Name ","Category" , "Caste","Contact" , "DOB","Total Age"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, NewVoterList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNo ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, " New Voters Report", mapFunc);
                return File(pdfBytes, "NewVoters/pdf", " NewVoters.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "New Voters Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", " NewVoters.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothPravasiExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.GetAllTotalPravasiList(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Pravasi Name","Category", "Caste ","Contact" , "Current Address","Occupation" , 
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PravasiVoter Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Pravasi Voters Report", mapFunc);
                return File(pdfBytes, " PravasiVoters/pdf", " PravasiVoters.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Pravasi Voters Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "  PravasiVoters.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothSahamatExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.getallsahmatlist(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name", "age ","Contact","Party","Occupation" ,
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedUnSatisfied Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.boothNo.ToString(),
                //item.Data.PollingStationBoothName.ToString(),
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Sahamat Report", mapFunc);
                return File(pdfBytes, " Sahamat/pdf", " Sahamat.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Sahamat Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "  Sahamat.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothAsahamatExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.getallAsahmatlist(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name", "age ","Contact","Party","Reason","Occupation" ,
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedUnSatisfied Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.boothNo.ToString(),
                //item.Data.PollingStationBoothName.ToString(),
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Asahamat Report", mapFunc);
                return File(pdfBytes, " Asahamat/pdf", " Asahamat.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Asahamat Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "  Asahamat.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothDoubleVotersExportReport(string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.getalldoubleVoterByBoothIncId(boothInchargeId);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Father Name","Voter Id","Current Address","Previous Address" ,"Reason",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, doubleVoter Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber.ToString(),
                //item.Data.PollingStationBoothName.ToString(),
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Double Voter/Married Report", mapFunc);
                return File(pdfBytes, " doubleVoterMarried/pdf", " doubleVoterMarried.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Double Voter/Married Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "  doubleVoterMarried.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothSeniorSitizenExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.GetAllSeniorCitizenByBoothIncId(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Category","Caste ","Address ","Mobile" ,
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno.ToString(),
                //item.Data.PollingStationBoothName.ToString(),
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Varishth Nagrik Report", mapFunc);
                return File(pdfBytes, " VarishthNagrik/pdf", " VarishthNagrik.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Varishth Nagrik Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "  VarishthNagrik.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothSamithiExportReport(string format, FilterModel filter,
int? limit = null, int? page = null)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.GetAllBoothSamitiByBoothIncId(boothInchargeId, filter,limit,page);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.","Mandal Name", "Sector Name", "Polling Station", "Village","Name","Designation","Category","Caste","Age ","Contact ","Occupation" ,
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BoothSamiti Data), List<string>> mapFunc = item => new List<string>
            {
               item.Index.ToString(),
                item.Data.BoothNumber.ToString(),
                //item.Data.PollingStationBoothName.ToString(),
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Booth Samithi Report", mapFunc);
                return File(pdfBytes, " BoothSamithi/pdf", " BoothSamithi.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Booth Samithi Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoothSamithi.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult BoothPrabhavsaliExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"].ToString();
            int boothInchargeId = Boothservice.GetBoothInchargeId(contact);
            var reportData = Boothservice.GetAllPrabhavsaliByBoothIncId(boothInchargeId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.N.","Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Name","Category","Caste","Contact ","Description","Designation" ,
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, EffectivePersonList Data), List<string>> mapFunc = item => new List<string>
            {
               item.Index.ToString(),
                item.Data.BoothNo.ToString(),
                //item.Data.PollingStationBoothName.ToString(),
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Prabhavsali Vyakti Report", mapFunc);
                return File(pdfBytes, " Prabhavsali/pdf", " PrabhavsaliVyakti.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Prabhavsali Vyakti Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PrabhavsaliVyakti.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }



        #endregion

        #region Sector Nazif
        public ActionResult sectorDesiabledExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllHandiCapBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Category", "Caste", "Address","Contact",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Sector Vikalaag Report", mapFunc);
                return File(pdfBytes, "SectorVikalaag/pdf", "SectorVikalaag.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Sector Vikalaag Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SectorVikalaag.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorBoothExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetBoothsBySectorId(sectorId,filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Booth Adhyaksh","Father Name", "Caste","Contact","Education",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, Booth Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.BoothNumber ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Sector Booths", mapFunc);
                return File(pdfBytes, "SectorBooth/pdf", "SectorBooth.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Sector Booth", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SectorBooth.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorPannaPramukhExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetPannaBySectorId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No", "Mandal", "Sector Name", "Booth Number","Polling station","Panna Pramukh", "Panna Number", "Village","Contact","Voter Id", "Caste","Address","Image",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PannaPramukh Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                item.Data.BoothNumber ?? "",
                //item.Data.PollingStationBoothName ?? "",
                item.Data.BoothName ?? "",
                item.Data.Pannapramukh ?? "",
                item.Data.PannaNumber ?? "",
                item.Data.Villages ?? "",
                item.Data.Mobile ?? "",
                item.Data.VoterNumber ?? "",
                item.Data.SubCasteName ?? "",
                item.Data.Address ?? "",
                item.Data.ProfileImageUrl ?? "",
            };
            var generator = new ReportGenerator<(int Index, PannaPramukh Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "PannaPramukh Report", mapFunc);
                return File(pdfBytes, "PannaPramukh/pdf", "PannaPramukh.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "PannaPramukh", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PannaPramukh.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorBoothSamitiExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllBoothSamitiBySectorIncId(sectorId,filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No",  "Mandal Name", "Sector Name","Booth No.", "Polling Station","Booth Adhyaksh","Name","Designation", "Category","Caste","Age","Contact","Occupation",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BoothSamiti Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
               
                item.Data.MandalName ?? "",
                item.Data.sectorName ?? "",
                 item.Data.BoothNumber ?? "",
                 item.Data.BoothName ?? "",
                item.Data.BoothIncharge ?? "",
                //item.Data.PollingStationBoothName ?? "",
                //item.Data.BoothName ?? "",
                item.Data.Name ?? "",
                item.Data.Designation ?? "",
                item.Data.CategoryName ?? "",
                item.Data.SubCasteName ?? "",
                item.Data.Age.ToString(),
                item.Data.Mobile ?? "",
                item.Data.Occupation ?? "",
            };
            var generator = new ReportGenerator<(int Index, BoothSamiti Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "BoothSamiti List", mapFunc);
                return File(pdfBytes, "BoothSamiti/pdf", "BoothSamiti.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "BoothSamiti List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoothSamiti.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorPravasiExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllPravasiBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No",  "Mandal Name", "Sector Name","Booth No.","Polling Station","Pravasi Name","Category","Caste","Contact","Current Add.","Occupation",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, PravasiList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                 item.Data.BoothNo ?? "",
                 //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "PravasiVoter List", mapFunc);
                return File(pdfBytes, "PravasiVoter/pdf", "PravasiVoter.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "PravasiVoter List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PravasiVoter.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorNewVotersExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllNewvoterBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

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
                //item.Data.PollingStationBoothName ?? "",
                item.Data.BoothName ?? "",
                //item.Data.BoothIncharge ?? "",
                item.Data.name ?? "",
                 item.Data.fathername ?? "",
                item.Data.Category ?? "",
                item.Data.Caste ?? "",
                //item.Data.Age.ToString(),
                item.Data.Mobile ?? "",
                item.Data.education ?? "",
                @DateTime.Parse(item.Data.dob).ToString("yyyy-MM-dd"),
                item.Data.totalAge ?? "",
            };
            var generator = new ReportGenerator<(int Index, NewVoterList Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "NewVoter List", mapFunc);
                return File(pdfBytes, "NewVoterList/pdf", "NewVoterList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "NewVoter List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NewVoterList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorPrabhausaliExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllEffectivePersonBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No",  "Mandal Name", "Sector Name","Booth No.",  "Polling Station","Name","Category","Caste","Contact","Description","Designation",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, EffectivePersonList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                 item.Data.BoothNo ?? "",
                 //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "EffectivePerson List", mapFunc);
                return File(pdfBytes, "EffectivePersonList/pdf", "EffectivePersonList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "EffectivePerson List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NewVoterList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }
        public ActionResult sectorDoubleVoterExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllDoublevoteBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No",  "Mandal Name", "Sector Name","Booth No.", "Polling Station","Village","Name","Father Name","Voter Id","Current Address","Previous Address","Reason"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, DoubleVoteList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.MandalName ?? "",
                item.Data.SectorName ?? "",
                 item.Data.BoothNo ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "DoubleVoter List", mapFunc);
                return File(pdfBytes, "DoubleVoterList/pdf", "DoubleVoterList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "DoubleVoter List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DoubleVoterList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorSahamatExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllSatisfiedBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No", "Mandal Name", "Sector Name","Booth No.","Polling Station","Village","Name","Age","Contact","Party"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.mandalname ?? "",
                item.Data.sectorname ?? "",
                 item.Data.boothno ?? "",
                //item.Data.PollingStationBoothName ?? "",
                item.Data.boothname ?? "",
                item.Data.village ?? "",
                item.Data.name ?? "",
                item.Data.age.ToString(),
                
                //item.Data.Age.ToString(),
                item.Data.mobile ?? "",
                 item.Data.party ?? "",


            };
            var generator = new ReportGenerator<(int Index, SatisfiedList Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Sahamat List", mapFunc);
                return File(pdfBytes, "SahamatList/pdf", "SahamatList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Sahamat List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SahamatList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }

        public ActionResult sectorAsahamatExportReport(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllUnSatisfiedBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No", "Mandal Name", "Sector Name","Booth No.","Polling Station","Village","Name","Age","Contact","Party","Reason"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SatisfiedList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),

                item.Data.mandalname ?? "",
                item.Data.sectorname ?? "",
                 item.Data.boothno ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Asahamat List", mapFunc);
                return File(pdfBytes, "AsahamatList/pdf", "AsahamatList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Asahamat List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "AsahamatList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }
        public ActionResult sectorSeniorExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetAllSeniorCitizenBySecIncId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No", "Booth No.", "Mandal Name", "Sector Name", "Polling Station", "Village","Name","Category", "Caste", "Address","Contact",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, SeniorCitizenList Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Boothno ?? "",
                //item.Data.PollingStationBoothName ?? "",
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

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "SeniorCitizen List", mapFunc);
                return File(pdfBytes, "SeniorCitizenList/pdf", "SeniorCitizenList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Sector Vikalaag Report", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SeniorCitizenList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }
        public ActionResult sectorBoothVoterExportReport(string format, FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            string contact = Session["Contact"]?.ToString();
            int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = sectorservice.GetBoothVoterdesBySectorId(sectorId, filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

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
                //item.Data.PollingStationBoothName ?? "",
                item.Data.TotalVoters.ToString(),
                item.Data.TotalMan.ToString(),
                item.Data.TotalWoman.ToString(),
                item.Data.TotalOther.ToString(),
            
            };
            var generator = new ReportGenerator<(int Index, BoothVotersDes Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "BoothVotersDes List", mapFunc);
                return File(pdfBytes, "BoothVotersDesList/pdf", "BoothVotersDesList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "BoothVotersDes List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoothVotersDes.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }




        public ActionResult PradhanListForExcelPdf(string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            //string contact = Session["Contact"]?.ToString();
            //int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = service.GetAllPradhan();

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No",  "Pradhan Name", "Village", "Gender", "Contact",
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, Pradhan Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.Name ?? "",
                item.Data.Village_Name ?? "",
                item.Data.GenderName ?? "",
                item.Data.Contact ?? "",
              

            };
            var generator = new ReportGenerator<(int Index, Pradhan Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Pradhan List", mapFunc);
                return File(pdfBytes, "PradhanList/pdf", "PradhanList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "BoothVotersDes List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PradhanList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }


        public ActionResult BDCListForExcelPdf(string format)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            //string contact = Session["Contact"]?.ToString();
            //int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = service.GetBDC();

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No",  "Ward No.", "Name", "Village", "Contact","Age","Caste","Education","Block","Party","ProfileImage"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, BDC Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.WardNumber.ToString(),
                item.Data.Name ?? "",
                item.Data.Village ?? "",
                item.Data.Contact ?? "",
                item.Data.Age.ToString(),
                item.Data.CasteName ?? "",
                item.Data.Education ?? "",
                item.Data.BlockName ?? "",
                item.Data.partyName ?? "",
                item.Data.ProfileImage ?? "",


            };
            var generator = new ReportGenerator<(int Index, BDC Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "BDC List", mapFunc);
                return File(pdfBytes, "BDCList/pdf", "BDCList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "BoothVotersDes List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BDCList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }



        public ActionResult BoothListForExcelPdf(string format,FilterModel filter)
        {
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

            //string contact = Session["Contact"]?.ToString();
            //int sectorId = Convert.ToInt32(sectorservice.GetsectorInchargeId(contact));
            var reportData = service.GetBoothList(filter);

            if (reportData == null || !reportData.Any())
                return HttpNotFound("No data to generate report.");

            var headers = new string[]
            {
            "S.No",  "Mandal Name", "Sector Name", "Booth No.", "Polling Station","Booth adhyaksh","Contact","Village","Caste"
            };

            var indexedData = reportData.Select((item, index) => (Index: index + 1, Data: item)).ToList();

            Func<(int Index, Booth Data), List<string>> mapFunc = item => new List<string>
            {
                item.Index.ToString(),
                item.Data.MandalName.ToString(),
                item.Data.SectorName ?? "",
                item.Data.BoothNumber ?? "",
                item.Data.BoothName ?? "",
                item.Data.InchargeName.ToString(),
                item.Data.PhoneNumber ?? "",
                string.Join(", ", item.Data.VillageNames ?? new List<string>()),
                item.Data.SubCasteName ?? ""


            };
            var generator = new ReportGenerator<(int Index, Booth Data)>();

            if (format == "pdf")
            {
                var pdfBytes = generator.ExportToPdf(indexedData, headers, "Booth List", mapFunc);
                return File(pdfBytes, "BoothList/pdf", "BoothList.pdf");
            }
            else if (format == "excel")
            {
                var excelBytes = generator.ExportToExcel(indexedData, headers, "Booth List", mapFunc);
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "BoothList.xlsx");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid format.");
            }
        }
        #endregion
    }
}