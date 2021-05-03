using ADE.Dominio.Models;
using System;
using System.Collections.Generic;
using static ADE.Utilidades.Handlers.TextParser;
using static ADE.Utilidades.Helpers.PropertyGenerator;
using ADE.Utilidades.Constants;
using System.Text;
using ADE.Dominio.Interfaces;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Html;
using ADE.Utilidades.Helpers;
using System.IO;

namespace ADE.Utilidades.Extensions
{
    public static class ClassToHTML
    {
        public static HtmlString ExibirEntidadeColorCoded(this IColorCoded entidade, string elemento = "p", string imgclass = "img-thumbnail rounded float-left")
        {
            List<KeyValuePair<string, string>> ConteudoStyles = new List<KeyValuePair<string, string>>()
            {
               new KeyValuePair<string, string>("border-bottom", $"3px solid {entidade.Color}")
            };

            string ConteudoStyle = CreateHTMLStyle(ConteudoStyles);
            string Conteudo = AninharEmElemento(elemento, entidade.Nome, CreateHTMLProperty("class","card-body instituicao-title") + ConteudoStyle);
            string HrefConteudo = AninharEmElemento("a", Conteudo);
            string Img = string.Empty;

            if(entidade.Logo != null)
            {
                List<KeyValuePair<string, string>> ImageProperties = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>("class", $"{imgclass}"),
                   new KeyValuePair<string, string>("src", $"data:image;base64,{Convert.ToBase64String(entidade.Logo)}")
                };
                Img = HTMLElements.Img(CreateHTMLProperties(ImageProperties));
            }

            string DivClasse  = CreateHTMLProperty("class", CSSCustomClassNames.InstituicaoDiv());
            string Properties = DivClasse;
            string Elemento = AninharEmDiv(Img + HrefConteudo, Properties);
            HtmlString html = new HtmlString(Elemento);
            return html;
        }

        public static HtmlString ExibirEntidadeColorCodedComNomePersonalizado(this IColorCoded entidade, string texto, string elemento = "p", string imgclass = "img-thumbnail rounded float-left")
        {
            List<KeyValuePair<string, string>> ConteudoStyles = new List<KeyValuePair<string, string>>()
            {
               new KeyValuePair<string, string>("border-bottom", $"3px solid {entidade.Color}")
            };

            string ConteudoStyle = CreateHTMLStyle(ConteudoStyles);
            string Conteudo = AninharEmElemento(elemento, texto, CreateHTMLProperty("class", "card-body instituicao-title") + ConteudoStyle);
            string HrefConteudo = AninharEmElemento("a", Conteudo);
            string Img = string.Empty;

            if (entidade.Logo != null)
            {
                List<KeyValuePair<string, string>> ImageProperties = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>("class", $"{imgclass}"),
                   new KeyValuePair<string, string>("src", $"data:image;base64,{Convert.ToBase64String(entidade.Logo)}")
                };
                Img = HTMLElements.Img(CreateHTMLProperties(ImageProperties));
            }

