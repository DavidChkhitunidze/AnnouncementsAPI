using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Announcements.Helpers.Extensions
{
    public static class JsonExtensions
    {
        public static JsonResult JsonSerialize(this object obj) => new JsonResult(obj);

        public static JsonResult JsonSerialize(this object obj, JsonSerializerSettings jsonSerializerSettings) => new JsonResult(obj, jsonSerializerSettings);
    }
}
