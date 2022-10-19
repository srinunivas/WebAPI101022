using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SampleWebAPI.Controllers
{
    public class AspireSampleController : ControllerBase
    {
        [HttpGet]
        [Route("aspire/{EmployeeId}")]
        public async Task<IActionResult> aspireRoute(string EmployeeId)
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



                RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
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

                string personURL = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Content["person"].ToString())["url"].ToString();
                RestClient personClient = new RestClient(BaseURL + personURL);

                var personRequest = new RestRequest();
                //personRequest.AddHeader("Accept", "application/json");
                personRequest.AddHeader("Content-Type", "application/json");
                personRequest.AddHeader("Authorization", "Bearer " + AccessToken);
                personRequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                var personResponse = personClient.Get(personRequest);                                                                                     //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);

                var personContent = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(personResponse.Content.ToString());


                var aspireURL = "https://cloud-api.youraspire.com/swagger/index.html";

                AspireSampleModal aspirepayload = new AspireSampleModal();

                aspirepayload.email = Content["email"].ToString();
                aspirepayload.EmployeeNumber = Content["EmployeeNumber"].ToString();
                aspirepayload.lastName = Content["lastName"].ToString();
                aspirepayload.firstName = Content["firstName"].ToString();
                aspirepayload.TerminationDate = Content["TerminationDate"].ToString();

                if (personContent["phones"].Length > 0)
                {
                    aspirepayload.phone = personContent["phones"][0];
                }
                else
                {
                    aspirepayload.phone = "553";
                }

                RestClient aspireClient = new RestClient(aspireURL);

                var aspireRequest = new RestRequest();
                //personRequest.AddHeader("Accept", "application/json");
                aspireRequest.AddHeader("Content-Type", "application/json");
                //aspireRequest.AddHeader("Authorization", "Bearer " + AccessToken); change the token
                aspireRequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");
                aspireRequest.AddJsonBody(aspirepayload);

                //var aspireResponse = aspireClient.Post(aspireRequest);                                                                                     //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);


                return Ok(JsonConvert.SerializeObject(aspirepayload));

            }
            catch (Exception er)
            {
                return Problem(er.ToString());
            }
        }

    }

   
}

public class AspireSampleModal
{
    public string email { get; set; }
    public string phone { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string EmployeeNumber { get; set; }
    public string TerminationDate { get; set; }

}