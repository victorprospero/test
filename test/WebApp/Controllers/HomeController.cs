using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize()]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly string[] _scope;


        public HomeController(ILogger<HomeController> logger, IHttpClientFactory clientFactory, IConfiguration configuration, ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _configuration = configuration;
            _tokenAcquisition = tokenAcquisition;
            _scope = new string[] { "api://e52d269b-4f01-41e0-8739-cd036272e73c/user.read" };
        }

        public async Task<IActionResult> Index()
        {
            var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(_scope);

            var client = _clientFactory.CreateClient();
            client.BaseAddress = new System.Uri("https://localhost:44306/");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.GetAsync("weatherforecast");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
