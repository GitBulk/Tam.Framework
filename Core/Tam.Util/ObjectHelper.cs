using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tam.Util
{
    public static class ObjectHelper
    {
        /// <summary>
        /// Convert an object to a dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ConvertToDictionary(object obj)
        {
            var propertyDictionary = new Dictionary<string, object>();
            Type type = obj.GetType();
            IEnumerable<PropertyInfo> propertyInfos = type.GetRuntimeProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                string name = propertyInfo.Name;
                propertyDictionary[name] = type.GetRuntimeProperty(name).GetValue(obj);
            }
            return propertyDictionary;
        }
    }
}
