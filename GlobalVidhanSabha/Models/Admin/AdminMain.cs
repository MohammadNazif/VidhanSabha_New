using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlobalVidhanSabha.Models.AdminMain
{
    public class designationMain
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string DesignationType { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class stateCountMain
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public int Count { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? RemainingCount { get; set; }
    }

    public class DistrictCountModel
    {
        public int Id { get; set; }
        public int StateId { get; set; }
        public int DistrictId { get; set; }
        public int Count { get; set; }
        public string DistrictName { get; set; }
        public string StateName{ get; set; }

        public int? RemainingCount{ get; set; }


}
    public class DistrictModel
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int stateId { get; set; }
    }

    public class VidhanSabhaRegister
    {
        public int Id { get; set; }

        public int? StateId { get; set; }
        public int? DistrictId { get; set; }
        public string VidhanSabhaName { get; set; }
        public bool Prabhari { get; set; }

        // Optional fields
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public int? Category { get; set; }
        public string CategoryName { get; set; }
        
        public int? Caste { get; set; }
        public String SubCasteName { get; set; }
     
        public string Profile { get; set; }
   
        public string Education { get; set; }
        public string Address { get; set; }
        public string Profession { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Status { get; set; }

       
        public string DistrictName { get; set; }
        public string StateName { get; set; }


    }

    public class Dashboard
    {
        public int TotalVidhanSabhaCount { get; set; }
        public int TotalVidhanSabhaWithPrabhari { get; set; }
        public int TotalVidhanSabhaWithoutPrabhari { get; set; }

        public int TotalUsedStates { get; set; }
        public int TotalUsedDistrict { get; set; }
    }



    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; }
    }

    public class CasteCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PagedResult<T>
    {
        public List<T> data { get; set; }
        public int totalRecords { get; set; }
    }
    public class    Pagination
    {
        public int PageNumber { get; set; }
        public int Items { get; set; }
        public string search { get; set; }
    }
}
