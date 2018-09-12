using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LuisBot.models;

namespace Builtin.DateTimeV2
{
    public class DateRange : ModelBase
    {
        public string TimeX { get; set; }
        public string Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}