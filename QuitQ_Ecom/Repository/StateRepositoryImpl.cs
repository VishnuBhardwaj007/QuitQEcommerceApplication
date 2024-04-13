using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using Microsoft.Extensions.Logging; // Add this namespace for ILogger

namespace QuitQ_Ecom.Repository
{
    public class StateRepositoryImpl : IState
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<StateRepositoryImpl> _logger; // Add ILogger

        public StateRepositoryImpl(QuitQEcomContext context, IMapper mapper, ILogger<StateRepositoryImpl> logger) // Add ILogger to constructor
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<StateDTO> AddState(StateDTO stateDTO)
        {
            try
            {
                var state = _mapper.Map<State>(stateDTO);
                _context.States.Add(state);
                await _context.SaveChangesAsync();
                return _mapper.Map<StateDTO>(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding state: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<bool> DeleteState(int stateId)
        {
            try
            {
                var state = await _context.States.FindAsync(stateId);
                if (state == null)
                    return false;

                _context.States.Remove(state);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting state with ID {stateId}: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<List<StateDTO>> GetAllStates()
        {
            try
            {
                var states = await _context.States.ToListAsync();
                return _mapper.Map<List<StateDTO>>(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving all states: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<StateDTO> GetStateById(int stateId)
        {
            try
            {
                var state = await _context.States.FindAsync(stateId);
                return _mapper.Map<StateDTO>(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving state with ID {stateId}: {ex.Message}");
                throw; // Re-throw the exception
            }
        }

        public async Task<StateDTO> UpdateState(int stateId, StateDTO stateDTO)
        {
            try
            {
                var state = await _context.States.FindAsync(stateId);
                if (state == null)
                    return null;

                state.StateName = stateDTO.StateName;
                await _context.SaveChangesAsync();
                return _mapper.Map<StateDTO>(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating state with ID {stateId}: {ex.Message}");
                throw; // Re-throw the exception
            }
        }
    }
}
