using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public static class DataAccessConstants
    {

        //provider constants
        //public const string CONNECTION_STRING_NAME = "connectionStringName";
        public const string IS_CURRENT_BUSINESS_OBJECT_PROVIDER = "isCurrentBusinessObjectProvider";
        //public const string CONNECTION_STRINGS = "connectionStrings";
        public const string FILE_PATH_OR_URL = "filePathOrUrl";

        public static string GENERIC_DOT_NET_RULES_CONNECTION = "GenericDotNetRulesDb";
        public static string CUSTOM_APPLICATION_CONNECTION_NAME = "CustomApplicationSecondaryDb";
        public static string CENTRAL_DEV_PNR_STORE = "CentralDevPnrStore";

        //parameter names
        //parameter names are column names as per the code generator, the sql helper then will prepare the parameter with the "@" prepended
        //on its own, no need to map individual parameter names if they are column names for a table any non column named parameters should
        //be mapped here with or without the "@" prepending it as the sqlhelper will do the correct thing either way.

        //sproc names
        public static string GET_CONFIGURATION_FILE = "GetConfigurationFileByPrimaryKey";
        public static string GET_RULE_DEFINITIONS = "GetRuleDefinitions";
        public static string INSERT_RULE_DEFINITION = "InsertRuleDefinition";
        public static string UPDATE_RULE_DEFINITION = "UpdateRuleDefinitionByPrimaryKey";
        public static string DELETE_RULE_DEFINITION = "MarkRuleDefinitionDeleted";
        public static string GET_RULE_STATISTICS = "GetRuleStatistics";
        public static string INSERT_RULE_STATISTIC = "InsertRuleStatistic";
        public static string GET_USER = "GetUser";


        //individual column names 

        public static class ApplicationTable
        {
            public const string APPLICATION_ID = "ApplicationID";
            public const string APPLICATION_NAME = "ApplicationName";
            public const string APPLICATION_DESCRIPTION = "ApplicationDescription";
        }

        public static class ConfigurationFileTable
        {
           public const string TYPE_ID = "TypeID";
           public const string CONFIGURATION_TYPE_ID = "ConfigurationTypeID";
           public const string CONFIGURATION_FILE = "ConfigurationFile";
        }

        public static class RuleDefinitionTable
        {
           public const string RULE_ID = "RuleID";
           public const string RULE_NAME = "RuleName";
           public const string DEFINITION = "Definition";
           public const string PAUSED = "Paused";
           public const string DATE_CREATED = "DateCreated";
           public const string CREATED_BY = "CreatedBy";
           public const string DELETED = "Deleted";
           public const string DATE_UPDATED = "DateUpdated";
           public const string UPDATED_BY = "UpdatedBy";
        }

        public static class RuleStatisticTable
        {
            public const string RULE_STATISTIC_ID = "RuleStatisticID";
            public const string DATE_INSERTED = "DateInserted";
            public const string REFERENCE_ID = "ReferenceID";
            public const string RESULT = "Result";
        }

        public static class TypeTable
        {
            public const string TYPE_ID = "TypeID";
            public const string TYPE_FULL_NAME = "TypeFullName";
        }

        public static class UserStoreTable
        {
            public const string USER_ID = "UserID";
            public const string FIRST_NAME = "FirstName";
            public const string LAST_NAME = "LastName";
            public const string EMAIL = "Email";
            public const string PASSWORD = "Password";
            public const string LAST_LOGIN = "LastLogin";
            public const string DATE_CREATED = "DateCreated";
            public const string CREATED_BY = "CreatedBy";
            public const string IS_SUPER_USER = "IsSuperUser";
        }

        public static class CustomDataAccessLoggers
        {
            public const string DEFAULT_LOGGER = "DefaultLogger";
        }

    }
}
