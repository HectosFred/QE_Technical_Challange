using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using QE_Tech_Chalange_API_Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Employee_API_UnitTests
{
    [TestClass]
    public class EmployeeAPI_DELETE_Tests
    {

        //Assert that requesting a single employee given a valid employee id, the API returns 200 OK with the requested employee
        [TestMethod]
        public async Task EmployeeAPI__DELETE_DeleteEmployeeGivenValidEmployeeId_Returns200AndTheEmployeeIsDeleted()
        {
            //Create new employee for us to delete
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);
            Assert.AreEqual(response.Item1, 201);
            //Get the employee id from the response JSON
            string employee_id = (string)JObject.Parse(response.Item2)["_id"];
            //Get the employee id for the above created user so we can confirm it matches the employee id returned in the response to the POST request. Just to make sure the employee is visible in the system
            string fetched_employee_id = await ApiHandling.GetEmployeeIdFromFirstAndLastName(employee.first_name_, employee.last_name_, GlobalVariables.api_url);
            Assert.IsTrue(string.Equals(employee_id, fetched_employee_id));

            //Delete the employee
            response = await ApiHandling.SendEmployeeDeleteRequest(GlobalVariables.api_url, employee_id);
            Assert.AreEqual(response.Item1, 200);
            //Check the employee is no longer visible
            try
            {
                fetched_employee_id = await ApiHandling.GetEmployeeIdFromFirstAndLastName(employee.first_name_, employee.last_name_, GlobalVariables.api_url);
                // Fail test is no exception is thrown (exception is expected if we attempt to fetch the id of a deleted employee)
                Assert.Fail();
            }
            catch (Exception e_)
            {
                //Pass test if the thrown exceptions is the expected type of exception
                Assert.IsTrue(string.Equals(e_.Message, "No matching employee"));
            }
            
        }

        //Assert that when providing an invalid employee id the api returns 404 NOT FOUND
        [TestMethod]
        public async Task EmployeeAPI_DELETE_DeleteEmployeeGivenInvalidEmployeeId_Returns404NotFoundA()
        {
            //Create new employee for us to delete
            Employee employee = new Employee();
            Tuple<int, string> response = await ApiHandling.SendEmployeeCreateRequest(GlobalVariables.api_url, employee);
            Assert.AreEqual(response.Item1, 201);
            //Get the employee id of the newly created employee
            string employee_id = (string)JObject.Parse(response.Item2)["_id"];
            //Get the employee id for the above created user so we can confirm it matches the employee id returned in the response to the POST request. Just to make sure the employee is visible in the system
            string fetched_employee_id = await ApiHandling.GetEmployeeIdFromFirstAndLastName(employee.first_name_, employee.last_name_, GlobalVariables.api_url);
            Assert.IsTrue(string.Equals(employee_id, fetched_employee_id));

            //Get all employees so we can confirm none are deleted after our invalid delete request
            Tuple<int,string> response_all_employees = await ApiHandling.SendAllEmployeeGetRequest(GlobalVariables.api_url);

            //Delete the employee
            response = await ApiHandling.SendEmployeeDeleteRequest(GlobalVariables.api_url, "notavalidemployeeid");
            Assert.AreEqual(response.Item1, 404);
            //Check the employee is still longer visible
            response = await ApiHandling.SendAllEmployeeGetRequest(GlobalVariables.api_url);
            Assert.IsTrue(string.Equals(response, response_all_employees));
        }
    }
}
