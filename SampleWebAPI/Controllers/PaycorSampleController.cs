using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using SampleWebAPI.Models;
using Newtonsoft;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SampleWebAPI.Controllers
{
    public class PaycorSampleController : ControllerBase
    {

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("LegalEntity")]

        public async Task<IActionResult> GetLegalEntity(string EntityId, string Token)
        {
            try
            {

                string BaseURL = "https://apis-sandbox.paycor.com";



                RestClient client = new RestClient(BaseURL+"/v1/legalentities/" + EntityId + "");
                //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);
                //var response = client.GetStringAsync(EntityId);

                var request = new RestRequest();
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + Token);
                request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");
                var response = client.Get(request);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> Content = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                string COntent = JsonConvert.SerializeObject(Content);

                //StreamWriter sw = new StreamWriter("LegalEntityGet.txt", append: true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                //await sw.WriteLineAsync(JsonConvert.SerializeObject(Content));
                await System.IO.File.WriteAllTextAsync("LegalEntityGet.txt", COntent);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                //File.WriteAllTextAsync("WriteText.txt", text);
                return Ok(Content);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }


       
        [HttpPost]
        [Route("EmergencyContact")]

        public async Task<IActionResult> AddEmergencyContact(string EmployeeId, [FromBody]Object _jrequest)
        {
            try
            {

                Dictionary<string, Object> _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_jrequest.ToString());

                string BaseURL = "https://apis-sandbox.paycor.com";



                RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);
                //var response = client.GetStringAsync(EntityId);

                var request = new RestRequest();
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJSUzI1NiIsImtpZCI6IjYxNGRlODEzZTgxMjRhNWEyYjgxYTRlY2VjMGRiNzQxIiwidHlwIjoiSldUIn0.eyJuYmYiOjE2NjMyNTIzODUsImV4cCI6MTY2MzI1NDE4NSwiaXNzIjoiaHR0cHM6Ly9hcGktZGVtby5wYXljb3IuY29tL3N0cy92Mi9jb21tb24iLCJhdWQiOiI5NmE1NjdkNmQ4ZTRkYjdmNjkwNiIsImNsaWVudF9pZCI6Ijk2YTU2N2Q2ZDhlNGRiN2Y2OTA2Iiwic2lkIjoiYmJmNGJjZTktZjA4ZC00NDBiLThkODgtMjAyMjYyMGY1OTVhIiwiYXV0aF90aW1lIjoxNjYzMjUyMjc3LCJ2ZXIiOjEsInN1YiI6IjFlMmUyZDg2LWRlNmItNDVmYy04Y2Q4LTc5MjY1NzFmMjdkOCIsImlkcCI6MywicGF5Y29yX3VzZXJpZGVudGl0eSI6IjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCIsInBheWNvcl90aW1lb3V0IjoyNDAsInBheWNvcl9idiI6InVXSk5LU2Vuc0V1Z2g4cnViWmN4YnciLCJwYXljb3JfcHJpdmlsZWdlcyI6Ikg0c0lBQUFBQUFBQUNsMVdzWkpqTVFqN2w5UXViSXdmOW5ZN08xdnY5amYzSlpuOCswbnZta2lGSFJRd3lHQkluby9QejYvSHg1K29YUDM2MndCL0FHL2hGOExvbzQwK3NVNGJvMk1sMXRWRzRQdUFIQXVMdUxCZ00yRXpvWnVCQmYyRVBpRW5mQ1RzVm0rQmMzTmg3ZDdtQ1N6SVo3WHN1eVZpSkdKa25wWUZmTGhPVzRpL3hteHI3YloydFQydmRoSysrMFRRbm9qZUwyNkZtSDFUc1RjMzBpYkhNUUVIalc4cU54ZVN3VWFwS0JWNGo2SzI0R3BzU3JjREVNUkdMNGNLY0JyQnhNVC9UUERxZzk4TkdFZkFWVEFWd1Z3RWt4SE1Sa3o0QytZa21KU1lQRVpXa2JUTE82RzBJNzhnditDMUFwbkFEV2l5cUYzVXJqdnJsSzVib3BlaVZEeDJhSHpvRDluRlJqdGVZZTVBWVZIdTV3c2YvSHlndnF6K3FWb2RPcVJUNFJFNHVzSlVlQWtNOVJ4cUhFdWhuUzJGU21NcWphbUJaaWpVdUZQanBocW5YaitWMVpLNGVNYnZFQzlhNEJaanZIT0ZhbnlFRlJwQm9LWWQ3U0V3SlRsb0dvSEhvQmlqcXdRT3VUNTZUZUNXb3FBRDN5R2EwUjZTMWd3dHF2Z3lYRm9uOUxCaEljUE8xc2RuRHdxUDNiRFpXNm05MWxac2pnbkZaZnJTVjRnNVlsanZpK2xpMlBqcEMrSDRNV3ordE5DY1Q5YU4zcDNXWXNQT0QrV0RzV2JZOU5hVW1IdUd0ZE13RFEzYmZMQkd4YmcwYkh6VDRxZVBINHR2OWNmQU5Xenh0TTA0a1IrS3pmOHkvOHNIbnVrdjE5djlMb3RmWmwvRy94aS9ZL25RZ2NSZkNjTTJOL2ViUFg0K3Z2SHY0UGw2L1FNUGVJOU1RQWdBQUE9PSIsInBheWNvcl96aXAiOiJwYXljb3JfcHJpdmlsZWdlcyIsIm5hbWUiOiJUZXN0IEFwcCAyIEludGVncmF0aW9uIiwiZ2l2ZW5fbmFtZSI6IlRlc3QgQXBwIDIiLCJmYW1pbHlfbmFtZSI6IkludGVncmF0aW9uIiwiZW1haWwiOiJJVC1JbnRlZ3JhdGlvbnNTdXBwb3J0RExAcGF5Y29yLmNvbSIsImlwYWRkciI6IjEwNi41MS4yNS4xODEiLCJqdGkiOiIwZjdjOTVmZGE1NDEzOTMxM2M1NWEyOGIwYzc0Yzg0ZSIsInNjb3BlIjpbImMxY2QwMTBhMGYzMGVkMTFhZTgzMjgxODc4MTk1MWE0Iiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdLCJwYXljb3JfY2xpZW50cyI6eyJDIjpbeyJDbGllbnRJZCI6Mjc0NTA2LCJDb21wYW55SWQiOjI5Nzc1MH1dfX0.GhARUaZUA3-v-zVNrmv8mbiM7qKlKoJ6IlQ7_PxD6RMuHEZjWWfW0DP9mutF8UH5bRx3kJRcRPW1QTXqh1nNO0JbntsrWE2PruryC0cFF6vfO3zw6d7JFTmdBlfGCs7lcE-noMohIJ1Yesu5aWRVW3WHaf4C8CPqtiTIA77pbSQizFDiBboJLX6B0YxM2-7M03ju0yFDpLHrmKwl7QMKxdIA0BS-ww_Tpf37hLOWqKjckE8lIHUxPc5dGyzGBxWFS_qF5z9b_XFwYEiADR3jIDTK9-jpocZIaE0XygvfXojjd6fdZ0Tm6n3NufIcxUj2V8kfFx_BMkChS-xpcDHIDw");
                request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_jrequest);
                var response = client.Get(request);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> Content = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                ParadoxSample paradox = new();
                if (Content.ContainsKey("id"))
                {
                    paradox.ex_Id = Content["id"].ToString();
                }
                if (Content.ContainsKey("firstName"))
                {
                    paradox.first_name = Content["firstName"].ToString();
                }
                if (Content.ContainsKey("lastName"))
                {
                    paradox.last_name = Content["lastName"].ToString();
                }
                if (Content.ContainsKey("email"))
                {

                    Dictionary<string, Object> EmailObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Content["email"].ToString());

                    if (EmailObj.ContainsKey("emailAddress"))
                    {
                        paradox.email = EmailObj["emailAddress"].ToString();
                    }

                    
                }

                if (Content.ContainsKey("positionData"))
                {

                    Dictionary<string, Object> PositionObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Content["positionData"].ToString());

                    if (PositionObj.ContainsKey("jobTitle"))
                    {
                        paradox.job_title = PositionObj["jobTitle"].ToString();
                    }


                }


                //string COntent = JsonConvert.SerializeObject(Content);
                string COntent = JsonConvert.SerializeObject(paradox);

                //StreamWriter sw = new StreamWriter("LegalEntityGet.txt", append: true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                //await sw.WriteLineAsync(JsonConvert.SerializeObject(Content));
                await System.IO.File.WriteAllTextAsync("EmployeeEmergencyContact.txt", COntent);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                //File.WriteAllTextAsync("WriteText.txt", text);
                return Ok(JsonConvert.SerializeObject(Content));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("paradoxSample")]

        public async Task<IActionResult> paradoxSample(string EmployeeId, string AuthKey, [FromBody] Object _jrequest)
        {
            try
            {


               

                Dictionary<string, Object> _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_jrequest.ToString());

                Dictionary<string,Object> Data = new Dictionary<string,Object>();
                Dictionary<string, Object> Content = new Dictionary<string, Object>();

                if (_request.ContainsKey("Data"))
                {
                    Data = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_request["Data"].ToString());


                    string BaseURL = "https://apis-sandbox.paycor.com";


                    #region Commented Code

                    #region Generate access token

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

                    #endregion

                    string AccessToken = "";
                    if(Authresponse.Content != null && AuthContent.ContainsKey("access_token"))
                    {
                        AccessToken = AuthContent["access_token"].ToString();

                        //await System.IO.File.WriteAllTextAsync("RefreshKey.txt", AuthContent["refresh_token"].ToString());
                    }

                    RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                    //var request = new RestRequest();
                    //var response = client.GetAsync(request);

                    var request = new RestRequest();
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "Bearer " + AccessToken);
                    request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                    request.AddJsonBody(_jrequest);
                    
                    var response = client.Get(request);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    //Dictionary<string, Object> Content = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    //List<JsonMapper> MapperList = new List<JsonMapper>();

                    //JsonMapper mapper = new JsonMapper();

                    //mapper.Source = "id";
                    //mapper.Target = "ex_Id";
                    //MapperList.Add(mapper);

                    //mapper = new JsonMapper();
                    //mapper.Source = "firstName";
                    //mapper.Target = "first_name";
                    //MapperList.Add(mapper);


                    //MapperList.Add(mapper);

                    #region Paradox Hardcoding

                    #endregion

                    ParadoxSample paradox = new();


                    #endregion

                    #region Paradox dynamic code

                    string tempSource;
                    string temptarget;

                    //List<Dictionary<string, Object>> Paradoxdict = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(JsonConvert.SerializeObject(paradox));

                    if (_request.ContainsKey("Mappings"))
                    {
                        List<JsonMapper> Paradoxdict = JsonConvert.DeserializeObject<List<JsonMapper>>(JsonConvert.SerializeObject(_request["Mappings"]));


                        foreach (var Obj in Paradoxdict)
                        {
                            //Dictionary<string, Object> DictObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(paradox.ToString());

                            //if (Content.ContainsKey(Obj.Source))
                            {
                                tempSource = Obj.Source;

                                Content.Add(Obj.Target, Data[Obj.Source]);

                                //Paradoxdict[Obj.Target]=Content[Obj.Source].ToString();
                            }
                        }
                    }
                    #endregion
                }

                //string COntent = JsonConvert.SerializeObject(Content);
                string COntent = JsonConvert.SerializeObject(Content);

                //StreamWriter sw = new StreamWriter("LegalEntityGet.txt", append: true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                //await sw.WriteLineAsync(JsonConvert.SerializeObject(Content));
                await System.IO.File.WriteAllTextAsync("EmployeeEmergencyContact.txt", COntent);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                //File.WriteAllTextAsync("WriteText.txt", text);
                return Ok(JsonConvert.SerializeObject(Content));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPost]
        [Route("MapperSample")]

        public async Task<IActionResult> MapperSample(string EmployeeId, string AuthKey/*, [FromBody] Object _jrequest*/)
        {
            try
            {

                Dictionary<string, Object> _request = new Dictionary<string, object>();

                string RefreshToken = "";

                //if (_jrequest == null)
                //{
                    //string MapperData = File.ReadAllText(@"C:\mapper.json");

                    using (StreamReader r = new StreamReader(@"C:\Users\lavak\RefreshKey.txt")) 
                    {
                        RefreshToken = r.ReadToEnd();
                    }

                    using (StreamReader r = new StreamReader(@"C:\Users\lavak\mapper.json"))
                    {

                        string MapperData = r.ReadToEnd();
                        _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(MapperData);
                    }
                    
                //}
                //else
                //{
                //    _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_jrequest.ToString());

                //}
                Dictionary<string, Object> Data = new Dictionary<string, Object>();
                Dictionary<string, Object> Content = new Dictionary<string, Object>();

                

                    string BaseURL = "https://apis-sandbox.paycor.com";


                    #region Commented Code

                    #region Generate access token

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

                    #endregion

                    string AccessToken = "";
                    if (Authresponse.Content != null && AuthContent.ContainsKey("access_token"))
                    {
                        AccessToken = AuthContent["access_token"].ToString();

                        await System.IO.File.WriteAllTextAsync("RefreshKey.txt", AuthContent["refresh_token"].ToString());
                    }

                    RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                    //var request = new RestRequest();
                    //var response = client.GetAsync(request);

                    var request = new RestRequest();
                    request.AddHeader("Accept", "application/json");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("Authorization", "Bearer " + AccessToken);
                    request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                    //request.AddJsonBody(_request);

                    var response = client.Get(request);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    Dictionary<string, Object> PayContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    //List<JsonMapper> MapperList = new List<JsonMapper>();

                    //JsonMapper mapper = new JsonMapper();

                    //mapper.Source = "id";
                    //mapper.Target = "ex_Id";
                    //MapperList.Add(mapper);

                    //mapper = new JsonMapper();
                    //mapper.Source = "firstName";
                    //mapper.Target = "first_name";
                    //MapperList.Add(mapper);


                    //MapperList.Add(mapper);

                    #region Paradox Hardcoding

                    #endregion

                    ParadoxSample paradox = new();


                    #endregion

                    #region Paradox dynamic code

                    string tempSource;
                    string temptarget;

                    //List<Dictionary<string, Object>> Paradoxdict = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(JsonConvert.SerializeObject(paradox));

                    if (_request.ContainsKey("Mappings"))
                    {
                        List<JsonMapper> Paradoxdict = JsonConvert.DeserializeObject<List<JsonMapper>>(JsonConvert.SerializeObject(_request["Mappings"]));


                        foreach (var Obj in Paradoxdict)
                        {
                            //Dictionary<string, Object> DictObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(paradox.ToString());

                            //if (Content.ContainsKey(Obj.Source))
                            {
                                tempSource = Obj.Source;

                                Content.Add(Obj.Target, PayContent[Obj.Source]);

                                //Paradoxdict[Obj.Target]=Content[Obj.Source].ToString();
                            }
                        }
                    }
                    #endregion
                

                //string COntent = JsonConvert.SerializeObject(Content);
                string COntent = JsonConvert.SerializeObject(Content);

                //StreamWriter sw = new StreamWriter("LegalEntityGet.txt", append: true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                //await sw.WriteLineAsync(JsonConvert.SerializeObject(Content));
                await System.IO.File.WriteAllTextAsync("EmployeeEmergencyContact.txt", COntent);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                //File.WriteAllTextAsync("WriteText.txt", text);
                return Ok(JsonConvert.SerializeObject(Content));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("MapperObjSample")]

        public async Task<IActionResult> MapperObjSample(string EmployeeId/*, [FromBody] Object _jrequest*/)
        {
            try
            {

                Dictionary<string, Object> _request = new Dictionary<string, object>();

                string RefreshToken = "";

                //if (_jrequest == null)
                //{
                //string MapperData = File.ReadAllText(@"C:\mapper.json");

                using (StreamReader r = new StreamReader(@"C:\Users\lavak\RefreshKey.txt"))
                {
                    RefreshToken = r.ReadToEnd();
                }

                using (StreamReader r = new StreamReader(@"C:\Users\lavak\WIWDataModel.json"))
                {

                    string MapperData = r.ReadToEnd();
                    _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(MapperData);
                }

                //}
                //else
                //{
                //    _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_jrequest.ToString());

                //}

                //foreach(var jKey in _request)
                //{

                //}
                Dictionary<string, Object> Data = new Dictionary<string, Object>();
                Dictionary<string, Object> Content = new Dictionary<string, Object>();



                string BaseURL = "https://apis-sandbox.paycor.com";
                string WIWBaseURL = "https://api.wheniwork.com";
                string WIWLoginURL = "https://api.login.wheniwork.com";


                #region Commented Code

                #region Generate access token

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

                #endregion

                string AccessToken = "";
                if (Authresponse.Content != null && AuthContent.ContainsKey("access_token"))
                {
                    AccessToken = AuthContent["access_token"].ToString();

                    await System.IO.File.WriteAllTextAsync("RefreshKey.txt", AuthContent["refresh_token"].ToString());
                }

                RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?/person?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                                                                                                             //var request = new RestRequest();
                                                                                                             //var response = client.GetAsync(request);

                var request = new RestRequest();
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + AccessToken);
                request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_request);

                var response = client.Get(request);

                

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> PayContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.


                #region Payrates API

                RestClient Rateclient = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "/payrates");

                var Raterequest = new RestRequest();
                Raterequest.AddHeader("Accept", "application/json");
                Raterequest.AddHeader("Content-Type", "application/json");
                Raterequest.AddHeader("Authorization", "Bearer " + AccessToken);
                Raterequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_request);

                var Rateresponse = Rateclient.Get(Raterequest);

                Dictionary<string, Object> RateContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Rateresponse.Content.ToString());

                double Payrate = 0;
                double PayHours = 0;
                if (RateContent.ContainsKey("records"))
                {
                    List<PaycorPayRatesModel> paycorPayRates = JsonConvert.DeserializeObject<List<PaycorPayRatesModel>>(JsonConvert.SerializeObject(RateContent["records"]));

                    Payrate = Convert.ToDouble(paycorPayRates[0].payRate.ToString());
                    PayHours = Convert.ToDouble(paycorPayRates[0].annualHours.ToString()) / 52;

                }
                #endregion

                #region Employee Get

                RestClient Empclient = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                
                var Emprequest = new RestRequest();
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + AccessToken);
                request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_request);

                var Empresponse = Empclient.Get(request);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> EmpPayContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Empresponse.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                #endregion

                #region Employee Person Get

                RestClient EmpPerclient = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "/person?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");

                var EmpPerrequest = new RestRequest();
                EmpPerrequest.AddHeader("Accept", "application/json");
                EmpPerrequest.AddHeader("Content-Type", "application/json");
                EmpPerrequest.AddHeader("Authorization", "Bearer " + AccessToken);
                EmpPerrequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_request);

                var Empperresponse = EmpPerclient.Get(EmpPerrequest);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> EmpperContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Empperresponse.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                #endregion


                #endregion

                #region Paradox dynamic code

                string tempSource;
                string temptarget;


                Dictionary<string, Object> Paradoxdict = JsonConvert.DeserializeObject<Dictionary<string,Object>>(JsonConvert.SerializeObject(_request));

                JObject keyValuePairs = JObject.Parse(JsonConvert.SerializeObject(_request));

                

                List<PaycorPhoneNumberResponse> paycorPhone = new List<PaycorPhoneNumberResponse>();

                WhenIworkSample whenIwork = new WhenIworkSample();

                
                if (EmpperContent.ContainsKey("phones"))
                {
                    paycorPhone = JsonConvert.DeserializeObject<List<PaycorPhoneNumberResponse>>(EmpperContent["phones"].ToString());

                    whenIwork.phone_number = paycorPhone[0].phoneNumber;
                }

                
                    
                if (PayContent.ContainsKey("firstName"))
                {
                    whenIwork.first_name = PayContent["firstName"].ToString();
                }
                if (PayContent.ContainsKey("lastName"))
                {
                    whenIwork.last_name = PayContent["lastName"].ToString();
                }
                if (EmpPayContent.ContainsKey("employeeNumber"))
                {
                    whenIwork.employee_code = Convert.ToInt32(EmpPayContent["employeeNumber"]);
                }
                if (PayContent.ContainsKey("email"))
                {

                    Dictionary<string, Object> EmailObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(PayContent["email"].ToString());

                    if (EmailObj.ContainsKey("emailAddress"))
                    {
                        whenIwork.email = EmailObj["emailAddress"].ToString();
                    }

                }
                whenIwork.hours_max = PayHours;
                whenIwork.hourly_rate = Payrate;



                #endregion
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

                await System.IO.File.WriteAllTextAsync(@"C:\Users\lavak\WIWPost.txt", WIWPostresponse.Content.ToString());

                #endregion


                //string COntent = JsonConvert.SerializeObject(Content);
                string COntent = JsonConvert.SerializeObject(WIWPostcontent);

                //StreamWriter sw = new StreamWriter("LegalEntityGet.txt", append: true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                //await sw.WriteLineAsync(JsonConvert.SerializeObject(Content));
                await System.IO.File.WriteAllTextAsync("EmployeeEmergencyContact.txt", COntent);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                //File.WriteAllTextAsync("WriteText.txt", text);
                return Ok(JsonConvert.SerializeObject(COntent));
            }
            catch (Exception ex)
            {
                return Problem(ex.ToString());
            }
        }


        [HttpPost]
        [Route("WhenIWorkSample")]

        public async Task<IActionResult> WhenIWork([FromBody] Object _jrequest)
        {
            try
            {

                Dictionary<string, Object> _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_jrequest.ToString());

                string BaseURL = "https://apis-sandbox.paycor.com";

                //if(_request.ContainsKey())

                //RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                RestClient client = new RestClient(BaseURL + "/v1/employees");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                //var request= new HttpRequestMessage(HttpMethod.Get, EntityId);
                //var response = client.GetStringAsync(EntityId);

                var request = new RestRequest();
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + "eyJhbGciOiJSUzI1NiIsImtpZCI6IjYxNGRlODEzZTgxMjRhNWEyYjgxYTRlY2VjMGRiNzQxIiwidHlwIjoiSldUIn0.eyJuYmYiOjE2NjM4MzgxNTIsImV4cCI6MTY2MzgzOTk1MiwiaXNzIjoiaHR0cHM6Ly9hcGktZGVtby5wYXljb3IuY29tL3N0cy92Mi9jb21tb24iLCJhdWQiOiI5NmE1NjdkNmQ4ZTRkYjdmNjkwNiIsImNsaWVudF9pZCI6Ijk2YTU2N2Q2ZDhlNGRiN2Y2OTA2Iiwic2lkIjoiZmFjMTQ4NTEtYmUwZi00YTQ5LWE2ZWEtMDc3NzM2ZTQ1ZTM1IiwiYXV0aF90aW1lIjoxNjYzODM4MDY3LCJ2ZXIiOjEsInN1YiI6IjFlMmUyZDg2LWRlNmItNDVmYy04Y2Q4LTc5MjY1NzFmMjdkOCIsImlkcCI6MywicGF5Y29yX3VzZXJpZGVudGl0eSI6IjAwMDAwMDAwLTAwMDAtMDAwMC0wMDAwLTAwMDAwMDAwMDAwMCIsInBheWNvcl90aW1lb3V0IjoyNDAsInBheWNvcl9idiI6InVXSk5LU2Vuc0V1Z2g4cnViWmN4YnciLCJwYXljb3JfcHJpdmlsZWdlcyI6Ikg0c0lBQUFBQUFBQUNsMVdzWkpqTVFqN2w5UXViSXdmOW5ZN08xdnY5amYzSlpuOCswbnZta2lGSFJRd3lHQkluby9QejYvSHg1K29YUDM2MndCL0FHL2hGOExvbzQwK3NVNGJvMk1sMXRWRzRQdUFIQXVMdUxCZ00yRXpvWnVCQmYyRVBpRW5mQ1RzVm0rQmMzTmg3ZDdtQ1N6SVo3WHN1eVZpSkdKa25wWUZmTGhPVzRpL3hteHI3YloydFQydmRoSysrMFRRbm9qZUwyNkZtSDFUc1RjMzBpYkhNUUVIalc4cU54ZVN3VWFwS0JWNGo2SzI0R3BzU3JjREVNUkdMNGNLY0JyQnhNVC9UUERxZzk4TkdFZkFWVEFWd1Z3RWt4SE1Sa3o0QytZa21KU1lQRVpXa2JUTE82RzBJNzhnditDMUFwbkFEV2l5cUYzVXJqdnJsSzVib3BlaVZEeDJhSHpvRDluRlJqdGVZZTVBWVZIdTV3c2YvSHlndnF6K3FWb2RPcVJUNFJFNHVzSlVlQWtNOVJ4cUhFdWhuUzJGU21NcWphbUJaaWpVdUZQanBocW5YaitWMVpLNGVNYnZFQzlhNEJaanZIT0ZhbnlFRlJwQm9LWWQ3U0V3SlRsb0dvSEhvQmlqcXdRT3VUNTZUZUNXb3FBRDN5R2EwUjZTMWd3dHF2Z3lYRm9uOUxCaEljUE8xc2RuRHdxUDNiRFpXNm05MWxac2pnbkZaZnJTVjRnNVlsanZpK2xpMlBqcEMrSDRNV3ordE5DY1Q5YU4zcDNXWXNQT0QrV0RzV2JZOU5hVW1IdUd0ZE13RFEzYmZMQkd4YmcwYkh6VDRxZVBINHR2OWNmQU5Xenh0TTA0a1IrS3pmOHkvOHNIbnVrdjE5djlMb3RmWmwvRy94aS9ZL25RZ2NSZkNjTTJOL2ViUFg0K3Z2SHY0UGw2L1FNUGVJOU1RQWdBQUE9PSIsInBheWNvcl96aXAiOiJwYXljb3JfcHJpdmlsZWdlcyIsIm5hbWUiOiJUZXN0IEFwcCAyIEludGVncmF0aW9uIiwiZ2l2ZW5fbmFtZSI6IlRlc3QgQXBwIDIiLCJmYW1pbHlfbmFtZSI6IkludGVncmF0aW9uIiwiZW1haWwiOiJJVC1JbnRlZ3JhdGlvbnNTdXBwb3J0RExAcGF5Y29yLmNvbSIsImlwYWRkciI6IjEyNC4xMjMuMTA2LjIxMiIsImp0aSI6ImU2Njk5YzVhMGNkMDBkMDE1ZWI4OGU5YmFiNjliODNkIiwic2NvcGUiOlsiYzFjZDAxMGEwZjMwZWQxMWFlODMyODE4NzgxOTUxYTQiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIl0sInBheWNvcl9jbGllbnRzIjp7IkMiOlt7IkNsaWVudElkIjoyNzQ1MDYsIkNvbXBhbnlJZCI6Mjk3NzUwfV19fQ.Kd3FiU6_nlsRmaz4dvFYCnDei402FAnCjoFxdh49obIiOQxiiPXqccXhCog7whwmarAa9msOKzRzQlsxk-sgm9ngbFoSzUZ0St0WUhcel_8MUZR_Uu_8YKjoiQkjFMcrDR34cWEAANRJjUP_pqEoOoUE9g7KUaJpJhQMDGDErVWoGYBXLaLZPIehk6tr9CrcPO8_uLuC55rS3Q1jYkK4KY4RUtks5O0XfOyPAZB924WQ0J6zXeJxlfkLLh6VQg3M5SlqjMUNvx6AdsFNGvczq_3oA4JSnTQbSWZnTeniCw1dqBLVlaZc9mVg8Tf1DyLgcnuGL5Wtbf04bCIKDrvxsw");
                request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                request.AddJsonBody(_jrequest);
                var response = client.Post(request);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> Content = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {



                    List<JsonMapper> MapperList = new List<JsonMapper>();

                    JsonMapper mapper = new JsonMapper();
                   
                    #region WhenIwork Hardcoding

                    WhenIworkSample whenIwork = new WhenIworkSample();


                    #endregion

                    #region Paradox dynamic code

                    string tempSource;
                    string temptarget;

                    Dictionary<string, Object> WhenIworkdict = JsonConvert.DeserializeObject<Dictionary<string, Object>>(JsonConvert.SerializeObject(_request));

                    whenIwork.employee_code = Convert.ToInt32(_request["EmployeeNumber"]);
                    whenIwork.first_name = _request["FirstName"].ToString();
                    whenIwork.last_name = _request["LastName"].ToString();
                    whenIwork.email = _request["WorkEmail"].ToString();
                    



                    #endregion

                    //string COntent = JsonConvert.SerializeObject(Content);
                    string COntent = JsonConvert.SerializeObject(WhenIworkdict);

                    //StreamWriter sw = new StreamWriter("LegalEntityGet.txt", append: true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    //await sw.WriteLineAsync(JsonConvert.SerializeObject(Content));
                    await System.IO.File.WriteAllTextAsync("EmployeeEmergencyContact.txt", COntent);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    //File.WriteAllTextAsync("WriteText.txt", text);
                    return Ok(JsonConvert.SerializeObject(Content));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpPost]
        [Route("WhenIWorkObjSample")]

        public async Task<IActionResult> WhenIWorkObj([FromBody] Object _jrequest)
        {
            try
            {
                string EmployeeId = "3f55033f-05ec-0000-0000-00004a300400";

                Dictionary<string, Object> _request = new Dictionary<string, object>();

                string RefreshToken = "";

                //if (_jrequest == null)
                //{
                //string MapperData = File.ReadAllText(@"C:\mapper.json");

                //using (StreamReader r = new StreamReader(@"C:\Users\lavak\RefreshKey.txt"))
                //{
                //    RefreshToken = r.ReadToEnd();
                //}

                //using (StreamReader r = new StreamReader(@"C:\Users\lavak\mapper.json"))
                //{

                //    string MapperData = r.ReadToEnd();
                //    _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(MapperData);
                //}

                //}
                //else
                //{
                    _request = JsonConvert.DeserializeObject<Dictionary<string, Object>>(_jrequest.ToString());

                //}
                Dictionary<string, Object> Data = new Dictionary<string, Object>();
                Dictionary<string, Object> Content = new Dictionary<string, Object>();



                string BaseURL = "https://apis-sandbox.paycor.com";


               

                #region Generate access token

                RestClient Authclient = new RestClient(BaseURL + "/sts/v1/common/token?subscription-key=a47dfa90b3ab40569e4e499055bb43a8");
                var Authrequest = new RestRequest();
                //Authrequest.AddParameter("application/x-www-form-urlencoded", $"grant_type=\"refresh_token\"&refresh_token=\"c51e8d5ee1cfd2abc284ca9057efa176a10bb0ee51737cb56cb145fa144c39bf\"&client_id=\"96a567d6d8e4db7f6906\"&client_secret=\"GnnLZaFcbm3GvK6dtl5Hhjsf/yb/nEvGDd9SNIi3Q\"");
                //Authrequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                Authrequest.AddParameter("grant_type", "refresh_token");
                Authrequest.AddParameter("refresh_token", "c51e8d5ee1cfd2abc284ca9057efa176a10bb0ee51737cb56cb145fa144c39bf");
                Authrequest.AddParameter("client_id", "96a567d6d8e4db7f6906");
                Authrequest.AddParameter("client_secret", "GnnLZaFcbm3GvK6dtl5Hhjsf/yb/nEvGDd9SNIi3Q");

                var Authresponse = Authclient.Post(Authrequest);

               

                Dictionary<string, Object> AuthContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Authresponse.Content.ToString());

                #endregion

                string AccessToken = "";
                if (Authresponse.Content != null && AuthContent.ContainsKey("access_token"))
                {
                    AccessToken = AuthContent["access_token"].ToString();

                    //await System.IO.File.WriteAllTextAsync("RefreshKey.txt", AuthContent["refresh_token"].ToString());
                }

                RestClient client = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");//"/v1/employees/"+ EmployeeId+"/emergencycontact");
                                                                                                             //var request = new RestRequest();
                                                                                                             //var response = client.GetAsync(request);

                var request = new RestRequest();
                request.AddHeader("Accept", "application/json");
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("Authorization", "Bearer " + AccessToken);
                request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

                //request.AddJsonBody(_request);

                var response = client.Get(request);

                var Stresponse = response.Content.ToString();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                Dictionary<string, Object> PayContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(response.Content.ToString());
#pragma warning restore CS8602 // Dereference of a possibly null reference.

               
                

                #region Transform code

               

                if (_request.ContainsKey("Mappings"))
                {
                    JObject mapperObj = JObject.Parse(Stresponse);

                    List<JsonMapper> Paradoxdict = JsonConvert.DeserializeObject<List<JsonMapper>>(JsonConvert.SerializeObject(_request["Mappings"]));


                    foreach (var Obj in Paradoxdict)
                    {
                        //Dictionary<string, Object> DictObj = JsonConvert.DeserializeObject<Dictionary<string, Object>>(paradox.ToString());

                        //if (PayContent.ContainsKey(Obj.Source))
                        {

                           

                           string tempSource = "$." + Obj.Source;

                            

                            JToken jToken = mapperObj.SelectToken("$."+ Obj.Source);

                            Content.Add(Obj.Target, jToken);

                            //Paradoxdict[Obj.Target]=Content[Obj.Source].ToString();



                        }
                    }
                }
                #endregion


                
                string COntent = JsonConvert.SerializeObject(Content);

                

                //await System.IO.File.WriteAllTextAsync("EmployeeEmergencyContact.txt", COntent);

                
                return Ok(JsonConvert.SerializeObject(Content));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


    }

    public class JsonMapper
    {
        public string Source { get; set; }
        public string Target { get; set; }
    }

    public class ParadoxSample
    {
        public string ex_Id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string job_title { get; set; }

    }

    public class WhenIworkSample
    {
        public int role { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public int employee_code { get; set; }
        public double hours_max { get; set; }
        public double hourly_rate { get; set; }
    }
    
    public class WIWAuthModel
    {
        public string email { get; set; }
        public string password { get; set; }
    }
  
}


