using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    /// <summary>
    /// usage example:
    /// 
    /// // Let’s start with the simple examples:
    
//var func = Linq.Func((int a, int b) => a + b);
//var expr = Linq.Expr((int a, int b) => a + b);

//// .. and using anonymous types is possible as well!
//var func = Linq.Func((int a, int b) => 
//  new { Sum = a + b, Mul = a * b });
//var expr = Linq.Expr ((int a, int b) => 
//  new { Sum = a + b, Mul = a * b });

    /// </summary>


    public static class Linq
    {
        // Returns the given anonymous method as a lambda expression
        public static Expression<Func<T, R>>
            Expr<T, R>(Expression<Func<T, R>> f)
        {
            return f;
        }

        // Returns the given anonymous function as a Func delegate
        public static Func<T, R>
            Func<T, R>(Func<T, R> f)
        {
            return f;
        }
    }
}
