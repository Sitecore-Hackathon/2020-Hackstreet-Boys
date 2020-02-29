using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ExperienceForms.Models;  
using System.Reflection;

namespace Hackathon.Feature.Forms.Helper
{
    public static class SubmitActionHelper
    {
        public static string GetFieldValue(this IList<IViewModel> fields, string name)
        {
            var field = fields.FirstOrDefault(f => name.Equals(f.Name, StringComparison.OrdinalIgnoreCase));
            if (field == null)
                return null;

            PropertyInfo property = field.GetType().GetProperty("Value");
            if (property == null)
                return null;

            return property.GetValue(field) as string;
        }
    }
}