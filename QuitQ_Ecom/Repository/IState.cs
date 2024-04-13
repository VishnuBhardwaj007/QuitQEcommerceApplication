using QuitQ_Ecom.DTOs;

namespace QuitQ_Ecom.Repository
{
    public interface IState
    {
        Task<List<StateDTO>> GetAllStates();
        Task<StateDTO> GetStateById(int stateId);
        Task<StateDTO> AddState(StateDTO stateDTO);
        Task<StateDTO> UpdateState(int stateId, StateDTO stateDTO);
        Task<bool> DeleteState(int stateId);
    }
}
