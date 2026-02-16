using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VishanSabha.Models;

namespace VishanSabha.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly string _connectionString;

        public AuthService()
        {
                _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
        public int GetBoothInchargeId(string contactNo)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "GetBoothInchargeId");
                    cmd.Parameters.AddWithValue("@PhoneNumber", contactNo);

                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        if (sdr.Read()) 
                        {
                            int boothId = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0;
                            int sectorId = sdr["Id"] != DBNull.Value ? Convert.ToInt32(sdr["Id"]) : 0;

                          
                            return boothId != 0 ? boothId : sectorId;
                        }
                    }
                }

             
                return 0;
            }
            catch (Exception ex)
            {
            
                throw new Exception("Error fetching Booth/Sector Incharge ID", ex);
            }
        }

        //public int GetBoothInchargeId(string contactno)
        //{
        //    int Sector_id = 0;
        //    int Booth_id = 0;

        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(_connectionString))
        //        {
        //            con.Open();
        //            using (SqlCommand cmd = new SqlCommand("SP_Booth", con))
        //            {
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@Action", "GetBoothInchargeId");
        //                cmd.Parameters.AddWithValue("@PhoneNumber", contactno);

        //                using (SqlDataReader sdr = cmd.ExecuteReader())
        //                {
        //                    if (sdr.HasRows)
        //                    {
        //                        while (sdr.Read())
        //                        {
        //                            Booth_id = sdr["Booth_Id"] != DBNull.Value ? Convert.ToInt32(sdr["Booth_Id"]) : 0;
        //                            Sector_id = sdr["SectorId"] != DBNull.Value ? Convert.ToInt32(sdr["SectorId"]) : 0;
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (Sector_id != 0)
        //        {
        //            return Sector_id;
        //        }

        //        return Booth_id;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public Role GetUserByContact(string contact)
        {
            Role user = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
               
                SqlCommand cmd = new SqlCommand("SP_ValidateLogin", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "Role");
                cmd.Parameters.AddWithValue("@Contact", contact);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new Role
                    {
                        Contact = reader["Contact"].ToString(),
                        role = reader["Role"].ToString()
                    };
                }

                reader.Close();
            }

            return user;
        }

  

        public Login ValidateUser(string contact, string password)
        {
            Login user = null;

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_ValidateLogin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Contact", contact);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Action", "LOGIN");

                    con.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Login
                            {
                                Id = Convert.ToInt32(reader["Id"]),

                                Contact = reader["Contact"].ToString(),
                                Password = reader["Password"].ToString(),
                                Role = reader["Role"].ToString(),
                                Status = Convert.ToInt32(reader["Status"]),
                                VidhanSabhaId =reader["VidhanSabhaId"] !=DBNull.Value? Convert.ToInt32(reader["VidhanSabhaId"]):(int?)null
                            };
                        }
                    }
                }
            }

            return user;
        }
        public async Task<string> GetLocation(string latitude, string longitude, int userId)
        {
            try
            {
                string url = $"https://nominatim.openstreetmap.org/reverse?lat={latitude}&lon={longitude}&format=json";

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();

                    // ✅ REQUIRED by Nominatim
                    client.DefaultRequestHeaders.Add(
                        "User-Agent",
                        "VishanSabhaApp/1.0 (support@vishansabha.com)"
                    );

                    client.DefaultRequestHeaders.Add("Accept-Language", "en");

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Nominatim API failed: {(int)response.StatusCode} - {response.ReasonPhrase}");
                    }

                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(responseContent);

                    string city = json["address"]?["city"]?.ToString()
                               ?? json["address"]?["town"]?.ToString()
                               ?? json["address"]?["village"]?.ToString()
                               ?? json["address"]?["hamlet"]?.ToString()
                               ?? "";

                    string state = json["address"]?["state"]?.ToString() ?? "";

                    string location = $"{city}, {state}".Trim(',', ' ');

                    // Save to DB
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        string query = @"
                INSERT INTO tbl_getLocationForLogin
                (userid, latitude, longitude, Location, status, actionDate)
                VALUES (@userid, @latitude, @longitude, @location, 1, GETDATE())";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@userid", userId);
                            cmd.Parameters.AddWithValue("@latitude", latitude);
                            cmd.Parameters.AddWithValue("@longitude", longitude);
                            cmd.Parameters.AddWithValue("@location", location);

                            await conn.OpenAsync();
                            await cmd.ExecuteNonQueryAsync();
                        }
                    }

                    return location;
                }
            }
            catch
            {
                throw;
            }
        }

        public List<getLocation> getAllLocation()
        {
            List<getLocation> data = new List<getLocation>();
            try
            {
                using(SqlConnection con=new SqlConnection(_connectionString))
                {
                    con.Open();
                    using(SqlCommand cmd=new SqlCommand("sp_getLocationForLogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getLocationForLogin");
                        SqlDataReader sdr = cmd.ExecuteReader();
                        if(sdr.HasRows)
                        {
                            while(sdr.Read())
                            {
                                data.Add(new getLocation
                                {
                                    userid = Convert.ToInt32(sdr["userid"]),
                                    latitude = sdr["latitude"].ToString(),
                                    location = sdr["location"].ToString(),
                                    longitude = sdr["longitude"].ToString()
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

    }
}
