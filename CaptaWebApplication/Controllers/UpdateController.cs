using CaptaWebApplication.Models;
using CaptaWebApplication.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace CaptaWebApplication.Controllers
{
    public class UpdateController : Controller
    {
        private readonly ILogger<UpdateController> _logger;

        public UpdateController(ILogger<UpdateController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> UpdateClientAsync(string cpf)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            Client clientUpdate = new Client();
            using (var client = new HttpClient())
            {
                // Defina as credenciais de autenticação básica
                var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(configuration.GetSection("Authorization").Value)));

                // Defina o cabeçalho de autorização da solicitação HTTP
                client.DefaultRequestHeaders.Authorization = authHeader;

                // Faça a solicitação HTTP GET para a URL protegida
                var response = await client.GetAsync(configuration.GetSection("GetClient").Value + cpf);

                if (response.IsSuccessStatusCode)
                {
                    // Obtenha o conteúdo da resposta como uma string
                    var responseBody = await response.Content.ReadAsStringAsync();
                    string json = responseBody.ToString();
                    // Desserialize o conteúdo da resposta em um objeto
                    if (json != null)
                    {
                        DeserializeUtilities deserializeUtilities = new DeserializeUtilities();
                        clientUpdate = deserializeUtilities.Deserialize<Client>(json);
                    }
                    // Faça algo com o objeto aqui
                    //Console.WriteLine($"O objeto tem o valor '{obj.Value}'");
                }

            }
            return View(clientUpdate);
            //return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> UpdateAsync(Client client)
        {
            //Instancia o builder para pegar informação do appsettings
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            string url = configuration.GetSection("UpdateClient").Value;

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
