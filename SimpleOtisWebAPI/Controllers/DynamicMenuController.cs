using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleOtisAPI.Domain.DTOs;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.Models;
using SimpleOtisWebAPI.Models;

namespace SimpleOtisWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DynamicMenuController : ControllerBase
    {
        private readonly IDynamicMenu _dynamicMenu;
        private readonly IMapper _mapper;
        public DynamicMenuController(IDynamicMenu dynamicMenu, IMapper mapper)
        {
            _dynamicMenu = dynamicMenu;
            _mapper = mapper;  
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        public async Task<ActionResult<NavigationData>> DynamicMenu([FromQuery]DynamicMenuDTO dynamic)
        {
            try
            {
                var dynamicDTO = _mapper.Map<DynamicMenuDBDTO>(dynamic);
                var dynamicResult = await _dynamicMenu.DynamicMenuDB(dynamicDTO);

                return Ok(dynamicResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
