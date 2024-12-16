using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;

namespace IPS_A
{
    internal static class Settings
    {
        private static string Enc(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return System.Convert.ToBase64String(bytes);

        }

        private static string Dec(string text)
        {
            var s = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(s);
        }

        public static void Save(string s)
        {
            XDocument xDocument = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("root", new XAttribute("value", Enc(s))));
            xDocument.Save(Application.ProductName);
        }

        public static string Restore()
        {
            XDocument xDocument = XDocument.Load(Application.ProductName);
            var val = xDocument.Descendants("root").First().Attribute("value").Value;
            return Dec(val);
        }
    }
}
