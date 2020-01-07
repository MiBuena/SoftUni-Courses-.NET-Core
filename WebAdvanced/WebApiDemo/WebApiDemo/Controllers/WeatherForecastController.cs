using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers
{
    public class WeatherForecastController : BaseApiController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetSettings")]
        public ActionResult<string> GetSettings()
        {
            return null;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetOneForecast/{id}")]
        public ActionResult<IEnumerable<WeatherForecast>> GetOneForecast()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("GetBall")]
        public ActionResult<Ball> GetBall()
        {
            var ball = new Ball()
            {
                Weight = 2
            };

            return ball;
        }


        [HttpGet("GetFish/{year:int}")]
        public ActionResult<Ball> GetFish(int year)
        {
            var ball = new Ball()
            {
                Weight = 2
            };

            //return ball;

            return this.BadRequest();
        }

        [HttpPut("{id}")]
        public void GetPutFish(int id, [FromBody]string data)
        {

        }
    }
}
