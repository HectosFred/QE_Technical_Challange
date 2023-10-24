using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QE_Tech_Chalange_API_Tests
{
    public class HttpPosting
    {
        static bool is_http_client_created = false;
        static HttpClient http_client;
        public static HttpClient MakeHttpClient()
        {
            if (is_http_client_created == false)
            {
                http_client = new HttpClient();
                return http_client;
            }
            else
            {
                return http_client;
            }

        }

        public static async Task<Tuple<int, string>> SendRequestMessage(HttpRequestMessage request_)
        {
            if(is_http_client_created == false)
            {
                MakeHttpClient();
            }

            try
            {
                HttpResponseMessage response = http_client.SendAsync(request_).Result;
                int status_code = ((int)response.StatusCode);
                string response_text;
                using (StreamReader stream_reader = new StreamReader(await response.Content.ReadAsStreamAsync()))
                {
                    response_text = await stream_reader.ReadToEndAsync();
                }
                Tuple<int, string> response_pair = new Tuple<int, string>(status_code, response_text);
                return response_pair;
            }
            catch (WebException e_)
            {
                return new Tuple<int, string>(0, e_.Message);
            }

        }

        public static async Task<bool> IsSiteReachable(string url_)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url_);
            request.Method = HttpMethod.Get;
            Tuple<int, string> response = await SendRequestMessage(request);
            if (response.Item1 == 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}