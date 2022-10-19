using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace SampleWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
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

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Object request)
        {

            try
            {
                Dictionary<string, Object> _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(request.ToString());

                var response = new Dictionary<string, Object>();
                //string FirstName=  _request.ContainsKey("FirstName")) ?_request["FirstName"].ToString();

                string Var1 = String.Empty;
                dynamic Var2 = new Dictionary<string, Object>();
                string Var3 = String.Empty;

                if (_request.ContainsKey("FirstName"))
                {
                    var FirstNameType = _request["FirstName"].GetType();

                    if (_request["FirstName"].GetType() == typeof(string))
                    {
                        Var1 = _request["FirstName"].ToString();
                    }

                }
                if (_request.ContainsKey("LastName"))
                {
                    var FirstNameType = _request["LastName"].GetType();

                    if (_request["LastName"].GetType() == typeof(string))
                    {
                        Var1 = Var1 + _request["LastName"].ToString();
                    }

                }

                if (_request.ContainsKey("Address"))
                {
                    var FirstNameType = _request["Address"].GetType();

                    if (_request["Address"].GetType() == typeof(JObject) || _request["Address"].GetType() == typeof(JArray))
                    {
                        Var2 = FirstNameType.Name == "JObject" ? JsonConvert.DeserializeObject<Dictionary<string, Object>>(_request["Address"].ToString()) : FirstNameType.Name == "JArray" ? JsonConvert.DeserializeObject<JArray>(_request["Address"].ToString()) : null;
                        //Var2 = _request["Address"];
                        //Var2 = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_request["Address"].ToString());

                        if (_request["Address"].GetType() == typeof(JArray))
                        {
                            List<Dictionary<string, Object>> AddressList = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(_request["Address"].ToString());
                            foreach (Dictionary<string, object>? AddressObj in AddressList)
                            {
                                var Type = AddressObj.ContainsKey("Type") ? AddressObj["Type"].ToString() : null;
                                if (Type != null && Type == "Home")
                                {
                                    if (AddressObj.ContainsKey("Address1"))
                                    {
                                        //var FirstNameType = Var2["Address1"].GetType();

                                        if (AddressObj["Address1"].GetType() == typeof(string))
                                        {
                                            Var3 = AddressObj["Address1"].ToString();
                                        }

                                    }

                                    if (AddressObj.ContainsKey("Address2"))
                                    {
                                        //var FirstNameType = Var2["Address2"].GetType();

                                        if (AddressObj["Address2"].GetType() == typeof(string))
                                        {
                                            Var3 = Var3 + AddressObj["Address2"].ToString();
                                        }

                                        response.Add("Address", Var3);
                                    }

                                }
                            }
                        }
                        else
                        {
                            if (Var2.ContainsKey("Address1"))
                            {
                                //var FirstNameType = Var2["Address1"].GetType();

                                if (Var2["Address1"].GetType() == typeof(string))
                                {
                                    Var3 = Var2["Address1"].ToString();
                                }

                            }

                            if (Var2.ContainsKey("Address2"))
                            {
                                //var FirstNameType = Var2["Address2"].GetType();

                                if (Var2["Address2"].GetType() == typeof(string))
                                {
                                    Var3 = Var3 + Var2["Address2"].ToString();
                                }

                            }
                            response.Add("Address", Var3);
                        }
                    }


                    //else if(_request["Address"].GetType() == typeof(List<JObject>))
                    //{
                    //    List<Dictionary<string,Object>> AddressList = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(_request["Address"].ToString());
                    //    //foreach(Dictionary<string, object>? AddressObj in AddressList)
                    //    //{
                    //    //    if()
                    //    //}
                    //}


                }

            FinalStep:


                response.Add("Name", Var1);


                //Response
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }

    public class StudentInfo
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public int LastName { get; set; }
        public string CustomerId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}