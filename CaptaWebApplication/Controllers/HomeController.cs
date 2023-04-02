using CaptaWebApplication.Models;
using CaptaWebApplication.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace CaptaWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            VMLogin vmLogin = new VMLogin();

            using (var client = new HttpClient())
            {
                // Defina as credenciais de autenticação básica
                var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(configuration.GetSection("Authorization").Value)));

                // Defina o cabeçalho de autorização da solicitação HTTP
                client.DefaultRequestHeaders.Authorization = authHeader;

                // Faça a solicitação HTTP GET para a URL protegida
                var response = await client.GetAsync(configuration.GetSection("GetAllClients").Value);

                if (response.IsSuccessStatusCode)
                {
                    // Obtenha o conteúdo da resposta como uma string
                    var responseBody = await response.Content.ReadAsStringAsync();
                    string json = responseBody.ToString();
                    // Desserialize o conteúdo da resposta em um objeto
                    if (json != null)
                    {
                        DeserializeUtilities deserializeUtilities = new DeserializeUtilities();
                        vmLogin = deserializeUtilities.Deserialize<VMLogin>(json);
                    }
                    // Faça algo com o objeto aqui
                    //Console.WriteLine($"O objeto tem o valor '{obj.Value}'");
                }
            }

            return View(vmLogin);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> DeleteClientAsync(string cpf)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            using (var client = new HttpClient())
            {
                // Defina as credenciais de autenticação básica
                var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(configuration.GetSection("Authorization").Value)));

                // Defina o cabeçalho de autorização da solicitação HTTP
                client.DefaultRequestHeaders.Authorization = authHeader;

                // Faça a solicitação HTTP GET para a URL protegida
                var response = await client.DeleteAsync(configuration.GetSection("DeleteClient").Value + cpf);
            }
                return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}