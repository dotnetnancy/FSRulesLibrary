using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.CodeDom;


namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class AssignPropertyAction : RuleStatementAction
    {
        public AssignPropertyAction()
        {
        }

        public AssignPropertyAction(CodePropertyReferenceExpression propertyToSet,CodeExpression expressionValue):
            base()
        {
            CodeAssignStatement codeAssignStatement = new CodeAssignStatement(propertyToSet, expressionValue);
            base.CodeDomStatement = codeAssignStatement;            
        }

    }
}
