using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.Translation;
using System.Linq.Expressions;
using LinqCollectionExpressionLibrary;
using DotNetNancy.Rules.RuleSet.RuleSetDefinition;

namespace DotNetNancy.Rules.RuleSet.BaseRuleCollectionProcessingLibrary
{
    public abstract class BaseRuleCollectionProcessing
    {       

        public static bool AnyOneDynamic<T>(List<T> items, string propertyName, string propertyType, object valueToCompare, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "s");

            Expression<Func<T, bool>> myLambda = CollectionProcessingExpressions.GetLambda<T>(propertyName, valueToCompare, propertyType, operatorType, caseSensitive,setType);
            bool retVal = items.AsQueryable<T>().Any(myLambda);

            return retVal;

        }

        public static bool AnyOneDynamic<T, V>(Dictionary<T, V> items, V valueToCompare, OperatorTypes operatorType, bool caseSensitive,SetTypes setType)
        {
            List<V> listOfValues = (List<V>)items.Values.ToList<V>();

            return AnyOneDynamic<V>(listOfValues,valueToCompare, operatorType, caseSensitive,setType);

        }

        public static bool AnyOneDynamic<T>(List<T> items, T valueToCompare, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {
            Expression<Func<T, bool>> myLambda = CollectionProcessingExpressions.GetLambda<T>(valueToCompare, operatorType, caseSensitive,setType);
            bool retVal = items.AsQueryable<T>().Any(myLambda);
            return retVal;
        }


        public static bool AllDynamic<T, V>(Dictionary<T, V> items, V valueToCompare, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {
            List<V> listOfValues = (List<V>)items.Values.ToList<V>();

            return AllDynamic<V>(listOfValues, valueToCompare, operatorType, caseSensitive,setType);

        }

        public static bool AllDynamic<T>(List<T> items, T valueToCompare, OperatorTypes operatorType, bool caseSensitive, SetTypes setType)
        {
            Expression<Func<T, bool>> myLambda = CollectionProcessingExpressions.GetLambda<T>(valueToCompare, operatorType, caseSensitive,setType);
            bool retVal = items.AsQueryable<T>().All(myLambda);
            return retVal;
        }

       
        #region multiple property searches

        public bool AnyOneDynamic<T>(List<T> items, string[] properties, string[] operators,
            object[] values, string[] clauses, string [] setTypes, bool caseSensitive)
        {

            Expression<Func<T, bool>> myLambda = CollectionProcessingExpressions.GetLambda<T>(properties, operators, values, clauses,setTypes, caseSensitive);
            bool retVal = items.AsQueryable<T>().Any(myLambda);

            return retVal;
        }

        public bool AllDynamic<T>(List<T> items, string[] properties, string[] operators,
             object[] values, string[] clauses, string [] setTypes, bool caseSensitive)
        {

            Expression<Func<T, bool>> myLambda = CollectionProcessingExpressions.GetLambda<T>(properties, operators, values, clauses,setTypes, caseSensitive);
            bool retVal = items.AsQueryable<T>().All(myLambda);

            return retVal;
        }


        #endregion multiple property searches



    }
}
