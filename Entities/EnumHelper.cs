using System;
using System.ComponentModel;
using System.Reflection;

namespace SeCoGEST.Entities
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            // Ottiene il tipo dell'enumeratore
            Type enumType = value.GetType();

            // Ottiene il FieldInfo relativo al valore passato
            MemberInfo[] memberInfo = enumType.GetMember(value.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                // Cerca un attributo Description su quel campo
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            // Se non c'è Description, restituisce il nome dell'enum
            return value.ToString();
        }
    }
}