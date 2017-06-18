using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{
    public class RuleSetDefinition
    {

        private System.Guid _organizationID;
        
        private string _ruleName;
        
        private object _definition;
        
        private bool _paused = false;
        
        private System.DateTime _dateCreated;
        
        private System.Guid _createdBy;
        
        private System.Guid _ruleID;

        private bool _rootRule = false;

        public bool IsRootRule
        {
            get { return _rootRule; }
            set { _rootRule = value; }
        }
           
        private XmlDocument _sourceXmlDocument = null; 
    

        public RuleSetDefinition()
        {
        }

        public RuleSetDefinition(XmlDocument sourceXmlDocument)
        {
            Load(sourceXmlDocument);
        }

        public RuleSetDefinition(string stringOfXmlFromSql)
        {
            Load(stringOfXmlFromSql);
        }

        private void Load(string stringOfXmlFromSql)
        {
            XmlDocument sourceXmlDocument = new XmlDocument();
            sourceXmlDocument.LoadXml(stringOfXmlFromSql);
            Load(sourceXmlDocument);
        }

        /// <summary>
        /// TODO:  fill this dto with members and then fill those members values from the xml document
        /// </summary>
        /// <param name="sourceXmlDocument"></param>
        private void Load(XmlDocument sourceXmlDocument)
        {
            _sourceXmlDocument = sourceXmlDocument;
        }       
       
        
        public virtual System.Guid OrganizationID
        {
            get
            {
                return this._organizationID;
            }
            set
            {
                this._organizationID = value;
            }
        }
        
        public virtual string RuleName
        {
            get
            {
                return this._ruleName;
            }
            set
            {
                this._ruleName = value;
            }
        }
        
        public virtual object Definition
        {
            get
            {
                return this._definition;
            }
            set
            {
                this._definition = value;
            }
        }
        
        public virtual bool Paused
        {
            get
            {
                return this._paused;
            }
            set
            {
                this._paused = value;
            }
        }
        
        public  virtual System.DateTime DateCreated
        {
            get
            {
                return this._dateCreated;
            }
            set
            {
                this._dateCreated = value;
            }
        }
        
        public virtual System.Guid CreatedBy
        {
            get
            {
                return this._createdBy;
            }
            set
            {
                this._createdBy = value;
            }
        }
        
        public virtual System.Guid RuleID
        {
            get
            {
                return this._ruleID;
            }
            set
            {
                this._ruleID = value;
            }
        }        
      

        public virtual XmlDocument SourceXmlDocument
        {
            get 
            { 
                return _sourceXmlDocument;
            }
            set 
            { 
                _sourceXmlDocument = value; 
            }
        }
        

    }
}
