using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.MSRuleSetTranslation
{
    public class RuleStat
    {
        private Guid _ruleStatID;

        private Guid _applicationID;

        private Guid _typeID;
       
        private System.Guid _ruleID;

        private string _ruleName;

        private bool _result;

        private System.DateTime _dateInserted;

        private System.Guid _referenceID;       

        public RuleStat()
        {

        }

        internal RuleStat( DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleStatistic  dto)
        {
            _ruleStatID = dto.RuleStatisticID;
            _applicationID = dto.ApplicationID;
            _typeID = dto.TypeID;
            _ruleID = dto.RuleID;
            _ruleName = dto.RuleName;
            _result = dto.Result;
            _dateInserted = dto.DateInserted;
            _referenceID = dto.ReferenceID;
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

        public Guid ApplicationID
        {
            get { return _applicationID; }
            set { _applicationID = value; }
        }

        public Guid TypeID
        {
            get { return _typeID; }
            set { _typeID = value; }
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

       

        public virtual System.Guid ReferenceID
        {
            get { return this._referenceID; }
            set { this._referenceID = value; }
        }
  
    }
}
