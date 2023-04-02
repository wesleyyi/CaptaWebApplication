
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace CaptaWebApplication.Models
{
    public class VMLogin
    {
        public List<Client> clients { get; set; }
    }
    public class Client
    {
        public string nome { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###.###.###-##}")]
        public string cpf { get; set; }
        public string tipoCliente { get; set; }
        public string sexo { get; set; }
        public string situacaoCliente { get; set; }

    }
}
