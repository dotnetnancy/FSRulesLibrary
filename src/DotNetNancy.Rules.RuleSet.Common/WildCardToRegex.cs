using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DotNetNancy.Rules.RuleSet.Common
{
    /// <summary>
    /// Represents a wildcard running on the
    /// <see cref="System.Text.RegularExpressions"/> engine.
    /// </summary>
    public class WildCard : Regex
    {
        /// <summary>
        /// Initializes a wildcard with the given search pattern.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match.</param>
        public WildCard(string pattern)
            : base(WildCardToRegexStatic(pattern))
        {
        }

        /// <summary>
        /// Initializes a wildcard with the given search pattern and options.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to match.</param>
        /// <param name="options">A combination of one or more
        /// <see cref="System.Text.RegexOptions"/>.</param>
        public WildCard(string pattern, RegexOptions options)
            : base(WildCardToRegexStatic(pattern), options)
        {
        }

        public static string WildCardToRegexStatic(string wildcard)
        {
             StringBuilder sb = new StringBuilder(wildcard.Length + 8);
            //StringBuilder sb = new StringBuilder();
            //sb.Append("^");
            for (int i = 0; i < wildcard.Length; i++)
            {		
                char c = wildcard[i];
                switch(c)		
                {			
                    case '*':
                        sb.Append(".*");
                        break;			
                    case '?':
                        sb.Append(".");
                        break;	
                    case '\\':
                        if (i < wildcard.Length - 1)
                            sb.Append(Regex.Escape(wildcard[++i].ToString()));
                            break;
                    default:				
                        sb.Append(Regex.Escape(wildcard[i].ToString()));
                        break;
                }	
            }
            //sb.Append("$");	
            
            return sb.ToString();
        }            
        

        /// <summary>
        /// Converts a wildcard to a regex.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to convert.</param>
        /// <returns>A regex equivalent of the given wildcard.</returns>
        public string WildcardToRegex(string wildcard)
        {	
            StringBuilder sb = new StringBuilder(wildcard.Length + 8);
            //StringBuilder sb = new StringBuilder();
            //sb.Append("^");
            for (int i = 0; i < wildcard.Length; i++)
            {		
                char c = wildcard[i];
                switch(c)		
                {			
                    case '*':
                        sb.Append(".*");
                        break;			
                    case '?':
                        sb.Append(".");
                        break;	
                    case '\\':
                        if (i < wildcard.Length - 1)
                            sb.Append(Regex.Escape(wildcard[++i].ToString()));
                            break;
                    default:				
                        sb.Append(Regex.Escape(wildcard[i].ToString()));
                        break;
                }	
            }
            //sb.Append("$");	
            
            return sb.ToString();
        }
    }
}
