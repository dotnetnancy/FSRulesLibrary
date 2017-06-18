using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DotNetNancy.Rules.RuleSet.DataAccess.DTO;
using DotNetNancy.Core.Data;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Xml;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public class SqlGeneralProvider : GeneralProvider
    {
        private log4net.ILog _log = log4net.LogManager.GetLogger(DataAccessConstants.CustomDataAccessLoggers.DEFAULT_LOGGER);
        private String _connectionString;
        private bool _isCurrentBusinessObjectProvider = false;
        private CustomDatabaseHelper _customDatabaseHelper = new CustomDatabaseHelper();

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

        public override UserStore GetUser(string email, string password, Guid applicationID, Guid typeID)
        {
            DTO.UserStore user = null;

            List<SqlParameter> parameters = new List<SqlParameter>();

            SqlParameter emailParameter = SqlHelper.PrepareParameter(DataAccessConstants.UserStoreTable.EMAIL,
                email);

            parameters.Add(emailParameter);

            SqlParameter passwordParameter = SqlHelper.PrepareParameter(DataAccessConstants.UserStoreTable.PASSWORD,
                password);

            parameters.Add(passwordParameter);

            SqlParameter applicationParameter = SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID,
                applicationID);

            parameters.Add(applicationParameter);

            SqlParameter typeParameter = SqlHelper.PrepareParameter(DataAccessConstants.TypeTable.TYPE_ID,
                typeID);

            parameters.Add(typeParameter);

            SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.Connections[0],
                CommandType.StoredProcedure, DataAccessConstants.GET_USER, parameters);

            DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.UserStore list =
                new DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.UserStore(reader);

            if (list.Count > 0)
            {
                //there should only be one but only return the first one
                user = list.First();
            }

            return user;
        }
    }
}


