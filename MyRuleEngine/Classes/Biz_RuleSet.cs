using MyRuleEngine.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRuleEngine
{
    public class Biz_RuleSet
    {
        public event EventHandler<ActionInfoArg> ActionEvent;
        RuleSet dbRulesSet { set; get; }
        public Biz_RuleSet(string ruleSetName)
        {
            using (var context = new MyRuleEngineEntities())
            {
                var ruleSet = context.RuleSet.Where(x => x.Name == ruleSetName).FirstOrDefault();
                if (ruleSet != null)
                {
                    dbRulesSet = ruleSet;
                    if (dbRulesSet.RuleSet_Rule.Count == 0)
                    {
                        throw (new Exception("There in no rule defined in RuleSet!"));

                    }
                    GenerateBizRuleSetFromDBRuleset();
                }
                else
                    throw (new Exception("RuleSet '"+ruleSetName+"' in not defined!"));
            }
        }

        private void GenerateBizRuleSetFromDBRuleset()
        {
            foreach (var dbRule in dbRulesSet.RuleSet_Rule)
            {

                var dbCondition = dbRule.Rule.Condition;
                var conditionInstance = ReflectionHelper.GetClassInstance(dbCondition.AssemblyInfo.Name, dbCondition.AssemblyInfo.Path, dbCondition.ClassName);

                var dbAction = dbRule.Rule.Action;
                var actionInstance = ReflectionHelper.GetClassInstance(dbAction.AssemblyInfo.Name, dbAction.AssemblyInfo.Path, dbAction.ClassName);

                if (conditionInstance != null)
                {
                    if (actionInstance != null)
                    {

                        if (ReflectionHelper.ImplementsInterface(conditionInstance, typeof(ICondition)))
                        {
                            if (ReflectionHelper.ImplementsInterface(actionInstance, typeof(IAction)))
                            {
                                Biz_Rule rule = new Biz_Rule();
                                rule.Condition = (ICondition)conditionInstance;
                                rule.Action = (IAction)actionInstance;
                                rule.Action.ActionEvent += Action_ActionEvent;
                                if (Biz_Rules == null)
                                    Biz_Rules = new List<Biz_Rule>();
                                Biz_Rules.Add(rule);
                            }
                            else
                                throw (new Exception("Action class '" + dbAction.ClassName + "' is not of type IAction"));
                        }
                        else
                            throw (new Exception("Condition class '" + dbAction.ClassName + "' is not of type ICondition"));
                    }
                    else
                        throw (new Exception("Action class '" + dbAction.ClassName + "' colud not be found in assembly '" + dbAction.AssemblyInfo.Name + "'"));
                }
                else
                    throw (new Exception("Condition class '" + dbCondition.ClassName + "' colud not be found in assembly '" + dbCondition.AssemblyInfo.Name + "'"));


            }
        }

        void Action_ActionEvent(object sender, ActionInfoArg e)
        {
            if (ActionEvent != null)
                ActionEvent(sender, e);
        }


        public Biz_RuleSet(Biz_Rule rule)
        {
            Biz_Rules = new List<Biz_Rule>();
            Biz_Rules.Add(rule);
        }
        public Biz_RuleSet(List<Biz_Rule> rules)
        {
            Biz_Rules = rules;
        }
        public List<Biz_Rule> Biz_Rules { set; get; }

        public ActionResultEnum Execute(params object[] objects)
        {
            foreach (var rule in Biz_Rules)
            {
                if (rule.Condition.Evaluate(objects))
                {
                    return rule.Action.Execute(objects);
                }
            }
            return ActionResultEnum.Failed;
        }
    }
}
