using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.Models;
using SampleWebAPI.Utils;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SampleWebAPI.Controllers
{
    public class WIWSampleController : ControllerBase
    {
        [HttpGet]
        [Route("WIW/{EmployeeId}")]
        public async Task<IActionResult> WIWRoute(string EmployeeId)
        {
            try
            {
                string BaseURL = "https://apis-sandbox.paycor.com";
                string WIWBaseURL = "https://api.wheniwork.com";
                string WIWLoginURL = "https://api.login.wheniwork.com";

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

                PaycorHelper paycorHelper = new PaycorHelper();


                #region Commented Code

                //RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                //                                                                                             //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);
                //                                                                                             //var response = client.GetStringAsync(EntityId);

                //var request = new RestRequest();
                //request.AddHeader("Accept", "application/json");
                //request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Authorization", "Bearer " + AccessToken);
                //request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_jrequest);

                #endregion
                var response = paycorHelper.GetEmployeeData(BaseURL, EmployeeId, AccessToken);


#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                PaycorEmployeeModel paycorEmployee = JsonConvert.DeserializeObject<PaycorEmployeeModel>(response.Content.ToString());
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> Content = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                string personURL = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Content["person"].ToString())["url"].ToString();
                RestClient personClient = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "/person?include=Phones")
                {
                    Options = { ThrowOnAnyError = true }
                };

                var PersonRequest = new RestRequest();

                PersonRequest.AddHeader("Accept", "application/json");
                PersonRequest.AddHeader("Content-Type", "application/json");
                PersonRequest.AddHeader("Authorization", "Bearer " + AccessToken);
                PersonRequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                var PersonResponse = personClient.Get(PersonRequest);

                Dictionary<string, Object> PersonContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(PersonResponse.Content.ToString());

                //paycorEmployee.Phones = JsonConvert.DeserializeObject<List<PaycorPhoneNumberResponse>>(PersonContent["records"]);


                #region Payrates API

                #region Commented Code
                //RestClient Rateclient = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "/payrates");

                //var Raterequest = new RestRequest();
                //Raterequest.AddHeader("Accept", "application/json");
                //Raterequest.AddHeader("Content-Type", "application/json");
                //Raterequest.AddHeader("Authorization", "Bearer " + AccessToken);
                //Raterequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                ////request.AddJsonBody(_request);

                ////var Rateresponse = Rateclient.Get(Raterequest);

                #endregion


                var Rateresponse =paycorHelper.GetPayratesForEmployee(BaseURL, EmployeeId, AccessToken);

                Dictionary<string, Object> RateContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Rateresponse.Content.ToString());

                double Payrate = 0;
                double PayHours = 0;
                if (RateContent.ContainsKey("records"))
                {
                    List<PaycorPayRatesModel> paycorPayRates = JsonConvert.DeserializeObject<List<PaycorPayRatesModel>>(JsonConvert.SerializeObject(RateContent["records"]));

                    Payrate = Convert.ToDouble(paycorPayRates[0].payRate.ToString());
                    PayHours = Convert.ToDouble(paycorPayRates[0].annualHours.ToString()) / 52;

                    paycorEmployee.payRates = paycorPayRates[0];

                }
                #endregion

                string paycorEmployeeJson = JsonConvert.SerializeObject(paycorEmployee);

                var WIWURL = "https://apidocs.wheniwork.com/external/index.html#tag/Users/paths/~12~1users/post";

                WhenIworkSample whenIwork = new WhenIworkSample();


                //if (EmpperContent.ContainsKey("phones"))
                //{
                //    paycorPhone = JsonConvert.DeserializeObject<List<PaycorPhoneNumberResponse>>(EmpperContent["phones"].ToString());

                //    whenIwork.phone_number = paycorPhone[0].phoneNumber;
                //}



                if (Content.ContainsKey("firstName"))
                {
                    whenIwork.first_name = Content["firstName"].ToString();
                }
                if (Content.ContainsKey("lastName"))
                {
                    whenIwork.last_name = Content["lastName"].ToString();
                }
                if (Content.ContainsKey("employeeNumber"))
                {
                    whenIwork.employee_code = Convert.ToInt32(Content["employeeNumber"]);
                }
                if (RateContent.ContainsKey("email"))
                {

                    Dictionary<string, Object> EmailObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(RateContent["email"].ToString());

                    if (EmailObj.ContainsKey("emailAddress"))
                    {
                        whenIwork.email = EmailObj["emailAddress"].ToString();
                    }

                }
                whenIwork.hours_max = PayHours;
                whenIwork.hourly_rate = Payrate;




                RestClient AuthWIW = new RestClient(WIWLoginURL + "/login");

                WIWAuthModel wIWAuth = new WIWAuthModel
                {
                    email = "lavakumargreans@gmail.com",
                    password = "Paycor@123"
                };

                var WIWrequest = new RestRequest();
                //request.AddHeader("Accept", "application/json");
                WIWrequest.AddHeader("Content-Type", "application/json");
                WIWrequest.AddHeader("W-Key", "873d073ca440a7378f86fa0c32dff639378deaa0");
                WIWrequest.AddJsonBody(wIWAuth);


                var WIWAuthresponse = AuthWIW.Post(WIWrequest);

                var WIWAuthContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(WIWAuthresponse.Content.ToString());

                string WIWApiToken = "";

                if (WIWAuthContent.ContainsKey("token"))
                {
                    WIWApiToken = WIWAuthContent["token"].ToString();
                }

                #region WIW Post

                RestClient PostWIW = new RestClient(WIWBaseURL + "/2/users");


                var WIWPostrequest = new RestRequest();
                //request.AddHeader("Accept", "application/json");
                WIWPostrequest.AddHeader("Content-Type", "application/json");
                WIWPostrequest.AddHeader("W-Token", WIWApiToken);
                WIWPostrequest.AddJsonBody(whenIwork);

                string WIWPostBody = JsonConvert.SerializeObject(whenIwork);

                var WIWPostresponse = PostWIW.Post(WIWPostrequest);
                var WIWPostcontent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(WIWPostresponse.Content.ToString());

                #endregion


                return Ok(JsonConvert.SerializeObject(paycorEmployee));

            }
            catch (Exception er)
            {
                return Problem(er.ToString());
            }
        }

    }


}
