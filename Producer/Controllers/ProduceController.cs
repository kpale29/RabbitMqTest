using Microsoft.AspNetCore.Mvc;
using Producer.Models;
using Producer.Services.Interfaces;

namespace Producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProduceController : ControllerBase
    {
        private readonly IQueueService<ProduceModel> _queueService;
        public ProduceController(IQueueService<ProduceModel> queueService)
        {
            _queueService = queueService;
        }

        [HttpPost]
        public IActionResult post(ProduceModel model)
        {
            _queueService.Post(model);    
            return Ok();
        }


    }
}