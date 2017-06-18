using System;
using System.Collections.Generic;
using System.Text;
  

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{   
    
    public class RuleStats
    {
        
        private System.Guid _organizationID;
        
        private System.Guid _ruleID;
        
        private string _ruleName;
        
        private bool _result;
        
        private System.DateTime _dateInserted;
        
        private System.Guid _ruleStatID;

        private System.Guid _pnrToProcessID;

        public virtual Guid PnrToProcessID
        {
            get { return _pnrToProcessID; }
            set { _pnrToProcessID = value; }
        }
        
     
        
        public RuleStats()
        {
            
        }
        
        public virtual Guid OrganizationID
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
        
        public virtual bool Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
            }
        }
        
        public virtual System.DateTime DateInserted
        {
            get
            {
                return this._dateInserted;
            }
            set
            {
                this._dateInserted = value;
            }
        }
        
        public virtual System.Guid RuleStatID
        {
            get
            {
                return this._ruleStatID;
            }
            set
            {
                this._ruleStatID = value;
            }
        }      
       
    }
}
