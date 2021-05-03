using ADE.Dominio.Models;
using System;
using System.Linq;
using System.Reflection;

namespace ADE.Utilidades.Extensions
{
    public static class PropertyInfoExtension
    {
        public static bool IsADEDominio<T>(this PropertyInfo prop, T entity) where T : ModeloBase
        {
            try
            {
                string value = prop.GetValue(entity, null).ToString();
                return value.Contains("ADE.Dominio");
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsKey(this PropertyInfo prop) => prop.CustomAttributes.Any(x => x.AttributeType.Name == "KeyAttribute");
        
        public static bool IsDateTime(this PropertyInfo prop) => prop.PropertyType.AssemblyQualifiedName.Contains("DateTime");

        public static bool IsByteArray(this PropertyInfo prop) => prop.PropertyType.AssemblyQualifiedName.Contains("Byte[]");

        public static bool IsGenericCollection(this PropertyInfo prop) => prop.PropertyType.AssemblyQualifiedName.Contains("System.Collections.Generic.ICollection");

    }
}
