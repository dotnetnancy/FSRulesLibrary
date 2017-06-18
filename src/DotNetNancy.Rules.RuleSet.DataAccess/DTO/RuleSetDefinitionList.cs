using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Data;
using System.Xml;

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{
    public class RuleSetDefinitionList : List<RuleSetDefinition>
    {

        CustomDatabaseHelper _customDatabaseHelper = new CustomDatabaseHelper();

         public RuleSetDefinitionList(System.Data.SqlClient.SqlDataReader reader, bool byDkNumber)
        {
            this.AddItemsToListBySqlDataReader(reader, byDkNumber);
        }

         public RuleSetDefinitionList()
        {
        }
        
        public virtual void AddItemsToListBySqlDataReader(System.Data.SqlClient.SqlDataReader reader, bool byDkNumber)
        {
            if (byDkNumber)
            {
                ProcessCustomForDkNumber(reader);
            }
            else
            {
                ProcessNormally(reader);
            }
        }

        private void ProcessCustomForDkNumber(System.Data.SqlClient.SqlDataReader reader)
        {
            RuleSetDefinition dto;
            XmlDocument document;

            using (reader)
            {
                while (reader.Read())
                {
                    dto = new RuleSetDefinition();
                    document = new XmlDocument();

                    dto.OrganizationID =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleTable.ORGANIZATION_ID_COLUMN), reader);
                    dto.RuleName =
                    _customDatabaseHelper.resolveNullString(reader.GetOrdinal(DataAccessConstants.CustomDkResultSet.RULE_NAME_COLUMN), reader);
                    dto.Definition =
                    _customDatabaseHelper.retrieveNullableTypeFromDataReader(reader.GetOrdinal(DataAccessConstants.CustomDkResultSet.DEFINITION_COLUMN), reader);
                    dto.DateCreated =
                    _customDatabaseHelper.resolveNullDateTime(reader.GetOrdinal(DataAccessConstants.CustomDkResultSet.DATE_UPDATED_COLUMN), reader);
                    dto.RuleID =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.CustomDkResultSet.RULE_ID_COLUMN), reader);
                    dto.IsRootRule =
                        _customDatabaseHelper.resolveNullBoolean(reader.GetOrdinal(DataAccessConstants.CustomDkResultSet.ROOT_RULE), reader);
                    
                    document.Load(reader.GetSqlXml(reader.GetOrdinal(DataAccessConstants.CustomDkResultSet.DEFINITION_COLUMN)).CreateReader());

                    dto.SourceXmlDocument = document;

                    this.Add(dto);
                }
            }
        }

        private void ProcessNormally(System.Data.SqlClient.SqlDataReader reader)
        {
            RuleSetDefinition dto;
            XmlDocument document;

            using (reader)
            {
                while (reader.Read())
                {
                    dto = new RuleSetDefinition();
                    document = new XmlDocument();

                    dto.OrganizationID =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleTable.ORGANIZATION_ID_COLUMN), reader);
                    dto.RuleName =
                    _customDatabaseHelper.resolveNullString(reader.GetOrdinal(DataAccessConstants.RuleTable.RULE_NAME_COLUMN), reader);
                    dto.Definition =
                    _customDatabaseHelper.retrieveNullableTypeFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleTable.DEFINITION_COLUMN), reader);
                    dto.Paused =
                    _customDatabaseHelper.resolveNullBoolean(reader.GetOrdinal(DataAccessConstants.RuleTable.PAUSED_COLUMN), reader);
                    dto.DateCreated =
                    _customDatabaseHelper.resolveNullDateTime(reader.GetOrdinal(DataAccessConstants.RuleTable.DATE_CREATED_COLUMN), reader);
                    dto.CreatedBy =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleTable.CREATED_BY_COLUMN), reader);
                    dto.RuleID =
                    _customDatabaseHelper.retrieveGuidFromDataReader(reader.GetOrdinal(DataAccessConstants.RuleTable.RULE_ID_COLUMN), reader);

                    document.Load(reader.GetSqlXml(reader.GetOrdinal(DataAccessConstants.RuleTable.DEFINITION_COLUMN)).CreateReader());

                    dto.SourceXmlDocument = document;

                    this.Add(dto);
                }
            }
        }
    }
}
