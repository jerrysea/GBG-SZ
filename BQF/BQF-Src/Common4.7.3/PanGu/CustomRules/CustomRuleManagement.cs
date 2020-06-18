using SingleDigitRule;
using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu
{
    public class CustomRuleManagement
    {
        private List<ICustomRule> lstRules;
        
        public CustomRuleManagement(string txt)
        {
            Text = txt;
            lstRules = new List<ICustomRule>();
            lstRules.Add(new SingleDigit());
        }

        public string Text { get; set; }

        public void ExecuteRules(SuperLinkedList<WordInfo> result)
        {
            foreach (ICustomRule Rule in lstRules)
            {
                Rule.Text = Text;
                Rule.AfterSegment(result);
            }
        }
    }
}
