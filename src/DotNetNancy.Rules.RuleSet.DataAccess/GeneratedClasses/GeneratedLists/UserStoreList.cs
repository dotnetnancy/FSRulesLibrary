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


    public class UserStore : List<DTO.UserStore>
    {
        
        private CustomDatabaseHelper _baseDatabase = new CustomDatabaseHelper();
        
        public UserStore(System.Data.SqlClient.SqlDataReader reader)
        {
            this.AddItemsToListBySqlDataReader(reader);
        }
        
        public UserStore()
        {
        }
        
        public virtual void AddItemsToListBySqlDataReader(System.Data.SqlClient.SqlDataReader reader)
        {
        
            DTO.UserStore dto;
                using(reader)
                {

                while(reader.Read())
                {
            dto = new DTO.UserStore();
                    dto.FirstName = 
                    _baseDatabase.resolveNullString(reader.GetOrdinal("FirstName"), reader);
                    dto.LastName = 
                    _baseDatabase.resolveNullString(reader.GetOrdinal("LastName"), reader);
                    dto.Email = 
                    _baseDatabase.resolveNullString(reader.GetOrdinal("Email"), reader);
                    dto.Password = 
                    _baseDatabase.resolveNullString(reader.GetOrdinal("Password"), reader);
                    dto.LastLogin = 
                    _baseDatabase.resolveNullDateTime(reader.GetOrdinal("LastLogin"), reader);
                    dto.DateCreated = 
                    _baseDatabase.resolveNullDateTime(reader.GetOrdinal("DateCreated"), reader);
                    dto.CreatedBy = 
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("CreatedBy"), reader);
                    dto.IsSuperUser = 
                    _baseDatabase.resolveNullBoolean(reader.GetOrdinal("IsSuperUser"), reader);
                    dto.UserID = 
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("UserID"), reader);
                    this.Add(dto);
                }

                }

        }
    }
}
