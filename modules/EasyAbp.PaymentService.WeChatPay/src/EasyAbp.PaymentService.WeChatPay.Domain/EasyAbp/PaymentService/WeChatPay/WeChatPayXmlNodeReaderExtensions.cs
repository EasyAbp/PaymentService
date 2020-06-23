using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;

namespace EasyAbp.PaymentService.WeChatPay
{
    public static class WeChatPayXmlNodeReaderExtensions
    {
        public static string JoinNodesInnerTextAsString(this XmlNodeReader reader, string prefix, int count, string separator = ",")
        {
            var innerTexts = new List<string>();
            
            for (var i = 0; i < count; i++)
            {
                var nodeName = prefix + i;

                var innerText = reader[nodeName];

                if (innerText != null)
                {
                    innerTexts.Add(innerText);
                }
            }

            return innerTexts.JoinAsString(separator);
        }
    }
}