using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using QE_Tech_Chalange_API_Tests;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Employee_API_UnitTests
{

    //Assert that when providing a valid JSON input the API returs 201 RESOURCE CREATED along with a copy of the newly created employee JSON entry
    [TestClass]
    public class Api_POST_Tests
    {

        [TestMethod]
        public async Task EmployeeAPI_POST_ValidJsonInput_Returns201WithEmployeeJson()
        {
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);


            Assert.AreEqual(response.Item1, 201);
            //Confirm that the returned employee is the same as the one uploade
            Assert.IsTrue(Employee.Equals(employee, new Employee(JObject.Parse(response.Item2))));
        }


        //Assert that when providing invalid JSON input the post request fails with 400 BAD REQUEST
        [TestMethod]
        public async Task EmployeeAPI_POST_InvalidJsonInput_Returns400BadRequest()
        {
            Employee employee = new Employee();
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(GlobalVariables.api_url, "/employee")),
            };

            //Add additional curly brace to request payload. Invalidates JSON string
            HttpContent content = new StringContent(string.Concat("{" ,employee.ConvertEmployeeToJsonString()));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            Tuple<int, string> response = await HttpPosting.SendRequestMessage(request);

            Assert.AreEqual(response.Item1, 400);
        }

        [TestMethod]
        public async Task EmployeeAPI_POST_ValidateJsonSchemaOnEmployeeCreate()
        {
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);
            Assert.IsTrue(ApiHandling.IsEmployeeSchemaValid(response.Item2));
        }
    }
}
