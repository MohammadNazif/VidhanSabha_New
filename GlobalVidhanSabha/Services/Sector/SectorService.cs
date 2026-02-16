using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using VishanSabha.Models;

namespace VishanSabha.Services.SectorService
{
    public class SectorService
    {
        private readonly string conn;

        private SqlConnection con;
        public SectorService()
        {
            conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            con = new SqlConnection(conn);
        }

        public string GetsectorInchargeId(string contactno)
            {
            string contact = "";

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetSectorInchargeId");
                        cmd.Parameters.AddWithValue("@PhoneNumber", contactno);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                contact = sdr["Id"].ToString();

                            }
                        }
                    }
                }
                return contact;
            }
            catch
            {
                throw;
            }
        }
        public int GetBoothCountBySectorId(int sectorIdStr)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "getCountAllBoothSectorId");
                    cmd.Parameters.AddWithValue("@Id", sectorIdStr);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return count;
        }

        public List<Booth> GetBoothsBySectorId(int? sectorId,FilterModel filter,
int? limit = null, int? page = null)
        {
            List<Booth> boothList = new List<Booth>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                        con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "getAllBoothSectorId");
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Id", sectorId);
                        if (filter != null)
                        {
                            cmd.Parameters.AddWithValue("@boothIds", filter.boothIds != null ? filter.boothIds : null);
                            cmd.Parameters.AddWithValue("@casteIds", filter.casteIds != null ? filter.casteIds : null);
                        }

                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                boothList.Add(new Booth
                                {
                                    Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                    Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                    Sector_Id = dr["Sector_Id"] != DBNull.Value ? Convert.ToInt32(dr["Sector_Id"]) : 0,

                                    MandalName = dr["MandalName"] != DBNull.Value ? dr["MandalName"].ToString() : null,
                                    PollingStationBoothName = dr["PollingStationBoothName"] != DBNull.Value ? dr["PollingStationBoothName"].ToString() : null,
                                    SectorName = dr["SectorName"] != DBNull.Value ? dr["SectorName"].ToString() : null,
                                    BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : null,
                                    Village = dr["VillageName"] != DBNull.Value ? dr["VillageName"].ToString() : null,
                                    boothLocation = dr["boothLocation"] != DBNull.Value ? dr["boothLocation"].ToString() : null,
                                    BoothName = dr["BoothName"] != DBNull.Value ? dr["BoothName"].ToString() : null,
                                    //CasteId = dr["CasteId"] != DBNull.Value ? dr["CasteId"].ToString() : null,
                                    //SubCasteId = dr["SubCasteId"] != DBNull.Value ? dr["SubCasteId"].ToString() : null,

                                    BoothInchargeName = dr["BoothInchargeName"] != DBNull.Value ? Convert.ToInt32(dr["BoothInchargeName"]) : 0,
                                    InchargeName = dr["Incharge_Name"] != DBNull.Value ? dr["Incharge_Name"].ToString() : null,
                                    FatherName = dr["FatherName"] != DBNull.Value ? dr["FatherName"].ToString() : null,
                                    //Age = dr["Age"] != DBNull.Value ? Convert.ToInt32(dr["Age"]) : 0,
                                    //castname = dr["castname"] != DBNull.Value ? dr["castname"].ToString() : null,
                                    //subcaste = dr["subcaste"] != DBNull.Value ? dr["subcaste"].ToString() : null,
                                    Address = dr["Address"] != DBNull.Value ? dr["Address"].ToString() : null,
                                    Education = dr["Education"] != DBNull.Value ? dr["Education"].ToString() : null,
                                    PhoneNumber = dr["PhoneNumber"] != DBNull.Value ? dr["PhoneNumber"].ToString() : null,
                                    ProfileImage = dr["ProfileImage"] != DBNull.Value ? dr["ProfileImage"].ToString() : null,
                                    Status = dr["Status"] != DBNull.Value && Convert.ToBoolean(dr["Status"]),
                                    castname = dr["subcastename"].ToString()
                                });
                            }
                        }
                    }
                }

                return boothList;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving booth data.", ex);
            }

        }
        public int GetPannaCountBySectorId(int sectorIdStr)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "getPannaPramukhCount");
                    cmd.Parameters.AddWithValue("@id", sectorIdStr);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return count;
        }

        public List<PannaPramukh> GetPannaBySectorId(int sectorId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<PannaPramukh> PannaList = new List<PannaPramukh>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "getAllPannaSectorId");

                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Id", sectorId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds != null ? filter.casteIds : null);

                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            PannaList.Add(new PannaPramukh
                            {
                                //PannaPramukh_Id = dr["PannaPramukh_Id"] != DBNull.Value ? Convert.ToInt32(dr["PannaPramukh_Id"]) : 0,
                                //Mandal_Id = dr["Mandal_Id"] != DBNull.Value ? Convert.ToInt32(dr["Mandal_Id"]) : 0,
                                MandalName = dr["MandalName"]?.ToString(),
                                BoothNumber = dr["BoothNumber"] != DBNull.Value ? dr["BoothNumber"].ToString() : "",
                                SectorName = dr["SectorName"]?.ToString(),
                                Booth_Id = dr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(dr["Booth_Id"]) : 0,
                                BoothName = dr["BoothName"]?.ToString(),
                                Pannapramukh = dr["PannaPramukh"]?.ToString(),
                                PannaNumber = dr["PannaNumber"]?.ToString(),
                                PollingStationBoothName = dr["PollingStationBoothName"]?.ToString(),
                                //Village = dr["VillageName"]?.ToString(),
                                Villages = dr["VillageName"] != DBNull.Value ? dr["VillageName"].ToString() : null,
                                //Cast = dr["Cast"]?.ToString(),
                                //Castename = dr["Castename"]?.ToString(),
                                // Category = dr["Category"]?.ToString(),
                                 Categoryname = dr["Categoryname"]?.ToString(),
                                SubCasteName = dr["SubCasteName"]?.ToString(),
                                VoterNumber = dr["VoterNumber"]?.ToString(),
                                //AadharNumber = dr["AadharNumber"]?.ToString(),
                                Address = dr["Address"]?.ToString(),
                                Mobile = dr["Mobile"]?.ToString(),
                                ProfileImageUrl = dr["ProfileImageUrl"]?.ToString(),  // Assuming image path/url from DB
                                //Status = dr["Status"] != DBNull.Value && Convert.ToBoolean(dr["Status"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving panna data.", ex);
            }

            return PannaList;
        }


        //getallcountboothvoterdes

        public int GetBoothVoterDesCountBySectorId(int sectorIdStr)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "getCountAllTotalVoderdes");
                    cmd.Parameters.AddWithValue("@Id", sectorIdStr);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return count;
        }

        public List<BoothVotersDes> GetBoothVoterdesBySectorId(int sectorId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<BoothVotersDes> boothVotersDesList = new List<BoothVotersDes>();

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "getAllTotalVoderdesId");
                        cmd.Parameters.AddWithValue("@Id", sectorId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        con.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                BoothVotersDes boothDes = new BoothVotersDes
                                {
                                    Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
                                    BoothName = dr["BoothName"]?.ToString(),
                                    BoothNumber = dr["BoothNumber"]?.ToString(),
                                    PollingStationBoothName = dr["PollingStationBoothName"]?.ToString(),
                              
                                    TotalVoters = dr["TotalVoters"] != DBNull.Value ? Convert.ToInt32(dr["TotalVoters"]) : 0,
                                    TotalMan = dr["TotalMan"] != DBNull.Value ? Convert.ToInt32(dr["TotalMan"]) : 0,
                                    TotalWoman = dr["TotalWoman"] != DBNull.Value ? Convert.ToInt32(dr["TotalWoman"]) : 0,
                                    TotalOther = dr["TotalOther"] != DBNull.Value ? Convert.ToInt32(dr["TotalOther"]) : 0,
                                    BoothId = dr["BoothId"] != DBNull.Value ? Convert.ToInt32(dr["BoothId"]) : 0,

                                    // Optional: if needed
                                    SectorName = dr["SectorName"]?.ToString(),
                                    MandalName = dr["MandalName"]?.ToString()
                                };

                                boothVotersDesList.Add(boothDes);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the actual error
                throw new Exception("An error occurred while retrieving booth voter data: " + ex.Message, ex);
            }

            return boothVotersDesList;
        }

        //boothSamithi

        public int GetboothSamithiBySectorId(int sectorIdStr)
        {
            int count = 0;
            using (SqlConnection con = new SqlConnection(conn))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "getCountAllBoothsamithi");
                    cmd.Parameters.AddWithValue("@Id", sectorIdStr);

                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return count;
        }

        public List<BoothSamiti> GetAllBoothSamitiBySectorIncId(int SectorIncId,FilterModel filter,int? limit=null,int? page=null)
        {
            List<BoothSamiti> data = new List<BoothSamiti>();
            try
            {
                using(SqlConnection con=new SqlConnection(conn))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetBoothSamitiBySectorId");
                        cmd.Parameters.AddWithValue("@id",SectorIncId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@desgIds", filter.desgIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        //cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if(sdr.HasRows)
                        {
                            while(sdr.Read())
                            {
                                data.Add(new BoothSamiti
                                {
                                    BoothSamiti_Id = Convert.ToInt32(sdr["Id"]),
                                    BoothNumber =sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    BoothIncharge = sdr["Incharge_Name"].ToString(),
                                    //Village = sdr["VillageName"].ToString(),
                                    Name = sdr["Name"].ToString(),
                                    Designation = sdr["DesigName"].ToString(),
                                    Age = Convert.ToInt32(sdr["Age"]),
                                    Mobile = sdr["Mobile"].ToString(),
                                    Occupation = sdr["Occupation"].ToString(),
                                    CategoryName = sdr["CategoryName"].ToString(),
                                    SubCasteName = sdr["SubCasteName"].ToString(),
                                    MandalName = sdr["MandalName"].ToString(),
                                    sectorName = sdr["SectorName"].ToString(),
                                     PollingStationBoothName= sdr["PollingStationBoothName"]!=DBNull.Value? sdr["PollingStationBoothName"].ToString():""
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


        public int GetAllActivity()
        {
            int totalActivity = 0;
            try
            {
                using(SqlConnection con=new SqlConnection(conn))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getactivityCount");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                totalActivity = Convert.ToInt32(sdr["totalActivity"]);
                            }
                        }
                    }
                }
                return totalActivity;
            }
            catch
            {
                throw;
            }
        }

        public List<Activities> GetAllActivityForSectorInc(int? limit = null, int? page = null)
        {
            List<Activities> data = new List<Activities>();
            try
            {
                using(SqlConnection con=new SqlConnection(conn))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllActivityforSector");
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr=cmd.ExecuteReader();
                        if(sdr.HasRows)
                        {
                            while(sdr.Read())
                            {
                                data.Add(new Activities
                                {
                                    ActivityId = Convert.ToInt32(sdr["ActivityId"]),
                                    Title = sdr["Title"].ToString(),
                                    Description = sdr["Description"].ToString(),
                                    ActivityDate = Convert.ToDateTime(sdr["ActivityDate"]),
                                    VideoUrl = sdr["VideoUrl"]?.ToString(),
                                    ImagePaths = JsonConvert.DeserializeObject<List<string>>(sdr["ImagePaths"]?.ToString() ?? "[]"),
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

        public int GetPravasiCountBySecIncId(int secIncId)
        {
            int totalPravasi = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getPravasiCountBySectorIncCount");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                totalPravasi = Convert.ToInt32(sdr["Totalcount"]);
                            }
                        }
                    }
                }
                return totalPravasi;
            }
            catch
            {
                throw;
            }
        }
        public List<PravasiList> GetAllPravasiBySecIncId(int secIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<PravasiList> data = new List<PravasiList>();
            try
            {
                using(SqlConnection con=new SqlConnection(conn))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetPravasiListbySecIncId");
                        cmd.Parameters.AddWithValue("@id",secIncId);
                        cmd.Parameters.AddWithValue("@boothIds",filter.boothIds);
                        cmd.Parameters.AddWithValue("@casteIds",filter.casteIds);
                        cmd.Parameters.AddWithValue("@occuIds",filter.occuIds);

                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if(sdr.HasRows)
                        {
                            while(sdr.Read())
                            {
                                data.Add(new PravasiList
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    MandalName = sdr["MandalName"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    BoothNo = sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    Village = sdr["VillageName"].ToString(),
                                    name = sdr["name"].ToString(),
                                    Category = sdr["CategoryName"].ToString(),
                                    Caste = sdr["SubCasteName"].ToString(),
                                    Mobile = sdr["mobile"].ToString(),
                                    CurrAddress = sdr["currentAddress"].ToString(),
                                    Occupation = sdr["occupationName"].ToString(),
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

        public int GetNewVotersCountBySecIncId(int secIncId)
        {
            int totalNewVoter = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetNewVoterCountBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                totalNewVoter = Convert.ToInt32(sdr["Totalcount"]);
                            }
                        }
                    }
                }
                return totalNewVoter;
            }
            catch
            {
                throw;
            }
        }

        public List<NewVoterList> GetAllNewvoterBySecIncId(int secIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<NewVoterList> data = new List<NewVoterList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetNewvotersBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new NewVoterList
                                {
                                    id= Convert.ToInt32(sdr["id"]),
                                    MandalName = sdr["MandalName"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    BoothNo = sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    Village= sdr["VillageName"].ToString(),
                                    name = sdr["VotersName"].ToString(),
                                    Category = sdr["CategoryName"].ToString(),
                                    Caste = sdr["SubCasteName"].ToString(),
                                    Mobile = sdr["MobileNumber"].ToString(),
                                    fathername = sdr["FatherName"].ToString(),
                                    dob = sdr["DOB"].ToString(),
                                    totalAge = sdr["totalAge"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"].ToString(),
                                                                
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


        public int GetEffectivePersonCountBySecIncId(int secIncId)
        {
            int effectivePerson = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getEffectivePersonCountBysecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                effectivePerson = Convert.ToInt32(sdr["Totalcount"]);
                            }
                        }
                    }
                }
                return effectivePerson;
            }
            catch
            {
                throw;
            }
        }


        public List<EffectivePersonList> GetAllEffectivePersonBySecIncId(int secIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<EffectivePersonList> data = new List<EffectivePersonList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllEffectivePersonBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@desgIds", filter.desgIds);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new EffectivePersonList
                                {
                                    id = Convert.ToInt32(sdr["effectivePersonId"]),
                                    MandalName = sdr["MandalName"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    BoothNo = sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    Village = sdr["VillageName"].ToString(),
                                    Name = sdr["Name"].ToString(),
                                    Category = sdr["CategoryName"].ToString(),
                                    Caste = sdr["SubCasteName"].ToString(),
                                    Mobile = sdr["Mobile"].ToString(),
                                    Description = sdr["Description"].ToString(),
                                    Designation = sdr["Designation"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"].ToString(),
                               



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

        public int GetDoubleVoteCountBySecIncId(int secIncId)
        {
            int doubleVote = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "doubleVoteCountBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                doubleVote = Convert.ToInt32(sdr["Totalcount"]);
                            }
                        }
                    }
                }
                return doubleVote;
            }
            catch
            {
                throw;
            }
        }


        public List<DoubleVoteList> GetAllDoublevoteBySecIncId(int secIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<DoubleVoteList> data = new List<DoubleVoteList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllDoubleVoteBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new DoubleVoteList
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    MandalName = sdr["MandalName"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    BoothNo = sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    Village = sdr["VillageName"] != DBNull.Value ? sdr["VillageName"].ToString() : null,
                                    Name = sdr["Name"].ToString(),
                                    voterid = sdr["voterno"].ToString(),
                                    FatherName = sdr["fathername"].ToString(),
                                    currentAdd = sdr["currentAddress"].ToString(),
                                    preAdd = sdr["pastaddress"].ToString(),
                                    reason = sdr["reason"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"].ToString(),




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

        public int GetSatisfiedCountBySecIncId(int secIncId)
        {
            int satisfied = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllSatisfiedCountBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                satisfied = Convert.ToInt32(sdr["TotalSatisfiedCount"]);
                            }
                        }
                    }
                }
                return satisfied;
            }
            catch
            {
                throw;
            }
        }


        public int GetUnSatisfiedCountBySecIncId(int secIncId)
        {
            int satisfied = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllunSatisfiedCountBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                satisfied = Convert.ToInt32(sdr["TotalSatisfiedCount"]);
                            }
                        }
                    }
                }
                return satisfied;
            }
            catch
            {
                throw;
            }
        }


        public List<SatisfiedList> GetAllSatisfiedBySecIncId(int secIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<SatisfiedList> data = new List<SatisfiedList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllSatisfiedBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        //cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);

                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SatisfiedList
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    mandalname = sdr["MandalName"].ToString(),
                                    sectorname = sdr["SectorName"].ToString(),
                                    boothno = sdr["BoothNumber"].ToString(),
                                    boothname = sdr["BoothName"].ToString(),
                                    name = sdr["name"].ToString(),
                                    Satisfied = sdr["Type"].ToString(),
                                    age = Convert.ToInt32(sdr["age"]),
                                    mobile = sdr["mobile"].ToString(),
                                    party = sdr["party"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"].ToString(),
                                    villageName = sdr["VillageNames"] != DBNull.Value ? sdr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),
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
        public List<SatisfiedList> GetAllUnSatisfiedBySecIncId(int secIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<SatisfiedList> data = new List<SatisfiedList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllUnSatisfiedBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SatisfiedList
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    mandalname = sdr["MandalName"].ToString(),
                                    sectorname = sdr["SectorName"].ToString(),
                                    boothno = sdr["BoothNumber"].ToString(),
                                    boothname = sdr["BoothName"].ToString(),
                                    name = sdr["name"].ToString(),
                                    Unsatisfied = sdr["Type"].ToString(),
                                    age = Convert.ToInt32(sdr["age"]),
                                    mobile = sdr["mobile"].ToString(),
                                    party = sdr["party"].ToString(),
                                    reason = sdr["reason"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"].ToString(),
                                    villageName = sdr["VillageNames"] != DBNull.Value ? sdr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),



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


        public int GetSeniorCountBySecIncId(int secIncId)
        {
            int senior = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetAllSeniorCitizencountBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                senior = Convert.ToInt32(sdr["TotalSeniorCount"]);
                            }
                        }
                    }
                }
                return senior;
            }
            catch
            {
                throw;
            }
        }
        public int GetHandicapedCount(int secIncId)
        {
            int Handicaped = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "DashboardallCountBySectorIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                Handicaped = Convert.ToInt32(sdr["TotalDisabled"]);
                            }
                        }
                    }
                }
                return Handicaped;
            }
            catch
            {
                throw;
            }
        }
        public List<SeniorCitizenList> GetAllSeniorCitizenBySecIncId(int secIncId ,FilterModel filter, int? limit = null, int? page = null)
        {
            List<SeniorCitizenList> data = new List<SeniorCitizenList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllSeniorCitizenBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SeniorCitizenList
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    MandalName = sdr["MandalName"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    Village = sdr["VillageName"].ToString(),
                                    Boothno = sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    SeniorOrDiasabled = sdr["Type"].ToString(),
                                    Name = sdr["Name"].ToString(),
                                    Address = sdr["address"].ToString(),
                                    Category = sdr["CategoryName"].ToString(),
                                    Caste = sdr["SubCasteName"].ToString(),
                                    Mobile = sdr["mobile"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"].ToString(),
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
        public List<SeniorCitizenList> GetAllHandiCapBySecIncId(int secIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<SeniorCitizenList> data = new List<SeniorCitizenList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllHandiCapListBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        cmd.Parameters.AddWithValue("@boothIds", filter.boothIds);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SeniorCitizenList
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    MandalName = sdr["MandalName"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    Village = sdr["VillageName"].ToString(),
                                    Boothno = sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    SeniorOrDiasabled = sdr["Type"].ToString(),
                                    Name = sdr["Name"].ToString(),
                                    Address = sdr["address"].ToString(),
                                    Category = sdr["CategoryName"].ToString(),
                                    Caste = sdr["SubCasteName"].ToString(),
                                    Mobile = sdr["mobile"].ToString(),
                                    PollingStationBoothName = sdr["PollingStationBoothName"].ToString(),
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

        public List<AllowAccessList> getAllowAccessDataBySecIncId(int secIncId)
        {
            List<AllowAccessList> data = new List<AllowAccessList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllowaccessDetailBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new AllowAccessList
                                {
                                    id = Convert.ToInt32(sdr["Id"]),
                                    type = sdr["type"].ToString(),
                                    allowFor = sdr["AllowFor"].ToString(),
                                    allowStatus = sdr["allowStatus"].ToString(),
                                    sectorname = sdr["SectorName"].ToString(),
                                    sectorOrBoothId = sdr["sectorOrBoothId"].ToString()
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

        public List<SectorDashboardCardsCount> SectorDashboardCount(int SecIncId)
        {
            List<SectorDashboardCardsCount> data = new List<SectorDashboardCardsCount>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "DashboardallCountBySectorIncId");
                        cmd.Parameters.AddWithValue("@id",SecIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new SectorDashboardCardsCount
                                {
                                    SectorId = Convert.ToInt32(sdr["SectorId"]),
                                    TotalBooths = Convert.ToInt32(sdr["TotalBooths"]),
                                    TotalPannaPramukh = Convert.ToInt32(sdr["TotalPannaPramukh"]),
                                    TotalSatisfied = Convert.ToInt32(sdr["TotalSatisfied"]),
                                    TotalUnsatisfied = Convert.ToInt32(sdr["TotalUnsatisfied"]),
                                    TotalPravasi = Convert.ToInt32(sdr["TotalPravasi"]),
                                    TotalNewVoter = Convert.ToInt32(sdr["TotalNewVoter"]),
                                    totalDoubleVoter = Convert.ToInt32(sdr["totalDoubleVoter"]),
                                    TotalBoothSamiti = Convert.ToInt32(sdr["TotalBoothSamiti"]),
                                    TotalEffectivePerson = Convert.ToInt32(sdr["TotalEffectivePerson"]),
                                    TotalSenior = Convert.ToInt32(sdr["TotalSenior"]),
                                    TotalDisabled = Convert.ToInt32(sdr["TotalDisabled"]),
                                    TotalBoothVoterDes = Convert.ToInt32(sdr["TotalBoothVoterDes"]),
                                    TotalPost = Convert.ToInt32(sdr["TotalPost"]),
                                    TotalActivity = Convert.ToInt32(sdr["TotalActivity"]),
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
        public int BoothVoterDesBySecIncId(int secIncId)
        {
            int count = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "BoothVoterDesCountBySecIncId");
                        cmd.Parameters.AddWithValue("@id", secIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                count = Convert.ToInt32(sdr["TotalBoothVoterDes"]);
                            }
                        }
                    }
                }
                return count;
            }
            catch
            {
                throw;
            }
        }
        public List<SectorInchargeProfile> GetSectorIncProfile(int sectorId)
        {
            List<SectorInchargeProfile> data = new List<SectorInchargeProfile>();
            try
            {
                using(SqlConnection con=new SqlConnection(conn))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getsectorProfile");
                        cmd.Parameters.AddWithValue("@id", sectorId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if(sdr.HasRows)
                        {
                            while(sdr.Read())
                            {
                                data.Add(new SectorInchargeProfile
                                {
                                    SectorId = Convert.ToInt32(sdr["Id"]),
                                    SectorName = sdr["SectorName"].ToString(),
                                    SectorInchargeName = sdr["Incharge_Name"].ToString(),
                                    profileImg = sdr["ProfileImage"] != DBNull.Value ? sdr["ProfileImage"].ToString():""
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

        public List<SocialMediaPostLink> GetSocialMediaPostBySectorId(int sectorId)
        {
            List<SocialMediaPostLink> data = new List<SocialMediaPostLink>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_sectorInchargePannel", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetSocialMediaPostBysectorId");
                        cmd.Parameters.AddWithValue("@id", sectorId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                string platformsString = sdr["Platform"].ToString();
                                string[] platformArray = platformsString.Split(',').Select(p => p.Trim()).ToArray();

                                data.Add(new SocialMediaPostLink
                                {
                                    PostId = Convert.ToInt32(sdr["PostId"]),
                                    title = sdr["title"].ToString(),
                                    Description = sdr["Description"].ToString(),
                                    PostUrl = sdr["image"].ToString(),
                                    Platform = platformArray
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

        #region For Filter Some Common Services
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
        #endregion

        //public List<BoothVotersDes> GetBoothVoterdesBySectorId(int sectorId)
        //{
        //    List<BoothVotersDes> boothVotersDesList = new List<BoothVotersDes>();

        //    try
        //    {zdf
        //            using (SqlCommand cmd = new SqlCommand("SP_Sector", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@Action", "getAllTotalVoderdesId");
        //                cmd.Parameters.AddWithValue("@Id", sectorId);

        //                con.Open();
        //                using (SqlDataReader dr = cmd.ExecuteReader())
        //                {
        //                    while (dr.Read())
        //                    {
        //                        BoothVotersDes boothDes = new BoothVotersDes
        //                        {
        //                            Id = dr["Id"] != DBNull.Value ? Convert.ToInt32(dr["Id"]) : 0,
        //                            BoothName = dr["BoothName"]?.ToString(),
        //                            BoothNumber = dr["BoothNumber"]?.ToString(),
        //                            TotalVoters = dr["TotalVoters"] != DBNull.Value ? Convert.ToInt32(dr["TotalVoters"]) : 0,
        //                            TotalMan = dr["TotalMan"] != DBNull.Value ? Convert.ToInt32(dr["TotalMan"]) : 0,
        //                            TotalWoman = dr["TotalWoman"] != DBNull.Value ? Convert.ToInt32(dr["TotalWoman"]) : 0,
        //                            TotalOther = dr["TotalOther"] != DBNull.Value ? Convert.ToInt32(dr["TotalOther"]) : 0,
        //                            BoothId = dr["BoothId"] != DBNull.Value ? Convert.ToInt32(dr["BoothId"]) : 0,

        //                            // Optional: if needed
        //                            SectorName = dr["SectorName"]?.ToString(),
        //                            MandalName = dr["MandalName"]?.ToString()
        //                        };

        //                        boothVotersDesList.Add(boothDes);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the actual error
        //        throw new Exception("An error occurred while retrieving booth voter data: " + ex.Message, ex);
        //    }

        //    return boothVotersDesList;
        //}

    }
}