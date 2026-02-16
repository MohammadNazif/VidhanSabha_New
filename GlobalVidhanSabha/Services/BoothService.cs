using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using VishanSabha.Models;

namespace VishanSabha.Services
{
    public class BoothService
    {
        private readonly string conn;

        private SqlConnection con;
        public BoothService()
        {
            conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            con = new SqlConnection(conn);
        }

        public int GetBoothInchargeId(string contactno)
        {
            int contact = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetBoothInchargeId");
                        cmd.Parameters.AddWithValue("@PhoneNumber", contactno);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                contact = Convert.ToInt32(sdr["Id"]);
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
        public List<PannaPramukh> GetallPannaListByBoothIncId(int BoothIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<PannaPramukh> data = new List<PannaPramukh>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "PannaListByIncId");
                        cmd.Parameters.AddWithValue("@id", BoothIncId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new PannaPramukh
                                {
                                    PannaPramukh_Id = Convert.ToInt32(sdr["PannaPramukh_Id"]),
                                    Booth_Id = Convert.ToInt32(sdr["Booth_Id"]),
                                    BoothName = sdr["BoothName"].ToString(),  // Added
                                    Mandal_Id = Convert.ToInt32(sdr["Mandal_Id"]),
                                    MandalName = sdr["MandalName"].ToString(), // Added
                                    BoothNumber = sdr["BoothNumber"].ToString(), // Added
                                    Sector_Id = Convert.ToInt32(sdr["Sector_Id"]),
                                    SectorName = sdr["SectorName"].ToString(), // Added
                                    Pannapramukh = sdr["PannaPramukh"].ToString(),
                                    PannaNumber = sdr["PannaNumber"].ToString(),
                                    //Cast = reader["Cast"].ToString(),
                                    SubCasteName = sdr["SubCasteName"].ToString(),
                                    Categoryname = sdr["CategoryName"].ToString(),
                                    Category = sdr["Id"].ToString(),
                                    Cast = sdr["Cast"].ToString(),
                                    VoterNumber = sdr["VoterNumber"].ToString(),
                                    //AadharNumber = sdr["AadharNumber"].ToString(),
                                    Address = sdr["Address"].ToString(),
                                    Mobile = sdr["Mobile"].ToString(),
                                    ProfileImageUrl = sdr["ProfileImageUrl"]?.ToString(),
                                    Status = Convert.ToBoolean(sdr["Status"]),
                                     village = sdr["VillageNames"].ToString(),
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
        public int GetPannaCountByBoothIncId(int boothIncId)
        {
            int TotalPanna = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getAllPannaByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                TotalPanna = Convert.ToInt32(sdr["allPannaByInchargeId"]);
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
        public int GetBoothVoterCountByBoothIncId(int boothIncId)
        {
            int TotalBoothVoter = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getBoothVoterCountByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                TotalBoothVoter = Convert.ToInt32(sdr["TotalBoothVoter"]);
                            }
                        }
                    }
                }
                return TotalBoothVoter;
            }
            catch
            {
                throw;
            }
        }
        public List<BoothVotersDes> getBoothVoterDesByBoothIncId(int boothincId,int? limit = null, int? page = null)
        {
            List<BoothVotersDes> BoothVoterDesList = new List<BoothVotersDes>();


            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@action", "getBoothVoterListByBoothIncId");
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        cmd.Parameters.AddWithValue("@id", boothincId);

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            BoothVotersDes voter = new BoothVotersDes
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                BoothName = reader["BoothName"].ToString(),
                                BoothNumber = reader["BoothNumber"].ToString(),
                                SectorName = reader["SectorName"].ToString(),
                                MandalName = reader["MandalName"].ToString(),
                                TotalVoters = Convert.ToInt32(reader["TotalVoters"]),
                                TotalMan = Convert.ToInt32(reader["TotalMan"]),
                                TotalWoman = Convert.ToInt32(reader["TotalWoman"]),
                                TotalOther = Convert.ToInt32(reader["TotalOther"]),
                                PollingStationBoothName = reader["PollingStationBoothName"].ToString(),
                            };

