using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuitQ_Ecom.Repository
{
    public class CityRepositoryImpl : ICity
    {
        private readonly QuitQEcomContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CityRepositoryImpl> _logger;

        public CityRepositoryImpl(QuitQEcomContext context, IMapper mapper, ILogger<CityRepositoryImpl> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CityDTO> GetCityById(int cityId)
        {
            try
            {
                var city = await _context.Cities.FindAsync(cityId);
                return _mapper.Map<CityDTO>(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get city by ID {CityId}.", cityId);
                throw new Exception("Failed to get city by ID.", ex);
            }
        }

        public async Task<CityDTO> UpdateCityState(int cityId, int stateId)
        {
            try
            {
                var city = await _context.Cities.FindAsync(cityId);
                if (city == null)
                {
                    _logger.LogWarning("City with ID {CityId} not found.", cityId);
                    return null;
                }

                var state = await _context.States.FindAsync(stateId);
                if (state == null)
                {
                    _logger.LogWarning("State with ID {StateId} not found.", stateId);
                    return null;
                }

                city.StateId = stateId;
                await _context.SaveChangesAsync();

                return _mapper.Map<CityDTO>(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update city state.");
                throw new Exception("Failed to update city state.", ex);
            }
        }

        public async Task<List<CityDTO>> GetAllCities()
        {
            try
            {
                var cities = await _context.Cities.ToListAsync();
                return _mapper.Map<List<CityDTO>>(cities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all cities.");
                throw new Exception("Failed to get all cities.", ex);
            }
        }

        public async Task<CityDTO> AddCity(CityDTO cityDTO)
        {
            try
            {
                var city = _mapper.Map<City>(cityDTO);
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();
                return _mapper.Map<CityDTO>(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add city.");
                throw new Exception("Failed to add city.", ex);
            }
        }

        public async Task<bool> DeleteCity(int cityId)
        {
            try
            {
                var city = await _context.Cities.FindAsync(cityId);
                if (city == null)
                {
                    _logger.LogWarning("City with ID {CityId} not found.", cityId);
                    return false;
                }

                _context.Cities.Remove(city);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete city with ID {CityId}.", cityId);
                throw new Exception("Failed to delete city.", ex);
            }
        }
    }
}
