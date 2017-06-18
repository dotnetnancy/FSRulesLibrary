using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using DotNetNancy.Rules.RuleSet.DataAccess;

namespace DotNetNancy.Rules.RuleSet.RuleSetDefinition
{
    public class RuleSetDefinition 
    {
        private string _ruleName;

        private XmlDocument _definition;

        private bool _paused;

        private System.DateTime _dateCreated;

        private System.Guid _createdBy;

        private System.Guid _ruleID;

        private System.Guid _applicationID;

    	private bool _canBeEvaluated = true;

    	private bool _canBeRuntimeExecuted;  
    	
        public System.Guid ApplicationID
        {
            get { return _applicationID; }
            set { _applicationID = value; }
        }

        private System.Guid _typeID;


        private Guid _referenceID;

        private int _priority;

        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }

        public Guid ReferenceID
        {
            get { return _referenceID; }
            set { _referenceID = value; }
        }

        public System.Guid TypeID
        {
            get { return _typeID; }
            set { _typeID = value; }
        }

     
        private DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition _dto;

        public DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition Dto
        {
            get { return _dto; }
            set { _dto = value; }
        }       

        private XmlDocument _sourceXmlDocument = null;

        RuleSetMetaDataDefinition _ruleSetMetaDataDefinition = null;

        Condition _condition = null;

        Stack _actionsStack = null;

        SortedDictionary<int, Action> _ruleThenActionsDefined = null;       
        List<string> _fieldsUsed = new List<string>();

        public SortedDictionary<int, Action> RuleThenActionsDefined
        {
            get { return _ruleThenActionsDefined; }
            set { _ruleThenActionsDefined = value; }
        }

        public Stack ActionsStack
        {
            get { return _actionsStack; }
            set { _actionsStack = value; }
        }

        public Condition ConditionProperty
        {
            get { return _condition; }
            set { _condition = value; }
        }

        public List<string> FieldsUsed
        {
            get { return _fieldsUsed; }
        }

        public RuleSetDefinition(RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;
        	_canBeRuntimeExecuted = _ruleSetMetaDataDefinition.ExecuteActionsInRule;
        }

        internal RuleSetDefinition(DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition dto,
            RuleSetMetaDataDefinition ruleSetMetaDataDefinition)
        {
            _ruleSetMetaDataDefinition = ruleSetMetaDataDefinition;
            _canBeRuntimeExecuted = _ruleSetMetaDataDefinition.ExecuteActionsInRule;
            LoadDto(dto);
            LoadObjectModel(ref _fieldsUsed);
        }

        //private void Load(DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition dto)
        //{            
        //    LoadDto(dto);
        //    LoadObjectModel();

        //}

        private void LoadDto(DotNetNancy.Rules.RuleSet.DataAccess.DTO.RuleDefinition dto)
        {
            _createdBy = dto.CreatedBy;
            _dateCreated = dto.DateCreated;
            _definition = dto.Definition;
            _paused = dto.Paused;
            _ruleID = dto.RuleID;
            _ruleName = dto.RuleName;
            _applicationID = dto.ApplicationID;
            _typeID = dto.TypeID;
            _sourceXmlDocument = dto.Definition;
            _dto = dto;
        }

        private void LoadObjectModel(ref List<string> fieldsUsedInRule)
        {
            _condition = new Condition(_sourceXmlDocument, _ruleSetMetaDataDefinition, ref fieldsUsedInRule);
            _actionsStack = _condition.MasterEquationGroupProperty.FinalEquationGroup.ActionStackableItemsStack;
            Priority = _condition.MasterEquationGroupProperty.FinalEquationGroup.Priority;
        }

        public string RuleName
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

        public XmlDocument Definition
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

        public bool Paused
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

        public System.DateTime DateCreated
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

        public System.Guid CreatedBy
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

        public System.Guid RuleID
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


        public XmlDocument SourceXmlDocument
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

        public RuleSetMetaDataDefinition RuleSetMetaDataDefinitionProperty
        {
            get 
            { 
                return _ruleSetMetaDataDefinition; 
            }
            set 
            {
                _ruleSetMetaDataDefinition = value; 
            }
        }

        public bool ExecuteActionsInRule
        {
            get
            {
                return _ruleSetMetaDataDefinition.ExecuteActionsInRule;
            }
        }

		public bool CanBeEvaluated
		{
			get
			{
				return _canBeEvaluated;
			}
			set 
			{  
				_canBeEvaluated = value; 
			}
		}


		public bool CanBeRuntimeExecuted
		{
			get
			{
				return _canBeRuntimeExecuted;
			}
			set
			{
				_canBeRuntimeExecuted = value;
			}
		}
    }
}
