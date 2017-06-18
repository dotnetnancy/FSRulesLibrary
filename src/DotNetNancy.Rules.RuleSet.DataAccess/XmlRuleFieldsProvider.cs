using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotNetNancy.Rules.RuleSet.DataAccess.DTO;
using System.Xml;
using System.IO;

namespace DotNetNancy.Rules.RuleSet.DataAccess
{
    public class XmlFileRuleFieldsProvider : RuleFieldsProvider
    {
        private String _filePathOrUrl = string.Empty;
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

            _filePathOrUrl = config[DataAccessConstants.FILE_PATH_OR_URL];

            config.Remove(DataAccessConstants.FILE_PATH_OR_URL);
        }

        /// <summary>
        /// TODO:  forced to have these files with the executing application that is consuming the components really needs to
        /// be addressed at some point.  bascially each UI that consumes this needs to have a copy of these files and if you need to 
        /// make a "global" change you would have to make it with each application as well in each individual application
        /// </summary>
        /// <returns></returns>
        public override RuleFields GetData(Guid applicationID, Guid typeID,
            DotNetNancy.Rules.RuleSet.DataAccess.Enumerations.ConfigurationTypes configurationType)
        {
            throw new NotImplementedException();
            XmlDocument sourceXmlDocument = new XmlDocument();

            string pathToExecutingAssembly =
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            
            sourceXmlDocument.Load( pathToExecutingAssembly + @"\" + _filePathOrUrl);
            return new RuleFields(sourceXmlDocument);
        }


    }
}


