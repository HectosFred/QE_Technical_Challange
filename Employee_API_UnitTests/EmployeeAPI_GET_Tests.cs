using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QE_Tech_Chalange_API_Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_API_UnitTests
{
    [TestClass]
    public class EmployeeAPI_GET_Tests
    {

        //Assert that requesting all employees returns a list of employees
        [TestMethod]
        public async Task EmployeeAPI_GET_RequestAllEmployees_Returns200WithAllEmployeeJson()
        {
            //Create 2 new employees so we can confirm the number of employees shown is greater than 1, even if the endpoint currently holds no employee records
            Employee employee1 = new Employee();
            Employee employee2 = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee1);
            response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee2);

            //Make request for all employees
            response = await ApiHandling.SendAllEmployeeGetRequest(GlobalVariables.api_url);
            JArray employee_array = ApiHandling.DeserializeResponseToJArray(response);

            Assert.AreEqual(response.Item1, 200);
            Assert.IsTrue(employee_array.Count() > 1);
        }

        //Assert that requesting a single employee given a valid employee id, the API returns 200 OK with the requested employee
        [TestMethod]
        public async Task EmployeeAPI_GET_RequestSingleEmployeeUsingValidEmployeeId_Returns200WithJsonOfSpecifiedEmployee()
        {
            //Create new employee for us to fetch
            Employee employee = new Employee();         
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);

            //Get the employee id of the newly created employee
            string employee_id = (string)JObject.Parse(response.Item2)["_id"];

            //Make GET request for the specified employee
            response = await ApiHandling.SendEmployeeGetRequest(GlobalVariables.api_url, employee_id);

            Assert.AreEqual(response.Item1, 200);
            Assert.IsTrue(Employee.Equals(employee, new Employee(JObject.Parse(response.Item2))));
        }

        //Assert that when providing an invalid employee id the api returns 404 NOT FOUND
        [TestMethod]
        public async Task EmployeeAPI_GET_RequestSingleEmployeeUsingInvalidEmployeeId_Returns400BadRequest()
        {
            //Get the employee id of the newly created employee
            string employee_id = "notavalidemployeeid";
            //Make GET request for the specified employee
            Tuple<int, string> response = await ApiHandling.SendEmployeeGetRequest(GlobalVariables.api_url, employee_id);

            Assert.AreEqual(response.Item1, 404);
        }
    }
}
