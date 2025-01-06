using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Report_A_Crime.Models.Dtos;
using Report_A_Crime.Models.Services.Interface;

namespace Report_A_Crime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocationController : ControllerBase
    {
        private readonly IGeolocationService _geolocationService;

        public GeolocationController(IGeolocationService geolocationService)
        {
            _geolocationService = geolocationService;
        }

        [HttpPost("Create Geolocation")]
        public async Task<IActionResult> Geolocation([FromForm] GeolocationRequestModel requestModel, [FromQuery] Guid reportId)
        {
            if(requestModel == null || reportId == Guid.Empty)
            {
                return BadRequest(new
                {
                    Message = "Invalid request"
                });
            }
            try
            {

                var geolocation = await _geolocationService.CreateGeolocationAsync(requestModel, reportId);
                if(!geolocation.Status)
                {
                    return BadRequest(new
                    {
                        Message = geolocation.Message,
                        Status = geolocation.Status,
                    });
                }
                return Ok(new
                {
                    Data = geolocation,
                    Status = true,
                    Message = geolocation.Message
                });
            }

            catch (System.Exception ex)
            {
                return StatusCode(500, new 
                { 
                    Message = "An error occurred while creating geolocation.",
                    Error = ex.Message 
                });
            }
        }
    }
}
