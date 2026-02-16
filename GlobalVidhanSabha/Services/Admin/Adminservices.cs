using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.EnterpriseServices;
using System.Linq;
using System.Web.Http;
using OfficeOpenXml;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using VishanSabha.Models;
using static System.Collections.Specialized.BitVector32;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Windows.Media.Media3D;
using System.Web.Services.Description;
using System.Net;
using QuestPDF;
using QuestPDF.Infrastructure;

using QuestPDF.Helpers;
using QuestPDF.Fluent;
using System.Runtime.Remoting.Messaging;
using System.Linq.Expressions;
using static QuestPDF.Helpers.Colors;
using System.Web.UI;

namespace VishanSabha.Services
{
    public class AdminServices
    {
        private readonly string conn;

        private SqlConnection con;
        public AdminServices()
        {
            conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            con = new SqlConnection(conn);
        }


        ////// Sector Services //////

        public bool AddSector(Sector sc,int VidhanSabhaId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mandal_Id", sc.Mandal_Id);
                        //cmd.Parameters.AddWithValue("@SectorNo", sc.SectorNo);
                        cmd.Parameters.AddWithValue("@SectorName", sc.SectorName);
                        cmd.Parameters.AddWithValue("@SectorIncharge_Name", sc.SectorInchargeName);
                        cmd.Parameters.AddWithValue("@Incharge_Name", sc.InchargeName);
                        cmd.Parameters.AddWithValue("@FatherName", sc.FatherName);
                        cmd.Parameters.AddWithValue("@Age", sc.Age);
                        //cmd.Parameters.AddWithValue("@CasteId", sc.castename);
                        cmd.Parameters.AddWithValue("@CasteId", sc.CasteId);
                        cmd.Parameters.AddWithValue("@SubCaste", sc.subcaste);
                        cmd.Parameters.AddWithValue("@Address", sc.Address);
                        cmd.Parameters.AddWithValue("@Education", sc.Education);
                        cmd.Parameters.AddWithValue("@PhoneNumber", sc.PhoneNumber);
                        cmd.Parameters.AddWithValue("@VillageId", sc.Village);
                        cmd.Parameters.AddWithValue("@ProfileImage", sc.ProfileImage ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Id", sc.Id);
                        cmd.Parameters.AddWithValue("@UserName", sc.Username);
                        cmd.Parameters.AddWithValue("@Password", sc.Password);
                        cmd.Parameters.AddWithValue("@Role", "SectorIncharge");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@Action", sc.Id > 0 ? "UPDATE" : "INSERT");

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        public List<Sector> Get_Sector()
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Sector
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        //SectorNo = dr["SectorNo"] != DBNull.Value ? dr["SectorNo"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        SectorInchargeName = dr["SectorIncharge_Name"] != DBNull.Value ? Convert.ToInt32(dr["SectorIncharge_Name"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castename = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        subcaste = dr["SubCaste"] != DBNull.Value ? dr["SubCaste"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                    });
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return data;
        }

        public bool DeleteById_Sector(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn)) // <-- Assign connection string
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@Id", Id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public Sector GetById_Sector(int Id)
        {
            Sector data = new Sector();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetById");
                        cmd.Parameters.AddWithValue("@Id", Id);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                var villageList = dr["VillageNames"] != DBNull.Value
                                     ? dr["VillageNames"].ToString()
                                         .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                         .Select(x => x.Trim())
                                         .ToList()
                                     : new List<string>();
                                var village = dr["VillageId"] != DBNull.Value
                                  ? dr["VillageId"].ToString()
                                      .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(x => x.Trim())
                                      .ToList()
                                  : new List<string>();

                                data = new Sector
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    Mandal_Id = Convert.ToInt32(dr["Mandal_Id"]),
                                    //SectorNo = dr["SectorNo"].ToString(),
                                    SectorName = dr["SectorName"].ToString(),
                                    SectorInchargeName = dr["SectorIncharge_Name"] != DBNull.Value ? Convert.ToInt32(dr["SectorIncharge_Name"]) : 0,
                                    InchargeName = dr["Incharge_Name"]?.ToString(),
                                    FatherName = dr["FatherName"]?.ToString(),
                                    Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : (int?)null,
                                    CasteId = dr["CasteId"]?.ToString(),
                                    subCasteId = dr["SubCaste"]?.ToString(),
                                    castename = dr["CategoryName"]?.ToString(),
                                    subcaste = dr["SubCasteName"]?.ToString(),
                                    Address = dr["Address"]?.ToString(),
                                    Education = dr["Education"]?.ToString(),
                                    PhoneNumber = dr["PhoneNumber"]?.ToString(),
                                    ProfileImage = dr["ProfileImage"]?.ToString(),
                                    VillageNames = villageList,
                                    VillageCount = villageList.Count,
                                    villageId= village

                                };
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return data;
        }


        public List<Sector> GetAll_Sector(int VidhanSabhaId,int? limit = null,int? page = null)
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    // Parse village names
                                    var villageList = dr["VillageNames"] != DBNull.Value
                                        ? dr["VillageNames"].ToString()
                                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(x => x.Trim())
                                            .ToList()
                                        : new List<string>();
                                    var villageId = dr["VillageId"] != DBNull.Value
                                      ? dr["VillageId"].ToString()
                                          .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(x => x.Trim())
                                          .ToList()
                                      : new List<string>();

