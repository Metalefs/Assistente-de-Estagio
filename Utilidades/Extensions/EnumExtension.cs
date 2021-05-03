using System;
using System.ComponentModel;using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace ADE.Utilidades.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static string ObterNomeEnum<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var DisplayAttr = memInfo[0]
                            .GetCustomAttributes(typeof(DisplayNameAttribute), false)
                            .FirstOrDefault() as DisplayNameAttribute;

                        if (DisplayAttr != null)
                        {
                            return DisplayAttr.DisplayName;
                        }
                    }
                }
            }
            return string.Empty;
        }

    }
}
