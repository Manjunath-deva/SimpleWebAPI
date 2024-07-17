using Microsoft.AspNetCore.Mvc;
using SimpleOtisWebAPI.Models;
using SimpleOtisAPI.Domain.Services;
using AutoMapper;
using SimpleOtisAPI.Domain.Interfaces;
using SimpleOtisAPI.Domain.DTOs;
using System.Text.Json;

namespace SimpleOtisWebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WidgetController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWidgetData _widgetData;
        public WidgetController(IMapper mapper, IWidgetData widgetData)
        {
            _mapper = mapper;
            _widgetData = widgetData;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Widget>>> Widget([FromQuery]WidgetModelDTO widget)
        {
            //var json = JsonSerializer.Serialize(widget);
            //if (widget.JsonValue == true)
            //{
            //    var returnjson = $"Returns the Widget data with 3 values as json: {json}";
            //    return returnjson;
            //}
            //var dejson = JsonSerializer.Deserialize<WidgetModelDTO>(json);
            //var returndejson = $"Returns the Widget data with 3 values as Object Data : User: {dejson.User},Country: {dejson.Country}, CountryId: {dejson.CountryId}";

            //return returndejson;

            try
            {
                var widgetParams = _mapper.Map<WidgetDBDTO>(widget);
                //var widget = _mapper.Map<Widget>(widget);
                var widgetDetails = await _widgetData.WidgetDBData(widgetParams);

                return Ok(widgetDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
