using GlobalVidhanSabha.Models.SamithiMember;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static GlobalVidhanSabha.Models.SamithiMember.VidhanSabhaModel;

public class SamithiMemberService : ISamithiMemberService
{
    private readonly string conn;
    private SqlConnection con;

    public SamithiMemberService()
    {
        conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
    }
    // ADD THIS METHOD HERE
 
    public async Task<int> SaveMemberAsync(SamithiMemberModel member)
    {
        using (SqlConnection con = new SqlConnection(conn))
        using (SqlCommand cmd = new SqlCommand("sp_ManageSamithiMember", con))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            string action = member.Id.HasValue && member.Id > 0 ? "UPDATE" : "ADD";

            cmd.Parameters.AddWithValue("@Action", action);

            if (action == "UPDATE")
                cmd.Parameters.AddWithValue("@Id", member.Id.Value);

            cmd.Parameters.AddWithValue("@DesignationType", (object)member.DesignationType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SamithiMember", (object)member.SamithiMember ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)member.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PhoneNo", (object)member.PhoneNo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Category", (object)member.Category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Caste", (object)member.Caste ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Education", (object)member.Education ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Profession", (object)member.Profession ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Address", (object)member.Address ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProfilePath", (object)member.ProfilePath ?? DBNull.Value);

            await con.OpenAsync();

            return await cmd.ExecuteNonQueryAsync();
        }
    }


    public async Task<int> DeleteMemberAsync(int id)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_ManageSamithiMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "DELETE");
                cmd.Parameters.AddWithValue("@Id", id);

                await con.OpenAsync();
                int rows = await cmd.ExecuteNonQueryAsync();
                return rows;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting Samithi member: " + ex.Message);
        }
    }

    public async Task<List<SamithiMemberModel>> GetAllMembersAsync()
    {
        var members = new List<SamithiMemberModel>();
        try
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_ManageSamithiMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETALL");

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        members.Add(new SamithiMemberModel
                        {
                            Id = reader["Id"] as int?,
                            DesignationType = reader["DesignationType"] as int?,
                            SamithiMember = reader["SamithiMember"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNo = reader["PhoneNo"].ToString(),
                            Category = reader["Category"] as int?,
                            Caste = reader["Caste"] as int?,
                            Education = reader["Education"].ToString(),
                            Profession = reader["Profession"].ToString(),
                            Address = reader["Address"].ToString(),
                            ProfilePath = reader["ProfilePath"].ToString()
                        });
                    }
                }
            }
            return members;
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching all Samithi members: " + ex.Message);
        }
    }

    public async Task<SamithiMemberModel> GetMemberByIdAsync(int id)
    {
        SamithiMemberModel member = null;
        try
        {
            using (SqlConnection con = new SqlConnection(conn))
            using (SqlCommand cmd = new SqlCommand("sp_ManageSamithiMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "GETBYID");
                cmd.Parameters.AddWithValue("@Id", id);

                await con.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        member = new SamithiMemberModel
                        {
                            Id = reader["Id"] as int?,
                            DesignationType = reader["DesignationType"] as int?,
                            SamithiMember = reader["SamithiMember"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNo = reader["PhoneNo"].ToString(),
                            Category = reader["Category"] as int?,
                            Caste = reader["Caste"] as int?,
                            Education = reader["Education"].ToString(),
                            Profession = reader["Profession"].ToString(),
                            Address = reader["Address"].ToString(),
                            ProfilePath = reader["ProfilePath"].ToString()
                        };
                    }
                }
            }
            return member;
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching Samithi member by ID: " + ex.Message);
        }
    }
}
