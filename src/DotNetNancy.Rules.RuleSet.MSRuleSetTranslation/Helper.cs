using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.ComponentModel.Compiler;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public static class Helper
    {
        public static string GetErrorMessageFromValidationErrorsCollection(ValidationErrorCollection errors)
        {
            string errorMessage = string.Empty;

            if (errors.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ValidationError error in errors)
                {
                    sb.Append("Error:  ");
                    sb.Append(error.ErrorText);
                    sb.Append(Environment.NewLine);
                }

                errorMessage = sb.ToString();
            }

            return errorMessage;
        }
        
    }
}
