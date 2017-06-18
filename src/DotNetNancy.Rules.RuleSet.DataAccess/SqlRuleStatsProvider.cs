using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DotNetNancy.Rules.RuleSet.DataAccess;
using System.Data.SqlClient;
using DotNetNancy.Core.Data;
using System.Data;
using DotNetNancy.Rules.RuleSet.DataAccess.DTO;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public class SqlRuleStatsProvider : RuleStatsProvider
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


        

        public override List<RuleStatistic> GetRuleStats(Guid applicationID, Guid typeID)
        {
            SqlParameter[] parameters = new SqlParameter[] {SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID,
                applicationID), SqlHelper.PrepareParameter(DataAccessConstants.TypeTable.TYPE_ID,
                typeID)};

            SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.Connections[0],
                CommandType.StoredProcedure, DataAccessConstants.GET_RULE_STATISTICS, parameters);

            DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.RuleStatistic list =
                new DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.RuleStatistic(reader);

            return (List<RuleStatistic>)list;
        }



        public override bool InsertRuleStats(Guid applicationID,
            Guid typeID,
            Guid ruleID,
            string ruleName,
            bool result,
            DateTime createDate,
            Guid referenceID)
        {
            RuleStatistic dto = new RuleStatistic();

            bool successful = false;

            Guid ruleStatID = Guid.NewGuid();

            SqlParameter ruleStatIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleStatisticTable.RULE_STATISTIC_ID, ruleStatID,
                SqlDbType.UniqueIdentifier);           

            SqlParameter applicationIDParameter = 
                SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID, applicationID,
                SqlDbType.UniqueIdentifier);

            SqlParameter typeIDParameter =
                SqlHelper.PrepareParameter(DataAccessConstants.TypeTable.TYPE_ID, typeID,
                SqlDbType.UniqueIdentifier);

            SqlParameter dateInsertedParameter = 
                SqlHelper.PrepareParameter(DataAccessConstants.RuleStatisticTable.DATE_INSERTED, createDate, SqlDbType.DateTime);

            SqlParameter referenceIDParameter = 
                SqlHelper.PrepareParameter(DataAccessConstants.RuleStatisticTable.REFERENCE_ID, referenceID, SqlDbType.UniqueIdentifier);

            SqlParameter resultParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleStatisticTable.RESULT, result,
    SqlDbType.Bit);       


            SqlParameter ruleIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.RULE_ID, ruleID,
                SqlDbType.UniqueIdentifier);
            

            SqlParameter ruleNameParameter = SqlHelper.PrepareParameter(DataAccessConstants.RuleDefinitionTable.RULE_NAME, ruleName,
                SqlDbType.NVarChar);

            SqlParameter[] parameters = new SqlParameter[] { ruleStatIDParameter, applicationIDParameter,typeIDParameter,
            dateInsertedParameter, referenceIDParameter,resultParameter,ruleIDParameter,ruleNameParameter};


            int numberOfRowsAffected = 
                SqlHelper.ExecuteNonQuery(_connectionString, CommandType.StoredProcedure, DataAccessConstants.INSERT_RULE_STATISTIC, parameters);
            
            if (numberOfRowsAffected > 0)
            {
                successful = true;
            }

            return successful;
           
        }
    }
}
