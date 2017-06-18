using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{
    public class RuleOperators
    {
        XmlDocument _sourceXmlDocument = null;

        public XmlDocument SourceXmlDocument
        {
            get { return _sourceXmlDocument; }
            set { _sourceXmlDocument = value; }
        }

        public RuleOperators()
        {
        }

        public RuleOperators(XmlDocument sourceXmlDocument)
        {
            Load(sourceXmlDocument);
        }

        public RuleOperators(string stringOfXmlFromSql)
        {
            Load(stringOfXmlFromSql);
        }

        private void Load(string stringOfXmlFromSql)
        {
            XmlDocument sourceXmlDocument = new XmlDocument();
            sourceXmlDocument.LoadXml(stringOfXmlFromSql);
            Load(sourceXmlDocument);
        }

        private void Load(XmlDocument sourceXmlDocument)
        {
            _sourceXmlDocument = sourceXmlDocument;
        }


    }
}
