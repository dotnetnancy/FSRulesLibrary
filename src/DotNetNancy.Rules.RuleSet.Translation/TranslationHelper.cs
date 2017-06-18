using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.Translation
{
    public static class TranslationHelper
    {
        public  const char IN_DELIMITER = ',';


        public static CollectionTypes CollectionTypeTranslation(string collectionType)
        {
            switch (collectionType)
            {
                case Constants.XmlCollectionTypeConstants.IDICTIONARY:
                    {
                        return CollectionTypes.IDictionary;
                    }
                case Constants.XmlCollectionTypeConstants.IENUMERABLE:
                    {
                        return CollectionTypes.IEnumerable;
                    }
                default:
                    {
                        return CollectionTypes.NotSupported;
                    }
            }
        }

        public static InvocationTypes InvocationTypesTranslation(string invocationType)
        {
            switch (invocationType)
            {
                case Constants.XmlInvocationTypeConstants.AS_PROPERTY:
                    {
                        return InvocationTypes.AsProperty;
                    }
                case Constants.XmlInvocationTypeConstants.AS_METHOD_WITH_DEFINED_VALUE_AS_A_PARAMETER:
                    {
                        return InvocationTypes.AsMethodWithDefinedValueAsParameter;
                    }
                case Constants.XmlInvocationTypeConstants.AS_ANY_ONE_DYNAMIC:
                    {
                        return InvocationTypes.AsAnyOneDynamic;
                    }
                case Constants.XmlInvocationTypeConstants.AS_ALL_DYNAMIC:
                    {
                        return InvocationTypes.AsAllDynamic;
                    }
                default:
                    {
                        return InvocationTypes.NotSupported;
                    }
            }
        }

        public static SetTypes SetTypeTranslation(string set)
        {
            switch (set)
            {
                case Constants.XmlSetTypeConstants.STRING:
                    {
                        return SetTypes.String;
                    }
                case Constants.XmlSetTypeConstants.NUMERIC:
                    {
                        return SetTypes.Numeric;
                    }

                case Constants.XmlSetTypeConstants.DATE:
                    {
                        return SetTypes.Date;
                    }
                case Constants.XmlSetTypeConstants.TIME:
                    {
                        return SetTypes.Time;
                    }
                case Constants.XmlSetTypeConstants.BOOL:
                    {
                        return SetTypes.Bool;
                    }
                case Constants.XmlSetTypeConstants.ENUM:
                    {
                        return SetTypes.Enum;
                    }
                default:
                    {
                        return SetTypes.NotSupported;
                    }
            }
        }

        public static ClauseTypes ClauseTypeTranslation(string clauseValue)
        {
            switch (clauseValue)
            {
                case Constants.XmlClauseTypeConstants.AND:
                    {
                        return ClauseTypes.And;
                    }
                case Constants.XmlClauseTypeConstants.OR:
                    {
                        return ClauseTypes.Or;
                    }
                case Constants.XmlClauseTypeConstants.THEN:
                    {
                        return ClauseTypes.Then;
                    }

                default:
                    {
                        return ClauseTypes.NotSupported;
                    }
            }
        }

        public static string OperatorValueTranslation(OperatorTypes operatorType)
        {
            string returnVal = string.Empty;

            switch (operatorType)
            {
                case OperatorTypes.Contains:
                    {
                        return Constants.XmlOperatorTypeConstants.CONTAINS;
                    }
                case OperatorTypes.Ends:
                    {
                        return Constants.XmlOperatorTypeConstants.ENDS;
                    }
                case OperatorTypes.Is:
                    {
                        return Constants.XmlOperatorTypeConstants.IS;
                    }
                case OperatorTypes.Less:
                    {
                        return Constants.XmlOperatorTypeConstants.LESS;
                    }
                case OperatorTypes.LessIs:
                    {
                        return Constants.XmlOperatorTypeConstants.LESSIS;
                    }
                case OperatorTypes.More:
                    {
                        return Constants.XmlOperatorTypeConstants.MORE;
                    }
                case OperatorTypes.MoreIs:
                    {
                        return Constants.XmlOperatorTypeConstants.MOREIS;
                    }
                case OperatorTypes.NoContains:
                    {
                        return Constants.XmlOperatorTypeConstants.NOCONTAINS;
                    }
                case OperatorTypes.NoEnds:
                    {
                        return Constants.XmlOperatorTypeConstants.NOENDS;
                    }
                case OperatorTypes.NoStarts:
                    {
                        return Constants.XmlOperatorTypeConstants.NOSTARTS;
                    }
                case OperatorTypes.Not:
                    {
                        return Constants.XmlOperatorTypeConstants.NOT;
                    }
                case OperatorTypes.Starts:
                    {
                        return Constants.XmlOperatorTypeConstants.STARTS;
                    }
                case OperatorTypes.In:
                    {
                        return Constants.XmlOperatorTypeConstants.IN;
                    }
                case OperatorTypes.NotIn:
                    {
                        return Constants.XmlOperatorTypeConstants.NOTIN;
                    }
               
            }
            return returnVal;
        }

        public static OperatorTypes OperatorTypeTranslation(string operatorValue)
        {
            switch (operatorValue)
            {
                case Constants.XmlOperatorTypeConstants.CONTAINS:
                    {
                        return OperatorTypes.Contains;
                    }
                case Constants.XmlOperatorTypeConstants.ENDS:
                    {
                        return OperatorTypes.Ends;
                    }
                case Constants.XmlOperatorTypeConstants.IS:
                    {
                        return OperatorTypes.Is;
                    }
                case Constants.XmlOperatorTypeConstants.LESS:
                    {
                        return OperatorTypes.Less;
                    }
                case Constants.XmlOperatorTypeConstants.LESSIS:
                        {
                            return OperatorTypes.LessIs;
                        }
                case Constants.XmlOperatorTypeConstants.MORE:
                        {
                            return OperatorTypes.More;
                        }
                case Constants.XmlOperatorTypeConstants.MOREIS:
                        {
                            return OperatorTypes.MoreIs;
                        }
                case Constants.XmlOperatorTypeConstants.NOCONTAINS:
                        {
                            return OperatorTypes.NoContains;
                        }
                case Constants.XmlOperatorTypeConstants.NOENDS:
                        {
                            return OperatorTypes.NoEnds;
                        }
                case Constants.XmlOperatorTypeConstants.NOSTARTS:
                        {
                            return OperatorTypes.NoStarts;
                        }
                case Constants.XmlOperatorTypeConstants.NOT:
                        {
                            return OperatorTypes.Not;
                        }
                case Constants.XmlOperatorTypeConstants.STARTS:
                        {
                            return OperatorTypes.Starts;
                        }
                case Constants.XmlOperatorTypeConstants.IN:
                        {
                            return OperatorTypes.In;
                        }
                case Constants.XmlOperatorTypeConstants.NOTIN:
                        {
                            return OperatorTypes.NotIn;
                        }
                default:
                    {
                        return OperatorTypes.NotSupported;
                    }
            }
        }

        /// <summary>
        /// only the types of equality operators you can use with CompareTo
        /// </summary>
        /// <param name="operatorTypes"></param>
        /// <returns></returns>
        public static ComparisonType CompareToValidEqualityTypes(OperatorTypes operatorTypes)
        {
            switch (operatorTypes)
            {
                case OperatorTypes.Is:
                    {
                        return ComparisonType.Equal;
                    }
                case OperatorTypes.Less:
                    {
                        return ComparisonType.LessThan;
                    }
                case OperatorTypes.LessIs:
                    {
                        return ComparisonType.LessThanOrEqual;
                    }
                case OperatorTypes.More:
                    {
                        return ComparisonType.GreaterThan;
                    }
                case OperatorTypes.MoreIs:
                    {
                        return ComparisonType.GreaterThanOrEqual;
                    }
                case OperatorTypes.Not:
                    {
                        return ComparisonType.NotEqual;
                    }
                case OperatorTypes.In:
                    {
                        return ComparisonType.In;
                    }
                case OperatorTypes.NotIn:
                    {
                        return ComparisonType.NotIn;
                    }
                default:
                    {
                        return ComparisonType.NotSupported;
                    }
                   
            }
        }

        public static StringComparisonType StringComparisonValidTypes(OperatorTypes operatorTypes)
        {
            switch (operatorTypes)
            {
                case OperatorTypes.Contains:
                    {
                        return StringComparisonType.Contains;
                    }
                case OperatorTypes.NoContains:
                    {
                        return StringComparisonType.NotContains;
                    }
                case OperatorTypes.Ends:
                    {
                        return StringComparisonType.EndsWith;
                    }
                case OperatorTypes.NoEnds:
                    {
                        return StringComparisonType.NotEndsWith;
                    }
                case OperatorTypes.Starts:
                    {
                        return StringComparisonType.StartsWith;
                    }
                case OperatorTypes.NoStarts:
                    {
                        return StringComparisonType.NotStartsWith;
                    }
                default:
                    {
                        return StringComparisonType.NotSupported;
                    }
            }
        }

        public static bool IsCompareToOperator(OperatorTypes operatorTypes)
        {
            ComparisonType comparisonType = TranslationHelper.CompareToValidEqualityTypes(operatorTypes);

            if (comparisonType != ComparisonType.NotSupported)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsStringComparisonOperator(OperatorTypes operatorTypes)
        {
            StringComparisonType stringComparisonType = TranslationHelper.StringComparisonValidTypes(operatorTypes);

            if (stringComparisonType != StringComparisonType.NotSupported)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
