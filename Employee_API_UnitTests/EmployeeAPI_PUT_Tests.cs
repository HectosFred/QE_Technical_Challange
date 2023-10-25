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
    public class Api_PUT_Tests
    {

        [TestMethod]
        public async Task EmployeeAPI_PUT_UpdateEmployeeGivenValidJsonInput_Returns200()
        {
            //Make and upload new employee
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);
            //Get EmployeeId for newly created employee
            string employee_id = (string)JObject.Parse(response.Item2)["_id"];

            //Update first and last name of the previously uploaded employee object
            employee.first_name_ = "Test test test";
            employee.last_name_ = "Test test test";
            //Send updated employee object as an update request the existing employeer record
            response = await ApiHandling.SendEmployeeUpdateRequest(GlobalVariables.api_url, employee_id, employee);                                       

            Assert.AreEqual(response.Item1, 200);

            response = await ApiHandling.SendEmployeeGetRequest(GlobalVariables.api_url, employee_id);

            //Confirm that employee record reflects the updates                                                                                                
            Assert.IsTrue(Employee.Equals(employee, new Employee(JObject.Parse(response.Item2)))); 
        }


        //Assert that when providing invalid JSON input the post request fails with 400 BAD REQUEST
        [TestMethod]
        public async Task EmployeeAPI_PUT_UpdateEmployeeGivenInvalidJsonInput_Returns400BadRequest()
        {
            //Make and upload new employee
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);
            //Get EmployeeId for newly created employee
            string employee_id = (string)JObject.Parse(response.Item2)["_id"];
            //Update first and last name of the previously uploaded employee object
            employee.first_name_ = "Test test test";
            employee.last_name_ = "Test test test";
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(string.Concat(GlobalVariables.api_url, "/employee/", employee_id )),
            };
            //Add additional curly brace to request payload. Invalidates JSON string
            HttpContent content = new StringContent(string.Concat("{", employee.ConvertEmployeeToJsonString()));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = content;
            response = await HttpPosting.SendRequestMessage(request);

            Assert.AreEqual(response.Item1, 400);
        }

        [TestMethod]
        public async Task EmployeeAPI_PUT_UpdateEmployeeGivenInvalidEmployeeId_Returns404NotFound()
        {
            //Make and upload new employee
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);
            //Set an invalid employee id
            string employee_id = "notavalidemployeeid";
            //Update first and last name of the previously uploaded employee object
            employee.first_name_ = "Test test test";
            employee.last_name_ = "Test test test";

            //Make send an update request using the invalid employee id
            response = await ApiHandling.SendEmployeeUpdateRequest(GlobalVariables.api_url, employee_id, employee);

            Assert.AreEqual(response.Item1, 404);
        }

        [TestMethod]
        public async Task EmployeeAPI_PUT_ValidateJsonSchemaOnEmployeeCreate()
        {
            //Make and upload new employee
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);
            //Get EmployeeId for newly created employee
            string employee_id = (string)JObject.Parse(response.Item2)["_id"];

            //Update first and last name of the previously uploaded employee object
            employee.first_name_ = "Test test test";
            employee.last_name_ = "Test test test";
            //Send updated employee object as an update request the existing employeer record
            response = await ApiHandling.SendEmployeeUpdateRequest(GlobalVariables.api_url, employee_id, employee);
            Assert.AreEqual(response.Item1, 200);

            //Get updated employee
            response = await ApiHandling.SendEmployeeGetRequest(GlobalVariables.api_url, employee_id);   
            //Asser schema of updated employee is valid
            Assert.IsTrue(ApiHandling.IsEmployeeSchemaValid(response.Item2));
        }
    }
}
