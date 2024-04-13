using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Repository;
using System;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Controllers
{
    [Route("api/states")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IState _stateRepo;
        private readonly ILogger<StateController> _logger;

        public StateController(IState stateRepo, ILogger<StateController> logger)
        {
            _stateRepo = stateRepo;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllStates()
        {
            try
            {
                var states = await _stateRepo.GetAllStates();
                return Ok(states);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting all states: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("{stateId}")]
        public async Task<IActionResult> GetStateById(int stateId)
        {
            try
            {
                var state = await _stateRepo.GetStateById(stateId);
                if (state == null)
                {
                    return NotFound("State not found");
                }
                return Ok(state);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting state by ID: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddState([FromBody] StateDTO stateDTO)
        {
            try
            {
                var addedState = await _stateRepo.AddState(stateDTO);
                return CreatedAtAction(nameof(GetStateById), new { stateId = addedState.StateId }, addedState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding state: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut("{stateId}")]
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> UpdateState(int stateId, [FromBody] StateDTO stateDTO)
        {
            try
            {
                var updatedState = await _stateRepo.UpdateState(stateId, stateDTO);
                if (updatedState == null)
                {
                    return NotFound("State not found");
                }
                return Ok(updatedState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating state: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{stateId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteState(int stateId)
        {
            try
            {
                var deleted = await _stateRepo.DeleteState(stateId);
                if (!deleted)
                {
                    return NotFound("State not found");
                }
                return Ok("State deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting state: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
