using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAppConfig
{
    public class RegisterCompaniesConfig : ConfigurationSection
    {

        public static RegisterCompaniesConfig GetConfig()
        {
            return (RegisterCompaniesConfig)System.Configuration.ConfigurationManager.GetSection("RegisterCompanies") ?? new RegisterCompaniesConfig();
        }

        [ConfigurationProperty("Companies")]
        [ConfigurationCollection(typeof(Companies), AddItemName = "Company")]
        public Companies Companies
        {
            get
            {
                object o = this["Companies"];
                return o as Companies;
            }
        }

    }

    public class Companies : ConfigurationElementCollection
    {
        public Company this[int index]
        {
            get
            {
                return base.BaseGet(index) as Company;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public new Company this[string responseString]
        {
            get { return (Company)BaseGet(responseString); }
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new Company();
        }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            return ((Company)element).Name;
        }
    }

    
    public class Company : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return base["name"] as string;
            }
        }
        [ConfigurationProperty("code", IsRequired = true)]
        public string Code
        {
            get
            {
                return base["code"] as string;
            }
        }
    }
}
