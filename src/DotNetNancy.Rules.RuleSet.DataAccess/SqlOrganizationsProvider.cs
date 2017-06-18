using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using DotNetNancy.Rules.RuleSet.DataAccess;
using System.Data.SqlClient;
using DotNetNancy.Data;
using System.Data;
using DotNetNancy.Rules.RuleSet.DataAccess.DTO;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public class SqlOrganizationsProvider : OrganizationsProvider
    {
        private String _connectionString;
        private bool _isCurrentBusinessObjectProvider = false;
        private log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override Type GetProviderType
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
            SqlHelper.Connections[DataAccessConstants.DATABASE_CONNECTION_NAME].ConnectionString;
        }

        public override OrganizationData GetOrganizationByOrganizationID(Guid organizationID)
        {
            SqlParameter organizationIDParameter = SqlHelper.PrepareParameter(DataAccessConstants.ORGANIZATION_ID_PARAM, organizationID,
                SqlDbType.UniqueIdentifier);

            SqlParameter[] parameters = new SqlParameter[] { organizationIDParameter };

            SqlDataReader reader =
                SqlHelper.ExecuteReader(_connectionString, DataAccessConstants.GET_ORGANIZATION_BY_ORGANIZATION_ID,
                parameters);

            //inside this list a using block is around the reader so it cleans itself up
            OrganizationList returnList = new OrganizationList(reader);

            //this overload is by primary key so there should only be one returned
            if (returnList.Count == 1)
            {
                return ((List<OrganizationData>)returnList)[0];
            }
            else
                if (returnList.Count > 1)
                {
                    throw new ApplicationException("There are multiple Organizations returned for this Organization ID!  OrganizationID=" + organizationID.ToString());
                }
            //not found
            return null;
        }



        public override OrganizationData GetOrganizationByDkNumber(string dkNumber)
        {
            SqlParameter dkNumberParameter = SqlHelper.PrepareParameter(DataAccessConstants.DK_NUMBER_PARAM, dkNumber,
                SqlDbType.NVarChar);

            SqlParameter[] parameters = new SqlParameter[] { dkNumberParameter };

            SqlDataReader reader =
                SqlHelper.ExecuteReader(_connectionString, DataAccessConstants.GET_ORGANIZATION_BY_DK_NUMBER,
                parameters);

            //inside this list a using block is around the reader so it cleans itself up
            OrganizationList returnList = new OrganizationList(reader);

            //this overload is by primary key so there should only be one returned
            if (returnList.Count == 1)
            {
                return ((List<OrganizationData>)returnList)[0];
            }
            else
                if (returnList.Count > 1)
                {
                    throw new ApplicationException("There are multiple Organizations returned for this DK Number!  DKNumber=" + dkNumber.ToString());
                }
            //not found
            return null;


        }


    }
}
