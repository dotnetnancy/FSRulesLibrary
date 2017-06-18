using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;



namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{ 
    
    public class RuleStatsList : List<RuleStats>
    {
        
        private CustomDatabaseHelper _customDatabaseHelper = new CustomDatabaseHelper();
        
        public RuleStatsList(System.Data.SqlClient.SqlDataReader reader)
        {
            this.AddItemsToListBySqlDataReader(reader);
        }

        public RuleStatsList()
        {
        }
        
        public virtual void AddItemsToListBySqlDataReader(System.Data.SqlClient.SqlDataReader reader)
        {
            RuleStats dto;

            using(reader)
            {
                while (reader.Read())
                {
                    dto = new RuleStats();
                    dto.OrganizationID =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleStatsTable.ORGANIZATION_ID_COLUMN), reader);
                    dto.RuleID =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleStatsTable.RULE_ID_COLUMN), reader);
                    dto.RuleName =
                    _customDatabaseHelper.resolveNullString(reader.GetOrdinal(DataAccessConstants.RuleStatsTable.RULE_NAME), reader);
                    dto.Result =
                    _customDatabaseHelper.resolveNullBoolean(reader.GetOrdinal(DataAccessConstants.RuleStatsTable.RESULT), reader);
                    dto.DateInserted =
                    _customDatabaseHelper.resolveNullDateTime(reader.GetOrdinal(DataAccessConstants.RuleStatsTable.DATE_INSERTED), reader);
                    dto.RuleStatID =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleStatsTable.RULE_STAT_ID), reader);
                    dto.PnrToProcessID =
                        _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleStatsTable.PNR_TO_PROCESS_ID), reader);
                    this.Add(dto);
                }           
            }
        }
    }
}
