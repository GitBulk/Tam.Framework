using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tam.WebApi.Extension
{
    public interface IBaseRestfulHttpClient<T, TResourceIdentifier> where T : class
    {
        string UserAgent { get; set; }
        Task DeleteAsync(TResourceIdentifier identifier);
        Task<T> GetAsync(TResourceIdentifier identifier);
        Task<IEnumerable<T>> GetManyAsync();
        Task<T> PostAsync(T model);
        Task PutAsync(TResourceIdentifier identifier, T model);
    }
}