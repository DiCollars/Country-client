using System;
using System.Net.Http;

namespace Country_client.Helper
{
    public class ApiCatcher
    {
        public HttpClient Initial()
        {
            var Client = new HttpClient();
            Client.BaseAddress = new Uri("https://restcountries.eu/");
            return Client;

        }
    }
}
