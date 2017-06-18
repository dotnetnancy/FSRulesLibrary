using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNancy.Rules.RuleSet.Translation;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class Fields : Dictionary<SetTypes, List<Field>>
    {
        XmlDocument _fieldsDefinitionXml = null;

        Dictionary<string, CollectionField> _collectionFieldNameToCollectionField = new Dictionary<string,CollectionField>();
        Dictionary<string, List<Field>> _collectionFieldNameToCollectionMemberFields = new Dictionary<string,List<Field>>();


        public Fields(XmlDocument sourceXml)
            : base()
        {
           _fieldsDefinitionXml = sourceXml;

            foreach (XmlNode node in _fieldsDefinitionXml.DocumentElement.SelectSingleNode(Translation.Constants.XmlRuleElementConstants.FIELD_PATH).ChildNodes)
            {
                string dataType = node.Attributes[Translation.Constants.XmlRuleElementConstants.DATA_TYPE].Value;
                Field field = null;

                SetTypes setType = Translation.TranslationHelper.SetTypeTranslation(dataType) ;

                if (setType != SetTypes.Enum)
                {
                    if (!ProcessCollectionTypes(node, ref field))
                    {
                        field = new Field
                            {
                                DisplayName = node.Attributes[Translation.Constants.XmlRuleElementConstants.DISPLAY_NAME].Value,
                                PropertyName = node.Attributes[Translation.Constants.XmlRuleElementConstants.PROPERTY_NAME].Value,
                                DataType = node.Attributes[Translation.Constants.XmlRuleElementConstants.DATA_TYPE].Value,
                                Type = RuleElementTypes.Field,
                                MaxLength = node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH] != null ? (int?)int.Parse(node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH].Value) : null,
                                InvocationType = GetInvocationType(node)
                            };
                    }
                }
                else
                {
                    ProcessEnumField(node, ref field);
                }

                if (this.ContainsKey(setType))
                {
                    this[setType].Add(field);
                }
                else
                {
                    List<Field> list = new List<Field>();
                    list.Add(field);
                    this.Add(setType,list);
                }
            }

            ResolveCollectionTypesAndMembers();
        }

        private InvocationTypes GetInvocationType(XmlNode node)
        {
            //this is the default
            InvocationTypes invocationType = InvocationTypes.AsProperty;
            //unless it is specified
            XmlElement element = node as XmlElement;

            if (element.HasAttribute(Translation.Constants.XmlRuleElementConstants.INVOCATION_TYPE))
            {
                invocationType = 
                    TranslationHelper.InvocationTypesTranslation(element.GetAttribute(Translation.Constants.XmlRuleElementConstants.INVOCATION_TYPE));
            }

            return invocationType;

        }

        private void ResolveCollectionTypesAndMembers()
        {
            XmlNodeList nonDisplayedFields = _fieldsDefinitionXml.SelectNodes("//nonDisplayedFields/field");

            foreach (XmlNode node in nonDisplayedFields)
            {
                Field field = null;

                if (ProcessCollectionTypes(node, ref field))
                {
                    //then it was a collection type and it has been added into the items for the next loop etc
                }
            }

            //collection types may not have members but a member must belong to a collection type if it has been configured
            foreach (KeyValuePair<string, List<Field>> kvp in _collectionFieldNameToCollectionMemberFields)
            {
                if (_collectionFieldNameToCollectionField.ContainsKey(kvp.Key))
                {
                    _collectionFieldNameToCollectionField[kvp.Key].CollectionMembersList = kvp.Value;

                    
                    foreach (Field memberField in kvp.Value)
                    {
                        if (memberField is DictionaryCollectionMemberField)
                        {
                            ((DictionaryCollectionMemberField)memberField).CollectionField = _collectionFieldNameToCollectionField[kvp.Key];
                        }
                        else
                            if (memberField is EnumerableCollectionMemberField)
                            {
                                ((EnumerableCollectionMemberField)memberField).CollectionField = _collectionFieldNameToCollectionField[kvp.Key];
                            }
                    }
                
                }
                else
                {
                    
                    throw new ApplicationException("collection members configured witout matching collection to belong to");
                }
            }
        }

        private void ProcessEnumField(XmlNode node, ref Field field)
        {
            field = new EnumField
            {
                DisplayName = node.Attributes[Translation.Constants.XmlRuleElementConstants.DISPLAY_NAME].Value,
                PropertyName = node.Attributes[Translation.Constants.XmlRuleElementConstants.PROPERTY_NAME].Value,
                DataType = node.Attributes[Translation.Constants.XmlRuleElementConstants.DATA_TYPE].Value,
                Type = RuleElementTypes.Field,
                MaxLength = node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH] != null ? (int?)int.Parse(node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH].Value) : null,
                InvocationType = GetInvocationType(node)
            };
            if (node.ChildNodes.Count > 0) // enum type
            {
                foreach (XmlNode item in node.ChildNodes)
                {
                    Item itemObj = new Item
                    {
                        Value = item.Attributes[Translation.Constants.XmlRuleElementConstants.VALUE].Value,
                        DisplayName = item.Attributes[Translation.Constants.XmlRuleElementConstants.DISPLAY_NAME].Value
                    };

                    ((EnumField)field).Items.Add(itemObj);
                }
            }
        }

        private bool ProcessCollectionTypes(XmlNode node, ref Field field)
        {
            bool isCollectionTypeOrMember = false;

            XmlElement element = node as XmlElement;

            if (element.HasAttribute(Translation.Constants.XmlRuleElementConstants.COLLECTION_TYPE))
            {
                isCollectionTypeOrMember = true;
                //then this is a collection field
                field = GetCollectionField(node);

                if (!_collectionFieldNameToCollectionField.ContainsKey(field.PropertyName))
                {
                    _collectionFieldNameToCollectionField.Add(field.PropertyName,
                        (CollectionField)field);
                }
            }
            else
                if (element.HasAttribute(Translation.Constants.XmlRuleElementConstants.MEMBER_OF_COLLECTION))
                {
                    isCollectionTypeOrMember = true;

                    string memberOfCollectionName = string.Empty;
                    //then this is a collection member field
                    field = GetCollectionMemberField(node, ref memberOfCollectionName);

                   

                    if (!_collectionFieldNameToCollectionMemberFields.ContainsKey(memberOfCollectionName))
                    {
                        List<Field> list = new List<Field>();
                        list.Add(field);

                        _collectionFieldNameToCollectionMemberFields.Add(memberOfCollectionName,
                            list);
                    }
                    else
                    {
                        _collectionFieldNameToCollectionMemberFields[memberOfCollectionName].Add(field);

                    }
                }

            return isCollectionTypeOrMember;
        }

        private Field GetCollectionMemberField(XmlNode node, ref string memberOfCollectionName)
        {
            XmlElement element = node as XmlElement;
            Field field = null;

            //we only support IDictionary or IList at the moment
            if (element.HasAttribute(Translation.Constants.XmlRuleElementConstants.KEY))
            {
                field = new DictionaryCollectionMemberField
                {
                    DisplayName = node.Attributes[Translation.Constants.XmlRuleElementConstants.DISPLAY_NAME].Value,
                    PropertyName = node.Attributes[Translation.Constants.XmlRuleElementConstants.PROPERTY_NAME].Value,
                    DataType = node.Attributes[Translation.Constants.XmlRuleElementConstants.DATA_TYPE].Value,
                    Type = RuleElementTypes.Field,
                    MaxLength = node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH] != null ? (int?)int.Parse(node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH].Value) : null,
                    MemberOfCollection = node.Attributes[Translation.Constants.XmlRuleElementConstants.MEMBER_OF_COLLECTION].Value,
                    Key = node.Attributes[Translation.Constants.XmlRuleElementConstants.KEY].Value,
                    InvocationType = GetInvocationType(node)
                    
                };
                memberOfCollectionName = node.Attributes[Translation.Constants.XmlRuleElementConstants.MEMBER_OF_COLLECTION].Value;
            }
            else
            {
                field = new EnumerableCollectionMemberField
                {
                    DisplayName = node.Attributes[Translation.Constants.XmlRuleElementConstants.DISPLAY_NAME].Value,
                    PropertyName = node.Attributes[Translation.Constants.XmlRuleElementConstants.PROPERTY_NAME].Value,
                    DataType = node.Attributes[Translation.Constants.XmlRuleElementConstants.DATA_TYPE].Value,
                    Type = RuleElementTypes.Field,
                    MaxLength = node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH] != null ? (int?)int.Parse(node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH].Value) : null,
                    MemberOfCollection = node.Attributes[Translation.Constants.XmlRuleElementConstants.MEMBER_OF_COLLECTION].Value,
                    InvocationType = GetInvocationType(node)
                };
                memberOfCollectionName = node.Attributes[Translation.Constants.XmlRuleElementConstants.MEMBER_OF_COLLECTION].Value;

            }

            return field;
            
        }

        private CollectionField GetCollectionField(XmlNode node)
        {
             CollectionField field = new CollectionField();

                        
            field.DisplayName = node.Attributes[Translation.Constants.XmlRuleElementConstants.DISPLAY_NAME].Value;
            field.PropertyName = node.Attributes[Translation.Constants.XmlRuleElementConstants.PROPERTY_NAME].Value;
            field.DataType = node.Attributes[Translation.Constants.XmlRuleElementConstants.DATA_TYPE].Value;
            field.Type = RuleElementTypes.Field;
            field.MaxLength = node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH] != null ? (int?)int.Parse(node.Attributes[Translation.Constants.XmlRuleElementConstants.MAX_LENGTH].Value) : null;
            field.CollectionType =
            Translation.TranslationHelper.CollectionTypeTranslation(node.Attributes[Translation.Constants.XmlRuleElementConstants.COLLECTION_TYPE].Value);
            field.InvocationType = GetInvocationType(node);
          
                   

            return field;
        }
    }
}
