using System;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Models
{
    public partial class Requisito
    {
        public int IdRequisito { get; set; }
        public int UsuarioIdUsuario { get; set; }
        public string NomeAluno { get; set; }
        public string Ra { get; set; }
        public string Turno { get; set; }
        public string Turma { get; set; }
        public string Periodo { get; set; }
        public string EscolhaArea { get; set; }
        public string NomeSupervisor { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string NomeEmpresa { get; set; }
        public string CursoMatriculado { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Dddtel { get; set; }
        public string Dddcel { get; set; }

        public virtual Usuario UsuarioIdUsuarioNavigation { get; set; }
    }
}
