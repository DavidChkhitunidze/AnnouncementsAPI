using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AnnouncementsAPI.Services
{
    public class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
                return true;

            var splitedFields = fields.Split(',');

            foreach (var field in splitedFields)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    return false;
            }

            return true;
        }
    }
}
