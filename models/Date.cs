using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuisBot.models;

namespace Builtin.DateTimeV2
{
    public class Date : ModelBase
    {
        public string TimeX { get; set; }
        public string Type { get; set; }
        public DateTime Value { get; set; }
    }
}