using System;
using ADE.Dominio.Models.Enums;
using static ADE.Dominio.Models.Enums.TipoEvento;
using ADE.Dominio.Models.Individuais;
using System.Threading.Tasks;
using ADE.Dominio.Models;

namespace ADE.Utilidades.Handlers
{
    public static class LoggingHandler<T> where T : ModeloBase
    {
        public static string GerarMensagemTipoLog(UsuarioADE usuario, T entity, TipoEvento Acao)
        {
            try
            {
                string verbo = "";
                switch (Acao)
                {
                    case Criacao:
                        verbo = "CRIOU";
                        break;
                    case Alteracao:
                        verbo = "ALTEROU";
                        break;
                    case Delecao:
                        verbo = "DELETOU";
                        break;
                    default:
                        throw new InvalidOperationException("Ação invalida realizada");
                }
                if(usuario != null)
                {
                    return $"{usuario.UserName} (ID: {usuario.Id}) {verbo} {entity.GetType().Name} {entity.ToString()}";
                }
                else
                {
                    return $"{verbo} {entity.GetType().Name} {entity.ToString()}";
                }
            }
            catch(NullReferenceException)
            {
                return $"Erro ao gerar mensagem de log";
            }
        }
    }
}
