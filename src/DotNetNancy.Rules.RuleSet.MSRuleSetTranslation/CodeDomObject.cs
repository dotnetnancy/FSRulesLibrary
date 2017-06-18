using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.Reflection;
using System.Collections;
using System.Workflow.ComponentModel.Serialization;
using System.Xml;
using System.Workflow.Activities.Rules;


namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class CodeDomObject
    {
        private log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Type _concreteType;

        public Type ConcreteType
        {
            get { return _concreteType; }
            set { _concreteType = value; }
        }

        CodeThisReferenceExpression _thisReferenceExpression;

        public CodeThisReferenceExpression ThisReferenceExpression
        {
            get { return _thisReferenceExpression; }
            set { _thisReferenceExpression = value; }
        }

        Dictionary<string, CodePropertyReferenceExpression> _propertyReferenceExpressions =
            new Dictionary<string, CodePropertyReferenceExpression>();

        Dictionary<string, Type> _propertyNameToConcretePropertyType = new Dictionary<string, Type>();

        public Dictionary<string, Type> PropertyNameToConcretePropertyType
        {
            get { return _propertyNameToConcretePropertyType; }
            set { _propertyNameToConcretePropertyType = value; }
        }

        public Dictionary<string, CodePropertyReferenceExpression> PropertyReferenceExpressions
        {
            get { return _propertyReferenceExpressions; }
            set { _propertyReferenceExpressions = value; }
        }

        Dictionary<string, CodeMethodReferenceExpression> _methodReferenceExpressions =
            new Dictionary<string, CodeMethodReferenceExpression>();       

        public Dictionary<string, CodeMethodReferenceExpression> MethodReferenceExpressions
        {
            get { return _methodReferenceExpressions; }
            set { _methodReferenceExpressions = value; }
        }

        Dictionary<string, Dictionary<string, CodeTypeReferenceExpression>> _enumerationTypeReferenceExpressions =
           new Dictionary<string, Dictionary<string, CodeTypeReferenceExpression>>();

        public Dictionary<string, Dictionary<string, CodeTypeReferenceExpression>> EnumerationTypeReferenceExpressions
        {
            get { return _enumerationTypeReferenceExpressions; }
            set { _enumerationTypeReferenceExpressions = value; }
        }

        List<string> _propertiesThatAreIenumerable = new List<string>();

        public List<string> PropertiesThatAreIenumerable
        {
            get { return _propertiesThatAreIenumerable; }
            set { _propertiesThatAreIenumerable = value; }
        }

        public CodeDomObject(Type type)
        {
            InitializeTypeCodeDomItems(type);
        }
     
        private void InitializeTypeCodeDomItems(Type type)
        {
            _concreteType = type;

            _thisReferenceExpression = new CodeThisReferenceExpression();

            foreach (PropertyInfo propertyInfo in _concreteType.GetProperties())
            {
                CodePropertyReferenceExpression propertyReference =
                    new CodePropertyReferenceExpression(_thisReferenceExpression, propertyInfo.Name);

                _propertyReferenceExpressions.Add(propertyInfo.Name, propertyReference);

                _propertyNameToConcretePropertyType.Add(propertyInfo.Name, propertyInfo.PropertyType);

                if (propertyInfo.PropertyType.IsEnum)
                {
                    if (!_enumerationTypeReferenceExpressions.ContainsKey(propertyInfo.Name))
                    {
                        _enumerationTypeReferenceExpressions.Add(propertyInfo.Name,
                            new Dictionary<string, CodeTypeReferenceExpression>());

                        if (!_enumerationTypeReferenceExpressions[propertyInfo.Name].ContainsKey(propertyInfo.PropertyType.Name))
                        {
                            _enumerationTypeReferenceExpressions[propertyInfo.Name].Add(propertyInfo.PropertyType.AssemblyQualifiedName,
                                new CodeTypeReferenceExpression(propertyInfo.PropertyType));
                        }
                    }
                }

                if (propertyInfo.PropertyType is IEnumerable)
                {
                    _propertiesThatAreIenumerable.Add(propertyInfo.Name);
                }
            }

            foreach (MethodInfo methodInfo in _concreteType.GetMethods())
            {
                CodeMethodReferenceExpression methodReference =
                    new CodeMethodReferenceExpression(_thisReferenceExpression, methodInfo.Name);

                if (!_methodReferenceExpressions.ContainsKey(methodInfo.Name))
                {
                    _methodReferenceExpressions.Add(methodInfo.Name, methodReference);
                    
                }
            }
        }      
      
    }
}
