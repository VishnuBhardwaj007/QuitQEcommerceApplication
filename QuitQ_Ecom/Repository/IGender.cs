using QuitQ_Ecom.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Repository
{
    public interface IGender
    {
        Task<List<GenderDTO>> GetAllGenders();
        Task<GenderDTO> GetGenderById(int genderId);
        Task<GenderDTO> AddGender(GenderDTO genderDTO);
        Task<GenderDTO> UpdateGender(GenderDTO genderDTO);
        Task<bool> DeleteGender(int genderId);
    }
}
