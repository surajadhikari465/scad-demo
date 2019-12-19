using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace WFM.Mobile.OOS
{
    public static class OOSSettings
    {

        private static XmlDocument _xml;

        public static void Load()
        {
            _xml = new XmlDocument();
            _xml.Load("OOSSettings.xml");

        }
    }
}
