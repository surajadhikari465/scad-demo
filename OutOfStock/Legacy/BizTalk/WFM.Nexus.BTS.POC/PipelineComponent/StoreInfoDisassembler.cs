using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.Runtime.Serialization.Json;
using WFM.Nexus.Pipeline.Json.Model;

namespace WFM.Nexus.Pipeline.Json
{

    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Decoder)]
    [System.Runtime.InteropServices.Guid("CDB5D0D8-837F-4BE3-A640-16AAE24DFB50")]
    public class StoreInfoDisassembler :
                                        IBaseComponent,
                                        IComponentUI,
                                        IComponent,
                                        IPersistPropertyBag
   
    {


        #region IBaseComponent Members

        public string Description
        {
            get
            {
                return "Disassembles StoreInfo JSON";
            }
        }
        public string Name
        {
            get
            {
                return "WFM.Nexus.Pipeline.Json.StoreInfoDisassembler";
            }
        }
        public string Version
        {
            get
            {
                return "1.0.0.0";
            }
        }
        #endregion

        #region IComponentUI Members

        public IntPtr Icon
        {
            get
            {
                return new System.IntPtr();
            }
        }

        public System.Collections.IEnumerator Validate(object projectSystem)
        {
            return null;
        }

        #endregion

        #region IPersistPropertyBag Members

        //private string _Url;
        //public string Url
        //{
        //    get { return _Url; }
        //    set { _Url = value; }
        //}

        //private string _UserName;
        //public string UserName
        //{
        //    get { return _UserName; }
        //    set { _UserName = value; }
        //}

        //private string _Password;
        //public string Password
        //{
        //    get { return _Password; }
        //    set { _Password = value; }
        //}

        public void GetClassID(out Guid classID)
        {
            classID = new Guid("CDB5D0D8-837F-4BE3-A640-16AAE24DFB50");
        }

        public void InitNew()
        {

        }

        //private const string PropUrl = "Url";
        //private const string PropUserName = "UserName";
        //private const string PropPassword = "Password";

        public void Load(IPropertyBag propertyBag, int errorLog)
        {
            //_Url = ReadProperty(propertyBag, PropUrl, "http://www.wholefoodsmarket.com/common/irest/stores/");
            //_UserName = ReadProperty(propertyBag, PropUserName, "");
            //_Password = ReadProperty(propertyBag, PropPassword, "");
        }

        private string ReadProperty(IPropertyBag propertyBag, string Name, string defaultValue)
        {
            object val = null;
            try
            {
                propertyBag.Read(Name, out val, 0);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error reading propertybag: " + ex.Message);
            }

            string returnValue = defaultValue;
            if (val != null)
                returnValue = (string)val;

            return returnValue;
        }

        public void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            //WriteProperty(propertyBag, PropUrl, _Url);
            //WriteProperty(propertyBag, PropUserName, _UserName);
            //WriteProperty(propertyBag, PropPassword, _Password);   
        }

        private static void WriteProperty(IPropertyBag propertyBag, string PropertyName, string value)
        {
            object val = (object)value;
            propertyBag.Write(PropertyName, ref val);
        }

        #endregion

        #region IComponent Members

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            IBaseMessagePart bodyPart = pInMsg.BodyPart;
            string systemPropertiesNamespace = @"http://schemas.microsoft.com/BizTalk/2003/system-properties";
            string messageType = "";
            if (bodyPart != null)
            {
                Stream originalStream = bodyPart.GetOriginalDataStream();
                if (originalStream != null)
                {
                    //load entire JSON into an object model using a JSON deserializer.
                    StoresResult storesResult = DeserializeJSON<StoresResult>(originalStream);


                    //Serialize the same object model into XML, and put into the output stream
                    //note: for large messages this will use a lot of memory
                    MemoryStream memStream = SerializeXml<StoresResult>(storesResult);

                    memStream.Position = 0;
                    bodyPart.Data = memStream;
                    pContext.ResourceTracker.AddResource(memStream);
                }
            }

            string sNamespace = "http://api.wholelabs.com/v1/stores";
            string rootName = "StoresResult";

            messageType = sNamespace + "#" + rootName;
            pInMsg.Context.Promote("MessageType", systemPropertiesNamespace, messageType);
            return pInMsg;
        }

        public static MemoryStream SerializeXml<T>(T stores)
        {
            MemoryStream memStream = new MemoryStream();
            System.Xml.Serialization.XmlSerializer xSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            xSerializer.Serialize(memStream, stores);
            return memStream;
        }

        public static T DeserializeJSON<T>(Stream originalStream)
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(T));
            T _data = (T)jsonSerializer.ReadObject(originalStream);
            return _data;
        }

        #endregion
    }
}
