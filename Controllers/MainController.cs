using Microsoft.AspNetCore.Mvc;
using station.meteo.family.Models;
using station.meteo.family.Services;

namespace station.meteo.family.Controllers
{
    [Route("/V1")]
    public class MainController : Controller
    {
        private DBService _DB;


        public MainController(DBService db)
        {
            _DB = db;
        }


        // POST: V1/StationID/AuthToken?___ (data posted in uri)
        [HttpPost("/{id}/{Token}")]
        public ActionResult AddUri(long id, string Token, [FromQuery] V1Uri Data)
        {
            if (_DB.StationWriteAccess(id, Token))
            {
                _DB.SaveDataToDB(id, Data);

                return Created("", Data);
            }
            else
            {
                return Unauthorized("Token and ID does not match");
            }
        }

        // POST: V1/StationID/AuthToken/Body (data posted in Body)
        [HttpPost("/{id}/{Token}/Body")]
        public ActionResult AddBody(long id, string Token, [FromBody] V1Body Data)
        {
            if (_DB.StationWriteAccess(id, Token))
            {
                _DB.SaveDataToDB(id, Data);

                return Created("", Data);
            }
            else
            {
                return Unauthorized("Token and ID does not match");
            }
        }
    }
}
