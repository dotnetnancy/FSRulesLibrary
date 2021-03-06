//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO.List
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;
    
    
    public class RuleDefinition : List<DTO.RuleDefinition>
    {
        
        private CustomDatabaseHelper _baseDatabase = new CustomDatabaseHelper();
        
        public RuleDefinition(System.Data.SqlClient.SqlDataReader reader)
        {
            this.AddItemsToListBySqlDataReader(reader);
        }
        
        public RuleDefinition()
        {
        }
        
        public virtual void AddItemsToListBySqlDataReader(System.Data.SqlClient.SqlDataReader reader)
        {

            DTO.RuleDefinition dto;
                using(reader)
                {

                while(reader.Read())
                {
                    dto = new DTO.RuleDefinition();
                    dto.RuleName = 
                    _baseDatabase.resolveNullString(reader.GetOrdinal("RuleName"), reader);

                    dto.Definition = new System.Xml.XmlDocument();

                    dto.Definition.Load(reader.GetSqlXml(reader.GetOrdinal("Definition")).CreateReader());
                    dto.Paused = 
                    _baseDatabase.resolveNullBoolean(reader.GetOrdinal("Paused"), reader);
                    dto.DateCreated = 
                    _baseDatabase.resolveNullDateTime(reader.GetOrdinal("DateCreated"), reader);
                    dto.CreatedBy = 
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("CreatedBy"), reader);
                    dto.Deleted = 
                    _baseDatabase.resolveNullBoolean(reader.GetOrdinal("Deleted"), reader);
                    dto.DateUpdated = 
                    _baseDatabase.resolveNullDateTime(reader.GetOrdinal("DateUpdated"), reader);
                    dto.UpdatedBy = 
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("UpdatedBy"), reader);
                    dto.RuleID = 
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("RuleID"), reader);
                    dto.TypeID = 
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("TypeID"), reader);
                    dto.ApplicationID = 
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("ApplicationID"), reader);
                    this.Add(dto);
                }

                }

        }
    }
}
