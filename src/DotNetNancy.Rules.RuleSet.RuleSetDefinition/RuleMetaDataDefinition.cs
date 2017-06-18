using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class RuleSetMetaDataDefinition
    {
        bool _executeActionsInRule = false;

        /// <summary>
        /// by default unless otherwise specified do not execute the action methods in the rules processing
        /// for Console we do a Post Processing meaning once the condition has evaluated then those methods that are to be executed
        /// are going to be executed in a Post Processing step in some particular workflow for example
        /// </summary>
        public bool ExecuteActionsInRule
        {
            get { return _executeActionsInRule; }
            set { _executeActionsInRule = value; }
        }

        RuleFields _ruleFields = null;

        public RuleFields RuleFieldsProperty
        {
            get { return _ruleFields; }
            set { _ruleFields = value; }
        }
        RuleOperators _ruleOperators = null;

        public RuleOperators RuleOperatorsProperty
        {
            get { return _ruleOperators; }
            set { _ruleOperators = value; }
        }

        public RuleSetMetaDataDefinition(bool executeActionsInRule, Guid applicationID,Guid typeID)
        {
            _ruleFields = new RuleFields(applicationID, typeID,DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes.FLDS);
            _ruleOperators = new RuleOperators(applicationID, typeID,DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes.OPRS);
            _executeActionsInRule = executeActionsInRule;
        }


        public RuleSetMetaDataDefinition(Guid applicationID, Guid typeID)
        {
            _ruleFields = new RuleFields(applicationID, typeID, DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes.FLDS);
            _ruleOperators = new RuleOperators(applicationID, typeID, DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes.OPRS);            
        }
    }
}
