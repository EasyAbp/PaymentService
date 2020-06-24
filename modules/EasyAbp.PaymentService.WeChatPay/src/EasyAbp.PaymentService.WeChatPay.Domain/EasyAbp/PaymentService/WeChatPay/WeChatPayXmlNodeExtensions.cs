using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace EasyAbp.PaymentService.WeChatPay
{
    public static class WeChatPayXmlNodeExtensions
    {
        public static Dictionary<string, string> ToDictionary(this XmlNode doc)
        {
            return doc.ChildNodes.Cast<XmlNode>()
                .Where(node => node.ChildNodes.Count == 1 && !node.FirstChild.HasChildNodes)
                .ToDictionary(node => node.LocalName, node => node.FirstChild.Value);
        }
    }
}