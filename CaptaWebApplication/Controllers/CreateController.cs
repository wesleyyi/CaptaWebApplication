using CaptaWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CaptaWebApplication.Controllers
{
    public class CreateController : Controller
    {
        private readonly ILogger<CreateController> _logger;

        public CreateController(ILogger<CreateController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }

        public async Task<IActionResult> CreateClientPostAsync(Client client)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            string url = configuration.GetSection("CreateClient").Value;

            string authInfo = configuration.GetSection("Authorization").Value;
            string encodedAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedAuth);
            var content = new StringContent(JsonConvert.SerializeObject(client), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url, content);

            return RedirectToAction("Index", "Home");
        }
    }
}
