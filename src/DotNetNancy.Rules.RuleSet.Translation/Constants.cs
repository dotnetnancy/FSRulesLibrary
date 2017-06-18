using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.Translation
{
    public static class Constants
    {
        public static class InvocationTypeMethodNameConstants
        {
            public const string ANY_ONE_DYNAMIC = "AnyOneDynamic";
            public const string ALL_DYNAMIC = "AllDynamic";
        }

        public static class XmlRuleSetDefinitionConstants
        {
            public const string RULE = "rule";
            public const string ALL_PARENTHESES_PATH = "//parentheses";
            public const string ALL_TOP_LEVEL_PARENTHESES_PATH_UNDER_RULE = "//rule/parentheses";
            public const string PARENTHESES_PATH = "/parentheses";
            public const string RULE_PATH = "//DotNetNancy/rule";

            public const string FIELD = "field";
            public const string CLAUSE = "clause";
            public const string TYPE = "type";
            public const string PARENTHESES = "parentheses";
            public const string VALUE = "value";
            public const string OPERATOR = "operator";

            public const string DATA_TYPE = "dataType";
            public const string PROPERTY_NAME = "propertyName";
            public const string LEVEL = "level";
            public const string SEQUENCE = "sequence";
            public const string NAME = "name";
            public const string SQUARE_BRACKET_GROUPING = "grouping";


            
        }
        public static class XmlRuleElementConstants
        {
            public const string DISPLAY_NAME = "displayName";
            public const string METHOD_NAME = "methodName";
            public const string NAME = "name";
            public const string VALUE = "value";
            public const string PROPERTY_NAME = "propertyName";
            public const string DATA_TYPE = "dataType";
            public const string MAX_LENGTH = "maxLength";
            public const string SET = "set";
            public const string COLLECTION_TYPE = "collectionType";
            public const string KEY = "key";
            public const string MEMBER_OF_COLLECTION = "memberOfCollection";
            public const string CUSTOM_FIELD_NAME = "customFieldName";
            public const string CUSTOM_FIELD_VALUE = "customFieldValue";
            public const string NUMBER = "number";

            public const string ACTION_PATH = "/DotNetNancy/actions";
            public const string CLAUSE_PATH = "/DotNetNancy/clauses";
            public const string FIELD_PATH = "/DotNetNancy/fields";
            public const string OPERATOR_PATH = "/DotNetNancy/operators";
            public const string INVOCATION_TYPE = "invocationType";
        }

        public static class XmlCollectionTypeConstants
        {
            public const string IDICTIONARY = "IDictionary";
            public const string IENUMERABLE = "IEnumerable";
        }

        public static class XmlInvocationTypeConstants
        {
            public const string AS_PROPERTY = "AsProperty";
            public const string AS_METHOD_WITH_DEFINED_VALUE_AS_A_PARAMETER = "AsMethodPassDefinedValueAsParameter";
            public const string AS_ANY_ONE_DYNAMIC = "AsAnyOneDynamic";
            public const string AS_ALL_DYNAMIC = "AsAllDynamic";
        }

        public static class XmlSetTypeConstants
        {
            public const string STRING = "string";
            public const string NUMERIC = "numeric";
            public const string DATE = "date";
            public const string TIME = "time";
            public const string BOOL = "bool";
            public const string ENUM = "enum";
        }

        public static class XmlClauseTypeConstants
        {
            public const string AND = "and";
            public const string OR = "or";
            public const string THEN = "then";
        }

        public static class XmlOperatorTypeConstants
        {
            public const string IS = "is";
            public const string NOT = "not";
            public const string CONTAINS = "contains";
            public const string NOCONTAINS = "nocontains";
            public const string STARTS = "starts";
            public const string NOSTARTS = "nostarts";
            public const string ENDS = "ends";
            public const string NOENDS = "noends";
            public const string LESS = "less";
            public const string LESSIS = "lessis";
            public const string MORE = "more";
            public const string MOREIS = "moreis";
            public const string IN = "in";
            public const string NOTIN = "notin";
       
        }

        public static class CustomLoggers
        {
            public const string DEFAULT_LOGGER = "DefaultLogger";
            public const string RULE_VALIDATION_ERROR_LOGGER = "RuleValidationLogger";
            public const string RULE_RUNTIME_EXCEPTION_LOGGER = "RuleRuntimeExceptionLogger";
        }

        
    }
}
