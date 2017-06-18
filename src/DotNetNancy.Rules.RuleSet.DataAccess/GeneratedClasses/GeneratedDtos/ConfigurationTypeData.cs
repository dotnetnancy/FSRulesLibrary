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
    
    
    public class ConfigurationType
    {
        
        private char[] _configurationTypeDescription;
        
        private int _configurationTypeID;
        
        private Dictionary<string, bool> _isModifiedDictionary = new Dictionary<string, bool>();
        
        public ConfigurationType()
        {
            this.InitializeIsModifiedDictionary();
        }
        
        public virtual char[] ConfigurationTypeDescription
        {
            get
            {
                return this._configurationTypeDescription;
            }
            set
            {
                this._configurationTypeDescription = value;
                this.SetIsModified("ConfigurationTypeDescription");
            }
        }
        
        public virtual int ConfigurationTypeID
        {
            get
            {
                return this._configurationTypeID;
            }
            set
            {
                this._configurationTypeID = value;
                this.SetIsModified("ConfigurationTypeID");
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
            this.IsModifiedDictionary.Add("ConfigurationTypeDescription", false);
            this.IsModifiedDictionary.Add("ConfigurationTypeID", false);
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
