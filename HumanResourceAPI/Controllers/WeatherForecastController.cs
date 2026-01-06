//using Entities.Models;
//using HumanResourceAPI.Infrastrcuture;
//using HumanResourceAPI.Infrastrcuture.Repository;
//using Microsoft.AspNetCore.Mvc;

//namespace HumanResourceAPI.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private readonly ILoggerManager _logger;
//        private readonly IRepositoryBase<Company, Guid> _repository;
//        public WeatherForecastController(
//            ILoggerManager logger,
//            IRepositoryBase<Company, Guid> repository)
//        {
//            _logger = logger;
//            _repository = repository;
//        }
//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        //private readonly ILogger<WeatherForecastController> _logger;


//        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        //{
//        //    _logger = logger;
//        //}

//        [HttpGet(Name = "GetWeatherForecast")]
//        public IEnumerable<WeatherForecast> Get()
//        {
//            _logger.LogInfo("Fetching weather forecast data.");
//            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
//            {
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                TemperatureC = Random.Shared.Next(-20, 55),
//                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//            })
//            .ToArray();
//        }
//    }
//}
