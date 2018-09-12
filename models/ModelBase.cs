using System;
using System.Collections.Generic;

namespace LuisBot.models
{
    public class ModelBase
    {
        public override string ToString()
        {
            var properties = GetType().GetProperties();

            var result = new List<string>();
            foreach (var prop in properties)
            {
                if (prop.CanRead)
                    result.Add($"{prop.Name}: {prop.GetValue(this)}");
            }

            return String.Join(@"\n", result);
        }
    }
}