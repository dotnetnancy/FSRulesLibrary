using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DotNetNancy.Rules.RuleSet.DataAccess.DTO;

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{  
    
    public class OrganizationList : List<OrganizationData>
    {

        private CustomDatabaseHelper _baseDatabase = new CustomDatabaseHelper();
        
        public OrganizationList(System.Data.SqlClient.SqlDataReader reader)
        {
            this.AddItemsToListBySqlDataReader(reader);
        }
        
        public OrganizationList()
        {
        }
        
        public virtual void AddItemsToListBySqlDataReader(System.Data.SqlClient.SqlDataReader reader)
        {
        
            OrganizationData dto;    
            
            using (reader)
            {
                while (reader.Read())
                {
                    dto = new OrganizationData();
                    dto.OrganizationTypeID =
                    _baseDatabase.resolveNullChar(reader.GetOrdinal("OrganizationTypeID"), reader);
                    dto.OrganizationName =
                    _baseDatabase.resolveNullString(reader.GetOrdinal("OrganizationName"), reader);
                    dto.Gds =
                    _baseDatabase.resolveNullString(reader.GetOrdinal("Gds"), reader);
                    dto.EflowPcc =
                    _baseDatabase.resolveNullString(reader.GetOrdinal("EflowPcc"), reader);
                    dto.EflowQueue =
                    _baseDatabase.resolveNullString(reader.GetOrdinal("EflowQueue"), reader);
                    dto.Parallel =
                    _baseDatabase.resolveNullBoolean(reader.GetOrdinal("Parallel"), reader);
                    dto.Active =
                    _baseDatabase.resolveNullBoolean(reader.GetOrdinal("Active"), reader);
                    dto.DateCreated =
                    _baseDatabase.resolveNullDateTime(reader.GetOrdinal("DateCreated"), reader);
                    dto.CreatedBy =
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("CreatedBy"), reader);
                    dto.OrganizationID =
                    _baseDatabase.retrieveGuidFromDataReader(reader.GetOrdinal("OrganizationID"), reader);
                    this.Add(dto);
                }

            }

        }
    }
}
