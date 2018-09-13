using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Builtin.DateTimeV2;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Sample.LuisBot;

namespace LuisBot.models
{
    public static class ModelFactory
    {
        private static Dictionary<string, Type> _types;

        static ModelFactory()
        {
            RegisterTypes();
        }

        public static void RegisterTypes()
        {
            _types = new Dictionary<string, Type>();
            RegisterType(typeof(DateRange));
            RegisterType(typeof(Date));
        }

        private static void RegisterType(Type t) => _types.Add(t.FullName.ToLower(), t);

        public static async Task<T> CreateModel<T>(IDialogContext context, LuisResult result) where T : new()
        {
            var typeString = typeof(T).FullName.ToLower();

            var type = null as Type;
            if (_types.ContainsKey(typeString))
                type = _types[typeString];

            if (type == null)
            {
                throw new Exception($"Type {typeString} not registered");
            }

            var model = new T();

            var entity = result.Entities.FirstOrDefault(e => e.Type.Equals(typeString, StringComparison.OrdinalIgnoreCase));

            if (entity == null)
            {
                return default(T);
            }

            var skip = entity.Resolution.Values.Count - 1;
            // if there are two entries for the resolution, the second one (future) will be the one we want
            var props = (entity.Resolution.Values.Skip(skip).First() as IList<object>)?.First() as Dictionary<string, object>;

            if (props == null)
                return default(T);

            var properties = type.GetProperties();

            var typeName = type.FullName;

            foreach (var prop in properties)
            {
                if (props.ContainsKey(prop.Name.ToLower()))
                {
                    if (prop.PropertyType == typeof(DateTime))
                    {
                        var date = DateTime.Parse(props[prop.Name.ToLower()].ToString());
                        prop.SetValue(model, date);
                    }
                    else
                        prop.SetValue(model, props[prop.Name.ToLower()]);
                }
            }

            return model;
        }
    }
}