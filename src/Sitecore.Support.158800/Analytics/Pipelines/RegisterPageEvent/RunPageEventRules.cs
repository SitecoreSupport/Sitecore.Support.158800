using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Sitecore.Analytics.Data.Items;
using Sitecore.Analytics.Rules.PageEvents;
using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Analytics.Pipelines.RegisterPageEvent;
using Sitecore.Resources.Media;
using Sitecore.Analytics;

namespace Sitecore.Support.Analytics.Pipelines.RegisterPageEvent
{
    public class RunPageEventRules : RegisterPageEventProcessor
    {
        public override void Process(RegisterPageEventArgs args)
        {
            Assert.ArgumentNotNull(args, "args");

            if (MediaManager.IsMediaUrl(Context.RawUrl))
            {
                if ((Tracker.Current.Interaction.PreviousPage != null) && (args.PageEvent != null))
                {
                    if (Tracker.Current.Interaction.PreviousPage.PageEvents.Contains(args.PageEvent))
                    {
                        return;
                    }
                }
            }

            Field field = args.Definition.InnerItem.Fields[PageEventItem.FieldIDs.Rule];
            RuleList<PageEventRuleContext> rules = RuleFactory.GetRules<PageEventRuleContext>(field);
            if (rules == null)
            {
                return;
            }

            PageEventRuleContext ruleContext = new PageEventRuleContext
            {
                Item = Context.Item,
                PageEvent = args.PageEvent,
                PageEventDefinitionItem = args.Definition
            };
            rules.Run(ruleContext);
        }
    }
}
