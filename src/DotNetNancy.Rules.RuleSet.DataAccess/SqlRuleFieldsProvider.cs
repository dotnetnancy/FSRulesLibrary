﻿using System;
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
    public class SqlRuleFieldsProvider : RuleFieldsProvider
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

        /// <summary>
        /// TODO:  Need to implement this method once the xml is stored in the database and is ready to be used from
        /// that centrally stored location - currently the xml is in xml files
        /// </summary>
        /// <returns></returns>
        public override RuleFields GetData(Guid applicationID, Guid typeID, 
            DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes configurationType)
        {
            RuleFields ruleField = null;
            
            SqlParameter[] parameters = new SqlParameter[3] {SqlHelper.PrepareParameter(DataAccessConstants.ApplicationTable.APPLICATION_ID,applicationID),
                SqlHelper.PrepareParameter(DataAccessConstants.ConfigurationFileTable.TYPE_ID,
                typeID),SqlHelper.PrepareParameter(DataAccessConstants.ConfigurationFileTable.CONFIGURATION_TYPE_ID,
                (int)configurationType)};

            SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.Connections[0],
                CommandType.StoredProcedure,DataAccessConstants.GET_CONFIGURATION_FILE, parameters);

            DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.ConfigurationFile files = 
                new DotNetNancy.Rules.RuleSet.DataAccess.DTO.List.ConfigurationFile(reader);

            //there should only be one for this type and configuration type 
            if (files.Count==0)
            {
				throw new ApplicationException(string.Format("No configuration file found for application id {0}", applicationID.ToString()));
			}
			
			ruleField = new RuleFields(files[0].ConfigurationFile_Property);
            return ruleField;          
        }


    }
}


