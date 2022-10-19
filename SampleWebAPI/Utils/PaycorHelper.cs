using Microsoft.AspNetCore.Mvc;
using SampleWebAPI.Models;
using Newtonsoft.Json.Linq;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SampleWebAPI.Utils
{
    public class PaycorHelper
    {
        public RestResponse GetPayratesForEmployee(string BaseURL, string EmployeeId, string AccessToken)
        {
            RestClient Rateclient = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "/payrates");

            var Raterequest = new RestRequest();
            Raterequest.AddHeader("Accept", "application/json");
            Raterequest.AddHeader("Content-Type", "application/json");
            Raterequest.AddHeader("Authorization", "Bearer " + AccessToken);
            Raterequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

            //request.AddJsonBody(_request);

            var Rateresponse = Rateclient.Get(Raterequest);
            return Rateresponse;
        }

        public RestResponse GetEmployeeData(string BaseURL, string EmployeeId, string AccessToken)
        {
            RestClient Rateclient = new RestClient(BaseURL + "/v1/employees/" + EmployeeId + "?include=All");

            var Raterequest = new RestRequest();
            Raterequest.AddHeader("Accept", "application/json");
            Raterequest.AddHeader("Content-Type", "application/json");
            Raterequest.AddHeader("Authorization", "Bearer " + AccessToken);
            Raterequest.AddHeader("Ocp-Apim-Subscription-Key", "a47dfa90b3ab40569e4e499055bb43a8");

            //request.AddJsonBody(_request);

            var Empresponse = Rateclient.Get(Raterequest);
            return Empresponse;
        }
    }
}
