using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DotNetNancy.Rules.RuleSet.DataAccess.DTO;
using DotNetNancy.Core.Data;
using System.Data;
using System.Data.SqlClient;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public class SqlRuleSetDefinitionsProvider : RuleSetDefinitionsProvider
    {
        private String _connectionString;
        private bool _isCurrentBusinessObjectProvider = false;
        private log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override System.Type GetProviderType
        {
            get
            {
                return this.GetType();
            }

        }

        public override bool IsCurrentBusinessObjectProvider
        {
            get
            {
                return _isCurrentBusinessObjectProvider;
            }
        }

        public override void Initialize(string name,
        System.Collections.Specialized.NameValueCollection config)
        {
            //Let ProviderBase perform the basic initialization
            base.Initialize(name, config);

            _isCurrentBusinessObjectProvider = Convert.ToBoolean(config[DataAccessConstants.IS_CURRENT_BUSINESS_OBJECT_PROVIDER]);

            _connectionString =
            SqlHelper.Connections[0].ConnectionString;
        }

        public override List<DTO.RuleDefinition> GetRuleDefinitions(Guid applicationID, Guid typeID)
        {
            SqlParameter[] parameters = new SqlParameter[] {SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID,
                applicationID), SqlHelper.PrepareParameter(DataAccessConstants.TypeTable.TYPE_ID,
                typeID)};

            SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.Connections[0],
                CommandType.StoredProcedure, DataAccessConstants.GET_RULE_DEFINITIONS, parameters);

            DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.RuleDefinition list =
                new DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.RuleDefinition(reader);

            return (List<RuleDefinition>)list;
        }

        public override Guid InsertRuleDefinition(RuleDefinition ruleDefinition)
        {
            Guid ruleID;

            List<SqlParameter> parameters = new List<SqlParameter>();

            SqlParameter ruleIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.RULE_ID,
                Guid.NewGuid(),SqlDbType.UniqueIdentifier,ParameterDirection.InputOutput);

            parameters.Add(ruleIDParameter);

            SqlParameter typeIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.TypeTable.TYPE_ID,
                ruleDefinition.TypeID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(typeIDParameter);

            SqlParameter applicationIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID,
                ruleDefinition.ApplicationID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(applicationIDParameter);

            SqlParameter ruleNameParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.RULE_NAME,
                ruleDefinition.RuleName);

            parameters.Add(ruleNameParameter);

            SqlParameter definitionParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.DEFINITION,
                ruleDefinition.Definition);

            parameters.Add(definitionParameter);

            SqlParameter pausedParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.PAUSED,
                ruleDefinition.Paused);

            parameters.Add(pausedParameter);

            SqlParameter dateCreatedParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.DATE_CREATED,
                ruleDefinition.DateCreated);

            parameters.Add(dateCreatedParameter);

            SqlParameter createdByParameter = SqlHelper.PrepareParameter(DataAccessConstants.UserStoreTable.CREATED_BY,
                ruleDefinition.CreatedBy);

            parameters.Add(createdByParameter);

            SqlParameter deletedParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.DELETED,
                ruleDefinition.Deleted);

            parameters.Add(deletedParameter);

            SqlParameter dateUpdatedParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.DATE_UPDATED,
                ruleDefinition.DateUpdated);

            parameters.Add(dateUpdatedParameter);

            SqlParameter updatedByParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.UPDATED_BY,
                ruleDefinition.UpdatedBy);

            parameters.Add(updatedByParameter);

            SqlHelper.ExecuteNonQuery(SqlHelper.Connections[0],
                CommandType.StoredProcedure, DataAccessConstants.INSERT_RULE_DEFINITION, parameters);

            ruleID = (Guid)ruleIDParameter.Value;

            return ruleID;

        }

        public override void UpdateRuleDefinition(Guid ruleID, Guid applicationID, Guid typeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            SqlParameter ruleIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.RULE_ID,
                ruleID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(ruleIDParameter);

            SqlParameter typeIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.TypeTable.TYPE_ID,
                typeID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(typeIDParameter);

            SqlParameter applicationIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID,
                applicationID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(applicationIDParameter);

            SqlHelper.ExecuteNonQuery(SqlHelper.Connections[0],
                CommandType.StoredProcedure, DataAccessConstants.UPDATE_RULE_DEFINITION, parameters);

        }

        public override void DeleteRuleDefinition(Guid ruleID, Guid applicationID, Guid typeID)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();

            SqlParameter ruleIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.RULE_ID,
                ruleID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(ruleIDParameter);

            SqlParameter typeIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.TypeTable.TYPE_ID,
                typeID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(typeIDParameter);

            SqlParameter applicationIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID,
                applicationID, SqlDbType.UniqueIdentifier, ParameterDirection.InputOutput);

            parameters.Add(applicationIDParameter);

            SqlHelper.ExecuteNonQuery(SqlHelper.Connections[0],
                CommandType.StoredProcedure, DataAccessConstants.DELETE_RULE_DEFINITION, parameters);
        }
    }
}






