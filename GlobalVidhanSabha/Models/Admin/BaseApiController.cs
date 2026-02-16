using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace GlobalVidhanSabha.Models.AdminMain
{
    public class BaseApiController : ApiController
    {
        protected IHttpActionResult ApiOk<T>(T data, string message = "Data fetched successfully")
        {
            return Ok(new
            {
                code = "200",
                success = true,
                message,
                data
            });
        }

        protected IHttpActionResult ApiCreated<T>(T data, string message = "Resource created successfully")
        {
            return Content(HttpStatusCode.Created, new
            {
                code = "201",
                success = true,
                message,
                data
            });
        }

        protected IHttpActionResult ApiUpdated<T>(T data, string message = "Resource updated successfully")
        {
            return Ok(new
            {
                code = "200",
                success = true,
                message,
                data
            });
        }

        protected IHttpActionResult ApiBadRequest(string message = "Invalid request")
        {
            return Content(HttpStatusCode.BadRequest, new
            {
                code = "400",
                success = false,
                message
            });
        }

        protected IHttpActionResult ApiNotFound(string message = "Data not found")
        {
            return Content(HttpStatusCode.NotFound, new
            {
                code = "404",
                success = false,
                message
            });
        }

        protected IHttpActionResult ApiInternalServerError(Exception ex)
        {
            return Content(HttpStatusCode.InternalServerError, new
            {
                code = "500",
                success = false,
                message = "Internal Server Error",
                error = ex.Message
            });
        }

        // =========================
        // GET / SINGLE RESULT
        // =========================
        protected async Task<IHttpActionResult> ProcessRequestAsync<T>(
      Func<Task<T>> serviceCall)
        {
            try
            {
                var result = await serviceCall();

                if (result == null)
                    return ApiNotFound();

                return ApiOk(result);
            }
            catch (ArgumentException ex)
            {
                return ApiBadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return ApiInternalServerError(ex);
            }
        }

        protected async Task<IHttpActionResult> ProcessPagedRequestAsync<T>(
    Func<Task<PagedResult<T>>> serviceCall)
        {
            try
            {
                var result = await serviceCall();

                if (result == null || result.data == null)
                    return ApiNotFound("No data found");

                return ApiOk(result);
            }
            catch (Exception ex)
            {
                return ApiInternalServerError(ex);
            }
        }



        // =========================
        // CREATE / UPDATE
        // =========================
        protected async Task<IHttpActionResult> ProcessCreateOrUpdateAsync<T>(
            Func<Task<T>> serviceCall, bool isUpdate)
        {
            try
            {
                var result = await serviceCall();

                if (result == null)
                {
                    return isUpdate
                        ? ApiNotFound("Entity not found for update.")
                        : ApiBadRequest("Creation failed.");
                }

                if (result is bool && !(bool)(object)result)
                {
                    return ApiBadRequest(isUpdate ? "Update failed." : "Creation failed.");
                }

                return isUpdate
                    ? ApiUpdated(result, "Updated successfully.")
                    : ApiCreated(result, "Created successfully.");
            }
            catch (Exception ex)
            {
                return ApiInternalServerError(ex);
            }
        }

        protected IHttpActionResult ApiDeleted(string message = "Deleted successfully")
        {
            return Ok(new
            {
                code = "200",
                success = true,
                message
            });
        }

        protected async Task<IHttpActionResult> ProcessDeleteAsync(Func<Task<bool>> serviceCall)
        {
            try
            {
                bool result = await serviceCall();

                if (!result)
                    return ApiNotFound("Entity not found for delete.");

                return ApiDeleted();
            }
            catch (Exception ex)
            {
                return ApiInternalServerError(ex);
            }
        }


    }
}
