using GlobalVidhanSabha.Models.AdminMain;
using GlobalVidhanSabha.Models.SamithiMember;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.SqlClient;
using System.IO;
using System.Net.NetworkInformation;
using System.Web;


using static GlobalVidhanSabha.Models.SamithiMember.VidhanSabhaModel;

[RoutePrefix("api/samithimember")]
public class SamithiMemberController : BaseApiController
{
    private readonly ISamithiMemberService _service;

    public SamithiMemberController()
    {
        _service = new SamithiMemberService();
    }
    //[HttpPost]
    //[Route("add")]
    //public async Task<IHttpActionResult> Add([FromBody] SamithiMemberModel member)
    //{
    //    return await ProcessCreateOrUpdateAsync(async () =>
    //    {
    //        return await _service.AddMemberAsync(member);
    //    });
    //}

    private int? GetInt(string value)
    {
        if (int.TryParse(value, out int result))
            return result;

        return null;
    }

    [HttpPost]
    [Route("save")]
    public async Task<IHttpActionResult> Save()
    {
        try
        {
            var request = HttpContext.Current.Request;

            SamithiMemberModel model = new SamithiMemberModel
            {
                Id = GetInt(request.Form["Id"]),
                DesignationType = GetInt(request.Form["DesignationType"]),
                SamithiMember = request.Form["SamithiMember"],
                Email = request.Form["Email"],
                PhoneNo = request.Form["PhoneNo"],
                Category = GetInt(request.Form["Category"]),
                Caste = GetInt(request.Form["Caste"]),
                Education = request.Form["Education"],
                Profession = request.Form["Profession"],
                Address = request.Form["Address"]
            };

            // FILE UPLOAD
            if (request.Files.Count > 0)
            {
                var file = request.Files["Profile"];

                if (file != null && file.ContentLength > 0)
                {
                    string folder = HttpContext.Current.Server.MapPath("~/Uploads/Profile");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    string fullPath = Path.Combine(folder, fileName);

                    file.SaveAs(fullPath);

                    model.ProfilePath = "/Uploads/Profile/" + fileName;
                }
            }

            bool isUpdate = model.Id.HasValue && model.Id > 0;

            return await ProcessCreateOrUpdateAsync(async () =>
            {
                int result = await _service.SaveMemberAsync(model);

                return result > 0 ? result : (int?)null;

            }, isUpdate);
        }
        catch (Exception ex)
        {
            return InternalServerError(ex);
        }
    }



    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IHttpActionResult> Delete(int id)
    {
        return await ProcessDeleteAsync(async () =>
        {
            int result = await _service.DeleteMemberAsync(id);

            return result > 0; // convert to bool
        });
    }


    [HttpGet]
    [Route("getall")]
    public async Task<IHttpActionResult> GetAll()
    {

        return await ProcessRequestAsync(async () =>
        {
            return await _service.GetAllMembersAsync();
        });
    }
  


    [HttpGet]
    [Route("get/{id}")]
    public async Task<IHttpActionResult> GetById(int id)
    {    
        return await ProcessRequestAsync(async () =>
        {
            return await _service.GetMemberByIdAsync(id);
        });
    }
}
