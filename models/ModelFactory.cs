using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using Builtin.DateTimeV2;
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

        public static T CreateModel<T>(LuisResult result) where T : new()
        {
            var typeString = typeof(T).FullName;

            var type = _types.ContainsKey(typeString) ? _types[typeString] : null;

            if (type == null)
                throw new Exception($"Type {typeString} not registered");

            var model = new T();

            // each value in the Resolution dictionary is an IList<Dictionary<string, object>>

            var entity = result.Entities.FirstOrDefault(e => e.Type == typeString);
            if (entity == null)
                return default(T);

            var skip = entity.Resolution.Values.Count -1;
            // if there are two entries for the resolution, the second one (future) will be the one we want
            var props = (entity.Resolution.Values.Skip(skip).First() as IList<Dictionary<string, object>>)?.First();

            if (props == null)
                return default(T);

            var properties = type.GetProperties();

            var typeName = type.FullName;

            Console.WriteLine($"typeName: {typeName}");

            foreach (var prop in properties)
            {
                if (props.ContainsKey(prop.Name.ToLower()))
                {
                    prop.SetValue(model, props[prop.Name.ToLower()]);
                }
            }

            return model;
        }
    }
}