using Newtonsoft.Json;

namespace ADE.Dominio.Models
{
    public class DadosAlunoKV
    {
        public string Name { get; set; }
        public string Value { get; set; }

        [JsonIgnore]
        public Requisito Requisito { get; set; }
    }
}
