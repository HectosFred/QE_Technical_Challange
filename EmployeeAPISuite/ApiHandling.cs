using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net.Http;


namespace QE_Tech_Chalange_API_Tests
{

    public class ApiHandling
    {
        JSchema employee_schema = JSchema.Parse(@"{
          'type': 'object',
          'properties': {
            'Employee': {
              'type': 'object',
              'properties': {
                'FirstName': {
                  'type': 'string'
                },
                'LastName': {
                  'type': 'string'
                },
                'DateOfBirth': {
                  'type': 'string'
                },
                'StartDate': {
                  'type': 'string'
                },
                'Department': {
                  'type': 'string'
                },
                'JobTitle': {
                  'type': 'string'
                },
                'Email': {
                  'type': 'string'
                },
                'Mobile': {
                  'type': 'string'
                },
                'Adress': {
                  'type': 'string'
                },
                'BaseSalary': {
                  'type': 'string'
                }
              },
              'required': [
                'FirstName',
                'LastName',
                'DateOfBirth',
                'StartDate',
                'Department',
                'JobTitle',
                'Email',
                'Mobile',
                'Adress',
                'BaseSalary'
              ]
            }
          },
          'required': [
            'Employee'
          ]
        }");
        //Create new employee within API, based on a given Employee object  
        public static HttpRequestMessage StructureEmployeeCreateRequest(Employee employee_, string endpoint_url_)
        {
            HttpRequestMessage post_message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(endpoint_url_, "/employee")),
            };
            HttpContent content = new StringContent(employee_.ConvertEmployeeToJsonString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            post_message.Content = content;
            return post_message;
        }

        
        //Read a given employee from the API based on the provided emoployee ID
        public static HttpRequestMessage StructureEmployeeReadRequest(string employee_id_, string endpoint_url_)
        {
            HttpRequestMessage post_message = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(string.Concat(endpoint_url_, "/employee/", employee_id_)),
            };
            return post_message;
        }

        //Update a given existing employee based on a provided employee id and JSON string
        public static HttpRequestMessage StructureEmployeeUpdateRequest(string employee_id_, string endpoint_url_, string updated_employee_json_)
        {
            HttpRequestMessage post_message = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(string.Concat(endpoint_url_, "/employee/", employee_id_)),
            };
            HttpContent content = new StringContent(updated_employee_json_);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            post_message.Content = content;
            return post_message;
        }

        ////Update a given existing employee based on a provided employee id and Employee object
        public static HttpRequestMessage StructureEmployeeUpdateRequest(string employee_id_, string endpoint_url_, Employee updated_employee_)
        {
            HttpRequestMessage post_message = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(string.Concat(endpoint_url_, "/employee/", employee_id_)),
            };
            HttpContent content = new StringContent(updated_employee_.ConvertEmployeeToJsonString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            post_message.Content = content;
            return post_message;
        }

        public static HttpRequestMessage StructureEmployeeDeleteRequest(string employee_id_, string endpoint_url_)
        {
            HttpRequestMessage post_message = new HttpRequestMessage()
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(string.Concat(endpoint_url_, "/employee/", employee_id_)),
            };
            return post_message;
        }

        public static HttpRequestMessage StructureGetAllEmployeesRequest(string endpoint_url_)
        {
            HttpRequestMessage post_message = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(string.Concat(endpoint_url_, "/employee")),
            };
            return post_message;
        }

        public static async Task<Tuple<int, string>> SendAllEmployeeGetRequest(string api_url)
        {
            HttpRequestMessage request = StructureGetAllEmployeesRequest(api_url);
            try
            {
                Tuple<int, string> response = await HttpPosting.SendRequestMessage(request);
                return response;
            }
            catch(Exception e)
            {
                return new Tuple<int, string>(0, e.Message);
            }           
        }

        public static async Task<Tuple<int, string>> SendEmployeeGetRequest(string api_url_, string employee_id_)
        {
            HttpRequestMessage request = StructureEmployeeReadRequest(employee_id_ , api_url_);
            try
            {
                Tuple<int, string> response = await HttpPosting.SendRequestMessage(request);
                return response;
            }
            catch (Exception e)
            {
                return new Tuple<int, string>(0, e.Message);
            }
        }

        public static async Task<Tuple<int, string>> SendEmployeeUpdateRequest(string api_url_, string employee_id_, Employee updated_eployee)
        {
            HttpRequestMessage request = StructureEmployeeUpdateRequest(employee_id_, api_url_, updated_eployee);
            try
            {
                Tuple<int, string> response = await HttpPosting.SendRequestMessage(request);
                return response;
            }
            catch (Exception e)
            {
                return new Tuple<int, string>(0, e.Message);
            }
        }

        public static async Task<Tuple<int, string>> SendEmployeeDeleteRequest(string api_url_, string employee_id_)
        {
            HttpRequestMessage request = StructureEmployeeDeleteRequest(employee_id_, api_url_);
            try
            {
                Tuple<int, string> response = await HttpPosting.SendRequestMessage(request);
                return response;
            }
            catch (Exception e)
            {
                return new Tuple<int, string>(0, e.Message);
            }
        }

        public static async Task<Tuple<int, string>> SendEmployeeCreateRequest(string api_url_, Employee employee_)
        {
            HttpRequestMessage request = StructureEmployeeCreateRequest(employee_, api_url_);
            try
            {
                Tuple<int, string> response = await HttpPosting.SendRequestMessage(request);
                return response;
            }
            catch (Exception e)
            {
                return new Tuple<int, string>(0, e.Message);
            }
        }

        public static JArray DeserializeResponseToJArray(Tuple<int, string> response_)
        {
            if(response_.Item1 < 200 || response_.Item1 >= 300)
            {
                throw (new Exception("Response not in 200 range"));
            }
            try
            {
                JArray jarray = JArray.Parse(response_.Item2);
                return jarray;
            }
            catch (FormatException e)
            {
                return new JArray();
            }           
        }

        public static async Task<string> GetEmployeeIdFromFirstAndLastName(string first_name_, string last_name_, string api_url_)
        {
            Tuple<int, string> response = await SendAllEmployeeGetRequest(api_url_);
            JArray response_jarray = DeserializeResponseToJArray(response);
            for(int i = 0; i < response_jarray.Count; ++i )
            {
                if (string.Equals(response_jarray[i]["Employee"]["FirstName"].ToString(), first_name_) && string.Equals(response_jarray[i]["Employee"]["LastName"].ToString(), last_name_))
                {
                    return response_jarray[i]["_id"].ToString();
                }
            }
            throw new Exception("No matching employee");         
        }
    }






}
