using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unioteq.TrackNTrace.Service.Repository;

namespace Unioteq.TrackNTrace.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantController : ControllerBase
    {
        private readonly PlantRepository _plantRepository;

        public PlantController(PlantRepository plantRepository)
        {
            _plantRepository = plantRepository;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var plants = await _plantRepository.GetList();
            return Ok(plants);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(long id)
        {
            var product = await _plantRepository.GetById(id); // Properly await the task
            return Ok(product); // Return the actual result, not the Task
        }

    }
}
