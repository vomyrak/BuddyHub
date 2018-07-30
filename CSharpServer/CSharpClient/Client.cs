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
            string path = "http://localhost:8080/robotic_arm/";
            var result = "";
            string input = Console.ReadLine();
            while (input != null)
            {
                try
                {
                    result = getResponse(path + input).GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.WriteLine(result);
                input = Console.ReadLine();
            }
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
