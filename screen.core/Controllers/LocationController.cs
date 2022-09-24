using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Screen.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Screen.Core.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {

        private readonly ILogger<LocationController> _logger;
        private ILocationService _locationService;
        public LocationController(ILogger<LocationController> logger, ILocationService locationService)
        {
            _logger = logger;
            _locationService = locationService;
        }


        [Route("/api/Publish_ZJK_Data")]
        [HttpPost]
        public string Publish_ZJK_Data()
        {
            this._locationService.Publish_ZJK_Data();
            return "";
        }


        [Route("/api/Publish_FJK_Data")]
        [HttpPost]
        public string Publish_FJK_Data()
        {
            this._locationService.Publish_FJK_Data();
            return "";
        }


        [Route("/api/Publish_Other_Data")]
        [HttpPost]
        public string Publish_Other_Data()
        {
            this._locationService.Publish_EquTask_Data();
            this._locationService.Publish_TaskItem_Data();
            this._locationService.Pulish_LaneWayEmpty_Data();
            return "";
        }
    }
}
