using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KazanMobileSession1_25_07
{ 
    public class WebApi
    {
        private HttpClient client = new HttpClient();

        private string baseAddress = "http://10.0.2.2:50452/";

        public async Task<string> PostAsync(string address, string JSON)
        {
            if (JSON == null)
            {
                var url = baseAddress + address;
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return (responseString);
            }

            else
            {
                var url = baseAddress + address;
                var content = new StringContent(JSON, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();
                return (responseString);
            }
        }
    }
}
