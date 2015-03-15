using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Tam.Util
{
    public static class TypeExtension
    {
        public static bool HasDefaultConstructor(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes) != null;
        }
        public static string GetDescription(this Type type)
        {
            var description = type.GetCustomAttribute<DescriptionAttribute>();
            return description == null ? type.Name : description.Description;
        }
    }
}
