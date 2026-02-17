using System.Collections.Generic;
using System.Threading.Tasks;
using static GlobalVidhanSabha.Models.SamithiMember.VidhanSabhaModel;

namespace GlobalVidhanSabha.Models.SamithiMember
{
    public interface ISamithiMemberService
    {
        //Task<int> AddMemberAsync(SamithiMemberModel member);
        //Task<int> UpdateMemberAsync(SamithiMemberModel member);
        Task<int> SaveMemberAsync(SamithiMemberModel member);
        Task<int> DeleteMemberAsync(int id);
        Task<List<SamithiMemberModel>> GetAllMembersAsync();
        Task<SamithiMemberModel> GetMemberByIdAsync(int id);
    }
}