            string DivClasse = CreateHTMLProperty("class", CSSCustomClassNames.InstituicaoDiv());
            string Properties = DivClasse;
            string Elemento = AninharEmDiv(Img + HrefConteudo, Properties);
            HtmlString html = new HtmlString(Elemento);
            return html;
        }

        public static HtmlString ExibirEntidadeColorCodedSemImagem(this IColorCoded entidade, string elemento = "p")
        {
            List<KeyValuePair<string, string>> ConteudoStyles = new List<KeyValuePair<string, string>>()
            {
               new KeyValuePair<string, string>("border-bottom", $"3px solid {entidade.Color}")
            };

            string ConteudoStyle = CreateHTMLStyle(ConteudoStyles);
            string Conteudo = AninharEmElemento(elemento, entidade.Nome, CreateHTMLProperty("class", "card-body instituicao-title") + ConteudoStyle);
            string HrefConteudo = AninharEmElemento("a", Conteudo);

            string DivClasse = CreateHTMLProperty("class", CSSCustomClassNames.InstituicaoDiv());
            string Properties = DivClasse;
            string Elemento = AninharEmDiv(HrefConteudo, Properties);
            HtmlString html = new HtmlString(Elemento);
            return html;
        }

        public static HtmlString MensagemDePropriedadesEmHTML<T>(this T entity, string divClass, string OuterDivClass = null, string OuterDivId = null) where T : ModeloBase
        {
            OuterDivClass = OuterDivClass ?? string.Empty;
            OuterDivId = OuterDivId ?? string.Empty;

            StringBuilder Sb = new StringBuilder();
            foreach (var prop in entity.GetType().GetProperties())
            {
                try
                {
                    if (!prop.IsADEDominio(entity) && !prop.IsByteArray())
                    {
                        if (!prop.IsKey())
                        {
                            string NomeAtributo = ObterNomePropriedade(prop);
                            string ValorAtributo = ObterValorPropriedade(prop, entity);
                            string DescricaoAtributo = PropertyGenerator.CreateHTMLProperty("title", ObterDescricaoPropriedade(prop));
                            string LabelFor = CreateHTMLProperty("for", prop.Name);
                            string Div = AninharEmDiv(string.Format(" {0}{1} ", AninharEmLabel(NomeAtributo, LabelFor) +"</br>",AninharEmParagrafo(ValorAtributo, DescricaoAtributo + CreateHTMLProperty("id", prop.Name)), CreateHTMLProperty("class",divClass)));
                            Sb.AppendLine(Div);
                        }
                    }
                }
                catch (Exception) { continue; }
            }
            string texto = Sb.ToString();
            string OuterDivProperties = string.Empty;
            if (OuterDivClass != null)
                OuterDivProperties += CreateHTMLProperty("class", OuterDivClass);
            if (OuterDivId != null)
                OuterDivProperties += CreateHTMLProperty("id", OuterDivId);
            AninharEmDiv(ref texto, OuterDivProperties);
            HtmlString textoHtml = new HtmlString(texto);
            return textoHtml;
        }

        public static HtmlString MensagemDePropriedadesEmHTMLSemLabel<T>(this T entity, string divClass, string OuterDivClass = null, string OuterDivId = null) where T : ModeloBase
        {
            OuterDivClass = OuterDivClass ?? string.Empty;
            OuterDivId = OuterDivId ?? string.Empty;

            StringBuilder Sb = new StringBuilder();
            foreach (var prop in entity.GetType().GetProperties())
            {
                try
                {
                    if (!prop.IsADEDominio(entity) && !prop.IsByteArray())
                    {
                        if (!prop.IsKey())
                        {
                            string NomeAtributo = ObterNomePropriedade(prop);
                            string ValorAtributo = ObterValorPropriedade(prop, entity);
                            string DescricaoAtributo = PropertyGenerator.CreateHTMLProperty("title", NomeAtributo);
                            string Div = AninharEmDiv(string.Format(" {0} ", AninharEmParagrafo(ValorAtributo, DescricaoAtributo + CreateHTMLProperty("id", prop.Name)), CreateHTMLProperty("class", divClass)));
                            Sb.AppendLine(Div);
                        }
                    }
                }
                catch (Exception) { continue; }
            }
            string texto = Sb.ToString();
            string OuterDivProperties = string.Empty;
            if (OuterDivClass != null)
                OuterDivProperties += CreateHTMLProperty("class", OuterDivClass);
            if (OuterDivId != null)
                OuterDivProperties += CreateHTMLProperty("id", OuterDivId);
            AninharEmDiv(ref texto, OuterDivProperties);
            HtmlString textoHtml = new HtmlString(texto);
            return textoHtml;
        }

        public static HtmlString MensagemDePropriedadesEmHTMLOmitindoAtributos<T>(this T entity, string divClass, string OuterDivClass = null, string OuterDivId = null) where T : ModeloBase
        {
            OuterDivClass = OuterDivClass ?? string.Empty;
            OuterDivId = OuterDivId ?? string.Empty;

            StringBuilder Sb = new StringBuilder();
            foreach (var prop in entity.GetType().GetProperties())
            {
                try
                {
                    if (!prop.IsADEDominio(entity))
                    {
                        if (!prop.IsKey() && !prop.IsDateTime() && !prop.IsGenericCollection() && !prop.IsByteArray())
                        {
                            string NomeAtributo = ObterNomePropriedade(prop);
                            string ValorAtributo = ObterValorPropriedade(prop, entity);
                            string DescricaoAtributo = PropertyGenerator.CreateHTMLProperty("title", ObterDescricaoPropriedade(prop));
                            string LabelFor = CreateHTMLProperty("for", prop.Name);
                            string Div = AninharEmDiv(string.Format(" {0} {1} ", AninharEmLabel(NomeAtributo, LabelFor) + "</br>", AninharEmParagrafo(ValorAtributo, DescricaoAtributo + CreateHTMLProperty("id", prop.Name))), CreateHTMLProperty("class", divClass));
                            Sb.AppendLine(Div);
                        }
                    }
                }
                catch (Exception) { continue; }
            }
            string texto = Sb.ToString();
            string OuterDivProperties = string.Empty;
            if (OuterDivClass != null)
                OuterDivProperties += CreateHTMLProperty("class", OuterDivClass);
            if (OuterDivId != null)
                OuterDivProperties += CreateHTMLProperty("id", OuterDivId);
            AninharEmDiv(ref texto, OuterDivProperties);
            HtmlString textoHtml = new HtmlString(texto);
            return textoHtml;
        }

        public static HtmlString CriarFormularioDeEdicaoParaEntidadeHTML<T>(this T entity, string actionName, string actionController, string method, string divClass) where T : ModeloBase
        {
            StringBuilder Sb = new StringBuilder();
            foreach (var prop in entity.GetType().GetProperties())
            {
                try
                {
                    if (!prop.IsADEDominio(entity))
                    {
                        if (!prop.IsKey() && !prop.IsDateTime() && !prop.IsGenericCollection() && !prop.IsByteArray())
                        {
                            string InputDisplayDiv = string.Empty;
                            if (prop.Name.ToString().Contains("Color"))
                            {
                                InputDisplayDiv = CreateColorDisplay(prop, entity, divClass);
                            }
                            else if (prop.Name.ToString().Contains("Logo"))
                            {
                                InputDisplayDiv = CreateInputDisplay(prop, entity, divClass, "file");
                            }
                            else
                            {
                                InputDisplayDiv = CreateInputDisplay(prop,entity, divClass, "text");
                            }
                            Sb.AppendLine(InputDisplayDiv);
                        }
                        else if(prop.IsKey())
                        {
                            string KeyInput = CreateInputForKey(prop, entity);
                            Sb.AppendLine(KeyInput);
                        }
                    }
                }
                catch (Exception) { continue; }
            }
            Sb.Append(CreateCampoMensagemAlteracao(entity,divClass));
            Sb.Append(CreateInput("Salvar", "btn btn-blue", "submit"));
            string texto = Sb.ToString();
            string URL = Path.Combine(actionController, actionName);
            string FormProperties = CreateFormProperties(URL, method) + CreateHTMLProperty("enctype","multipart/form-data");
            texto = AninharEmForm(texto, FormProperties);
            HtmlString textoHtml = new HtmlString(texto);
            return textoHtml;
        }
        public static HtmlString CreateCampoMensagemAlteracao<T>(this T entity, string divClass) where T : ModeloBase
        {
            HtmlString htmlString = new HtmlString(CreateInputDisplay("Mensagem", string.Empty, "Insira uma descrição para a alteração", divClass, "text"));
            return htmlString;
        }
        public static string MensagemDePropriedadesComPlaceholderHTML<T>(this T entity, string divClass = null) where T : ModeloBase
        {
            StringBuilder Sb = new StringBuilder();
            foreach (var prop in entity.GetType().GetProperties())
            {
                try
                {
                    Sb.AppendLine(string.Format(" {0} {1} ", AninharEmPlaceholderDeLabel(prop.Name), AninharEmPlaceholderDeParagrafo(prop.GetValue(entity, null).ToString())));
                }
                catch (Exception) { continue; }
            }
            return AninharEmPlaceholderDeDiv(Sb.ToString());
        }

        private static string CreateInput(string value, string classe, string type)
        {
            List<KeyValuePair<string, string>> InputProperties = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("class", classe),
                new KeyValuePair<string, string>("value", value),
                new KeyValuePair<string, string>("type", type)
            };
            string Propriedades = CreateHTMLProperties(InputProperties);
            return HTMLElements.Input(Propriedades);
        }

        private static string CreateInputForKey<T>(PropertyInfo prop, T entity) where T : ModeloBase
        {
            string ValorAtributo = ObterValorPropriedade(prop, entity);
            List<KeyValuePair<string, string>> Properties = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("id", $"{prop.Name}"),
                new KeyValuePair<string, string>("name", $"{prop.Name}"),
                new KeyValuePair<string, string>("value", $"{ValorAtributo}"),
                new KeyValuePair<string, string>("type", $"hidden")
            };
            string Propriedades = CreateHTMLProperties(Properties);
            return HTMLElements.Input(Propriedades);
        }

        private static string CreateInputDisplay<T>(PropertyInfo prop , T entity, string divClass, string inputType) where T:ModeloBase
        {
            string NomeAtributo = ObterNomePropriedade(prop);
            string ValorAtributo = ObterValorPropriedade(prop, entity);
            string DescricaoAtributo = ObterDescricaoPropriedade(prop);
            List<KeyValuePair<string, string>> InputProperties = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("id", $"{prop.Name}"),
                new KeyValuePair<string, string>("name", $"{prop.Name}"),
                new KeyValuePair<string, string>("value", $"{ValorAtributo}"),
                new KeyValuePair<string, string>("class", $"form-control"),
                new KeyValuePair<string, string>("placeholder", $"Insira {NomeAtributo}"),
                new KeyValuePair<string, string>("type", $"{inputType}"),
                new KeyValuePair<string, string>("title", $"{DescricaoAtributo}")
            };
            string LabelFor = CreateHTMLProperty("for", prop.Name);
            string InputPropriedades = CreateHTMLProperties(InputProperties);
            string InputDisplayDiv = AninharEmDiv(string.Format(" {1} {0} ", AninharEmLabel(NomeAtributo, LabelFor), HTMLElements.Input(InputPropriedades)), CreateHTMLProperty("class", divClass));
            return InputDisplayDiv;
        }

        private static string CreateInputDisplay(string NomePropriedade, string value, string Descricao, string divClass, string inputType)
        {
            List<KeyValuePair<string, string>> InputProperties = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("id", $"{NomePropriedade}"),
                new KeyValuePair<string, string>("name", $"{NomePropriedade}"),
                new KeyValuePair<string, string>("value", $"{value}"),
                new KeyValuePair<string, string>("class", $"form-control"),
                new KeyValuePair<string, string>("placeholder", $"{Descricao}"),
                new KeyValuePair<string, string>("type", $"{inputType}"),
                new KeyValuePair<string, string>("title", $"{Descricao}")
            };
            string LabelFor = CreateHTMLProperty("for", NomePropriedade);
            string InputPropriedades = CreateHTMLProperties(InputProperties);
            string InputDisplayDiv = AninharEmDiv(string.Format(" {0}: {1} ", AninharEmLabel(NomePropriedade, LabelFor), HTMLElements.Input(InputPropriedades)), CreateHTMLProperty("class", divClass));
            return InputDisplayDiv;
        }

        private static string CreateColorDisplay<T>(PropertyInfo prop, T entity, string divClass) where T : ModeloBase
        {
            string NomeAtributo = ObterNomePropriedade(prop);
            string ValorAtributo = ObterValorPropriedade(prop, entity);
            string DescricaoAtributo = ObterDescricaoPropriedade(prop);
            List<KeyValuePair<string, string>> InputProperties = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("id", $"{prop.Name}"),
                new KeyValuePair<string, string>("name", $"{prop.Name}"),
                new KeyValuePair<string, string>("class", $"form-control"),
                new KeyValuePair<string, string>("value", $"{ValorAtributo}"),
                new KeyValuePair<string, string>("placeholder", $"{NomeAtributo}"),
                new KeyValuePair<string, string>("type", $"color"),
                new KeyValuePair<string, string>("title", $"{DescricaoAtributo}")
            };
            string LabelFor = CreateHTMLProperty("for", prop.Name);
            string InputPropriedades = CreateHTMLProperties(InputProperties);
            string InputDisplayDiv = AninharEmDiv(string.Format(" {0}: {1} ", AninharEmLabel(NomeAtributo, LabelFor), HTMLElements.Input(InputPropriedades)));
            return InputDisplayDiv;
        }

        private static string ObterValorPropriedade<T>(PropertyInfo prop, T entity) where T : ModeloBase
        {
            try
            {
                string ValorAtributo = string.Empty;
                string type = prop.PropertyType.Name;
                if (true == type.Contains("Enum"))
                {
                    ValorAtributo = Enum.GetName(prop.PropertyType, prop.GetValue(entity, null));
                }
                else
                {
                    ValorAtributo = prop.GetValue(entity, null).ToString();
                }
                return ValorAtributo;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string ObterNomePropriedade(PropertyInfo prop)
        {
            try
            {
                var DisplayAttr = prop.CustomAttributes.Where(x => x.AttributeType.Name == "DisplayAttribute").Take(1);
                string NomeAtributo = String.Empty;
                if (DisplayAttr != null)
                    NomeAtributo = DisplayAttr.FirstOrDefault().NamedArguments.FirstOrDefault().TypedValue.Value.ToString();
                else
                    NomeAtributo = prop.Name;
                return NomeAtributo;
            }
            catch (Exception)
            {
                return prop.Name;
            }
        }

        private static string ObterDescricaoPropriedade(PropertyInfo prop)
        {
            try
            {
                var descriptionAttribute = prop.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                string NomeAtributo = String.Empty;
                if (descriptionAttribute != null)
                    NomeAtributo = descriptionAttribute.Description;
                else
                    NomeAtributo = prop.Name;
                return NomeAtributo;
            }
            catch (Exception)
            {
                return prop.Name;
            }
        }
    }
}
