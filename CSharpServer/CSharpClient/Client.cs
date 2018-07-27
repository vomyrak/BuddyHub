using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace CSharpClient
{
    class Client
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            string path = "http://localhost:8080/test/";
            var result = "";
            try
            {
                result = getResponse(path).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static async Task<string> getResponse(string path)
        {
            string responseString = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                responseString = await client.GetStringAsync(path);
            }
            return responseString;
        }
    }
}
