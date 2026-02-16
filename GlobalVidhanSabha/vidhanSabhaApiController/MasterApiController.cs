using GlobalVidhanSabha.Models.AdminMain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GlobalVidhanSabha.vidhanSabhaApiController
{
    [RoutePrefix("api/Admin")]
    public class MasterApiController : BaseApiController
    {
        private readonly IAdminService _service;
       

        public MasterApiController()
        {
            _service = new AdminService();
            
        }

        [HttpGet]
        [Route("getAllDesignation")]
        public async Task<IHttpActionResult> GetAll([FromUri] Pagination paging)
        {
            return await ProcessRequestAsync(async () =>
            {
                var list = await _service.GetAllDesignationsAsync(paging);
                return list;
            });
        }    

        [HttpGet]
        [Route("getDesignationByid")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetDesignationByIdAsync(id); 
            });
        }


        [HttpPost]
        [Route("SaveDesignation")]
        public async Task<IHttpActionResult> SaveDesignation([FromBody] designationMain model)
        {
            if (model == null)
                return ApiBadRequest("Invalid designation data");

            bool isUpdate = model.DesignationId > 0;

            return await ProcessCreateOrUpdateAsync(async () =>
            {
                int id = await _service.SaveDesignationAsync(model); 
                return id > 0 ? id : (int?)null;
            }, isUpdate);
        }

        [HttpDelete]
        [Route("DeleteDesignation")]
        public async Task<IHttpActionResult> DeleteDesignation(int id)
        {
            return await ProcessDeleteAsync(async () =>
            {
                return await _service.DeleteDesignationAsync(id);
            });
        }




        //=========================================================================

        [HttpGet]
        [Route("getAllStateCount")]
        public async Task<IHttpActionResult> GetAllStateCount([FromUri] Pagination paging)
       {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetAllStateCountsAsync(paging);
            });
        }

        [HttpGet]
        [Route("getByIdStateCount")]
        public async Task<IHttpActionResult> GetByIdStateCount(int id)
        {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetStateCountByIdAsync(id);
            });
        }

        [HttpPost]
        [Route("saveStateCount")]
        public async Task<IHttpActionResult> Save([FromBody] stateCountMain model)
        {
            if (model == null)
                return ApiBadRequest("Invalid data");

            bool isUpdate = model.Id > 0;

            return await ProcessCreateOrUpdateAsync(async () =>
            {
                int id = await _service.SaveStateCountAsync(model);
                return id > 0 ? id : (int?)null;
            }, isUpdate);
        }

        [HttpDelete]
        [Route("deleteStateCount")]
        public async Task<IHttpActionResult> Delete(int StateId)
        {
            return await ProcessDeleteAsync(async () =>
            {
                return await _service.DeleteStateCountAsync(StateId);
            });
        }

        [HttpGet]
        [Route("getAllState")]
        public async Task<IHttpActionResult> GetAllState()
        {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetAllStateAsync();
            });
        }
        //=====================

        [HttpPost]
        [Route("SaveDistict")]
        public async Task<IHttpActionResult> SaveDistict([FromBody] DistrictCountModel model)
        {
            if (model == null)
                return ApiBadRequest("Invalid District data");

            try
            {
                int id = await _service.SaveDistictAsync(model);

                return Ok(new
                {
                    success = true,
                    message = model.Id == 0 ? "District added successfully!" : "District updated successfully!",
                    data = id
                });
            }
            catch (Exception ex)
            {
                // This will send RAISERROR message to client
                return Ok(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }


        [HttpDelete]
        [Route("deleteDistict")]
        public async Task<IHttpActionResult> DeleteDistict(int DistrictId)
        {
            return await ProcessDeleteAsync(async () =>
            {
                return await _service.DeleteDistictAsync(DistrictId);
            });
        }

        [HttpGet]
        [Route("getDistictById")]
        public async Task<IHttpActionResult> GetDistictById(int id)
        {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetDistictByIdAsync(id);
            });
        }

        [HttpGet]
        [Route("getAllDistict")]
        public async Task<IHttpActionResult> GetAllDistict([FromUri] Pagination paging)
        {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetAllDistictAsync(paging);
            });
        }

       

        [HttpGet]
        [Route("getDistrictsByState")]
        public async Task<IHttpActionResult> GetDistrictsByState(int stateId)
        {
            return await ProcessRequestAsync(async () =>
            {
                var districts = await _service.GetDistrictsByStateAsync(stateId);
                return new { success = true, data = districts }; // NO double nesting
            });
        }

        [HttpGet]
        [Route("GetCasteCategory")]
        public async Task<IHttpActionResult> GetCasteCategory()
        {
            return await ProcessRequestAsync(async () =>
            {
                var data = await _service.GetCasteCategoryAsync();
                return new { success = true, data = data };
            });
        }

        [HttpGet]
        [Route("GetSubCasteByCategory")]
        public async Task<IHttpActionResult> GetSubCasteByCategory(int categoryId)
        {
            return await ProcessRequestAsync(async () =>
            {
                var data = await _service.GetSubCasteByCategoryAsync(categoryId);
                return new { success = true, data = data };
            });


        }

        [HttpGet]
        [Route("getAllVidhanSabha")]
        public async Task<IHttpActionResult> GetAllVidhanSabha([FromUri] Pagination paging)
        {

            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetAllVidhanSabhaAsync(paging);
            });
        }



        [HttpPost]
        [Route("SaveVidhanSabhaRegistration")]
        public async Task<IHttpActionResult> SaveVidhanSabhaRegistration()
        {
            var request = HttpContext.Current.Request;

            var model = new VidhanSabhaRegister
            {
                Id = int.TryParse(request.Form["Id"], out var id) ? id : 0,
                VidhanSabhaName = request.Form["VidhanSabhaName"],
                Prabhari = request.Form["Prabhari"] == "true",

                Name = request.Form["Name"],
                Email = request.Form["Email"],
                PhoneNo = request.Form["PhoneNo"],
                Education = request.Form["Education"],
                Address = request.Form["Address"],
                Profession = request.Form["Profession"],

                StateId = int.TryParse(request.Form["StateId"], out var stateId) ? stateId : 0,
                DistrictId = int.TryParse(request.Form["DistrictId"], out var districtId) ? districtId : 0,


                Category = int.TryParse(request.Form["Category"], out var cat) ? cat : (int?)null,
                Caste = int.TryParse(request.Form["Caste"], out var caste) ? caste : (int?)null,
                Status = true
            };

            model.Username = model.VidhanSabhaName.Substring(0, 4) + model.PhoneNo.Substring(model.PhoneNo.Length - 4);
            model.Password = Guid.NewGuid().ToString().Substring(0, 8);


            // ===== FILE HANDLE =====
            if (request.Files.Count > 0)
            {
                var file = request.Files["Profile"];
                if (file != null && file.ContentLength > 0)
                {
                    string folder = HttpContext.Current.Server.MapPath("~/Uploads/VidhanSabha/");
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    file.SaveAs(Path.Combine(folder, fileName));

                    model.Profile = "/Uploads/VidhanSabha/" + fileName;
                }
            }

            bool isUpdate = model.Id > 0;

            int savedId = await _service.SaveVidhanSabhaRegistrationAsync(model);

            if (savedId > 0)
                return Ok(new { success = true, id = savedId });

            return BadRequest("Save failed");
        }

        [HttpGet]
        [Route("getVidhanSabhaById")]
        public async Task<IHttpActionResult> getVidhanSabhaById(int id)
        {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetVidhanSabhaByIdAsync(id);
            });
        }


        [HttpDelete]
        [Route("DeleteVidhanSabha")]
        public async Task<IHttpActionResult> DeleteVidhanSabha(int id)
        {
            return await ProcessDeleteAsync(async () =>
            {
                return await _service.DeleteVidhanSabhaAsync(id);
            });
        }

      

        //[HttpGet]
        //[Route("getDistrictByStateCount")]
        //public async Task<IHttpActionResult> getDistrictByStateCount(int stateId)
        //{
        //    return await ProcessRequestAsync(async () =>
        //    {
        //        return await _service.GetAllDistrictsDataByStateId(stateId);
        //   });

        

        //}

        [HttpGet]
        [Route("getDistrictByStateCount")]
        public async Task<IHttpActionResult> getDistrictByStateCount(int stateId)
        {
            return await ProcessRequestAsync(async () =>
            {
                var districts = await _service.GetAllDistrictsDataByStateId(stateId);

                return new
                {
                    data = districts,
                    totalRecords = districts.Count
                };
            });
        }

        [HttpGet]
        [Route("GetDashboardCounts")]
        public async Task<IHttpActionResult> GetDashboardCounts()
        {
            return await ProcessRequestAsync(async () =>
            {
                var data = await _service.GetDashboardCountsAsync();
                return new { success = true, data = data };
            });
        }

        [HttpGet]
        [Route("getVidhanSabhaByState")]
        public async Task<IHttpActionResult> GetVidhanSabhaByState([FromUri]  int DistrictId, [FromUri] Pagination paging)
        {
            //return await ProcessRequestAsync(async () =>
            //{
            //    var data = await _service.GetVidhanSabhaByStateIdAsync(stateId, paging);

            //     return new { success = true, data = data };
            //});

            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetVidhanSabhaByStateIdAsync(DistrictId, paging);
            });
        }

        [HttpGet]
        [Route("GetStateWiseVidhanSabhaChart")]
        public async Task<IHttpActionResult> GetStateWiseVidhanSabhaChart()
        {
            return await ProcessRequestAsync(async () =>
            {
                return await _service.GetStateWiseVidhanSabhaChartAsync();
               
            });
        }

        [HttpGet]
        [Route("GetDistrictWiseVidhanSabhaChart")]
        public async Task<IHttpActionResult> GetDistrictWiseVidhanSabhaChart()
        {
            return await ProcessRequestAsync(async () =>
            {
                return await  _service.GetDistrictWiseVidhanSabhaChartAsync();
              
            });
        }




    }
}
