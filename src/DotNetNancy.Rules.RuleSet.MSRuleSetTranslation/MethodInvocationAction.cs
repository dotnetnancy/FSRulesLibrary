using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities.Rules;
using System.CodeDom;


namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class MethodInvocationAction : RuleStatementAction
    {
        public MethodInvocationAction()
        {
        }

        public MethodInvocationAction( CodeExpression targetObject, CodeMethodReferenceExpression codeMethodReferenceExpression, CodeExpression [] parameters) :
            base(GetMethodInvocationExpression(targetObject,codeMethodReferenceExpression,parameters))
        {
        }

        public static CodeExpression GetMethodInvocationExpression(CodeExpression targetObject,
            CodeMethodReferenceExpression codeMethodReferenceExpression, 
            CodeExpression [] parameters)
        {
            return  new CodeMethodInvokeExpression(targetObject,codeMethodReferenceExpression.MethodName,parameters);
        }

    }
}
