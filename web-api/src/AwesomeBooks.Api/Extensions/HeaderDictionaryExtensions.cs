using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;

namespace AwesomeBooks.Api.Extensions
{
    public static class HeaderDictionaryExtensions
    {
        public static string AsString(this IHeaderDictionary headerDictionary)
        {
            var filteredHeaders = headerDictionary.Where(kvp => kvp.Key != "Authorization");
            return JsonConvert.SerializeObject(filteredHeaders, Formatting.Indented);
        }
    }
}
