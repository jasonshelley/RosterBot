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

        public static async Task<T> CreateModel<T>(IDialogContext context, LuisResult result) where T : class, new()
        {
            var typeString = typeof(T).FullName.ToLower();

            var type = null as Type;
            if (_types.ContainsKey(typeString))
                type = _types[typeString];

            if (type == null)
            {
                throw new Exception($"Type {typeString} not registered");
            }

            var entity = result.Entities.FirstOrDefault(e => e.Type.Equals(typeString, StringComparison.OrdinalIgnoreCase));

            if (entity == null)
            {
                return default(T);
            }

            var models = new List<T>();

            foreach (var res in entity.Resolution.Values)
            {
                var model = new T();

                // if there are two entries for the resolution, the second one (future) will be the one we want
                var reslist = res as IList<object>;
                foreach (var instances in reslist) { 
                var props = (res as IList<object>)?.First() as
                    Dictionary<string, object>;

                if (props == null)
                    return default(T);

                var properties = type.GetProperties();

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
                models.Add(model);
            }

            var createdModel = null as T;

            if (models.Count == 1)
               createdModel = models[0];
            else
            {
                if (type == typeof(Date))
                {
                    createdModel = models.Cast<Date>().OrderByDescending(d => d.Value).First() as T;
                }
                else if (type == typeof(DateRange))
                {
                    createdModel = models.Cast<DateRange>().OrderByDescending(d => d.Start).First() as T;
                }
            }


            return createdModel;
        }
    }
}