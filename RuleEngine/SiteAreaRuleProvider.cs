using System;
using System.Linq;
using Orchard;
using Orchard.Widgets.Services;

namespace ACME.Theme.Medical.RuleEngine {
    public class SiteAreaRuleProvider : IRuleProvider {
        private readonly WorkContext _workContext;

        public SiteAreaRuleProvider(
            WorkContext workContext) {
            _workContext = workContext;
        }

        public void Process(RuleContext ruleContext) {
            if (String.Equals(ruleContext.FunctionName, "sitearea", StringComparison.OrdinalIgnoreCase)) {
                var argument = ((object[]) ruleContext.Arguments)
                    .Cast<string>()
                    .Single();

                ruleContext.Result = _workContext.IsAreaRequest(argument);
                return;
            }

            ruleContext.Result = false;
        }
    }
}