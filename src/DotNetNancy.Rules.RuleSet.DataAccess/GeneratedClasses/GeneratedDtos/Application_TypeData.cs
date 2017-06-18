//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    
    public class Application_Type
    {
        
        private System.Guid _applicationID;
        
        private System.Guid _typeID;
        
        private Dictionary<string, bool> _isModifiedDictionary = new Dictionary<string, bool>();
        
        public Application_Type()
        {
            this.InitializeIsModifiedDictionary();
        }
        
        public virtual System.Guid ApplicationID
        {
            get
            {
                return this._applicationID;
            }
            set
            {
                this._applicationID = value;
                this.SetIsModified("ApplicationID");
            }
        }
        
        public virtual System.Guid TypeID
        {
            get
            {
                return this._typeID;
            }
            set
            {
                this._typeID = value;
                this.SetIsModified("TypeID");
            }
        }
        
        public virtual Dictionary<string, bool> IsModifiedDictionary
        {
            get
            {
                return this._isModifiedDictionary;
            }
            set
            {
                this._isModifiedDictionary = value;
            }
        }
        
        private void InitializeIsModifiedDictionary()
        {
            this.IsModifiedDictionary.Add("ApplicationID", false);
            this.IsModifiedDictionary.Add("TypeID", false);
        }
        
        private void SetIsModified(string columnName)
        {
            if ((this.IsModifiedDictionary.ContainsKey(columnName) == true))
            {
                IsModifiedDictionary[columnName] = true;
            }
        }
    }
}
