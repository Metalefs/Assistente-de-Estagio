using Newtonsoft.Json;

namespace ADE.Dominio.Models
{
    public class RequisitosBasicosCabecalho
    {
        public string NomeAluno { get; set; }
        public string RA { get; set; }
        public string Turma { get; set; }
        public string Carga_Horaria_Exigida { get; set; }
        public string AreaEstagio { get; set; }
        public string Nome_Instituicao { get; set; }
    }
}