                            BoothVoterDesList.Add(voter);
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
                con.Close();
            }

            return BoothVoterDesList;
        }
        public int GetBoothSamitiCountByBoothIncId(int boothIncId)
        {
            int TotalBoothSamiti = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getTotalCountBoothSamitiByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                TotalBoothSamiti = Convert.ToInt32(sdr["TotalBoothSamiti"]);
                            }
                        }
                    }
                }
                return TotalBoothSamiti;
            }
            catch
            {
                throw;
            }
        }

        public List<BoothSamiti> GetAllBoothSamitiByIncId(int boothIncId, int? limit = null, int? page = null)
        {
            List<BoothSamiti> list = new List<BoothSamiti>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getBoothSamitiListByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothIncId);

                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            BoothSamiti samiti = new BoothSamiti
                            {
                                BoothSamiti_Id = rdr["Id"] != DBNull.Value ? Convert.ToInt32(rdr["Id"]) : 0,
                                BoothId = rdr["BoothId"] != DBNull.Value ? Convert.ToInt32(rdr["BoothId"]) : 0,
                                BoothName = rdr["BoothName"] != DBNull.Value ? rdr["BoothName"].ToString() : string.Empty,
                                Designation = rdr["DesigName"] != DBNull.Value ? rdr["DesigName"].ToString() : string.Empty,
                                Name = rdr["Name"] != DBNull.Value ? rdr["Name"].ToString() : string.Empty,
                                Category = rdr["Category"].ToString(),
                                Cast = rdr["SubCasteName"] != DBNull.Value ? rdr["SubCasteName"].ToString() : string.Empty,
                                SubCasteName = rdr["Cast"].ToString(),
                                CategoryName = rdr["CategoryName"].ToString(),
                                Age = rdr["Age"] != DBNull.Value ? Convert.ToInt32(rdr["Age"]) : 0,
                                Mobile = rdr["Mobile"] != DBNull.Value ? rdr["Mobile"].ToString() : string.Empty,
                                Occupation = rdr["Occupation"] != DBNull.Value ? rdr["Occupation"].ToString() : string.Empty,
                                BoothIncharge= rdr["BoothIncharge"] != DBNull.Value ? rdr["BoothIncharge"].ToString() : string.Empty
                            };

                            list.Add(samiti);
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
        #region  Nazif

        public List<PravasiVoter> GetAllTotalPravasiList(int boothIncId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<PravasiVoter> PravasiList = new List<PravasiVoter>();


            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetPravasiListByIncharge"); // ✅ Make sure this action is handled in your SP
                        cmd.Parameters.AddWithValue("@id", boothIncId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // First result set: Pravasi List
                            while (reader.Read())
                            {
                                PravasiVoter pravasi = new PravasiVoter
                                {
                                    id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                                    boothno = reader["BoothNo"] != DBNull.Value ? Convert.ToInt32(reader["BoothNo"]) : 0,
                                    BoothName = reader["BoothName"] != DBNull.Value ? reader["BoothName"].ToString() : string.Empty,
                                    PollingStationBoothName = reader["PollingStationBoothName"] != DBNull.Value ? reader["PollingStationBoothName"].ToString() : string.Empty,
                                    MandalName = reader["mandalname"] != DBNull.Value ? reader["mandalname"].ToString() : string.Empty,
                                    SectorName = reader["SectorName"] != DBNull.Value ? reader["SectorName"].ToString() : string.Empty,
                                    Occupation = reader["OccupationName"] != DBNull.Value ? reader["OccupationName"].ToString() : string.Empty,

                                    Category = reader["categoryId"] != DBNull.Value ? Convert.ToInt32(reader["categoryId"]) : 0,
                                    BoothNumber = reader["BoothNumber"] != DBNull.Value ? reader["BoothNumber"].ToString() : "",
                                    Caste = reader["casteId"] != DBNull.Value ? Convert.ToInt32(reader["casteId"]) : 0,
                                    name = reader["name"] != DBNull.Value ? reader["name"].ToString() : string.Empty,
                                    mobile = reader["mobile"] != DBNull.Value ? reader["mobile"].ToString() : string.Empty,
                                    currentAddress = reader["currentAddress"] != DBNull.Value ? reader["CurrentAddress"].ToString() : string.Empty,
                                    CategoryName = reader["CategoryName"] != DBNull.Value ? reader["CategoryName"].ToString() : string.Empty,
                                    CasteName = reader["SubCasteName"] != DBNull.Value ? reader["SubCasteName"].ToString() : string.Empty
                                };

                                PravasiList.Add(pravasi);
                            }


                        }
                    }
                }
            }
            catch
            {
                throw; // Optionally log error here
            }

            return PravasiList;
        }
        public int GetTotalPravasiCount(int boothIncId)
        {
            int total = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GetPravasiListByIncharge");
                    cmd.Parameters.AddWithValue("@id", boothIncId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Skip the first result set (list of Pravasi)
                        if (reader.HasRows)
                        {
                            while (reader.Read()) { } // ignore rows
                        }

                        // Move to second result set (the count)
                        if (reader.NextResult() && reader.Read())
                        {
                            total = reader["TotalPravasi"] != DBNull.Value ? Convert.ToInt32(reader["TotalPravasi"]) : 0;
                        }
                    }
                }
            }

            return total;
        }

        public List<NewVoters> GetNewVotersListByIncharge(int boothInchargeId)
        {
            List<NewVoters> voterList = new List<NewVoters>();


            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GetNewVotersListByIncharge");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // First result: voter list
                        while (reader.Read())
                        {
                            NewVoters nv = new NewVoters
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                BoothNumber = reader["BoothNumber"].ToString(),
                                Name = reader["VotersName"].ToString(),
                                FatherName = reader["FatherName"].ToString(),
                                DOB = reader["DOB"] != DBNull.Value ? Convert.ToDateTime(reader["DOB"]) : DateTime.MinValue,
                                Education = reader["Education"].ToString(),
                                MobileNumber = reader["MobileNumber"].ToString(),
                                Booth_Id = Convert.ToInt32(reader["booth_no"]),
                                CategoryName = reader["CategoryName"].ToString(),
                                SubCasteName = reader["SubCasteName"].ToString(),
                                //Status = Convert.ToInt32(reader["Status"])
                            };
                            voterList.Add(nv);
                        }

                        // Second result: total count

                    }
                }
            }

            return voterList;
        }

        public int GetNewVotersCountByIncharge(int boothInchargeId)

        {

            int totalNewVoters = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GetNewVotersListByIncharge");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // First result: voter list
                        if (reader.HasRows)
                        {
                            while (reader.Read()) { } // ignore rows
                        }

                        // Second result: total count
                        if (reader.NextResult() && reader.Read())
                        {
                            totalNewVoters = Convert.ToInt32(reader["TotalNewVoters"]);
                        }
                    }
                }
            }

            return totalNewVoters;
        }
        public int GetAdminActivitiesCount()

        {

            int totalActivities = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GetAdminActivitiesCount");


                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // First result: voter list


                        // Second result: total count
                        if (reader.Read())
                        {
                            totalActivities = Convert.ToInt32(reader["TotalActivities"]);
                        }
                    }
                }
            }

            return totalActivities;
        }

        #endregion


        public List<BoothDashboardCardsCount> BoothDashboardCount(int BoothIncId)
        {
            List<BoothDashboardCardsCount> data = new List<BoothDashboardCardsCount>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "dashboardCountByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", BoothIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new BoothDashboardCardsCount
                                {
                                    boothId = Convert.ToInt32(sdr["boothId"]),
                                    //TotalBooths = Convert.ToInt32(sdr["TotalBooths"]),
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


        public List<CasteList> getCastVoterDesByBoothIncId(int boothincId,int boothVoterDesId)
        {
            List<CasteList> data = new List<CasteList>();


            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@action", "CasteDetaiByBoothVoterDesId");
                        cmd.Parameters.AddWithValue("@id", boothincId);
                        cmd.Parameters.AddWithValue("@BoothDesId", boothVoterDesId);

                        SqlDataReader reader = cmd.ExecuteReader();
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                data.Add(new CasteList
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    cast_name = reader["SubCasteName"].ToString(),
                                    cast_number= reader["Number"].ToString(),
                             
                                });
                            }
                        }
                       
                    }
                }
                return data;

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

        #region deep

        //Total Sahmat servive
        public int GetSahmatCountByIncharge(int boothInchargeId)
        {
            int totalSahmatCount = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GetSahmatCount");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalSahmatCount = Convert.ToInt32(reader["TotalSahmat"]);
                        }
                    }
                }
            }

            return totalSahmatCount;
        }

        //Total Asahamat servive
        public int GetAsahamatCountByIncharge(int boothInchargeId)
        {
            int TotalAsahamatCount = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GetAsahamatCount");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                           TotalAsahamatCount = Convert.ToInt32(reader["TotalAsahamat"]);
                        }
                    }
                }
            }
            return TotalAsahamatCount;
        }



        public List<SatisfiedUnSatisfied> getallsahmatlist(int boothInchargeId , FilterModel filter,

int? limit = null, int? page = null)
        {
            List<SatisfiedUnSatisfied> AsahamatList = new List<SatisfiedUnSatisfied>();   
                 
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con);
                cmd.CommandType=CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetAllsahamat");
                cmd.Parameters.AddWithValue("@Id", boothInchargeId);
                cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);

                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SatisfiedUnSatisfied Asahamat = new SatisfiedUnSatisfied
                    {
                        id = rdr["Id"] != DBNull.Value ? Convert.ToInt32(rdr["Id"]) : 0,
                        MandalName = rdr["MandalName"] != DBNull.Value ? rdr["MandalName"].ToString() : null,
                        sectorName = rdr["SectorName"] != DBNull.Value ? rdr["SectorName"].ToString() : null,
                        boothNo = rdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(rdr["BoothNumber"]) : 0,
                        BoothName = rdr["BoothName"] != DBNull.Value ? rdr["BoothName"].ToString() : null,

                        sahmatAsahmatName = rdr["Type"] != DBNull.Value ? rdr["Type"].ToString() : null,
                        name = rdr["name"] != DBNull.Value ? rdr["name"].ToString() : null,
                        age = rdr["age"] != DBNull.Value ? Convert.ToInt32(rdr["age"]) : 0,
                        mobile = rdr["mobile"] != DBNull.Value ? rdr["mobile"].ToString() : null,

                        VillageNames = rdr["villageNames"] != DBNull.Value
                            ? new List<string> { rdr["villageNames"].ToString() }
                            : new List<string>(),

                        party = rdr["party"] != DBNull.Value ? rdr["party"].ToString() : null,
                        reason = rdr["reason"] != DBNull.Value ? rdr["reason"].ToString() : null,
                        Occupation = rdr["OccupationName"] != DBNull.Value ? rdr["OccupationName"].ToString() : null,
                        PollingStationBoothName = rdr["PollingStationBoothName"] != DBNull.Value ? rdr["PollingStationBoothName"].ToString() : null
                    };


                    AsahamatList.Add(Asahamat);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return AsahamatList;
        }

        public List<SatisfiedUnSatisfied> getallAsahmatlist(int boothInchargeId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<SatisfiedUnSatisfied> AsahamatList = new List<SatisfiedUnSatisfied>();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetAllAsahamat");
                cmd.Parameters.AddWithValue("@Id", boothInchargeId);
                cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    SatisfiedUnSatisfied Asahamat = new SatisfiedUnSatisfied
                    {
                        id = rdr["Id"] != DBNull.Value ? Convert.ToInt32(rdr["Id"]) : 0,
                        MandalName = rdr["MandalName"] != DBNull.Value ? rdr["MandalName"].ToString() : null,
                        sectorName = rdr["SectorName"] != DBNull.Value ? rdr["SectorName"].ToString() : null,
                        boothNo = rdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(rdr["BoothNumber"]) : 0,
                        BoothName = rdr["BoothName"] != DBNull.Value ? rdr["BoothName"].ToString() : null,

                        sahmatAsahmatName = rdr["Type"] != DBNull.Value ? rdr["Type"].ToString() : null,
                        name = rdr["name"] != DBNull.Value ? rdr["name"].ToString() : null,
                        age = rdr["age"] != DBNull.Value ? Convert.ToInt32(rdr["age"]) : 0,
                        mobile = rdr["mobile"] != DBNull.Value ? rdr["mobile"].ToString() : null,

                        VillageNames = rdr["villageNames"] != DBNull.Value
                            ? new List<string> { rdr["villageNames"].ToString() }
                            : new List<string>(),

                        party = rdr["party"] != DBNull.Value ? rdr["party"].ToString() : null,
                        reason = rdr["reason"] != DBNull.Value ? rdr["reason"].ToString() : null,
                        Occupation = rdr["OccupationName"] != DBNull.Value ? rdr["OccupationName"].ToString() : null,
                        PollingStationBoothName = rdr["PollingStationBoothName"] != DBNull.Value ? rdr["PollingStationBoothName"].ToString() : null,
                    };


                    AsahamatList.Add(Asahamat);
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
            return AsahamatList;
        }
        public int GetTotalDoubleVoterCount(int boothInchargeId)
        {
            int totaldoubleVoter = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "CountDoubleVoterByBoothIncId");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totaldoubleVoter = Convert.ToInt32(reader["totaldoubleVoter"]);
                        }
                    }
                }
            }
            return totaldoubleVoter;
        }

        public List<doubleVoter> getalldoubleVoterByBoothIncId(int boothInchargeId, int? limit = null, int? page = null)
        {
            List<doubleVoter> DoubleList = new List<doubleVoter>();

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GetAlldoubleVoterByBoothIncId");
                cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", boothInchargeId);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    doubleVoter Voter = new doubleVoter
                    {
                        id = rdr["Id"] != DBNull.Value ? Convert.ToInt32(rdr["Id"]) : 0,
                        MandalName = rdr["MandalName"] != DBNull.Value ? rdr["MandalName"].ToString() : null,
                        SectorName = rdr["SectorName"] != DBNull.Value ? rdr["SectorName"].ToString() : null,
                        BoothNumber = rdr["BoothNumber"] != DBNull.Value ? Convert.ToInt32(rdr["BoothNumber"]) : 0,
                        BoothName = rdr["BoothName"].ToString(),

                        //VillageNames = rdr["VillageNames"] != DBNull.Value ? rdr["VillageNames"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList() : new List<string>(),

                        fathername = rdr["FatherName"]?.ToString(),
                        name = rdr["name"]?.ToString(),
                        currentAddress = rdr["currentAddress"]?.ToString(),
                        pastAddress = rdr["past"]?.ToString(),
                        reason = rdr["reason"]?.ToString(),
                        voterno = rdr["voternumber"]?.ToString(),
                        PollingStationBoothName = rdr["PollingStationBoothName"]?.ToString(),
                    }; 


                    DoubleList.Add(Voter);
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
            return DoubleList;
        }

        public List<BoothInchargeProfile> GetBoothInchargeProfileByBoothIncId(int BoothIncId)
        {
            List<BoothInchargeProfile> data = new List<BoothInchargeProfile>();
            try
            {
                using(SqlConnection con=new SqlConnection(conn))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetBoothInchargeProfile");
                        cmd.Parameters.AddWithValue("@id", BoothIncId);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if(sdr.HasRows)
                        {
                            while(sdr.Read())
                            {
                                data.Add(new BoothInchargeProfile
                                {
                                    BoothId = Convert.ToInt32(sdr["Booth_Id"]),
                                    BoothName = sdr["BoothName"].ToString(),
                                    BoothIncharge = sdr["Incharge_Name"].ToString(),
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
        public int GetTotalBoothSamithiCount(int boothInchargeId)
        {
            int totalBoothSamithVoter = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "CountBoothSamithiByBoothIncId");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalBoothSamithVoter = Convert.ToInt32(reader["TotalBoothSamiti"]);
                        }
                    }
                }
            }
            return totalBoothSamithVoter;
        }

        public List<BoothSamiti> GetAllBoothSamitiByBoothIncId(int boothInchargeId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<BoothSamiti> SamithiList = new List<BoothSamiti>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "GetAllBoothSamithiByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothInchargeId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@desgIds", filter.desgIds);
                        cmd.Parameters.AddWithValue("@occuIds", filter.occuIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                SamithiList.Add(new BoothSamiti
                                {
                                    BoothSamiti_Id = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0,

                                    BoothNumber = sdr["BoothNumber"] != DBNull.Value ? sdr["BoothNumber"].ToString() : null,
                                    BoothName = sdr["BoothName"] != DBNull.Value ? sdr["BoothName"].ToString() : null,
                                    PollingStationBoothName = sdr["PollingStationBoothName"] != DBNull.Value ? sdr["PollingStationBoothName"].ToString() : null,
                                    sectorName = sdr["SectorName"] != DBNull.Value ? sdr["SectorName"].ToString() : null,
                                    MandalName = sdr["MandalName"] != DBNull.Value ? sdr["MandalName"].ToString() : null,
                                    CategoryName = sdr["CategoryName"] != DBNull.Value ? sdr["CategoryName"].ToString() : null,
                                    SubCasteName = sdr["SubCasteName"] != DBNull.Value ? sdr["SubCasteName"].ToString() : null,

                                    Name = sdr["Name"] != DBNull.Value ? sdr["Name"].ToString() : null,
                                    Designation = sdr["DesigName"] != DBNull.Value ? sdr["DesigName"].ToString() : null,
                                    Age = sdr["Age"] != DBNull.Value ? Convert.ToInt32(sdr["Age"]) : 0,
                                    Mobile = sdr["Mobile"] != DBNull.Value ? sdr["Mobile"].ToString() : null,
                                    Occupation = sdr["Occupation"] != DBNull.Value ? sdr["Occupation"].ToString() : null
                                });

                            }
                        }
                    }
                }
                return SamithiList;
            }
            catch
            {
                throw;
            }
        }

        public int GetTotalPrabhavsaliCount(int boothInchargeId)
        {
            int TotalPrabhavsali = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "CountPrabhavsaliByBoothIncId");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);
                 


                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TotalPrabhavsali = Convert.ToInt32(reader["TotalPrabhavsali"]);
                        }
                    }
                }
            }
            return TotalPrabhavsali;
        }


        public List<EffectivePersonList> GetAllPrabhavsaliByBoothIncId(int boothInchargeId,FilterModel filter,
int? limit = null, int? page = null)
        {
            List<EffectivePersonList> data = new List<EffectivePersonList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetAllPrabhavsaliByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothInchargeId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@desgIds", filter.desgIds);
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
                                    Designation = sdr["EffectiveDesignationdata"].ToString(),
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

        public int GetTotalNewVotersCount(int boothInchargeId)
        {
            int VoterCount = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "CountNewVoterByBoothIncId");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            VoterCount = Convert.ToInt32(reader["NewVoterCount"]);
                        }
                    }
                }
            }
            return VoterCount;
        }

        public List<NewVoterList> GetAllNewvoterByBoothIncId(int boothInchargeId,FilterModel filter, int? limit = null, int? page = null)
        {
            List<NewVoterList> data = new List<NewVoterList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetAllNewVoterByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothInchargeId);
                        cmd.Parameters.AddWithValue("@casteIds", filter.casteIds);
                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if (sdr.HasRows)
                        {
                            while (sdr.Read())
                            {
                                data.Add(new NewVoterList
                                {
                                    id = Convert.ToInt32(sdr["id"]),
                                    MandalName = sdr["MandalName"].ToString(),
                                    SectorName = sdr["SectorName"].ToString(),
                                    BoothNo = sdr["BoothNumber"].ToString(),
                                    BoothName = sdr["BoothName"].ToString(),
                                    Village = sdr["VillageName"].ToString(),
                                    name = sdr["VotersName"].ToString(),
                                    Category = sdr["CategoryName"].ToString(),
                                    Caste = sdr["SubCasteName"].ToString(),
                                    Mobile = sdr["MobileNumber"].ToString(),
                                    fathername = sdr["FatherName"].ToString(),
                                    dob = sdr["DOB"].ToString(),
                                    totalAge = sdr["totalAge"].ToString(),
                                    education = sdr["Education"].ToString(),
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


        public int GetTotalSeniorCitizenCount(int boothInchargeId)
        {
            int CountSeniorCitizen = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "CountSeniorCitizenByBoothIncId");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CountSeniorCitizen = Convert.ToInt32(reader["TotalCountSeniorCitizen"]);
                        }
                    }
                }
            }
            return CountSeniorCitizen;
        }


        public int CountDisabledByBoothIncId(int boothInchargeId)
        {
            int CountDisabled = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "CountDisabledByBoothIncId");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CountDisabled = Convert.ToInt32(reader["CountDisabled"]);
                        }
                    }
                }
            }
            return CountDisabled;
        }

        public List<SocialMediaPostLink> GetSocialMediaPostByBoothId(int boothInchargeId,
int? limit = null, int? page = null)
        {
            List<SocialMediaPostLink> data = new List<SocialMediaPostLink>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetSocialMediaByBoothId");


                        cmd.Parameters.AddWithValue("@limit", limit.HasValue ? (object)limit : DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", page.HasValue ? (object)page : DBNull.Value);
                        cmd.Parameters.AddWithValue("@Id", boothInchargeId);
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
                                    IsPost = sdr["IsPost"].ToString(),
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

        public List<SeniorCitizenList> GetAllSeniorCitizenByBoothIncId(int boothInchargeId ,FilterModel filter, int? limit = null, int? page = null)
        {
            List<SeniorCitizenList> data = new List<SeniorCitizenList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetALLSeniorCitizenByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothInchargeId);
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
                                    //Village = sdr["VillageName"].ToString(),
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


        public List<SeniorCitizenList> GetAllHandiCapByBoothIncId(int boothInchargeId, FilterModel filter, int? limit = null, int? page = null)
        {
            List<SeniorCitizenList> data = new List<SeniorCitizenList>();
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "GetAllHandicapByBoothIncId");
                        cmd.Parameters.AddWithValue("@id", boothInchargeId);
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


        public int GetCountSocialMediaByBoothId(int boothInchargeId)
        {
            int TotalActivity = 0;

            using (SqlConnection con = new SqlConnection(conn))
            {
                using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "GetCountSocialMediaByBoothId");
                    cmd.Parameters.AddWithValue("@id", boothInchargeId);

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TotalActivity = Convert.ToInt32(reader["TotalActivity"]);
                        }
                    }
                }
            }
            return TotalActivity;
        }

        public void SaveFacebookPostStatus(int userId, int postId, string status)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(conn))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("sp_BoothIncharge", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@BoothId", userId);
                        cmd.Parameters.AddWithValue("@PostId", postId);
                        cmd.Parameters.AddWithValue("@status", "done");
                        cmd.Parameters.AddWithValue("@action", "PostUploadByBooth");

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch 
            {
                throw;
            }
           
        }


        #endregion
    }
}