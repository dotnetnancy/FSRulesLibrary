using System;
using System.Collections.Generic;
using System.Text;


namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{    
    
    public class OrganizationData
    {
        
        private char[] _organizationTypeID;
        
        private string _organizationName;
        
        private string _gds;
        
        private string _eflowPcc;
        
        private string _eflowQueue;
        
        private bool _parallel;
        
        private bool _active;
        
        private System.DateTime _dateCreated;
        
        private System.Guid _createdBy;
        
        private System.Guid _organizationID;
        
        
        public OrganizationData()
        {
        }
        
        public virtual char[] OrganizationTypeID
        {
            get
            {
                return this._organizationTypeID;
            }
            set
            {
                this._organizationTypeID = value;
            }
        }
        
        public virtual string OrganizationName
        {
            get
            {
                return this._organizationName;
            }
            set
            {
                this._organizationName = value;
            }
        }
        
        public virtual string Gds
        {
            get
            {
                return this._gds;
            }
            set
            {
                this._gds = value;
            }
        }
        
        public virtual string EflowPcc
        {
            get
            {
                return this._eflowPcc;
            }
            set
            {
                this._eflowPcc = value;
            }
        }
        
        public virtual string EflowQueue
        {
            get
            {
                return this._eflowQueue;
            }
            set
            {
                this._eflowQueue = value;
            }
        }
        
        public virtual bool Parallel
        {
            get
            {
                return this._parallel;
            }
            set
            {
                this._parallel = value;
            }
        }
        
        public virtual bool Active
        {
            get
            {
                return this._active;
            }
            set
            {
                this._active = value;
            }
        }
        
        public virtual System.DateTime DateCreated
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
        
      
    }
}
