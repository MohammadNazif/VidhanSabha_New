using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace GlobalVidhanSabha.Models.AdminMain
{
    public class AdminService : IAdminService
    {
       
        private readonly string conn;
        private SqlConnection con;

        public AdminService()
        {
            conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        // CREATE / UPDATE

        public async Task<int> SaveDesignationAsync(designationMain model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", model.DesignationId > 0 ? "updateDesignation" : "addDesignation");
                        cmd.Parameters.AddWithValue("@DesignationId", model.DesignationId);

                    cmd.Parameters.AddWithValue("@DesignationName", model.DesignationName);
                    cmd.Parameters.AddWithValue("@DesignationType", model.DesignationType);

                    await con.OpenAsync();

                    object result = await cmd.ExecuteScalarAsync();

                    return model.DesignationId == 0
                        ? (result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0)
                        : model.DesignationId;
                }
            }
            catch (SqlException ex)
            {
                // Throw with exact SQL message
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // DELETE
      
        public async Task<bool> DeleteDesignationAsync(int designationId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@DesignationId", designationId);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting designation", ex);
            }
        }

      
        // GET BY ID   
        public async Task<designationMain> GetDesignationByIdAsync(int designationId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GETBYID");
                    cmd.Parameters.AddWithValue("@DesignationId", designationId);

                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync())
                            return null;

                        return new designationMain
                        {
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = dr["DesignationName"].ToString(),
                            DesignationType = dr["DesignationType"].ToString(),
                            Status = Convert.ToBoolean(dr["Status"]),
                            CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                            UpdateDate = dr["UpdateDate"] == DBNull.Value
                                            ? null
                                            : (DateTime?)Convert.ToDateTime(dr["UpdateDate"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching designation by id", ex);
            }
        }


     

        public async Task<PagedResult<designationMain>> GetAllDesignationsAsync(Pagination paging)
        {
            List<designationMain> list = new List<designationMain>();
            int totalRecords = 0;

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETALL");
                cmd.Parameters.AddWithValue("@PageNumber", paging.PageNumber);
                cmd.Parameters.AddWithValue("@PageSize", paging.Items);
                cmd.Parameters.AddWithValue("@Search",
                    string.IsNullOrWhiteSpace(paging.search) ? null : paging.search);

                await con.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    // TotalRecords
                    if (await dr.ReadAsync())
                        totalRecords = Convert.ToInt32(dr["TotalRecords"]);

                    // Next result
                    await dr.NextResultAsync();

                    while (await dr.ReadAsync())
                    {
                        list.Add(new designationMain
                        {
                            DesignationId = Convert.ToInt32(dr["DesignationId"]),
                            DesignationName = dr["DesignationName"].ToString(),
                            DesignationType = dr["DesignationType"].ToString()
                        });
                    }
                }
            }

            return new PagedResult<designationMain>
            {
                data = list,
                totalRecords = totalRecords
            };
        }


        //============================================================================


        public async Task<int> SaveStateCountAsync(stateCountMain model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", model.Id == 0 ? "Add" : "Update");
                    if (model.Id > 0)
                        cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@StateId", model.StateId);
                    cmd.Parameters.AddWithValue("@Count", model.Count);

                    await con.OpenAsync();

                    // If adding, get new Id, else return existing Id
                    object result = await cmd.ExecuteScalarAsync();
                    return model.Id == 0
                        ? (result != null ? Convert.ToInt32(result) : 0)
                        : model.Id;
                }
            }
            catch (SqlException ex)
            {
                // Throw with exact SQL message
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Delete state count by Id
        public async Task<bool> DeleteStateCountAsync(int StateId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Delete");
                    cmd.Parameters.AddWithValue("@StateId", StateId);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync(); // executes soft delete
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting state count", ex);
            }
        }


        // Get state count by Id
        public async Task<stateCountMain> GetStateCountByIdAsync(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetById");
                    cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync()) return null;

                        return new stateCountMain
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            StateName = dr["StateName"].ToString(),
                            Count = Convert.ToInt32(dr["Count"]),
                            CreatedDate = dr["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedDate"]) : (DateTime?)null,
                            UpdatedDate = dr["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(dr["UpdatedDate"]) : (DateTime?)null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching state count by id", ex);
            }
        }

        // Get all state counts

        public async Task<PagedResult<stateCountMain>> GetAllStateCountsAsync(Pagination paging)
        {
            int totalRecords = 0;
            List<stateCountMain> list = new List<stateCountMain>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetAll");
                    //cmd.Parameters.AddWithValue("@PageNumber", paging.PageNumber);
                    //cmd.Parameters.AddWithValue("@PageSize", paging.Items);
                    //cmd.Parameters.AddWithValue("@Search",
                    //    string.IsNullOrWhiteSpace(paging.search) ? null : paging.search);

                    // Nullable safe pagination
                    cmd.Parameters.Add("@PageNumber", SqlDbType.Int).Value =
                        paging?.PageNumber ?? (object)DBNull.Value;

                    cmd.Parameters.Add("@PageSize", SqlDbType.Int).Value =
                        paging?.Items ?? (object)DBNull.Value;

                    cmd.Parameters.AddWithValue("@Search",
    string.IsNullOrWhiteSpace(paging?.search)
        ? (object)DBNull.Value
        : paging.search);


                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            if (totalRecords == 0 && dr["TotalCount"] != DBNull.Value)
                                totalRecords = Convert.ToInt32(dr["TotalCount"]);

                            list.Add(new stateCountMain
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                StateId = Convert.ToInt32(dr["StateId"]),
                                StateName = dr["StateName"].ToString(),
                                Count = Convert.ToInt32(dr["Count"]),
                                CreatedDate = dr["CreatedDate"] != DBNull.Value
                                    ? Convert.ToDateTime(dr["CreatedDate"])
                                    : (DateTime?)null,

                                RemainingCount = Convert.ToInt32(dr["RemainingCount"])

                            });
                        }
                    }
                }

                return new PagedResult<stateCountMain>
                {
                    data = list,
                    totalRecords = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching state counts", ex);
            }
        }

        public async Task<List<State>> GetAllStateAsync()
        {
            List<State> list = new List<State>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetAllState");

                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(new State
                            {
                                StateId = Convert.ToInt32(dr["StateId"]),
                                StateName = dr["StateName"].ToString()
                            });
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching states", ex);
            }
        }


        public async Task<int> SaveDistictAsync(DistrictCountModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", model.Id == 0 ? "addDistict" : "UpdateDistict");
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@StateId", model.StateId);
                    cmd.Parameters.AddWithValue("@DistrictId", model.DistrictId);
                    cmd.Parameters.AddWithValue("@Count", model.Count);

                    await con.OpenAsync();

                    if (model.Id == 0)
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        await cmd.ExecuteNonQueryAsync();
                        return model.Id;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Throw with exact SQL message
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<bool> DeleteDistictAsync(int DistrictId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DeleteDistict");
                    cmd.Parameters.AddWithValue("@DistrictId", DistrictId);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting district count", ex);
            }
        }

        public async Task<DistrictCountModel> GetDistictByIdAsync(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetByIdDistict");
                    cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync()) return null;

                        return new DistrictCountModel
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            StateId = Convert.ToInt32(dr["StateId"]),
                            DistrictId = Convert.ToInt32(dr["DistrictId"]),
                            Count = Convert.ToInt32(dr["Count"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching district count by id", ex);
            }
        }

        public async Task<PagedResult<DistrictCountModel>> GetAllDistictAsync(Pagination paging)
        {
            int totalRecords = 0;
            List<DistrictCountModel> list = new List<DistrictCountModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetAllDistict");
                    cmd.Parameters.AddWithValue("@PageNumber", paging.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", paging.Items);
                    cmd.Parameters.AddWithValue("@Search", string.IsNullOrWhiteSpace(paging.search) ? null : paging.search);

                    await con.OpenAsync();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            if (totalRecords == 0 && dr["TotalCount"] != DBNull.Value)
                                totalRecords = Convert.ToInt32(dr["TotalCount"]);

                            list.Add(new DistrictCountModel
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                StateId = Convert.ToInt32(dr["StateId"]),
                                DistrictId = Convert.ToInt32(dr["DistrictId"]),
                                DistrictName = dr["DistrictName"].ToString(),
                                StateName = dr["StateName"].ToString(),
                                Count = Convert.ToInt32(dr["Count"]),

                                RemainingCount = Convert.ToInt32(dr["RemainingCount"])
                            });
                        }
                    }
                }

                return new PagedResult<DistrictCountModel>
                {
                    data = list,
                    totalRecords = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching all district counts", ex);
            }
        }

        public async Task<List<DistrictModel>> GetDistrictsByStateAsync(int stateId)
        {
            var list = new List<DistrictModel>();
            SqlConnection con = null;

            try
            {
                con = new SqlConnection(conn);
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetDistrictsByStateId");
                    cmd.Parameters.AddWithValue("@StateId", stateId);

                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            list.Add(new DistrictModel
                            {
                                DistrictId = Convert.ToInt32(dr["DistrictId"]),
                                DistrictName = dr["DistrictName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
          
                throw new Exception($"Error fetching districts for StateId {stateId}", ex);
            }
            

            return list;
        }

        public async Task<int> SaveVidhanSabhaRegistrationAsync(VidhanSabhaRegister model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    string generatedPassword = null;                  

                    if (model.Prabhari)
                    {
                        generatedPassword = PasswordGenerator.GeneratePassword(
                            model.Name,
                            model.PhoneNo
                        );
                    }
                    cmd.Parameters.AddWithValue("@Action",
                                 model.Id > 0 ? "UpdateVidhanSabhaRagitration" : "VidhanSabhaRagitration");
                    //cmd.Parameters.AddWithValue("@Action", "VidhanSabhaRagitration");
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@VidhanSabhaName", model.VidhanSabhaName);
                    cmd.Parameters.AddWithValue("@Prabhari", model.Prabhari);
                    cmd.Parameters.AddWithValue("@Name", (object)model.Name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PhoneNo", (object)model.PhoneNo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Category", (object)model.Category ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Caste", (object)model.Caste ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@StateId", model.StateId);
                    cmd.Parameters.AddWithValue("@DistrictId", model.DistrictId);
                    cmd.Parameters.AddWithValue("@Profile", (object)model.Profile ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Education", (object)model.Education ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", (object)model.Address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Profession", (object)model.Profession ?? DBNull.Value);
                    // FIX: Add this line
                    cmd.Parameters.AddWithValue("@UserName", (object)model.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Password", (object)generatedPassword ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Role", "Prabhari");

                    await con.OpenAsync();

                    int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                    if (model.Prabhari && newId > 0)
                    {
                        await EmailService.SendLoginEmailAsync(
                            model.Email,
                            model.Email,
                            generatedPassword
                        );
                    }

                    return newId;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving VidhanSabha: " + ex.Message);
            }
        }


        //public async Task<int> SaveVidhanSabhaRegistrationAsync(VidhanSabhaRegister model)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(conn))
        //        using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@Action",
        //                model.Id > 0 ? "UpdateVidhanSabhaRagitration" : "VidhanSabhaRagitration");

        //            cmd.Parameters.AddWithValue("@Id", model.Id);
        //            cmd.Parameters.AddWithValue("@VidhanSabhaName", model.VidhanSabhaName);
        //            cmd.Parameters.AddWithValue("@Prabhari", model.Prabhari);

        //            cmd.Parameters.AddWithValue("@Name", (object)model.Name ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@Email", (object)model.Email ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@PhoneNo", (object)model.PhoneNo ?? DBNull.Value);
        //            //cmd.Parameters.AddWithValue("@Category", (object)model.Category ?? DBNull.Value);
        //            //cmd.Parameters.AddWithValue("@Caste", (object)model.Caste ?? DBNull.Value);

        //            cmd.Parameters.Add("@Category", SqlDbType.Int)
        //               .Value = (object)model.Category ?? DBNull.Value; 

        //            cmd.Parameters.Add("@Caste", SqlDbType.Int)
        //                .Value = (object)model.Caste ?? DBNull.Value;

        //            cmd.Parameters.AddWithValue("@StateId", (object)model.StateId ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@DistrictId", (object)model.DistrictId ?? DBNull.Value);

        //            cmd.Parameters.AddWithValue("@Profile", (object)model.Profile ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@Education", (object)model.Education ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@Address", (object)model.Address ?? DBNull.Value);
        //            cmd.Parameters.AddWithValue("@Profession", (object)model.Profession ?? DBNull.Value);

        //            await con.OpenAsync();

        //            // 🔹 INSERT
        //            if (model.Id == 0)
        //            {
        //                object result = await cmd.ExecuteScalarAsync();

        //                if (result == null || result == DBNull.Value)
        //                    return 0;

        //                return Convert.ToInt32(result);
        //            }
        //            // 🔹 UPDATE
        //            else
        //            {
        //                await cmd.ExecuteNonQueryAsync();
        //                return model.Id;
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {

        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        // General error
        //        throw new Exception("Error while saving VidhanSabha registration: " + ex.Message);
        //    }
        //}

        public async Task<List<CasteCategory>> GetCasteCategoryAsync()
        {
            var list = new List<CasteCategory>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetCasteCategory");

                await con.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        list.Add(new CasteCategory
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["CategoryName"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public async Task<List<CasteCategory>> GetSubCasteByCategoryAsync(int categoryId)
        {
            var list = new List<CasteCategory>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetSubCasteByCategory");
                cmd.Parameters.AddWithValue("@categoryId", categoryId);

                await con.OpenAsync();
                using (var dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        list.Add(new CasteCategory
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = dr["SubCasteName"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public async Task<PagedResult<VidhanSabhaRegister>> GetAllVidhanSabhaAsync(Pagination paging, bool? Prabhari = null)
       {
            int totalRecords = 0;
            List<VidhanSabhaRegister> list = new List<VidhanSabhaRegister>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "getAllVidhanSabha");
                    cmd.Parameters.AddWithValue("@PageNumber", paging.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", paging.Items);
                    cmd.Parameters.AddWithValue("@Search",
                        string.IsNullOrWhiteSpace(paging.search) ? null : paging.search);

                    cmd.Parameters.Add("@Prabhari", SqlDbType.Bit)
                     .Value = (object)Prabhari ?? DBNull.Value;

                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            if (totalRecords == 0 && dr["TotalCount"] != DBNull.Value)
                                totalRecords = Convert.ToInt32(dr["TotalCount"]);

                            list.Add(new VidhanSabhaRegister
                            {
                                Id = Convert.ToInt32(dr["Id"]),

                                //StateId = Convert.ToInt32(dr["StateId"]),
                                //DistrictId = Convert.ToInt32(dr["DistrictId"]),

                                StateId = dr["StateId"] as int?,
                                DistrictId = dr["DistrictId"] as int?,

                                VidhanSabhaName = dr["VidhanSabhaName"].ToString(),
                                Prabhari = Convert.ToBoolean(dr["Prabhari"]),
                                Name = dr["Name"] as string,
                                Email = dr["Email"] as string,
                                PhoneNo = dr["PhoneNo"] as string,
                                Category = dr["Category"] as int?,
                                Caste = dr["Caste"] as int?,
                                Profile = dr["Profile"] as string,
                                Education = dr["Education"] as string,
                                Address = dr["Address"] as string,
                                Profession = dr["Profession"] as string,
                                CreatedAt = Convert.ToDateTime(dr["CreatedAt"]),
                                UpdatedAt = dr["UpdatedAt"] as DateTime?,
                                Status = Convert.ToBoolean(dr["Status"]),

                                DistrictName = dr["DistrictName"] as string,   
                                StateName = dr["StateName"] as string
                            });
                        }
                    }
                }

                return new PagedResult<VidhanSabhaRegister>
                {
                    data = list,
                    totalRecords = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching VidhanSabha list", ex);
            }
        }

        // GET VIDHANSABHA BY ID
        public async Task<VidhanSabhaRegister> GetVidhanSabhaByIdAsync(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetVidhanSabhaById");
                    cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        if (!await dr.ReadAsync())
                            return null;

                        return new VidhanSabhaRegister
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            VidhanSabhaName = dr["VidhanSabhaName"].ToString(),
                            Prabhari = Convert.ToBoolean(dr["Prabhari"]),
                            Name = dr["Name"] as string,
                            Email = dr["Email"] as string,
                            PhoneNo = dr["PhoneNo"] as string,

                            StateId = dr["StateId"] as int?,
                            DistrictId = dr["DistrictId"] as int?,

                            StateName = dr["StateName"] as string,
                            DistrictName = dr["DistrictName"] as string,

                            Category = dr["Category"] as int?,
                            CategoryName = dr["CategoryName"] as string,
                            Caste = dr["Caste"] as int?,
                            SubCasteName = dr["SubCasteName"] as string,

                            Profile = dr["UserProfile"] as string,
                            Education = dr["Education"] as string,
                            Address = dr["Address"] as string,
                            Profession = dr["Profession"] as string,
                            CreatedAt = Convert.ToDateTime(dr["CreatedAt"]),
                            UpdatedAt = dr["UpdatedAt"] as DateTime?,
                            Status = Convert.ToBoolean(dr["Status"])
                        };
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("SQL error while fetching VidhanSabha by ID: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while fetching VidhanSabha by ID: " + ex.Message, ex);
            }
        }


        // SOFT DELETE VIDHANSABHA
        public async Task<bool> DeleteVidhanSabhaAsync(int id)
        {
           

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DeleteVidhanSabha");
                    cmd.Parameters.AddWithValue("@Id", id);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("SQL error while deleting VidhanSabha: " + sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while deleting VidhanSabha: " + ex.Message, ex);
            }
           
        }

        public async Task<List<DistrictCountModel>> GetAllDistrictsDataByStateId(int stateId)
        {
            var districts = new List<DistrictCountModel>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_UserStateCount", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetAllDistrictsDataByStateId");
                    cmd.Parameters.AddWithValue("@StateId", stateId);

                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            districts.Add(new DistrictCountModel
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                StateId = Convert.ToInt32(dr["StateId"]),
                                DistrictId = Convert.ToInt32(dr["DistrictId"]),
                                DistrictName = dr["DistrictName"].ToString(),
                                StateName = dr["StateName"].ToString(),
                                Count = Convert.ToInt32(dr["Count"]),

                                RemainingCount= Convert.ToInt32(dr["RemainingCount"]),


                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log DB specific error
                throw new Exception("Database error while fetching districts by state.", ex);
            }
            catch (Exception ex)
            {
                // Log general error
                throw new Exception("Unexpected error occurred while fetching districts.", ex);
            }

            return districts;
        }

        public async Task<Dashboard> GetDashboardCountsAsync()
        {
            // ek hi object me combine kar rahe hain
            var dashboard = new Dashboard();

            using (SqlConnection con = new SqlConnection(conn))
            {
                await con.OpenAsync();

                // Total VidhanSabha
                using (SqlCommand cmd = new SqlCommand("usp_Dashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "TotalVidhanSabhaCount");
                    var result = await cmd.ExecuteScalarAsync();
                    dashboard.TotalVidhanSabhaCount = result != null ? Convert.ToInt32(result) : 0;
                }

                // With Prabhari
                using (SqlCommand cmd = new SqlCommand("usp_Dashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "TotalVidhanSabhaWithPrabhari");
                    var result = await cmd.ExecuteScalarAsync();
                    dashboard.TotalVidhanSabhaWithPrabhari = result != null ? Convert.ToInt32(result) : 0;
                }

                // Without Prabhari
                using (SqlCommand cmd = new SqlCommand("usp_Dashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "TotalVidhanSabhaWithoutPrabhari");
                    var result = await cmd.ExecuteScalarAsync();
                    dashboard.TotalVidhanSabhaWithoutPrabhari = result != null ? Convert.ToInt32(result) : 0;
                }

                using (SqlCommand cmd = new SqlCommand("usp_Dashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "TotalUsedStates");
                    var result = await cmd.ExecuteScalarAsync();
                    dashboard.TotalUsedStates = result != null ? Convert.ToInt32(result) : 0;
                }

                using (SqlCommand cmd = new SqlCommand("usp_Dashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "TotalUsedDistrict");
                    var result = await cmd.ExecuteScalarAsync();
                    dashboard.TotalUsedDistrict = result != null ? Convert.ToInt32(result) : 0;
                }
            }

            return dashboard;
        }

        public async Task<PagedResult<VidhanSabhaRegister>> GetVidhanSabhaByStateIdAsync(int DistrictId, Pagination paging)
        {
            int totalRecords = 0;
            var list = new List<VidhanSabhaRegister>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetVidhanSabhaByStateId");
                    cmd.Parameters.AddWithValue("@DistrictId", DistrictId);
                    cmd.Parameters.AddWithValue("@PageNumber", paging.PageNumber);
                    cmd.Parameters.AddWithValue("@PageSize", paging.Items);
                    cmd.Parameters.AddWithValue("@Search",
                        string.IsNullOrWhiteSpace(paging.search) ? null : paging.search);

                    await con.OpenAsync();

                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                        {
                            if (totalRecords == 0 && dr["TotalCount"] != DBNull.Value)
                                totalRecords = Convert.ToInt32(dr["TotalCount"]);

                            list.Add(new VidhanSabhaRegister
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                VidhanSabhaName = dr["VidhanSabhaName"].ToString(),
                                Prabhari = Convert.ToBoolean(dr["Prabhari"]),
                                Name = dr["Name"] as string,
                                Email = dr["Email"] as string,
                                PhoneNo = dr["PhoneNo"] as string,
                                Category = dr["Category"] as int?,
                                Caste = dr["Caste"] as int?,
                                Profile = dr["Profile"] as string,
                                Education = dr["Education"] as string,
                                Address = dr["Address"] as string,
                                Profession = dr["Profession"] as string,
                                CreatedAt = Convert.ToDateTime(dr["CreatedAt"]),
                                UpdatedAt = dr["UpdatedAt"] as DateTime?,
                                Status = Convert.ToBoolean(dr["Status"]),

                                DistrictName = dr["DistrictName"] as string,
                                StateName = dr["StateName"] as string
                            });
                        }
                        return new PagedResult<VidhanSabhaRegister>
                        {
                            data = list,
                            totalRecords = totalRecords
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error while fetching VidhanSabha by StateId", ex);
            }


        }

        public async Task<List<KeyValuePair<string, int>>> GetStateWiseVidhanSabhaChartAsync()
        {
            var list = new List<KeyValuePair<string, int>>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_Dashboard", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ChartDataGetStateWiseVidhanSabha");

                await con.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        list.Add(new KeyValuePair<string, int>(
                            dr["StateName"].ToString(),
                            Convert.ToInt32(dr["TotalVidhanSabha"])
                        ));
                    }
                }
            }
            return list;
        }

        public async Task<List<KeyValuePair<string, int>>> GetDistrictWiseVidhanSabhaChartAsync()
        {
            var list = new List<KeyValuePair<string, int>>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_Dashboard", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "ChartDataGetDistrictWiseVidhanSabha");

                await con.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        list.Add(new KeyValuePair<string, int>(
                            dr["DistrictName"].ToString(),
                            Convert.ToInt32(dr["TotalVidhanSabha"])
                        ));
                    }
                }
            }
            return list;
        }


        public async Task<List<designationMain>> GetDesignationTypeAsync()
        {
            var list = new List<designationMain>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_DesignationType_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetdesignationType");

                await con.OpenAsync();

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                {
                    while (await dr.ReadAsync())
                    {
                        list.Add(new designationMain
                        {
                            DesignationId = dr["DesignationId"] != DBNull.Value
                                ? Convert.ToInt32(dr["DesignationId"]) : 0,                         

                            DesignationType = dr["DesignationType"]?.ToString(),

                          
                        });
                    }
                }
            }

            return list;
        }



    }
}

