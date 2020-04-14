using System.Collections.Generic;
using System.Threading.Tasks;

using AdvertiseApi.Models;
using AdvertiseApi.Services;

using Microsoft.AspNetCore.Mvc;

namespace AdvertiseApi.Controllers
{
    [Route("api/v1/Advertise")]
    [ApiController]
    public class AdvertiseController : ControllerBase
    {
        private readonly IAdvertiseStorageService _storageService;

        public AdvertiseController(IAdvertiseStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost]
        [Route("Create")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Create))]
        public async Task<IActionResult> Create(AdvertiseModel model)
        {
            try
            {
                var id = await _storageService.AddAsync(model);
                return Created("", new CreatedAdvertiseResponse { Id = id });
            }
            catch(System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("Confirm")]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Update))]
        public async Task<IActionResult> Confirm(ConfirmAdvertiseModel model)
        {
            try
            {
                await _storageService.ConfirmAsync(model);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}