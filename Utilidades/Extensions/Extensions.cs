using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace ADE.Utilidades.Extensions
{
    public static class GeneralExtensions
    {
        private static async Task<byte[]> ToByteArrayAsync(this FormFile formFile)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }
        private static async Task<byte[]> ToByteArrayAsync(this IFormFile formFile)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                await formFile.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        public static string ParseHistoricoDate(this DateTime datetime)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            switch (datetime.Month)
            {
                case 1:
                    sb.Append("Janeiro");
                    break;
                case 2:
                    sb.Append("Fevereiro");
                    break;
                case 3:
                    sb.Append("Março");
                    break;
                case 4:
                    sb.Append("Abril");
                    break;
                case 5:
                    sb.Append("Maio");
                    break;
                case 6:
                    sb.Append("Junho");
                    break;
                case 7:
                    sb.Append("Julho");
                    break;
                case 8:
                    sb.Append("Agosto");
                    break;
                case 9:
                    sb.Append("Setembro");
                    break;
                case 10:
                    sb.Append("Outubro");
                    break;
                case 11:
                    sb.Append("Novembro");
                    break;
                case 12:
                    sb.Append("Dezembro");
                    break;
            }
            sb.Append($", {datetime.Year}");
            return sb.ToString();
        }

        public static string ParseHistoricoDay(this DateTime datetime)
        {
            return $"{datetime.Day}/{datetime.Month.ToString().PadLeft(2,'0')} {datetime.ToString("dddd", new CultureInfo("pt-BR"))}";
        }
    }
}
