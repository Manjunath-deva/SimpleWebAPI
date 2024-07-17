using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using SimpleOtisWebAPI.Models;

namespace SimpleOtisWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPreferenceController : ControllerBase
    {
        private readonly IUserPreference _userPreference;
        private readonly IMapper _mapper;
        public UserPreferenceController(IUserPreference userPreference, IMapper mapper)
        {
            _userPreference = userPreference;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<tblPreference>>> UserPreferences([FromQuery]UserPreferenceDTO userPreferences)
        {

            try
            {
                var userPreferenceData = _mapper.Map<UserPreferenceDBDTO>(userPreferences);

                var preferenceResult = await _userPreference.UserPreference(userPreferenceData);

                return Ok(preferenceResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
