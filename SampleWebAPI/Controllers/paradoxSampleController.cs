﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SampleWebAPI.Controllers
{
    public class ParadoxSampleController : ControllerBase
    {
        [HttpGet]
        [Route("paradox/{EmployeeId}")]
        public async Task<IActionResult> paradoxRoute(string EmployeeId)
        {
            try
            {
                string BaseURL = "https://apis-sandbox.paycor.com";

                RestClient Authclient = new RestClient(BaseURL + "/sts/v1/common/token?subscription-key=a47dfa90b3ab40569e4e499055bb43a8");
                var Authrequest = new RestRequest();
                //Authrequest.AddParameter("application/x-www-form-urlencoded", $"grant_type=\"refresh_token\"&refresh_token=\"c51e8d5ee1cfd2abc284ca9057efa176a10bb0ee51737cb56cb145fa144c39bf\"&client_id=\"96a567d6d8e4db7f6906\"&client_secret=\"GnnLZaFcbm3GvK6dtl5Hhjsf/yb/nEvGDd9SNIi3Q\"");
                //Authrequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                Authrequest.AddParameter("grant_type", "refresh_token");
                Authrequest.AddParameter("refresh_token", "c51e8d5ee1cfd2abc284ca9057efa176a10bb0ee51737cb56cb145fa144c39bf");
                Authrequest.AddParameter("client_id", "96a567d6d8e4db7f6906");
                Authrequest.AddParameter("client_secret", "GnnLZaFcbm3GvK6dtl5Hhjsf/yb/nEvGDd9SNIi3Q");

                var Authresponse = Authclient.Post(Authrequest);

                //var request = new RestRequest();
                //request.AddHeader("Accept", "application/json");
                //request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Authorization", "Bearer " + AuthKey);
                //request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_jrequest);

                Dictionary<string, Object> AuthContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Authresponse.Content.ToString());



                string AccessToken = "";
                if (Authresponse.Content != null && AuthContent.ContainsKey("access_token"))
                {
                    AccessToken = AuthContent["access_token"].ToString();

                    //await System.IO.File.WriteAllTextAsync("RefreshKey.txt", AuthContent["refresh_token"].ToString());
                }
                else throw new System.Exception("no token");



                RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId+ "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                                                                                            //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);
                                                                                            //var response = client.GetStringAsync(EntityId);

                var request = new RestRequest();
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + AccessToken);
                request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_jrequest);
                var response = client.Get(request);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> Content = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                string personURL= JsonConvert.DeserializeObject<Dictionary<string, Object>>(Content["person"].ToString())["url"].ToString();
                RestClient personClient = new RestClient(BaseURL + personURL)
                {
                    Options = {ThrowOnAnyError=true}
                };//"/v1/employees/"+ EmployeeId+"/emergencycontact");


                var personRequest = new RestRequest();
                //personRequest.AddHeader("Accept", "application/json");
                personRequest.AddHeader("Content-Type", "application/json");
                personRequest.AddHeader("Authorization", "Bearer " + AccessToken);
                personRequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                var personResponse =  personClient.Get(personRequest);                                                                                     //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);
                //Dictionary<string, Object> personContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(personResponse.Content.ToString());


                var paradoxURL = "https://toast-api-server/labor/v1/employees";
            

                return Ok(JsonConvert.SerializeObject(personResponse));

            }
            catch (Exception er)
            {
                return Problem(er.ToString());
            }
        }

    }

    public class ParadoxSampleModal
    {
        public string email { get; set; }
        public string phone { get; set; }
        public string job_title { get; set; }

    }
}


