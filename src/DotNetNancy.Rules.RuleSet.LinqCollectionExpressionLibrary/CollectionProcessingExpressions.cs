using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using DotNetNancy.Rules.RuleSet.Translation;
using DotNetNancy.Rules.RuleSet.Common;
using System.Text.RegularExpressions;
using System.Reflection;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;

namespace LinqCollectionExpressionLibrary
{
    public static class CollectionProcessingExpressions
    {

        public static Expression<Func<T, bool>> GetLambda<T>(T valueToCompare, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "s");

            Expression rightExpression = Expression.Constant(valueToCompare, typeof(T));

            Expression cond = GetConditionExpressionByOperatorType(param, rightExpression, operatorType, caseSensitive,typeof(T).Name, setType);

            return Expression.Lambda<Func<T, bool>>(cond,param);

        }

        public static Expression<Func<T, bool>> GetLambda<T>(string propertyName,
                                             object valueToCompare,
                                             string propertyType,
                                             OperatorTypes operatorType,
                                             bool caseSensitive,
                                             SetTypes setType
                                             )
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "s");

            Expression cond = GetConditionExpressionByOperatorType(param, propertyName, valueToCompare, operatorType, caseSensitive, propertyType, setType);

            return Expression.Lambda<Func<T, bool>>(cond, param);
        }

        public static Expression<Func<T, bool>> GetLambda<T>(string propertyName, List<object> values, string propertyType, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "s");

            Expression cond = GetConditionExpressionByOperatorType(param, propertyName, values, caseSensitive, operatorType,propertyType, setType);

            return Expression.Lambda<Func<T, bool>>(cond, param);

        }

        

       

        public static Expression<Func<T, bool>> GetLambda<T>(this
    SortedDictionary<int, KeyValuePair<Equation, Clause>> sequenceToEquationAndClause,
                                            bool caseSensitive
                                            )
        {
            //the type of T in this instance would be an object with properties that the equations reference

            ParameterExpression param = Expression.Parameter(typeof(T), "s");
            Expression cumulativeConditionExpression = null;

            SortedDictionary<int, Expression> individualConditions = new SortedDictionary<int, Expression>();
            SortedDictionary<int, Clause> individualClauses = new SortedDictionary<int, Clause>();


            for (int i = 1; i <= sequenceToEquationAndClause.Count; i++)
            {
                //since a sorted dictionary is already in sequence

                Expression cond = GetCondition(param, i, sequenceToEquationAndClause[i],caseSensitive);

                individualConditions.Add(i, cond);

                //if the clause is not equal to null
                if (sequenceToEquationAndClause[i].Value != null)
                {
                    individualClauses.Add(i, sequenceToEquationAndClause[i].Value);
                }
            }

            if (individualClauses.Count > 0)
            {
                //then we have at least 2 equations and at least one clause between them
                for (int i = 1; i <= individualConditions.Count; i++)
                {
                    //then this is the first one so initialize cumulative expression with it
                    if (cumulativeConditionExpression == null)
                    {
                        cumulativeConditionExpression = individualConditions[i];
                        continue;
                    }
                    else
                    {
                        if (individualClauses.Count >= i)
                        {
                            if (individualClauses[i] != null)
                            {
                                cumulativeConditionExpression =
                                    GetClauseExpression(cumulativeConditionExpression, individualClauses[i].ClauseType, individualConditions[i]);
                            }
                            else
                            {
                                //we are done
                                break;
                            }
                        }
                        else
                        {
                            //we are done
                            break;
                        }
                    }
                }
            }
            else
            {
                //then we should only have one equation
                if (individualConditions.Count == 1)
                {
                    cumulativeConditionExpression = individualConditions.First().Value;
                }
            }

            return Expression.Lambda<Func<T, bool>>(cumulativeConditionExpression, param);
        }

        /// <summary>
        /// this is a workaround to allow a code dom statement method call to pass us a parameter of at least an array.  code dom does not 
        /// support initializing an array that is being passed in here with any more than a primitive type.  nor can we initialize a dictionary
        /// or use a generic type of any sort (.net ruleset limitations not code dom for generics) We need to initialize the values
        /// outside of this method call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="properties"></param>
        /// <param name="caseSensitive"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetLambda<T>(string [] properties, string [] operators, object [] values, string [] clauses, string [] setTypes,
                                            bool caseSensitive
                                            )
        {
            //given the arrays we could then put them into the sorted dictionary form to call the existing method with the sorted dictionary
            //as the parameter
            SortedDictionary<int, KeyValuePair<Equation, Clause>> dictionaryToPass =
                new SortedDictionary<int, KeyValuePair<Equation, Clause>>();

            for (int i = 0; i < properties.Length; i++)
            {
                int sequence = i + 1;

                string propertyName = properties[i];
                string operatorName = operators[i];
                string setTypeName = setTypes[i];
                object value = values[i];

                string clauseName = null;

                int clausesLength = clauses.Length;

                if (clausesLength > i)
                {
                    clauseName = clauses[i];
                }

                Equation equation = new Equation();
                equation.FieldProperty = new EquationField();
                equation.FieldProperty.PropertyName = propertyName;
                equation.FieldProperty.DataType = TranslationHelper.SetTypeTranslation(setTypeName);

                equation.OperatorProperty = new EquationOperator();
                equation.OperatorProperty.Operator = TranslationHelper.OperatorTypeTranslation(operatorName);

                equation.ValueProperty = new EquationValue();
                equation.ValueProperty.Value = value;

                Clause clause = null;
                if (clauseName != null)
                {
                    clause = new Clause();
                    clause.ClauseType = (ClauseTypes)Enum.Parse(typeof(ClauseTypes), clauseName);
                }

                KeyValuePair<Equation, Clause> kvp = new KeyValuePair<Equation, Clause>(equation, clause);
                
                dictionaryToPass.Add(sequence, kvp);         

            }

            return GetLambda<T>(dictionaryToPass, caseSensitive);
           
        }

       


        private static Expression GetClauseExpression(Expression leftConditionExpression, ClauseTypes clauseTypes, Expression rightConditionExpression)
        {
            switch (clauseTypes)
            {
                case ClauseTypes.And:
                    {
                        return GetAndExpression(leftConditionExpression, rightConditionExpression);
                    }
                case ClauseTypes.Or:
                    {
                        return GetOrExpression(leftConditionExpression, rightConditionExpression);
                    }
            }

            return null;
        }

        private static Expression GetAndExpression(Expression leftConditionExpression, Expression rightConditionExpression)
        {
            return Expression.And(leftConditionExpression, rightConditionExpression);
        }

        private static Expression GetOrExpression(Expression leftConditionExpression, Expression rightConditionExpression)
        {
            return Expression.Or(leftConditionExpression, rightConditionExpression);
        }

        private static Expression GetCondition(ParameterExpression param, int sequence, KeyValuePair<Equation, Clause> equationAndClause, bool caseSensitive)
        {
            Expression prop = GetNestedPropertyExpression(param, equationAndClause.Key.FieldProperty.PropertyName);

            //based on the equation get the equation expressed as a condition like left >= right etc.
            Expression cond = GetConditionExpressionByOperatorType(param,
                equationAndClause.Key.FieldProperty.PropertyName,
                 equationAndClause.Key.ValueProperty.Value,
                 equationAndClause.Key.OperatorProperty.Operator,
                 caseSensitive,
                 prop.Type.Name,
                 equationAndClause.Key.FieldProperty.DataType);

            return cond;
        }


        private static Expression GetConditionExpressionByOperatorType(ParameterExpression param, Expression rightExpression, OperatorTypes operatorType, bool caseSensitive, string typeName, SetTypes setType)
        {
            //it can be part of the set of string manipulation type comparisons or just regular CompareTo comparisons

            if (TranslationHelper.IsCompareToOperator(operatorType))
            {
                if (typeName.Contains("String") || typeName.Contains("string"))
                {
                    return GetCompareToExpressionStringNoPropertyName(param, rightExpression, operatorType, caseSensitive);
                }
                else
                {
                    return GetCompareToExpressionNoPropertyName(param, rightExpression, operatorType, caseSensitive,setType);
                }
            }
            else
                if (TranslationHelper.IsStringComparisonOperator(operatorType))
                {
                    return GetStringComparisonExpression(param, rightExpression, operatorType,caseSensitive);
                }

            return null;
        }

        private static Expression GetCompareToExpressionStringNoPropertyName(ParameterExpression param, Expression rightExpression, OperatorTypes operatorType, bool caseSensitive)
        {
            //case sensitive is a bool coming in and to this method the case sensitivity means "ignore case" so the meaning to the compare
            //function is opposite what the bool value coming in is as this parameter means "treat as case sensitive"
            Expression casing = Expression.Constant(!caseSensitive);

            Expression[] parameters = new Expression[] { param, rightExpression, casing };

            Expression stringCompareMethodCall =
            Expression.Call(typeof(String).GetMethod("Compare", new Type[] { typeof(String), typeof(String), typeof(bool) }), parameters);

            //compare to returns an integer of 0 if equal less than 0 or greater than 0 for strings we do not support greater than or less than
            //just equal
            Expression zeroValue = Expression.Constant(0);

            return GetEqualsExpr(stringCompareMethodCall, zeroValue);
        }

        private static Expression GetStringComparisonExpression(ParameterExpression param, Expression rightExpression, OperatorTypes operatorType, bool caseSensitive)
        {
            //any string comparison expressions are wanting to treat anythign as a string so the param on the left or expression on the left
            //coming in here may not actually be of type string, it may be an int for example FlightNo which is an int on the object but we
            //want to treat it as a string so that it is searchable we do this by the rulefields.config so call to string first

            Expression toStringExpression = Expression.Call(param, typeof(object).GetMethod("ToString"));

            switch (operatorType)
            {
                case OperatorTypes.Starts:
                    {
                        return GetStartsWithExpr(toStringExpression, rightExpression, caseSensitive);
                    }
                case OperatorTypes.NoStarts:
                    {
                        return GetNotStartsWithExpr(toStringExpression, rightExpression, caseSensitive);
                    }
                case OperatorTypes.Ends:
                    {
                        return GetEndsWithExpr(toStringExpression, rightExpression, caseSensitive);
                    }
                case OperatorTypes.NoEnds:
                    {
                        return GetNotEndsWithExpr(toStringExpression, rightExpression, caseSensitive);
                    }
                case OperatorTypes.Contains:
                    {
                        return GetContainsExpr(toStringExpression, rightExpression, caseSensitive);
                    }
                case OperatorTypes.NoContains:
                    {
                        return GetNotContainsExpr(toStringExpression, rightExpression, caseSensitive);
                    }
            }

            return null;
        }

       


        private static Expression GetConditionExpressionByOperatorType(ParameterExpression param, string propertyName, object valueToCompare, OperatorTypes operatorType,bool caseSensitive,string propertyType, SetTypes setType)
        {
            //it can be part of the set of string manipulation type comparisons or just regular CompareTo comparisons

            if (TranslationHelper.IsCompareToOperator(operatorType))
            {
                if (propertyType.Contains("String") || propertyType.Contains("string"))
                {
                    return GetCompareToExpressionString(param, propertyName, valueToCompare, operatorType, caseSensitive);
                }
                else
                {
                    return GetCompareToExpression(param, propertyName, valueToCompare, operatorType, propertyType, setType);
                }
            }
            else
                if (TranslationHelper.IsStringComparisonOperator(operatorType))
                {
                    return GetStringComparisonExpression(param,propertyName,valueToCompare.ToString(),operatorType, caseSensitive);
                }

            return null;

        }

        private static Expression GetConditionExpressionByOperatorType(ParameterExpression param, string propertyName, List<object> values, bool caseSensitive,OperatorTypes operatorType, string propertyType, SetTypes setType)
        {
            if (TranslationHelper.IsCompareToOperator(operatorType))
            {
                if (propertyType.Contains("String") || propertyType.Contains("string"))
                {
                    return GetCompareToExpressionString(param, propertyName, values, operatorType, caseSensitive);
                }
                else
                {
                    return GetCompareToExpression(param, propertyName, values, operatorType, propertyType, setType);
                }
            }
           

            return null;
        }

        private static Expression GetConditionExpressionByOperatorType(Expression property, object valueToCompare, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {
            //it can be part of the set of string manipulation type comparisons or just regular CompareTo comparisons

            if (TranslationHelper.IsCompareToOperator(operatorType))
            {
                if (property.Type.Name.Contains("String") || property.Type.Name.Contains("string"))
                {
                    return GetCompareToExpressionString(property,valueToCompare, operatorType, caseSensitive);
                }
                else
                {
                    return GetCompareToExpression(property, valueToCompare, operatorType,setType);
                }
            }
            else
                if (TranslationHelper.IsStringComparisonOperator(operatorType))
                {
                    return GetStringComparisonExpression(property, valueToCompare.ToString(), operatorType, caseSensitive);
                }

            return null;

        }

        private static Expression GetCompareToExpressionString(ParameterExpression param, string propertyName, object valueToCompare, OperatorTypes operatorType, bool caseSensitive)
        {
            Expression prop = GetNestedPropertyExpression(param, propertyName);
            Expression val = Expression.Constant(valueToCompare.ToString());
            //case sensitive is a bool coming in and to this method the case sensitivity means "ignore case" so the meaning to the compare
            //function is opposite what the bool value coming in is as this parameter means "treat as case sensitive"
            Expression casing = Expression.Constant(!caseSensitive);

            Expression[] parameters = new Expression[] { prop, val, casing };

            Expression stringCompareMethodCall =
            Expression.Call(typeof(String).GetMethod("Compare", new Type[] { typeof(String), typeof(String), typeof(bool) }), parameters);

            //compare to returns an integer of 0 if equal less than 0 or greater than 0 for strings we do not support greater than or less than
            //just equal
            Expression zeroValue = Expression.Constant(0);

            return GetEqualsExpr(stringCompareMethodCall, zeroValue);
           
        }

        private static Expression GetCompareToExpressionString(Expression prop, object valueToCompare, OperatorTypes operatorType, bool caseSensitive)
        {
            Expression val = Expression.Constant(valueToCompare.ToString());
            //case sensitive is a bool coming in and to this method the case sensitivity means "ignore case" so the meaning to the compare
            //function is opposite what the bool value coming in is as this parameter means "treat as case sensitive"
            Expression casing = Expression.Constant(!caseSensitive);

            Expression[] parameters = new Expression[] { prop, val, casing };

            Expression stringCompareMethodCall =
            Expression.Call(typeof(String).GetMethod("Compare", new Type[] { typeof(String), typeof(String), typeof(bool) }), parameters);

            //compare to returns an integer of 0 if equal less than 0 or greater than 0 for strings we do not support greater than or less than
            //just equal
            Expression zeroValue = Expression.Constant(0);

            return GetEqualsExpr(stringCompareMethodCall, zeroValue);

        }

        private static Expression GetStringComparisonExpression(Expression prop, String valueToCompare, OperatorTypes operatorType, bool caseSensitive)
        {
            Expression rightExpression = Expression.Constant(valueToCompare);

            switch (operatorType)
            {
                case OperatorTypes.Starts:
                    {
                        return GetStartsWithExpr(prop,rightExpression,caseSensitive);
                    }
                case OperatorTypes.NoStarts:
                    {
                        return GetNotStartsWithExpr(prop, rightExpression, caseSensitive);
                    }
                case OperatorTypes.Ends:
                    {
                        return GetEndsWithExpr(prop, rightExpression, caseSensitive);
                    }
                case OperatorTypes.NoEnds:
                    {
                        return GetNotEndsWithExpr(prop, rightExpression, caseSensitive);
                    }
                case OperatorTypes.Contains:
                    {
                        return GetContainsExpr(prop, rightExpression, caseSensitive);
                    }
                case OperatorTypes.NoContains:
                    {
                        return GetNotContainsExpr(prop, rightExpression, caseSensitive);
                    }
            }
            return null;
        }


        private static Expression GetStringComparisonExpression(ParameterExpression param, string propertyName, String valueToCompare, OperatorTypes operatorType,bool caseSensitive)
        {
            switch (operatorType)
            {
                case OperatorTypes.Starts:
                    {
                        return GetStartsWithExpr(param, propertyName, valueToCompare,caseSensitive);
                    }
                case OperatorTypes.NoStarts:
                    {
                        return GetNotStartsWithExpr(param, propertyName, valueToCompare, caseSensitive);
                    }
                case OperatorTypes.Ends:
                    {
                        return GetEndsWithExpr(param, propertyName, valueToCompare, caseSensitive);
                    }
                case OperatorTypes.NoEnds:
                    {
                        return GetNotEndsWithExpr(param, propertyName, valueToCompare, caseSensitive);
                    }
                case OperatorTypes.Contains:
                    {
                        return GetContainsExpr(param, propertyName, valueToCompare, caseSensitive);
                    }
                case OperatorTypes.NoContains:
                    {
                        return GetNotContainsExpr(param, propertyName, valueToCompare, caseSensitive);
                    }
            }
            return null;
        }

        private static Expression GetNotContainsExpr(ParameterExpression param, string propertyName, string valueToCompare, bool caseSensitive)
        {
            Expression prop = GetNestedPropertyExpression(param, propertyName);
            Expression valueToPass = Expression.Constant(valueToCompare);

            Expression toStringExpression = Expression.Call(prop, typeof(object).GetMethod("ToString"));

            return GetNotContainsExpr(toStringExpression, valueToPass, caseSensitive);
        }

        private static Expression GetNotContainsExpr(Expression prop, Expression valueToPass, bool caseSensitive)
        {
            return Expression.Not(GetContainsExpr(prop,valueToPass,caseSensitive));
        }

        private static Expression GetContainsExpr(ParameterExpression param, string propertyName, String mustBeStringValue, bool caseSensitive)
        {         

            bool containsWildcard = DoesStringContainWilcard(mustBeStringValue);
            Expression prop = GetNestedPropertyExpression(param, propertyName);
            Expression valueToPass = Expression.Constant(mustBeStringValue);

            //any string comparison expressions are wanting to treat anything as a string so the param on the left or expression on the left
            //coming in here may not actually be of type string, it may be an int for example FlightNo which is an int on the object but we
            //want to treat it as a string so that it is searchable we did  this by setting the type to string in the rulefields.config 
            //so call ToString on the property first

            Expression toStringExpression = Expression.Call(prop, typeof(object).GetMethod("ToString"));


            if (!containsWildcard)
            {
                return GetContainsExpr(toStringExpression, valueToPass, caseSensitive);
            }
            else
            {
                return GetContainsWildcardExpr(toStringExpression, mustBeStringValue, caseSensitive);
            }

        }

        private static Expression GetNestedPropertyExpression(ParameterExpression param, string propertyName)
        {
            string[] props = propertyName.Split('.');
            Type type = param.Type;
            Expression expr = param;

            if (props != null)
            {
                //then there are nested properties
                if (props.Length > 1)
                {
                    foreach (string prop in props)
                    {
                        // use reflection (not ComponentModel) to mirror LINQ
                        PropertyInfo pi = type.GetProperty(prop);
                        expr = Expression.Property(expr, pi);
                        type = pi.PropertyType;
                    }
                }
                //else there are not and we just return the value of param assigned to expr
            }
            
            return expr;
        }

        private static Expression GetContainsWildcardExpr(Expression prop, string mustBeStringValue, bool caseSensitive)
        {

            WildCard wildCard = null;
            string patternToMatch = null;

            
            //default behaviour of .net is to do it case sensitive match
            wildCard = new WildCard(mustBeStringValue);
            patternToMatch = wildCard.WildcardToRegex(mustBeStringValue);

            Expression val = Expression.Constant(patternToMatch);

            Expression[] parameters = null;

            Expression regexMatchMethodCall = null;

            if (caseSensitive)
            {

                parameters = new Expression[] { prop, val };
                regexMatchMethodCall =
                    Expression.Call(typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(String), typeof(String) }), parameters);

            }
            else
            {
                Expression casing = Expression.Constant(RegexOptions.IgnoreCase, typeof(RegexOptions));
                parameters = new Expression[] { prop, val, casing };
                regexMatchMethodCall =
                        Expression.Call(typeof(Regex).GetMethod("IsMatch", new Type[] { typeof(String), typeof(String), typeof(RegexOptions) }), parameters);

            }

            //compare to returns an integer of 0 if equal less than 0 or greater than 0 for strings we do not support greater than or less than
            //just equal
            Expression zeroValue = Expression.Constant(false);

            return GetEqualsExpr(regexMatchMethodCall, zeroValue);
          
        }

        private static bool DoesStringContainWilcard(string mustBeStringValue)
        {
            bool containsWildcard = false;

            if (mustBeStringValue.Contains('*') ||
                                mustBeStringValue.Contains('?'))
            {
                containsWildcard = true;
            }

            return containsWildcard;
        }

        private static Expression GetContainsExpr(Expression itemToCallMethodAgainst, Expression expressionOfValueToPass, bool caseSensitive)
        {            
            //.net's default behavior is to make a case sensitive comparison
            if (caseSensitive)
            {
                Expression[] parameters = new Expression[] { expressionOfValueToPass };
                Expression methodCall =
                    Expression.Call(itemToCallMethodAgainst, typeof(String).GetMethod("Contains", new Type[] { typeof(String) }), parameters);

                return methodCall;
            }
            else
            {
                Expression toLowerPropertyMethodCall =
                    Expression.Call(itemToCallMethodAgainst, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));

                Expression toLowerValueMethodCall =
                    Expression.Call(expressionOfValueToPass, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));

                Expression[] parameters = new Expression[] { toLowerValueMethodCall };

                Expression ContainsMethodCall =
                    Expression.Call(toLowerPropertyMethodCall, typeof(String).GetMethod("Contains", new Type[] { typeof(String) }), parameters);

                return ContainsMethodCall;
            }
        }

        private static Expression GetNotStartsWithExpr(ParameterExpression param, string propertyName, String mustBeStringValue, bool caseSensitive)
        {
            Expression prop = GetNestedPropertyExpression(param, propertyName);
            Expression valueToPass = Expression.Constant(mustBeStringValue);

            //any string comparison expressions are wanting to treat anything as a string so the param on the left or expression on the left
            //coming in here may not actually be of type string, it may be an int for example FlightNo which is an int on the object but we
            //want to treat it as a string so that it is searchable we did  this by setting the type to string in the rulefields.config 
            //so call ToString on the property first

            Expression toStringExpression = Expression.Call(prop, typeof(object).GetMethod("ToString"));

            return GetNotStartsWithExpr(toStringExpression, valueToPass, caseSensitive);
        }

        private static Expression GetNotStartsWithExpr(Expression itemToCallMethodAgainst, Expression expressionOfValueToPass, bool caseSensitive)
        {           
             return Expression.Not(GetStartsWithExpr(itemToCallMethodAgainst,expressionOfValueToPass,caseSensitive));
        }
       
        private static Expression GetStartsWithExpr(ParameterExpression param, string propertyName, String mustBeStringValue, bool caseSensitive)
        {
            Expression prop = GetNestedPropertyExpression(param, propertyName);            

            Expression valueToPass = Expression.Constant(mustBeStringValue);

            //any string comparison expressions are wanting to treat anything as a string so the param on the left or expression on the left
            //coming in here may not actually be of type string, it may be an int for example FlightNo which is an int on the object but we
            //want to treat it as a string so that it is searchable we did  this by setting the type to string in the rulefields.config 
            //so call ToString on the property first

            Expression toStringExpression = Expression.Call(prop, typeof(object).GetMethod("ToString"));

            return GetStartsWithExpr(toStringExpression, valueToPass, caseSensitive);
        }

        private static Expression GetStartsWithExpr(Expression itemToCallMethodAgainst, Expression expressionOfValueToPass, bool caseSensitive)
        {           

            //.net's default behavior is to make a case sensitive comparison
            if (caseSensitive)
            {
                Expression[] parameters = new Expression[] { expressionOfValueToPass };
                Expression methodCall =
                    Expression.Call(itemToCallMethodAgainst, typeof(String).GetMethod("StartsWith", new Type[] { typeof(String) }), parameters);
                
                return methodCall;
            }
            else
            {
                Expression toLowerPropertyMethodCall =
                    Expression.Call (itemToCallMethodAgainst, typeof (string).GetMethod ("ToLower", System.Type.EmptyTypes));

                Expression toLowerValueMethodCall =
                    Expression.Call(expressionOfValueToPass, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));

                Expression[] parameters = new Expression[] { toLowerValueMethodCall };

                Expression StartsWithMethodCall =
                    Expression.Call(toLowerPropertyMethodCall, typeof(String).GetMethod("StartsWith", new Type[] { typeof(String) }), parameters);
                
                return StartsWithMethodCall;
            }
        }


        private static Expression GetEndsWithExpr(ParameterExpression param, string propertyName, String mustBeStringValue, bool caseSensitive)
        {
            Expression prop = GetNestedPropertyExpression(param, propertyName);
            Expression valueToPass = Expression.Constant(mustBeStringValue);

            //any string comparison expressions are wanting to treat anything as a string so the param on the left or expression on the left
            //coming in here may not actually be of type string, it may be an int for example FlightNo which is an int on the object but we
            //want to treat it as a string so that it is searchable we did  this by setting the type to string in the rulefields.config 
            //so call ToString on the property first

            Expression toStringExpression = Expression.Call(prop, typeof(object).GetMethod("ToString"));

            return GetEndsWithExpr(toStringExpression, valueToPass, caseSensitive);
        }

        private static Expression GetEndsWithExpr(Expression itemToCallMethodAgainst, Expression expressionOfValueToPass, bool caseSensitive)
        {

            //.net's default behavior is to make a case sensitive comparison
            if (caseSensitive)
            {
                Expression[] parameters = new Expression[] { expressionOfValueToPass };
                Expression methodCall =
                    Expression.Call(itemToCallMethodAgainst, typeof(String).GetMethod("EndsWith", new Type[] { typeof(String) }), parameters);

                return methodCall;
            }
            else
            {
                Expression toLowerPropertyMethodCall =
                    Expression.Call(itemToCallMethodAgainst, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));

                Expression toLowerValueMethodCall =
                    Expression.Call(expressionOfValueToPass, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));

                Expression[] parameters = new Expression[] { toLowerValueMethodCall };

                Expression EndsWithMethodCall =
                    Expression.Call(toLowerPropertyMethodCall, typeof(String).GetMethod("EndsWith", new Type[] { typeof(String) }), parameters);

                return EndsWithMethodCall;
            }
        }

        private static Expression GetNotEndsWithExpr(ParameterExpression param, string propertyName, String mustBeStringValue, bool caseSensitive)
        {
            Expression prop = GetNestedPropertyExpression(param, propertyName);
            Expression valueToPass = Expression.Constant(mustBeStringValue);

            //any string comparison expressions are wanting to treat anything as a string so the param on the left or expression on the left
            //coming in here may not actually be of type string, it may be an int for example FlightNo which is an int on the object but we
            //want to treat it as a string so that it is searchable we did  this by setting the type to string in the rulefields.config 
            //so call ToString on the property first

            Expression toStringExpression = Expression.Call(prop, typeof(object).GetMethod("ToString"));

            return GetNotEndsWithExpr(toStringExpression, valueToPass, caseSensitive);
        }

        private static Expression GetNotEndsWithExpr(Expression itemToCallMethodAgainst, Expression expressionOfValueToPass, bool caseSensitive)
        {            
            return Expression.Not(GetEndsWithExpr(itemToCallMethodAgainst,expressionOfValueToPass,caseSensitive));
        }

        private static Expression GetCompareToExpressionNoPropertyName(ParameterExpression param,  Expression valueToCompare, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {

            Expression rightExpression = GetConvertMethodInvocation(param.Type, valueToCompare.ToString(), setType);//Expression.Constant(valueToCompare);
            Expression leftExpression = GetConvertMethodInvocation(param, setType);

            
            switch (operatorType)
            {
                case OperatorTypes.Is:
                    {
                        return GetEqualsExpr((Expression)param,  valueToCompare);
                    }
                case OperatorTypes.Not:
                    {
                        return GetNotEqualsExpr((Expression)param, valueToCompare);
                    }
                case OperatorTypes.More:
                    {
                        return GetGreaterThanExpr((Expression)param, valueToCompare);
                    }
                case OperatorTypes.MoreIs:
                    {
                        return GetGreaterThanOrEqualExpr((Expression)param, valueToCompare);
                    }
                case OperatorTypes.Less:
                    {
                        return GetLessThanExpr((Expression)param, valueToCompare);
                    }
                case OperatorTypes.LessIs:
                    {
                        return GetLessThanOrEqualExpr((Expression)param, valueToCompare);
                    }
            }
            return null;
        }

        private static Expression GetCompareToExpression(Expression prop , object valueToCompare, OperatorTypes operatorType, SetTypes setType)
        {
            Expression rightExpression = GetConvertMethodInvocation(prop.Type,valueToCompare.ToString(), setType);//Expression.Constant(valueToCompare);
            Expression leftExpression = GetConvertMethodInvocation(prop, setType);

            switch (operatorType)
            {
                case OperatorTypes.Is:
                    {
                        return GetEqualsExpr(leftExpression, rightExpression);
                    }
                case OperatorTypes.Not:
                    {
                        return GetNotEqualsExpr(leftExpression, rightExpression);
                    }
                case OperatorTypes.More:
                    {
                        return GetGreaterThanExpr(leftExpression, rightExpression);
                    }
                case OperatorTypes.MoreIs:
                    {
                        return GetGreaterThanOrEqualExpr(leftExpression, rightExpression);
                    }
                case OperatorTypes.Less:
                    {
                        return GetLessThanExpr(leftExpression, rightExpression);
                    }
                case OperatorTypes.LessIs:
                    {
                        return GetLessThanOrEqualExpr(leftExpression, rightExpression);
                    }
            }

            return null;
        }

        private static Expression GetConvertMethodInvocation(Expression prop, SetTypes setType)
        {
            Expression expression = prop;

            if (setType == SetTypes.Date)
            {
                expression = Expression.Property(prop, "Date");
            }
            else
            {
                if (setType == SetTypes.Time)
                {
                    expression = Expression.Property(prop, "TimeOfDay");
                }               
            }

            return expression;
        }


        private static Expression GetCompareToExpression(ParameterExpression param, string propertyName, object valueToCompare, OperatorTypes operatorType, string propertyType, SetTypes setType)
        {
            switch (operatorType)
            {
                case OperatorTypes.Is:
                case OperatorTypes.In:
                case OperatorTypes.NotIn:
                    {
                        return GetEqualsExpr(param, propertyName, valueToCompare, setType);
                    }
                case OperatorTypes.Not:
                    {
                        return GetNotEqualsExpr(param, propertyName, valueToCompare, setType);
                    }
                case OperatorTypes.More:
                    {
                        return GetGreaterThanExpr(param, propertyName, valueToCompare, setType);
                    }
                case OperatorTypes.MoreIs:
                    {
                        return GetGreaterThanOrEqualExpr(param, propertyName, valueToCompare, setType);
                    }
                case OperatorTypes.Less:
                    {
                        return GetLessThanExpr(param, propertyName, valueToCompare, setType);
                    }
                case OperatorTypes.LessIs:
                    {
                        return GetLessThanOrEqualExpr(param, propertyName, valueToCompare, setType);
                    }
            }

            return null;
        }      


        //public static Expression GetEqualsExpr(Expression leftExpression, object valueToCompare, bool caseSensitive)
        //{
        //    Expression val = Expression.Constant(valueToCompare);

        //    return GetEqualsExpr(leftExpression, val, caseSensitive);
        //}
        public static Expression GetEqualsExpr(Expression leftExpression, Expression rightExpression)
        {
              return Expression.Equal(leftExpression, rightExpression);
        }

        public static Expression GetEqualsExpr(ParameterExpression param,
                                         string property,
                                         object value,
                                         SetTypes setType)
        {
            Expression prop = GetNestedPropertyExpression(param, property);
            Expression val = GetConvertMethodInvocation(prop.Type,value,setType);//Expression.Constant(value);
            
            Expression left = GetConvertMethodInvocation(prop, setType);
            Expression right = val;

            return GetEqualsExpr(left, right);
        }

        public static Expression GetNotEqualsExpr(Expression leftExpression, Expression rightExpression)
        {           
              return Expression.NotEqual(leftExpression, rightExpression);
        }

        public static Expression GetNotEqualsExpr(ParameterExpression param,
                                         string property,
                                         object value,
                                         SetTypes setType)
        {
            Expression prop = GetNestedPropertyExpression(param, property);
            Expression val = GetConvertMethodInvocation(prop.Type, value.ToString(),setType);//Expression.Constant(value);

            Expression left = GetConvertMethodInvocation(prop, setType);
            Expression right = val;

            return GetNotEqualsExpr(left, right);
        }

        public static Expression GetGreaterThanExpr(Expression leftExpression, Expression rightExpression)
        {
            return Expression.GreaterThan(leftExpression, rightExpression);
        }

        public static Expression GetGreaterThanExpr(ParameterExpression param,
                                         string property,
                                         object value,
                                         SetTypes setType)
        {
            Expression prop = GetNestedPropertyExpression(param, property);
            Expression val = GetConvertMethodInvocation(prop.Type, value.ToString(),setType);//Expression.Constant(value);

            Expression left = GetConvertMethodInvocation(prop, setType);
            Expression right = val;

            return GetGreaterThanExpr(left, right);
        }

        public static Expression GetGreaterThanOrEqualExpr(Expression leftExpression, Expression rightExpression)
        {
            return Expression.GreaterThanOrEqual(leftExpression, rightExpression);
        }

        public static Expression GetGreaterThanOrEqualExpr(ParameterExpression param,
                                         string property,
                                         object value,
                                         SetTypes setType)
        {
            Expression prop = GetNestedPropertyExpression(param, property);
            Expression val = GetConvertMethodInvocation(prop.Type, value.ToString(),setType);//Expression.Constant(value);

            Expression left = GetConvertMethodInvocation(prop, setType);
            Expression right = val;

            return GetGreaterThanOrEqualExpr(left, right);
        }

        public static Expression GetLessThanExpr(Expression leftExpression, Expression rightExpression)
        {
            return Expression.LessThan(leftExpression, rightExpression);
        }

        public static Expression GetLessThanExpr(ParameterExpression param,
                                         string property,
                                         object value,
                                         SetTypes setType)
        {
            Expression prop = GetNestedPropertyExpression(param, property);
            Expression val = GetConvertMethodInvocation(prop.Type, value.ToString(), setType);//Expression.Constant(value);

            Expression left = GetConvertMethodInvocation(prop, setType);
            Expression right = val;

            return GetLessThanExpr(left, right);
        }

        public static Expression GetLessThanOrEqualExpr(Expression leftExpression, Expression rightExpression)
        {
            return Expression.LessThanOrEqual(leftExpression, rightExpression);
        }

        public static Expression GetLessThanOrEqualExpr(ParameterExpression param,
                                         string property,
                                         object value,
                                         SetTypes setType)
        {
            Expression prop = GetNestedPropertyExpression(param, property);
            Expression val = GetConvertMethodInvocation(prop.Type, value.ToString(),setType);//Expression.Constant(value);

            Expression left = GetConvertMethodInvocation(prop, setType);
            Expression right = val;

            return GetLessThanOrEqualExpr(left, right);
        }

        public static Expression GetConvertMethodInvocation(Type propertyType, object valueToCompare, SetTypes setType)
        {
            //this represents the casting on the right hand side object so if date or time based on setTypes parameter then
            //either parse Date or TimeOfDay, also if enum, need to process as we do in code dom meaning we opened up the possibility
            //of creating "string" enums which is not supported inherently by .net

            Expression expression = null;

            ConstantExpression valuePrimitiveExpression = Expression.Constant(valueToCompare);        

            if (setType == SetTypes.Date)
            {
                expression = GetDateExpression(valuePrimitiveExpression);
            }
            else
                if (setType == SetTypes.Time)
                {
                    expression = GetTimeExpression(valuePrimitiveExpression);
                }
                else
                    if (setType == SetTypes.Enum)
                    {
                        expression = GetEnumExpression(valuePrimitiveExpression, propertyType);
                    }
                    else
                    {
                        string convertMethodBasedOnType = null;

                        if (propertyType.Equals(typeof(String)) ||
                            propertyType.Equals(typeof(string)))
                        {
                            convertMethodBasedOnType = "ToString";
                        }
                        else
                            if (propertyType.Equals(typeof(Boolean)) ||
                                propertyType.Equals(typeof(bool)))
                            {
                                convertMethodBasedOnType = "ToBoolean";
                            }
                            else
                                if (propertyType.Equals(typeof(Decimal)) ||
                                    propertyType.Equals(typeof(decimal)))
                                {
                                    convertMethodBasedOnType = "ToDecimal";
                                }
                                else
                                    if (propertyType.Equals(typeof(Double)) ||
                                        propertyType.Equals(typeof(double)))
                                    {
                                        convertMethodBasedOnType = "ToDouble";
                                    }
                                    else
                                        if (propertyType.Equals(typeof(short)) ||
                                            propertyType.Equals(typeof(Int16)))
                                        {
                                            convertMethodBasedOnType = "ToInt16";
                                        }
                                        else
                                            if (propertyType.Equals(typeof(int)) ||
                                                propertyType.Equals(typeof(Int32)))
                                            {
                                                convertMethodBasedOnType = "ToInt32";
                                            }
                                            else
                                                if (propertyType.Equals(typeof(Int64)) ||
                                                    propertyType.Equals(typeof(long)))
                                                {
                                                    convertMethodBasedOnType = "ToInt64";
                                                }
                                                else
                                                    if (propertyType.Equals(typeof(Single)) ||
                                                        propertyType.Equals(typeof(float)))
                                                    {
                                                        convertMethodBasedOnType = "ToSingle";
                                                    }
                        expression = Expression.Call(typeof(System.Convert).GetMethod(convertMethodBasedOnType, new Type[] { typeof(object) }), new Expression[] { valuePrimitiveExpression });

                    }


            return expression;
        }

        private static Expression GetEnumExpression(object valueToCompare, Type propertyType)
        {
            if (StringEnum.IsStringDefined(Type.GetType(propertyType.FullName), valueToCompare.ToString()))
            {
                //if this is true then this is a string enumeration which means that the attribute on the enumerated value is
                //what would be found in the xml - .net does not support string enums just numeric enums

                //converts back to an enum value from String Value (case insensitive)
                //and now shows the actual enum string from the attribute which is what is fed to the fieldreferenceexpression

                //valueToCompare = StringEnum.Parse(Type.GetType(enumTypeName),
                //    equation.ValueProperty.Value.ToString(), false).ToString();

                //if we want the string value
                valueToCompare = StringEnum.Parse(propertyType,
                    valueToCompare.ToString(), false).ToString();

                //if we are in this block it is because there is a string attribute defined and this is a string
                //enum in that case we want the numeric value as a string 

                valueToCompare = Enum.Parse(Type.GetType(propertyType.FullName), valueToCompare.ToString()).ToString();

            }

            return Expression.Constant(valueToCompare);
        }

        private static Expression GetTimeExpression(ConstantExpression valuePrimitiveExpression)
        {
            Expression dateTimeParse = Expression.Call(typeof(System.DateTime).GetMethod("Parse", new Type[] { typeof(object) }), new Expression[] { valuePrimitiveExpression });
            Expression expression = Expression.Property(dateTimeParse, "TimeOfDay");
            return expression;
        }

        private static Expression GetDateExpression(ConstantExpression valuePrimitiveExpression)
        {
            Expression dateTimeParse = Expression.Call(typeof(System.DateTime).GetMethod("Parse", new Type[] { typeof(object) }), new Expression[] { valuePrimitiveExpression });
            Expression expression = Expression.Property(dateTimeParse, "Date");
            return expression;
        }



        



        #region ideas about a true dynamic where clause such that the consumer can define one item in a list that meets multiple criteria



        ////this example would be used by another function to return the list of items that meet the "where" conditions
        ////i think that this would take a Stack of items so that we coudl have varying ands and ors in between
        //public static IQueryable<T> Filter<T>(this IQueryable<T> query, Dictionary<string, string> dictionary)
        //{
        //    Type t = typeof(T);
        //    StringBuilder sb = new StringBuilder();
        //    PropertyInfo[] properties = t.GetProperties();
        //    foreach (string key in dictionary.Keys)
        //    {
        //        PropertyInfo property = properties.Where(p => p.Name == key).SingleOrDefault();
        //        if (property != null)
        //        {
        //            if (sb.Length > 0) sb.Append(" && ");

        //            string value = dictionary[key];

        //            sb.Append(string.Format(@"{0}.ToString().Contains(""{1}"")", key, value));
        //        }
        //    }

        //    if (sb.Length > 0)
        //        return query.Where(sb.ToString());
        //    else
        //        return query;
        //}

        #endregion ideas about a true dynamic where clause such that the consumer can define one item in a list that meets multiple criteria





        
    }

}
