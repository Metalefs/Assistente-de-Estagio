namespace ADE.Utilidades.Constants
{
    public class HTMLPlaceholders
    {
        public static string Div(bool end) => !end ? "_@D_" : "_/@D_";
        public static string P(bool end) => !end ? "_@P_" : "_/@P_";
        public static string Label(bool end) => !end ? "_@L_" : "_/@L_";
    }
}
