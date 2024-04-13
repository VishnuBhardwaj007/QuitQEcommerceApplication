using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface ICity
    {
        Task<CityDTO> GetCityById(int cityId);
        Task<CityDTO> UpdateCityState(int cityId, int stateId);
        Task<List<CityDTO>> GetAllCities();
        Task<CityDTO> AddCity(CityDTO cityDTO);
        Task<bool> DeleteCity(int cityId);
    }
}
