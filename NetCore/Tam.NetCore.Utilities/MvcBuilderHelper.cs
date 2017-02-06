using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace Tam.NetCore.Utilities
{
    public static class MvcBuilderHelper
    {
        public static void AddMvcCamelCasePropertyNames(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(o => o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
        }
    }
}
