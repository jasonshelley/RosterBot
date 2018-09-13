using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Builtin.DateTimeV2;
using Chronic;
using LuisBot.models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        private static Dictionary<DateTime, string> _roster = new Dictionary<DateTime, string>() {
            {   new DateTime(   2018    ,   9   ,   17  )   ,   "Matt"    }   ,
            {   new DateTime(   2018    ,   9   ,   24  )   ,   "Michael" }   ,
            {   new DateTime(   2018    ,   10  ,   1   )   ,   "Doug"    }   ,
            {   new DateTime(   2018    ,   10  ,   8   )   ,   "Will"    }   ,
            {   new DateTime(   2018    ,   10  ,   15  )   ,   "Hazz"    }   ,
            {   new DateTime(   2018    ,   10  ,   22  )   ,   "Jay" }   ,
            {   new DateTime(   2018    ,   10  ,   29  )   ,   "Jase"    }   ,
            {   new DateTime(   2018    ,   11  ,   5   )   ,   "Rick"    }   ,
            {   new DateTime(   2018    ,   11  ,   12  )   ,   "Ben T"   }   ,
            {   new DateTime(   2018    ,   11  ,   19  )   ,   "Nick"    }   ,
            {   new DateTime(   2018    ,   11  ,   26  )   ,   "Tom" }   ,
            {   new DateTime(   2018    ,   12  ,   3   )   ,   "Scott"   }   ,
            {   new DateTime(   2018    ,   12  ,   10  )   ,   "PK"  }   ,
            {   new DateTime(   2018    ,   12  ,   17  )   ,   "Ben R"   }   ,
            {   new DateTime(   2019    ,   1   ,   7   )   ,   "Gaz" }   ,
            {   new DateTime(   2019    ,   1   ,   14  )   ,   "Matt"    }   ,
            {   new DateTime(   2019    ,   1   ,   21  )   ,   "Michael" }   ,
            {   new DateTime(   2019    ,   1   ,   28  )   ,   "Doug"    }   ,
            {   new DateTime(   2019    ,   2   ,   4   )   ,   "Will"    }   ,
            {   new DateTime(   2019    ,   2   ,   11  )   ,   "Hazz"    }   ,
            {   new DateTime(   2019    ,   2   ,   18  )   ,   "Jay" }   ,
            {   new DateTime(   2019    ,   2   ,   25  )   ,   "Jase"    }   ,
            {   new DateTime(   2019    ,   3   ,   4   )   ,   "Rick"    }   ,
            {   new DateTime(   2019    ,   3   ,   11  )   ,   "Ben T"   }   ,
            {   new DateTime(   2019    ,   3   ,   18  )   ,   "Nick"    }   ,
            {   new DateTime(   2019    ,   3   ,   25  )   ,   "Tom" }   ,
            {   new DateTime(   2019    ,   4   ,   1   )   ,   "Scott"   }   ,
            {   new DateTime(   2019    ,   4   ,   8   )   ,   "PK"  }   ,
            {   new DateTime(   2019    ,   4   ,   15  )   ,   "Ben R"   }   ,
            {   new DateTime(   2019    ,   4   ,   22  )   ,   "Gaz" }   ,
            {   new DateTime(   2019    ,   4   ,   29  )   ,   "Matt"    }   ,
            {   new DateTime(   2019    ,   5   ,   6   )   ,   "Michael" }   ,
            {   new DateTime(   2019    ,   5   ,   13  )   ,   "Doug"    }   ,
            {   new DateTime(   2019    ,   5   ,   20  )   ,   "Will"    }   ,
            {   new DateTime(   2019    ,   5   ,   27  )   ,   "Hazz"    }   ,
            {   new DateTime(   2019    ,   6   ,   3   )   ,   "Jay" }   ,
            {   new DateTime(   2019    ,   6   ,   10  )   ,   "Jase"    }   ,
            {   new DateTime(   2019    ,   6   ,   17  )   ,   "Rick"    }   ,
            {   new DateTime(   2019    ,   6   ,   24  )   ,   "Ben T"   }   ,
            {   new DateTime(   2019    ,   7   ,   1   )   ,   "Nick"    }   ,
            {   new DateTime(   2019    ,   7   ,   8   )   ,   "Tom" }   ,
            {   new DateTime(   2019    ,   7   ,   15  )   ,   "Scott"   }   ,
            {   new DateTime(   2019    ,   7   ,   22  )   ,   "PK"  }   ,
            {   new DateTime(   2019    ,   7   ,   29  )   ,   "Ben R"   }   ,
            {   new DateTime(   2019    ,   8   ,   5   )   ,   "Gaz" }   ,
            {   new DateTime(   2019    ,   8   ,   12  )   ,   "Matt"    }   ,
            {   new DateTime(   2019    ,   8   ,   19  )   ,   "Michael	" }   ,
            {   new DateTime(   2019    ,   8   ,   26  )   ,   "Doug"    }   ,
            {   new DateTime(   2019    ,   9   ,   2   )   ,   "Will"    }   ,
            {   new DateTime(   2019    ,   9   ,   9   )   ,   "Hazz"    }
        };

        public Dictionary<string, string> _nickNames = new Dictionary<string, string>()
        {
            { "douglas", "Doug" },
            { "dougie", "Doug" },
            { "gary", "Gaz" },
            { "gazza", "Gaz" },
            { "harry", "Hazz" },
            { "hazza", "Hazz" },
            { "jason", "Jase" },
            { "matty", "Matt" },
            { "matthew", "Matt" },
            { "mick", "Michael" },
            { "paul", "PK" },
            { "mr kerry", "PK" },
            { "william", "Will" },
        };

        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }



        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Roster.When")]
        public async Task RosterWhenQuery(IDialogContext context, LuisResult result)
        {
            var complete = BuildCompleteList();

            var subject = GetEntityValue(result, "Subject").ToLower();
            if (_nickNames.ContainsKey(subject))
                subject = _nickNames[subject];

            var timeDirection = GetEntityValue(result, "TimeDirection");

            var now = DateTime.Now;

            if (timeDirection == null || timeDirection.Equals("next"))
            {
                var next = complete.FirstOrDefault(k =>
                    k.Key > now && k.Value.ToLower().Equals(subject, StringComparison.OrdinalIgnoreCase));
                if (next.Value == null)
                    await context.PostAsync($"{subject} doesn't appear to on the roster after {now}");
                else
                {
                    await context.PostAsync($"{next.Value} is up next on {next.Key: dddd MMM dd}");
                }
            }
            else if (timeDirection.Equals("last", StringComparison.OrdinalIgnoreCase))
            {
                var prev = complete.OrderByDescending(k => k.Key).FirstOrDefault(k => k.Key < now);

                if (prev.Value == null)
                    await context.PostAsync($"{subject} doesn't appear to be on the roster previously");
                else
                {
                    await context.PostAsync($"{prev.Value} did it last on {prev.Key: dddd MMM dd}");
                }

            }
        }


        [LuisIntent("Roster.WhosUp")]
        public async Task WhosUpIntent(IDialogContext context, LuisResult result)
        {
            var date = await ModelFactory.CreateModel<Date>(context, result);
            var dateRange = await ModelFactory.CreateModel<DateRange>(context, result);

            var complete = BuildCompleteList();

            if (date != null)
            {
                if (complete.ContainsKey(date.Value))
                {
                    await context.PostAsync($"{complete[date.Value]} will be doing it that day.");
                }
                else
                {
                    if (date.Value.DayOfWeek == DayOfWeek.Saturday || date.Value.DayOfWeek == DayOfWeek.Sunday)
                        await context.PostAsync($"That's a weekend.");
                    else
                        await context.PostAsync(
                        $"I'm sorry I haven't found anyone rostered on for that day ({date.Value: dddd yyyy-MMM-dd})");
                }
            }
            else if (dateRange != null)
            {
                var victims = complete.Where(c => c.Key >= dateRange.Start && c.Key < dateRange.End);
                var bob = new StringBuilder();
                bob.AppendLine($"Here are your volunteers for that time: ");
                victims.ForEach(v => bob.AppendLine($"{v.Key: dddd MMM-dd}: {v.Value}"));

                await context.PostAsync(bob.ToString());
            }
        }

        private string GetEntityValue(LuisResult result, string name) =>
            result.Entities.FirstOrDefault(e => e.Type.Equals(name, StringComparison.OrdinalIgnoreCase))?.Entity;

        private Dictionary<DateTime, string> BuildCompleteList()
        {
            var complete = new Dictionary<DateTime, string>();
            _roster.Keys.ForEach(k =>
            {
                for (int i = 0; i < 5; i++)
                {
                    complete.Add(k + TimeSpan.FromDays(i), _roster[k]);
                }
            });

            return complete;
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            var intent = result.TopScoringIntent;
            await context.PostAsync($"You have reached {intent.Intent}. You said: {result.Query}");
            foreach (var entity in result.Entities) {
                await context.PostAsync($"{entity.Entity}: {entity.Type}");
            }
            
            context.Wait(MessageReceived);
        }
    }
}