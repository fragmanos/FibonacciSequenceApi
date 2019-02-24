using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Xunit;
using static Xunit.Assert;

namespace TddPlayground.Integration
{
    public class FibonacciControllerTests : WebBase
    {        

        public FibonacciControllerTests()
        {
            BuildWebHost();
        }

        [Fact]
        public async Task GivenValidQueryParameter_WhenControllerProcessTheRequest_ThenShouldReturnStatus200_AndStringOfFibonacciElements()
        {
            var response = await GetRequest("http://localhost:1234/fibonacci/elements", "?e=9");
            Equal((HttpStatusCode) StatusCodes.Status200OK, response.StatusCode);
            Equal("[0,1,1,2,3,5,8,13,21]", response.Content.ReadAsStringAsync().Result);
        }
        
        [Fact]
        public async Task GivenInValidQueryParameter_WhenControllerProcessTheRequest_ThenShouldReturnStatus400()
        {
            var response = await GetRequest("http://localhost:1234/fibonacci/elements", "?e=-1");
            Equal((HttpStatusCode) StatusCodes.Status400BadRequest, response.StatusCode);
        }
        
    }

    public class WebBase
    {
        private static IWebHost WebHost { get; set; }
        private static HttpClient HttpClient { get; set; }

        private static IConfigurationRoot Config { get; set; }
        private const string BuildServer = "build_server";

        protected WebBase()
        {
            Config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .Build();
            HttpClient = new HttpClient();
        }
        
        protected static void BuildWebHost()
        {
            WebHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(Config)
                .UseUrls("http://localhost:1234")
                .UseEnvironment(BuildServer)
                .Build();
        }
        
        protected static async Task<HttpResponseMessage> GetRequest(string requestUri, string paramWithContent)
        {
            HttpResponseMessage response;
            try
            {
                WebHost.Start();
                response = await HttpClient.GetAsync(requestUri + paramWithContent);
            }
            finally
            {
                await WebHost.StopAsync();
                WebHost.Dispose();
            }

            return response;
        }
    }
}