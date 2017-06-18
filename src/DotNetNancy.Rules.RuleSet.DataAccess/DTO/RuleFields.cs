using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;

namespace DotNetNancy.Rules.RuleSet.DataAccess.DTO
{
    public class RuleFields
    {
        private XmlDocument _sourceXmlDocument = null;

        public XmlDocument SourceXmlDocument
        {
            get { return _sourceXmlDocument; }
            set { _sourceXmlDocument = value; }
        }

        public RuleFields()
        {
        }

        public RuleFields(XmlDocument sourceXmlDocument)
        {
            Load(sourceXmlDocument);
        }

        public RuleFields(string stringOfXmlFromSql)
        {
            Load(stringOfXmlFromSql);
        }

        private void Load(string stringOfXmlFromSql)
        {
            XmlDocument sourceXmlDocument = new XmlDocument();
            sourceXmlDocument.LoadXml(stringOfXmlFromSql);
            Load(sourceXmlDocument);
        }

        /// <summary>
        /// TODO:  fill this dto with members and then fill those members values from the xml document
        /// </summary>
        /// <param name="sourceXmlDocument"></param>
        private void Load(XmlDocument sourceXmlDocument)
        {
            _sourceXmlDocument = sourceXmlDocument;
            //throw new NotImplementedException();
        }
    }
}
