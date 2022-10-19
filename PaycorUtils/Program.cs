using RestSharp;
using Newtonsoft.Json;

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


//request.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

//request.AddJsonBody(_jrequest);

Dictionary<string, Object> AuthContent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(Authresponse.Content.ToString());

#endregion

#region Authenticate WIW

string WIWLoginURL = "https://api.login.wheniwork.com";
string WIWBaseURL = "https://api.wheniwork.com";

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

var WIWAuthContent= JsonConvert.DeserializeObject<Dictionary<string, Object>>(WIWAuthresponse.Content.ToString());

string WIWApiToken = "";

if (WIWAuthContent.ContainsKey("token"))
{
    WIWApiToken=WIWAuthContent["token"].ToString();
}

#endregion

#region WIW Get

RestClient GetWIW = new RestClient(WIWBaseURL + "/login");


var WIWGetrequest = new RestRequest();
//request.AddHeader("Accept", "application/json");
WIWGetrequest.AddHeader("Content-Type", "application/json");
WIWGetrequest.AddHeader("W-Token", WIWApiToken);
//WIWGetrequest.AddJsonBody(wIWAuth);


var WIWGetresponse = AuthWIW.Get(WIWGetrequest);
var WIWGetcontent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(WIWGetresponse.Content.ToString());

await System.IO.File.WriteAllTextAsync(@"C:\Users\lavak\WIWGet.txt", WIWGetresponse.Content.ToString());

#endregion

string AccessToken = "";
Console.WriteLine("Enter EmployeeID: ");
string EmployeeId = Console.ReadLine();

if (Authresponse.Content != null && AuthContent.ContainsKey("access_token"))
{
    AccessToken = AuthContent["access_token"].ToString();

    await System.IO.File.WriteAllTextAsync(@"C:\Users\lavak\RefreshKey.txt", AuthContent["refresh_token"].ToString());
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

//ParadoxSample paradox = new();


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

           temptarget = PayContent[Obj.Source].ToString();

            Content.Add(Obj.Target, PayContent[Obj.Source]);

            //Paradoxdict[Obj.Target]=Content[Obj.Source].ToString();
        }
    }
}
#endregion

#region WIW Post

RestClient PostWIW = new RestClient(WIWBaseURL + "/2/users");


var WIWPostrequest = new RestRequest();
//request.AddHeader("Accept", "application/json");
WIWPostrequest.AddHeader("Content-Type", "application/json");
WIWPostrequest.AddHeader("W-Token", WIWApiToken);
WIWPostrequest.AddJsonBody(Content);


var WIWPostresponse = PostWIW.Post(WIWPostrequest);
var WIWPostcontent = JsonConvert.DeserializeObject<Dictionary<string, Object>>(WIWPostresponse.Content.ToString());

await System.IO.File.WriteAllTextAsync(@"C:\Users\lavak\WIWPost.txt", WIWPostresponse.Content.ToString());

#endregion

//string COntent = JsonConvert.SerializeObject(Content);
string COntent = JsonConvert.SerializeObject(Content);

//StreamWriter sw = new StreamWriter("LegalEntityGet.txt", append: true);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
//await sw.WriteLineAsync(JsonConvert.SerializeObject(Content));
await System.IO.File.WriteAllTextAsync(@"C:\Users\lavak\MapperResult.txt", COntent);


Console.WriteLine("Mapped Output:");
Console.WriteLine(COntent);


public class JsonMapper
{
    public string Source { get; set; }
    public string Target { get; set; }
}

public class WIWAuthModel
{
    public string email { get; set; }
    public string password { get; set; }
}