using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalVidhanSabha.Models.AdminMain
{
    internal interface IAdminService
    {

        // Designation Methods
        Task<int> SaveDesignationAsync(designationMain model);
        Task<bool> DeleteDesignationAsync(int designationId);
        Task<designationMain> GetDesignationByIdAsync(int designationId);
        Task<PagedResult<designationMain>> GetAllDesignationsAsync(Pagination paging);

        // StateCount Methods
        Task<int> SaveStateCountAsync(stateCountMain model);
        Task<bool> DeleteStateCountAsync(int StateId);
        Task<stateCountMain> GetStateCountByIdAsync(int id);
        Task<PagedResult<stateCountMain>> GetAllStateCountsAsync(Pagination paging);
        Task<List<State>> GetAllStateAsync();

        // Distict Methods
        Task<int> SaveDistictAsync(DistrictCountModel model);
        Task<bool> DeleteDistictAsync(int DistrictId);
        Task<DistrictCountModel> GetDistictByIdAsync(int id);
        Task<PagedResult<DistrictCountModel>> GetAllDistictAsync(Pagination paging);
        Task<List<DistrictModel>> GetDistrictsByStateAsync(int stateId);

        //Caste and category

        Task<List<CasteCategory>> GetSubCasteByCategoryAsync(int categoryId);
        Task<List<CasteCategory>> GetCasteCategoryAsync();

        //VidhanSabhaRagitertration
        Task<int> SaveVidhanSabhaRegistrationAsync(VidhanSabhaRegister model);
        Task<PagedResult<VidhanSabhaRegister>> GetAllVidhanSabhaAsync(Pagination paging, bool? Prabhari = null);
        Task<VidhanSabhaRegister> GetVidhanSabhaByIdAsync(int id);
        Task<bool> DeleteVidhanSabhaAsync(int id);


        Task<List<DistrictCountModel>> GetAllDistrictsDataByStateId(int stateId);
        Task<PagedResult<VidhanSabhaRegister>> GetVidhanSabhaByStateIdAsync(int DistrictId, Pagination paging);


        Task<Dashboard> GetDashboardCountsAsync();
        Task<List<KeyValuePair<string, int>>> GetStateWiseVidhanSabhaChartAsync();
        Task<List<KeyValuePair<string, int>>> GetDistrictWiseVidhanSabhaChartAsync();

        Task<List<designationMain>> GetDesignationTypeAsync();


    }


}
