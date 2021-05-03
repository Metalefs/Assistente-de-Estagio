using System;
using System.Collections.Generic;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assistente_de_Estagio.Areas.Acesso.Models
{
    public enum EnumErrorMessages
    {
        [Description ("Requisição errada: O servidor recusou-se a processar o pedido")]
        Status400,
        [Description ("Ação negada")]
        Status403,
        [Description ("O caminho requisitado não foi encontrado.")]
        Status404,
        [Description ("O usuário não está autenticado.")]
        Status405,
        [Description ("Autenticação de proxy necessária, contate o seu administrador de rede")]
        Status407,
    }
}