                                    data.Add(new Sector
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        SectorInchargeName = dr["SectorIncharge_Name"] != DBNull.Value ? Convert.ToInt32(dr["SectorIncharge_Name"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : (int?)null,
                                        CasteId = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        subCasteId = dr["SubCaste"] != DBNull.Value ? dr["SubCaste"].ToString() : null,
                                        castename = dr["CategoryName"] != DBNull.Value ? dr["CategoryName"].ToString() : null,
                                        subcaste = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,

                                        
                                        VillageNames = villageList,
                                        VillageCount = villageList.Count,
                                        villageId= villageId
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                throw new Exception("Error fetching sectors", ex); // optional: log exception
            }

            return data;
        }

        public List<Sector> GetAll_SectorDetails(FilterModel filter, int? limit = null, int? page = null)
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SectorAllReportDetails");
                        cmd.Parameters.AddWithValue("@mandalIds", filter.mandalIds);
                        cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Sector
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        SectorNo = dr["SectorNo"]?.ToString(),
                                        SectorName = dr["SectorName"]?.ToString(),
                                        //PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value? dr["PollingStationBoothName"]?.ToString():"",
                                        SectorInchargeName = dr["SectorInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["SectorInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"]?.ToString(),
                                        FatherName = dr["FatherName"]?.ToString(),
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : (int?)null,

                                        //castename = dr["CasteId"]?.ToString(),
                                        subcaste = dr["subcaste"]?.ToString(),
                                        Address = dr["Address"]?.ToString(),
                                        Education = dr["Education"]?.ToString(),
                                        PhoneNumber = dr["PhoneNumber"]?.ToString(),
                                        ProfileImage = dr["ProfileImage"]?.ToString(),
                                        MandalName = dr["MandalName"]?.ToString(),

                                        TotalBooths = dr["TotalBooths"] != DBNull.Value ? Convert.ToInt32(dr["TotalBooths"]) : 0,
                                        //TotalVotes = dr["TotalVotes"] != DBNull.Value ? Convert.ToInt32(dr["TotalVotes"]) : 0,
                                        SeniorCitizen = dr["SeniorCitizen"] != DBNull.Value ? Convert.ToInt32(dr["SeniorCitizen"]) : 0,
                                        Handicap = dr["Handicap"] != DBNull.Value ? Convert.ToInt32(dr["Handicap"]) : 0,
                                        DoubleVotes = dr["DoubleVotes"] != DBNull.Value ? Convert.ToInt32(dr["DoubleVotes"]) : 0,
                                        Pravasi = dr["Pravasi"] != DBNull.Value ? Convert.ToInt32(dr["Pravasi"]) : 0,

                                        // Split the comma-separated village names into a list
                                        VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),



                                             BoothNumber= dr["BoothNumber"]?.ToString(),
                                     
                                        BoothName = dr["BoothName"]?.ToString(),
                                      
                                        BoothIncharge = dr["BoothIncharge"]?.ToString(),
                                        BoothCaste = dr["SubCasteName"]?.ToString(),
                                        BoothPhone = dr["BoothPhone"]?.ToString(),


                                        BoothFatherName = dr["BoothInchargeFather"] != DBNull.Value ? dr["BoothInchargeFather"].ToString() : null,
                                        BoothAge = dr["BoothInchargeAge"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeAge"]) : 0,
                                        //Age = Convert.ToInt32(dr["Age"]) ,
                                        BoothAddress = dr["BoothAddress"] != DBNull.Value ? dr["BoothAddress"].ToString() : null,
                                        BoothEducation = dr["boothEducation"] != DBNull.Value ? dr["boothEducation"].ToString() : null,

                                        BoothProfileImage = dr["BoothProfile"] != DBNull.Value ? dr["BoothProfile"].ToString() : null,
                                        // Split the comma-separated village names into a list
                                        BoothVillageNames = dr["VillageName"] != DBNull.Value ? dr["VillageName"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>()


                                    });
                                }
                            }
                            if (dr.NextResult())
                            {
                                while (dr.Read())
                                {
                                    int boothid = Convert.ToInt32(dr["boothId"]);
                                    int totalVotes = Convert.ToInt32(dr["boothvoters"]);

                                    // Assign TotalVotes to the matching Mandal in the list
                                    var booths = data.FirstOrDefault(m => m.Id == boothid);
                                    if (booths != null)
                                    {
                                        booths.TotalVotes = totalVotes;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return data;
        }




        public List<Sector> GetSectorNameId()
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");
                        //cmd.Parameters.AddWithValue("@Action", "GetAll");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Sector
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        //SectorNo = dr["SectorNo"] != DBNull.Value ? dr["SectorNo"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        SectorInchargeName = dr["SectorIncharge_Name"] != DBNull.Value ? Convert.ToInt32(dr["SectorIncharge_Name"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castename = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        subcaste = dr["SubCaste"] != DBNull.Value ? dr["SubCaste"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                    });
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return data;
        }
        public List<Sector> GetSectorByMandalId(string mandalId, int? limit = null, int? page = null)
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetByMandal");
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Mandal_Id", mandalId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                data.Add(new Sector
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    SectorName = dr["SectorName"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw; // or log the exception if needed
            }

            return data;
        }




        ///// Caste/////

        public bool AddCaste(Caste caste)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Caste", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CasteName", caste.CasteName);
                        cmd.Parameters.AddWithValue("@Caste_Id", caste.Caste_Id);
                        cmd.Parameters.AddWithValue("@Action", caste.Caste_Id > 0 ? "Update" : "Insert");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();

            }
        }

        public bool DeleteById_Caste(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Caste", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Caste_Id", Id);
                        cmd.Parameters.AddWithValue("@Action", "Delete");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }

        public Caste GetById_Caste(int Id)
        {
            Caste caste = new Caste();
            try
            {
                using (SqlConnection con = new SqlConnection())
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Caste", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetById");
                        cmd.Parameters.AddWithValue("@Caste_Id", Id);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                caste = new Caste
                                {
                                    Caste_Id = Convert.ToInt32(dr["Caste_Id"]),
                                    CasteName = dr["CasteName"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return caste;
        }

        public bool AddSubCaste(SubCaste Sc)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;

            try
            {
                con = new SqlConnection(conn); // Use proper connection string
                con.Open();

                cmd = new SqlCommand("Sp_SubCaste", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Assuming Sc.SubCaste_Id is being used as Id for GetById
                cmd.Parameters.AddWithValue("@Action", "GetById");
                cmd.Parameters.AddWithValue("@SubCaste_Id", Sc.SubCaste_Id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        SubCaste sc = new SubCaste
                        {
                            SubCaste_Id = Convert.ToInt32(dr["SubCaste_Id"]),
                            SubCasteName = dr["SubCasteName"].ToString()
                        };


                    }
                }

                // Close the reader before executing again
                cmd.Parameters.Clear(); // Clear previous parameters
                cmd.CommandText = "Sp_SubCaste"; // Reassign if necessary

                cmd.Parameters.AddWithValue("@Caste_Id", Sc.Caste_Id);  // Fixed: was Sc.SubCasteName
                cmd.Parameters.AddWithValue("@SubCasteName", Sc.SubCasteName);
                cmd.Parameters.AddWithValue("@SubCaste_Id", Sc.SubCaste_Id);
                cmd.Parameters.AddWithValue("@Action", Sc.SubCaste_Id > 0 ? "Update" : "Insert");

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public List<SubCaste> GetAllSubCaste()
        {
            List<SubCaste> sc = new List<SubCaste>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn)) // make sure 'conn' is your connection string variable
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("Sp_SubCaste", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    sc.Add(new SubCaste
                                    {
                                        SubCaste_Id = Convert.ToInt32(dr["Id"]),
                                        SubCasteName = dr["SubCasteName"].ToString()
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sc;
        }

        public List<SubCaste> GetByCasteId_SubCaste(int casteId)
        {
            List<SubCaste> data = new List<SubCaste>();
            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_SubCaste", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SubCasteByCasteId");
                        cmd.Parameters.AddWithValue("@Caste_Id", casteId);

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new SubCaste
                                    {
                                        SubCaste_Id = Convert.ToInt32(sdr["Id"]),        // Confirm these column names!
                                        Caste_Id = Convert.ToInt32(sdr["CategoryId"]),  // Confirm column names
                                        SubCasteName = sdr["SubCasteName"].ToString(),
                                        Status = Convert.ToInt32(sdr["status"])
                                    });
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log error somewhere, e.g., Console.WriteLine(ex.Message);
                    throw; // or handle gracefully
                }
            }
            return data;
        }


        public bool DeleteById_Booth(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "DELETE");
                        cmd.Parameters.AddWithValue("@Booth_Id", Id);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return true;
        }



        //booth services
        public Booth GetById_Booth(int Id)
        {
            Booth boothdata = new Booth();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetById");
                        cmd.Parameters.AddWithValue("@Booth_Id", Id);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    boothdata = new Booth
                                    {
                                        Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                        BoothNumber = dr["BoothNumber"]?.ToString(),
                                        PollingStationBoothName = dr["PollingStationBoothName"] != DBNull.Value ? dr["PollingStationBoothName"].ToString():"",
                                        BoothName = dr["BoothName"]?.ToString(),
                                        BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"]?.ToString(),
                                        FatherName = dr["FatherName"]?.ToString(),
                                        Age = (int?)(dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : (int?)null),
                                        castname = dr["CasteId"]?.ToString(),
                                        subcaste = dr["SubCaste"]?.ToString(),
                                        Address = dr["Address"]?.ToString(),
                                        Education = dr["Education"]?.ToString(),
                                        PhoneNumber = dr["PhoneNumber"]?.ToString(),
                                        ProfileImage = dr["ProfileImage"]?.ToString(),
                                        Aanshik = dr["Aanshik"]?.ToString(),

                                        VillageNames = dr["VillageNames"] != DBNull.Value
          ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList()
          : new List<string>(),

                                        villageId = dr["VillageId"] != DBNull.Value
          ? dr["VillageId"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList()
          : new List<string>(),
                                        //Aanshik = dr["Aanshik"] != DBNull.Value ? dr["Aanshik"].ToString() : "",
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : "",
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : "",
                                        //CasteId = dr["CasteId"]!=DBNull.Value? Convert.ToInt32(dr["CasteId"]):0,
                                        //SubCasteId = dr["SubCaste"] != DBNull.Value ? dr["SubCaste"].ToString():"",
                                        boothLocation = dr["boothLocation"] != DBNull.Value ? dr["boothLocation"].ToString() : "",



                                    };


                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //errorMessage = ex.Message;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return boothdata;
        }



        public bool AddBooth(Booth booth,int VidhanSabhaId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Mandal_Id", booth.Mandal_Id);
                        cmd.Parameters.AddWithValue("@Sector_Id", booth.Sector_Id);
                        cmd.Parameters.AddWithValue("@BoothNumber", booth.BoothNumber);
                        cmd.Parameters.AddWithValue("@PollingStationBoothName", booth.PollingStationBoothName);
                        cmd.Parameters.AddWithValue("@BoothName", booth.BoothName);
                        cmd.Parameters.AddWithValue("@BoothInchargeName", booth.BoothInchargeName);
                        cmd.Parameters.AddWithValue("@Incharge_Name", booth.InchargeName);
                        cmd.Parameters.AddWithValue("@FatherName", booth.FatherName);
                        cmd.Parameters.AddWithValue("@Age", booth.Age);
                        cmd.Parameters.AddWithValue("@CasteId", booth.CasteId);
                        cmd.Parameters.AddWithValue("@SubCaste", booth.subcaste);

                        cmd.Parameters.AddWithValue("@Address", booth.Address);
                        cmd.Parameters.AddWithValue("@Education", booth.Education);
                        cmd.Parameters.AddWithValue("@PhoneNumber", booth.PhoneNumber);
                        cmd.Parameters.AddWithValue("@VillageId", booth.Village);
                        cmd.Parameters.AddWithValue("@boothLocation", booth.boothLocation);
                        cmd.Parameters.AddWithValue("@ProfileImage", booth.ProfileImage ?? (object)DBNull.Value);
                        //cmd.Parameters.AddWithValue("@SubCaste", booth.subcaste);
                        cmd.Parameters.AddWithValue("@Aanshik", booth.Aanshik ?? "No");
                        // cmd.Parameters.AddWithValue("Status", 1);
                        cmd.Parameters.AddWithValue("@Booth_Id", booth.Booth_Id);
                        cmd.Parameters.AddWithValue("@UserName", booth.Username);
                        cmd.Parameters.AddWithValue("@Password", booth.Password);
                        cmd.Parameters.AddWithValue("@Role", "BoothIncharge");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@Action", booth.Booth_Id > 0 ? "UPDATE" : "INSERT");
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
        }

        public List<Booth> GetAll_Booth(int VidhanSabhaId)
        {
            List<Booth> booth = new List<Booth>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    booth.Add(new Booth
                                    {
                                        Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                        BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : "",
                                        BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : "",
                                        BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : "",
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : "",
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castname = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : "",
                                        subcaste = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : "",
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        //SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        //MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                        //SubCasteName = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,

                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

            return booth;
        }

        public List<Booth> GetAll_BoothForTable(int VidhanSabhaId,int? limit = null, int? page = null)
        {
            List<Booth> booth = new List<Booth>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", "getBoothAllDataForTable");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    booth.Add(new Booth
                                    {
                                        Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                        BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : "",
                                        PollingStationBoothName = dr["PollingStationBoothName"] != DBNull.Value ? dr["PollingStationBoothName"].ToString() : null,
                                        BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : "",
                                        BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : "",
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : "",
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,

                                        castname = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : "",
                                        subcaste = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : "",
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                        SubCasteName = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,

                                        VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),
                                        villageId = dr["VillageId"] != DBNull.Value ? dr["VillageId"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),
                                        boothLocation = dr["boothLocation"] != DBNull.Value ? dr["boothLocation"].ToString() : "",

                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            //return booth; 

            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

            return booth;
        }




        public List<Booth> GetBoothReport(FilterModel filter, int? limit = null, int? page = null)
        {
            List<Booth> reportList = new List<Booth>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetBoothReport");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                    cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Booth report = new Booth
                            {
                                Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                BoothNo = dr["BoothNo"]?.ToString(),
                                BoothName = dr["BoothName"]?.ToString(),
                                InchargeName = dr["BoothIncharge"]?.ToString(),
                                PollingStationBoothName =dr["PollingStationBoothName"]!=DBNull.Value? dr["PollingStationBoothName"]?.ToString():"",
                                Village = dr["VillageNames"]!=DBNull.Value? dr["VillageNames"].ToString():"",
                                //VillageNames = dr["VillageNames"] != DBNull.Value
                                //? dr["VillageNames"].ToString()
                                //    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                //    .Select(x => x.Trim())
                                //    .ToList()
                                //: new List<string>(),

                                //VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                                castname = dr["Caste"]?.ToString(),
                                Mobile = dr["MobileNo"]?.ToString(),
                                //TotalVotes = dr["BoothTotalVotes"] != DBNull.Value ? Convert.ToInt32(dr["BoothTotalVotes"]) : 0,
                                SeniorCitizen = dr["SeniorCitizen"] != DBNull.Value ? Convert.ToInt32(dr["SeniorCitizen"]) : 0,
                                Handicap = dr["Handicap"] != DBNull.Value ? Convert.ToInt32(dr["Handicap"]) : 0,
                                DoubleVotes = dr["DoubleVotes"] != DBNull.Value ? Convert.ToInt32(dr["DoubleVotes"]) : 0,
                                Pravasi = dr["Pravasi"] != DBNull.Value ? Convert.ToInt32(dr["Pravasi"]) : 0
                            };

                            reportList.Add(report);
                        }

                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                int boothid = Convert.ToInt32(dr["boothId"]);
                                int totalVotes = Convert.ToInt32(dr["boothvoters"]);

                                // Assign TotalVotes to the matching Mandal in the list
                                var booths = reportList.FirstOrDefault(m => m.Booth_Id == boothid);
                                if (booths != null)
                                {
                                    booths.TotalVotes = totalVotes;
                                }
                            }
                        }
                    }
                }
            }

            return reportList;
        }


        public List<Booth> GetAll_BoothForList()
        {
            List<Booth> booth = new List<Booth>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAllBoothList");

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    booth.Add(new Booth
                                    {
                                        Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                        BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : null,
                                        BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : null,
                                        BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castname = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        subcaste = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; // Or log the exception
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

            return booth;
        }


        //Mandal service
        public bool AddMandal(Mandal model,int VidhanSabhaId)
        {
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(conn);
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@MandalName", model.Name);
                    cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                    cmd.Parameters.AddWithValue("@Action", model.Id > 0 ? "Update" : "Insert");

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public bool DeleteById_Mandal(int Id)
        {
            SqlConnection con = null;

            try
            {
                con = new SqlConnection(conn); // ✅ Fix: missing conn
                con.Open();

                using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Delete");
                    cmd.Parameters.AddWithValue("@Id", Id);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed)
                    con.Close();
            }
        }


        public Mandal GetById_Mandal(int Id)
        {
            Mandal mandal = new Mandal();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetById");
                        cmd.Parameters.AddWithValue("@Id", Id);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                mandal = new Mandal
                                {
                                    Id = Convert.ToInt32(dr["Id"]),
                                    Name = dr["MandalName"].ToString()
                                };
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return mandal;
        }

        public List<Mandal> GetAllMandal(int VidhanSabhaId)
        {
            List<Mandal> data = new List<Mandal>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GETAll");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                  
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Mandal
                                    {
                                        Id = Convert.ToInt32(dr["Id"]),
                                        Name = dr["MandalName"].ToString()
                                    });
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return data;
        }

        public List<Mandal> GetAllMandalByVidhan(int VidhansabhaId)
        {
            List<Mandal> data = new List<Mandal>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GETAll");
                        cmd.Parameters.AddWithValue("@VidhansabhaId", VidhansabhaId);
                  
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Mandal
                                    {
                                        Id = Convert.ToInt32(dr["Id"]),
                                        Name = dr["MandalName"].ToString()
                                    });
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return data;
        }





        public List<Mandal> GetMandalReport(int? limit = null, int? page = null)
        {
            var list = new List<Mandal>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "getCountAllSectorbyMandalId");
                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // 1️⃣ First result set: mandal-level stats
                        while (dr.Read())
                        {
                            list.Add(new Mandal
                            {
                                Id = dr["MandalId"] != DBNull.Value ? Convert.ToInt32(dr["MandalId"]) : 0,
                                Name = dr["MandalName"]?.ToString(),
                                TotalSectors = dr["TotalSectors"] != DBNull.Value ? Convert.ToInt32(dr["TotalSectors"]) : 0,
                                TotalBooths = dr["TotalBooths"] != DBNull.Value ? Convert.ToInt32(dr["TotalBooths"]) : 0,
                                // We’ll set TotalVotes later
                                SeniorCount = dr["SeniorCount"] != DBNull.Value ? Convert.ToInt32(dr["SeniorCount"]) : 0,
                                DisabledCount = dr["DisabledCount"] != DBNull.Value ? Convert.ToInt32(dr["DisabledCount"]) : 0,
                                DoubleVotes = dr["DoubleVotes"] != DBNull.Value ? Convert.ToInt32(dr["DoubleVotes"]) : 0,
                                PravasiCount = dr["PravasiCount"] != DBNull.Value ? Convert.ToInt32(dr["PravasiCount"]) : 0,
                                EffectivePerson = dr["EffectivePerson"] != DBNull.Value ? Convert.ToInt32(dr["EffectivePerson"]) : 0
                            });
                        }

                        // 2️⃣ Second result set: TotalVoters per Mandal
                        if (dr.NextResult())
                        {
                            while (dr.Read())
                            {
                                int mandalId = Convert.ToInt32(dr["Mandal_Id"]);
                                int totalVotes = Convert.ToInt32(dr["TotalVoters"]);

                                // Assign TotalVotes to the matching Mandal in the list
                                var mandal = list.FirstOrDefault(m => m.Id == mandalId);
                                if (mandal != null)
                                {
                                    mandal.TotalVotes = totalVotes;
                                }
                            }
                        }
                    }
                }
            }

            return list;
        }


        public List<Sector> GetSectorsByMandalId(string mandalId)
        {
            var sectors = new List<Sector>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "MandalIdGetsectorIDDeatils");
                cmd.Parameters.AddWithValue("@MandalIds", mandalId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        sectors.Add(new Sector
                        {
                            Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                            Id = dr["SectorId"] != DBNull.Value ? Convert.ToInt32(dr["SectorId"]) : 0,
                            SectorName = dr["SectorName"]?.ToString(),
                            InchargeName = dr["InchargeName"]?.ToString(),
                            castename = dr["Cast"]?.ToString(),
                            PhoneNumber = dr["Mobile"]?.ToString(),

                            TotalBooths = dr["Booths"] != DBNull.Value ? Convert.ToInt32(dr["Booths"]) : 0, // ✅ Changed alias
                            TotalVotes = dr["TotalVotes"] != DBNull.Value ? Convert.ToInt32(dr["TotalVotes"]) : 0,

                            SeniorCitizen = dr["SeniorCitizen"] != DBNull.Value ? Convert.ToInt32(dr["SeniorCitizen"]) : 0,
                            Handicap = dr["Handicap"] != DBNull.Value ? Convert.ToInt32(dr["Handicap"]) : 0,
                            DoubleVotes = dr["DoubleVotes"] != DBNull.Value ? Convert.ToInt32(dr["DoubleVotes"]) : 0,
                            Pravasi = dr["Pravasi"] != DBNull.Value ? Convert.ToInt32(dr["Pravasi"]) : 0
                        });
                    }
                }
            }

            return sectors;
        }

        public List<Booth> GetBoothsByMandalId(int mandalId)
        {
            var booths = new List<Booth>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "MandalIdGetBoothIDDeatils");
                cmd.Parameters.AddWithValue("@MandalId", mandalId);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        booths.Add(new Booth
                        {
                            Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                            //MandalId = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                            //SectorId = dr["SectorId"] != DBNull.Value ? Convert.ToInt32(dr["SectorId"]) : 0,
                            BoothNo = dr["BoothNo"]?.ToString(),
                            PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value? dr["PollingStationBoothName"]?.ToString():"",
                            BoothName = dr["BoothName"]?.ToString(),
                            InchargeName = dr["Incharge"]?.ToString(),
                            castname = dr["Cast"]?.ToString(),
                            Mobile = dr["Mobile"]?.ToString(),

                            TotalVotes = dr["BoothTotalVotes"] != DBNull.Value ? Convert.ToInt32(dr["BoothTotalVotes"]) : 0,
                            SeniorCitizen = dr["SeniorCitizen"] != DBNull.Value ? Convert.ToInt32(dr["SeniorCitizen"]) : 0,
                            Handicap = dr["Handicap"] != DBNull.Value ? Convert.ToInt32(dr["Handicap"]) : 0,
                            DoubleVotes = dr["DoubleVotes"] != DBNull.Value ? Convert.ToInt32(dr["DoubleVotes"]) : 0,
                            Pravasi = dr["Pravasi"] != DBNull.Value ? Convert.ToInt32(dr["Pravasi"]) : 0
                        });
                    }
                }
            }

            return booths;
        }

        public List<Booth> GetBoothslistbySectorId(int sectorId,int? limit=null, int? page=null)
        {
            var booths = new List<Booth>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getboothnobysectorId");

                cmd.Parameters.AddWithValue("@SectorId", sectorId);
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                //cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                //cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                //cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                //cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);

                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        booths.Add(new Booth
                        {
                            SectorId = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                            //SectorName = dr["SectorName"]?.ToString(),
                            BoothId = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                            BoothNo = dr["BoothNumber"]?.ToString(),
                            PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value? dr["PollingStationBoothName"]?.ToString():"",
                            BoothName = dr["BoothName"]?.ToString(),
                            TotalVotes = dr["TotalVotes"] != DBNull.Value ? Convert.ToInt32(dr["TotalVotes"]) : 0,
                            InchargeName = dr["InchargeName"]?.ToString(),
                            SubCasteName = dr["Caste"]?.ToString(),
                            Mobile = dr["Mobile"]?.ToString(),

                            SeniorCitizen = dr["SeniorCitizen"] != DBNull.Value ? Convert.ToInt32(dr["SeniorCitizen"]) : 0,
                            Handicap = dr["Handicap"] != DBNull.Value ? Convert.ToInt32(dr["Handicap"]) : 0,
                            DoubleVotes = dr["DoubleVotes"] != DBNull.Value ? Convert.ToInt32(dr["DoubleVotes"]) : 0,
                            Pravasi = dr["Pravasi"] != DBNull.Value ? Convert.ToInt32(dr["Pravasi"]) : 0,

                            FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                            Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                            //Age = Convert.ToInt32(dr["Age"]) ,
                            Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                            Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,

                            ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                            // Split the comma-separated village names into a list
                            VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>()


                        });
                    }
                }
            }

            return booths;
        }

        //sectorcount

        public int SectorCount()
        {
            int count = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SectorCount");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                count = Convert.ToInt32(sdr["SectorCount"]);
                            }
                        }
                    }
                }
            }
            catch
            {
                return count;
            }
            return count;
        }

        //boothcount
        public int BoothCount()
        {
            int count = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "BoothCount");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                count = Convert.ToInt32(sdr["BoothCount"]);
                            }
                        }
                    }
                }
            }
            catch
            {
                return count;
            }
            return count;
        }

        public List<Sector> GetAllSectorWithBoothCount()
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAllSector");
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Sector
                                    {
                                        Id = dr["SectorId"] != DBNull.Value ? Convert.ToInt32(dr["SectorId"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        //SectorNo = dr["SectorNo"] != DBNull.Value ? dr["SectorNo"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        SectorInchargeName = dr["SectorIncharge_Name"] != DBNull.Value ? Convert.ToInt32(dr["SectorIncharge_Name"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castename = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        SubCasteName = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,
                                        CasteCategoryName = dr["CategoryName"] != DBNull.Value ? dr["CategoryName"].ToString() : null,  // Get Caste Category
                                        //SubCasteName = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,  // Get Sub Caste Name
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                        TotalBooth = Convert.ToInt32(dr["BoothCount"])
                                    });


                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return data;
        }



        public List<Booth> GetBoothBySectorId(string sectorId)
        {
            List<Booth> Booths = new List<Booth>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetBoothBysectorId");
                        cmd.Parameters.AddWithValue("@SectorIds", sectorId);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    Booths.Add(new Booth
                                    {
                                        Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                        BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : null,
                                        BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : null,
                                        BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castname = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        subcaste = dr["SubCaste"] != DBNull.Value ? dr["SubCaste"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Booths;
        }

        public List<PannaPramukh> GetAll_PannaPramukhforfilter(int VidhanSabhaId,string[] boothNumbers, int? limit = null, int? page = null)
        {
            List<PannaPramukh> pannaList = new List<PannaPramukh>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@BoothNumber", string.Join(",", boothNumbers ?? new[] { "all" }));
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PannaPramukh panna = new PannaPramukh
                                {
                                    PannaPramukh_Id = Convert.ToInt32(reader["PannaPramukh_Id"]),
                                    Booth_Id = Convert.ToInt32(reader["Booth_Id"]),
                                    BoothNumber = reader["BoothNumber"].ToString(),
                                    BoothName = reader["BoothName"].ToString(),
                                    Mandal_Id = Convert.ToInt32(reader["Mandal_Id"]),
                                    MandalName = reader["MandalName"].ToString(),
                                    Sector_Id = Convert.ToInt32(reader["Sector_Id"]),
                                    SectorName = reader["SectorName"].ToString(),
                                    Pannapramukh = reader["PannaPramukh"].ToString(),
                                    PannaNumber = reader["PannaNumber"].ToString(),
                                    PannaNumTo = reader["PannaNumTo"].ToString(),
                                    SubCasteName = reader["SubCasteName"].ToString(),
                                    Categoryname = reader["CategoryName"].ToString(),
                                    Category = reader["CategoryId"].ToString(),
                                    Cast = reader["Cast"].ToString(),
                                    VoterNumber = reader["VoterNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    ProfileImageUrl = reader["ProfileImageUrl"]?.ToString(),
                                    Status = Convert.ToBoolean(reader["Status"]),
                                    VillageListId = reader["VillageId"] != DBNull.Value ? reader["VillageId"].ToString():"",
                                    village = reader["villageName"] != DBNull.Value ? reader["villageName"].ToString():""
                                    
                                };
                                pannaList.Add(panna);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return pannaList;
        }






        public List<PannaPramukh> GetAll_PannaPramukh(string boothNumber = "all")
        {
            List<PannaPramukh> pannaList = new List<PannaPramukh>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");
                        cmd.Parameters.AddWithValue("@BoothNumber",
                            string.IsNullOrEmpty(boothNumber) ? "all" : boothNumber);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PannaPramukh panna = new PannaPramukh
                                {
                                    PannaPramukh_Id = Convert.ToInt32(reader["PannaPramukh_Id"]),
                                    Booth_Id = Convert.ToInt32(reader["Booth_Id"]),
                                    BoothNumber = reader["BoothNumber"].ToString(),
                                    BoothName = reader["BoothName"].ToString(),
                                    Mandal_Id = Convert.ToInt32(reader["Mandal_Id"]),
                                    MandalName = reader["MandalName"].ToString(),
                                    Sector_Id = Convert.ToInt32(reader["Sector_Id"]),
                                    SectorName = reader["SectorName"].ToString(),
                                    Pannapramukh = reader["PannaPramukh"].ToString(),
                                    PannaNumber = reader["PannaNumber"].ToString(),
                                    PannaNumTo = reader["PannaNumTo"].ToString(),
                                    SubCasteName = reader["SubCasteName"].ToString(),
                                    Categoryname = reader["CategoryName"].ToString(),
                                    Category = reader["CategoryId"].ToString(),
                                    Cast = reader["Cast"].ToString(),
                                    VoterNumber = reader["VoterNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    ProfileImageUrl = reader["ProfileImageUrl"]?.ToString(),
                                    Status = Convert.ToBoolean(reader["Status"])
                                };
                                pannaList.Add(panna);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return pannaList;
        }



        public List<PannaPramukh> GetAllPannaPramukhList(int? limit = null, int? page = null)
        {
            List<PannaPramukh> pannaList = new List<PannaPramukh>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAllPannaPramukh");
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PannaPramukh panna = new PannaPramukh
                                {
                                    PannaPramukh_Id = Convert.ToInt32(reader["PannaPramukh_Id"]),
                                    Booth_Id = Convert.ToInt32(reader["Booth_Id"]),
                                    BoothNumber = reader["BoothNumber"].ToString(),
                                    BoothName = reader["BoothName"].ToString(),
                                    Mandal_Id = Convert.ToInt32(reader["Mandal_Id"]),
                                    MandalName = reader["MandalName"].ToString(),
                                    Sector_Id = Convert.ToInt32(reader["Sector_Id"]),
                                    SectorName = reader["SectorName"].ToString(),
                                    Pannapramukh = reader["PannaPramukh"].ToString(),
                                    PannaNumber = reader["PannaNumber"].ToString(),
                                    //PannaNumTo = reader["PannaNumTo"].ToString(),
                                    SubCasteName = reader["SubCasteName"].ToString(),
                                    Categoryname = reader["CategoryName"].ToString(),
                                    Category = reader["CategoryId"].ToString(),
                                    Cast = reader["Cast"].ToString(),
                                    VoterNumber = reader["VoterNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    ProfileImageUrl = reader["ProfileImageUrl"]?.ToString(),
                                    Status = Convert.ToBoolean(reader["Status"]),
                                    village = reader["villageName"] != DBNull.Value ? reader["villageName"].ToString():"",
                                    VillageListId = reader["VillageId"] != DBNull.Value ? reader["VillageId"].ToString():"",
                                    
                                };
                                pannaList.Add(panna);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return pannaList;
        }

        public List<PannaPramukh> GetAll_PannaPramukh()
        {
            List<PannaPramukh> pannaList = new List<PannaPramukh>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAll");

                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PannaPramukh panna = new PannaPramukh
                                {
                                    PannaPramukh_Id = Convert.ToInt32(reader["PannaPramukh_Id"]),
                                    Booth_Id = Convert.ToInt32(reader["Booth_Id"]),
                                    BoothNumber = reader["BoothNumber"].ToString(),
                                    PollingStationBoothName = reader["PollingStationBoothName"] != DBNull.Value ? reader["PollingStationBoothName"].ToString():"",
                                    BoothName = reader["BoothName"].ToString(),
                                    Mandal_Id = Convert.ToInt32(reader["Mandal_Id"]),
                                    MandalName = reader["MandalName"].ToString(),
                                    Sector_Id = Convert.ToInt32(reader["Sector_Id"]),
                                    SectorName = reader["SectorName"].ToString(),
                                    Pannapramukh = reader["PannaPramukh"].ToString(),
                                    PannaNumber = reader["PannaNumber"].ToString(),
                                    PannaNumTo = reader["PannaNumTo"].ToString(),
                                    SubCasteName = reader["SubCasteName"].ToString(),
                                    Categoryname = reader["CategoryName"].ToString(),
                                    Category = reader["CategoryId"].ToString(),
                                    Cast = reader["Cast"].ToString(),
                                    VoterNumber = reader["VoterNumber"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    ProfileImageUrl = reader["ProfileImageUrl"]?.ToString(),
                                    Status = Convert.ToBoolean(reader["Status"]),
                                    village = reader["villageName"] != DBNull.Value ? reader["villageName"].ToString() : "",
                                    //location = reader["boothLocation"] != DBNull.Value ? reader["boothLocation"].ToString() : "",

                                };
                                pannaList.Add(panna);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return pannaList;
        }


        public List<PannaPramukh> GetAll_PannaPramukhById(int pannaId)
        {
            List<PannaPramukh> pannaList = new List<PannaPramukh>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetPannaById");
                        cmd.Parameters.AddWithValue("@PannaPramukh_Id", pannaId);

                        con.Open();  // OPEN the SqlConnection object

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PannaPramukh panna = new PannaPramukh
                                {
                                    PannaPramukh_Id = Convert.ToInt32(reader["PannaPramukh_Id"]),
                                    Booth_Id = Convert.ToInt32(reader["Booth_Id"]),
                                    BoothName = reader["BoothName"].ToString(),  // Added
                                    Mandal_Id = Convert.ToInt32(reader["Mandal_Id"]),
                                    MandalName = reader["MandalName"].ToString(), // Added
                                    Sector_Id = Convert.ToInt32(reader["Sector_Id"]),
                                    SectorName = reader["SectorName"].ToString(), // Added
                                    Pannapramukh = reader["PannaPramukh"].ToString(),
                                    PannaNumber = reader["PannaNumber"].ToString(),
                                    BoothNumber = reader["BoothNumber"].ToString(),
                                    PannaNumTo = reader["PannaNumTo"].ToString(),
                                    //Cast = reader["Cast"].ToString(),
                                    SubCasteName = reader["SubCasteName"].ToString(),
                                    Categoryname = reader["CategoryName"].ToString(),
                                    Category = reader["Id"].ToString(),
                                    Cast = reader["Cast"].ToString(),
                                    VoterNumber = reader["VoterNumber"].ToString(),

                                    Address = reader["Address"].ToString(),
                                    Mobile = reader["Mobile"].ToString(),
                                    ProfileImageUrl = reader["ProfileImageUrl"]?.ToString(),
                                    Status = Convert.ToBoolean(reader["Status"]),
                                    VillageListId = reader["VillageId"] == DBNull.Value ? "" : reader["VillageId"].ToString(),
                                    village = reader["VillageNames"] == DBNull.Value ? "" : reader["VillageNames"].ToString(),
                                };

                                pannaList.Add(panna);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log exception or handle it as needed
                    throw;
                }
            }

            return pannaList;
        }


        public List<Sector> GetSecInchargeBySecId(int sectorid)
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", sectorid);
                        cmd.Parameters.AddWithValue("@Action", "GetInchargeBySectorId");
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Sector
                                    {
                                        Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        //SectorNo = dr["SectorNo"] != DBNull.Value ? dr["SectorNo"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        SectorInchargeName = dr["SectorIncharge_Name"] != DBNull.Value ? Convert.ToInt32(dr["SectorIncharge_Name"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castename = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        subcaste = dr["SubCaste"] != DBNull.Value ? dr["SubCaste"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                        //TotalBooth = Convert.ToInt32(dr["BoothCount"])
                                        SubCasteName = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,
                                        CasteCategoryName = dr["CategoryName"] != DBNull.Value ? dr["CategoryName"].ToString() : null,
                                        Username = dr["UserName"] != DBNull.Value ? dr["UserName"].ToString() : null,
                                        Contact = dr["Contact"] != DBNull.Value ? dr["Contact"].ToString() : null,
                                        Password = dr["Password"] != DBNull.Value ? dr["Password"].ToString() : null,
                                    });

                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return data;
        }
        public List<Booth> GetBoothInchargeByBoothId(int Boothid)
        {
            List<Booth> Booths = new List<Booth>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetBoothInchargeViewProfile");
                        cmd.Parameters.AddWithValue("@Booth_Id", Boothid);
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Booths.Add(new Booth
                                {
                                    Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                    Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                    Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                    SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                    BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : null,
                                    BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : null,
                                    BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                    InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                    FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                    Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                    castname = dr["CategoryName"] != DBNull.Value ? dr["CategoryName"].ToString() : null,
                                    subcaste = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,
                                    Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                    Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                    PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                    ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                    MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                    Username = dr["UserName"] != DBNull.Value ? dr["UserName"].ToString() : null,
                                    Userid = dr["Contact"] != DBNull.Value ? dr["Contact"].ToString() : null,
                                    Password = dr["Password"] != DBNull.Value ? dr["Password"].ToString() : null

                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;

            }
            return Booths;
        }
        public List<DashboardCount> DashboardTotalCount()
        {
            List<DashboardCount> dashboards = new List<DashboardCount>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetTotalDashboardCount");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                dashboards.Add(new DashboardCount
                                {
                                    TotalMandal = Convert.ToInt32(sdr["TotalMandal"]),
                                    TotalSector = Convert.ToInt32(sdr["TotalSector"]),
                                    TotalBooth = Convert.ToInt32(sdr["Totalbooth"]),
                                    TotalPanna = Convert.ToInt32(sdr["TotalPanna"]),
                                    TotalPravasiVoter = Convert.ToInt32(sdr["TotalPravasiVoter"]),
                                    TotalNewVoter = Convert.ToInt32(sdr["TotalNewVoter"]),
                                    TotalDoublevoter = Convert.ToInt32(sdr["TotalDoublevoter"]),
                                    TotalSeniorCitizen = Convert.ToInt32(sdr["TotalSeniorCitizen"]),
                                    TotalHandicap = Convert.ToInt32(sdr["TotalHandicap"]),
                                    TotalBoothVoterDes = Convert.ToInt32(sdr["TotalBoothVoterDes"]),
                                    TotalSatisfied = Convert.ToInt32(sdr["TotalSatisfied"]),
                                    TotalUnsatisfied = Convert.ToInt32(sdr["Totalunsatisfied"]),
                                    TotalActivity = Convert.ToInt32(sdr["TotalActivity"]),
                                    TotalBDC = Convert.ToInt32(sdr["TotalBDC"]),
                                    TotalBlock = Convert.ToInt32(sdr["TotalBlock"]),
                                    TotalBoothSamiti = Convert.ToInt32(sdr["TotalBoothSamiti"]),
                                    TotalSocialMediaPost = Convert.ToInt32(sdr["TotalSocialMediaPost"]),
                                    TotalEffectivePerson = Convert.ToInt32(sdr["TotalEffective"]),
                                    TotalInfluencer = Convert.ToInt32(sdr["TotalInfluencer"])
                                });
                            }
                        }
                    }
                }
                return dashboards;

            }
            catch
            {
                throw;
            }
        }

        #region manisha
        public bool ProcessBoothExcel(Stream fileStream)
        {
            try
            {

                using (var package = new ExcelPackage(fileStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    using (SqlConnection con = new SqlConnection(conn))
                    {
                        con.Open();

                        for (int row = 3; row <= rowCount; row++)
                        {

                            string Mandal = worksheet.Cells[row, 2].Text.Trim();
                            string BoothName = worksheet.Cells[row, 3].Text.Trim();
                            string SectorName = worksheet.Cells[row, 4].Text.Trim();
                            string sectorConvenor = worksheet.Cells[row, 5].Text.Trim();
                            string SectorConvenorMobile = worksheet.Cells[row, 6].Text.Trim();
                            string boothPresident = worksheet.Cells[row, 7].Text.Trim();
                            string fatherName = worksheet.Cells[row, 8].Text.Trim();
                            int age = int.TryParse(worksheet.Cells[row, 9].Text, out int a) ? a : 0;
                            string caste = worksheet.Cells[row, 10].Text.Trim();
                            string address = worksheet.Cells[row, 11].Text.Trim();
                            string qualification = worksheet.Cells[row, 12].Text.Trim();
                            string phoneCell = worksheet.Cells[row, 13].Text.Trim();


                            string[] phoneNumbers = phoneCell
                                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(p => p.Trim())
                                .Where(p => !string.IsNullOrEmpty(p))
                                .ToArray();


                            string allPhones = string.Join(",", phoneNumbers);


                            using (SqlCommand cmd = new SqlCommand("sp_BoothExcelData", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@Mandal", Mandal);
                                cmd.Parameters.AddWithValue("@BoothName", BoothName);
                                cmd.Parameters.AddWithValue("@SectorName", SectorName);
                                cmd.Parameters.AddWithValue("@Sector_Convenor_Name", sectorConvenor);
                                cmd.Parameters.AddWithValue("@Sector_Incharge_Mobile", SectorConvenorMobile);
                                cmd.Parameters.AddWithValue("@Booth_President_Name", boothPresident);
                                cmd.Parameters.AddWithValue("@Father_Name", fatherName);
                                cmd.Parameters.AddWithValue("@Age", age);
                                cmd.Parameters.AddWithValue("@Caste", caste);
                                cmd.Parameters.AddWithValue("@Address", address);
                                cmd.Parameters.AddWithValue("@Educational_Qualification", qualification);
                                cmd.Parameters.AddWithValue("@Phone_No", allPhones);
                                cmd.Parameters.AddWithValue("@action", "uploadBoothExcelData");
                                cmd.ExecuteNonQuery();
                            }

                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AddDoubleVoters(doubleVoter data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_doubleVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", data.id > 0 ? "update" : "insert");
                        cmd.Parameters.AddWithValue("@id", data.id);
                        cmd.Parameters.AddWithValue("@name", data.name);
                        cmd.Parameters.AddWithValue("@fathername", data.fathername);
                        cmd.Parameters.AddWithValue("@voterno", data.voterno);
                        cmd.Parameters.AddWithValue("@BoothNo", data.BoothNumber);
                        cmd.Parameters.AddWithValue("@pastaddress", data.pastAddress);
                        cmd.Parameters.AddWithValue("@currentAddress", data.currAddress);
                        cmd.Parameters.AddWithValue("@VillageId", data.VillageListId);
                        cmd.Parameters.AddWithValue("@reason", data.description);
                        int res = cmd.ExecuteNonQuery();
                        return res > 0;
                    }
                }

            }
            catch
            {
                return false;
            }
        }
        public List<doubleVoter> GetAllDoubleVoters(int VidhanSabhaId,int? limit = null, int? page = null)
        {
            List<doubleVoter> data = new List<doubleVoter>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_doubleVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "selectAll");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new doubleVoter
                                {
                                    id = sdr["id"] != DBNull.Value ? Convert.ToInt32(sdr["id"]) : 0,
                                    boothid = sdr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(sdr["Booth_Id"]) : 0,
                                    BoothNumber = sdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(sdr["BoothNumber"]) : 0,
                                    PollingStationBoothName = sdr["PollingStationBoothName"] != DBNull.Value ? sdr["PollingStationBoothName"].ToString() : null,
                                    name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : null,
                                    fathername = sdr["fathername"] != DBNull.Value ? sdr["fathername"].ToString() : null,
                                    voterno = sdr["voterno"] != DBNull.Value ? sdr["voterno"].ToString() : null,
                                    pastAddress = sdr["pastaddress"] != DBNull.Value ? sdr["pastaddress"].ToString() : null,
                                    currAddress = sdr["currentAddress"] != DBNull.Value ? sdr["currentAddress"].ToString() : null,
                                    description = sdr["reason"] != DBNull.Value ? sdr["reason"].ToString() : null,
                                    village = sdr["villageName"] != DBNull.Value ? sdr["villageName"].ToString() : null,
                                    VillageListId = sdr["VillageId"] != DBNull.Value ? sdr["VillageId"].ToString() : null,
                                    BoothName = sdr["BoothName"] != DBNull.Value ? sdr["BoothName"].ToString() : null
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        public bool deleteDoubleVoter(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_doubleVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "delete");
                        cmd.Parameters.AddWithValue("@id", id);
                        int res = cmd.ExecuteNonQuery();
                        return res > 0;

                    }
                }

            }
            catch
            {
                throw;
            }
        }
        public doubleVoter getDoubleVoterById(int? id)
        {
            doubleVoter data = new doubleVoter();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_doubleVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@action", "getById");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data = new doubleVoter();
                                data.id = sdr["id"] != DBNull.Value ? Convert.ToInt32(sdr["id"]) : 0;
                                data.boothid = sdr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(sdr["Booth_Id"]) : 0;
                                data.BoothNumber = sdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(sdr["BoothNumber"]) : 0;
                                data.name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : "";
                                data.fathername = sdr["fathername"] != DBNull.Value ? sdr["fathername"].ToString() : "";
                                data.voterno = sdr["voterno"] != DBNull.Value ? sdr["voterno"].ToString() : "null";
                                data.pastAddress = sdr["pastaddress"] != DBNull.Value ? sdr["pastaddress"].ToString() : "";
                                data.currAddress = sdr["currentAddress"] != DBNull.Value ? sdr["currentAddress"].ToString() : "";
                                data.description = sdr["reason"] != DBNull.Value ? sdr["reason"].ToString() : "";
                                data.VillageListId = sdr["VillageId"] != DBNull.Value ? sdr["VillageId"].ToString() : "";
                                data.VillageListName = sdr["villageName"] != DBNull.Value ? sdr["villageName"].ToString() : "";
                              
                            }
                        }

                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        public List<Caste> GetAllCategory()
        {
            List<Caste> data = new List<Caste>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetAllCategory");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new Caste
                                {
                                    Caste_Id = Convert.ToInt32(sdr["Id"]),
                                    CasteName = sdr["CategoryName"].ToString()
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }
        public List<SubCaste> GeSubcasteByCategoryId(int id)
        {
            List<SubCaste> data = new List<SubCaste>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetSubcasteByCateId");
                        cmd.Parameters.AddWithValue("@categoryId", id);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SubCaste
                                {
                                    SubCaste_Id = Convert.ToInt32(sdr["Id"]),
                                    Caste_Id = Convert.ToInt32(sdr["CategoryId"]),
                                    SubCasteName = sdr["SubCasteName"].ToString(),

                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        public bool AddPravasiVoters(PravasiVoter data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", data.id);
                        cmd.Parameters.AddWithValue("@boothno", data.boothno);
                        cmd.Parameters.AddWithValue("@name", data.name);
                        cmd.Parameters.AddWithValue("@categoryId", data.Category);
                        cmd.Parameters.AddWithValue("@casteId", data.Caste);
                        cmd.Parameters.AddWithValue("@mobile", data.mobile);
                        cmd.Parameters.AddWithValue("@currentAddress", data.currentAddress);
                        cmd.Parameters.AddWithValue("@occupation", data.Occupation);
                        cmd.Parameters.AddWithValue("@VillageId", data.VillageListId);
                        cmd.Parameters.AddWithValue("@action", data.id > 0 ? "update" : "insert");

                        int res = cmd.ExecuteNonQuery();
                        return res > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        #region API PRAVASI NAZIF
        public List<PravasiVoter> GetAllPravasiVoterData(int VidhansabhaId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<PravasiVoter> data = new List<PravasiVoter>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "selectAll");
                        cmd.Parameters.AddWithValue("@VidhansabhaId", VidhansabhaId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new PravasiVoter
                                {
                                    id = sdr["id"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["id"]),
                                    boothno = sdr["boothno"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["boothno"]),
                                    Occupation = sdr["occupationName"] == DBNull.Value ? "" : sdr["occupationName"].ToString(),
                                    Occupations =  Convert.ToInt32(sdr["occupation"]),
                                    BoothNumber = sdr["BoothNumber"] == DBNull.Value ? "" : sdr["BoothNumber"].ToString(),
                                    SectorId = sdr["Id"] == DBNull.Value ? "" : sdr["Id"].ToString(),
                                    SectorName = sdr["SectorName"] == DBNull.Value ? "" : sdr["SectorName"].ToString(),
                                    SectorIncharge = sdr["Incharge_Name"] == DBNull.Value ? "" : sdr["Incharge_Name"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"] == DBNull.Value ? "" : sdr["PollingStationBoothName"].ToString(),
                                    name = sdr["name"] == DBNull.Value ? null : sdr["name"].ToString(),
                                    Caste = sdr["casteId"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["casteId"]),
                                    Category = sdr["categoryId"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["categoryId"]),
                                    CategoryName = sdr["CategoryName"] == DBNull.Value ? null : sdr["CategoryName"].ToString(),
                                    CasteName = sdr["SubCasteName"] == DBNull.Value ? null : sdr["SubCasteName"].ToString(),
                                    VillageListId = sdr["VillageId"] == DBNull.Value ? null : sdr["VillageId"].ToString(),
                                  
                                    mobile = sdr["mobile"] == DBNull.Value ? null : sdr["mobile"].ToString(),
                                    currentAddress = sdr["currentAddress"] == DBNull.Value ? null : sdr["currentAddress"].ToString(),
                                    VillageListName = sdr["VillageNames"] == DBNull.Value ? null : sdr["VillageNames"].ToString()
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        #endregion
        public bool DeletePravasiVoter(int id)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "delete");
                        cmd.Parameters.AddWithValue("@id", id);
                        int res = cmd.ExecuteNonQuery();
                        return res > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public PravasiVoter getPravasiDataById(int? id)
        {
            PravasiVoter data = new PravasiVoter();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getbyId");
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data = new PravasiVoter();
                                data.id = Convert.ToInt32(sdr["id"]);
                                data.Category = Convert.ToInt32(sdr["categoryId"]);
                                data.CategoryName = sdr["CategoryName"].ToString();
                                data.Caste = Convert.ToInt32(sdr["casteId"]);
                                data.CasteName = sdr["SubCasteName"].ToString();
                                data.name = sdr["name"].ToString();
                                data.mobile = sdr["mobile"].ToString();
                                data.currentAddress = sdr["currentAddress"].ToString();
                                data.BoothNumber = sdr["BoothNumber"].ToString();
                                data.Occupations = Convert.ToInt32(sdr["occupation"]); ;
                                data.boothid = Convert.ToInt32(sdr["Booth_Id"]);
                                data.VillageListId = sdr["VillageId"].ToString();


                            }
                        }
                    }
                }
                return data;

            }
            catch
            {
                throw;
            }
        }
        public int GetTotalPannaPramukhCount()
        {
            int TotalPanna = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "CountTotalPanna");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                TotalPanna = Convert.ToInt32(sdr["TotalPanna"]);
                            }
                        }
                    }
                }
                return TotalPanna;
            }
            catch
            {
                throw;
            }
        }
        public int GetTotalPravasi()
        {
            int TotalPravasi = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetTotalPravasi");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                TotalPravasi = Convert.ToInt32(sdr["TotalPravasi"]);
                            }
                        }
                    }
                }
                return TotalPravasi;
            }
            catch
            {
                throw;
            }
        }

        public int GetTotalVoters()
        {
            int TotalVoters = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_NewVoters", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Actions", "GetTotalVoter");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                TotalVoters = Convert.ToInt32(sdr["TotalVoters"]);
                            }
                        }
                    }
                }
                return TotalVoters;
            }
            catch
            {
                throw;
            }
        }
        public int GetTotalDoubleVoter()
        {
            int TotalDoubleVoter = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_doubleVoter", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetTotalDoubleVoter");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                TotalDoubleVoter = Convert.ToInt32(sdr["totalDoubleVoter"]);
                            }
                        }
                    }
                }
                return TotalDoubleVoter;
            }
            catch
            {
                throw;
            }
        }

        public int GetTotalEffectivePerson()
        {
            int TotalEffectivePerson = 0;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_EffectivePerson_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetAllEffectivePerson");

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        TotalEffectivePerson = Convert.ToInt32(result);
                    }
                }
            }
            return TotalEffectivePerson;
        }

        public List<Sector> GetAllSectorWithBoothCountByMandalId(int MandalId)
        {
            List<Sector> data = new List<Sector>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "getAllSectorWithBothCountByMandalId");
                        cmd.Parameters.AddWithValue("@id", MandalId);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    data.Add(new Sector
                                    {
                                        Id = dr["SectorId"] != DBNull.Value ? Convert.ToInt32(dr["SectorId"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        //SectorNo = dr["SectorNo"] != DBNull.Value ? dr["SectorNo"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        SectorInchargeName = dr["SectorIncharge_Name"] != DBNull.Value ? Convert.ToInt32(dr["SectorIncharge_Name"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castename = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                        subcaste = dr["SubCaste"] != DBNull.Value ? dr["SubCaste"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                        TotalBooth = Convert.ToInt32(dr["BoothCount"])
                                    });

                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }
            return data;
        }

        public List<Booth> GetAll_BoothForListBySectorId(int sectorId)
        {
            List<Booth> booth = new List<Booth>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAllBoothBySectorId");
                        cmd.Parameters.AddWithValue("@Sector_Id", sectorId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    booth.Add(new Booth
                                    {
                                        Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                        BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : null,
                                        BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : null,
                                        BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castname = dr["CategoryName"] != DBNull.Value ? dr["CategoryName"].ToString() : null,
                                        subcaste = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; // Or log the exception
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

            return booth;
        }

        public List<Booth> GetAll_BoothForListByMandalId(int mandalId)
        {
            List<Booth> booth = new List<Booth>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "getBoothListByMandalId");
                        cmd.Parameters.AddWithValue("@MandalId", mandalId);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    booth.Add(new Booth
                                    {
                                        Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                        Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                        Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,
                                        BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : null,
                                        BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : null,
                                        BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                        InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                        FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                        Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                        castname = dr["CategoryName"] != DBNull.Value ? dr["CategoryName"].ToString() : null,
                                        subcaste = dr["SubCasteName"] != DBNull.Value ? dr["SubCasteName"].ToString() : null,
                                        Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                        Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                        PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                        ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                        SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                        MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex; // Or log the exception
            }
            finally
            {
                if (con.State != ConnectionState.Closed)
                    con.Close();
            }

            return booth;
        }


        public List<MandalSectorBoothCount> selectAllSectorBoothCount()
        {
            List<MandalSectorBoothCount> data = new List<MandalSectorBoothCount>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("vidhanUser.Sp_Mandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "MandalTotalSectorTotalBoothCount");

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if (sdr.HasRows)
                            {
                                while (sdr.Read())
                                {
                                    data.Add(new MandalSectorBoothCount
                                    {
                                        mandalId = sdr["MandalId"] != DBNull.Value ? Convert.ToInt32(sdr["MandalId"]) : 0,
                                        MandalName = sdr["MandalName"] != DBNull.Value ? sdr["MandalName"].ToString() : string.Empty,
                                        sectorcount = sdr["SectorCount"] != DBNull.Value ? Convert.ToInt32(sdr["SectorCount"]) : 0,
                                        boothcount = sdr["BoothCount"] != DBNull.Value ? Convert.ToInt32(sdr["BoothCount"]) : 0
                                    });
                                }
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }


        #endregion

        #region nazif

        public bool InsertPannaPramukh(PannaPramukh panna)
        {
            bool isInserted = false;
            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", "Insert");
                        cmd.Parameters.AddWithValue("@Booth_Id", panna.Booth_Id);
                        cmd.Parameters.AddWithValue("@PannaPramukh", panna.Pannapramukh ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Cast", panna.Cast ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Category", panna.Category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PannaNumber", panna.PannaNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PannaNumTo", panna.PannaNumTo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@VoterNumber", panna.VoterNumber ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Address", panna.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Mobile", panna.Mobile ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ProfileImageUrl", panna.ProfileImageUrl ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", panna.Status);
                        cmd.Parameters.AddWithValue("@VillageId", panna.VillageListId);
                        con.Open();
                        var result = cmd.ExecuteScalar();
                        isInserted = (result != null && Convert.ToInt32(result) > 0);
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Log sqlEx.Message or handle as needed
                    throw new Exception("Database error occurred while inserting Panna Pramukh.", sqlEx);
                }
                catch (Exception ex)
                {
                    // Log ex.Message or handle as needed
                    throw new Exception("An error occurred while inserting Panna Pramukh.", ex);
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            return isInserted;
        }


        //Nazif 
        public bool UpdatePannaPramukh(PannaPramukh panna)
        {
            bool isUpdated = false;

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", "Update");
                        cmd.Parameters.AddWithValue("@PannaPramukh_Id", panna.PannaPramukh_Id);
                        cmd.Parameters.AddWithValue("@Booth_Id", panna.Booth_Id);
                        cmd.Parameters.AddWithValue("@PannaPramukh", panna.Pannapramukh ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PannaNumber", panna.PannaNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PannaNumTo", panna.PannaNumTo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Cast", panna.Cast ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Category", panna.Category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@VoterNumber", panna.VoterNumber ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Address", panna.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Mobile", panna.Mobile ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ProfileImageUrl", panna.ProfileImageUrl ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Status", panna.Status);
                        cmd.Parameters.AddWithValue("@VillageId", panna.VillageListId);

                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        isUpdated = rowsAffected > 0;
                    }
                }
                catch (SqlException sqlEx)
                {
                    // Log or handle exception
                    throw new Exception("Database error occurred while updating Panna Pramukh.", sqlEx);
                }
                catch (Exception ex)
                {
                    // Log or handle exception
                    throw new Exception("An error occurred while updating Panna Pramukh.", ex);
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                        con.Close();
                }
            }

            return isUpdated;
        }
        public bool DeleteById_PannaPramukh(int id)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("SP_PannaPramukh", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@PannaPramukh_Id", id);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public bool InsertBoothSamiti(BoothSamiti model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("InsertBoothSamiti", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Action", "Insert");
                    cmd.Parameters.AddWithValue("@BoothId", model.BoothId);
                    cmd.Parameters.AddWithValue("@DesignationId", model.DesignationId);

                    cmd.Parameters.AddWithValue("@Name", model.Name ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Cast", model.Cast ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Category", model.Category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Age", model.Age != null ? model.Age : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Mobile", model.Mobile ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Occupation", model.Occupation ?? (object)DBNull.Value);


                    SqlParameter returnParameter = new SqlParameter();
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnParameter);

                    con.Open();


                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }

            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public List<BoothSamiti> GetAllBoothSamiti()
        {
            List<BoothSamiti> list = new List<BoothSamiti>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("InsertBoothSamiti", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SelectAll");

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    BoothSamiti samiti = new BoothSamiti
                    {
                        BoothSamiti_Id = rdr["Id"] != DBNull.Value ? Convert.ToInt32(rdr["Id"]) : 0,
                        BoothId = rdr["BoothId"] != DBNull.Value ? Convert.ToInt32(rdr["BoothId"]) : 0,
                        BoothName = rdr["BoothName"] != DBNull.Value ? rdr["BoothName"].ToString() : string.Empty,
                        DesignationId = rdr["DesignationId"] != DBNull.Value ? Convert.ToInt32(rdr["DesignationId"]) : 0,
                        Name = rdr["Name"] != DBNull.Value ? rdr["Name"].ToString() : string.Empty,
                        Category = rdr["Category"].ToString(),
                        Cast = rdr["SubCasteName"] != DBNull.Value ? rdr["SubCasteName"].ToString() : string.Empty,
                        SubCasteName = rdr["Cast"].ToString(),
                        CategoryName = rdr["CategoryName"].ToString(),
                        Age = rdr["Age"] != DBNull.Value ? Convert.ToInt32(rdr["Age"]) : 0,
                        Mobile = rdr["Mobile"] != DBNull.Value ? rdr["Mobile"].ToString() : string.Empty,
                        Occupation = rdr["Occupation"] != DBNull.Value ? rdr["Occupation"].ToString() : string.Empty,
                        BoothIncharge = rdr["BoothIncharge"] != DBNull.Value ? rdr["BoothIncharge"].ToString() : string.Empty
                    };

                    list.Add(samiti);
                }
            }
            return list;
        }

        public bool InsertBoothIncharge(BoothSamiti model)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("spBoothSamitiIncharge", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "Insert");
                cmd.Parameters.AddWithValue("@Booth_Id", model.BoothId);
                cmd.Parameters.AddWithValue("@Status", true);

                SqlParameter returnParameter = new SqlParameter();
                returnParameter.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(returnParameter);

                con.Open();
                cmd.ExecuteNonQuery();

                int result = (int)returnParameter.Value;
                return result > 0;
            }
        }


        public List<BoothSamiti> GetAllBoothSamitiIncharges(int? limit = null, int? page = null)
        {
            var list = new List<BoothSamiti>();

            using (var con = new SqlConnection(conn))
            using (var cmd = new SqlCommand("spBoothSamitiIncharge", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SelectAll");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new BoothSamiti
                        {
                            TotalMembers = (int)reader["BoothSamitiMember"],
                            Samiti_Id = (int)reader["Samiti_Id"],
                            BoothId = (int)reader["Booth_Id"],
                            BoothNumber = reader["BoothNumber"] as string,
                            BoothName = reader["BoothName"] as string,
                            BoothIncharge = reader["Incharge_Name"] as string,
                            Mobile = reader["PhoneNumber"] as string,
                            Status = (bool)reader["Status"],
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                            PollingStationBoothName = reader["PollingStationBoothName"] != DBNull.Value ? reader["PollingStationBoothName"].ToString() : ""
                            // You can also map CreatedAt if part of your model
                        });
                    }
                }
            }

            return list;
        }

        public string GetBoothInchargeNameByBoothId(int boothId)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("SELECT Incharge_Name FROM Booth WHERE Booth_Id = @BoothId", con))
            {
                cmd.Parameters.AddWithValue("@BoothId", boothId);
                con.Open();
                var result = cmd.ExecuteScalar();
                return result?.ToString();
            }
        }
        public List<BoothSamiti> GetBoothSamitiByBoothId(int boothId)
        {
            List<BoothSamiti> result = new List<BoothSamiti>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.InsertBoothSamiti", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BoothId", boothId);
                    cmd.Parameters.AddWithValue("@Action", "SelectAll");

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new BoothSamiti
                            {

                                BoothSamiti_Id = Convert.ToInt32(reader["Id"]),
                                BoothId = Convert.ToInt32(reader["BoothId"]),
                                BoothName = reader["BoothName"].ToString(),
                                Name = reader["Name"].ToString(),
                                Designation = reader["Designation"].ToString(),
                                Mobile = reader["Mobile"].ToString(),
                                Age = reader["Age"] != DBNull.Value ? Convert.ToInt32(reader["Age"]) : 0,
                                Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : "",
                                Occupation = reader["Occupation"].ToString(),

                                CategoryName = reader["CategoryName"].ToString(),
                                SubCasteName = reader["SubCasteName"].ToString(),
                                Cast = reader["Cast"].ToString(),
                                DesignationId = Convert.ToInt32(reader["DesignationId"])
                            });
                        }
                    }
                }
            }

            return result;
        }

        public bool UpdateBoothSamiti(BoothSamiti model)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("InsertBoothSamiti", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", "Update");
                        cmd.Parameters.AddWithValue("@Id", model.BoothSamiti_Id);
                        //cmd.Parameters.AddWithValue("@BoothId", model.BoothId);

                        cmd.Parameters.AddWithValue("@DesignationId", model.DesignationId);
                        cmd.Parameters.AddWithValue("@Name", model.Name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Cast", model.Cast ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Category", model.Category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", model.Age);
                        cmd.Parameters.AddWithValue("@Mobile", model.Mobile ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Occupation", model.Occupation ?? (object)DBNull.Value);
                        //cmd.Parameters.AddWithValue("@BoothIncharge", model.BoothIncharge ?? (object)DBNull.Value);

                        SqlParameter returnParameter = new SqlParameter();
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnParameter);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        int rowsAffected = (int)returnParameter.Value;
                        return true; // Always true if no exception
                    }
                }
                catch (Exception ex)
                {
                    // Log exception
                    throw new Exception("Error updating Booth Samiti.", ex);
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                        con.Close();
                }
            }
        }

        public bool SoftDeleteBoothSamiti(int id)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("InsertBoothSamiti", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", "Delete");
                        cmd.Parameters.AddWithValue("@Id", id);

                        SqlParameter returnParameter = new SqlParameter();
                        returnParameter.Direction = ParameterDirection.ReturnValue;
                        cmd.Parameters.Add(returnParameter);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        int rowsAffected = cmd.ExecuteNonQuery(); // This returns number of rows affected
                        return rowsAffected > 0;

                    }
                }
                catch (Exception ex)
                {
                    // Log or handle exception
                    throw new Exception("Error deleting Booth Samiti.", ex);
                }
                finally
                {
                    if (con.State != ConnectionState.Closed)
                        con.Close();
                }
            }
        }

        public List<EffectivePerson> GetAllEffectivePersons(int? limit = null, int? page = null)
        {
            var list = new List<EffectivePerson>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getEffectiveperson");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                //cmd.Parameters.AddWithValue("@BoothNumber", boothId); // 👈 Booth filter

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new EffectivePerson
                        {
                            effectivePersonId = reader["effectivePersonId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["effectivePersonId"]),
                            //Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),
                    
                            Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                            PollingStationBoothName = reader["PollingStationBoothName"] == DBNull.Value ? null : reader["PollingStationBoothName"].ToString(),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            Cast = reader["Cast"] == DBNull.Value ? null : reader["Cast"].ToString(),
                            Booth_Id = reader["Booth_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Booth_Id"]),
                            BoothNumber = reader["BoothNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BoothNumber"]),
                            Category = reader["Category"] == DBNull.Value ? null : reader["Category"].ToString(),
                            Castename = reader["SubCasteName"] == DBNull.Value ? null : reader["SubCasteName"].ToString(),
                            CategoryName = reader["CategoryName"] == DBNull.Value ? null : reader["CategoryName"].ToString(),
                            Mobile = reader["Mobile"] == DBNull.Value ? null : reader["Mobile"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                               DesignationId = reader["Designation"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Designation"]),
                            Designation = reader["Designation"] == DBNull.Value ? "" :reader["Designation"].ToString(),
                            //Status = reader["Status"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["Status"])
                        };

                        list.Add(person);
                    }
                }
            }

            return list;
        }


        public List<EffectivePerson> GetAllDoctorList(int? limit = null, int? page = null)
        {
            var list = new List<EffectivePerson>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getAllDoctorList");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                //cmd.Parameters.AddWithValue("@BoothNumber", boothId); // 👈 Booth filter

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new EffectivePerson
                        {
                            effectivePersonId = reader["effectivePersonId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["effectivePersonId"]),
                            //Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),

                            Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                            PollingStationBoothName = reader["PollingStationBoothName"] == DBNull.Value ? null : reader["PollingStationBoothName"].ToString(),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            Cast = reader["Cast"] == DBNull.Value ? null : reader["Cast"].ToString(),
                            Booth_Id = reader["Booth_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Booth_Id"]),
                            BoothNumber = reader["BoothNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BoothNumber"]),
                            Category = reader["Category"] == DBNull.Value ? null : reader["Category"].ToString(),
                            Castename = reader["SubCasteName"] == DBNull.Value ? null : reader["SubCasteName"].ToString(),
                            CategoryName = reader["CategoryName"] == DBNull.Value ? null : reader["CategoryName"].ToString(),
                            Mobile = reader["Mobile"] == DBNull.Value ? null : reader["Mobile"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                            DesignationId = reader["Designation"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Designation"]),
                            Designation = reader["Designation"] == DBNull.Value ? "" : reader["Designation"].ToString(),
                            //Status = reader["Status"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["Status"])
                        };

                        list.Add(person);
                    }
                }
            }

            return list;
        }

        public List<EffectivePerson> GetAllAdvocateList(int? limit = null, int? page = null)
        {
            var list = new List<EffectivePerson>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getAllAdvocateList");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                //cmd.Parameters.AddWithValue("@BoothNumber", boothId); // 👈 Booth filter

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new EffectivePerson
                        {
                            effectivePersonId = reader["effectivePersonId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["effectivePersonId"]),
                            //Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),

                            Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                            PollingStationBoothName = reader["PollingStationBoothName"] == DBNull.Value ? null : reader["PollingStationBoothName"].ToString(),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            Cast = reader["Cast"] == DBNull.Value ? null : reader["Cast"].ToString(),
                            Booth_Id = reader["Booth_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Booth_Id"]),
                            BoothNumber = reader["BoothNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BoothNumber"]),
                            Category = reader["Category"] == DBNull.Value ? null : reader["Category"].ToString(),
                            Castename = reader["SubCasteName"] == DBNull.Value ? null : reader["SubCasteName"].ToString(),
                            CategoryName = reader["CategoryName"] == DBNull.Value ? null : reader["CategoryName"].ToString(),
                            Mobile = reader["Mobile"] == DBNull.Value ? null : reader["Mobile"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                            DesignationId = reader["Designation"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Designation"]),
                            Designation = reader["Designation"] == DBNull.Value ? "" : reader["Designation"].ToString(),
                            //Status = reader["Status"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["Status"])
                        };

                        list.Add(person);
                    }
                }
            }

            return list;
        }

        public List<EffectivePerson> GetAllGovtEmpList(int? limit = null, int? page = null)
        {
            var list = new List<EffectivePerson>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getAllGovtEmpList");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                //cmd.Parameters.AddWithValue("@BoothNumber", boothId); // 👈 Booth filter

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new EffectivePerson
                        {
                            effectivePersonId = reader["effectivePersonId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["effectivePersonId"]),
                            //Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),

                            Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                            PollingStationBoothName = reader["PollingStationBoothName"] == DBNull.Value ? null : reader["PollingStationBoothName"].ToString(),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            Cast = reader["Cast"] == DBNull.Value ? null : reader["Cast"].ToString(),
                            Booth_Id = reader["Booth_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Booth_Id"]),
                            BoothNumber = reader["BoothNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BoothNumber"]),
                            Category = reader["Category"] == DBNull.Value ? null : reader["Category"].ToString(),
                            Castename = reader["SubCasteName"] == DBNull.Value ? null : reader["SubCasteName"].ToString(),
                            CategoryName = reader["CategoryName"] == DBNull.Value ? null : reader["CategoryName"].ToString(),
                            Mobile = reader["Mobile"] == DBNull.Value ? null : reader["Mobile"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                            DesignationId = reader["Designation"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Designation"]),
                            Designation = reader["Designation"] == DBNull.Value ? "" : reader["Designation"].ToString(),
                            //Status = reader["Status"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["Status"])
                        };

                        list.Add(person);
                    }
                }
            }

            return list;
        }



        public List<EffectivePerson> GetAllEffectivePersonsforapi(int? limit = null, int? page = null)
        {
            var list = new List<EffectivePerson>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getEffectiveperson");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                //cmd.Parameters.AddWithValue("@BoothNumber", boothId); // 👈 Booth filter

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new EffectivePerson
                        {
                            effectivePersonId = reader["effectivePersonId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["effectivePersonId"]),
                            Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),
                            Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            Cast = reader["Cast"] == DBNull.Value ? null : reader["Cast"].ToString(),
                            Booth_Id = reader["Booth_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Booth_Id"]),
                            BoothNumber = reader["BoothNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BoothNumber"]),
                            Category = reader["Category"] == DBNull.Value ? null : reader["Category"].ToString(),
                            Castename = reader["SubCasteName"] == DBNull.Value ? null : reader["SubCasteName"].ToString(),
                            CategoryName = reader["CategoryName"] == DBNull.Value ? null : reader["CategoryName"].ToString(),
                            Mobile = reader["Mobile"] == DBNull.Value ? null : reader["Mobile"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            VillageListId = reader["VillageId"] == DBNull.Value ? null : reader["VillageId"].ToString(),
                            VillageListName = reader["VillageName"] == DBNull.Value ? null : reader["VillageName"].ToString(),
                            //village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : ""
                            //Status = reader["Status"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(reader["Status"])
                        };

                        list.Add(person);
                    }
                }
            }

            return list;
        }
        public List<EffectivePerson> GetEffectivePersonsPaged(FilterModel filter, int? type,int? pageNumber, int? pageSize, int? limit = null, int? page = null)
        {
            var list = new List<EffectivePerson>();


            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "GET");
                cmd.Parameters.AddWithValue("@boothIds", filter.boothIds != null ? filter.boothIds : null);
                cmd.Parameters.AddWithValue("@casteIds", filter.casteIds != null ? filter.casteIds : null);
                cmd.Parameters.AddWithValue("@villageIds", filter.villageIds != null ? filter.villageIds : null);

                // new:
                cmd.Parameters.AddWithValue("@Designation", type ?? (object)DBNull.Value);

                // paging parameters
                cmd.Parameters.AddWithValue("@PageNumber", pageNumber.HasValue ?pageNumber:null);
                cmd.Parameters.AddWithValue("@PageSize", pageSize.HasValue?pageSize:null);
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);


                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var person = new EffectivePerson
                        {
                            effectivePersonId = reader["effectivePersonId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["effectivePersonId"]),
                            Designation = reader["Designation"] == DBNull.Value ? null : reader["Designation"].ToString(),
                            Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            Cast = reader["Cast"] == DBNull.Value ? null : reader["Cast"].ToString(),
                            Booth_Id = reader["Booth_Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Booth_Id"]),
                            BoothNumber = reader["BoothNumber"] == DBNull.Value ? 0 : Convert.ToInt32(reader["BoothNumber"]),
                            Category = reader["Category"] == DBNull.Value ? null : reader["Category"].ToString(),
                            Castename = reader["SubCasteName"] == DBNull.Value ? null : reader["SubCasteName"].ToString(),
                            CategoryName = reader["CategoryName"] == DBNull.Value ? null : reader["CategoryName"].ToString(),
                            Mobile = reader["Mobile"] == DBNull.Value ? null : reader["Mobile"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                            PollingStationBoothName = reader["PollingStationBoothName"] != DBNull.Value ? reader["PollingStationBoothName"].ToString() : "",
                            SectorId = reader["SectorId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SectorId"]),
                            SectorName = reader["SectorName"] == DBNull.Value ? "" : reader["SectorName"].ToString(),
                            SectorIncharge = reader["Incharge_Name"] == DBNull.Value ? "" : reader["Incharge_Name"].ToString(),
                        };

                        list.Add(person);
                    }
                }


            }

            return list;


        }

        public bool InsertEffectivePerson(EffectivePerson model)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "INSERT");
                cmd.Parameters.AddWithValue("@Designation", model.Designation ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", model.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cast", model.Cast ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Booth_Id", model.Booth_Id);
                cmd.Parameters.AddWithValue("@Category", model.Category ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@VillageListId", model.VillageListId ?? (object)DBNull.Value);
                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery(); // <- This is enough
                return rowsAffected > 0;
            }
        }
        public bool UpdateEffectivePerson(EffectivePerson model)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "UPDATE");
                cmd.Parameters.AddWithValue("@effectivePersonId", model.effectivePersonId);
                cmd.Parameters.AddWithValue("@Designation", model.Designation ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Name", model.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Cast", model.Cast ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Booth_Id", model.Booth_Id);
                cmd.Parameters.AddWithValue("@Category", model.Category ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@VillageListId", model.VillageListId ?? (object)DBNull.Value);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public EffectivePerson GetEffectivePersonById(int id)
        {
            EffectivePerson person = null;

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@effectivePersonId", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    person = new EffectivePerson
                    {
                        effectivePersonId = Convert.ToInt32(reader["effectivePersonId"]),
                        Designation = reader["Designation"]?.ToString(),
                        Name = reader["Name"]?.ToString(),
                        Cast = reader["Cast"]?.ToString(),
                        Booth_Id = Convert.ToInt32(reader["Booth_Id"]),
                        BoothNumber = Convert.ToInt32(reader["BoothNumber"]),
                        Castename = reader["SubCasteName"].ToString(),
                        CategoryName = reader["CategoryName"].ToString(),
                        Category = reader["Category"]?.ToString(),
                        Mobile = reader["Mobile"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        VillageListId = reader["VillageId"]?.ToString(),
                         VillageListName= reader["villageName"]?.ToString()
                    };
                }
            }

            return person;
        }

        public bool DeleteEffectivePerson(int id)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_EffectivePerson_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Delete");
                cmd.Parameters.AddWithValue("@effectivePersonId", id);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public bool InsertSeniorOrDisabled(SeniorOrDisabled model)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@action", "insert");
                cmd.Parameters.AddWithValue("@id", 0);
                cmd.Parameters.AddWithValue("@boothno", model.boothId);
                cmd.Parameters.AddWithValue("@SeniorOrDisabled", model.SeniorOrDisableddata);
                cmd.Parameters.AddWithValue("@name", model.Name );
                cmd.Parameters.AddWithValue("@address", model.Address );
                cmd.Parameters.AddWithValue("@caste", model.Caste );
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@mobile", model.Mobile );
                cmd.Parameters.AddWithValue("@VillageListId", model.VillageListId);
                //cmd.Parameters.AddWithValue("@createdAt", DBNull.Value);
                //cmd.Parameters.AddWithValue("@updatedAt", DBNull.Value);
                //cmd.Parameters.AddWithValue("@status", 1);
                //cmd.Parameters.AddWithValue("@BoothNumber", 0); // Not used in insert

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }



        public bool UpdateSeniorOrDisabled(SeniorOrDisabled model)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", model.Id);
                cmd.Parameters.AddWithValue("@SeniorOrDisabled", model.SeniorOrDisableddata);

                cmd.Parameters.AddWithValue("@name", model.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@address", model.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@caste", model.Caste);
                cmd.Parameters.AddWithValue("@boothno", model.boothId);
                cmd.Parameters.AddWithValue("@Category", model.Category);
                cmd.Parameters.AddWithValue("@VillageListId", model.VillageListId);
                cmd.Parameters.AddWithValue("@mobile", model.Mobile ?? (object)DBNull.Value);
                //cmd.Parameters.AddWithValue("@createdAt", DBNull.Value);
                cmd.Parameters.AddWithValue("@updatedAt", DBNull.Value);
                //cmd.Parameters.AddWithValue("@status", 1);
                cmd.Parameters.AddWithValue("@action", "update");

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
        public List<SeniorOrDisabled> GetAllSeniorOrDisabled(int? limit = null, int? page = null)
        {
            var list = new List<SeniorOrDisabled>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "selectAllseniorordisabled");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new SeniorOrDisabled
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                SeniorOrDisabledStatus = reader["SeniorOrDisabled"].ToString(),
                                PollingStationBoothName = reader["PollingStationBoothName"]!=DBNull.Value? reader["PollingStationBoothName"].ToString():"",
                                SeniorOrDisabledType = reader["Type"].ToString(),
                                Name = reader["name"].ToString(),
                                SubCasteName = reader["SubCasteName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                Address = reader["address"].ToString(),
                                Caste = reader["caste"] != DBNull.Value ? Convert.ToInt32(reader["caste"]) : 0,
                                BoothNumber = reader["BoothNumber"] != DBNull.Value ? Convert.ToInt32(reader["BoothNumber"]) : 0,
                                Category = reader["Category"] != DBNull.Value ? Convert.ToInt32(reader["Category"]) : 0,
                                Mobile = reader["mobile"].ToString(),
                                CreatedAt = reader["createdAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["createdAt"]) : null,
                                UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                Status = reader["status"] != DBNull.Value ? Convert.ToInt32(reader["status"]) : 0,
                                village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : ""
                            };

                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public List<SeniorOrDisabled> GetAllSeniorList(int? limit = null, int? page = null)
        {
            var list = new List<SeniorOrDisabled>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "selectAllseniorData");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new SeniorOrDisabled
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                SeniorOrDisabledStatus = reader["SeniorOrDisabled"].ToString(),
                                PollingStationBoothName = reader["PollingStationBoothName"] != DBNull.Value ? reader["PollingStationBoothName"].ToString() : "",
                                SeniorOrDisabledType = reader["Type"].ToString(),
                                Name = reader["name"].ToString(),
                                SubCasteName = reader["SubCasteName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                Address = reader["address"].ToString(),
                                Caste = reader["caste"] != DBNull.Value ? Convert.ToInt32(reader["caste"]) : 0,
                                BoothNumber = reader["BoothNumber"] != DBNull.Value ? Convert.ToInt32(reader["BoothNumber"]) : 0,
                                Category = reader["Category"] != DBNull.Value ? Convert.ToInt32(reader["Category"]) : 0,
                                Mobile = reader["mobile"].ToString(),
                                CreatedAt = reader["createdAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["createdAt"]) : null,
                                UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                Status = reader["status"] != DBNull.Value ? Convert.ToInt32(reader["status"]) : 0,
                                village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : ""
                            };

                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }


        public List<SeniorOrDisabled> GetAllDisabledList(int? limit = null, int? page = null)
        {
            var list = new List<SeniorOrDisabled>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "selectAllDisabledData");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var item = new SeniorOrDisabled
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                SeniorOrDisabledStatus = reader["SeniorOrDisabled"].ToString(),
                                PollingStationBoothName = reader["PollingStationBoothName"] != DBNull.Value ? reader["PollingStationBoothName"].ToString() : "",
                                SeniorOrDisabledType = reader["Type"].ToString(),
                                Name = reader["name"].ToString(),
                                SubCasteName = reader["SubCasteName"].ToString(),
                                CategoryName = reader["CategoryName"].ToString(),
                                Address = reader["address"].ToString(),
                                Caste = reader["caste"] != DBNull.Value ? Convert.ToInt32(reader["caste"]) : 0,
                                BoothNumber = reader["BoothNumber"] != DBNull.Value ? Convert.ToInt32(reader["BoothNumber"]) : 0,
                                Category = reader["Category"] != DBNull.Value ? Convert.ToInt32(reader["Category"]) : 0,
                                Mobile = reader["mobile"].ToString(),
                                CreatedAt = reader["createdAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["createdAt"]) : null,
                                UpdatedAt = reader["updatedAt"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["updatedAt"]) : null,
                                Status = reader["status"] != DBNull.Value ? Convert.ToInt32(reader["status"]) : 0,
                                village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : ""
                            };

                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }


        public List<SeniorOrDisabled> GetSeniorOrDisabled(int VidhanSabhaId,FilterModel filter,int? type, int? limit=null, int? page=null)
        {
            var list = new List<SeniorOrDisabled>();
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                cmd.Parameters.AddWithValue("@action", "selectAll");
                cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                cmd.Parameters.AddWithValue("@SeniorOrDisabled", type ?? (object)DBNull.Value);
                  cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
  cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                // New paging params
                //cmd.Parameters.AddWithValue("@PageNumber", pageNumber ?? (object)DBNull.Value);
                //cmd.Parameters.AddWithValue("@PageSize", pageSize ?? (object)DBNull.Value);

              

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new SeniorOrDisabled
                        {
                            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                            SeniorOrDisabledStatus = reader["Type"]?.ToString(),
                            Name = reader["name"]?.ToString(),
                            SubCasteName = reader["SubCasteName"]?.ToString(),
                            CategoryName = reader["CategoryName"]?.ToString(),
                            Address = reader["address"]?.ToString(),
                            Caste = reader["caste"] != DBNull.Value ? Convert.ToInt32(reader["caste"]) : 0,
                            SeniorOrDisableddata = reader["SeniorOrDisabled"] != DBNull.Value ? Convert.ToInt32(reader["SeniorOrDisabled"]) : 0,
                            BoothNumber = reader["boothno"] != DBNull.Value ? Convert.ToInt32(reader["boothno"]) : 0,
                            Category = reader["Category"] != DBNull.Value ? Convert.ToInt32(reader["Category"]) : 0,
                            Mobile = reader["mobile"]?.ToString(),
                            PollingStationBoothName = reader["PollingStationBoothName"] !=DBNull.Value?reader["PollingStationBoothName"]?.ToString():"",
                            CreatedAt = reader["createdAt"] != DBNull.Value
                                        ? (DateTime?)Convert.ToDateTime(reader["createdAt"])
                                        : null,
                            UpdatedAt = reader["updatedAt"] != DBNull.Value
                                        ? (DateTime?)Convert.ToDateTime(reader["updatedAt"])
                                        : null,
                            Status = reader["status"] != DBNull.Value ? Convert.ToInt32(reader["status"]) : 0,
                            VillageListId = reader["VillageId"] != DBNull.Value ? reader["VillageId"].ToString() : "",
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                            SectorId = reader["SectorId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["SectorId"]),
                            SectorName = reader["SectorName"] == DBNull.Value ? "" : reader["SectorName"].ToString(),
                            SectorIncharge = reader["Incharge_Name"] == DBNull.Value ? "" : reader["Incharge_Name"].ToString(),

                        };

                        list.Add(item);
                    }
                }

            }

            return list;

        }



        public List<SeniorOrDisabled> GetSeniorOrDisabledforapi(int? limit = null, int? page = null)
        {
            var list = new List<SeniorOrDisabled>();
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                //cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                //cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                cmd.Parameters.AddWithValue("@action", "selectAllseniorordisabled");
                //cmd.Parameters.AddWithValue("@SeniorOrDisabled", type ?? (object)DBNull.Value);

                //// New paging params
                //cmd.Parameters.AddWithValue("@PageNumber", pageNumber ?? (object)DBNull.Value);
                //cmd.Parameters.AddWithValue("@PageSize", pageSize ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);


                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new SeniorOrDisabled
                        {
                            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                            //SeniorOrDisabledStatus = reader["Type"]?.ToString(),
                            Name = reader["name"]?.ToString(),
                            SubCasteName = reader["SubCasteName"]?.ToString(),
                            SeniorOrDisableddata = Convert.ToInt32(reader["SeniorOrDisabled"]),
                            SeniorOrDisabledType = reader["Type"].ToString(),
                            CategoryName = reader["CategoryName"]?.ToString(),
                            Address = reader["address"]?.ToString(),
                            boothId = Convert.ToInt32(reader["Booth_Id"]),
                            Caste = reader["caste"] != DBNull.Value ? Convert.ToInt32(reader["caste"]) : 0,
                            BoothNumber = reader["BoothNumber"] != DBNull.Value ? Convert.ToInt32(reader["BoothNumber"]) : 0,
                            Category = reader["Category"] != DBNull.Value ? Convert.ToInt32(reader["Category"]) : 0,
                            Mobile = reader["mobile"]?.ToString(),
                            CreatedAt = reader["createdAt"] != DBNull.Value
                                        ? (DateTime?)Convert.ToDateTime(reader["createdAt"])
                                        : null,
                            UpdatedAt = reader["updatedAt"] != DBNull.Value
                                        ? (DateTime?)Convert.ToDateTime(reader["updatedAt"])
                                        : null,
                            Status = reader["status"] != DBNull.Value ? Convert.ToInt32(reader["status"]) : 0,
                            village = reader["VillageName"] != DBNull.Value ? reader["VillageName"].ToString() : "",
                           VillageListId  = reader["VillageId"] != DBNull.Value ? reader["VillageId"].ToString() : ""
                        };

                        list.Add(item);
                    }
                }

            }

            return list;

        }


        public bool DeleteSeniorOrDisabled(int Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "delete");

                con.Open();

                cmd.Parameters.AddWithValue("Id", Id);
                //cmd.Parameters.AddWithValue("@Status", "1");

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return true;
        }

        public int TotalMandalCount()
        {
            int mandalcount = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Mandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "TotalMandalCount");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                mandalcount = Convert.ToInt32(sdr["MandalCount"]);
                            }
                        }
                    }
                }
                return mandalcount;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        public bool AddNewVoters(NewVoters voter)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NewVoters", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                cmd.Parameters.AddWithValue("@Actions", voter.Id > 0 ? "UpdateVoters" : "AddVoters");
                cmd.Parameters.AddWithValue("@Id", voter.Id);
                cmd.Parameters.AddWithValue("@VotersName", voter.Name);
                cmd.Parameters.AddWithValue("@FatherName", voter.FatherName);
                //cmd.Parameters.AddWithValue("@DOB", voter.DOB);
                cmd.Parameters.AddWithValue("@DOB", voter.DOB.HasValue ? (object)voter.DOB.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@totalAge", voter.totalage);
                cmd.Parameters.AddWithValue("@Education", voter.Education);
                cmd.Parameters.AddWithValue("@MobileNumber", voter.MobileNumber);

                // ✅ Add these new parameters
                cmd.Parameters.AddWithValue("@booth_no", voter.Booth_Id);
                cmd.Parameters.AddWithValue("@Caste", voter.SubCasteId);
                cmd.Parameters.AddWithValue("@CasteCategory", voter.CasteId);
                cmd.Parameters.AddWithValue("@VillageListId", voter.VillageListId);

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public List<NewVoters> GetNewVoters(int VidhanSabhaId,FilterModel filter,int? limit,int? page)
        {
            List<NewVoters> newvoterslist = new List<NewVoters>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_NewVoters", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Actions", "getNewVoters");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            NewVoters voters = new NewVoters
                            {
                                Id = Convert.ToInt32(rd["Id"]),
                                Name = rd["VotersName"]?.ToString(),
                                FatherName = rd["FatherName"]?.ToString(),
                                caste = rd["SubCasteName"]?.ToString(),
                                CategoryName = rd["CategoryName"]?.ToString(),
                                DOB = rd["DOB"] != DBNull.Value ? Convert.ToDateTime(rd["DOB"]) : DateTime.MinValue,
                                MobileNumber = rd["MobileNumber"]?.ToString(),
                                BoothNumber = rd["BoothNumber"]?.ToString(),
                                SubCasteName = rd["SubCasteName"]?.ToString(),
                                totalage = rd["totalAge"]?.ToString(),
                                //village = rd["villageName"] != DBNull.Value ? rd["villageName"].ToString() : "",
                                //PollingStationBoothName = rd["PollingStationBoothName"] != DBNull.Value ? rd["PollingStationBoothName"].ToString() : "",
                                Booth_Id = Convert.ToInt32(rd["Booth_id"]),
                                CasteId = Convert.ToInt32(rd["CasteCategory"]),
                                SubCasteId = Convert.ToInt32(rd["caste"]),
                                SectorId = rd["Id"] == DBNull.Value ? 0 : Convert.ToInt32(rd["Id"]),
                                SectorName = rd["SectorName"] == DBNull.Value ? "" : rd["SectorName"].ToString(),
                                SectorIncharge = rd["Incharge_Name"] == DBNull.Value ? "" : rd["Incharge_Name"].ToString(),
                                VillageListId = rd["VillageId"] == DBNull.Value ? "" : rd["VillageId"].ToString(),
                                Village = rd["villageName"] == DBNull.Value ? "" : rd["villageName"].ToString(),
                            };
                            newvoterslist.Add(voters);
                        }
                        rd.Close();
                    }
                }
            }
            finally
            {
                con.Close();
            }

            return newvoterslist;
        }
        //Delete New Voters

        public bool deleteNewVoter(int Id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("SP_NewVoters", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Actions", "DeleteVoters");

                con.Open();

                cmd.Parameters.AddWithValue("Id", Id);
                //cmd.Parameters.AddWithValue("@Status", "1");

                int result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return true;
        }

        public NewVoters getVoterById(int Id)
        {
            NewVoters voter = new NewVoters();

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_NewVoters", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Actions", "VotersGetById");
                cmd.Parameters.AddWithValue("@Id", Id);

                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.Read())
                {
                    voter.Booth_Id = Convert.ToInt32(rd["booth_no"]);
                    voter.Id = Convert.ToInt32(rd["Id"]);
                    voter.BoothNumber = rd["BoothNumber"].ToString();
                    voter.Name = rd["VotersName"]?.ToString();
                    voter.FatherName = rd["FatherName"]?.ToString();
                    voter.CasteId = Convert.ToInt32(rd["CasteCategory"]);
                    voter.SubCasteId = Convert.ToInt32(rd["Caste"]);
                    voter.DOB = rd["DOB"] != DBNull.Value ? Convert.ToDateTime(rd["DOB"]) : (DateTime?)null;
                    voter.Education = rd["Education"]?.ToString();
                    voter.MobileNumber = rd["MobileNumber"]?.ToString();
                    voter.totalage = rd["totalAge"]?.ToString();
                    voter.CategoryName = rd["CategoryName"]?.ToString();
                    voter.SubCasteName = rd["SubCasteName"]?.ToString();
                    voter.VillageListId = rd["VillageId"]?.ToString();
                    voter.Village = rd["villageName"]?.ToString();
            
                    voter.CasteId = Convert.ToInt32(rd["CasteCategory"]);
                    voter.SubCasteId = Convert.ToInt32(rd["caste"]);

                }

                rd.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return voter;
        }
        //addboothdescription Services--deep singh

        public BoothVotersDes getBoohDesById(int Id)
        {
            BoothVotersDes BoohDes = new BoothVotersDes();

            using (SqlConnection con = new SqlConnection(conn))
            {
                try
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Actions", "getBoothVoterDesById");
                        cmd.Parameters.AddWithValue("@Id", Id);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                BoohDes.Id = Convert.ToInt32(rd["Id"]);
                                BoohDes.BoothName = rd["BoothName"]?.ToString();
                                BoohDes.BoothNumber = rd["BoothNumber"]?.ToString();
                                BoohDes.VillageListId = rd["VillageId"]?.ToString();
                                BoohDes.VillageName = rd["VillageName"]?.ToString();
                                BoohDes.BoothNumber = rd["BoothNumber"]?.ToString();
                                BoohDes.TotalVoters = rd["TotalVoters"] != DBNull.Value ? Convert.ToInt32(rd["TotalVoters"]) : 0;
                                BoohDes.TotalMan = rd["TotalMan"] != DBNull.Value ? Convert.ToInt32(rd["TotalMan"]) : 0;
                                BoohDes.TotalWoman = rd["TotalWoman"] != DBNull.Value ? Convert.ToInt32(rd["TotalWoman"]) : 0;
                                BoohDes.TotalOther = rd["TotalOther"] != DBNull.Value ? Convert.ToInt32(rd["TotalOther"]) : 0;
                                BoohDes.BoothId = rd["BoothId"] != DBNull.Value ? Convert.ToInt32(rd["BoothId"]) : 0;

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error or handle it as needed
                    throw;
                }
            }

            return BoohDes;
        }


        public List<BoothVotersDes> GetBoothDropdown(int VidhanSabhaId)
        {
            var boothList = new List<BoothVotersDes>();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Actions", "SELECT_BOOTH_LIST");
                cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    var booth = new BoothVotersDes
                    {
                        BoothId = Convert.ToInt32(rdr["Booth_Id"]),
                        BoothName = rdr["BoothName"].ToString(),
                        BoothNumber = rdr["BoothNumber"].ToString()
                    };

                    boothList.Add(booth);
                }

                rdr.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error: " + ex.Message);

            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return boothList;
        }

        public bool addOrUpdateBoothVoterDes(BoothVotersDes description)
        {
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Correct the Actions param logic: Id > 0 means Update, else Insert
                cmd.Parameters.AddWithValue("@Actions", description.Id > 0 ? "UpdateBoothVoter" : "insertBoothVoterDes");
                cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(description.Id));
                cmd.Parameters.AddWithValue("@BoothName", description.BoothName);
                cmd.Parameters.AddWithValue("@BoothNumber", description.BoothNumber);
                cmd.Parameters.AddWithValue("@VillageId", description.VillageListId);
                cmd.Parameters.AddWithValue("@TotalVoters", Convert.ToInt32(description.TotalVoters));
                cmd.Parameters.AddWithValue("@TotalMan", Convert.ToInt32(description.TotalMan));
                cmd.Parameters.AddWithValue("@TotalWoman", Convert.ToInt32(description.TotalWoman));
                cmd.Parameters.AddWithValue("@TotalOther", Convert.ToInt32(description.TotalOther));
                cmd.Parameters.AddWithValue("@BoothId", Convert.ToInt32(description.BoothId));

                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        public List<BoothVotersDes> getBoothVoterDes(int VidhansabhaId,int? limit,int? page, string boothId = "all")
        {
            List<BoothVotersDes> BoothVoterDesList = new List<BoothVotersDes>();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Actions", "getBoothVoterDes");
                cmd.Parameters.AddWithValue("@VidhansabhaId", VidhansabhaId);
                cmd.Parameters.AddWithValue("@BoothNo", boothId); // comma-separated IDs ya 'all'
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    BoothVotersDes voter = new BoothVotersDes
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        BoothName = reader["BoothName"].ToString(),
                        BoothNumber = reader["BoothNumber"].ToString(),
                        VillageListId= reader["VillageId"].ToString(),
                        VillageName = reader["villageName"].ToString(),
                        PollingStationBoothName =reader["PollingStationBoothName"] !=DBNull.Value? reader["PollingStationBoothName"].ToString():"",
                        TotalVoters = Convert.ToInt32(reader["TotalVoters"]),
                        TotalMan = Convert.ToInt32(reader["TotalMan"]),
                        TotalWoman = Convert.ToInt32(reader["TotalWoman"]),
                        TotalOther = Convert.ToInt32(reader["TotalOther"]),
                        BoothId = Convert.ToInt32(reader["BoothId"])
                    };
                    BoothVoterDesList.Add(voter);
                }
            }
            finally
            {
                con.Close();
            }

            return BoothVoterDesList;
        }

        public bool DeleteBoothVoterDes(int id)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Actions", "deleteBoothVoterDes");
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //today -deep singh


        public bool AddCastVotersDes(CastVotersDes model)
        {
            bool allSaved = true;

            try
            {
                con.Open();

                for (int i = 0; i < model.CastName.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();  // clear BEFORE adding params

                    cmd.Parameters.AddWithValue("@Actions", "AddCastVoterDesColumn");
                    // cmd.Parameters.AddWithValue("@Id", 0);
                    cmd.Parameters.AddWithValue("@CastName", model.CastName[i]);
                    cmd.Parameters.AddWithValue("@Number", model.Number[i]);
                    cmd.Parameters.AddWithValue("@VoterDes", model.VoterDes);

                    int result = cmd.ExecuteNonQuery();

                    if (result <= 0)
                    {
                        allSaved = false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return allSaved;
        }





        #region Activity & Social Media Post

        public SeniorOrDisabled GetSeniorOrDisabledById(int id)
        {
            SeniorOrDisabled data = null;

            using (SqlConnection con = new SqlConnection(conn))
            {
                string query = @"SELECT Id, SeniorOrDisabled, Name, Address, Caste, Mobile 
                         FROM tbl_SeniorCitizenAndDisabled 
                         WHERE Id = @Id AND Status = 1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data = new SeniorOrDisabled
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                SeniorOrDisabledStatus = reader["SeniorOrDisabled"].ToString(),
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                Caste = Convert.ToInt32(reader["Caste"]),
                                Mobile = reader["Mobile"].ToString()
                            };
                        }
                    }
                }
            }

            return data;
        }
        //Activity
        public bool AddOrUpdateActivity(Activities model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_ManageActivity", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ActionType", model.ActivityId > 0 ? "Update" : "Insert");
                        cmd.Parameters.AddWithValue("@ActivityId", model.ActivityId);
                        cmd.Parameters.AddWithValue("@Title", model.Title);
                        cmd.Parameters.AddWithValue("@Description", model.Description);
                        cmd.Parameters.AddWithValue("@ActivityDate", model.ActivityDate);
                        cmd.Parameters.AddWithValue("@VideoUrl", (object)model.VideoUrl ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ImagePaths", JsonConvert.SerializeObject(model.ImagePaths));
                        cmd.Parameters.AddWithValue("@Status", model.Status);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Activities> GetAllActivities(int? limit = null, int? page = null)
        {
            List<Activities> activities = new List<Activities>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ManageActivity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "GetAll");
                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            activities.Add(new Activities
                            {
                                ActivityId = Convert.ToInt32(reader["ActivityId"]),
                                Title = reader["Title"].ToString(),
                                Description = reader["Description"].ToString(),
                                ActivityDate = Convert.ToDateTime(reader["ActivityDate"]),
                                VideoUrl = reader["YouTubeLink"]?.ToString(),
                                LinkType = reader["LinkType"]?.ToString(),
                                VideoFile = reader["NonYouTubeLink"]?.ToString(),
                                ImagePaths = JsonConvert.DeserializeObject<List<string>>(reader["ImagePaths"]?.ToString() ?? "[]"),
                                Status = Convert.ToBoolean(reader["Status"])
                            });
                        }
                    }
                }
            }

            return activities;
        }
        public bool DeleteActivity(int id)
        {
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ManageActivity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "Delete");
                    cmd.Parameters.AddWithValue("@ActivityId", id);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public Activities GetActivityById(int id)
        {
            Activities activity = null;

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ManageActivity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ActionType", "GetById");
                    cmd.Parameters.AddWithValue("@ActivityId", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            activity = new Activities
                            {
                                ActivityId = Convert.ToInt32(reader["ActivityId"]),
                                Title = reader["Title"]?.ToString(),
                                Description = reader["Description"]?.ToString(),
                                ActivityDate = Convert.ToDateTime(reader["ActivityDate"]),
                                VideoUrl = reader["VideoUrl"]?.ToString(),
                                ImagePaths = JsonConvert.DeserializeObject<List<string>>(reader["ImagePaths"]?.ToString() ?? "[]"),
                                Status = Convert.ToBoolean(reader["Status"])
                            };
                        }
                    }
                }
            }

            return activity;
        }
        public int InsertPost(SocialMediaPost post, string boothIds, string sectorIds, string platformCsv)
        {
          
            int newPostId = 0;

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_SocialMediaPost_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@title", post.title ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Description", post.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@image", post.PostUrl ?? (object)DBNull.Value);


                cmd.Parameters.AddWithValue("@Platform", string.IsNullOrEmpty(platformCsv) ? (object)DBNull.Value : platformCsv);

                //cmd.Parameters.AddWithValue("@Link", post.PostUrl ?? (object)DBNull.Value);


                cmd.Parameters.AddWithValue("@Status", post.Status == "Posted" ? 1 : 0);


                cmd.Parameters.AddWithValue("@BoothIds", string.IsNullOrEmpty(boothIds) ? (object)DBNull.Value : boothIds);
                cmd.Parameters.AddWithValue("@SectorIds", string.IsNullOrEmpty(sectorIds) ? (object)DBNull.Value : sectorIds);
                cmd.Parameters.AddWithValue("@PostId", post.PostId);
                //cmd.Parameters.AddWithValue("@IsPost", ispost);
                cmd.Parameters.AddWithValue("@Action", post.PostId > 0 ? "UPDATE" : "INSERT");

                con.Open();
                object result = cmd.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    newPostId = id;
                }
            }

            return newPostId;
        }

        public List<SocialMediaPost> GetAllPosts(int? limit = null, int? page = null)
        {
            List<SocialMediaPost> posts = new List<SocialMediaPost>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_SocialMediaPost_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETALL");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SocialMediaPost post = new SocialMediaPost
                        {
                            PostId = Convert.ToInt32(reader["PostId"]),
                            title = reader["title"] == DBNull.Value ? null : reader["title"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            Platform = reader["Platform"] == DBNull.Value ? new string[] { } : reader["Platform"].ToString().Split(','),
                            PostUrl = reader["image"] == DBNull.Value ? null : reader["image"].ToString()
                        };


                        posts.Add(post);
                    }
                }
            }

            return posts;
        }

        public bool UpdatePost(SocialMediaPost post, string boothIds, string sectorIds)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_SocialMediaPost_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "UPDATE");
                cmd.Parameters.AddWithValue("@PostId", post.PostId);
                cmd.Parameters.AddWithValue("@image", post.PostUrl);
                cmd.Parameters.AddWithValue("@title", post.title);

                cmd.Parameters.AddWithValue("@Description", post.Description ?? (object)DBNull.Value);
                //cmd.Parameters.AddWithValue("@Platform", post.Platform ?? (object)DBNull.Value);
                //cmd.Parameters.AddWithValue("@Link", post.Link ?? (object)DBNull.Value);
                // Convert string array to comma-separated string
                cmd.Parameters.AddWithValue("@Platform", post.Platform != null ? string.Join(",", post.Platform) : (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@Status", post.Status == "Posted" ? 1 : 0);
                cmd.Parameters.AddWithValue("@BoothIds", boothIds ?? "");
                cmd.Parameters.AddWithValue("@SectorIds", sectorIds ?? "");

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public bool DeletePost(int postId)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_SocialMediaPost_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@PostId", postId);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public SocialMediaPost GetPostById(int? postId)
        {
            SocialMediaPost post = null;

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("usp_SocialMediaPost_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETById");
                cmd.Parameters.AddWithValue("@PostId", postId);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        post = new SocialMediaPost
                        {
                            PostId = Convert.ToInt32(reader["PostId"]),
                            title = reader["title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Platform = reader["Platform"] == DBNull.Value ? new string[] { } : reader["Platform"].ToString().Split(','),
                            PostUrl = reader["image"].ToString(),


                        };

                        // Parse BoothIds JSON string outside the initializer
                        string boothJson = reader["BoothIds"].ToString();

                        // Deserialize to a list
                        List<BoothWrapper> boothWrappers = JsonConvert.DeserializeObject<List<BoothWrapper>>(boothJson);

                        // Safely assign BoothIds (handles nulls and casts properly)
                        post.BoothIds = boothWrappers?
                            .Select(b => (int?)b.BoothId) // Cast to nullable int
                            .ToList() ?? new List<int?>();

                        string sectorJson = reader["SectorIds"].ToString();

                        List<SectorWrapper> sectorWrappers = JsonConvert.DeserializeObject<List<SectorWrapper>>(sectorJson);

                        post.SectorIds = sectorWrappers?
                            .Select(s => (int?)s.SectorId)  // Cast to nullable int
                            .ToList() ?? new List<int?>();
                    }
                }
            }

            return post;
        }

        public List<SeniorOrDisabled> GetSeniorCitizenReport(FilterModel filter, int? limit, int? page)
        {
            List<SeniorOrDisabled> list = new List<SeniorOrDisabled>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetSeniorCitizen");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                    cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            SeniorOrDisabled report = new SeniorOrDisabled
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                BoothNo = dr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(dr["BoothNumber"]) : 0,

                                BoothIncharge = dr["Incharge_Name"].ToString(),
                                SectorIncharge = dr["SectorIncharge"].ToString(),
                                SectorName = dr["SectorName"].ToString(),
                                PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value?   dr["PollingStationBoothName"].ToString():"",

                                // Split the comma-separated village names into a list
                                VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                                Name = dr["Name"]?.ToString(),
                                CasteName = dr["Caste"]?.ToString(),
                                Address = dr["Address"]?.ToString(),
                                Contact = dr["Contact"]?.ToString(),
                                Status = dr["Status"] != DBNull.Value ? Convert.ToInt32(dr["Status"]) : 0,
                                SeniorOrDisabledType = dr["SeniorOrDisabled"]?.ToString()
                            };

                            list.Add(report);
                        }
                    }
                }
            }

            return list;
        }

        public List<SeniorOrDisabled> GetHandicapReport(FilterModel filter)
        {
            List<SeniorOrDisabled> list = new List<SeniorOrDisabled>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_SeniorOrDisabed", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetHandicap");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                    cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            SeniorOrDisabled report = new SeniorOrDisabled
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                BoothNo = dr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(dr["BoothNumber"]) : 0,


                                BoothIncharge = dr["Incharge_Name"].ToString(),
                                SectorIncharge = dr["SectorIncharge"].ToString(),
                                SectorName = dr["SectorName"].ToString(),
                                PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value? dr["PollingStationBoothName"].ToString():"",

                                // Split the comma-separated village names into a list
                                VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                                Name = dr["Name"]?.ToString(),
                                CasteName = dr["Caste"]?.ToString(),
                                Address = dr["Address"]?.ToString(),
                                Contact = dr["Contact"]?.ToString(),
                                Status = dr["Status"] != DBNull.Value ? Convert.ToInt32(dr["Status"]) : 0,
                                SeniorOrDisabledType = dr["SeniorOrDisabled"]?.ToString()
                            };

                            list.Add(report);
                        }
                    }
                }
            }

            return list;
        }

        public List<EffectivePerson> GetEffectivePersonReport(FilterModel filter)
        {
            List<EffectivePerson> list = new List<EffectivePerson>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_EffectivePerson_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "EffectivePersonReport");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                    cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            EffectivePerson report = new EffectivePerson
                            {
                                effectivePersonId = dr["effectivePersonId"] != DBNull.Value ? Convert.ToInt32(dr["effectivePersonId"]) : 0,
                                BoothNumber = dr["boothnumber"] != DBNull.Value ? Convert.ToInt32(dr["boothnumber"]) : 0,

                                BoothIncharge = dr["Incharge_Name"].ToString(),
                                SectorIncharge = dr["SectorIncharge"].ToString(),
                                SectorName = dr["SectorName"].ToString(),


                                // Split the comma-separated village names into a list
                                VillageNames = dr["VillageName"] != DBNull.Value ? dr["VillageName"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                                Designation = dr["EffectiveDesignationdata"]?.ToString(),
                                Name = dr["Name"]?.ToString(),
                                Category = dr["Category"]?.ToString(),
                                Castename = dr["Caste"]?.ToString(),
                                Mobile = dr["Contact"]?.ToString(),
                                Description = dr["Description"]?.ToString(),
                                PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value? dr["PollingStationBoothName"]?.ToString():""
                            };

                            list.Add(report);
                        }
                    }
                }
            }

            return list;
        }


        public List<PravasiVoter> GetPravasiVoterReport(FilterModel filter)
        {
            List<PravasiVoter> list = new List<PravasiVoter>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_pravasiVoter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "PravasiVoterReport");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                    cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                    cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                    cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            PravasiVoter report = new PravasiVoter
                            {
                                id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                boothno = dr["boothno"] == DBNull.Value ? 0 : Convert.ToInt32(dr["boothno"]),
                                Occupation = dr["occupationName"] == DBNull.Value ? "" : dr["occupationName"].ToString(),
                                BoothNumber = dr["BoothNumber"] == DBNull.Value ? "" : dr["BoothNumber"].ToString(),
                                BoothIncharge = dr["Incharge_Name"].ToString(),
                                SectorIncharge = dr["SectorIncharge"].ToString(),
                                SectorName = dr["SectorName"].ToString(),
                                Caste = dr["casteId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["casteId"]),
                                Category = dr["categoryId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["categoryId"]),
                                // Split the comma-separated village names into a list

                                VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                                name = dr["Name"]?.ToString(),
                                currentAddress = dr["CurrentAddress"]?.ToString(),

                                CategoryName = dr["CategoryName"]?.ToString(),
                                CasteName = dr["SubCasteName"]?.ToString(),
                                mobile = dr["Contact"]?.ToString(),
                                PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value?dr["PollingStationBoothName"]?.ToString():"",



                            };

                            list.Add(report);
                        }
                    }
                }
            }

            return list;
        }
        public List<doubleVoter> GetDoubleVoterReport(FilterModel filter, int? limit = null, int? page = null)
        {
            List<doubleVoter> list = new List<doubleVoter>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_doubleVoter", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "DoubleVoterReport");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                    cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            doubleVoter report = new doubleVoter
                            {
                                id = dr["id"] != DBNull.Value ? Convert.ToInt32(dr["id"]) : 0,
                                BoothNumber = dr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(dr["BoothNumber"]) : 0,
                                BoothIncharge = dr["Incharge_Name"].ToString(),
                                SectorIncharge = dr["SectorIncharge"].ToString(),
                                SectorName = dr["SectorName"].ToString(),
                                PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value?  dr["PollingStationBoothName"].ToString():"",

                                VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                                fathername = dr["fathername"]?.ToString(),
                                name = dr["Name"]?.ToString(),
                                currentAddress = dr["CurrentAddress"]?.ToString(),
                                pastAddress = dr["pastAddress"]?.ToString(),
                                reason = dr["reason"]?.ToString(),
                                voterno = dr["voterno"]?.ToString(),

                            };

                            list.Add(report);
                        }
                    }
                }
            }

            return list;
        }

        public List<NewVoters> GetNewVoterReport(FilterModel filter, int? limit = null, int? page = null)
        {
            List<NewVoters> list = new List<NewVoters>();

            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_NewVoters", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Actions", "NewVoterReport");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                    cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            NewVoters report = new NewVoters
                            {
                                Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                BoothNo = dr["BoothNo"] != DBNull.Value ? Convert.ToInt32(dr["BoothNo"]) : 0,
                                BoothIncharge = dr["Incharge_Name"].ToString(),
                                SectorIncharge = dr["SectorIncharge"].ToString(),
                                SectorName = dr["SectorName"].ToString(),
                                Village = dr["VillageNames"].ToString(),

                                //VillageNames = dr["VillageNames"] != DBNull.Value ? dr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),
                                //VillageNames = dr["VillageName"].ToString(),

                                VoterName = dr["VotersName"]?.ToString(),
                                FatherName = dr["FatherName"]?.ToString(),
                                totalage = dr["totalAge"]?.ToString(),
                                dateofbirth = dr["DOB"] != DBNull.Value ? Convert.ToDateTime(dr["DOB"]) : DateTime.MinValue,
                                MobileNumber = dr["Contact"]?.ToString(),
                                Education = dr["Education"]?.ToString(),
                                Category = dr["Category"]?.ToString(),
                                caste = dr["Caste"]?.ToString(),
                                PollingStationBoothName = dr["PollingStationBoothName"]!=DBNull.Value?   dr["PollingStationBoothName"]?.ToString():"",

                            };

                            list.Add(report);
                        }
                    }
                }
            }

            return list;
        }
        #endregion



        #region preety

        public List<AllowAccess> GetDataForAllowAccess(string type)
        {
            List<AllowAccess> list = new List<AllowAccess>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Authorization", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetDataForAllowAccess");
                        cmd.Parameters.AddWithValue("@type", type);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                list.Add(new AllowAccess
                                {
                                    value = Convert.ToInt32(sdr["Value"]),
                                    Data = sdr["Data"].ToString()
                                });
                            }
                        }
                    }
                }
                return list;
            }
            catch
            {
                throw;
            }
        }
        public bool AddAllowAccess(string type, string refIds, string allowedPermissions)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Authorization", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "InsertAllowPermission");
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@sectorOrBoothId", refIds);
                        cmd.Parameters.AddWithValue("@AllowFor", allowedPermissions);
                        cmd.Parameters.AddWithValue("@allowStatus", "allow");


                        int res = cmd.ExecuteNonQuery();
                        return res > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public List<VillageList> getVillageByMandalId(int mandalId,int VidhanSabhaId)
        {
            List<VillageList> list = new List<VillageList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_VillageListForMandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "selectVillageByMandalId");
                        cmd.Parameters.AddWithValue("@MandalId", mandalId);
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                list.Add(new VillageList
                                {
                                    villageId = Convert.ToInt32(sdr["id"]),
                                    Villagename = sdr["villageName"].ToString()
                                });
                            }
                        }
                    }
                }
                return list;
            }
            catch
            {
                throw;
            }
        }

        


        public List<VillageList> GetVillageBySectorId(string sectorId)
        {
            List<VillageList> data = new List<VillageList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "getVillageBySectorId");
                        cmd.Parameters.AddWithValue("@SectorIds", sectorId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                  data.Add(new VillageList
                            {
                                villageId = sdr["VillageId"] == DBNull.Value ? 0 : Convert.ToInt32(sdr["VillageId"]),
                                Villagename = sdr["VillageName"] == DBNull.Value ? string.Empty : sdr["VillageName"].ToString()
                            });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }
        #endregion
        public List<Booth> GetBoothIdAndNumber(int VidhanSabhaId)
        {
            List<Booth> data = new List<Booth>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "SelectBoothNumber");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new Booth
                                {
                                    Booth_Id = Convert.ToInt32(sdr["Booth_Id"]),
                                    BoothNumber = sdr["BoothNumber"].ToString()
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        public List<VillageList> VillageListByBoothId(string boothId)
        {
            List<VillageList> data = new List<VillageList>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "SelectVillageByBoothId");
                        cmd.Parameters.AddWithValue("@boothId", boothId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new VillageList
                                {
                                    villageId = Convert.ToInt32(sdr["VillageId"]),
                                    Villagename = sdr["VillageName"].ToString()

                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        public bool AddSatisfiedUnsatisfied(SatisfiedUnSatisfied data)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@boothId", data.boothNo);
                        cmd.Parameters.AddWithValue("@village", data.village);
                        cmd.Parameters.AddWithValue("@type", data.sahmatAsahmat);
                        cmd.Parameters.AddWithValue("@name", data.name);
                        cmd.Parameters.AddWithValue("@age", data.age);
                        cmd.Parameters.AddWithValue("@mobile", data.mobile);
                        cmd.Parameters.AddWithValue("@party", data.party);
                        cmd.Parameters.AddWithValue("@reason", data.reason);
                        cmd.Parameters.AddWithValue("@Occupation", data.Occupations);
                        cmd.Parameters.AddWithValue("@id", data.id);
                        cmd.Parameters.AddWithValue("@action", data.id > 0 ? "UpdateSahmatUsahmat" : "insertSatisFiedUnsatisfied");
                        int res = cmd.ExecuteNonQuery();
                        return res > 0;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public List<SatisfiedUnSatisfied> getAllDataSatisfiedUnsatisfied(int? limit = null, int? page = null)
        {
            List<SatisfiedUnSatisfied> data = new List<SatisfiedUnSatisfied>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllSatisfiedUnsatisfied");

                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SatisfiedUnSatisfied
                                {
                                    id = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0,
                                    boothNo = sdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(sdr["BoothNumber"]) : 0,
                                    BoothName = sdr["BoothName"] != DBNull.Value ? sdr["BoothName"].ToString() : null,
                                    sahmatAsahmat = sdr["sahmtAshmtType"] != DBNull.Value ? Convert.ToInt32(sdr["sahmtAshmtType"]) : 0,

                                    //sahmatAsahmat = sdr["Type"] != DBNull.Value ? Convert.ToInt32(sdr["Type"]) : 0,
                                    sahmatAsahmatName = sdr["SahmatAsahmat"] != DBNull.Value ? sdr["SahmatAsahmat"].ToString() : null,
                                    name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : null,
                                    age = sdr["age"] != DBNull.Value ? Convert.ToInt32(sdr["age"]) : 0,
                                    mobile = sdr["mobile"] != DBNull.Value ? sdr["mobile"].ToString() : null,

                                    village = sdr["VillageName"] != DBNull.Value ? sdr["VillageName"].ToString() : null,
                                    party = sdr["party"] != DBNull.Value ? sdr["party"].ToString() : null,
                                    reason = sdr["reason"] != DBNull.Value ? sdr["reason"].ToString() : null,
                                    Occupation = sdr["OccupationName"] != DBNull.Value ? sdr["OccupationName"].ToString() : null
                                });

                            }
                        }

                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Data", ex);

            }
        }


        public List<SatisfiedUnSatisfied> getAllSahmatData(int? limit = null, int? page = null)
        {
            List<SatisfiedUnSatisfied> data = new List<SatisfiedUnSatisfied>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetAllSahmatDataforApi");
                        //cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                        //cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                        //cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                        //cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SatisfiedUnSatisfied
                                {
                                    id = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0,
                                    boothNo = sdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(sdr["BoothNumber"]) : 0,
                                    BoothName = sdr["BoothName"] != DBNull.Value ? sdr["BoothName"].ToString() : null,
                                    sahmatAsahmat = sdr["sahmtAshmtType"] != DBNull.Value ? Convert.ToInt32(sdr["sahmtAshmtType"]) : 0,

                                    //sahmatAsahmat = sdr["Type"] != DBNull.Value ? Convert.ToInt32(sdr["Type"]) : 0,
                                    sahmatAsahmatName = sdr["SahmatAsahmat"] != DBNull.Value ? sdr["SahmatAsahmat"].ToString() : null,
                                    name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : null,
                                    age = sdr["age"] != DBNull.Value ? Convert.ToInt32(sdr["age"]) : 0,
                                    mobile = sdr["mobile"] != DBNull.Value ? sdr["mobile"].ToString() : null,

                                    village = sdr["VillageName"] != DBNull.Value ? sdr["VillageName"].ToString() : null,
                                    party = sdr["party"] != DBNull.Value ? sdr["party"].ToString() : null,
                                    reason = sdr["reason"] != DBNull.Value ? sdr["reason"].ToString() : null,
                                    Occupation = sdr["OccupationName"] != DBNull.Value ? sdr["OccupationName"].ToString() : null
                                });

                            }
                        }

                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Data", ex);

            }
        }

        public List<SatisfiedUnSatisfied> getAllASahmatData(int? limit = null, int? page = null)
        {
            List<SatisfiedUnSatisfied> data = new List<SatisfiedUnSatisfied>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetAllASahmatDataforApi");
                        //cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                        //cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                        //cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                        //cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SatisfiedUnSatisfied
                                {
                                    id = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0,
                                    boothNo = sdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(sdr["BoothNumber"]) : 0,
                                    BoothName = sdr["BoothName"] != DBNull.Value ? sdr["BoothName"].ToString() : null,
                                    sahmatAsahmat = sdr["sahmtAshmtType"] != DBNull.Value ? Convert.ToInt32(sdr["sahmtAshmtType"]) : 0,

                                    //sahmatAsahmat = sdr["Type"] != DBNull.Value ? Convert.ToInt32(sdr["Type"]) : 0,
                                    sahmatAsahmatName = sdr["SahmatAsahmat"] != DBNull.Value ? sdr["SahmatAsahmat"].ToString() : null,
                                    name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : null,
                                    age = sdr["age"] != DBNull.Value ? Convert.ToInt32(sdr["age"]) : 0,
                                    mobile = sdr["mobile"] != DBNull.Value ? sdr["mobile"].ToString() : null,

                                    village = sdr["VillageName"] != DBNull.Value ? sdr["VillageName"].ToString() : null,
                                    party = sdr["party"] != DBNull.Value ? sdr["party"].ToString() : null,
                                    reason = sdr["reason"] != DBNull.Value ? sdr["reason"].ToString() : null,
                                    Occupation = sdr["OccupationName"] != DBNull.Value ? sdr["OccupationName"].ToString() : null
                                });

                            }
                        }

                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Data", ex);

            }
        }

        public List<SatisfiedUnSatisfied> GetAllDataSatisfiedUnsatisfied(int VidhanSabhaId,int? type, FilterModel filter,int? limit=null,int? page=null )
        {
            List<SatisfiedUnSatisfied> data = new List<SatisfiedUnSatisfied>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parameters
                        cmd.Parameters.AddWithValue("@action", "getAllSatisfiedUnsatisfied");
                        cmd.Parameters.AddWithValue("@VidhanSabhaId", VidhanSabhaId);
                        cmd.Parameters.AddWithValue("@type", (object)type ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                        cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SatisfiedUnSatisfied
                                {
                                    id = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0,
                                    boothNo = sdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(sdr["BoothNumber"]) : 0,
                                    sahmatAsahmat = sdr["sahmtAshmtType"] != DBNull.Value ? Convert.ToInt32(sdr["sahmtAshmtType"]) : 0,
                                    BoothName = sdr["BoothName"] != DBNull.Value ? sdr["BoothName"].ToString() : null,

                                    sahmatAsahmatName = sdr["SahmatAsahmat"] != DBNull.Value ? sdr["SahmatAsahmat"].ToString() : null,
                                    name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : null,
                                    age = sdr["age"] != DBNull.Value ? Convert.ToInt32(sdr["age"]) : 0,
                                    mobile = sdr["mobile"] != DBNull.Value ? sdr["mobile"].ToString() : null,

                                    village = sdr["VillageName"] != DBNull.Value ? sdr["VillageName"].ToString() : null,
                                    party = sdr["party"] != DBNull.Value ? sdr["party"].ToString() : null,
                                    reason = sdr["reason"] != DBNull.Value ? sdr["reason"].ToString() : null,
                                    Occupation = sdr["OccupationName"] != DBNull.Value ? sdr["OccupationName"].ToString() : null,
                                    PollingStationBoothName = sdr["PollingStationBoothName"] != DBNull.Value ? sdr["PollingStationBoothName"].ToString() : null
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Count", ex);

            }
        }




        public List<SatisfiedUnSatisfied> GetAllDataSatisfiedUnsatisfiedforapi(int? limit, int? page)
        {
            List<SatisfiedUnSatisfied> data = new List<SatisfiedUnSatisfied>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Parameters
                        cmd.Parameters.AddWithValue("@action", "selectallsahmatandasahmat");
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        //cmd.Parameters.AddWithValue("@type", (object)type ?? DBNull.Value);
                        //cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        //cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                        //cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);
                        //cmd.Parameters.AddWithValue("@PageNumber", pageNumber);
                        //cmd.Parameters.AddWithValue("@PageSize", pageSize);

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SatisfiedUnSatisfied
                                {
                                    id = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0,
                                    boothNo = sdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(sdr["BoothNumber"]) : 0,
                                    BoothName = sdr["BoothName"] != DBNull.Value ? sdr["BoothName"].ToString() : null,

                                    sahmatAsahmatName = sdr["Type"] != DBNull.Value ? sdr["Type"].ToString() : null,
                                    name = sdr["name"] != DBNull.Value ? sdr["name"].ToString() : null,
                                    age = sdr["age"] != DBNull.Value ? Convert.ToInt32(sdr["age"]) : 0,
                                    mobile = sdr["mobile"] != DBNull.Value ? sdr["mobile"].ToString() : null,

                                    villageName = sdr["VillageName"] != DBNull.Value ? sdr["VillageName"].ToString() : null,
                                    party = sdr["party"] != DBNull.Value ? sdr["party"].ToString() : null,
                                    reason = sdr["reason"] != DBNull.Value ? sdr["reason"].ToString() : null,
                                    Occupation = sdr["OccupationName"] != DBNull.Value ? sdr["OccupationName"].ToString() : null,
                                    partyId = sdr["partyId"] != DBNull.Value ? Convert.ToInt32(sdr["partyId"]) : 0,
                                     boothid= sdr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(sdr["Booth_Id"]) : 0,
                                     sahmatAsahmat= sdr["typeId"] != DBNull.Value ? Convert.ToInt32(sdr["typeId"]) : 0,
                                    Occupations = sdr["OccupationId"] != DBNull.Value ? Convert.ToInt32(sdr["OccupationId"]) : 0,
                                    village_Id = sdr["Village"] != DBNull.Value ? sdr["Village"].ToString() : "",

                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Count", ex);

            }
        }


        public int getSahamatCount()
        {
            int count = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "CountSahamat");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                count = sdr["SahamatCount"] != DBNull.Value ? Convert.ToInt32(sdr["SahamatCount"]) : 0;

                            }
                        }

                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Count", ex);

            }
        }
        public int getAsahamatCount()
        {
            int count = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "CountAsahamat");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                count = sdr["AsahamatCount"] != DBNull.Value ? Convert.ToInt32(sdr["AsahamatCount"]) : 0;

                            }
                        }

                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Count", ex);

            }
        }

        public bool deleteSatisfiedUnsatisfied(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "deleteSatisfiedUnsatisfiedById");
                        cmd.Parameters.AddWithValue("@id", id);
                        int res = cmd.ExecuteNonQuery();
                        return res > 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Data", ex);
            }
        }

        //Conbined Report Service
        public List<Booth> GetCombinedReport(FilterModel filter,int? limit = null, int? page = null)
        {
            List<Booth> reportList = new List<Booth>();
            try
            {

                con.Open();

                SqlCommand cmd = new SqlCommand("SP_Booth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mandalIds", filter.mandalIds);
                cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                cmd.Parameters.AddWithValue("@Action", "CombinedReport");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Booth report = new Booth
                    {
                        BoothNumber = rdr["BoothNumber"].ToString(),
                        MandalName = rdr["MandalName"].ToString(),
                        BoothName = rdr["BoothName"].ToString(),
                        SectorName = rdr["SectorName"].ToString(),
                        SectorIncName = rdr["SectorIncName"].ToString(),
                        SectorIncPhone = rdr["SectorIncPhone"].ToString(),
                        InchargeName = rdr["Incharge_Name"].ToString(),
                        FatherName = rdr["FatherName"].ToString(),
                        Age = rdr["Age"] != DBNull.Value ? Convert.ToInt32(rdr["Age"]) : 0,

                        SubCasteName = rdr["SubCasteName"].ToString(),
                        Address = rdr["Address"].ToString(),
                        Education = rdr["Education"].ToString(),
                        PhoneNumber = rdr["PhoneNumber"].ToString(),
                        ProfileImage = rdr["ProfileImage"].ToString(),
                        PollingStationBoothName = rdr["PollingStationBoothName"]!=DBNull.Value? rdr["PollingStationBoothName"].ToString():"",

                        // Split the comma-separated village names into a list
                        VillageNames = rdr["VillageNames"] != DBNull.Value ? rdr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>()
                    };

                    reportList.Add(report);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Combined Report", ex);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
            }

            return reportList;
        }
        public SatisfiedUnSatisfied GetAllSatisfiedUnsatisfiedDataById(int? id)
        {
            SatisfiedUnSatisfied data = new SatisfiedUnSatisfied();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getSahmatAsahmatById");
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader sdr = cmd.ExecuteReader();
                         if (sdr.HasRows)
                        {
                         while (sdr.Read())
                            {
                                data = new SatisfiedUnSatisfied
                                {
                                    id = Convert.ToInt32(sdr["Id"]),
                                    boothNo = Convert.ToInt32(sdr["boothId"]),
                                    boothid = Convert.ToInt32(sdr["boothId"]),
                                    BoothNumber = sdr["BoothNumber"].ToString(),
                                    sahmatAsahmat = Convert.ToInt32(sdr["sahmttype"]),
                                    sahmatAsahmatName = sdr["Type"].ToString(),
                                    name = sdr["name"].ToString(),
                                    age = Convert.ToInt32(sdr["age"]),
                                    mobile = sdr["mobile"].ToString(),
                                    reason = sdr["reason"].ToString(),
                                    Occupations = Convert.ToInt32(sdr["Occupation"]),
                                    partyId = Convert.ToInt32(sdr["id"]),
                                    party = sdr["partyName"].ToString(),
                                    village_Id = sdr["Village"].ToString(),
                                    villageName = sdr["villageName"].ToString()
                                    //VillageNames = sdr["VillageNames"] != DBNull.Value ? sdr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),
                                    //villageId = sdr["Village"] != DBNull.Value ? sdr["Village"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                                };
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }

        public List<SahmatAsahmatType> getSahmatAsahmatType()
        {
            List<SahmatAsahmatType> data = new List<SahmatAsahmatType>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_SahmatAndAsahmat", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@Action", "TYPEshmatahsahmt");


                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SahmatAsahmatType
                                {
                                    Id = Convert.ToInt32(sdr["Id"]),
                                    type = sdr["Type"].ToString(),
                                });
                            }
                        }
                    }
                }
                return data;  
            }
            catch
            {
                 throw;
            }

        }
        public List<Occupation> getOccupation()

        {
            List<Occupation> data = new List<Occupation>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_ManageMasterData", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@Action", "GetAllOccupation");


                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new Occupation
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    occupation = sdr["Occupation"].ToString(),
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }

        }

        public List<SocialMediaPost> GetSocialMediaPostDetailById(int id)
        {
            List<SocialMediaPost> data = new List<SocialMediaPost>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("usp_SocialMediaPost_CRUD", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.AddWithValue("@Action", "GetBoothSectorDetailByPostId");
                        cmd.Parameters.AddWithValue("@postId", id);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SocialMediaPost
                                {
                                    PostId = Convert.ToInt32(sdr["PostId"]),
                                    Booth_id = Convert.ToInt32(sdr["Booth_Id"]),
                                    Boothnumber = Convert.ToInt32(sdr["BoothNumber"]),
                                    boothName = sdr["BoothName"].ToString(),
                                    BoothIncharge = sdr["BoothIncharge"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    SectorInchargeName = sdr["SectorIncharge"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"]!=DBNull.Value? sdr["PollingStationBoothName"].ToString():"",
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }

        }



        public bool DeleteCastVoter(int id)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Actions", "DeleteCastVoter");
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public List<CastVotersDes> GetCastVotersDesById(int voterDesId)
        {
            var result = new List<CastVotersDes>();
            var tempDict = new Dictionary<int, CastVotersDes>();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Actions", "GetCastVoterDesById");
                cmd.Parameters.AddWithValue("@Id", voterDesId);

                con.Open();

                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    int id = rd["CastVoterId"] != DBNull.Value ? Convert.ToInt32(rd["CastVoterId"]) : 0;
                    int voterDes = rd["VoterDes"] != DBNull.Value ? Convert.ToInt32(rd["VoterDes"]) : 0;
                    string castName = rd["SubCasteName"]?.ToString();
                    int castNameId = rd["CastName"] != DBNull.Value ? Convert.ToInt32(rd["CastName"]) : 0;
                    int number = rd["Number"] != DBNull.Value ? Convert.ToInt32(rd["Number"]) : 0;

                    if (!tempDict.ContainsKey(voterDes))
                    {
                        tempDict[voterDes] = new CastVotersDes
                        {
                            //Id = id,
                            VoterDes = voterDes,
                            castVoterId = new List<int>(),
                            CastName = new List<string>(),
                            Number = new List<int>(),
                            castNameId = new List<int>()
                        };
                    }

                    tempDict[voterDes].castVoterId.Add(id);
                    tempDict[voterDes].CastName.Add(castName);
                    tempDict[voterDes].Number.Add(number);
                    tempDict[voterDes].castNameId.Add(castNameId);
                }

                rd.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            result = tempDict.Values.ToList();
            return result;
        }
        public List<CastVotersDes> GetCastListById(int id)
        {
            var result = new List<CastVotersDes>();

            try
            {
                SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Actions", "GetValueCastbyId");
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    result.Add(new CastVotersDes
                    {
                        Id = rd["Id"] != DBNull.Value ? Convert.ToInt32(rd["Id"]) : 0,
                        VoterDes = rd["VoterDes"] != DBNull.Value ? Convert.ToInt32(rd["VoterDes"]) : 0,
                        castNameId = new List<int> { Convert.ToInt32(rd["CastName"] )},
                        Number = new List<int> { rd["Number"] != DBNull.Value ? Convert.ToInt32(rd["Number"]) : 0 },
                        CastName = new List<string> { rd["SubCasteName"]?.ToString() ?? string.Empty }
                      
                    });
                }

                rd.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        public bool UpdateCastVoter(int id, int subCasteId, int number)
        {
            bool isSuccess = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_BoothVoterDescription", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Actions", "UpdateCastVoter");
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@CastName", subCasteId);
                    cmd.Parameters.AddWithValue("@Number", number);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    isSuccess = rowsAffected > 0;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                con.Close();
            }

            return isSuccess;
        }

        #region Block Nazif

        public IEnumerable<Block> GetAllBlocks(FilterModel filter, int? limit = null, int? page = null)
        {
            var list = new List<Block>();

            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("SP_Block_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTALL");
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                    cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);

                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new Block
                            {
                                Block_Id = Convert.ToInt32(rdr["Block_Id"]),
                                BlockName = rdr["BlockName"] as string,
                                InchargeName = rdr["InchargeName"] as string,
                                Contact = rdr["Contact"] as string,
                                Address = rdr["Address"] as string,
                                Category = rdr["Category"] as int?,
                                CasteName = rdr["CasteName"] as string,
                                party = Convert.ToInt32(rdr["party"]),
                                partyName = rdr["partyName"] != DBNull.Value ? rdr["partyName"].ToString() : null,
                                CategoryName = rdr["CategoryName"] as string,
                                Caste = rdr["Caste"] as int?,
                                Occupation = rdr["Occupation"] as string,
                                ProfileImage = rdr["ProfileImage"] as string
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error (use logging framework or write to file/db/event log)
                throw new ApplicationException("Error retrieving blocks.", ex);
            }

            return list;
        }
        public int GetAllBlocksCounts()
        {
            int count = 0;

            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("SP_Block_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "TotalCount");

                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            count = Convert.ToInt32(rdr["blockCounts"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Error retrieving blocks Counts.", ex);
            }

            return count;
        }

        public Block GetBlockById(int id)
        {
            Block block = null;

            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("SP_Block_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTBYID");
                    cmd.Parameters.AddWithValue("@Block_Id", id);

                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            block = new Block
                            {
                                Block_Id = Convert.ToInt32(rdr["Block_Id"]),
                                BlockName = rdr["BlockName"] as string,
                                InchargeName = rdr["InchargeName"] as string,
                                Contact = rdr["Contact"] as string,
                                Address = rdr["Address"] as string,
                                Category = rdr["Category"] as int?,
                                Caste = rdr["Caste"] as int?,
                                party = Convert.ToInt32(rdr["party"]),
                                Occupation = rdr["Occupation"] as string,
                                partyName = rdr["partyname"] as string,
                                ProfileImage = rdr["ProfileImage"] as string
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving block with ID {id}.", ex);
            }

            return block;
        }
        public int SaveBlock(Block block)
        {
            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("SP_Block_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    bool isUpdate = block.Block_Id > 0;

                    cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");

                    if (isUpdate)
                        cmd.Parameters.AddWithValue("@Block_Id", block.Block_Id);

                    cmd.Parameters.AddWithValue("@BlockName", block.BlockName);
                    cmd.Parameters.AddWithValue("@InchargeName", block.InchargeName);
                    cmd.Parameters.AddWithValue("@Contact", block.Contact);
                    cmd.Parameters.AddWithValue("@Address", block.Address);
                    cmd.Parameters.AddWithValue("@party", block.party);
                    cmd.Parameters.AddWithValue("@Category", (object)block.Category ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Caste", (object)block.Caste ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Occupation", block.Occupation);
                  
                    cmd.Parameters.AddWithValue("@ProfileImage", block.ProfileImage ?? (object)DBNull.Value);

                    con.Open();

                    if (isUpdate)
                    {
                        int rows = cmd.ExecuteNonQuery();
                        return rows > 0 ? block.Block_Id : -1;
                    }
                    else
                    {
                        var result = cmd.ExecuteScalar();
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error saving block data.", ex);
            }
        }
     
        public bool DeleteBlock(int id)
        {
            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("SP_Block_CRUD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@Block_Id", id);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting block with ID {id}.", ex);
            }
        }
        public List<VillageList> GetAllVillage()
        {
            List<VillageList> data = new List<VillageList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_VillageListForMandal", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "selectVillage");

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new VillageList
                                {
                                    villageId = Convert.ToInt32(sdr["id"]),
                                    Villagename = sdr["villageName"].ToString()
                                });
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Bdc Nazif
        public IEnumerable<BDC> GetBDC( int? limit = null, int? page = null, int? id = null)
        {
            var list = new List<BDC>();

            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_ManageBDCProfile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (id.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@Action", "GETBYID");
                        cmd.Parameters.AddWithValue("@BDC_Id", id.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Action", "GETALL");
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                    }

                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new BDC
                            {
                                BDC_Id = rdr["BDC_Id"] != DBNull.Value ? Convert.ToInt32(rdr["BDC_Id"]) : 0,
                                Block_Id = rdr["Block_Id"] != DBNull.Value ? Convert.ToInt32(rdr["Block_Id"]) : 0,
                                BlockName = rdr["BlockName"] != DBNull.Value ? rdr["BlockName"].ToString() : null,
                                Name = rdr["Name"] != DBNull.Value ? rdr["Name"].ToString() : null,
                                Village = rdr["villageName"] != DBNull.Value ? rdr["villageName"].ToString() : null,
                                villageId = rdr["Village_Id"] != DBNull.Value ? rdr["Village_Id"].ToString() : null,
                                //villageId = rdr["Village_Id"] != DBNull.Value ? Convert.ToInt32(rdr["Village_Id"]) : 0,
                                Category = rdr["Category"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["Category"]) : null,
                                CategoryName = rdr["CategoryName"] != DBNull.Value ? rdr["CategoryName"].ToString() : null,
                                Caste = rdr["Caste"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["Caste"]) : null,
                                partyName = rdr["partyName"] != DBNull.Value ? rdr["partyName"].ToString() : null,
                                party = rdr["party"] != DBNull.Value ? Convert.ToInt32(rdr["party"]) : 0,
                                CasteName = rdr["SubCasteName"] != DBNull.Value ? rdr["SubCasteName"].ToString() : null,
                                Age = rdr["Age"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["Age"]) : null,

                                Contact = rdr["Contact"] != DBNull.Value ? rdr["Contact"].ToString() : null,
                                Education = rdr["Education"] != DBNull.Value ? rdr["Education"].ToString() : null,
                                ProfileImage = rdr["profileImage"] != DBNull.Value ? rdr["profileImage"].ToString() : null,
                                WardNumber = rdr["WardNumber"] != DBNull.Value ? Convert.ToInt32(rdr["WardNumber"]) : 0
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(id.HasValue ?
                    $"Error retrieving BDC profile with ID {id.Value}." :
                    "Error retrieving all BDC profiles.", ex);
            }

            return list;
        }
        public int GetBDCCount(int? id = null)
        {
            int count = 0;

            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_ManageBDCProfile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;



                    cmd.Parameters.AddWithValue("@Action", "CountBdc");

                    con.Open();
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            count = rdr["bdcCount"] != DBNull.Value ? Convert.ToInt32(rdr["bdcCount"]) : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(id.HasValue ?
                    $"Error retrieving BDC profile with ID {id.Value}." :
                    "Error retrieving all BDC profiles.", ex);
            }

            return count;
        }
        // Insert or Update BDC profile based on presence of BDC_Id
            public void SaveBDC(BDC bdc)
            {
                if (bdc == null) throw new ArgumentNullException(nameof(bdc));

                string action = bdc.BDC_Id == 0 ? "INSERT" : "UPDATE";

                try
                {
                    using (var con = new SqlConnection(conn))
                    using (var cmd = new SqlCommand("sp_ManageBDCProfile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", action);

                        if (action == "UPDATE")
                            cmd.Parameters.AddWithValue("@BDC_Id", bdc.BDC_Id);

                        cmd.Parameters.AddWithValue("@Block_Id", bdc.Block_Id);
                        // IMPORTANT: You must provide Village_Id here; currently passing 0, update accordingly
                        //cmd.Parameters.AddWithValue("@Village_Id", bdc.villageId);
                        cmd.Parameters.AddWithValue("@Village_Id", bdc.villageId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Name", bdc.Name ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Category", (object)bdc.Category ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Caste", (object)bdc.Caste ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Age", bdc.Age);
                        cmd.Parameters.AddWithValue("@party", bdc.party);
                        cmd.Parameters.AddWithValue("@Contact", bdc.Contact ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Education", bdc.Education ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ProfileImage", bdc.ProfileImage ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@WardNumber", bdc.WardNumber);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Error during {action} operation for BDC profile.", ex);
                }
            }

        // Soft Delete by setting Status = 0
        public bool DeleteBDC(int id)
        {
            try
            {
                using (var con = new SqlConnection(conn))
                using (var cmd = new SqlCommand("sp_ManageBDCProfile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@BDC_Id", id);

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }

            }

            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting BDC profile with ID {id}.", ex);
            }
        }

        #endregion

        public List<Designation> GetDesignations()
        {
            var designations = new List<Designation>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertBoothSamiti", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "Binddesig");

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            designations.Add(new Designation
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                DesigName = reader["DesigName"]?.ToString(),
                                Status = Convert.ToBoolean(reader["Status"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching designations: " + ex.Message, ex);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }

            return designations;
        }

         public List<Party>  Getallparty()
        {
            List<Party> party = new List<Party>();
            try
            {
                using (SqlCommand cmd = new SqlCommand("spparties", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "getParty");

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            party.Add(new Party
                            {
                                id = Convert.ToInt32(reader["id"]),
                                party = reader["party"]?.ToString(),
                              
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching getallparty: " + ex.Message, ex);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return party;
        }

        //get Effective designations -
        public List<EffectiveDesignation> GetEffectiveDesignations()
            {
            List<EffectiveDesignation> list = new List<EffectiveDesignation>();

            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("Sp_ManageMasterData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetAllEffectiveDesignation");

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new EffectiveDesignation
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        EffectiveDesignationdata = reader["EffectiveDesignationdata"]?.ToString(),
                        Status = Convert.ToBoolean(reader["Status"])
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving designations: " + ex.Message, ex);
            }
            finally
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }

            return list;
        }

        //get SeniorOrDisabled-
        public List<SeniorDisabled> GetSeniorDisabledtype()
        {
            List<SeniorDisabled> typedata = new List<SeniorDisabled>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("Sp_ManageMasterData", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAllSeniorDisabled");

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                typedata.Add(new SeniorDisabled
                                {
                                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    Type = reader["Type"] != DBNull.Value ? reader["Type"].ToString() : null,
                                    Status = reader["Status"] != DBNull.Value && Convert.ToBoolean(reader["Status"])
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Senior/Disabled types: " + ex.Message, ex);
            }

            return typedata;
        }

        #region Influencer Management
        public bool InsertInfluencer(Influencer model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("sp_manageInfluencer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Determine action
                    string action = model.Id.HasValue && model.Id > 0 ? "UPDATE" : "INSERT";
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@id", model.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@isEffective", model.IsEffective);

                    if (model.IsEffective)
                    {
                        cmd.Parameters.AddWithValue("@designation", model.Designation ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@personId", model.PersonId ?? (object)DBNull.Value);
                    }
                    else
                    {
                        // Manual entry
                        cmd.Parameters.AddWithValue("@personName", model.PersonName ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@category", model.Category ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@caste", model.Caste ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@mobile", model.Mobile ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@boothId", model.BoothId);
                        cmd.Parameters.AddWithValue("@description", model.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@VillageId", model.VillageListId ?? (object)DBNull.Value);
                    }

                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch(Exception ex)
            {
                throw ex;   
            }
        }
        public List<Influencer> GetInfluencers(FilterModel filter,int? limit, int? page)
        {
            var influencers = new List<Influencer>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                using (SqlCommand cmd = new SqlCommand("sp_manageInfluencer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                    cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                    cmd.Parameters.AddWithValue("@desgIds", filter.occuIds);
                    cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                    cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var influencer = new Influencer
                            {
                                Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                                IsEffective = reader["isEffective"] != DBNull.Value && Convert.ToBoolean(reader["isEffective"]),
                                Designation = reader["designation"] != DBNull.Value ? (int?)Convert.ToInt32(reader["designation"]) : null,
                                PersonId = reader["personId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["personId"]) : null,
                                PollingStationBoothName = reader["PollingStationBoothName"] != DBNull.Value ?reader["PollingStationBoothName"].ToString() : null,
                                PersonName = reader["personName"]?.ToString(),
                                Category = reader["category"] != DBNull.Value ? (int?)Convert.ToInt32(reader["category"]) : null,
                                Caste = reader["caste"] != DBNull.Value ? (int?)Convert.ToInt32(reader["caste"]) : null,
                                Mobile = reader["mobile"] != DBNull.Value ? (long?)Convert.ToInt64(reader["mobile"]) : null,
                                BoothId = reader["boothId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["boothId"]) : null,
                                Description = reader["description"]?.ToString(),

                                // Joined data
                                EffectiveDesignationdata = reader["EffectiveDesignationdata"]?.ToString(),
                                BoothNumber = reader["BoothNumber"]?.ToString(),
                                CategoryName = reader["CategoryName"]?.ToString(),
                                SubCasteName = reader["SubCasteName"]?.ToString(),
                                VillageListId = reader["VillageId"]?.ToString(),
                                VillageListName = reader["villageName"]?.ToString(),
                            };

                            influencers.Add(influencer);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return influencers;
        }
        public bool DeleteInfluencerPerson(int id)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_manageInfluencer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public Influencer GetInfluencerById(int id)
        {
            Influencer influencer = null;

            using (SqlCommand cmd = new SqlCommand("sp_manageInfluencer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "SELECTBYID");
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        influencer = new Influencer
                        {
                            Id = reader["id"] != DBNull.Value ? Convert.ToInt32(reader["id"]) : 0,
                            IsEffective = reader["isEffective"] != DBNull.Value && Convert.ToBoolean(reader["isEffective"]),
                            Designation = reader["designation"] != DBNull.Value ? (int?)Convert.ToInt32(reader["designation"]) : null,
                            PersonId = reader["personId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["personId"]) : null,
                            PersonName = reader["personName"]?.ToString(),
                            Category = reader["category"] != DBNull.Value ? (int?)Convert.ToInt32(reader["category"]) : null,
                            Caste = reader["caste"] != DBNull.Value ? (int?)Convert.ToInt32(reader["caste"]) : null,
                            Mobile = reader["mobile"] != DBNull.Value ? (long?)Convert.ToInt64(reader["mobile"]) : null,
                            BoothId = reader["boothId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["boothId"]) : null,
                            Description = reader["description"]?.ToString(),

                            // Joined data
                            EffectiveDesignationdata = reader["EffectiveDesignationdata"]?.ToString(),
                            BoothNumber = reader["BoothNumber"]?.ToString(),
                            CategoryName = reader["CategoryName"]?.ToString(),
                            SubCasteName = reader["SubCasteName"]?.ToString(),
                            VillageListId = reader["VillageId"]?.ToString(),
                            VillageListName = reader["VillageName"]?.ToString(),
                        };
                    }
                }
            }

            return influencer;
        }


        public int GetInfluencerCount()
        {
            int count = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_manageInfluencer", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "InfluencerCount");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                count = Convert.ToInt32(sdr["InfluencerCount"]);
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            return count;
        }

        #endregion


        public List<VillageList> GetVillageListByBoothId(string BoothId)
        {
            List<VillageList> data = new List<VillageList>();
            try
            {
                using(SqlConnection con=new SqlConnection(conn))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("SP_PannaPramukh", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetVillageByBoothId");
                        cmd.Parameters.AddWithValue("@BoothIds", BoothId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if(sdr.HasRows)
                        {
                            while(sdr.Read())
                            {
                                data.Add(new VillageList
                                {
                                    villageId = sdr["VillageId"] != DBNull.Value && int.TryParse(sdr["VillageId"].ToString(), out int id) ? id : 0,
                                    Villagename = sdr["VillageName"]?.ToString() ?? ""
                                });
                            }
                        }
                    }

                }
                return data;
            }
            catch
            {
                throw;
            }
          
        }
        public List<Booth> GetBoothList(FilterModel filter, int? limit=null, int? page=null)
        {
            List<Booth> reportList = new List<Booth>();

            try
            {

                con.Open();

                SqlCommand cmd = new SqlCommand("SP_Booth", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mandalIds", filter.mandalIds);
                cmd.Parameters.AddWithValue("@sectorIds", filter.sectorIds);
                cmd.Parameters.AddWithValue("@BoothIds", filter.boothIds);
                //cmd.Parameters.AddWithValue("@villageIds", filter.villageIds);
                cmd.Parameters.AddWithValue("@Action", "BoothList");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Booth report = new Booth
                    {
                        BoothId = Convert.ToInt32(rdr["Booth_Id"]),
                        BoothNumber = rdr["BoothNumber"].ToString(),
                        MandalName = rdr["MandalName"].ToString(),
                        BoothName = rdr["BoothName"].ToString(),
                        SectorName = rdr["SectorName"].ToString(),
                        SectorIncName = rdr["SectorIncName"].ToString(),
                        SectorIncPhone = rdr["SectorIncPhone"].ToString(),
                        InchargeName = rdr["Incharge_Name"].ToString(),
                        FatherName = rdr["FatherName"].ToString(),
                        Age = rdr["Age"] != DBNull.Value ? Convert.ToInt32(rdr["Age"]) : 0,

                        SubCasteName = rdr["SubCasteName"].ToString(),
                        Address = rdr["Address"].ToString(),
                        Education = rdr["Education"].ToString(),
                        PhoneNumber = rdr["PhoneNumber"].ToString(),
                        ProfileImage = rdr["ProfileImage"].ToString(),
                        PollingStationBoothName = rdr["PollingStationBoothName"] != DBNull.Value ? rdr["PollingStationBoothName"].ToString() : "",

                        // Split the comma-separated village names into a list
                        VillageNames = rdr["VillageNames"] != DBNull.Value ? rdr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>()
                    };

                    reportList.Add(report);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching Combined Report", ex);
            }
            finally
            {
                if (con != null)
                {
                    if (con.State == ConnectionState.Open)
                        con.Close();
                    con.Dispose();
                }
            }

            return reportList;
        }


        public List<Pradhan> GetAllPradhan(int? limit = null, int? page = null)
        {
            var list = new List<Pradhan>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("dbo.sp_Pradhan_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "getPradhan");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pradhan = new Pradhan
                        {
                            Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"]),
                            Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                            villageId = reader["Village_Id"] == DBNull.Value ? null : reader["Village_Id"].ToString(),
                            Village_Name = reader["VillageNames"] == DBNull.Value ? null : reader["VillageNames"].ToString(),
                            Designation = reader["Designation"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Designation"]),
                            Gender = reader["Gender"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Gender"]),  // 
                            GenderName = reader["GenderName"] == DBNull.Value ? null : reader["GenderName"].ToString(),  // 
                            Contact = reader["Contact"] == DBNull.Value ? null : reader["Contact"].ToString(),
                            CreatedAt = reader["CreatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedAt"]),
                            UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                            Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                            MandalName = reader["MandalName"] == DBNull.Value ? null : reader["MandalName"].ToString(),
                            //VillageNames = reader["VillageNames"] == DBNull.Value
                            //    ? new List<string>()
                            //    : reader["VillageNames"].ToString().Split(',').ToList()
                        };

                        list.Add(pradhan);
                    }
                }
            }

            return list;
        }


        public bool InsertPradhan(Pradhan model)
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_Pradhan_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", model.Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Village_Id", model.villageId ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Village_Name", model.Village_Name ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Designation", model.Designation ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Gender", model.Gender ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Contact", model.Contact ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MandalId", model.Mandal_Id);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Action",model.Id>0? "UPDATE" : "INSERT");

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }


        public bool DeletePradhan(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn)) // <-- Assign connection string
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Pradhan_CRUD", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "deletePradhan");
                        cmd.Parameters.AddWithValue("@Id", Id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public List<Pradhan> GetAllPradhanById(int id)
        {
            var list = new List<Pradhan>();

            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_Pradhan_CRUD", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetByIdPradhan"); // 👈 action name for SP
                cmd.Parameters.AddWithValue("@Id", id); // 👈 action name for SP

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        {
                            var pradhan = new Pradhan
                            {
                                Id = reader["Id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"] == DBNull.Value ? null : reader["Name"].ToString(),
                                villageId = reader["Village_Id"] == DBNull.Value ? null : reader["Village_Id"].ToString(),
                                Village_Name = reader["VillageNames"] == DBNull.Value ? null : reader["VillageNames"].ToString(),
                                Designation = reader["Designation"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Designation"]),
                                Gender = reader["Gender"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Gender"]),  // 
                                GenderName = reader["GenderName"] == DBNull.Value ? null : reader["GenderName"].ToString(),  // 
                                Contact = reader["Contact"] == DBNull.Value ? null : reader["Contact"].ToString(),
                                CreatedAt = reader["CreatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["CreatedAt"]),
                                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["UpdatedAt"]),
                                Designationdata = reader["EffectiveDesignationdata"] == DBNull.Value ? null : reader["EffectiveDesignationdata"].ToString(),
                                MandalName = reader["MandalName"] == DBNull.Value ? null : reader["MandalName"].ToString(),
                                //VillageNames = reader["VillageNames"] == DBNull.Value
                                //    ? new List<string>()
                                //    : reader["VillageNames"].ToString().Split(',').ToList()
                            };

                            list.Add(pradhan);
                        }
                    }
                }

                return list;
            }

            //public bool UpdatePradhan(Pradhan model)
            //{
            //    using (SqlConnection con = new SqlConnection(conn))

            //    using (SqlCommand cmd = new SqlCommand("sp_Pradhan_CRUD", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.AddWithValue("@Action", "UPDATE");
            //        cmd.Parameters.AddWithValue("@Id", model.Id);
            //        cmd.Parameters.AddWithValue("@Name", model.Name ?? (object)DBNull.Value);
            //        cmd.Parameters.AddWithValue("@Village_Id", model.villageId ?? (object)DBNull.Value);
            //        cmd.Parameters.AddWithValue("@Village_Name", model.Village_Name ?? (object)DBNull.Value);
            //        cmd.Parameters.AddWithValue("@Designation", model.Designation ?? (object)DBNull.Value);
            //        cmd.Parameters.AddWithValue("@Gender", model.Gender ?? (object)DBNull.Value);
            //        cmd.Parameters.AddWithValue("@Contact", model.Contact ?? (object)DBNull.Value);
            //        cmd.Parameters.AddWithValue("@Mandal_Id", model.Mandal_Id);
            //        con.Open();
            //        return cmd.ExecuteNonQuery() > 0;
            //    }
            //}
        }


    }
}
