using ADE.Dominio.Models;
using static ADE.Utilidades.Handlers.TextParser;
using System.Text;

namespace ADE.Utilidades.Extensions
{
    public static class ListProperties
    {
        public static string ListarPropriedades<T>(this T entity) where T : ModeloBase
        {
            StringBuilder Sb = new StringBuilder();
            foreach (var prop in entity.GetType().GetProperties())
            {
                try
                {
                    Sb.AppendLine(string.Format(" {0}: {1} ", prop.Name, prop.GetValue(entity, null)));
                }
                catch (System.Exception) { continue; }
            }
            return Sb.ToString();
        }

        public static string GerarMensagemAlteracao<T>(this T entity, T entidadeAntiga, string Mensagem = null) where T : ModeloBase
        {
            StringBuilder Sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Mensagem))
            {
                Sb.AppendLine(Mensagem);
            }
            foreach (var prop in entity.GetType().GetProperties())
            {
                try
                {
                    string ValorAtual = prop.GetValue(entity, null).ToString();
                    string ValorAntigo = prop.GetValue(entidadeAntiga, null).ToString();
                    if (!ValorAtual.Equals(ValorAntigo) && !prop.IsDateTime())
                    {
                        Sb.AppendLine(string.Format("{0} {1} ({2}) alterado(a) para : {3} ", entity.ToString(), prop.Name, ValorAntigo, ValorAtual));
                        System.Console.WriteLine(Sb.ToString());
                    }
                }
                catch (System.Exception) { continue; }
            }
            return Sb.ToString();
        }

        #region Referencia
                   /*
                    Referencia:
                    _@D_ = <div>
                    _@P_ = <p>
                    _@L_ = <label>
                        */
                   #endregion
    }
}
