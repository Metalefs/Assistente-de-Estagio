using ADE.Dominio.Models.Enums;
using ADE.Dominio.Models.Individuais;
using Assistente_de_Estagio.Areas.Shared;
using System.Collections.Generic;

namespace Assistente_de_Estagio.Areas.Acesso.Models
{
    public class NotificacaoViewmodel
    {
        public List<NotificacaoIndividual> NotificacacoesIndividuais { get; set; }
        public List<Notificacao> NotificacacoesGerais { get; set; }
    }
}
