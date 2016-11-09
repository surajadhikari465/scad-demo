﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.Schemas.Wfm.Contracts
{
    using System.Xml.Serialization;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Address/V2")]
    [System.Xml.Serialization.XmlRootAttribute("address", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Address/V2", IsNullable = false)]
    public partial class AddressType
    {

        private int idField;

        private bool idFieldSpecified;

        private AddressUsageType usageField;

        private AddressTypeType typeField;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idSpecified
        {
            get
            {
                return this.idFieldSpecified;
            }
            set
            {
                this.idFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AddressUsageType usage
        {
            get
            {
                return this.usageField;
            }
            set
            {
                this.usageField = value;
            }
        }

        /// <remarks/>
        public AddressTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/AddressUsage/V2")]
    [System.Xml.Serialization.XmlRootAttribute("addressUsage", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/AddressUsage/V2", IsNullable = false)]
    public partial class AddressUsageType
    {

        private string codeField;

        private AddressUsgDescType descriptionField;

        private bool isPrimaryField;

        private bool isPrimaryFieldSpecified;

        private bool isPrivateField;

        private bool isPrivateFieldSpecified;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public AddressUsgDescType description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsPrimary
        {
            get
            {
                return this.isPrimaryField;
            }
            set
            {
                this.isPrimaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrimarySpecified
        {
            get
            {
                return this.isPrimaryFieldSpecified;
            }
            set
            {
                this.isPrimaryFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool IsPrivate
        {
            get
            {
                return this.isPrivateField;
            }
            set
            {
                this.isPrivateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsPrivateSpecified
        {
            get
            {
                return this.isPrivateFieldSpecified;
            }
            set
            {
                this.isPrivateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum AddressUsgDescType
    {

        /// <remarks/>
        billing,

        /// <remarks/>
        business,

        /// <remarks/>
        fax,

        /// <remarks/>
        home,

        /// <remarks/>
        legal,

        /// <remarks/>
        mobile,

        /// <remarks/>
        work,

        /// <remarks/>
        street,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/PhoneAddress/V1")]
    [System.Xml.Serialization.XmlRootAttribute("phone", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/PhoneAddress/V1", IsNullable = false)]
    public partial class PhoneType
    {

        private ushort internationalCountryCodeField;

        private bool internationalCountryCodeFieldSpecified;

        private uint areaCodeField;

        private bool areaCodeFieldSpecified;

        private ulong subscriberNumberField;

        private bool subscriberNumberFieldSpecified;

        private ulong extensionNumberField;

        private bool extensionNumberFieldSpecified;

        /// <remarks/>
        public ushort internationalCountryCode
        {
            get
            {
                return this.internationalCountryCodeField;
            }
            set
            {
                this.internationalCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool internationalCountryCodeSpecified
        {
            get
            {
                return this.internationalCountryCodeFieldSpecified;
            }
            set
            {
                this.internationalCountryCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public uint areaCode
        {
            get
            {
                return this.areaCodeField;
            }
            set
            {
                this.areaCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool areaCodeSpecified
        {
            get
            {
                return this.areaCodeFieldSpecified;
            }
            set
            {
                this.areaCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ulong subscriberNumber
        {
            get
            {
                return this.subscriberNumberField;
            }
            set
            {
                this.subscriberNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool subscriberNumberSpecified
        {
            get
            {
                return this.subscriberNumberFieldSpecified;
            }
            set
            {
                this.subscriberNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ulong extensionNumber
        {
            get
            {
                return this.extensionNumberField;
            }
            set
            {
                this.extensionNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool extensionNumberSpecified
        {
            get
            {
                return this.extensionNumberFieldSpecified;
            }
            set
            {
                this.extensionNumberFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/Timezone/V2")]
    [System.Xml.Serialization.XmlRootAttribute("timezone", Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/Timezone/V2", IsNullable = false)]
    public partial class TimezoneType
    {

        private string codeField;

        private TimezoneNameType nameField;

        private bool nameFieldSpecified;

        private GmtOffsetType gmtOffsetField;

        private bool gmtOffsetFieldSpecified;

        private System.DateTime dstStartField;

        private bool dstStartFieldSpecified;

        private System.DateTime dstEndField;

        private bool dstEndFieldSpecified;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public TimezoneNameType name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool nameSpecified
        {
            get
            {
                return this.nameFieldSpecified;
            }
            set
            {
                this.nameFieldSpecified = value;
            }
        }

        /// <remarks/>
        public GmtOffsetType gmtOffset
        {
            get
            {
                return this.gmtOffsetField;
            }
            set
            {
                this.gmtOffsetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool gmtOffsetSpecified
        {
            get
            {
                return this.gmtOffsetFieldSpecified;
            }
            set
            {
                this.gmtOffsetFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime dstStart
        {
            get
            {
                return this.dstStartField;
            }
            set
            {
                this.dstStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dstStartSpecified
        {
            get
            {
                return this.dstStartFieldSpecified;
            }
            set
            {
                this.dstStartFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime dstEnd
        {
            get
            {
                return this.dstEndField;
            }
            set
            {
                this.dstEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dstEndSpecified
        {
            get
            {
                return this.dstEndFieldSpecified;
            }
            set
            {
                this.dstEndFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum TimezoneNameType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Abidjan")]
        AfricaAbidjan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Accra")]
        AfricaAccra,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Addis_Ababa")]
        AfricaAddis_Ababa,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Algiers")]
        AfricaAlgiers,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Asmara")]
        AfricaAsmara,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Asmera")]
        AfricaAsmera,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Bamako")]
        AfricaBamako,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Bangui")]
        AfricaBangui,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Banjul")]
        AfricaBanjul,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Bissau")]
        AfricaBissau,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Blantyre")]
        AfricaBlantyre,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Brazzaville")]
        AfricaBrazzaville,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Bujumbura")]
        AfricaBujumbura,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Cairo")]
        AfricaCairo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Casablanca")]
        AfricaCasablanca,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Ceuta")]
        AfricaCeuta,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Conakry")]
        AfricaConakry,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Dakar")]
        AfricaDakar,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Dar_es_Salaam")]
        AfricaDar_es_Salaam,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Djibouti")]
        AfricaDjibouti,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Douala")]
        AfricaDouala,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/El_Aaiun")]
        AfricaEl_Aaiun,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Freetown")]
        AfricaFreetown,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Gaborone")]
        AfricaGaborone,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Harare")]
        AfricaHarare,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Johannesburg")]
        AfricaJohannesburg,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Juba")]
        AfricaJuba,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Kampala")]
        AfricaKampala,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Khartoum")]
        AfricaKhartoum,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Kigali")]
        AfricaKigali,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Kinshasa")]
        AfricaKinshasa,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Lagos")]
        AfricaLagos,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Libreville")]
        AfricaLibreville,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Lome")]
        AfricaLome,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Luanda")]
        AfricaLuanda,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Lubumbashi")]
        AfricaLubumbashi,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Lusaka")]
        AfricaLusaka,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Malabo")]
        AfricaMalabo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Maputo")]
        AfricaMaputo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Maseru")]
        AfricaMaseru,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Mbabane")]
        AfricaMbabane,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Mogadishu")]
        AfricaMogadishu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Monrovia")]
        AfricaMonrovia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Nairobi")]
        AfricaNairobi,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Ndjamena")]
        AfricaNdjamena,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Niamey")]
        AfricaNiamey,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Nouakchott")]
        AfricaNouakchott,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Ouagadougou")]
        AfricaOuagadougou,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Porto-Novo")]
        AfricaPortoNovo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Sao_Tome")]
        AfricaSao_Tome,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Timbuktu")]
        AfricaTimbuktu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Tripoli")]
        AfricaTripoli,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Tunis")]
        AfricaTunis,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Africa/Windhoek")]
        AfricaWindhoek,

        /// <remarks/>
        AKST9AKDT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Adak")]
        AmericaAdak,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Anchorage")]
        AmericaAnchorage,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Anguilla")]
        AmericaAnguilla,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Antigua")]
        AmericaAntigua,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Araguaina")]
        AmericaAraguaina,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Buenos_Aires")]
        AmericaArgentinaBuenos_Aires,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Catamarca")]
        AmericaArgentinaCatamarca,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/ComodRivadavia")]
        AmericaArgentinaComodRivadavia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Cordoba")]
        AmericaArgentinaCordoba,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Jujuy")]
        AmericaArgentinaJujuy,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/La_Rioja")]
        AmericaArgentinaLa_Rioja,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Mendoza")]
        AmericaArgentinaMendoza,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Rio_Gallegos")]
        AmericaArgentinaRio_Gallegos,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Salta")]
        AmericaArgentinaSalta,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/San_Juan")]
        AmericaArgentinaSan_Juan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/San_Luis")]
        AmericaArgentinaSan_Luis,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Tucuman")]
        AmericaArgentinaTucuman,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Argentina/Ushuaia")]
        AmericaArgentinaUshuaia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Aruba")]
        AmericaAruba,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Asuncion")]
        AmericaAsuncion,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Atikokan")]
        AmericaAtikokan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Atka")]
        AmericaAtka,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Bahia")]
        AmericaBahia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Bahia_Banderas")]
        AmericaBahia_Banderas,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Barbados")]
        AmericaBarbados,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Belem")]
        AmericaBelem,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Belize")]
        AmericaBelize,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Blanc-Sablon")]
        AmericaBlancSablon,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Boa_Vista")]
        AmericaBoa_Vista,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Bogota")]
        AmericaBogota,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Boise")]
        AmericaBoise,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Buenos_Aires")]
        AmericaBuenos_Aires,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Cambridge_Bay")]
        AmericaCambridge_Bay,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Campo_Grande")]
        AmericaCampo_Grande,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Cancun")]
        AmericaCancun,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Caracas")]
        AmericaCaracas,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Catamarca")]
        AmericaCatamarca,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Cayenne")]
        AmericaCayenne,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Cayman")]
        AmericaCayman,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Chicago")]
        AmericaChicago,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Chihuahua")]
        AmericaChihuahua,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Coral_Harbour")]
        AmericaCoral_Harbour,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Cordoba")]
        AmericaCordoba,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Costa_Rica")]
        AmericaCosta_Rica,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Creston")]
        AmericaCreston,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Cuiaba")]
        AmericaCuiaba,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Curacao")]
        AmericaCuracao,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Danmarkshavn")]
        AmericaDanmarkshavn,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Dawson")]
        AmericaDawson,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Dawson_Creek")]
        AmericaDawson_Creek,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Denver")]
        AmericaDenver,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Detroit")]
        AmericaDetroit,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Dominica")]
        AmericaDominica,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Edmonton")]
        AmericaEdmonton,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Eirunepe")]
        AmericaEirunepe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/El_Salvador")]
        AmericaEl_Salvador,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Ensenada")]
        AmericaEnsenada,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Fort_Wayne")]
        AmericaFort_Wayne,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Fortaleza")]
        AmericaFortaleza,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Glace_Bay")]
        AmericaGlace_Bay,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Godthab")]
        AmericaGodthab,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Goose_Bay")]
        AmericaGoose_Bay,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Grand_Turk")]
        AmericaGrand_Turk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Grenada")]
        AmericaGrenada,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Guadeloupe")]
        AmericaGuadeloupe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Guatemala")]
        AmericaGuatemala,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Guayaquil")]
        AmericaGuayaquil,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Guyana")]
        AmericaGuyana,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Halifax")]
        AmericaHalifax,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Havana")]
        AmericaHavana,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Hermosillo")]
        AmericaHermosillo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Indianapolis")]
        AmericaIndianaIndianapolis,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Knox")]
        AmericaIndianaKnox,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Marengo")]
        AmericaIndianaMarengo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Petersburg")]
        AmericaIndianaPetersburg,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Tell_City")]
        AmericaIndianaTell_City,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Vevay")]
        AmericaIndianaVevay,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Vincennes")]
        AmericaIndianaVincennes,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indiana/Winamac")]
        AmericaIndianaWinamac,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Indianapolis")]
        AmericaIndianapolis,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Inuvik")]
        AmericaInuvik,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Iqaluit")]
        AmericaIqaluit,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Jamaica")]
        AmericaJamaica,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Jujuy")]
        AmericaJujuy,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Juneau")]
        AmericaJuneau,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Kentucky/Louisville")]
        AmericaKentuckyLouisville,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Kentucky/Monticello")]
        AmericaKentuckyMonticello,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Knox_IN")]
        AmericaKnox_IN,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Kralendijk")]
        AmericaKralendijk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/La_Paz")]
        AmericaLa_Paz,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Lima")]
        AmericaLima,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Los_Angeles")]
        AmericaLos_Angeles,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Louisville")]
        AmericaLouisville,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Lower_Princes")]
        AmericaLower_Princes,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Maceio")]
        AmericaMaceio,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Managua")]
        AmericaManagua,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Manaus")]
        AmericaManaus,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Marigot")]
        AmericaMarigot,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Martinique")]
        AmericaMartinique,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Matamoros")]
        AmericaMatamoros,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Mazatlan")]
        AmericaMazatlan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Mendoza")]
        AmericaMendoza,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Menominee")]
        AmericaMenominee,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Merida")]
        AmericaMerida,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Metlakatla")]
        AmericaMetlakatla,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Mexico_City")]
        AmericaMexico_City,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Miquelon")]
        AmericaMiquelon,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Moncton")]
        AmericaMoncton,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Monterrey")]
        AmericaMonterrey,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Montevideo")]
        AmericaMontevideo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Montreal")]
        AmericaMontreal,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Montserrat")]
        AmericaMontserrat,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Nassau")]
        AmericaNassau,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/New_York")]
        AmericaNew_York,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Nipigon")]
        AmericaNipigon,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Nome")]
        AmericaNome,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Noronha")]
        AmericaNoronha,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/North_Dakota/Beulah")]
        AmericaNorth_DakotaBeulah,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/North_Dakota/Center")]
        AmericaNorth_DakotaCenter,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/North_Dakota/New_Salem")]
        AmericaNorth_DakotaNew_Salem,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Ojinaga")]
        AmericaOjinaga,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Panama")]
        AmericaPanama,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Pangnirtung")]
        AmericaPangnirtung,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Paramaribo")]
        AmericaParamaribo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Phoenix")]
        AmericaPhoenix,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Port_of_Spain")]
        AmericaPort_of_Spain,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Port-au-Prince")]
        AmericaPortauPrince,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Porto_Acre")]
        AmericaPorto_Acre,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Porto_Velho")]
        AmericaPorto_Velho,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Puerto_Rico")]
        AmericaPuerto_Rico,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Rainy_River")]
        AmericaRainy_River,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Rankin_Inlet")]
        AmericaRankin_Inlet,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Recife")]
        AmericaRecife,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Regina")]
        AmericaRegina,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Resolute")]
        AmericaResolute,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Rio_Branco")]
        AmericaRio_Branco,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Rosario")]
        AmericaRosario,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Santa_Isabel")]
        AmericaSanta_Isabel,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Santarem")]
        AmericaSantarem,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Santiago")]
        AmericaSantiago,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Santo_Domingo")]
        AmericaSanto_Domingo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Sao_Paulo")]
        AmericaSao_Paulo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Scoresbysund")]
        AmericaScoresbysund,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Shiprock")]
        AmericaShiprock,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Sitka")]
        AmericaSitka,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/St_Barthelemy")]
        AmericaSt_Barthelemy,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/St_Johns")]
        AmericaSt_Johns,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/St_Kitts")]
        AmericaSt_Kitts,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/St_Lucia")]
        AmericaSt_Lucia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/St_Thomas")]
        AmericaSt_Thomas,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/St_Vincent")]
        AmericaSt_Vincent,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Swift_Current")]
        AmericaSwift_Current,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Tegucigalpa")]
        AmericaTegucigalpa,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Thule")]
        AmericaThule,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Thunder_Bay")]
        AmericaThunder_Bay,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Tijuana")]
        AmericaTijuana,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Toronto")]
        AmericaToronto,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Tortola")]
        AmericaTortola,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Vancouver")]
        AmericaVancouver,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Virgin")]
        AmericaVirgin,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Whitehorse")]
        AmericaWhitehorse,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Winnipeg")]
        AmericaWinnipeg,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Yakutat")]
        AmericaYakutat,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("America/Yellowknife")]
        AmericaYellowknife,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Casey")]
        AntarcticaCasey,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Davis")]
        AntarcticaDavis,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/DumontDUrville")]
        AntarcticaDumontDUrville,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Macquarie")]
        AntarcticaMacquarie,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Mawson")]
        AntarcticaMawson,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/McMurdo")]
        AntarcticaMcMurdo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Palmer")]
        AntarcticaPalmer,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Rothera")]
        AntarcticaRothera,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/South_Pole")]
        AntarcticaSouth_Pole,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Syowa")]
        AntarcticaSyowa,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Antarctica/Vostok")]
        AntarcticaVostok,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Arctic/Longyearbyen")]
        ArcticLongyearbyen,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Aden")]
        AsiaAden,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Almaty")]
        AsiaAlmaty,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Amman")]
        AsiaAmman,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Anadyr")]
        AsiaAnadyr,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Aqtau")]
        AsiaAqtau,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Aqtobe")]
        AsiaAqtobe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Ashgabat")]
        AsiaAshgabat,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Ashkhabad")]
        AsiaAshkhabad,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Baghdad")]
        AsiaBaghdad,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Bahrain")]
        AsiaBahrain,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Baku")]
        AsiaBaku,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Bangkok")]
        AsiaBangkok,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Beirut")]
        AsiaBeirut,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Bishkek")]
        AsiaBishkek,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Brunei")]
        AsiaBrunei,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Calcutta")]
        AsiaCalcutta,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Choibalsan")]
        AsiaChoibalsan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Chongqing")]
        AsiaChongqing,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Chungking")]
        AsiaChungking,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Colombo")]
        AsiaColombo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Dacca")]
        AsiaDacca,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Damascus")]
        AsiaDamascus,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Dhaka")]
        AsiaDhaka,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Dili")]
        AsiaDili,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Dubai")]
        AsiaDubai,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Dushanbe")]
        AsiaDushanbe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Gaza")]
        AsiaGaza,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Harbin")]
        AsiaHarbin,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Hebron")]
        AsiaHebron,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Ho_Chi_Minh")]
        AsiaHo_Chi_Minh,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Hong_Kong")]
        AsiaHong_Kong,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Hovd")]
        AsiaHovd,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Irkutsk")]
        AsiaIrkutsk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Istanbul")]
        AsiaIstanbul,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Jakarta")]
        AsiaJakarta,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Jayapura")]
        AsiaJayapura,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Jerusalem")]
        AsiaJerusalem,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kabul")]
        AsiaKabul,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kamchatka")]
        AsiaKamchatka,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Karachi")]
        AsiaKarachi,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kashgar")]
        AsiaKashgar,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kathmandu")]
        AsiaKathmandu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Katmandu")]
        AsiaKatmandu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kolkata")]
        AsiaKolkata,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Krasnoyarsk")]
        AsiaKrasnoyarsk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kuala_Lumpur")]
        AsiaKuala_Lumpur,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kuching")]
        AsiaKuching,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Kuwait")]
        AsiaKuwait,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Macao")]
        AsiaMacao,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Macau")]
        AsiaMacau,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Magadan")]
        AsiaMagadan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Makassar")]
        AsiaMakassar,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Manila")]
        AsiaManila,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Muscat")]
        AsiaMuscat,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Nicosia")]
        AsiaNicosia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Novokuznetsk")]
        AsiaNovokuznetsk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Novosibirsk")]
        AsiaNovosibirsk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Omsk")]
        AsiaOmsk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Oral")]
        AsiaOral,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Phnom_Penh")]
        AsiaPhnom_Penh,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Pontianak")]
        AsiaPontianak,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Pyongyang")]
        AsiaPyongyang,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Qatar")]
        AsiaQatar,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Qyzylorda")]
        AsiaQyzylorda,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Rangoon")]
        AsiaRangoon,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Riyadh")]
        AsiaRiyadh,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Saigon")]
        AsiaSaigon,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Sakhalin")]
        AsiaSakhalin,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Samarkand")]
        AsiaSamarkand,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Seoul")]
        AsiaSeoul,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Shanghai")]
        AsiaShanghai,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Singapore")]
        AsiaSingapore,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Taipei")]
        AsiaTaipei,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Tashkent")]
        AsiaTashkent,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Tbilisi")]
        AsiaTbilisi,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Tehran")]
        AsiaTehran,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Tel_Aviv")]
        AsiaTel_Aviv,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Thimbu")]
        AsiaThimbu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Thimphu")]
        AsiaThimphu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Tokyo")]
        AsiaTokyo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Ujung_Pandang")]
        AsiaUjung_Pandang,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Ulaanbaatar")]
        AsiaUlaanbaatar,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Ulan_Bator")]
        AsiaUlan_Bator,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Urumqi")]
        AsiaUrumqi,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Vientiane")]
        AsiaVientiane,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Vladivostok")]
        AsiaVladivostok,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Yakutsk")]
        AsiaYakutsk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Yekaterinburg")]
        AsiaYekaterinburg,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Asia/Yerevan")]
        AsiaYerevan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Azores")]
        AtlanticAzores,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Bermuda")]
        AtlanticBermuda,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Canary")]
        AtlanticCanary,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Cape_Verde")]
        AtlanticCape_Verde,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Faeroe")]
        AtlanticFaeroe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Faroe")]
        AtlanticFaroe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Jan_Mayen")]
        AtlanticJan_Mayen,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Madeira")]
        AtlanticMadeira,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Reykjavik")]
        AtlanticReykjavik,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/South_Georgia")]
        AtlanticSouth_Georgia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/St_Helena")]
        AtlanticSt_Helena,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Atlantic/Stanley")]
        AtlanticStanley,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/ACT")]
        AustraliaACT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Adelaide")]
        AustraliaAdelaide,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Brisbane")]
        AustraliaBrisbane,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Broken_Hill")]
        AustraliaBroken_Hill,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Canberra")]
        AustraliaCanberra,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Currie")]
        AustraliaCurrie,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Darwin")]
        AustraliaDarwin,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Eucla")]
        AustraliaEucla,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Hobart")]
        AustraliaHobart,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/LHI")]
        AustraliaLHI,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Lindeman")]
        AustraliaLindeman,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Lord_Howe")]
        AustraliaLord_Howe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Melbourne")]
        AustraliaMelbourne,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/North")]
        AustraliaNorth,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/NSW")]
        AustraliaNSW,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Perth")]
        AustraliaPerth,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Queensland")]
        AustraliaQueensland,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/South")]
        AustraliaSouth,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Sydney")]
        AustraliaSydney,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Tasmania")]
        AustraliaTasmania,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Victoria")]
        AustraliaVictoria,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/West")]
        AustraliaWest,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Australia/Yancowinna")]
        AustraliaYancowinna,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Brazil/Acre")]
        BrazilAcre,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Brazil/DeNoronha")]
        BrazilDeNoronha,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Brazil/East")]
        BrazilEast,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Brazil/West")]
        BrazilWest,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Atlantic")]
        CanadaAtlantic,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Central")]
        CanadaCentral,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Eastern")]
        CanadaEastern,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/East-Saskatchewan")]
        CanadaEastSaskatchewan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Mountain")]
        CanadaMountain,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Newfoundland")]
        CanadaNewfoundland,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Pacific")]
        CanadaPacific,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Saskatchewan")]
        CanadaSaskatchewan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Canada/Yukon")]
        CanadaYukon,

        /// <remarks/>
        CET,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Chile/Continental")]
        ChileContinental,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Chile/EasterIsland")]
        ChileEasterIsland,

        /// <remarks/>
        CST6CDT,

        /// <remarks/>
        Cuba,

        /// <remarks/>
        EET,

        /// <remarks/>
        Egypt,

        /// <remarks/>
        Eire,

        /// <remarks/>
        EST,

        /// <remarks/>
        EST5EDT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Etc/GMT")]
        EtcGMT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Etc/GMT+0")]
        EtcGMT0,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Etc/UCT")]
        EtcUCT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Etc/Universal")]
        EtcUniversal,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Etc/UTC")]
        EtcUTC,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Etc/Zulu")]
        EtcZulu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Amsterdam")]
        EuropeAmsterdam,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Andorra")]
        EuropeAndorra,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Athens")]
        EuropeAthens,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Belfast")]
        EuropeBelfast,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Belgrade")]
        EuropeBelgrade,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Berlin")]
        EuropeBerlin,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Bratislava")]
        EuropeBratislava,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Brussels")]
        EuropeBrussels,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Bucharest")]
        EuropeBucharest,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Budapest")]
        EuropeBudapest,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Chisinau")]
        EuropeChisinau,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Copenhagen")]
        EuropeCopenhagen,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Dublin")]
        EuropeDublin,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Gibraltar")]
        EuropeGibraltar,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Guernsey")]
        EuropeGuernsey,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Helsinki")]
        EuropeHelsinki,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Isle_of_Man")]
        EuropeIsle_of_Man,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Istanbul")]
        EuropeIstanbul,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Jersey")]
        EuropeJersey,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Kaliningrad")]
        EuropeKaliningrad,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Kiev")]
        EuropeKiev,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Lisbon")]
        EuropeLisbon,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Ljubljana")]
        EuropeLjubljana,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/London")]
        EuropeLondon,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Luxembourg")]
        EuropeLuxembourg,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Madrid")]
        EuropeMadrid,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Malta")]
        EuropeMalta,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Mariehamn")]
        EuropeMariehamn,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Minsk")]
        EuropeMinsk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Monaco")]
        EuropeMonaco,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Moscow")]
        EuropeMoscow,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Nicosia")]
        EuropeNicosia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Oslo")]
        EuropeOslo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Paris")]
        EuropeParis,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Podgorica")]
        EuropePodgorica,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Prague")]
        EuropePrague,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Riga")]
        EuropeRiga,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Rome")]
        EuropeRome,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Samara")]
        EuropeSamara,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/San_Marino")]
        EuropeSan_Marino,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Sarajevo")]
        EuropeSarajevo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Simferopol")]
        EuropeSimferopol,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Skopje")]
        EuropeSkopje,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Sofia")]
        EuropeSofia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Stockholm")]
        EuropeStockholm,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Tallinn")]
        EuropeTallinn,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Tirane")]
        EuropeTirane,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Tiraspol")]
        EuropeTiraspol,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Uzhgorod")]
        EuropeUzhgorod,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Vaduz")]
        EuropeVaduz,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Vatican")]
        EuropeVatican,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Vienna")]
        EuropeVienna,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Vilnius")]
        EuropeVilnius,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Volgograd")]
        EuropeVolgograd,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Warsaw")]
        EuropeWarsaw,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Zagreb")]
        EuropeZagreb,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Zaporozhye")]
        EuropeZaporozhye,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Europe/Zurich")]
        EuropeZurich,

        /// <remarks/>
        GB,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GB-Eire")]
        GBEire,

        /// <remarks/>
        GMT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GMT+0")]
        GMT0,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GMT0")]
        GMT01,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("GMT-0")]
        GMT02,

        /// <remarks/>
        Greenwich,

        /// <remarks/>
        Hongkong,

        /// <remarks/>
        HST,

        /// <remarks/>
        Iceland,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Antananarivo")]
        IndianAntananarivo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Chagos")]
        IndianChagos,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Christmas")]
        IndianChristmas,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Cocos")]
        IndianCocos,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Comoro")]
        IndianComoro,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Kerguelen")]
        IndianKerguelen,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Mahe")]
        IndianMahe,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Maldives")]
        IndianMaldives,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Mauritius")]
        IndianMauritius,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Mayotte")]
        IndianMayotte,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Indian/Reunion")]
        IndianReunion,

        /// <remarks/>
        Iran,

        /// <remarks/>
        Israel,

        /// <remarks/>
        Jamaica,

        /// <remarks/>
        Japan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("JST-9")]
        JST9,

        /// <remarks/>
        Kwajalein,

        /// <remarks/>
        Libya,

        /// <remarks/>
        MET,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Mexico/BajaNorte")]
        MexicoBajaNorte,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Mexico/BajaSur")]
        MexicoBajaSur,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Mexico/General")]
        MexicoGeneral,

        /// <remarks/>
        MST,

        /// <remarks/>
        MST7MDT,

        /// <remarks/>
        Navajo,

        /// <remarks/>
        NZ,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("NZ-CHAT")]
        NZCHAT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Apia")]
        PacificApia,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Auckland")]
        PacificAuckland,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Chatham")]
        PacificChatham,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Chuuk")]
        PacificChuuk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Easter")]
        PacificEaster,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Efate")]
        PacificEfate,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Enderbury")]
        PacificEnderbury,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Fakaofo")]
        PacificFakaofo,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Fiji")]
        PacificFiji,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Funafuti")]
        PacificFunafuti,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Galapagos")]
        PacificGalapagos,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Gambier")]
        PacificGambier,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Guadalcanal")]
        PacificGuadalcanal,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Guam")]
        PacificGuam,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Honolulu")]
        PacificHonolulu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Johnston")]
        PacificJohnston,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Kiritimati")]
        PacificKiritimati,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Kosrae")]
        PacificKosrae,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Kwajalein")]
        PacificKwajalein,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Majuro")]
        PacificMajuro,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Marquesas")]
        PacificMarquesas,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Midway")]
        PacificMidway,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Nauru")]
        PacificNauru,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Niue")]
        PacificNiue,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Norfolk")]
        PacificNorfolk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Noumea")]
        PacificNoumea,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Pago_Pago")]
        PacificPago_Pago,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Palau")]
        PacificPalau,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Pitcairn")]
        PacificPitcairn,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Pohnpei")]
        PacificPohnpei,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Ponape")]
        PacificPonape,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Port_Moresby")]
        PacificPort_Moresby,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Rarotonga")]
        PacificRarotonga,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Saipan")]
        PacificSaipan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Samoa")]
        PacificSamoa,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Tahiti")]
        PacificTahiti,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Tarawa")]
        PacificTarawa,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Tongatapu")]
        PacificTongatapu,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Truk")]
        PacificTruk,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Wake")]
        PacificWake,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Wallis")]
        PacificWallis,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Pacific/Yap")]
        PacificYap,

        /// <remarks/>
        Poland,

        /// <remarks/>
        Portugal,

        /// <remarks/>
        PRC,

        /// <remarks/>
        PST8PDT,

        /// <remarks/>
        ROC,

        /// <remarks/>
        ROK,

        /// <remarks/>
        Singapore,

        /// <remarks/>
        Turkey,

        /// <remarks/>
        UCT,

        /// <remarks/>
        Universal,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Alaska")]
        USAlaska,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Aleutian")]
        USAleutian,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Arizona")]
        USArizona,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Central")]
        USCentral,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Eastern")]
        USEastern,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/East-Indiana")]
        USEastIndiana,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Hawaii")]
        USHawaii,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Indiana-Starke")]
        USIndianaStarke,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Michigan")]
        USMichigan,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Mountain")]
        USMountain,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Pacific")]
        USPacific,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Pacific-New")]
        USPacificNew,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("US/Samoa")]
        USSamoa,

        /// <remarks/>
        UTC,

        /// <remarks/>
        WET,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("W-SU")]
        WSU,

        /// <remarks/>
        Zulu,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum GmtOffsetType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+00:00")]
        Item0000,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+01:00")]
        Item0100,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+02:00")]
        Item0200,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+03:00")]
        Item0300,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+03:30")]
        Item0330,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+04:00")]
        Item0400,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+04:30")]
        Item0430,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+05:00")]
        Item0500,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+05:30")]
        Item0530,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+05:45")]
        Item0545,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+06:00")]
        Item0600,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+06:30")]
        Item0630,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+07:00")]
        Item0700,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+08:00")]
        Item0800,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+08:45")]
        Item0845,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+09:00")]
        Item0900,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+09:30")]
        Item0930,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+10:00")]
        Item1000,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+10:30")]
        Item1030,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+11:00")]
        Item1100,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+11:30")]
        Item1130,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+12:00")]
        Item1200,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+12:45")]
        Item1245,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+13:00")]
        Item1300,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("+14:00")]
        Item1400,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-01:00")]
        Item01001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-02:00")]
        Item02001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-03:00")]
        Item03001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-03:30")]
        Item03301,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-04:00")]
        Item04001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-04:30")]
        Item04301,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-05:00")]
        Item05001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-06:00")]
        Item06001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-07:00")]
        Item07001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-08:00")]
        Item08001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-09:00")]
        Item09001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-09:30")]
        Item09301,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-10:00")]
        Item10001,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("-11:00")]
        Item11001,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Country/V2")]
    [System.Xml.Serialization.XmlRootAttribute("country", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Country/V2", IsNullable = false)]
    public partial class CountryType
    {

        private string codeField;

        private string nameField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TerritoryType
    {

        private string codeField;

        private string nameField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class CityType
    {

        private string codeField;

        private string nameField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/PhysicalAddress/V1")]
    [System.Xml.Serialization.XmlRootAttribute("physicalAddress", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/PhysicalAddress/V1", IsNullable = false)]
    public partial class PhysicalAddressType
    {

        private string addressLine1Field;

        private string addressLine2Field;

        private string addressLine3Field;

        private CityType cityTypeField;

        private TerritoryType territoryTypeField;

        private string postalCodeField;

        private CountryType countryField;

        private TimezoneType timezoneField;

        private string latitudeField;

        private string longitudeField;

        /// <remarks/>
        public string addressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        public string addressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        public string addressLine3
        {
            get
            {
                return this.addressLine3Field;
            }
            set
            {
                this.addressLine3Field = value;
            }
        }

        /// <remarks/>
        public CityType cityType
        {
            get
            {
                return this.cityTypeField;
            }
            set
            {
                this.cityTypeField = value;
            }
        }

        /// <remarks/>
        public TerritoryType territoryType
        {
            get
            {
                return this.territoryTypeField;
            }
            set
            {
                this.territoryTypeField = value;
            }
        }

        /// <remarks/>
        public string postalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        public CountryType country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public TimezoneType timezone
        {
            get
            {
                return this.timezoneField;
            }
            set
            {
                this.timezoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
        public string latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
        public string longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/AddressType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("addressType", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/AddressType/V2", IsNullable = false)]
    public partial class AddressTypeType
    {

        private string codeField;

        private AddressDescType descriptionField;

        private object itemField;

        private ItemChoiceType itemElementNameField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public AddressDescType description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("email", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("phone", typeof(PhoneType))]
        [System.Xml.Serialization.XmlElementAttribute("physical", typeof(PhysicalAddressType))]
        [System.Xml.Serialization.XmlElementAttribute("url", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum AddressDescType
    {

        /// <remarks/>
        email,

        /// <remarks/>
        physical,

        /// <remarks/>
        phone,

        /// <remarks/>
        url,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/AddressType/V2", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        email,

        /// <remarks/>
        phone,

        /// <remarks/>
        physical,

        /// <remarks/>
        url,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "CityType", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/City/V2")]
    [System.Xml.Serialization.XmlRootAttribute("city", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/City/V2", IsNullable = false)]
    public partial class CityType1
    {

        private CityType typeField;

        private CountyType countyField;

        private TerritoryType1 territoryField;

        /// <remarks/>
        public CityType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public CountyType county
        {
            get
            {
                return this.countyField;
            }
            set
            {
                this.countyField = value;
            }
        }

        /// <remarks/>
        public TerritoryType1 territory
        {
            get
            {
                return this.territoryField;
            }
            set
            {
                this.territoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/County/V2")]
    [System.Xml.Serialization.XmlRootAttribute("county", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/County/V2", IsNullable = false)]
    public partial class CountyType
    {

        private string codeField;

        private string nameField;

        private TerritoryType1 territoryField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public TerritoryType1 territory
        {
            get
            {
                return this.territoryField;
            }
            set
            {
                this.territoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "TerritoryType", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Territory/V2")]
    [System.Xml.Serialization.XmlRootAttribute("territory", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/Territory/V2", IsNullable = false)]
    public partial class TerritoryType1
    {

        private TerritoryType typeField;

        private CountryType countryField;

        /// <remarks/>
        public TerritoryType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public CountryType country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum UsaStateCodeType
    {

        /// <remarks/>
        AL,

        /// <remarks/>
        AK,

        /// <remarks/>
        AZ,

        /// <remarks/>
        AR,

        /// <remarks/>
        CA,

        /// <remarks/>
        CO,

        /// <remarks/>
        CT,

        /// <remarks/>
        DE,

        /// <remarks/>
        DC,

        /// <remarks/>
        FL,

        /// <remarks/>
        GA,

        /// <remarks/>
        HI,

        /// <remarks/>
        ID,

        /// <remarks/>
        IL,

        /// <remarks/>
        IN,

        /// <remarks/>
        IA,

        /// <remarks/>
        KS,

        /// <remarks/>
        KY,

        /// <remarks/>
        LA,

        /// <remarks/>
        ME,

        /// <remarks/>
        MD,

        /// <remarks/>
        MA,

        /// <remarks/>
        MI,

        /// <remarks/>
        MN,

        /// <remarks/>
        MS,

        /// <remarks/>
        MO,

        /// <remarks/>
        MT,

        /// <remarks/>
        NE,

        /// <remarks/>
        NV,

        /// <remarks/>
        NH,

        /// <remarks/>
        NJ,

        /// <remarks/>
        NM,

        /// <remarks/>
        NY,

        /// <remarks/>
        NC,

        /// <remarks/>
        ND,

        /// <remarks/>
        OH,

        /// <remarks/>
        OK,

        /// <remarks/>
        OR,

        /// <remarks/>
        PA,

        /// <remarks/>
        RI,

        /// <remarks/>
        SC,

        /// <remarks/>
        SD,

        /// <remarks/>
        TN,

        /// <remarks/>
        TX,

        /// <remarks/>
        UT,

        /// <remarks/>
        VT,

        /// <remarks/>
        VA,

        /// <remarks/>
        WA,

        /// <remarks/>
        WV,

        /// <remarks/>
        WI,

        /// <remarks/>
        WY,

        /// <remarks/>
        FD,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum UsaStateNameType
    {

        /// <remarks/>
        Alabama,

        /// <remarks/>
        Alaska,

        /// <remarks/>
        Arizona,

        /// <remarks/>
        Arkansas,

        /// <remarks/>
        California,

        /// <remarks/>
        Colorado,

        /// <remarks/>
        Connecticut,

        /// <remarks/>
        Delaware,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("District of Columbia")]
        DistrictofColumbia,

        /// <remarks/>
        Florida,

        /// <remarks/>
        Georgia,

        /// <remarks/>
        Hawaii,

        /// <remarks/>
        Idaho,

        /// <remarks/>
        Illinois,

        /// <remarks/>
        Indiana,

        /// <remarks/>
        Iowa,

        /// <remarks/>
        Kansas,

        /// <remarks/>
        Kentucky,

        /// <remarks/>
        Louisiana,

        /// <remarks/>
        Maine,

        /// <remarks/>
        Maryland,

        /// <remarks/>
        Massachusetts,

        /// <remarks/>
        Michigan,

        /// <remarks/>
        Minnesota,

        /// <remarks/>
        Mississippi,

        /// <remarks/>
        Missouri,

        /// <remarks/>
        Montana,

        /// <remarks/>
        Nebraska,

        /// <remarks/>
        Nevada,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("New Hampshire")]
        NewHampshire,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("New Jersey")]
        NewJersey,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("New Mexico")]
        NewMexico,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("New York")]
        NewYork,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("North Carolina")]
        NorthCarolina,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("North Dakota")]
        NorthDakota,

        /// <remarks/>
        Ohio,

        /// <remarks/>
        Oklahoma,

        /// <remarks/>
        Oregon,

        /// <remarks/>
        Pennsylvania,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Rhode Island")]
        RhodeIsland,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("South Carolina")]
        SouthCarolina,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("South Dakota")]
        SouthDakota,

        /// <remarks/>
        Tennessee,

        /// <remarks/>
        Texas,

        /// <remarks/>
        Utah,

        /// <remarks/>
        Vermont,

        /// <remarks/>
        Virginia,

        /// <remarks/>
        Washington,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("West Virginia")]
        WestVirginia,

        /// <remarks/>
        Wisconsin,

        /// <remarks/>
        Wyoming,

        /// <remarks/>
        Federal,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class EmailType
    {

        private string addressField;

        /// <remarks/>
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/PostalCode/V2")]
    [System.Xml.Serialization.XmlRootAttribute("postalCode", Namespace = "http://schemas.wfm.com/Enterprise/AddressMgmt/PostalCode/V2", IsNullable = false)]
    public partial class PostalCodeType
    {

        private string codeField;

        private CountyType countyField;

        private CountryType countryField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public CountyType county
        {
            get
            {
                return this.countyField;
            }
            set
            {
                this.countyField = value;
            }
        }

        /// <remarks/>
        public CountryType country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AffinityMgmt/Affinity/V1")]
    [System.Xml.Serialization.XmlRootAttribute("affinity", Namespace = "http://schemas.wfm.com/Enterprise/AffinityMgmt/Affinity/V1", IsNullable = false)]
    public partial class AffinityType
    {

        private string idField;

        private string typeCodeField;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AffinityMgmt/AffinityType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("affinityType", Namespace = "http://schemas.wfm.com/Enterprise/AffinityMgmt/AffinityType/V1", IsNullable = false)]
    public partial class AffinityTypeType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/TransactionAffinity/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionAffinity", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/TransactionAffinity/V1", IsNullable = false)]
    public partial class TransactionAffinityType
    {

        private AffinityType affinityField;

        /// <remarks/>
        public AffinityType affinity
        {
            get
            {
                return this.affinityField;
            }
            set
            {
                this.affinityField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MonetaryAmountCommonData))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ApportionmentAmountType2))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CouponReducesTaxationAmount))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ApportionmentAmountType1))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ApportionmentAmountType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PriceAmount))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AmountRoundedType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AmountMgmt/Amount/V1")]
    [System.Xml.Serialization.XmlRootAttribute("amount", Namespace = "http://schemas.wfm.com/Enterprise/AmountMgmt/Amount/V1", IsNullable = false)]
    public partial class AmountType
    {

        private decimal amountField;

        private bool amountFieldSpecified;

        private CurrencyTypeCodeEnum currencyField;

        private bool currencyFieldSpecified;

        private decimal foreignAmountField;

        private bool foreignAmountFieldSpecified;

        private string denominationField;

        private string typeCodeField;

        /// <remarks/>
        public decimal amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool amountSpecified
        {
            get
            {
                return this.amountFieldSpecified;
            }
            set
            {
                this.amountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public CurrencyTypeCodeEnum currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool currencySpecified
        {
            get
            {
                return this.currencyFieldSpecified;
            }
            set
            {
                this.currencyFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal foreignAmount
        {
            get
            {
                return this.foreignAmountField;
            }
            set
            {
                this.foreignAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool foreignAmountSpecified
        {
            get
            {
                return this.foreignAmountFieldSpecified;
            }
            set
            {
                this.foreignAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string denomination
        {
            get
            {
                return this.denominationField;
            }
            set
            {
                this.denominationField = value;
            }
        }

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum CurrencyTypeCodeEnum
    {

        /// <remarks/>
        CAD,

        /// <remarks/>
        EUR,

        /// <remarks/>
        GBP,

        /// <remarks/>
        USD,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class MonetaryAmountCommonData : AmountType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ApportionmentAmountType", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ApportionmentAmountType", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ApportionmentAmountType2 : AmountType
    {

        private string participatorIdField;

        private string participatorField;

        /// <remarks/>
        public string participatorId
        {
            get
            {
                return this.participatorIdField;
            }
            set
            {
                this.participatorIdField = value;
            }
        }

        /// <remarks/>
        public string participator
        {
            get
            {
                return this.participatorField;
            }
            set
            {
                this.participatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class CouponReducesTaxationAmount : AmountType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ApportionmentAmountType", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute("ApportionmentAmountType", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class ApportionmentAmountType1 : AmountType
    {

        private string participatorField;

        private string participatorIdField;

        /// <remarks/>
        public string participator
        {
            get
            {
                return this.participatorField;
            }
            set
            {
                this.participatorField = value;
            }
        }

        /// <remarks/>
        public string participatorId
        {
            get
            {
                return this.participatorIdField;
            }
            set
            {
                this.participatorIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class ApportionmentAmountType : AmountType
    {

        private string participatorField;

        private string participatorIdField;

        /// <remarks/>
        public string participator
        {
            get
            {
                return this.participatorField;
            }
            set
            {
                this.participatorField = value;
            }
        }

        /// <remarks/>
        public string participatorId
        {
            get
            {
                return this.participatorIdField;
            }
            set
            {
                this.participatorIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class PriceAmount : AmountType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/AmountMgmt/Amount/V1")]
    [System.Xml.Serialization.XmlRootAttribute("amountRounded", Namespace = "http://schemas.wfm.com/Enterprise/AmountMgmt/Amount/V1", IsNullable = false)]
    public partial class AmountRoundedType : AmountType
    {

        private RoundingActionEnum roundingDirectionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public RoundingActionEnum RoundingDirection
        {
            get
            {
                return this.roundingDirectionField;
            }
            set
            {
                this.roundingDirectionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum RoundingActionEnum
    {

        /// <remarks/>
        Up,

        /// <remarks/>
        Down,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum ActionEnum
    {

        /// <remarks/>
        Add,

        /// <remarks/>
        AddOrUpdate,

        /// <remarks/>
        Archive,

        /// <remarks/>
        Delete,

        /// <remarks/>
        Inactivate,

        /// <remarks/>
        Inherit,

        /// <remarks/>
        Insert,

        /// <remarks/>
        Reset,

        /// <remarks/>
        Update,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum LanguageCodeEnum
    {

        /// <remarks/>
        eng,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum CultureTypeCodeEnum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("en-CA")]
        enCA,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("en-GB")]
        enGB,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("en-US")]
        enUS,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum DayOfWeekNameEnum
    {

        /// <remarks/>
        MONDAY,

        /// <remarks/>
        TUESDAY,

        /// <remarks/>
        WEDNESDAY,

        /// <remarks/>
        THURSDAY,

        /// <remarks/>
        FRIDAY,

        /// <remarks/>
        SATURDAY,

        /// <remarks/>
        SUNDAY,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum DayOfWeekAbbrEnum
    {

        /// <remarks/>
        MON,

        /// <remarks/>
        TU,

        /// <remarks/>
        TUE,

        /// <remarks/>
        TUES,

        /// <remarks/>
        WED,

        /// <remarks/>
        TH,

        /// <remarks/>
        THU,

        /// <remarks/>
        THUR,

        /// <remarks/>
        THURS,

        /// <remarks/>
        FRI,

        /// <remarks/>
        SAT,

        /// <remarks/>
        SUN,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum MonthOfYearNameFullEnum
    {

        /// <remarks/>
        JANUARY,

        /// <remarks/>
        FEBRUARY,

        /// <remarks/>
        MARCH,

        /// <remarks/>
        APRIL,

        /// <remarks/>
        MAY,

        /// <remarks/>
        JUNE,

        /// <remarks/>
        JULY,

        /// <remarks/>
        AUGUST,

        /// <remarks/>
        SEPTEMBER,

        /// <remarks/>
        OCTOBER,

        /// <remarks/>
        NOVEMBER,

        /// <remarks/>
        DECEMBER,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum MonthOfYearAbbrEnum
    {

        /// <remarks/>
        JAN,

        /// <remarks/>
        FEB,

        /// <remarks/>
        MAR,

        /// <remarks/>
        APR,

        /// <remarks/>
        MAY,

        /// <remarks/>
        JUN,

        /// <remarks/>
        JUL,

        /// <remarks/>
        AUG,

        /// <remarks/>
        SEP,

        /// <remarks/>
        OCT,

        /// <remarks/>
        NOV,

        /// <remarks/>
        DEC,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TransactionTypeEnum
    {

        /// <remarks/>
        Retail,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Eat-In")]
        EatIn,

        /// <remarks/>
        Catering,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class DayofWeekType
    {

        private DayOfWeekNameEnum nameField;

        private bool nameFieldSpecified;

        private DayOfWeekAbbrEnum abbreviationField;

        private bool abbreviationFieldSpecified;

        /// <remarks/>
        public DayOfWeekNameEnum name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool nameSpecified
        {
            get
            {
                return this.nameFieldSpecified;
            }
            set
            {
                this.nameFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DayOfWeekAbbrEnum abbreviation
        {
            get
            {
                return this.abbreviationField;
            }
            set
            {
                this.abbreviationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool abbreviationSpecified
        {
            get
            {
                return this.abbreviationFieldSpecified;
            }
            set
            {
                this.abbreviationFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class DayType
    {

        private int calendarNumberField;

        private bool calendarNumberFieldSpecified;

        private DayofWeekType dayOfWeekField;

        /// <remarks/>
        public int calendarNumber
        {
            get
            {
                return this.calendarNumberField;
            }
            set
            {
                this.calendarNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool calendarNumberSpecified
        {
            get
            {
                return this.calendarNumberFieldSpecified;
            }
            set
            {
                this.calendarNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DayofWeekType dayOfWeek
        {
            get
            {
                return this.dayOfWeekField;
            }
            set
            {
                this.dayOfWeekField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class MonthOfYearType
    {

        private MonthOfYearNameFullEnum nameField;

        private bool nameFieldSpecified;

        private MonthOfYearAbbrEnum abbreviationField;

        private bool abbreviationFieldSpecified;

        /// <remarks/>
        public MonthOfYearNameFullEnum name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool nameSpecified
        {
            get
            {
                return this.nameFieldSpecified;
            }
            set
            {
                this.nameFieldSpecified = value;
            }
        }

        /// <remarks/>
        public MonthOfYearAbbrEnum abbreviation
        {
            get
            {
                return this.abbreviationField;
            }
            set
            {
                this.abbreviationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool abbreviationSpecified
        {
            get
            {
                return this.abbreviationFieldSpecified;
            }
            set
            {
                this.abbreviationFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class MonthType
    {

        private int calendarNumberField;

        private bool calendarNumberFieldSpecified;

        private MonthOfYearType monthOfYearField;

        /// <remarks/>
        public int calendarNumber
        {
            get
            {
                return this.calendarNumberField;
            }
            set
            {
                this.calendarNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool calendarNumberSpecified
        {
            get
            {
                return this.calendarNumberFieldSpecified;
            }
            set
            {
                this.calendarNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public MonthOfYearType monthOfYear
        {
            get
            {
                return this.monthOfYearField;
            }
            set
            {
                this.monthOfYearField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class DateSeparatedType
    {

        private string yearField;

        private DayType dayField;

        private MonthType monthField;

        /// <remarks/>
        public string year
        {
            get
            {
                return this.yearField;
            }
            set
            {
                this.yearField = value;
            }
        }

        /// <remarks/>
        public DayType day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        public MonthType month
        {
            get
            {
                return this.monthField;
            }
            set
            {
                this.monthField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ReasonCodeType : NameTypeType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReasonCodeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PointOfSaleRegisterTypeType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/NameType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("nameType", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/NameType/V2", IsNullable = false)]
    public partial class NameTypeType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class PointOfSaleRegisterTypeType : NameTypeType
    {

        private PointOfSaleRegisterType[] registerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("register")]
        public PointOfSaleRegisterType[] register
        {
            get
            {
                return this.registerField;
            }
            set
            {
                this.registerField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class PointOfSaleRegisterType
    {

        private string idField;

        private string descriptionField;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class EncryptedDataType
    {

        private string cryptogramField;

        private byte[] valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Cryptogram
        {
            get
            {
                return this.cryptogramField;
            }
            set
            {
                this.cryptogramField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "base64Binary")]
        public byte[] Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/GroupType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("groupType", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/GroupType/V2", IsNullable = false)]
    public partial class GroupTypeType
    {

        private string idField;

        private string descriptionField;

        private string nameField;

        private string typeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/LinkType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("linkType", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/LinkType/V1", IsNullable = false)]
    public partial class LinkTypeType
    {

        private int parentIdField;

        private bool parentIdFieldSpecified;

        private int childIdField;

        private bool childIdFieldSpecified;

        /// <remarks/>
        public int parentId
        {
            get
            {
                return this.parentIdField;
            }
            set
            {
                this.parentIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool parentIdSpecified
        {
            get
            {
                return this.parentIdFieldSpecified;
            }
            set
            {
                this.parentIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int childId
        {
            get
            {
                return this.childIdField;
            }
            set
            {
                this.childIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool childIdSpecified
        {
            get
            {
                return this.childIdFieldSpecified;
            }
            set
            {
                this.childIdFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/Demographic/V1")]
    [System.Xml.Serialization.XmlRootAttribute("demographic", Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/Demographic/V1", IsNullable = false)]
    public partial class DemographicType
    {

        private string idField;

        private string codeField;

        private DemographicGroupType groupField;

        private DemographicTypeType typeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public DemographicGroupType group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        public DemographicTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/DemographicGroup/V1")]
    [System.Xml.Serialization.XmlRootAttribute("DemographicGroup", Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/DemographicGroup/V1", IsNullable = false)]
    public partial class DemographicGroupType
    {

        private int codeField;

        private string descriptionField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public int code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/DemographicType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("demographicType", Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/DemographicType/V1", IsNullable = false)]
    public partial class DemographicTypeType
    {

        private string descriptionField;

        private DemographicValueType[] valueField;

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public DemographicValueType[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/DemographicValue/V1")]
    [System.Xml.Serialization.XmlRootAttribute("demographicValue", Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/DemographicValue/V1", IsNullable = false)]
    public partial class DemographicValueType
    {

        private string valueField;

        private UomType uomField;

        /// <remarks/>
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public UomType uom
        {
            get
            {
                return this.uomField;
            }
            set
            {
                this.uomField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2")]
    [System.Xml.Serialization.XmlRootAttribute("uom", Namespace = "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/UnitOfMeasure/V2", IsNullable = false)]
    public partial class UomType
    {

        private WfmUomCodeEnumType codeField;

        private bool codeFieldSpecified;

        private WfmUomDescEnumType nameField;

        private bool nameFieldSpecified;

        /// <remarks/>
        public WfmUomCodeEnumType code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool codeSpecified
        {
            get
            {
                return this.codeFieldSpecified;
            }
            set
            {
                this.codeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public WfmUomDescEnumType name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool nameSpecified
        {
            get
            {
                return this.nameFieldSpecified;
            }
            set
            {
                this.nameFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/CommonRefType/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/CommonRefType/V2", IsNullable = false)]
    public enum WfmUomCodeEnumType
    {

        /// <remarks/>
        BG,

        /// <remarks/>
        BR,

        /// <remarks/>
        BK,

        /// <remarks/>
        BT,

        /// <remarks/>
        BOX,

        /// <remarks/>
        BX,

        /// <remarks/>
        BKT,

        /// <remarks/>
        BC,

        /// <remarks/>
        BCKG,

        /// <remarks/>
        BCLB,

        /// <remarks/>
        CN,

        /// <remarks/>
        CPLT,

        /// <remarks/>
        CP,

        /// <remarks/>
        CPSL,

        /// <remarks/>
        CS,

        /// <remarks/>
        CPK,

        /// <remarks/>
        CG,

        /// <remarks/>
        CL,

        /// <remarks/>
        CM,

        /// <remarks/>
        CHW,

        /// <remarks/>
        CT,

        /// <remarks/>
        CC,

        /// <remarks/>
        CF,

        /// <remarks/>
        DAY,

        /// <remarks/>
        DG,

        /// <remarks/>
        DL,

        /// <remarks/>
        DM,

        /// <remarks/>
        DZ,

        /// <remarks/>
        DR,

        /// <remarks/>
        EA,

        /// <remarks/>
        EXP,

        /// <remarks/>
        FT,

        /// <remarks/>
        FW,

        /// <remarks/>
        FZ,

        /// <remarks/>
        GL,

        /// <remarks/>
        GC,

        /// <remarks/>
        GR,

        /// <remarks/>
        IN,

        /// <remarks/>
        JAR,

        /// <remarks/>
        JUG,

        /// <remarks/>
        KG,

        /// <remarks/>
        KT,

        /// <remarks/>
        LT,

        /// <remarks/>
        MT,

        /// <remarks/>
        MG,

        /// <remarks/>
        ML,

        /// <remarks/>
        MM,

        /// <remarks/>
        OZ,

        /// <remarks/>
        PK,

        /// <remarks/>
        PAIL,

        /// <remarks/>
        PR,

        /// <remarks/>
        PLT,

        /// <remarks/>
        PC,

        /// <remarks/>
        PCK,

        /// <remarks/>
        PT,

        /// <remarks/>
        LB,

        /// <remarks/>
        QT,

        /// <remarks/>
        DQ,

        /// <remarks/>
        RW,

        /// <remarks/>
        SET,

        /// <remarks/>
        SH,

        /// <remarks/>
        SP,

        /// <remarks/>
        SZ,

        /// <remarks/>
        SL,

        /// <remarks/>
        SG,

        /// <remarks/>
        SF,

        /// <remarks/>
        SM,

        /// <remarks/>
        TB,

        /// <remarks/>
        TBSP,

        /// <remarks/>
        TSP,

        /// <remarks/>
        UNIT,

        /// <remarks/>
        VC,

        /// <remarks/>
        VCAP,

        /// <remarks/>
        WFR,

        /// <remarks/>
        WT,

        /// <remarks/>
        WHEEL,

        /// <remarks/>
        YD,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/CommonRefType/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/UnitOfMeasureMgmt/CommonRefType/V2", IsNullable = false)]
    public enum WfmUomDescEnumType
    {

        /// <remarks/>
        BAGS,

        /// <remarks/>
        BAG,

        /// <remarks/>
        BAR,

        /// <remarks/>
        BOOK,

        /// <remarks/>
        BOTTLE,

        /// <remarks/>
        BOX,

        /// <remarks/>
        BUCKET,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BY COUNT")]
        BYCOUNT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BY COUNT KILOGRAMS")]
        BYCOUNTKILOGRAMS,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("BY COUNT POUNDS")]
        BYCOUNTPOUNDS,

        /// <remarks/>
        CAN,

        /// <remarks/>
        CAPLETS,

        /// <remarks/>
        CAPS,

        /// <remarks/>
        CAPSULE,

        /// <remarks/>
        CASE,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CASE PACK")]
        CASEPACK,

        /// <remarks/>
        CENTIGRAMS,

        /// <remarks/>
        CENTILITERS,

        /// <remarks/>
        CENTIMETERS,

        /// <remarks/>
        CHEWABLE,

        /// <remarks/>
        COUNT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CUBIC CENTIMETER")]
        CUBICCENTIMETER,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("CUBIC FEET")]
        CUBICFEET,

        /// <remarks/>
        DAY,

        /// <remarks/>
        DECIGRAMS,

        /// <remarks/>
        DECILITERS,

        /// <remarks/>
        DECIMETERS,

        /// <remarks/>
        DOZEN,

        /// <remarks/>
        DOZENS,

        /// <remarks/>
        DRAM,

        /// <remarks/>
        EACH,

        /// <remarks/>
        EXPOSURES,

        /// <remarks/>
        FEET,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FIXED WEIGHT")]
        FIXEDWEIGHT,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("FLUID OUNCES")]
        FLUIDOUNCES,

        /// <remarks/>
        GALLON,

        /// <remarks/>
        GALLONS,

        /// <remarks/>
        GELCAP,

        /// <remarks/>
        GRAMS,

        /// <remarks/>
        INCHES,

        /// <remarks/>
        JAR,

        /// <remarks/>
        JUG,

        /// <remarks/>
        KILOGRAM,

        /// <remarks/>
        KILOGRAMS,

        /// <remarks/>
        KIT,

        /// <remarks/>
        POUND,

        /// <remarks/>
        LITERS,

        /// <remarks/>
        METERS,

        /// <remarks/>
        MILLIGRAM,

        /// <remarks/>
        MILLIGRAMS,

        /// <remarks/>
        MILLILITERS,

        /// <remarks/>
        MILLIMETERS,

        /// <remarks/>
        OUNCE,

        /// <remarks/>
        OUNCES,

        /// <remarks/>
        PACK,

        /// <remarks/>
        PAIL,

        /// <remarks/>
        PAIR,

        /// <remarks/>
        PALLET,

        /// <remarks/>
        PIECE,

        /// <remarks/>
        PECKS,

        /// <remarks/>
        PINT,

        /// <remarks/>
        PINTS,

        /// <remarks/>
        QUART,

        /// <remarks/>
        QUARTS,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("QUART DRY")]
        QUARTDRY,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("RANDOM WEIGHT")]
        RANDOMWEIGHT,

        /// <remarks/>
        SET,

        /// <remarks/>
        SHEETS,

        /// <remarks/>
        SHIPPER,

        /// <remarks/>
        SIZE,

        /// <remarks/>
        SLICE,

        /// <remarks/>
        SOFTGEL,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SQUARE FEET")]
        SQUAREFEET,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("SQUARE METERS")]
        SQUAREMETERS,

        /// <remarks/>
        TABS,

        /// <remarks/>
        TABLESPOON,

        /// <remarks/>
        TABLET,

        /// <remarks/>
        TEASPOON,

        /// <remarks/>
        UNIT,

        /// <remarks/>
        VEGICAP,

        /// <remarks/>
        WAFERS,

        /// <remarks/>
        WATTS,

        /// <remarks/>
        WHEEL,

        /// <remarks/>
        YARDS,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/Demographic/V1")]
    [System.Xml.Serialization.XmlRootAttribute("demographics", Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/Demographic/V1", IsNullable = false)]
    public partial class DemographicsType
    {

        private DemographicType[] demographicField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("demographic")]
        public DemographicType[] demographic
        {
            get
            {
                return this.demographicField;
            }
            set
            {
                this.demographicField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/FinancialTransactionAccount/V1")]
    [System.Xml.Serialization.XmlRootAttribute("financialTransactionAccount", Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/FinancialTransactionAccount/V1", IsNullable = false)]
    public partial class FinancialTransactionAccountType
    {

        private FinancialSubteamType[] financialSubteamsField;

        private GainshareType[] gainsharesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("subteam", Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Subteam/V1", IsNullable = false)]
        public FinancialSubteamType[] financialSubteams
        {
            get
            {
                return this.financialSubteamsField;
            }
            set
            {
                this.financialSubteamsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("gainshare", Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Gainshare/V1", IsNullable = false)]
        public GainshareType[] gainshares
        {
            get
            {
                return this.gainsharesField;
            }
            set
            {
                this.gainsharesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Subteam/V1")]
    [System.Xml.Serialization.XmlRootAttribute("financialSubteam", Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Subteam/V1", IsNullable = false)]
    public partial class FinancialSubteamType
    {

        private int idField;

        private string nameField;

        private string descriptionField;

        private FinancialTeamType teamField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public FinancialTeamType team
        {
            get
            {
                return this.teamField;
            }
            set
            {
                this.teamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/Subteam/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/Subteam/CommonRefTypes/V1", IsNullable = true)]
    public partial class FinancialTeamType
    {

        private int idField;

        private string nameField;

        private string descriptionField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Gainshare/V1")]
    [System.Xml.Serialization.XmlRootAttribute("gainshare", Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Gainshare/V1", IsNullable = false)]
    public partial class GainshareType
    {

        private bool isComboTeamField;

        private decimal gainshareTeamNumberField;

        private int gainshareTeamIdField;

        private int gainshareSubteamIdField;

        private string teamRenamedField;

        private FinancialSubteamType financialSubteamField;

        private GainshareLaborType laborField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        public GainshareType()
        {
            this.isComboTeamField = false;
        }

        /// <remarks/>
        public bool isComboTeam
        {
            get
            {
                return this.isComboTeamField;
            }
            set
            {
                this.isComboTeamField = value;
            }
        }

        /// <remarks/>
        public decimal gainshareTeamNumber
        {
            get
            {
                return this.gainshareTeamNumberField;
            }
            set
            {
                this.gainshareTeamNumberField = value;
            }
        }

        /// <remarks/>
        public int gainshareTeamId
        {
            get
            {
                return this.gainshareTeamIdField;
            }
            set
            {
                this.gainshareTeamIdField = value;
            }
        }

        /// <remarks/>
        public int gainshareSubteamId
        {
            get
            {
                return this.gainshareSubteamIdField;
            }
            set
            {
                this.gainshareSubteamIdField = value;
            }
        }

        /// <remarks/>
        public string teamRenamed
        {
            get
            {
                return this.teamRenamedField;
            }
            set
            {
                this.teamRenamedField = value;
            }
        }

        /// <remarks/>
        public FinancialSubteamType financialSubteam
        {
            get
            {
                return this.financialSubteamField;
            }
            set
            {
                this.financialSubteamField = value;
            }
        }

        /// <remarks/>
        public GainshareLaborType labor
        {
            get
            {
                return this.laborField;
            }
            set
            {
                this.laborField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/GainshareMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/GainshareMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class GainshareLaborType
    {

        private int accountIdField;

        private GainsharePercentType[] percentsField;

        /// <remarks/>
        public int accountId
        {
            get
            {
                return this.accountIdField;
            }
            set
            {
                this.accountIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("percent", IsNullable = false)]
        public GainsharePercentType[] percents
        {
            get
            {
                return this.percentsField;
            }
            set
            {
                this.percentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/GainshareMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/GainshareMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class GainsharePercentType
    {

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        private string fiscalYearField;

        private int fiscalPeriodField;

        private decimal valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public string fiscalYear
        {
            get
            {
                return this.fiscalYearField;
            }
            set
            {
                this.fiscalYearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public int fiscalPeriod
        {
            get
            {
                return this.fiscalPeriodField;
            }
            set
            {
                this.fiscalPeriodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Gainshare/V1")]
    [System.Xml.Serialization.XmlRootAttribute("gainshares", Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Gainshare/V1", IsNullable = false)]
    public partial class GainsharesType
    {

        private GainshareType[] gainshareField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("gainshare")]
        public GainshareType[] gainshare
        {
            get
            {
                return this.gainshareField;
            }
            set
            {
                this.gainshareField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Subteam/V1")]
    [System.Xml.Serialization.XmlRootAttribute("financialSubteams", Namespace = "http://schemas.wfm.com/Enterprise/FinancialMgmt/Subteam/V1", IsNullable = false)]
    public partial class FinancialSubteamsType
    {

        private FinancialSubteamType[] subteamField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("subteam")]
        public FinancialSubteamType[] subteam
        {
            get
            {
                return this.subteamField;
            }
            set
            {
                this.subteamField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class hierarchyParentClassType
    {

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public int Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/Hierarchy/V2")]
    [System.Xml.Serialization.XmlRootAttribute("hierarchy", Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/Hierarchy/V2", IsNullable = false)]
    public partial class HierarchyType
    {

        private int idField;

        private bool idFieldSpecified;

        private string nameField;

        private HierarchyPrototypeType prototypeField;

        private HierarchyClassType[] classField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idSpecified
        {
            get
            {
                return this.idFieldSpecified;
            }
            set
            {
                this.idFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public HierarchyPrototypeType prototype
        {
            get
            {
                return this.prototypeField;
            }
            set
            {
                this.prototypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("class")]
        public HierarchyClassType[] @class
        {
            get
            {
                return this.classField;
            }
            set
            {
                this.classField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/HierarchyPrototype/V1")]
    [System.Xml.Serialization.XmlRootAttribute("hierarchyPrototype", Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/HierarchyPrototype/V1", IsNullable = false)]
    public partial class HierarchyPrototypeType
    {

        private string hierarchyLevelNameField;

        private string itemsAttachedField;

        /// <remarks/>
        public string hierarchyLevelName
        {
            get
            {
                return this.hierarchyLevelNameField;
            }
            set
            {
                this.hierarchyLevelNameField = value;
            }
        }

        /// <remarks/>
        public string itemsAttached
        {
            get
            {
                return this.itemsAttachedField;
            }
            set
            {
                this.itemsAttachedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/HierarchyClass/V2")]
    [System.Xml.Serialization.XmlRootAttribute("hierarchyClass", Namespace = "http://schemas.wfm.com/Enterprise/HierarchyMgmt/HierarchyClass/V2", IsNullable = false)]
    public partial class HierarchyClassType
    {

        private string idField;

        private string nameField;

        private int levelField;

        private bool levelFieldSpecified;

        private hierarchyParentClassType parentIdField;

        private TraitType[] traitsField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public int level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool levelSpecified
        {
            get
            {
                return this.levelFieldSpecified;
            }
            set
            {
                this.levelFieldSpecified = value;
            }
        }

        /// <remarks/>
        public hierarchyParentClassType parentId
        {
            get
            {
                return this.parentIdField;
            }
            set
            {
                this.parentIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/Trait/V2")]
    [System.Xml.Serialization.XmlRootAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/Trait/V2", IsNullable = false)]
    public partial class TraitType
    {

        private string codeField;

        private string patternField;

        private TraitGroupType groupField;

        private TraitTypeType typeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string pattern
        {
            get
            {
                return this.patternField;
            }
            set
            {
                this.patternField = value;
            }
        }

        /// <remarks/>
        public TraitGroupType group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        public TraitTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitGroup/V2")]
    [System.Xml.Serialization.XmlRootAttribute("traitGroup", Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitGroup/V2", IsNullable = false)]
    public partial class TraitGroupType
    {

        private int codeField;

        private string descriptionField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public int code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("traitType", Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitType/V2", IsNullable = false)]
    public partial class TraitTypeType
    {

        private string descriptionField;

        private TraitValueType[] valueField;

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public TraitValueType[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitValue/V2")]
    [System.Xml.Serialization.XmlRootAttribute("traitValue", Namespace = "http://schemas.wfm.com/Enterprise/TraitMgmt/TraitValue/V2", IsNullable = false)]
    public partial class TraitValueType
    {

        private string valueField;

        private UomType uomField;

        /// <remarks/>
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public UomType uom
        {
            get
            {
                return this.uomField;
            }
            set
            {
                this.uomField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualBase/V1")]
    [System.Xml.Serialization.XmlRootAttribute("individualBase", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualBase/V1", IsNullable = false)]
    public partial class IndividualBaseType
    {

        private IndividualNameTypeType[] namesField;

        private GenderType genderField;

        private EthnicityType ethnicityField;

        private AgeType ageField;

        private DateSeparatedType dateOfBirthField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("name", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1", IsNullable = false)]
        public IndividualNameTypeType[] names
        {
            get
            {
                return this.namesField;
            }
            set
            {
                this.namesField = value;
            }
        }

        /// <remarks/>
        public GenderType gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        public EthnicityType ethnicity
        {
            get
            {
                return this.ethnicityField;
            }
            set
            {
                this.ethnicityField = value;
            }
        }

        /// <remarks/>
        public AgeType age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }

        /// <remarks/>
        public DateSeparatedType dateOfBirth
        {
            get
            {
                return this.dateOfBirthField;
            }
            set
            {
                this.dateOfBirthField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualNameType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("individualNameType", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualNameType/V1", IsNullable = false)]
    public partial class IndividualNameTypeType
    {

        private NameTypeType typeField;

        private IndividualNameType nameField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public NameTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public IndividualNameType name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualName/V2")]
    [System.Xml.Serialization.XmlRootAttribute("individualName", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualName/V2", IsNullable = false)]
    public partial class IndividualNameType
    {

        private string givenNameField;

        private string middleNameField;

        private string familyNameField;

        private string fullNameField;

        private string titleField;

        private string suffixField;

        /// <remarks/>
        public string givenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }

        /// <remarks/>
        public string middleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string familyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
            }
        }

        /// <remarks/>
        public string fullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
            }
        }

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string suffix
        {
            get
            {
                return this.suffixField;
            }
            set
            {
                this.suffixField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class GenderType
    {

        private GenderCodeType codeField;

        private bool codeFieldSpecified;

        private GenderDescType descriptionField;

        /// <remarks/>
        public GenderCodeType code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool codeSpecified
        {
            get
            {
                return this.codeFieldSpecified;
            }
            set
            {
                this.codeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public GenderDescType description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum GenderCodeType
    {

        /// <remarks/>
        f,

        /// <remarks/>
        m,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum GenderDescType
    {

        /// <remarks/>
        female,

        /// <remarks/>
        male,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class EthnicityType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class AgeType
    {

        private int valueField;

        private UomType uomField;

        /// <remarks/>
        public int value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public UomType uom
        {
            get
            {
                return this.uomField;
            }
            set
            {
                this.uomField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/Individual/V2")]
    [System.Xml.Serialization.XmlRootAttribute("individual", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/Individual/V2", IsNullable = false)]
    public partial class IndividualType
    {

        private IndividualBaseType baseAttributesField;

        private AddressType[] addressesField;

        private TraitType[] traitsField;

        private PersonaType[] personasField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public IndividualBaseType baseAttributes
        {
            get
            {
                return this.baseAttributesField;
            }
            set
            {
                this.baseAttributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("address", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualAddress/V1", IsNullable = false)]
        public AddressType[] addresses
        {
            get
            {
                return this.addressesField;
            }
            set
            {
                this.addressesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualTrait/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("persona", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/Persona/V2", IsNullable = false)]
        public PersonaType[] personas
        {
            get
            {
                return this.personasField;
            }
            set
            {
                this.personasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/Persona/V2")]
    [System.Xml.Serialization.XmlRootAttribute("persona", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/Persona/V2", IsNullable = false)]
    public partial class PersonaType
    {

        private string idField;

        private PersonaTypeType typeField;

        private PersonaIdentifierType[] personaIdentifiersField;

        private StatusHistoryType[] statusHistoriesField;

        private TraitType[] traitsField;

        private OrganizationType organizationField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public PersonaTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("identifier", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/CommonRefTypes/V1", IsNullable = false)]
        public PersonaIdentifierType[] personaIdentifiers
        {
            get
            {
                return this.personaIdentifiersField;
            }
            set
            {
                this.personaIdentifiersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("statusHistory", typeof(StatusHistoryType), Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/StatusHistory/V1", IsNullable = false)]
        public StatusHistoryType[] statusHistories
        {
            get
            {
                return this.statusHistoriesField;
            }
            set
            {
                this.statusHistoriesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaTrait/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        public OrganizationType organization
        {
            get
            {
                return this.organizationField;
            }
            set
            {
                this.organizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("personaType", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaType/V2", IsNullable = false)]
    public partial class PersonaTypeType
    {

        private string codeField;

        private PersonaDescriptionType descriptionField;

        private bool descriptionFieldSpecified;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public PersonaDescriptionType description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool descriptionSpecified
        {
            get
            {
                return this.descriptionFieldSpecified;
            }
            set
            {
                this.descriptionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum PersonaDescriptionType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Contingent Worker")]
        ContingentWorker,

        /// <remarks/>
        Customer,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Team Member")]
        TeamMember,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaIdentifier/V2")]
    [System.Xml.Serialization.XmlRootAttribute("personaIdentifier", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaIdentifier/V2", IsNullable = false)]
    public partial class PersonaIdentifierType
    {

        private PersonaIdentifierTypeType typeField;

        private string valueField;

        /// <remarks/>
        public PersonaIdentifierTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaIdentifierType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("personaIdentifierType", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaIdentifierType/V2", IsNullable = false)]
    public partial class PersonaIdentifierTypeType
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/StatusHistory/V1")]
    [System.Xml.Serialization.XmlRootAttribute("statusHistory", Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/StatusHistory/V1", IsNullable = false)]
    public partial class StatusHistoryType
    {

        private System.DateTime effectiveDateField;

        private bool effectiveDateFieldSpecified;

        private StatusType statusField;

        /// <remarks/>
        public System.DateTime effectiveDate
        {
            get
            {
                return this.effectiveDateField;
            }
            set
            {
                this.effectiveDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool effectiveDateSpecified
        {
            get
            {
                return this.effectiveDateFieldSpecified;
            }
            set
            {
                this.effectiveDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public StatusType status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/Status/V2")]
    [System.Xml.Serialization.XmlRootAttribute("status", Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/Status/V2", IsNullable = false)]
    public partial class StatusType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/Organization/V2")]
    [System.Xml.Serialization.XmlRootAttribute("organization", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/Organization/V2", IsNullable = false)]
    public partial class OrganizationType
    {

        private string orgPartyIDField;

        private string descriptionField;

        private OrganizationNameTypeType[] nameTypesField;

        private TraitType[] traitsField;

        private OrganizationTypeType[] typeField;

        /// <remarks/>
        public string orgPartyID
        {
            get
            {
                return this.orgPartyIDField;
            }
            set
            {
                this.orgPartyIDField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("nameType", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationNameType/V2", IsNullable = false)]
        public OrganizationNameTypeType[] nameTypes
        {
            get
            {
                return this.nameTypesField;
            }
            set
            {
                this.nameTypesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("type")]
        public OrganizationTypeType[] type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationNameType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("organizationNameType", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationNameType/V2", IsNullable = false)]
    public partial class OrganizationNameTypeType
    {

        private NameTypeType typeField;

        private OrganizationNameType nameField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public NameTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public OrganizationNameType name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationName/V2")]
    [System.Xml.Serialization.XmlRootAttribute("organizationName", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationName/V2", IsNullable = false)]
    public partial class OrganizationNameType
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("organizationType", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationType/V2", IsNullable = false)]
    public partial class OrganizationTypeType
    {

        private string typeCodeField;

        private string typeDescriptionField;

        private ParentOrgPartyType parentOrgPartyIDField;

        private TraitType[] traitsField;

        private LocaleType[] localeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        public string typeDescription
        {
            get
            {
                return this.typeDescriptionField;
            }
            set
            {
                this.typeDescriptionField = value;
            }
        }

        /// <remarks/>
        public ParentOrgPartyType parentOrgPartyID
        {
            get
            {
                return this.parentOrgPartyIDField;
            }
            set
            {
                this.parentOrgPartyIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationTrait/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("locale")]
        public LocaleType[] locale
        {
            get
            {
                return this.localeField;
            }
            set
            {
                this.localeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ParentOrgPartyType
    {

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/Locale/V2")]
    [System.Xml.Serialization.XmlRootAttribute("locale", Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/Locale/V2", IsNullable = false)]
    public partial class LocaleType
    {

        private string idField;

        private int parentIdField;

        private bool parentIdFieldSpecified;

        private string nameField;

        private System.DateTime openDateField;

        private bool openDateFieldSpecified;

        private System.DateTime closeDateField;

        private bool closeDateFieldSpecified;

        private AddressType[] addressesField;

        private TraitType[] traitsField;

        private LocaleTypeType typeField;

        private FinancialTransactionAccountType financialTransactionAccountsField;

        private object itemField;

        private LocaleType[] localesField;

        private StoreType storeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public int parentId
        {
            get
            {
                return this.parentIdField;
            }
            set
            {
                this.parentIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool parentIdSpecified
        {
            get
            {
                return this.parentIdFieldSpecified;
            }
            set
            {
                this.parentIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime openDate
        {
            get
            {
                return this.openDateField;
            }
            set
            {
                this.openDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool openDateSpecified
        {
            get
            {
                return this.openDateFieldSpecified;
            }
            set
            {
                this.openDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime closeDate
        {
            get
            {
                return this.closeDateField;
            }
            set
            {
                this.closeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool closeDateSpecified
        {
            get
            {
                return this.closeDateFieldSpecified;
            }
            set
            {
                this.closeDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("address", Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/CommonRefTypes/V1", IsNullable = false)]
        public AddressType[] addresses
        {
            get
            {
                return this.addressesField;
            }
            set
            {
                this.addressesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        public LocaleTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public FinancialTransactionAccountType financialTransactionAccounts
        {
            get
            {
                return this.financialTransactionAccountsField;
            }
            set
            {
                this.financialTransactionAccountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("enterpriseItemAttributes", typeof(EnterpriseItemAttributesType))]
        [System.Xml.Serialization.XmlElementAttribute("regionalItemAttributes", typeof(RegionalItemAttributesType))]
        [System.Xml.Serialization.XmlElementAttribute("storeItemAttributes", typeof(StoreItemAttributesType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("locale", Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/CommonRefTypes/V1", IsNullable = false)]
        public LocaleType[] locales
        {
            get
            {
                return this.localesField;
            }
            set
            {
                this.localesField = value;
            }
        }

        /// <remarks/>
        public StoreType store
        {
            get
            {
                return this.storeField;
            }
            set
            {
                this.storeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/LocaleType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("localeType", Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/LocaleType/V2", IsNullable = false)]
    public partial class LocaleTypeType
    {

        private LocaleCodeType codeField;

        private LocaleDescType descriptionField;

        /// <remarks/>
        public LocaleCodeType code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public LocaleDescType description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum LocaleCodeType
    {

        /// <remarks/>
        CMP,

        /// <remarks/>
        CHN,

        /// <remarks/>
        REG,

        /// <remarks/>
        STR,

        /// <remarks/>
        OFF,

        /// <remarks/>
        MTR,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum LocaleDescType
    {

        /// <remarks/>
        Company,

        /// <remarks/>
        Chain,

        /// <remarks/>
        Region,

        /// <remarks/>
        Metro,

        /// <remarks/>
        Store,

        /// <remarks/>
        Office,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemAttributesEnterprise/V2")]
    [System.Xml.Serialization.XmlRootAttribute("enterpriseItemAttributes", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemAttributesEnterprise/V2", IsNullable = false)]
    public partial class EnterpriseItemAttributesType
    {

        private ScanCodeType[] scanCodesField;

        private LinkTypeType[] linksField;

        private GroupTypeType[] groupsField;

        private HierarchyType[] hierarchiesField;

        private TraitType[] traitsField;

        private PriceType[] pricesField;

        private SelectionGroupsType selectionGroupsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("scanCode", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ScanCode/V2", IsNullable = false)]
        public ScanCodeType[] scanCodes
        {
            get
            {
                return this.scanCodesField;
            }
            set
            {
                this.scanCodesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("link", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public LinkTypeType[] links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("group", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public GroupTypeType[] groups
        {
            get
            {
                return this.groupsField;
            }
            set
            {
                this.groupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("hierarchy", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public HierarchyType[] hierarchies
        {
            get
            {
                return this.hierarchiesField;
            }
            set
            {
                this.hierarchiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("price", Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2", IsNullable = false)]
        public PriceType[] prices
        {
            get
            {
                return this.pricesField;
            }
            set
            {
                this.pricesField = value;
            }
        }

        /// <remarks/>
        public SelectionGroupsType selectionGroups
        {
            get
            {
                return this.selectionGroupsField;
            }
            set
            {
                this.selectionGroupsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ScanCode/V2")]
    [System.Xml.Serialization.XmlRootAttribute("scanCode", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ScanCode/V2", IsNullable = false)]
    public partial class ScanCodeType
    {

        private int idField;

        private bool idFieldSpecified;

        private string codeField;

        private int typeIdField;

        private bool typeIdFieldSpecified;

        private string typeDescriptionField;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idSpecified
        {
            get
            {
                return this.idFieldSpecified;
            }
            set
            {
                this.idFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public int typeId
        {
            get
            {
                return this.typeIdField;
            }
            set
            {
                this.typeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeIdSpecified
        {
            get
            {
                return this.typeIdFieldSpecified;
            }
            set
            {
                this.typeIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string typeDescription
        {
            get
            {
                return this.typeDescriptionField;
            }
            set
            {
                this.typeDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2")]
    [System.Xml.Serialization.XmlRootAttribute("price", Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2", IsNullable = false)]
    public partial class PriceType
    {

        private string idField;

        private PriceTypeType typeField;

        private UomType uomField;

        private CurrencyTypeCodeEnum currencyTypeCodeField;

        private PriceAmount priceAmountField;

        private int priceMultipleField;

        private int breakPointStartQtyField;

        private bool breakPointStartQtyFieldSpecified;

        private int breakPointEndQtyField;

        private bool breakPointEndQtyFieldSpecified;

        private System.DateTime priceStartDateField;

        private bool priceStartDateFieldSpecified;

        private System.DateTime priceEndDateField;

        private bool priceEndDateFieldSpecified;

        public PriceType()
        {
            this.priceMultipleField = 1;
        }

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public PriceTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public UomType uom
        {
            get
            {
                return this.uomField;
            }
            set
            {
                this.uomField = value;
            }
        }

        /// <remarks/>
        public CurrencyTypeCodeEnum currencyTypeCode
        {
            get
            {
                return this.currencyTypeCodeField;
            }
            set
            {
                this.currencyTypeCodeField = value;
            }
        }

        /// <remarks/>
        public PriceAmount priceAmount
        {
            get
            {
                return this.priceAmountField;
            }
            set
            {
                this.priceAmountField = value;
            }
        }

        /// <remarks/>
        public int priceMultiple
        {
            get
            {
                return this.priceMultipleField;
            }
            set
            {
                this.priceMultipleField = value;
            }
        }

        /// <remarks/>
        public int breakPointStartQty
        {
            get
            {
                return this.breakPointStartQtyField;
            }
            set
            {
                this.breakPointStartQtyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool breakPointStartQtySpecified
        {
            get
            {
                return this.breakPointStartQtyFieldSpecified;
            }
            set
            {
                this.breakPointStartQtyFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int breakPointEndQty
        {
            get
            {
                return this.breakPointEndQtyField;
            }
            set
            {
                this.breakPointEndQtyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool breakPointEndQtySpecified
        {
            get
            {
                return this.breakPointEndQtyFieldSpecified;
            }
            set
            {
                this.breakPointEndQtyFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime priceStartDate
        {
            get
            {
                return this.priceStartDateField;
            }
            set
            {
                this.priceStartDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool priceStartDateSpecified
        {
            get
            {
                return this.priceStartDateFieldSpecified;
            }
            set
            {
                this.priceStartDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public System.DateTime priceEndDate
        {
            get
            {
                return this.priceEndDateField;
            }
            set
            {
                this.priceEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool priceEndDateSpecified
        {
            get
            {
                return this.priceEndDateFieldSpecified;
            }
            set
            {
                this.priceEndDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/PriceType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("priceType", Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/PriceType/V1", IsNullable = false)]
    public partial class PriceTypeType
    {

        private string idField;

        private string descriptionField;

        private PriceTypeType typeField;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public PriceTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/SelectionGrpMgmt/SelectionGrp/V2")]
    [System.Xml.Serialization.XmlRootAttribute("selectionGroups", Namespace = "http://schemas.wfm.com/Enterprise/SelectionGrpMgmt/SelectionGrp/V2", IsNullable = false)]
    public partial class SelectionGroupsType
    {

        private string nameField;

        private string descriptionField;

        private GroupTypeType[] groupField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("group")]
        public GroupTypeType[] group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemAttributesRegional/V2")]
    [System.Xml.Serialization.XmlRootAttribute("regionalItemAttributes", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemAttributesRegional/V2", IsNullable = false)]
    public partial class RegionalItemAttributesType
    {

        private ScanCodeType[] scanCodeField;

        private LinkTypeType[] linksField;

        private GroupTypeType[] groupsField;

        private HierarchyType[] hierarchiesField;

        private TraitType[] traitsField;

        private PriceType[] pricesField;

        private SelectionGroupsType selectionGroupsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("scanCode")]
        public ScanCodeType[] scanCode
        {
            get
            {
                return this.scanCodeField;
            }
            set
            {
                this.scanCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("link", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public LinkTypeType[] links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("group", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public GroupTypeType[] groups
        {
            get
            {
                return this.groupsField;
            }
            set
            {
                this.groupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("hierarchy", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public HierarchyType[] hierarchies
        {
            get
            {
                return this.hierarchiesField;
            }
            set
            {
                this.hierarchiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("price", Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2", IsNullable = false)]
        public PriceType[] prices
        {
            get
            {
                return this.pricesField;
            }
            set
            {
                this.pricesField = value;
            }
        }

        /// <remarks/>
        public SelectionGroupsType selectionGroups
        {
            get
            {
                return this.selectionGroupsField;
            }
            set
            {
                this.selectionGroupsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemAttributesStore/V2")]
    [System.Xml.Serialization.XmlRootAttribute("storeItemAttributes", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemAttributesStore/V2", IsNullable = false)]
    public partial class StoreItemAttributesType
    {

        private ScanCodeType[] scanCodeField;

        private LinkTypeType[] linksField;

        private GroupTypeType[] groupsField;

        private HierarchyType[] hierarchiesField;

        private TraitType[] traitsField;

        private PriceType[] pricesField;

        private SelectionGroupsType selectionGroupsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("scanCode")]
        public ScanCodeType[] scanCode
        {
            get
            {
                return this.scanCodeField;
            }
            set
            {
                this.scanCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("link", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public LinkTypeType[] links
        {
            get
            {
                return this.linksField;
            }
            set
            {
                this.linksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("group", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public GroupTypeType[] groups
        {
            get
            {
                return this.groupsField;
            }
            set
            {
                this.groupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("hierarchy", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public HierarchyType[] hierarchies
        {
            get
            {
                return this.hierarchiesField;
            }
            set
            {
                this.hierarchiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("price", Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2", IsNullable = false)]
        public PriceType[] prices
        {
            get
            {
                return this.pricesField;
            }
            set
            {
                this.pricesField = value;
            }
        }

        /// <remarks/>
        public SelectionGroupsType selectionGroups
        {
            get
            {
                return this.selectionGroupsField;
            }
            set
            {
                this.selectionGroupsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/Store/V2")]
    [System.Xml.Serialization.XmlRootAttribute("store", Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/Store/V2", IsNullable = false)]
    public partial class StoreType
    {

        private string idField;

        private TraitType[] traitsField;

        private StoreGroupType[] groupsField;

        private PointOfSaleRegistersType pointOfSaleRegistersField;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("storeGroup", Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/CommonRefTypes/V1", IsNullable = false)]
        public StoreGroupType[] groups
        {
            get
            {
                return this.groupsField;
            }
            set
            {
                this.groupsField = value;
            }
        }

        /// <remarks/>
        public PointOfSaleRegistersType pointOfSaleRegisters
        {
            get
            {
                return this.pointOfSaleRegistersField;
            }
            set
            {
                this.pointOfSaleRegistersField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/StoreGroup/V1")]
    [System.Xml.Serialization.XmlRootAttribute("storeGroup", Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/StoreGroup/V1", IsNullable = false)]
    public partial class StoreGroupType
    {

        private int storeGroupIdField;

        private bool storeGroupIdFieldSpecified;

        private string storeGroupNameField;

        private string storeGroupDescriptionField;

        private StoreGroupType1 storeGroupTypeField;

        /// <remarks/>
        public int storeGroupId
        {
            get
            {
                return this.storeGroupIdField;
            }
            set
            {
                this.storeGroupIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool storeGroupIdSpecified
        {
            get
            {
                return this.storeGroupIdFieldSpecified;
            }
            set
            {
                this.storeGroupIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string storeGroupName
        {
            get
            {
                return this.storeGroupNameField;
            }
            set
            {
                this.storeGroupNameField = value;
            }
        }

        /// <remarks/>
        public string storeGroupDescription
        {
            get
            {
                return this.storeGroupDescriptionField;
            }
            set
            {
                this.storeGroupDescriptionField = value;
            }
        }

        /// <remarks/>
        public StoreGroupType1 storeGroupType
        {
            get
            {
                return this.storeGroupTypeField;
            }
            set
            {
                this.storeGroupTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StoreGroupType", Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/StoreGroupType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("storeGroupType", Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/StoreGroupType/V1", IsNullable = false)]
    public partial class StoreGroupType1
    {

        private int storeGroupTypeIdField;

        private bool storeGroupTypeIdFieldSpecified;

        private string storeGroupTypeDescriptionField;

        /// <remarks/>
        public int storeGroupTypeId
        {
            get
            {
                return this.storeGroupTypeIdField;
            }
            set
            {
                this.storeGroupTypeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool storeGroupTypeIdSpecified
        {
            get
            {
                return this.storeGroupTypeIdFieldSpecified;
            }
            set
            {
                this.storeGroupTypeIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string storeGroupTypeDescription
        {
            get
            {
                return this.storeGroupTypeDescriptionField;
            }
            set
            {
                this.storeGroupTypeDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StoreMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class PointOfSaleRegistersType
    {

        private PointOfSaleRegisterTypeType[] typesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("type", IsNullable = false)]
        public PointOfSaleRegisterTypeType[] types
        {
            get
            {
                return this.typesField;
            }
            set
            {
                this.typesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualAddress/V1")]
    [System.Xml.Serialization.XmlRootAttribute("individualAddresses", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualAddress/V1", IsNullable = false)]
    public partial class IndividualAddressesType
    {

        private AddressType[] addressField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("address")]
        public AddressType[] address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualTrait/V1")]
    [System.Xml.Serialization.XmlRootAttribute("individualTraits", Namespace = "http://schemas.wfm.com/Enterprise/IndividualMgmt/IndividualTrait/V1", IsNullable = false)]
    public partial class IndividualTraitsType
    {

        private TraitType[] traitField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("trait")]
        public TraitType[] trait
        {
            get
            {
                return this.traitField;
            }
            set
            {
                this.traitField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/InventoryMgmt/Inventory/V1")]
    [System.Xml.Serialization.XmlRootAttribute("inventory", Namespace = "http://schemas.wfm.com/Enterprise/InventoryMgmt/Inventory/V1", IsNullable = false)]
    public partial class InventoryType
    {

        private string inventoryIdField;

        private string typeCodeField;

        /// <remarks/>
        public string inventoryId
        {
            get
            {
                return this.inventoryIdField;
            }
            set
            {
                this.inventoryIdField = value;
            }
        }

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/InventoryMgmt/InventoryType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("inventoryType", Namespace = "http://schemas.wfm.com/Enterprise/InventoryMgmt/InventoryType/V1", IsNullable = false)]
    public partial class InventoryTypeType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/TransactionInventory/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionInventory", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/TransactionInventory/V1", IsNullable = false)]
    public partial class TransactionInventoryType
    {

        private InventoryType inventoryItemField;

        /// <remarks/>
        public InventoryType inventoryItem
        {
            get
            {
                return this.inventoryItemField;
            }
            set
            {
                this.inventoryItemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/BaseItem/V1")]
    [System.Xml.Serialization.XmlRootAttribute("baseItem", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/BaseItem/V1", IsNullable = false)]
    public partial class BaseItemType
    {

        private ItemTypeType typeField;

        private ConsumerInformationType consumerInformationField;

        private TraitType[] traitsField;

        /// <remarks/>
        public ItemTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public ConsumerInformationType consumerInformation
        {
            get
            {
                return this.consumerInformationField;
            }
            set
            {
                this.consumerInformationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trait", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public TraitType[] traits
        {
            get
            {
                return this.traitsField;
            }
            set
            {
                this.traitsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("itemType", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ItemType/V1", IsNullable = false)]
    public partial class ItemTypeType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ConsumerInformation/V1")]
    [System.Xml.Serialization.XmlRootAttribute("consumerInformation", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ConsumerInformation/V1", IsNullable = false)]
    public partial class ConsumerInformationType
    {

        private StockProductLabelType stockItemConsumerProductLabelField;

        /// <remarks/>
        public StockProductLabelType stockItemConsumerProductLabel
        {
            get
            {
                return this.stockItemConsumerProductLabelField;
            }
            set
            {
                this.stockItemConsumerProductLabelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class StockProductLabelType
    {

        private string consumerLabelTypeCodeField;

        private UomType servingSizeUomField;

        private decimal servingSizeUomCountField;

        private UomType servingsInRetailSaleUnitCountField;

        private decimal caloriesCountField;

        private decimal caloriesFromFatCountField;

        private decimal totalFatGramsAmountField;

        private decimal totalFatDailyIntakePercentField;

        private decimal saturatedFatGramsAmountField;

        private decimal saturatedFatPercentField;

        private decimal cholesterolMilligramsCountField;

        private decimal cholesterolPercentField;

        private decimal sodiumMilligramsCountField;

        private decimal sodiumPercentField;

        private decimal totalCarbohydrateMilligramsCountField;

        private decimal totalCarbohydratePercentField;

        private decimal dietaryFiberGramsCountField;

        private decimal sugarsGramsCountField;

        private decimal proteinGramsCountField;

        private decimal vitaminADailyMinimumPercentField;

        private decimal vitaminBDailyMinimumPercentField;

        private decimal vitaminCDailyMinimumPercentField;

        private decimal calciumDailyMinimumPercentField;

        private decimal ironDailyMinimumPercentField;

        private string nutritionalDescriptionTextField;

        private bool isHazardousMaterialField;

        private string hazardousMaterialTypeCodeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        public StockProductLabelType()
        {
            this.servingSizeUomCountField = ((decimal)(0m));
            this.caloriesCountField = ((decimal)(0m));
            this.caloriesFromFatCountField = ((decimal)(0m));
            this.totalFatGramsAmountField = ((decimal)(0m));
            this.totalFatDailyIntakePercentField = ((decimal)(0m));
            this.saturatedFatGramsAmountField = ((decimal)(0m));
            this.saturatedFatPercentField = ((decimal)(0m));
            this.cholesterolMilligramsCountField = ((decimal)(0m));
            this.cholesterolPercentField = ((decimal)(0m));
            this.sodiumMilligramsCountField = ((decimal)(0m));
            this.sodiumPercentField = ((decimal)(0m));
            this.totalCarbohydrateMilligramsCountField = ((decimal)(0m));
            this.totalCarbohydratePercentField = ((decimal)(0m));
            this.dietaryFiberGramsCountField = ((decimal)(0m));
            this.sugarsGramsCountField = ((decimal)(0m));
            this.proteinGramsCountField = ((decimal)(0m));
            this.vitaminADailyMinimumPercentField = ((decimal)(0m));
            this.vitaminBDailyMinimumPercentField = ((decimal)(0m));
            this.vitaminCDailyMinimumPercentField = ((decimal)(0m));
            this.calciumDailyMinimumPercentField = ((decimal)(0m));
            this.ironDailyMinimumPercentField = ((decimal)(0m));
            this.isHazardousMaterialField = false;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string consumerLabelTypeCode
        {
            get
            {
                return this.consumerLabelTypeCodeField;
            }
            set
            {
                this.consumerLabelTypeCodeField = value;
            }
        }

        /// <remarks/>
        public UomType servingSizeUom
        {
            get
            {
                return this.servingSizeUomField;
            }
            set
            {
                this.servingSizeUomField = value;
            }
        }

        /// <remarks/>
        public decimal servingSizeUomCount
        {
            get
            {
                return this.servingSizeUomCountField;
            }
            set
            {
                this.servingSizeUomCountField = value;
            }
        }

        /// <remarks/>
        public UomType servingsInRetailSaleUnitCount
        {
            get
            {
                return this.servingsInRetailSaleUnitCountField;
            }
            set
            {
                this.servingsInRetailSaleUnitCountField = value;
            }
        }

        /// <remarks/>
        public decimal caloriesCount
        {
            get
            {
                return this.caloriesCountField;
            }
            set
            {
                this.caloriesCountField = value;
            }
        }

        /// <remarks/>
        public decimal caloriesFromFatCount
        {
            get
            {
                return this.caloriesFromFatCountField;
            }
            set
            {
                this.caloriesFromFatCountField = value;
            }
        }

        /// <remarks/>
        public decimal totalFatGramsAmount
        {
            get
            {
                return this.totalFatGramsAmountField;
            }
            set
            {
                this.totalFatGramsAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalFatDailyIntakePercent
        {
            get
            {
                return this.totalFatDailyIntakePercentField;
            }
            set
            {
                this.totalFatDailyIntakePercentField = value;
            }
        }

        /// <remarks/>
        public decimal saturatedFatGramsAmount
        {
            get
            {
                return this.saturatedFatGramsAmountField;
            }
            set
            {
                this.saturatedFatGramsAmountField = value;
            }
        }

        /// <remarks/>
        public decimal saturatedFatPercent
        {
            get
            {
                return this.saturatedFatPercentField;
            }
            set
            {
                this.saturatedFatPercentField = value;
            }
        }

        /// <remarks/>
        public decimal cholesterolMilligramsCount
        {
            get
            {
                return this.cholesterolMilligramsCountField;
            }
            set
            {
                this.cholesterolMilligramsCountField = value;
            }
        }

        /// <remarks/>
        public decimal cholesterolPercent
        {
            get
            {
                return this.cholesterolPercentField;
            }
            set
            {
                this.cholesterolPercentField = value;
            }
        }

        /// <remarks/>
        public decimal sodiumMilligramsCount
        {
            get
            {
                return this.sodiumMilligramsCountField;
            }
            set
            {
                this.sodiumMilligramsCountField = value;
            }
        }

        /// <remarks/>
        public decimal sodiumPercent
        {
            get
            {
                return this.sodiumPercentField;
            }
            set
            {
                this.sodiumPercentField = value;
            }
        }

        /// <remarks/>
        public decimal totalCarbohydrateMilligramsCount
        {
            get
            {
                return this.totalCarbohydrateMilligramsCountField;
            }
            set
            {
                this.totalCarbohydrateMilligramsCountField = value;
            }
        }

        /// <remarks/>
        public decimal totalCarbohydratePercent
        {
            get
            {
                return this.totalCarbohydratePercentField;
            }
            set
            {
                this.totalCarbohydratePercentField = value;
            }
        }

        /// <remarks/>
        public decimal dietaryFiberGramsCount
        {
            get
            {
                return this.dietaryFiberGramsCountField;
            }
            set
            {
                this.dietaryFiberGramsCountField = value;
            }
        }

        /// <remarks/>
        public decimal sugarsGramsCount
        {
            get
            {
                return this.sugarsGramsCountField;
            }
            set
            {
                this.sugarsGramsCountField = value;
            }
        }

        /// <remarks/>
        public decimal proteinGramsCount
        {
            get
            {
                return this.proteinGramsCountField;
            }
            set
            {
                this.proteinGramsCountField = value;
            }
        }

        /// <remarks/>
        public decimal vitaminADailyMinimumPercent
        {
            get
            {
                return this.vitaminADailyMinimumPercentField;
            }
            set
            {
                this.vitaminADailyMinimumPercentField = value;
            }
        }

        /// <remarks/>
        public decimal vitaminBDailyMinimumPercent
        {
            get
            {
                return this.vitaminBDailyMinimumPercentField;
            }
            set
            {
                this.vitaminBDailyMinimumPercentField = value;
            }
        }

        /// <remarks/>
        public decimal vitaminCDailyMinimumPercent
        {
            get
            {
                return this.vitaminCDailyMinimumPercentField;
            }
            set
            {
                this.vitaminCDailyMinimumPercentField = value;
            }
        }

        /// <remarks/>
        public decimal calciumDailyMinimumPercent
        {
            get
            {
                return this.calciumDailyMinimumPercentField;
            }
            set
            {
                this.calciumDailyMinimumPercentField = value;
            }
        }

        /// <remarks/>
        public decimal ironDailyMinimumPercent
        {
            get
            {
                return this.ironDailyMinimumPercentField;
            }
            set
            {
                this.ironDailyMinimumPercentField = value;
            }
        }

        /// <remarks/>
        public string nutritionalDescriptionText
        {
            get
            {
                return this.nutritionalDescriptionTextField;
            }
            set
            {
                this.nutritionalDescriptionTextField = value;
            }
        }

        /// <remarks/>
        public bool isHazardousMaterial
        {
            get
            {
                return this.isHazardousMaterialField;
            }
            set
            {
                this.isHazardousMaterialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string hazardousMaterialTypeCode
        {
            get
            {
                return this.hazardousMaterialTypeCodeField;
            }
            set
            {
                this.hazardousMaterialTypeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/Item/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/Item/V1", IsNullable = false)]
    public partial class items
    {

        private ItemType[] itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("item")]
        public ItemType[] item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/Item/V1")]
    [System.Xml.Serialization.XmlRootAttribute("item", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/Item/V1", IsNullable = false)]
    public partial class ItemType
    {

        private int idField;

        private BaseItemType baseField;

        private LocaleType[] localeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public int id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public BaseItemType @base
        {
            get
            {
                return this.baseField;
            }
            set
            {
                this.baseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("locale")]
        public LocaleType[] locale
        {
            get
            {
                return this.localeField;
            }
            set
            {
                this.localeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ScanCode/V2")]
    [System.Xml.Serialization.XmlRootAttribute("scanCodes", Namespace = "http://schemas.wfm.com/Enterprise/ItemMgmt/ScanCode/V2", IsNullable = false)]
    public partial class ScanCodesType
    {

        private ScanCodeType[] scanCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("scanCode")]
        public ScanCodeType[] scanCode
        {
            get
            {
                return this.scanCodeField;
            }
            set
            {
                this.scanCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/RetailTransaction/LineItemType/" +
        "TransactionItemBaseType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionItemBase", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/RetailTransaction/LineItemType/" +
        "TransactionItemBaseType/V1", IsNullable = false)]
    public partial class TransactionItemBaseType
    {

        private bool isNotOnFileField;

        private bool isNotOnFileFieldSpecified;

        private string idField;

        private RetailTransactionItemTypeEnum posItemTypeField;

        private bool posItemTypeFieldSpecified;

        private ItemType posItemField;

        private AmountType extendedAmountField;

        private AmountType extendedDiscountAmountField;

        private QuantityType[] quantitiesField;

        private RetailPriceModifierType[] retailPriceModifiersField;

        private GroupIndicatorType groupIndicatorsField;

        private PromotionReward[] promotionTenderRewardsField;

        private PromotionDeferredReward[] promotionDeferredRewardsField;

        private TransactionTaxType[] taxesField;

        private WICType wicField;

        /// <remarks/>
        public bool isNotOnFile
        {
            get
            {
                return this.isNotOnFileField;
            }
            set
            {
                this.isNotOnFileField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isNotOnFileSpecified
        {
            get
            {
                return this.isNotOnFileFieldSpecified;
            }
            set
            {
                this.isNotOnFileFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public RetailTransactionItemTypeEnum posItemType
        {
            get
            {
                return this.posItemTypeField;
            }
            set
            {
                this.posItemTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool posItemTypeSpecified
        {
            get
            {
                return this.posItemTypeFieldSpecified;
            }
            set
            {
                this.posItemTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ItemType posItem
        {
            get
            {
                return this.posItemField;
            }
            set
            {
                this.posItemField = value;
            }
        }

        /// <remarks/>
        public AmountType extendedAmount
        {
            get
            {
                return this.extendedAmountField;
            }
            set
            {
                this.extendedAmountField = value;
            }
        }

        /// <remarks/>
        public AmountType extendedDiscountAmount
        {
            get
            {
                return this.extendedDiscountAmountField;
            }
            set
            {
                this.extendedDiscountAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("quantity", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = false)]
        public QuantityType[] quantities
        {
            get
            {
                return this.quantitiesField;
            }
            set
            {
                this.quantitiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("modifier", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = false)]
        public RetailPriceModifierType[] retailPriceModifiers
        {
            get
            {
                return this.retailPriceModifiersField;
            }
            set
            {
                this.retailPriceModifiersField = value;
            }
        }

        /// <remarks/>
        public GroupIndicatorType groupIndicators
        {
            get
            {
                return this.groupIndicatorsField;
            }
            set
            {
                this.groupIndicatorsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("reward", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = false)]
        public PromotionReward[] promotionTenderRewards
        {
            get
            {
                return this.promotionTenderRewardsField;
            }
            set
            {
                this.promotionTenderRewardsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("deferredReward", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = false)]
        public PromotionDeferredReward[] promotionDeferredRewards
        {
            get
            {
                return this.promotionDeferredRewardsField;
            }
            set
            {
                this.promotionDeferredRewardsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("tax", Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TransactionTax/V1", IsNullable = false)]
        public TransactionTaxType[] taxes
        {
            get
            {
                return this.taxesField;
            }
            set
            {
                this.taxesField = value;
            }
        }

        /// <remarks/>
        public WICType wic
        {
            get
            {
                return this.wicField;
            }
            set
            {
                this.wicField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum RetailTransactionItemTypeEnum
    {

        /// <remarks/>
        Coupon,

        /// <remarks/>
        Deposit,

        /// <remarks/>
        DepositRefund,

        /// <remarks/>
        Fee,

        /// <remarks/>
        FeeRefund,

        /// <remarks/>
        ItemCollection,

        /// <remarks/>
        Service,

        /// <remarks/>
        Tare,

        /// <remarks/>
        Warranty,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class QuantityType
    {

        private string typeField;

        private decimal valueField;

        private bool valueFieldSpecified;

        private UnitsType unitsField;

        private EntryMethodEnum entryMethodField;

        private bool entryMethodFieldSpecified;

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }

        /// <remarks/>
        public UnitsType units
        {
            get
            {
                return this.unitsField;
            }
            set
            {
                this.unitsField = value;
            }
        }

        /// <remarks/>
        public EntryMethodEnum entryMethod
        {
            get
            {
                return this.entryMethodField;
            }
            set
            {
                this.entryMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entryMethodSpecified
        {
            get
            {
                return this.entryMethodFieldSpecified;
            }
            set
            {
                this.entryMethodFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class UnitsType
    {

        private decimal valueField;

        private bool valueFieldSpecified;

        private UomType uomField;

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }

        /// <remarks/>
        public UomType uom
        {
            get
            {
                return this.uomField;
            }
            set
            {
                this.uomField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum EntryMethodEnum
    {

        /// <remarks/>
        Automatic,

        /// <remarks/>
        CardholderNotPresent,

        /// <remarks/>
        Ecommerce,

        /// <remarks/>
        Imported,

        /// <remarks/>
        Imprint,

        /// <remarks/>
        IntegratedChipCard,

        /// <remarks/>
        Keyed,

        /// <remarks/>
        KeyedCoupon,

        /// <remarks/>
        MICR,

        /// <remarks/>
        Measured,

        /// <remarks/>
        NotRelevant,

        /// <remarks/>
        MSR,

        /// <remarks/>
        Promotion,

        /// <remarks/>
        Repeat,

        /// <remarks/>
        RFID,

        /// <remarks/>
        Scanned,

        /// <remarks/>
        SmartCard,

        /// <remarks/>
        Swiped,

        /// <remarks/>
        Tapped,

        /// <remarks/>
        Waved,

        /// <remarks/>
        Weighed,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class RetailPriceModifierType
    {

        private RetailPriceModifierMethodEnum methodField;

        private bool methodFieldSpecified;

        private ulong sequenceNumberField;

        private bool sequenceNumberFieldSpecified;

        private AmountType amountField;

        private decimal percentField;

        private bool percentFieldSpecified;

        private System.DateTime expirationDateField;

        private bool expirationDateFieldSpecified;

        private PriceType previousPriceField;

        private string promotionIdField;

        private DescriptionCommonData descriptionField;

        private LinkTypeType[] itemLinksField;

        private QuantityType quantityField;

        private ReasonCodeType reasonCodeField;

        private LineItemRewardPromotionType lineItemRewardPromotionField;

        /// <remarks/>
        public RetailPriceModifierMethodEnum method
        {
            get
            {
                return this.methodField;
            }
            set
            {
                this.methodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool methodSpecified
        {
            get
            {
                return this.methodFieldSpecified;
            }
            set
            {
                this.methodFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sequenceNumberSpecified
        {
            get
            {
                return this.sequenceNumberFieldSpecified;
            }
            set
            {
                this.sequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AmountType amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public decimal percent
        {
            get
            {
                return this.percentField;
            }
            set
            {
                this.percentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentSpecified
        {
            get
            {
                return this.percentFieldSpecified;
            }
            set
            {
                this.percentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expirationDateSpecified
        {
            get
            {
                return this.expirationDateFieldSpecified;
            }
            set
            {
                this.expirationDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public PriceType previousPrice
        {
            get
            {
                return this.previousPriceField;
            }
            set
            {
                this.previousPriceField = value;
            }
        }

        /// <remarks/>
        public string promotionId
        {
            get
            {
                return this.promotionIdField;
            }
            set
            {
                this.promotionIdField = value;
            }
        }

        /// <remarks/>
        public DescriptionCommonData description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("link", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public LinkTypeType[] itemLinks
        {
            get
            {
                return this.itemLinksField;
            }
            set
            {
                this.itemLinksField = value;
            }
        }

        /// <remarks/>
        public QuantityType quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType reasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
            }
        }

        /// <remarks/>
        public LineItemRewardPromotionType lineItemRewardPromotion
        {
            get
            {
                return this.lineItemRewardPromotionField;
            }
            set
            {
                this.lineItemRewardPromotionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum RetailPriceModifierMethodEnum
    {

        /// <remarks/>
        PriceOverride,

        /// <remarks/>
        PriceRule,

        /// <remarks/>
        Promotion,

        /// <remarks/>
        SeniorCitizen,

        /// <remarks/>
        Markdown,

        /// <remarks/>
        Coupon,

        /// <remarks/>
        QuantityDiscount,

        /// <remarks/>
        Rebate,

        /// <remarks/>
        CashDiscount,

        /// <remarks/>
        TradeInKind,

        /// <remarks/>
        GeneralDiscount,

        /// <remarks/>
        GiftVoucher,

        /// <remarks/>
        FlexibleDiscount,

        /// <remarks/>
        RewardProgram,

        /// <remarks/>
        ManufacturerReward,

        /// <remarks/>
        CreditCardReward,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class DescriptionCommonData
    {

        private LanguageCodeEnum languageField;

        private bool languageFieldSpecified;

        private string typeCodeField;

        private CultureTypeCodeEnum cultureField;

        private bool cultureFieldSpecified;

        private string descriptionField;

        /// <remarks/>
        public LanguageCodeEnum language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool languageSpecified
        {
            get
            {
                return this.languageFieldSpecified;
            }
            set
            {
                this.languageFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        public CultureTypeCodeEnum culture
        {
            get
            {
                return this.cultureField;
            }
            set
            {
                this.cultureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool cultureSpecified
        {
            get
            {
                return this.cultureFieldSpecified;
            }
            set
            {
                this.cultureFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class LineItemRewardPromotionType
    {

        private QuantityType triggerQuantityField;

        private ApportionmentAmountType1[] apportionmentAmountField;

        private AmountType rewardSplitAmountField;

        private TenderId tenderIDField;

        private RewardLevel rewardLevelField;

        /// <remarks/>
        public QuantityType triggerQuantity
        {
            get
            {
                return this.triggerQuantityField;
            }
            set
            {
                this.triggerQuantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("apportionmentAmount")]
        public ApportionmentAmountType1[] apportionmentAmount
        {
            get
            {
                return this.apportionmentAmountField;
            }
            set
            {
                this.apportionmentAmountField = value;
            }
        }

        /// <remarks/>
        public AmountType rewardSplitAmount
        {
            get
            {
                return this.rewardSplitAmountField;
            }
            set
            {
                this.rewardSplitAmountField = value;
            }
        }

        /// <remarks/>
        public TenderId tenderID
        {
            get
            {
                return this.tenderIDField;
            }
            set
            {
                this.tenderIDField = value;
            }
        }

        /// <remarks/>
        public RewardLevel rewardLevel
        {
            get
            {
                return this.rewardLevelField;
            }
            set
            {
                this.rewardLevelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderId
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class RewardLevel
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public int Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class GroupIndicatorType
    {

        private string indicatorField;

        private string groupField;

        private string symbolField;

        private int priorityField;

        private bool priorityFieldSpecified;

        /// <remarks/>
        public string indicator
        {
            get
            {
                return this.indicatorField;
            }
            set
            {
                this.indicatorField = value;
            }
        }

        /// <remarks/>
        public string group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        public string symbol
        {
            get
            {
                return this.symbolField;
            }
            set
            {
                this.symbolField = value;
            }
        }

        /// <remarks/>
        public int priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified
        {
            get
            {
                return this.priorityFieldSpecified;
            }
            set
            {
                this.priorityFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PromotionDeferredReward))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class PromotionReward
    {

        private string typeField;

        private decimal valueField;

        private string promotionIDField;

        private DescriptionCommonData descriptionField;

        private LineItemRewardPromotionType lineItemRewardPromotionField;

        private ulong sequenceNumberField;

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public string promotionID
        {
            get
            {
                return this.promotionIDField;
            }
            set
            {
                this.promotionIDField = value;
            }
        }

        /// <remarks/>
        public DescriptionCommonData description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public LineItemRewardPromotionType lineItemRewardPromotion
        {
            get
            {
                return this.lineItemRewardPromotionField;
            }
            set
            {
                this.lineItemRewardPromotionField = value;
            }
        }

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class PromotionDeferredReward : PromotionReward
    {

        private DeferredId deferredIdField;

        /// <remarks/>
        public DeferredId deferredId
        {
            get
            {
                return this.deferredIdField;
            }
            set
            {
                this.deferredIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class DeferredId
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TransactionTax/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionTax", Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TransactionTax/V1", IsNullable = false)]
    public partial class TransactionTaxType
    {

        private bool isEatInField;

        private bool isEatInFieldSpecified;

        private bool isCateringField;

        private bool isCateringFieldSpecified;

        private bool isReversedField;

        private bool isReversedFieldSpecified;

        private TaxType taxTypeField;

        private TaxSaleTypeType taxSaleTypeField;

        private bool taxSaleTypeFieldSpecified;

        private TaxSubType taxSubTypeField;

        private TaxAtSourceTypeCodeEnum taxAtSourceField;

        private bool taxAtSourceFieldSpecified;

        private Sign signField;

        private Imposition impositionField;

        private CalulcationMethod calculationMethodField;

        private OriginalPercent orignalPercentField;

        private CouponReducesTaxationAmount couponReducesTaxationAmountField;

        private TaxRoundingMethod taxRoundingMethodField;

        private ulong sequenceNumberField;

        private bool sequenceNumberFieldSpecified;

        private TaxAuthorityType taxAuthorityField;

        private AmountType taxableAmountField;

        private AmountType amountField;

        private decimal percentField;

        private bool percentFieldSpecified;

        private ReasonCodeType reasonCodeField;

        private string[] taxRuleIDField;

        private string[] taxGroupIDField;

        private string dependenceTaxRuleIDField;

        private TaxExemptionBase taxExemptionField;

        private TaxOverrideBase taxOverrideField;

        /// <remarks/>
        public bool isEatIn
        {
            get
            {
                return this.isEatInField;
            }
            set
            {
                this.isEatInField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isEatInSpecified
        {
            get
            {
                return this.isEatInFieldSpecified;
            }
            set
            {
                this.isEatInFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool isCatering
        {
            get
            {
                return this.isCateringField;
            }
            set
            {
                this.isCateringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isCateringSpecified
        {
            get
            {
                return this.isCateringFieldSpecified;
            }
            set
            {
                this.isCateringFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool isReversed
        {
            get
            {
                return this.isReversedField;
            }
            set
            {
                this.isReversedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isReversedSpecified
        {
            get
            {
                return this.isReversedFieldSpecified;
            }
            set
            {
                this.isReversedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TaxType taxType
        {
            get
            {
                return this.taxTypeField;
            }
            set
            {
                this.taxTypeField = value;
            }
        }

        /// <remarks/>
        public TaxSaleTypeType taxSaleType
        {
            get
            {
                return this.taxSaleTypeField;
            }
            set
            {
                this.taxSaleTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool taxSaleTypeSpecified
        {
            get
            {
                return this.taxSaleTypeFieldSpecified;
            }
            set
            {
                this.taxSaleTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TaxSubType taxSubType
        {
            get
            {
                return this.taxSubTypeField;
            }
            set
            {
                this.taxSubTypeField = value;
            }
        }

        /// <remarks/>
        public TaxAtSourceTypeCodeEnum taxAtSource
        {
            get
            {
                return this.taxAtSourceField;
            }
            set
            {
                this.taxAtSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool taxAtSourceSpecified
        {
            get
            {
                return this.taxAtSourceFieldSpecified;
            }
            set
            {
                this.taxAtSourceFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Sign sign
        {
            get
            {
                return this.signField;
            }
            set
            {
                this.signField = value;
            }
        }

        /// <remarks/>
        public Imposition imposition
        {
            get
            {
                return this.impositionField;
            }
            set
            {
                this.impositionField = value;
            }
        }

        /// <remarks/>
        public CalulcationMethod calculationMethod
        {
            get
            {
                return this.calculationMethodField;
            }
            set
            {
                this.calculationMethodField = value;
            }
        }

        /// <remarks/>
        public OriginalPercent orignalPercent
        {
            get
            {
                return this.orignalPercentField;
            }
            set
            {
                this.orignalPercentField = value;
            }
        }

        /// <remarks/>
        public CouponReducesTaxationAmount couponReducesTaxationAmount
        {
            get
            {
                return this.couponReducesTaxationAmountField;
            }
            set
            {
                this.couponReducesTaxationAmountField = value;
            }
        }

        /// <remarks/>
        public TaxRoundingMethod taxRoundingMethod
        {
            get
            {
                return this.taxRoundingMethodField;
            }
            set
            {
                this.taxRoundingMethodField = value;
            }
        }

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sequenceNumberSpecified
        {
            get
            {
                return this.sequenceNumberFieldSpecified;
            }
            set
            {
                this.sequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TaxAuthorityType taxAuthority
        {
            get
            {
                return this.taxAuthorityField;
            }
            set
            {
                this.taxAuthorityField = value;
            }
        }

        /// <remarks/>
        public AmountType taxableAmount
        {
            get
            {
                return this.taxableAmountField;
            }
            set
            {
                this.taxableAmountField = value;
            }
        }

        /// <remarks/>
        public AmountType amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public decimal percent
        {
            get
            {
                return this.percentField;
            }
            set
            {
                this.percentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool percentSpecified
        {
            get
            {
                return this.percentFieldSpecified;
            }
            set
            {
                this.percentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType reasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxRuleID")]
        public string[] taxRuleID
        {
            get
            {
                return this.taxRuleIDField;
            }
            set
            {
                this.taxRuleIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxGroupID")]
        public string[] taxGroupID
        {
            get
            {
                return this.taxGroupIDField;
            }
            set
            {
                this.taxGroupIDField = value;
            }
        }

        /// <remarks/>
        public string dependenceTaxRuleID
        {
            get
            {
                return this.dependenceTaxRuleIDField;
            }
            set
            {
                this.dependenceTaxRuleIDField = value;
            }
        }

        /// <remarks/>
        public TaxExemptionBase taxExemption
        {
            get
            {
                return this.taxExemptionField;
            }
            set
            {
                this.taxExemptionField = value;
            }
        }

        /// <remarks/>
        public TaxOverrideBase taxOverride
        {
            get
            {
                return this.taxOverrideField;
            }
            set
            {
                this.taxOverrideField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxType
    {

        private TaxTypeCodeEnum valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public TaxTypeCodeEnum Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = false)]
    public enum TaxTypeCodeEnum
    {

        /// <remarks/>
        Sales,

        /// <remarks/>
        VAT,

        /// <remarks/>
        GST,

        /// <remarks/>
        PST,

        /// <remarks/>
        HST,

        /// <remarks/>
        Excise,

        /// <remarks/>
        UseTax,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = false)]
    public enum TaxSaleTypeType
    {

        /// <remarks/>
        Sale,

        /// <remarks/>
        Refund,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxSubType
    {

        private TaxSubTypeCode valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public TaxSubTypeCode Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute("TaxSubTypeCodeEnum", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = false)]
    public enum TaxSubTypeCode
    {

        /// <remarks/>
        Standard,

        /// <remarks/>
        Luxury,

        /// <remarks/>
        Exempt,

        /// <remarks/>
        ZeroRated,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = false)]
    public enum TaxAtSourceTypeCodeEnum
    {

        /// <remarks/>
        TaxedAtOrigin,

        /// <remarks/>
        TaxedAtDestination,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class Sign
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class Imposition
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class CalulcationMethod
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class OriginalPercent
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxRoundingMethod
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxAuthority/V1")]
    [System.Xml.Serialization.XmlRootAttribute("taxAuthority", Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxAuthority/V1", IsNullable = false)]
    public partial class TaxAuthorityType
    {

        private string idField;

        private string nameField;

        private AuthorityLevelTypeEnum levelField;

        private bool levelFieldSpecified;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public AuthorityLevelTypeEnum Level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LevelSpecified
        {
            get
            {
                return this.levelFieldSpecified;
            }
            set
            {
                this.levelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum AuthorityLevelTypeEnum
    {

        /// <remarks/>
        Federal,

        /// <remarks/>
        State,

        /// <remarks/>
        County,

        /// <remarks/>
        City,

        /// <remarks/>
        Local,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxExemptionBase
    {

        private string customerExemptionIDField;

        private AmountType exemptTaxableAmountField;

        private AmountType exemptTaxAmountField;

        private ReasonCodeType reasonCodeField;

        private ApprovalBase[] operatorBypassApprovalField;

        private int certificateNumberField;

        private bool certificateNumberFieldSpecified;

        private string certificateHolderNameField;

        /// <remarks/>
        public string customerExemptionID
        {
            get
            {
                return this.customerExemptionIDField;
            }
            set
            {
                this.customerExemptionIDField = value;
            }
        }

        /// <remarks/>
        public AmountType exemptTaxableAmount
        {
            get
            {
                return this.exemptTaxableAmountField;
            }
            set
            {
                this.exemptTaxableAmountField = value;
            }
        }

        /// <remarks/>
        public AmountType exemptTaxAmount
        {
            get
            {
                return this.exemptTaxAmountField;
            }
            set
            {
                this.exemptTaxAmountField = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType reasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("operatorBypassApproval")]
        public ApprovalBase[] operatorBypassApproval
        {
            get
            {
                return this.operatorBypassApprovalField;
            }
            set
            {
                this.operatorBypassApprovalField = value;
            }
        }

        /// <remarks/>
        public int certificateNumber
        {
            get
            {
                return this.certificateNumberField;
            }
            set
            {
                this.certificateNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool certificateNumberSpecified
        {
            get
            {
                return this.certificateNumberFieldSpecified;
            }
            set
            {
                this.certificateNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string certificateHolderName
        {
            get
            {
                return this.certificateHolderNameField;
            }
            set
            {
                this.certificateHolderNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ApprovalBase
    {

        private ulong sequenceNumberField;

        private PartyType approverField;

        private LineApprovalCode lineApprovalCodeField;

        private DescriptionCommonData descriptionField;

        private EntryMethodEnum entryMethodField;

        private bool entryMethodFieldSpecified;

        private DateTimezoneType approvalDateTimeField;

        private BusinessRuleManagerType businessRuleManagerField;

        private PolicyType[] policyRuleManagerField;

        private ReturnPolicy[] returnPolicyField;

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public PartyType approver
        {
            get
            {
                return this.approverField;
            }
            set
            {
                this.approverField = value;
            }
        }

        /// <remarks/>
        public LineApprovalCode lineApprovalCode
        {
            get
            {
                return this.lineApprovalCodeField;
            }
            set
            {
                this.lineApprovalCodeField = value;
            }
        }

        /// <remarks/>
        public DescriptionCommonData description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public EntryMethodEnum entryMethod
        {
            get
            {
                return this.entryMethodField;
            }
            set
            {
                this.entryMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entryMethodSpecified
        {
            get
            {
                return this.entryMethodFieldSpecified;
            }
            set
            {
                this.entryMethodFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType approvalDateTime
        {
            get
            {
                return this.approvalDateTimeField;
            }
            set
            {
                this.approvalDateTimeField = value;
            }
        }

        /// <remarks/>
        public BusinessRuleManagerType businessRuleManager
        {
            get
            {
                return this.businessRuleManagerField;
            }
            set
            {
                this.businessRuleManagerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("policy", IsNullable = false)]
        public PolicyType[] policyRuleManager
        {
            get
            {
                return this.policyRuleManagerField;
            }
            set
            {
                this.policyRuleManagerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("returnPolicy")]
        public ReturnPolicy[] returnPolicy
        {
            get
            {
                return this.returnPolicyField;
            }
            set
            {
                this.returnPolicyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/Party/V2")]
    [System.Xml.Serialization.XmlRootAttribute("party", Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/Party/V2", IsNullable = false)]
    public partial class PartyType
    {

        private string idField;

        private PartyIdentifiersType identifiersField;

        private DemographicType[] demographicsField;

        private PartyTypeType typeField;

        private StatusHistoryType[] statusHistoriesField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public PartyIdentifiersType identifiers
        {
            get
            {
                return this.identifiersField;
            }
            set
            {
                this.identifiersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("demographic", Namespace = "http://schemas.wfm.com/Enterprise/DemographicMgmt/Demographic/V1", IsNullable = false)]
        public DemographicType[] demographics
        {
            get
            {
                return this.demographicsField;
            }
            set
            {
                this.demographicsField = value;
            }
        }

        /// <remarks/>
        public PartyTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("statusHistory", Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/StatusHistory/V1", IsNullable = false)]
        public StatusHistoryType[] statusHistories
        {
            get
            {
                return this.statusHistoriesField;
            }
            set
            {
                this.statusHistoriesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyIdentifier/V2")]
    [System.Xml.Serialization.XmlRootAttribute("partyIdentifiersType", Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyIdentifier/V2", IsNullable = false)]
    public partial class PartyIdentifiersType
    {

        private PartyIdentifierType identifierField;

        /// <remarks/>
        public PartyIdentifierType identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyIdentifier/V2")]
    [System.Xml.Serialization.XmlRootAttribute("partyIdentifier", Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyIdentifier/V2", IsNullable = false)]
    public partial class PartyIdentifierType
    {

        private PartyIdentifierTypeType typeField;

        private string valueField;

        /// <remarks/>
        public PartyIdentifierTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyIdentifierType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("partyIdentifierType", Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyIdentifierType/V2", IsNullable = false)]
    public partial class PartyIdentifierTypeType
    {

        private string nameField;

        private string descriptionField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("partyType", Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyType/V2", IsNullable = false)]
    public partial class PartyTypeType
    {

        private string typeCodeField;

        private PartyTypeDescType typeDescriptionField;

        private object itemField;

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        public PartyTypeDescType typeDescription
        {
            get
            {
                return this.typeDescriptionField;
            }
            set
            {
                this.typeDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("individual", typeof(IndividualType))]
        [System.Xml.Serialization.XmlElementAttribute("organization", typeof(OrganizationType))]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum PartyTypeDescType
    {

        /// <remarks/>
        Individual,

        /// <remarks/>
        Organization,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class LineApprovalCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/Timezone/V2")]
    [System.Xml.Serialization.XmlRootAttribute("dateTimezone", Namespace = "http://schemas.wfm.com/Enterprise/TimezoneMgmt/Timezone/V2", IsNullable = false)]
    public partial class DateTimezoneType
    {

        private System.DateTime dateTimeField;

        private TimezoneType timezoneField;

        /// <remarks/>
        public System.DateTime dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        public TimezoneType timezone
        {
            get
            {
                return this.timezoneField;
            }
            set
            {
                this.timezoneField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class BusinessRuleManagerType
    {

        private BusinessRuleType businessRuleField;

        /// <remarks/>
        public BusinessRuleType businessRule
        {
            get
            {
                return this.businessRuleField;
            }
            set
            {
                this.businessRuleField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class BusinessRuleType
    {

        private BusinessActionType businessActionField;

        private RuleType ruleTypeField;

        private RuleName ruleNameField;

        /// <remarks/>
        public BusinessActionType businessAction
        {
            get
            {
                return this.businessActionField;
            }
            set
            {
                this.businessActionField = value;
            }
        }

        /// <remarks/>
        public RuleType ruleType
        {
            get
            {
                return this.ruleTypeField;
            }
            set
            {
                this.ruleTypeField = value;
            }
        }

        /// <remarks/>
        public RuleName ruleName
        {
            get
            {
                return this.ruleNameField;
            }
            set
            {
                this.ruleNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class BusinessActionType
    {

        private string messageNameField;

        private string inputTypeField;

        private string actionTypeField;

        private string inputValueField;

        private bool isApprovedField;

        private bool isApprovedFieldSpecified;

        private ulong lineItemSequenceLinkField;

        private bool lineItemSequenceLinkFieldSpecified;

        private string reasonGroupField;

        private string reasonNameField;

        private ReasonCodeType reasonCodeField;

        private bool requiresAttendantField;

        private bool requiresAttendantFieldSpecified;

        /// <remarks/>
        public string messageName
        {
            get
            {
                return this.messageNameField;
            }
            set
            {
                this.messageNameField = value;
            }
        }

        /// <remarks/>
        public string inputType
        {
            get
            {
                return this.inputTypeField;
            }
            set
            {
                this.inputTypeField = value;
            }
        }

        /// <remarks/>
        public string actionType
        {
            get
            {
                return this.actionTypeField;
            }
            set
            {
                this.actionTypeField = value;
            }
        }

        /// <remarks/>
        public string inputValue
        {
            get
            {
                return this.inputValueField;
            }
            set
            {
                this.inputValueField = value;
            }
        }

        /// <remarks/>
        public bool isApproved
        {
            get
            {
                return this.isApprovedField;
            }
            set
            {
                this.isApprovedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isApprovedSpecified
        {
            get
            {
                return this.isApprovedFieldSpecified;
            }
            set
            {
                this.isApprovedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ulong lineItemSequenceLink
        {
            get
            {
                return this.lineItemSequenceLinkField;
            }
            set
            {
                this.lineItemSequenceLinkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lineItemSequenceLinkSpecified
        {
            get
            {
                return this.lineItemSequenceLinkFieldSpecified;
            }
            set
            {
                this.lineItemSequenceLinkFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string reasonGroup
        {
            get
            {
                return this.reasonGroupField;
            }
            set
            {
                this.reasonGroupField = value;
            }
        }

        /// <remarks/>
        public string reasonName
        {
            get
            {
                return this.reasonNameField;
            }
            set
            {
                this.reasonNameField = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType reasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
            }
        }

        /// <remarks/>
        public bool requiresAttendant
        {
            get
            {
                return this.requiresAttendantField;
            }
            set
            {
                this.requiresAttendantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool requiresAttendantSpecified
        {
            get
            {
                return this.requiresAttendantFieldSpecified;
            }
            set
            {
                this.requiresAttendantFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class RuleType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class RuleName
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class PolicyType
    {

        private PolicyRuleType[] policyRulesField;

        private string policyIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("rule", IsNullable = false)]
        public PolicyRuleType[] policyRules
        {
            get
            {
                return this.policyRulesField;
            }
            set
            {
                this.policyRulesField = value;
            }
        }

        /// <remarks/>
        public string policyID
        {
            get
            {
                return this.policyIDField;
            }
            set
            {
                this.policyIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class PolicyRuleType
    {

        private BusinessActionType businessActionField;

        private PartyType approverIDField;

        private string ruleTypeField;

        private EntryMethodEnum entryMethodField;

        /// <remarks/>
        public BusinessActionType businessAction
        {
            get
            {
                return this.businessActionField;
            }
            set
            {
                this.businessActionField = value;
            }
        }

        /// <remarks/>
        public PartyType approverID
        {
            get
            {
                return this.approverIDField;
            }
            set
            {
                this.approverIDField = value;
            }
        }

        /// <remarks/>
        public string ruleType
        {
            get
            {
                return this.ruleTypeField;
            }
            set
            {
                this.ruleTypeField = value;
            }
        }

        /// <remarks/>
        public EntryMethodEnum entryMethod
        {
            get
            {
                return this.entryMethodField;
            }
            set
            {
                this.entryMethodField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ReturnPolicy
    {

        private Rule ruleField;

        private string idField;

        /// <remarks/>
        public Rule rule
        {
            get
            {
                return this.ruleField;
            }
            set
            {
                this.ruleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class Rule
    {

        private ActionType actionField;

        private string ruleNameField;

        /// <remarks/>
        public ActionType action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        public string ruleName
        {
            get
            {
                return this.ruleNameField;
            }
            set
            {
                this.ruleNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ActionType
    {

        private string messageNameField;

        private string actionTypeField;

        private bool isApprovedField;

        private bool isApprovedFieldSpecified;

        /// <remarks/>
        public string messageName
        {
            get
            {
                return this.messageNameField;
            }
            set
            {
                this.messageNameField = value;
            }
        }

        /// <remarks/>
        public string actionType
        {
            get
            {
                return this.actionTypeField;
            }
            set
            {
                this.actionTypeField = value;
            }
        }

        /// <remarks/>
        public bool isApproved
        {
            get
            {
                return this.isApprovedField;
            }
            set
            {
                this.isApprovedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isApprovedSpecified
        {
            get
            {
                return this.isApprovedFieldSpecified;
            }
            set
            {
                this.isApprovedFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxOverrideBase
    {

        private string customerOverrideIDField;

        private AmountType taxableAmountField;

        private decimal originalPercentField;

        private AmountType originalTaxAmountField;

        private decimal newTaxPercentField;

        private AmountType newTaxAmountField;

        private ReasonCodeType reasonCodeField;

        private int certificateNumberField;

        private bool certificateNumberFieldSpecified;

        private string certificateHolderNameField;

        private ApprovalBase[] operatorBypassApprovalField;

        /// <remarks/>
        public string customerOverrideID
        {
            get
            {
                return this.customerOverrideIDField;
            }
            set
            {
                this.customerOverrideIDField = value;
            }
        }

        /// <remarks/>
        public AmountType taxableAmount
        {
            get
            {
                return this.taxableAmountField;
            }
            set
            {
                this.taxableAmountField = value;
            }
        }

        /// <remarks/>
        public decimal originalPercent
        {
            get
            {
                return this.originalPercentField;
            }
            set
            {
                this.originalPercentField = value;
            }
        }

        /// <remarks/>
        public AmountType originalTaxAmount
        {
            get
            {
                return this.originalTaxAmountField;
            }
            set
            {
                this.originalTaxAmountField = value;
            }
        }

        /// <remarks/>
        public decimal newTaxPercent
        {
            get
            {
                return this.newTaxPercentField;
            }
            set
            {
                this.newTaxPercentField = value;
            }
        }

        /// <remarks/>
        public AmountType newTaxAmount
        {
            get
            {
                return this.newTaxAmountField;
            }
            set
            {
                this.newTaxAmountField = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType reasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
            }
        }

        /// <remarks/>
        public int certificateNumber
        {
            get
            {
                return this.certificateNumberField;
            }
            set
            {
                this.certificateNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool certificateNumberSpecified
        {
            get
            {
                return this.certificateNumberFieldSpecified;
            }
            set
            {
                this.certificateNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string certificateHolderName
        {
            get
            {
                return this.certificateHolderNameField;
            }
            set
            {
                this.certificateHolderNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("operatorBypassApproval")]
        public ApprovalBase[] operatorBypassApproval
        {
            get
            {
                return this.operatorBypassApprovalField;
            }
            set
            {
                this.operatorBypassApprovalField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class WICType
    {

        private AmountType amountField;

        private AmountType notExceedAmountField;

        private AmountType lostAmountField;

        /// <remarks/>
        public AmountType amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public AmountType notExceedAmount
        {
            get
            {
                return this.notExceedAmountField;
            }
            set
            {
                this.notExceedAmountField = value;
            }
        }

        /// <remarks/>
        public AmountType lostAmount
        {
            get
            {
                return this.lostAmountField;
            }
            set
            {
                this.lostAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/LocaleTrait/V1")]
    [System.Xml.Serialization.XmlRootAttribute("localeTrait", Namespace = "http://schemas.wfm.com/Enterprise/LocaleMgmt/LocaleTrait/V1", IsNullable = false)]
    public partial class LocaleTraitType
    {

        private TraitType traitField;

        /// <remarks/>
        public TraitType trait
        {
            get
            {
                return this.traitField;
            }
            set
            {
                this.traitField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/Organization/V2")]
    [System.Xml.Serialization.XmlRootAttribute("organizations", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/Organization/V2", IsNullable = false)]
    public partial class OrganizationsType
    {

        private OrganizationType[] organizationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("organization")]
        public OrganizationType[] organization
        {
            get
            {
                return this.organizationField;
            }
            set
            {
                this.organizationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationNameType/V2")]
    [System.Xml.Serialization.XmlRootAttribute("organizationNameTypes", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationNameType/V2", IsNullable = false)]
    public partial class OrganizationNameTypesType
    {

        private OrganizationNameTypeType[] nameTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("nameType")]
        public OrganizationNameTypeType[] nameType
        {
            get
            {
                return this.nameTypeField;
            }
            set
            {
                this.nameTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationTrait/V1")]
    [System.Xml.Serialization.XmlRootAttribute("OrganizationTraits", Namespace = "http://schemas.wfm.com/Enterprise/OrganizationMgmt/OrganizationTrait/V1", IsNullable = false)]
    public partial class OrganizationTraitsType
    {

        private TraitType[] traitField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("trait")]
        public TraitType[] trait
        {
            get
            {
                return this.traitField;
            }
            set
            {
                this.traitField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TransactionAttributes
    {

        private string entryMethodField;

        private string scanDataField;

        private string taxCertificateField;

        private string taxregistrationField;

        private string taxExemptReasonField;

        private bool isVoidField;

        private bool isVoidFieldSpecified;

        private bool isAuthenticatedOfflineField;

        private bool isAuthenticatedOfflineFieldSpecified;

        /// <remarks/>
        public string entryMethod
        {
            get
            {
                return this.entryMethodField;
            }
            set
            {
                this.entryMethodField = value;
            }
        }

        /// <remarks/>
        public string scanData
        {
            get
            {
                return this.scanDataField;
            }
            set
            {
                this.scanDataField = value;
            }
        }

        /// <remarks/>
        public string taxCertificate
        {
            get
            {
                return this.taxCertificateField;
            }
            set
            {
                this.taxCertificateField = value;
            }
        }

        /// <remarks/>
        public string taxregistration
        {
            get
            {
                return this.taxregistrationField;
            }
            set
            {
                this.taxregistrationField = value;
            }
        }

        /// <remarks/>
        public string taxExemptReason
        {
            get
            {
                return this.taxExemptReasonField;
            }
            set
            {
                this.taxExemptReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isVoid
        {
            get
            {
                return this.isVoidField;
            }
            set
            {
                this.isVoidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isVoidSpecified
        {
            get
            {
                return this.isVoidFieldSpecified;
            }
            set
            {
                this.isVoidFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isAuthenticatedOffline
        {
            get
            {
                return this.isAuthenticatedOfflineField;
            }
            set
            {
                this.isAuthenticatedOfflineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isAuthenticatedOfflineSpecified
        {
            get
            {
                return this.isAuthenticatedOfflineFieldSpecified;
            }
            set
            {
                this.isAuthenticatedOfflineFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/Party/V2")]
    [System.Xml.Serialization.XmlRootAttribute("parties", Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/Party/V2", IsNullable = false)]
    public partial class PartiesType
    {

        private PartyType[] partyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("party")]
        public PartyType[] party
        {
            get
            {
                return this.partyField;
            }
            set
            {
                this.partyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyStatus/V1")]
    [System.Xml.Serialization.XmlRootAttribute("partyStatus", Namespace = "http://schemas.wfm.com/Enterprise/PartyMgmt/PartyStatus/V1", IsNullable = false)]
    public partial class PartyStatusType
    {

        private string partyStatusCodeField;

        private PartyTypeDescType partyStatusDescField;

        /// <remarks/>
        public string partyStatusCode
        {
            get
            {
                return this.partyStatusCodeField;
            }
            set
            {
                this.partyStatusCodeField = value;
            }
        }

        /// <remarks/>
        public PartyTypeDescType partyStatusDesc
        {
            get
            {
                return this.partyStatusDescField;
            }
            set
            {
                this.partyStatusDescField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/Party/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionParty", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/Party/V1", IsNullable = false)]
    public partial class TransactionPartyType
    {

        private TransactionAttributes transactionAttributesField;

        private PartyType partyField;

        /// <remarks/>
        public TransactionAttributes transactionAttributes
        {
            get
            {
                return this.transactionAttributesField;
            }
            set
            {
                this.transactionAttributesField = value;
            }
        }

        /// <remarks/>
        public PartyType party
        {
            get
            {
                return this.partyField;
            }
            set
            {
                this.partyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/Party/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionParties", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/Party/V1", IsNullable = false)]
    public partial class TransactionPartiesType
    {

        private TransactionPartyType[] transactionPartyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("transactionParty")]
        public TransactionPartyType[] transactionParty
        {
            get
            {
                return this.transactionPartyField;
            }
            set
            {
                this.transactionPartyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/Persona/V2")]
    [System.Xml.Serialization.XmlRootAttribute("personas", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/Persona/V2", IsNullable = false)]
    public partial class PersonasType
    {

        private PersonaType[] personaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("persona")]
        public PersonaType[] persona
        {
            get
            {
                return this.personaField;
            }
            set
            {
                this.personaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaTrait/V1")]
    [System.Xml.Serialization.XmlRootAttribute("PersonaTraits", Namespace = "http://schemas.wfm.com/Enterprise/PersonaMgmt/PersonaTrait/V1", IsNullable = false)]
    public partial class PersonaTraitsType
    {

        private TraitType[] traitField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("trait")]
        public TraitType[] trait
        {
            get
            {
                return this.traitField;
            }
            set
            {
                this.traitField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2")]
    [System.Xml.Serialization.XmlRootAttribute("prices", Namespace = "http://schemas.wfm.com/Enterprise/PriceMgmt/Price/V2", IsNullable = false)]
    public partial class PricesType
    {

        private PriceType[] priceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("price")]
        public PriceType[] price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PromoMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PromoMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TransactionPromotionAttrType : PromotionSummaryType
    {

        private RefundablePromotionsType refundablePromotionsField;

        /// <remarks/>
        public RefundablePromotionsType refundablePromotions
        {
            get
            {
                return this.refundablePromotionsField;
            }
            set
            {
                this.refundablePromotionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute("RefundablePromotions", Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = false)]
    public partial class RefundablePromotionsType
    {

        private byte[] refundPromotionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] RefundPromotions
        {
            get
            {
                return this.refundPromotionsField;
            }
            set
            {
                this.refundPromotionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TransactionPromotionAttrType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class PromotionSummaryType
    {

        private ulong sequenceNumberField;

        private bool sequenceNumberFieldSpecified;

        private string promotionIDField;

        private DescriptionCommonData promotionDescriptionField;

        private QuantityType redemptionQuantityField;

        private AmountType totalRewardAmountField;

        private string triggerTypeField;

        private AffinityType[] affinityAccountField;

        private RedeemedCouponType[] redeemedCouponsField;

        private IssueCouponType[] issuedCouponsField;

        private string tenderIdField;

        private PromotionSummaryTypeMarkdownApportionment markdownApportionmentField;

        private string messageField;

        private string rewardTypeField;

        private decimal qualifyingSpentField;

        private bool qualifyingSpentFieldSpecified;

        private bool isCustomerRegistrationRequiredField;

        private bool isCustomerRegistrationRequiredFieldSpecified;

        private PromotionTriggerTiming triggerTimingField;

        private bool triggerTimingFieldSpecified;

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sequenceNumberSpecified
        {
            get
            {
                return this.sequenceNumberFieldSpecified;
            }
            set
            {
                this.sequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string promotionID
        {
            get
            {
                return this.promotionIDField;
            }
            set
            {
                this.promotionIDField = value;
            }
        }

        /// <remarks/>
        public DescriptionCommonData promotionDescription
        {
            get
            {
                return this.promotionDescriptionField;
            }
            set
            {
                this.promotionDescriptionField = value;
            }
        }

        /// <remarks/>
        public QuantityType redemptionQuantity
        {
            get
            {
                return this.redemptionQuantityField;
            }
            set
            {
                this.redemptionQuantityField = value;
            }
        }

        /// <remarks/>
        public AmountType totalRewardAmount
        {
            get
            {
                return this.totalRewardAmountField;
            }
            set
            {
                this.totalRewardAmountField = value;
            }
        }

        /// <remarks/>
        public string triggerType
        {
            get
            {
                return this.triggerTypeField;
            }
            set
            {
                this.triggerTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("affinityAccount")]
        public AffinityType[] affinityAccount
        {
            get
            {
                return this.affinityAccountField;
            }
            set
            {
                this.affinityAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("redeemedCoupon", IsNullable = false)]
        public RedeemedCouponType[] redeemedCoupons
        {
            get
            {
                return this.redeemedCouponsField;
            }
            set
            {
                this.redeemedCouponsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("coupon", IsNullable = false)]
        public IssueCouponType[] issuedCoupons
        {
            get
            {
                return this.issuedCouponsField;
            }
            set
            {
                this.issuedCouponsField = value;
            }
        }

        /// <remarks/>
        public string tenderId
        {
            get
            {
                return this.tenderIdField;
            }
            set
            {
                this.tenderIdField = value;
            }
        }

        /// <remarks/>
        public PromotionSummaryTypeMarkdownApportionment markdownApportionment
        {
            get
            {
                return this.markdownApportionmentField;
            }
            set
            {
                this.markdownApportionmentField = value;
            }
        }

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public string rewardType
        {
            get
            {
                return this.rewardTypeField;
            }
            set
            {
                this.rewardTypeField = value;
            }
        }

        /// <remarks/>
        public decimal qualifyingSpent
        {
            get
            {
                return this.qualifyingSpentField;
            }
            set
            {
                this.qualifyingSpentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool qualifyingSpentSpecified
        {
            get
            {
                return this.qualifyingSpentFieldSpecified;
            }
            set
            {
                this.qualifyingSpentFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool IsCustomerRegistrationRequired
        {
            get
            {
                return this.isCustomerRegistrationRequiredField;
            }
            set
            {
                this.isCustomerRegistrationRequiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IsCustomerRegistrationRequiredSpecified
        {
            get
            {
                return this.isCustomerRegistrationRequiredFieldSpecified;
            }
            set
            {
                this.isCustomerRegistrationRequiredFieldSpecified = value;
            }
        }

        /// <remarks/>
        public PromotionTriggerTiming triggerTiming
        {
            get
            {
                return this.triggerTimingField;
            }
            set
            {
                this.triggerTimingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool triggerTimingSpecified
        {
            get
            {
                return this.triggerTimingFieldSpecified;
            }
            set
            {
                this.triggerTimingFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class RedeemedCouponType
    {

        private QuantityType quantityField;

        private ScanDataBase scanCodeField;

        private string seriesIdField;

        private string couponTypeField;

        private string offerIdField;

        private string identifierField;

        /// <remarks/>
        public QuantityType quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public ScanDataBase scanCode
        {
            get
            {
                return this.scanCodeField;
            }
            set
            {
                this.scanCodeField = value;
            }
        }

        /// <remarks/>
        public string seriesId
        {
            get
            {
                return this.seriesIdField;
            }
            set
            {
                this.seriesIdField = value;
            }
        }

        /// <remarks/>
        public string couponType
        {
            get
            {
                return this.couponTypeField;
            }
            set
            {
                this.couponTypeField = value;
            }
        }

        /// <remarks/>
        public string offerId
        {
            get
            {
                return this.offerIdField;
            }
            set
            {
                this.offerIdField = value;
            }
        }

        /// <remarks/>
        public string identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ScanDataBase
    {

        private ScanDataTypeCodeEnum typeCodeField;

        private bool typeCodeFieldSpecified;

        private SubTypeTypeCodeEnum subTypeField;

        private bool subTypeFieldSpecified;

        private string scanDataField;

        /// <remarks/>
        public ScanDataTypeCodeEnum typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeCodeSpecified
        {
            get
            {
                return this.typeCodeFieldSpecified;
            }
            set
            {
                this.typeCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public SubTypeTypeCodeEnum subType
        {
            get
            {
                return this.subTypeField;
            }
            set
            {
                this.subTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool subTypeSpecified
        {
            get
            {
                return this.subTypeFieldSpecified;
            }
            set
            {
                this.subTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string scanData
        {
            get
            {
                return this.scanDataField;
            }
            set
            {
                this.scanDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum ScanDataTypeCodeEnum
    {

        /// <remarks/>
        Barcode,

        /// <remarks/>
        Coupon,

        /// <remarks/>
        MSR,

        /// <remarks/>
        RFID,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum SubTypeTypeCodeEnum
    {

        /// <remarks/>
        EmbeddedPrice,

        /// <remarks/>
        EmbeddedAmount,

        /// <remarks/>
        OverrideAmountEmbedded,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = true)]
    public partial class IssueCouponType : TenderCouponBase
    {

        private string customerIdField;

        private string identifierField;

        private string seriesIdField;

        private string offerIdField;

        private decimal rewardValueField;

        private bool rewardValueFieldSpecified;

        private System.DateTime startDateField;

        private bool startDateFieldSpecified;

        private System.DateTime expiryDateField;

        private bool expiryDateFieldSpecified;

        /// <remarks/>
        public string customerId
        {
            get
            {
                return this.customerIdField;
            }
            set
            {
                this.customerIdField = value;
            }
        }

        /// <remarks/>
        public string identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        /// <remarks/>
        public string seriesId
        {
            get
            {
                return this.seriesIdField;
            }
            set
            {
                this.seriesIdField = value;
            }
        }

        /// <remarks/>
        public string offerId
        {
            get
            {
                return this.offerIdField;
            }
            set
            {
                this.offerIdField = value;
            }
        }

        /// <remarks/>
        public decimal rewardValue
        {
            get
            {
                return this.rewardValueField;
            }
            set
            {
                this.rewardValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool rewardValueSpecified
        {
            get
            {
                return this.rewardValueFieldSpecified;
            }
            set
            {
                this.rewardValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startDateSpecified
        {
            get
            {
                return this.startDateFieldSpecified;
            }
            set
            {
                this.startDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime expiryDate
        {
            get
            {
                return this.expiryDateField;
            }
            set
            {
                this.expiryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expiryDateSpecified
        {
            get
            {
                return this.expiryDateFieldSpecified;
            }
            set
            {
                this.expiryDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IssueCouponType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderCouponBase
    {

        private string nameField;

        private DescriptionCommonData descriptionField;

        private QuantityType quantityField;

        private ManufacturingId manufacturerIDField;

        private FamilyCode familyCodeField;

        private System.DateTime expirationDateField;

        private bool expirationDateFieldSpecified;

        private PromotionCode promotionCodeField;

        private ScanDataBase scanCodeField;

        private DispositionCode dispositionCodeField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public DescriptionCommonData description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public QuantityType quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public ManufacturingId manufacturerID
        {
            get
            {
                return this.manufacturerIDField;
            }
            set
            {
                this.manufacturerIDField = value;
            }
        }

        /// <remarks/>
        public FamilyCode familyCode
        {
            get
            {
                return this.familyCodeField;
            }
            set
            {
                this.familyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expirationDateSpecified
        {
            get
            {
                return this.expirationDateFieldSpecified;
            }
            set
            {
                this.expirationDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public PromotionCode promotionCode
        {
            get
            {
                return this.promotionCodeField;
            }
            set
            {
                this.promotionCodeField = value;
            }
        }

        /// <remarks/>
        public ScanDataBase scanCode
        {
            get
            {
                return this.scanCodeField;
            }
            set
            {
                this.scanCodeField = value;
            }
        }

        /// <remarks/>
        public DispositionCode dispositionCode
        {
            get
            {
                return this.dispositionCodeField;
            }
            set
            {
                this.dispositionCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ManufacturingId
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class FamilyCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class PromotionCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class DispositionCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    public partial class PromotionSummaryTypeMarkdownApportionment
    {

        private AmountType apportionmentAmountField;

        private HierarchyType[] hierarchyField;

        /// <remarks/>
        public AmountType ApportionmentAmount
        {
            get
            {
                return this.apportionmentAmountField;
            }
            set
            {
                this.apportionmentAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("hierarchy", Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1", IsNullable = false)]
        public HierarchyType[] Hierarchy
        {
            get
            {
                return this.hierarchyField;
            }
            set
            {
                this.hierarchyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = false)]
    public enum PromotionTriggerTiming
    {

        /// <remarks/>
        Line,

        /// <remarks/>
        UponTotal,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/PromoMgmt/PromoType/V1")]
    [System.Xml.Serialization.XmlRootAttribute("promoType", Namespace = "http://schemas.wfm.com/Enterprise/PromoMgmt/PromoType/V1", IsNullable = false)]
    public partial class PromoTypeType
    {

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/Transactions/RetailTransaction/LineItemType/Tra" +
        "nsactionPromotion/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionPromotions", Namespace = "http://schemas.wfm.com/Enterprise/Transactions/RetailTransaction/LineItemType/Tra" +
        "nsactionPromotion/V1", IsNullable = false)]
    public partial class TransactionPromosType
    {

        private TransactionPromoType[] promotionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("promotion")]
        public TransactionPromoType[] promotion
        {
            get
            {
                return this.promotionField;
            }
            set
            {
                this.promotionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/Transactions/RetailTransaction/LineItemType/Tra" +
        "nsactionPromotion/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionPromotion", Namespace = "http://schemas.wfm.com/Enterprise/Transactions/RetailTransaction/LineItemType/Tra" +
        "nsactionPromotion/V1", IsNullable = false)]
    public partial class TransactionPromoType
    {

        private TransactionPromotionAttrType transactionAttributesField;

        private string promotionField;

        /// <remarks/>
        public TransactionPromotionAttrType transactionAttributes
        {
            get
            {
                return this.transactionAttributesField;
            }
            set
            {
                this.transactionAttributesField = value;
            }
        }

        /// <remarks/>
        public string promotion
        {
            get
            {
                return this.promotionField;
            }
            set
            {
                this.promotionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/StatusHistory/V1")]
    [System.Xml.Serialization.XmlRootAttribute("statusHistories", Namespace = "http://schemas.wfm.com/Enterprise/StatusMgmt/StatusHistory/V1", IsNullable = false)]
    public partial class StatusHistoriesType
    {

        private StatusHistoryType[] statusHistoryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("statusHistory")]
        public StatusHistoryType[] statusHistory
        {
            get
            {
                return this.statusHistoryField;
            }
            set
            {
                this.statusHistoryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum TaxRateTypeEnum
    {

        /// <remarks/>
        Tax,

        /// <remarks/>
        Fee,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum TaxCalculationMethodTypeEnum
    {

        /// <remarks/>
        Fixed,

        /// <remarks/>
        Percentage,

        /// <remarks/>
        Tiered,

        /// <remarks/>
        TopTiered,

        /// <remarks/>
        Volume,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1", IsNullable = false)]
    public enum RoundingTypeEnum
    {

        /// <remarks/>
        C_FL,

        /// <remarks/>
        C_ID,

        /// <remarks/>
        C_PA,

        /// <remarks/>
        Down,

        /// <remarks/>
        Nearest,

        /// <remarks/>
        None,

        /// <remarks/>
        Up,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TaxBaseType
    {

        private decimal taxRateField;

        private decimal minBaseField;

        private bool minBaseFieldSpecified;

        private decimal maxBaseField;

        private bool maxBaseFieldSpecified;

        private UomType uomField;

        /// <remarks/>
        public decimal taxRate
        {
            get
            {
                return this.taxRateField;
            }
            set
            {
                this.taxRateField = value;
            }
        }

        /// <remarks/>
        public decimal minBase
        {
            get
            {
                return this.minBaseField;
            }
            set
            {
                this.minBaseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minBaseSpecified
        {
            get
            {
                return this.minBaseFieldSpecified;
            }
            set
            {
                this.minBaseFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal maxBase
        {
            get
            {
                return this.maxBaseField;
            }
            set
            {
                this.maxBaseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxBaseSpecified
        {
            get
            {
                return this.maxBaseFieldSpecified;
            }
            set
            {
                this.maxBaseFieldSpecified = value;
            }
        }

        /// <remarks/>
        public UomType uom
        {
            get
            {
                return this.uomField;
            }
            set
            {
                this.uomField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxAuthority/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxAuthority/V1", IsNullable = false)]
    public partial class taxAuthorities
    {

        private TaxAuthorityType[] taxAuthorityField;

        private int cchVersionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxAuthority")]
        public TaxAuthorityType[] taxAuthority
        {
            get
            {
                return this.taxAuthorityField;
            }
            set
            {
                this.taxAuthorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
        public int cchVersion
        {
            get
            {
                return this.cchVersionField;
            }
            set
            {
                this.cchVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxHierarchyClass/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxHierarchyClass/V1", IsNullable = false)]
    public partial class taxHierarchyClasses
    {

        private HierarchyClassType[] taxHierarchyClassField;

        private int cchVersionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxHierarchyClass")]
        public HierarchyClassType[] taxHierarchyClass
        {
            get
            {
                return this.taxHierarchyClassField;
            }
            set
            {
                this.taxHierarchyClassField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
        public int cchVersion
        {
            get
            {
                return this.cchVersionField;
            }
            set
            {
                this.cchVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxRate/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxRate/V1", IsNullable = false)]
    public partial class taxRates
    {

        private TaxRateType[] taxRateField;

        private int cchVersionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxRate")]
        public TaxRateType[] taxRate
        {
            get
            {
                return this.taxRateField;
            }
            set
            {
                this.taxRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
        public int cchVersion
        {
            get
            {
                return this.cchVersionField;
            }
            set
            {
                this.cchVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxRate/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxRate/V1", IsNullable = true)]
    public partial class TaxRateType
    {

        private bool isReversedField;

        private bool isReversedFieldSpecified;

        private string idField;

        private TaxRateTypeEnum taxRateTypeField;

        private TaxAuthorityType taxAuthorityField;

        private string taxTypeField;

        private string taxCatField;

        private string taxDescriptionField;

        private bool isIncludedField;

        private TaxCalculationMethodTypeEnum taxCalculationMethodField;

        private TaxBaseType[] taxBaseField;

        private HierarchyClassType taxClassHierarchyField;

        private System.DateTime startDateField;

        private bool startDateFieldSpecified;

        private System.DateTime endDateField;

        private bool endDateFieldSpecified;

        private TransactionTypeEnum transactionTypeField;

        private TaxZoneType taxZoneField;

        private string taxIndicatorField;

        private string[] dependenceRateIDField;

        private string unitTypeField;

        private RoundingTypeEnum roundingTypeField;

        private bool couponReducesTaxationField;

        private bool couponReducesTaxationFieldSpecified;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public bool isReversed
        {
            get
            {
                return this.isReversedField;
            }
            set
            {
                this.isReversedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isReversedSpecified
        {
            get
            {
                return this.isReversedFieldSpecified;
            }
            set
            {
                this.isReversedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public TaxRateTypeEnum taxRateType
        {
            get
            {
                return this.taxRateTypeField;
            }
            set
            {
                this.taxRateTypeField = value;
            }
        }

        /// <remarks/>
        public TaxAuthorityType taxAuthority
        {
            get
            {
                return this.taxAuthorityField;
            }
            set
            {
                this.taxAuthorityField = value;
            }
        }

        /// <remarks/>
        public string taxType
        {
            get
            {
                return this.taxTypeField;
            }
            set
            {
                this.taxTypeField = value;
            }
        }

        /// <remarks/>
        public string taxCat
        {
            get
            {
                return this.taxCatField;
            }
            set
            {
                this.taxCatField = value;
            }
        }

        /// <remarks/>
        public string taxDescription
        {
            get
            {
                return this.taxDescriptionField;
            }
            set
            {
                this.taxDescriptionField = value;
            }
        }

        /// <remarks/>
        public bool isIncluded
        {
            get
            {
                return this.isIncludedField;
            }
            set
            {
                this.isIncludedField = value;
            }
        }

        /// <remarks/>
        public TaxCalculationMethodTypeEnum taxCalculationMethod
        {
            get
            {
                return this.taxCalculationMethodField;
            }
            set
            {
                this.taxCalculationMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxBase")]
        public TaxBaseType[] taxBase
        {
            get
            {
                return this.taxBaseField;
            }
            set
            {
                this.taxBaseField = value;
            }
        }

        /// <remarks/>
        public HierarchyClassType taxClassHierarchy
        {
            get
            {
                return this.taxClassHierarchyField;
            }
            set
            {
                this.taxClassHierarchyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startDateSpecified
        {
            get
            {
                return this.startDateFieldSpecified;
            }
            set
            {
                this.startDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endDateSpecified
        {
            get
            {
                return this.endDateFieldSpecified;
            }
            set
            {
                this.endDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TransactionTypeEnum transactionType
        {
            get
            {
                return this.transactionTypeField;
            }
            set
            {
                this.transactionTypeField = value;
            }
        }

        /// <remarks/>
        public TaxZoneType taxZone
        {
            get
            {
                return this.taxZoneField;
            }
            set
            {
                this.taxZoneField = value;
            }
        }

        /// <remarks/>
        public string taxIndicator
        {
            get
            {
                return this.taxIndicatorField;
            }
            set
            {
                this.taxIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dependenceRateID")]
        public string[] dependenceRateID
        {
            get
            {
                return this.dependenceRateIDField;
            }
            set
            {
                this.dependenceRateIDField = value;
            }
        }

        /// <remarks/>
        public string unitType
        {
            get
            {
                return this.unitTypeField;
            }
            set
            {
                this.unitTypeField = value;
            }
        }

        /// <remarks/>
        public RoundingTypeEnum roundingType
        {
            get
            {
                return this.roundingTypeField;
            }
            set
            {
                this.roundingTypeField = value;
            }
        }

        /// <remarks/>
        public bool couponReducesTaxation
        {
            get
            {
                return this.couponReducesTaxationField;
            }
            set
            {
                this.couponReducesTaxationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool couponReducesTaxationSpecified
        {
            get
            {
                return this.couponReducesTaxationFieldSpecified;
            }
            set
            {
                this.couponReducesTaxationFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxZone/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxZone/V1", IsNullable = true)]
    public partial class TaxZoneType
    {

        private string idField;

        private LocaleType localeField;

        private ActionEnum actionField;

        private bool actionFieldSpecified;

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public LocaleType locale
        {
            get
            {
                return this.localeField;
            }
            set
            {
                this.localeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
        public ActionEnum Action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ActionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxZone/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TaxZone/V1", IsNullable = false)]
    public partial class taxZones
    {

        private TaxZoneType[] taxZoneField;

        private int cchVersionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxZone")]
        public TaxZoneType[] taxZone
        {
            get
            {
                return this.taxZoneField;
            }
            set
            {
                this.taxZoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/CommonRefTypes/V1")]
        public int cchVersion
        {
            get
            {
                return this.cchVersionField;
            }
            set
            {
                this.cchVersionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TransactionTax/V1")]
    [System.Xml.Serialization.XmlRootAttribute("transactionTaxes", Namespace = "http://schemas.wfm.com/Legal/Retail/TaxMgmt/TransactionTax/V1", IsNullable = false)]
    public partial class TransactionTaxesType
    {

        private TransactionTaxType[] taxField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("tax")]
        public TransactionTaxType[] tax
        {
            get
            {
                return this.taxField;
            }
            set
            {
                this.taxField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum AccountabilityTypeCodeEnum
    {

        /// <remarks/>
        Operator,

        /// <remarks/>
        Register,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum BusinessActionTypeEnum
    {

        /// <remarks/>
        AgeConfirmation,

        /// <remarks/>
        ApprovalRequired,

        /// <remarks/>
        ConfirmationRequired,

        /// <remarks/>
        Notification,

        /// <remarks/>
        Prohibit,

        /// <remarks/>
        ReasonCodeRequired,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum CheckTypeCodeEnum
    {

        /// <remarks/>
        Personal,

        /// <remarks/>
        Company,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum CreditDebitCardTypeEnum
    {

        /// <remarks/>
        Credit,

        /// <remarks/>
        Debit,

        /// <remarks/>
        Affinity,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum CreditDebitPaymentMethodCodeEnum
    {

        /// <remarks/>
        Once,

        /// <remarks/>
        Revolving,

        /// <remarks/>
        Bonus,

        /// <remarks/>
        Divided,

        /// <remarks/>
        DividedWithBonus,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum CryptogramTypeCodeEnum
    {

        /// <remarks/>
        TransactionCertificate,

        /// <remarks/>
        AuthorizationRequestCryptogram,

        /// <remarks/>
        ApplicationAuthenticationCryptogram,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum EntryModeEnum
    {

        /// <remarks/>
        Normal,

        /// <remarks/>
        Manager,

        /// <remarks/>
        Maintenance,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum PasswordChangeStatusEnum
    {

        /// <remarks/>
        Cancelled,

        /// <remarks/>
        FailedNewPassword,

        /// <remarks/>
        FailedOldPassword,

        /// <remarks/>
        Reset,

        /// <remarks/>
        Successful,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum POSLogDeclineTypeEnum
    {

        /// <remarks/>
        DeviceFailure,

        /// <remarks/>
        CustomerRefuses,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum POSLogCouponTypeEnum
    {

        /// <remarks/>
        ManufacturersCoupon,

        /// <remarks/>
        ElectronicCoupon,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum POSLogVoucherUnspentDispositionEnum
    {

        /// <remarks/>
        VoucherChange,

        /// <remarks/>
        CashChange,

        /// <remarks/>
        MixedChange,

        /// <remarks/>
        NoChange,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum RetailTransactionTotalTypeEnum
    {

        /// <remarks/>
        TransactionGrossAmount,

        /// <remarks/>
        TransactionNetAmount,

        /// <remarks/>
        TransactionTaxAmount,

        /// <remarks/>
        TransactionGrandAmount,

        /// <remarks/>
        TransactionTaxExemptAmount,

        /// <remarks/>
        TransactionTaxForgivenAmount,

        /// <remarks/>
        TransactionNonSalesAmount,

        /// <remarks/>
        TransactionPurchaseQuantity,

        /// <remarks/>
        TransactionSubtotal,

        /// <remarks/>
        TransactionTaxFee,

        /// <remarks/>
        TransactionBalanceDueAmount,

        /// <remarks/>
        TransactionFoodstampTotalAmount,

        /// <remarks/>
        TransactionFoodstampBalanceDue,

        /// <remarks/>
        TransactionCouponTotal,

        /// <remarks/>
        TransactionCouponCount,

        /// <remarks/>
        TransactionTotalSavings,

        /// <remarks/>
        TransactionTenderApplied,

        /// <remarks/>
        TransactionTaxIncluded,

        /// <remarks/>
        TransactionTaxSurcharge,

        /// <remarks/>
        ItemTaxFee,

        /// <remarks/>
        TransactionTotalVoidAmount,

        /// <remarks/>
        TransactionTotalReturnAmount,

        /// <remarks/>
        CashbackTotalAmount,

        /// <remarks/>
        TransactionMerchandiseAmount,

        /// <remarks/>
        TransactionNonMerchandiseAmount,

        /// <remarks/>
        NonResettableGrandTotal,

        /// <remarks/>
        PreviousNonResettableGrandTotal,

        /// <remarks/>
        TransactionTotalWICAmount,

        /// <remarks/>
        TransactionTotalWICNTEAmount,

        /// <remarks/>
        TransactionTotalWICNTELostAmount,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum RetailTransactionTotalSubTypeEnum
    {

        /// <remarks/>
        Estimated,

        /// <remarks/>
        Actual,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum ReturnTypeCodeEnum
    {

        /// <remarks/>
        NoneReceiptReturn,

        /// <remarks/>
        GiftReceiptReturn,

        /// <remarks/>
        BottleDepositReturn,

        /// <remarks/>
        ReturnAll,

        /// <remarks/>
        ReceiptReturn,

        /// <remarks/>
        ReceiptedReturn,

        /// <remarks/>
        NoneReceiptedReturn,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum SubTenderTypeCodesEnum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("32")]
        Item32,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("33")]
        Item33,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("35")]
        Item35,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("37")]
        Item37,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("38")]
        Item38,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("40")]
        Item40,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("45")]
        Item45,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("47")]
        Item47,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("59")]
        Item59,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("60")]
        Item60,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("62")]
        Item62,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("66")]
        Item66,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("90")]
        Item90,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("99")]
        Item99,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("121")]
        Item121,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("184")]
        Item184,

        /// <remarks/>
        Amex,

        /// <remarks/>
        DinersClub,

        /// <remarks/>
        DiscoverCard,

        /// <remarks/>
        Fleet,

        /// <remarks/>
        MasterCard,

        /// <remarks/>
        Visa,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TenderAdjustmentTypeEnum
    {

        /// <remarks/>
        Summary,

        /// <remarks/>
        Detail,

        /// <remarks/>
        BankDeposit,

        /// <remarks/>
        BankReceipt,

        /// <remarks/>
        BlindPickup,

        /// <remarks/>
        PaidIn,

        /// <remarks/>
        PaidOut,

        /// <remarks/>
        SafeTransfer,

        /// <remarks/>
        TenderDeclaration,

        /// <remarks/>
        TenderPickup,

        /// <remarks/>
        TenderLoan,

        /// <remarks/>
        TillSettles,

        /// <remarks/>
        SafeSettles,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TenderAuthorizationMethodTypeEnum
    {

        /// <remarks/>
        Automatically,

        /// <remarks/>
        BalanceInquiry,

        /// <remarks/>
        Manually,

        /// <remarks/>
        PreAuthorization,

        /// <remarks/>
        PreAuthorizationCompletion,

        /// <remarks/>
        Systematically,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TenderLedgerTypeCodeEnum
    {

        /// <remarks/>
        Actual,

        /// <remarks/>
        Estimated,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TenderLoanTypeEnum
    {

        /// <remarks/>
        Normal,

        /// <remarks/>
        OpenFloat,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TenderSalesTypeCodeEnum
    {

        /// <remarks/>
        Sale,

        /// <remarks/>
        Refund,

        /// <remarks/>
        Reissue,

        /// <remarks/>
        PreAuthorize,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TenderTypeCodeEnum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18")]
        Item18,

        /// <remarks/>
        AccountsReceivable,

        /// <remarks/>
        Affinity,

        /// <remarks/>
        AirmilesConversion,

        /// <remarks/>
        CapitalBond,

        /// <remarks/>
        Cash,

        /// <remarks/>
        Check,

        /// <remarks/>
        CheckCard,

        /// <remarks/>
        Cheque,

        /// <remarks/>
        CoPay,

        /// <remarks/>
        Coupon,

        /// <remarks/>
        CreditDebit,

        /// <remarks/>
        CustomerAccount,

        /// <remarks/>
        EBTCash,

        /// <remarks/>
        EBTFoodstamps,

        /// <remarks/>
        ElectronicTollCollection,

        /// <remarks/>
        eWic,

        /// <remarks/>
        FoodStamps,

        /// <remarks/>
        GiftCard,

        /// <remarks/>
        GiftCertificate,

        /// <remarks/>
        HouseAccount,

        /// <remarks/>
        InternationalMaestro,

        /// <remarks/>
        ManufacturerCoupon,

        /// <remarks/>
        Mobile,

        /// <remarks/>
        PurchaseOrder,

        /// <remarks/>
        Refund,

        /// <remarks/>
        StaffDressAllowance,

        /// <remarks/>
        StoredValue,

        /// <remarks/>
        TravelersCheck,

        /// <remarks/>
        UKMaestro,

        /// <remarks/>
        Voucher,

        /// <remarks/>
        WICCheck,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TerminalActionCodeTypeCodeEnum
    {

        /// <remarks/>
        Default,

        /// <remarks/>
        Denial,

        /// <remarks/>
        Online,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TotalSalesTypeCodeEnum
    {

        /// <remarks/>
        Sale,

        /// <remarks/>
        Return,

        /// <remarks/>
        PreAuthorize,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TransactionLinkReasonEnum
    {

        /// <remarks/>
        BlindPickup,

        /// <remarks/>
        DeferredBill,

        /// <remarks/>
        FailedSignIn,

        /// <remarks/>
        GiftReceipt,

        /// <remarks/>
        LayAway,

        /// <remarks/>
        PaidOut,

        /// <remarks/>
        ReceiptReprint,

        /// <remarks/>
        SuggestedItem,

        /// <remarks/>
        Reservation,

        /// <remarks/>
        Resume,

        /// <remarks/>
        RetrospectiveLoyalty,

        /// <remarks/>
        Return,

        /// <remarks/>
        UnauthorizedOpen,

        /// <remarks/>
        Voided,

        /// <remarks/>
        WrongData,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TransactionStatusEnum
    {

        /// <remarks/>
        Canceled,

        /// <remarks/>
        Delivered,

        /// <remarks/>
        Failed,

        /// <remarks/>
        Finished,

        /// <remarks/>
        InProcess,

        /// <remarks/>
        PostVoided,

        /// <remarks/>
        Projection,

        /// <remarks/>
        Replaced,

        /// <remarks/>
        Resumed,

        /// <remarks/>
        Returned,

        /// <remarks/>
        Revision,

        /// <remarks/>
        Subtotal,

        /// <remarks/>
        Suspended,

        /// <remarks/>
        SuspendedDeleted,

        /// <remarks/>
        SuspendedRetrieved,

        /// <remarks/>
        Tendered,

        /// <remarks/>
        TransactionVoided,

        /// <remarks/>
        Totaled,

        /// <remarks/>
        Voided,

        /// <remarks/>
        Waste,

        /// <remarks/>
        Unknown,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum TransactionTypeCodeEnum
    {

        /// <remarks/>
        Selfscan,

        /// <remarks/>
        HomeShopping,

        /// <remarks/>
        WIC,

        /// <remarks/>
        DriveOff,

        /// <remarks/>
        Unpaid,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum ServiceTypeCodeEnum
    {

        /// <remarks/>
        TopUp,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefEnumTypes/V1", IsNullable = false)]
    public enum VerifiedByPINStatusTypeCodeEnum
    {

        /// <remarks/>
        Unknown,

        /// <remarks/>
        Verified,

        /// <remarks/>
        NotVerified,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = false)]
    public enum ItemIDTypeCodeEnum
    {

        /// <remarks/>
        GTIN,

        /// <remarks/>
        SGTIN,

        /// <remarks/>
        SSCC,

        /// <remarks/>
        SIN,

        /// <remarks/>
        PLU,

        /// <remarks/>
        SKU,

        /// <remarks/>
        ItemID,

        /// <remarks/>
        ISBN,

        /// <remarks/>
        ISSN,

        /// <remarks/>
        EPC,

        /// <remarks/>
        TUC,

        /// <remarks/>
        POSDepartment,

        /// <remarks/>
        RFID,

        /// <remarks/>
        UPC,

        /// <remarks/>
        MUZEID,

        /// <remarks/>
        AMGID,

        /// <remarks/>
        MenuID,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-A")]
        UPCA,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-AWithSupplementalBarcode")]
        UPCAWithSupplementalBarcode,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-E")]
        UPCE,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-EWithSupplementalBarcode")]
        UPCEWithSupplementalBarcode,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-D1")]
        UPCD1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-D2")]
        UPCD2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-D3")]
        UPCD3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-D4")]
        UPCD4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("UPC-D5")]
        UPCD5,

        /// <remarks/>
        EAN8,

        /// <remarks/>
        JAN8,

        /// <remarks/>
        EAN8WithSupplementalBarcode,

        /// <remarks/>
        EAN13,

        /// <remarks/>
        JAN13,

        /// <remarks/>
        EAN13WithSupplementalBarcode,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("EAN-128")]
        EAN128,

        /// <remarks/>
        Standard2Of5,

        /// <remarks/>
        Interleaved2Of5,

        /// <remarks/>
        Codabar,

        /// <remarks/>
        Code39,

        /// <remarks/>
        Code93,

        /// <remarks/>
        Code128,

        /// <remarks/>
        OCRA,

        /// <remarks/>
        OCRB,

        /// <remarks/>
        PDF417,

        /// <remarks/>
        MAXICODE,

        /// <remarks/>
        OTHER,

        /// <remarks/>
        UNKNOWN,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = false)]
    public enum DescriptionTypeCodeEnumeration
    {

        /// <remarks/>
        Long,

        /// <remarks/>
        Short,

        /// <remarks/>
        Web,

        /// <remarks/>
        Supplier,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("L-Romance")]
        LRomance,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("S-Romance")]
        SRomance,

        /// <remarks/>
        LongRomance,

        /// <remarks/>
        ShortRomance,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class POSItemIDType
    {

        private int posItemIdField;

        private string posQualifierField;

        private string posIdTypeField;

        /// <remarks/>
        public int posItemId
        {
            get
            {
                return this.posItemIdField;
            }
            set
            {
                this.posItemIdField = value;
            }
        }

        /// <remarks/>
        public string posQualifier
        {
            get
            {
                return this.posQualifierField;
            }
            set
            {
                this.posQualifierField = value;
            }
        }

        /// <remarks/>
        public string posIdType
        {
            get
            {
                return this.posIdTypeField;
            }
            set
            {
                this.posIdTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class MerchandiseyHeirarchyType : HierarchiesType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(MerchandiseyHeirarchyType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/RetailMgmt/CommonRefTypes/V1")]
    public partial class HierarchiesType
    {

        private HierarchyType[] hierarchyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("hierarchy")]
        public HierarchyType[] hierarchy
        {
            get
            {
                return this.hierarchyField;
            }
            set
            {
                this.hierarchyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class UnitPriceCommonDataType
    {

        private decimal quanityField;

        private bool quanityFieldSpecified;

        private UomType unitofmeasureCodeCommonDataField;

        private AmountType amountField;

        private string typeCodeField;

        /// <remarks/>
        public decimal quanity
        {
            get
            {
                return this.quanityField;
            }
            set
            {
                this.quanityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quanitySpecified
        {
            get
            {
                return this.quanityFieldSpecified;
            }
            set
            {
                this.quanityFieldSpecified = value;
            }
        }

        /// <remarks/>
        public UomType unitofmeasureCodeCommonData
        {
            get
            {
                return this.unitofmeasureCodeCommonDataField;
            }
            set
            {
                this.unitofmeasureCodeCommonDataField = value;
            }
        }

        /// <remarks/>
        public AmountType Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefItemTypes/V1", IsNullable = true)]
    public partial class ConsumableGroupType
    {

        private string typeField;

        private string idField;

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefPromotionTypes/V1", IsNullable = false)]
    public enum POSLogCouponTypeEnumeration
    {

        /// <remarks/>
        ManufacturersCoupon,

        /// <remarks/>
        ElectronicCoupon,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = false)]
    public enum TaxSummaryEnum
    {

        /// <remarks/>
        ConsumptionTax,

        /// <remarks/>
        ExcludedConsumptionTax,

        /// <remarks/>
        IncludedConsumptionTax,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxCertificateCommonData
    {

        private string numberField;

        private string holderNameField;

        private string formNumberField;

        private string jurisdictionIDField;

        private DateTimezoneType issueDateField;

        private DateTimezoneType expirationDateField;

        private string[] taxGroupIDField;

        private string typeCodeField;

        /// <remarks/>
        public string number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        public string holderName
        {
            get
            {
                return this.holderNameField;
            }
            set
            {
                this.holderNameField = value;
            }
        }

        /// <remarks/>
        public string formNumber
        {
            get
            {
                return this.formNumberField;
            }
            set
            {
                this.formNumberField = value;
            }
        }

        /// <remarks/>
        public string jurisdictionID
        {
            get
            {
                return this.jurisdictionIDField;
            }
            set
            {
                this.jurisdictionIDField = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType issueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taxGroupID")]
        public string[] taxGroupID
        {
            get
            {
                return this.taxGroupIDField;
            }
            set
            {
                this.taxGroupIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class tieredLevelType
    {

        private decimal fromValueField;

        private decimal toValueField;

        private bool toValueFieldSpecified;

        private decimal valueField;

        private string taxIndicatorField;

        private string impositionIdField;

        /// <remarks/>
        public decimal fromValue
        {
            get
            {
                return this.fromValueField;
            }
            set
            {
                this.fromValueField = value;
            }
        }

        /// <remarks/>
        public decimal toValue
        {
            get
            {
                return this.toValueField;
            }
            set
            {
                this.toValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool toValueSpecified
        {
            get
            {
                return this.toValueFieldSpecified;
            }
            set
            {
                this.toValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public string taxIndicator
        {
            get
            {
                return this.taxIndicatorField;
            }
            set
            {
                this.taxIndicatorField = value;
            }
        }

        /// <remarks/>
        public string impositionId
        {
            get
            {
                return this.impositionIdField;
            }
            set
            {
                this.impositionIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxCalculatedMethodBaseType
    {

        private decimal valueField;

        private string impositionIdField;

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public string impositionId
        {
            get
            {
                return this.impositionIdField;
            }
            set
            {
                this.impositionIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxProductSelectConditionType
    {

        private string productGroupIdField;

        /// <remarks/>
        public string ProductGroupId
        {
            get
            {
                return this.productGroupIdField;
            }
            set
            {
                this.productGroupIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxProductCategoryConditionType
    {

        private string categoryIdField;

        private string categoryLabelField;

        /// <remarks/>
        public string categoryId
        {
            get
            {
                return this.categoryIdField;
            }
            set
            {
                this.categoryIdField = value;
            }
        }

        /// <remarks/>
        public string categoryLabel
        {
            get
            {
                return this.categoryLabelField;
            }
            set
            {
                this.categoryLabelField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxDateConditionType
    {

        private DateTimezoneType startDateTimeField;

        private DateTimezoneType endDateTimeField;

        /// <remarks/>
        public DateTimezoneType startDateTime
        {
            get
            {
                return this.startDateTimeField;
            }
            set
            {
                this.startDateTimeField = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType endDateTime
        {
            get
            {
                return this.endDateTimeField;
            }
            set
            {
                this.endDateTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class MinimumTaxableAmountConditionType
    {

        private decimal valueField;

        /// <remarks/>
        public decimal Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class EatInConditionType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxZoneConditionType
    {

        private string taxZoneIdField;

        /// <remarks/>
        public string TaxZoneId
        {
            get
            {
                return this.taxZoneIdField;
            }
            set
            {
                this.taxZoneIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxRuleId
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxGroupID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class TaxJurisdictionID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTaxTypes/V1", IsNullable = true)]
    public partial class DependenceTaxRuleID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTenderTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTenderTypes/V1", IsNullable = true)]
    public partial class TenderForeignCurrencyBase
    {

        private DateTimezoneType dateTimeField;

        private CurrencyTypeCodeEnum currencyCodeField;

        private bool currencyCodeFieldSpecified;

        private AmountType originalFaceAmountField;

        private decimal exchangeRateField;

        private bool exchangeRateFieldSpecified;

        private AmountType serviceChargeField;

        /// <remarks/>
        public DateTimezoneType dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        public CurrencyTypeCodeEnum currencyCode
        {
            get
            {
                return this.currencyCodeField;
            }
            set
            {
                this.currencyCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool currencyCodeSpecified
        {
            get
            {
                return this.currencyCodeFieldSpecified;
            }
            set
            {
                this.currencyCodeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AmountType originalFaceAmount
        {
            get
            {
                return this.originalFaceAmountField;
            }
            set
            {
                this.originalFaceAmountField = value;
            }
        }

        /// <remarks/>
        public decimal exchangeRate
        {
            get
            {
                return this.exchangeRateField;
            }
            set
            {
                this.exchangeRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool exchangeRateSpecified
        {
            get
            {
                return this.exchangeRateFieldSpecified;
            }
            set
            {
                this.exchangeRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AmountType serviceCharge
        {
            get
            {
                return this.serviceChargeField;
            }
            set
            {
                this.serviceChargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class AdjudicationCodeType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class BankReceiptType
    {

        private string bankField;

        private string accountField;

        private string descriptionField;

        private AmountType amountField;

        private ReceiptDetailType[] receiptDetailField;

        /// <remarks/>
        public string Bank
        {
            get
            {
                return this.bankField;
            }
            set
            {
                this.bankField = value;
            }
        }

        /// <remarks/>
        public string Account
        {
            get
            {
                return this.accountField;
            }
            set
            {
                this.accountField = value;
            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public AmountType Amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ReceiptDetail")]
        public ReceiptDetailType[] ReceiptDetail
        {
            get
            {
                return this.receiptDetailField;
            }
            set
            {
                this.receiptDetailField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ReceiptDetailType
    {

        private POSLogTotalsBaseType totalsField;

        /// <remarks/>
        public POSLogTotalsBaseType Totals
        {
            get
            {
                return this.totalsField;
            }
            set
            {
                this.totalsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class POSLogTotalsBaseType
    {

        private TenderTypeCodeEnum tenderTypeField;

        private int subTenderTypeField;

        private int denominationField;

        private AmountType amountField;

        private int countField;

        private bool countFieldSpecified;

        private ReasonCodeType reasonField;

        private TenderForeignCurrencyBaseType foreignCurrencyField;

        private int tenderIDField;

        private bool tenderIDFieldSpecified;

        /// <remarks/>
        public TenderTypeCodeEnum tenderType
        {
            get
            {
                return this.tenderTypeField;
            }
            set
            {
                this.tenderTypeField = value;
            }
        }

        /// <remarks/>
        public int subTenderType
        {
            get
            {
                return this.subTenderTypeField;
            }
            set
            {
                this.subTenderTypeField = value;
            }
        }

        /// <remarks/>
        public int denomination
        {
            get
            {
                return this.denominationField;
            }
            set
            {
                this.denominationField = value;
            }
        }

        /// <remarks/>
        public AmountType amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public int count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool countSpecified
        {
            get
            {
                return this.countFieldSpecified;
            }
            set
            {
                this.countFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }

        /// <remarks/>
        public TenderForeignCurrencyBaseType foreignCurrency
        {
            get
            {
                return this.foreignCurrencyField;
            }
            set
            {
                this.foreignCurrencyField = value;
            }
        }

        /// <remarks/>
        public int tenderID
        {
            get
            {
                return this.tenderIDField;
            }
            set
            {
                this.tenderIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool tenderIDSpecified
        {
            get
            {
                return this.tenderIDFieldSpecified;
            }
            set
            {
                this.tenderIDFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TenderForeignCurrencyBaseType
    {

        private DateTimezoneType dateTimeField;

        private AmountType originalFaceAmountField;

        private decimal exchangeRateField;

        private bool exchangeRateFieldSpecified;

        private AmountType serviceChargeField;

        /// <remarks/>
        public DateTimezoneType dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        public AmountType originalFaceAmount
        {
            get
            {
                return this.originalFaceAmountField;
            }
            set
            {
                this.originalFaceAmountField = value;
            }
        }

        /// <remarks/>
        public decimal exchangeRate
        {
            get
            {
                return this.exchangeRateField;
            }
            set
            {
                this.exchangeRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool exchangeRateSpecified
        {
            get
            {
                return this.exchangeRateFieldSpecified;
            }
            set
            {
                this.exchangeRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AmountType serviceCharge
        {
            get
            {
                return this.serviceChargeField;
            }
            set
            {
                this.serviceChargeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class CouponLogType
    {

        private ulong sequenceNumberField;

        private bool sequenceNumberFieldSpecified;

        private string couponIDField;

        private ScanDataBase scanDataField;

        private int seriesIDField;

        private bool seriesIDFieldSpecified;

        private string offerIdField;

        private AmountType amountField;

        private QuantityType quantityField;

        private string promotionIdField;

        private decimal thresholdField;

        private bool thresholdFieldSpecified;

        private string rewardTypeField;

        private bool fallbackField;

        private bool fallbackFieldSpecified;

        private bool extendedValidationField;

        private bool extendedValidationFieldSpecified;

        private int validationLevelField;

        private bool validationLevelFieldSpecified;

        private bool offlineField;

        private bool offlineFieldSpecified;

        private string groupIdField;

        private string reasonField;

        private DateTimezoneType endDateTimeField;

        private EntryMethodEnum entryMethodField;

        private bool entryMethodFieldSpecified;

        private ReasonCodeType refusalReasonField;

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sequenceNumberSpecified
        {
            get
            {
                return this.sequenceNumberFieldSpecified;
            }
            set
            {
                this.sequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string couponID
        {
            get
            {
                return this.couponIDField;
            }
            set
            {
                this.couponIDField = value;
            }
        }

        /// <remarks/>
        public ScanDataBase scanData
        {
            get
            {
                return this.scanDataField;
            }
            set
            {
                this.scanDataField = value;
            }
        }

        /// <remarks/>
        public int seriesID
        {
            get
            {
                return this.seriesIDField;
            }
            set
            {
                this.seriesIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool seriesIDSpecified
        {
            get
            {
                return this.seriesIDFieldSpecified;
            }
            set
            {
                this.seriesIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string offerId
        {
            get
            {
                return this.offerIdField;
            }
            set
            {
                this.offerIdField = value;
            }
        }

        /// <remarks/>
        public AmountType amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public QuantityType quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public string promotionId
        {
            get
            {
                return this.promotionIdField;
            }
            set
            {
                this.promotionIdField = value;
            }
        }

        /// <remarks/>
        public decimal threshold
        {
            get
            {
                return this.thresholdField;
            }
            set
            {
                this.thresholdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool thresholdSpecified
        {
            get
            {
                return this.thresholdFieldSpecified;
            }
            set
            {
                this.thresholdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string rewardType
        {
            get
            {
                return this.rewardTypeField;
            }
            set
            {
                this.rewardTypeField = value;
            }
        }

        /// <remarks/>
        public bool fallback
        {
            get
            {
                return this.fallbackField;
            }
            set
            {
                this.fallbackField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fallbackSpecified
        {
            get
            {
                return this.fallbackFieldSpecified;
            }
            set
            {
                this.fallbackFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool extendedValidation
        {
            get
            {
                return this.extendedValidationField;
            }
            set
            {
                this.extendedValidationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool extendedValidationSpecified
        {
            get
            {
                return this.extendedValidationFieldSpecified;
            }
            set
            {
                this.extendedValidationFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int validationLevel
        {
            get
            {
                return this.validationLevelField;
            }
            set
            {
                this.validationLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validationLevelSpecified
        {
            get
            {
                return this.validationLevelFieldSpecified;
            }
            set
            {
                this.validationLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool offline
        {
            get
            {
                return this.offlineField;
            }
            set
            {
                this.offlineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool offlineSpecified
        {
            get
            {
                return this.offlineFieldSpecified;
            }
            set
            {
                this.offlineFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string groupId
        {
            get
            {
                return this.groupIdField;
            }
            set
            {
                this.groupIdField = value;
            }
        }

        /// <remarks/>
        public string reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType endDateTime
        {
            get
            {
                return this.endDateTimeField;
            }
            set
            {
                this.endDateTimeField = value;
            }
        }

        /// <remarks/>
        public EntryMethodEnum entryMethod
        {
            get
            {
                return this.entryMethodField;
            }
            set
            {
                this.entryMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entryMethodSpecified
        {
            get
            {
                return this.entryMethodFieldSpecified;
            }
            set
            {
                this.entryMethodFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType refusalReason
        {
            get
            {
                return this.refusalReasonField;
            }
            set
            {
                this.refusalReasonField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class OverShortType
    {

        private POSLogTotalsBaseType[] overField;

        private POSLogTotalsBaseType[] shortField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Over")]
        public POSLogTotalsBaseType[] Over
        {
            get
            {
                return this.overField;
            }
            set
            {
                this.overField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Short")]
        public POSLogTotalsBaseType[] Short
        {
            get
            {
                return this.shortField;
            }
            set
            {
                this.shortField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ReceiptImageDataType
    {

        private string kindField;

        private string formatField;

        private string notPrintedField;

        private string lineField;

        /// <remarks/>
        public string kind
        {
            get
            {
                return this.kindField;
            }
            set
            {
                this.kindField = value;
            }
        }

        /// <remarks/>
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }

        /// <remarks/>
        public string notPrinted
        {
            get
            {
                return this.notPrintedField;
            }
            set
            {
                this.notPrintedField = value;
            }
        }

        /// <remarks/>
        public string line
        {
            get
            {
                return this.lineField;
            }
            set
            {
                this.lineField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class ReversalType
    {

        private ulong sequenceNumberField;

        private bool sequenceNumberFieldSpecified;

        private DateTimezoneType dateTimeField;

        private AdjudicationCodeType adjudicationCodeField;

        private string referenceNumberField;

        private string hostTextField;

        private AmountType reversedAmountField;

        private string reversalReasonField;

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sequenceNumberSpecified
        {
            get
            {
                return this.sequenceNumberFieldSpecified;
            }
            set
            {
                this.sequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType dateTime
        {
            get
            {
                return this.dateTimeField;
            }
            set
            {
                this.dateTimeField = value;
            }
        }

        /// <remarks/>
        public AdjudicationCodeType adjudicationCode
        {
            get
            {
                return this.adjudicationCodeField;
            }
            set
            {
                this.adjudicationCodeField = value;
            }
        }

        /// <remarks/>
        public string referenceNumber
        {
            get
            {
                return this.referenceNumberField;
            }
            set
            {
                this.referenceNumberField = value;
            }
        }

        /// <remarks/>
        public string hostText
        {
            get
            {
                return this.hostTextField;
            }
            set
            {
                this.hostTextField = value;
            }
        }

        /// <remarks/>
        public AmountType reversedAmount
        {
            get
            {
                return this.reversedAmountField;
            }
            set
            {
                this.reversedAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ReversalReason
        {
            get
            {
                return this.reversalReasonField;
            }
            set
            {
                this.reversalReasonField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TaxDefinitionType
    {

        private TaxAuthorityType taxAuthorityField;

        private TaxRateType taxRateField;

        /// <remarks/>
        public TaxAuthorityType taxAuthority
        {
            get
            {
                return this.taxAuthorityField;
            }
            set
            {
                this.taxAuthorityField = value;
            }
        }

        /// <remarks/>
        public TaxRateType taxRate
        {
            get
            {
                return this.taxRateField;
            }
            set
            {
                this.taxRateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TransactionLinkReasonType
    {

        private TransactionLinkReasonEnum valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public TransactionLinkReasonEnum Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TransactionLinkType
    {

        private string transactionIdField;

        private TransactionLinkReasonType reasonField;

        private LocaleType localeField;

        private System.DateTime businessDayDateStartField;

        private bool businessDayDateStartFieldSpecified;

        private System.DateTime businessDayDateEndField;

        private bool businessDayDateEndFieldSpecified;

        private DateTimezoneType beginDateTimeField;

        private DateTimezoneType endDateTimeField;

        private ulong lineItemSequenceNumberField;

        private bool lineItemSequenceNumberFieldSpecified;

        private ulong sequenceNumberField;

        private bool sequenceNumberFieldSpecified;

        /// <remarks/>
        public string transactionId
        {
            get
            {
                return this.transactionIdField;
            }
            set
            {
                this.transactionIdField = value;
            }
        }

        /// <remarks/>
        public TransactionLinkReasonType reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }

        /// <remarks/>
        public LocaleType locale
        {
            get
            {
                return this.localeField;
            }
            set
            {
                this.localeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime businessDayDateStart
        {
            get
            {
                return this.businessDayDateStartField;
            }
            set
            {
                this.businessDayDateStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool businessDayDateStartSpecified
        {
            get
            {
                return this.businessDayDateStartFieldSpecified;
            }
            set
            {
                this.businessDayDateStartFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime businessDayDateEnd
        {
            get
            {
                return this.businessDayDateEndField;
            }
            set
            {
                this.businessDayDateEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool businessDayDateEndSpecified
        {
            get
            {
                return this.businessDayDateEndFieldSpecified;
            }
            set
            {
                this.businessDayDateEndFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType beginDateTime
        {
            get
            {
                return this.beginDateTimeField;
            }
            set
            {
                this.beginDateTimeField = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType endDateTime
        {
            get
            {
                return this.endDateTimeField;
            }
            set
            {
                this.endDateTimeField = value;
            }
        }

        /// <remarks/>
        public ulong lineItemSequenceNumber
        {
            get
            {
                return this.lineItemSequenceNumberField;
            }
            set
            {
                this.lineItemSequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lineItemSequenceNumberSpecified
        {
            get
            {
                return this.lineItemSequenceNumberFieldSpecified;
            }
            set
            {
                this.lineItemSequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sequenceNumberSpecified
        {
            get
            {
                return this.sequenceNumberFieldSpecified;
            }
            set
            {
                this.sequenceNumberFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/CommonRefTypes/V1", IsNullable = true)]
    public partial class TransactionLineItemLinkType
    {

        private ulong lineItemSequenceNumberField;

        private bool lineItemSequenceNumberFieldSpecified;

        private DateTimezoneType beginDateTimeField;

        private DateTimezoneType endDateTimeField;

        private TransactionLinkType[] transactionLinksField;

        /// <remarks/>
        public ulong lineItemSequenceNumber
        {
            get
            {
                return this.lineItemSequenceNumberField;
            }
            set
            {
                this.lineItemSequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lineItemSequenceNumberSpecified
        {
            get
            {
                return this.lineItemSequenceNumberFieldSpecified;
            }
            set
            {
                this.lineItemSequenceNumberFieldSpecified = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType beginDateTime
        {
            get
            {
                return this.beginDateTimeField;
            }
            set
            {
                this.beginDateTimeField = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType endDateTime
        {
            get
            {
                return this.endDateTimeField;
            }
            set
            {
                this.endDateTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("link", IsNullable = false)]
        public TransactionLinkType[] transactionLinks
        {
            get
            {
                return this.transactionLinksField;
            }
            set
            {
                this.transactionLinksField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1", IsNullable = true)]
    public partial class RetailTransactionType
    {

        private bool isVoidedField;

        private string statusField;

        private LineItemType[] lineItemsField;

        /// <remarks/>
        public bool isVoided
        {
            get
            {
                return this.isVoidedField;
            }
            set
            {
                this.isVoidedField = value;
            }
        }

        /// <remarks/>
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("lineItem", IsNullable = false)]
        public LineItemType[] lineItems
        {
            get
            {
                return this.lineItemsField;
            }
            set
            {
                this.lineItemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1", IsNullable = true)]
    public partial class LineItemType
    {

        private bool isVoidedField;

        private bool isTaxedField;

        private bool isTaxedFieldSpecified;

        private bool isFoodStampField;

        private bool isFoodStampFieldSpecified;

        private int sequenceNumberField;

        private string scanTypeField;

        private int taxTableField;

        private bool taxTableFieldSpecified;

        private string subTeamField;

        private System.DateTime endDateTImeField;

        private LineItemTypeType typeField;

        /// <remarks/>
        public bool isVoided
        {
            get
            {
                return this.isVoidedField;
            }
            set
            {
                this.isVoidedField = value;
            }
        }

        /// <remarks/>
        public bool isTaxed
        {
            get
            {
                return this.isTaxedField;
            }
            set
            {
                this.isTaxedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isTaxedSpecified
        {
            get
            {
                return this.isTaxedFieldSpecified;
            }
            set
            {
                this.isTaxedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool isFoodStamp
        {
            get
            {
                return this.isFoodStampField;
            }
            set
            {
                this.isFoodStampField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isFoodStampSpecified
        {
            get
            {
                return this.isFoodStampFieldSpecified;
            }
            set
            {
                this.isFoodStampFieldSpecified = value;
            }
        }

        /// <remarks/>
        public int sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string scanType
        {
            get
            {
                return this.scanTypeField;
            }
            set
            {
                this.scanTypeField = value;
            }
        }

        /// <remarks/>
        public int taxTable
        {
            get
            {
                return this.taxTableField;
            }
            set
            {
                this.taxTableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool taxTableSpecified
        {
            get
            {
                return this.taxTableFieldSpecified;
            }
            set
            {
                this.taxTableFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string subTeam
        {
            get
            {
                return this.subTeamField;
            }
            set
            {
                this.subTeamField = value;
            }
        }

        /// <remarks/>
        public System.DateTime endDateTIme
        {
            get
            {
                return this.endDateTImeField;
            }
            set
            {
                this.endDateTImeField = value;
            }
        }

        /// <remarks/>
        public LineItemTypeType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1", IsNullable = true)]
    public partial class LineItemTypeType
    {

        private LineItemContentType itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("return", typeof(ReturnType))]
        [System.Xml.Serialization.XmlElementAttribute("sale", typeof(SaleType))]
        public LineItemContentType Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1", IsNullable = true)]
    public partial class ReturnType : LineItemContentType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ReturnType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SaleType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1", IsNullable = true)]
    public partial class LineItemContentType
    {

        private int itemIdField;

        private QuantityType quantityField;

        private PriceType priceField;

        private AmountType extendedDiscountAmountField;

        /// <remarks/>
        public int itemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public QuantityType quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        public PriceType price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
            }
        }

        /// <remarks/>
        public AmountType extendedDiscountAmount
        {
            get
            {
                return this.extendedDiscountAmountField;
            }
            set
            {
                this.extendedDiscountAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/ItemMovement/CommonRefTypes/V1", IsNullable = true)]
    public partial class SaleType : LineItemContentType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://services.wfm.com/Enterprise/TransactionMgmt/ItemMovement/V1")]
    [System.Xml.Serialization.XmlRootAttribute("itemMovementTransaction", Namespace = "http://services.wfm.com/Enterprise/TransactionMgmt/ItemMovement/V1", IsNullable = false)]
    public partial class ItemMovementTransactionType
    {

        private string transactionNumberField;

        private ulong sequenceNumberField;

        private RetailTransactionType retailTransactionField;

        private LocaleType localeField;

        private PartyType partyField;

        /// <remarks/>
        public string transactionNumber
        {
            get
            {
                return this.transactionNumberField;
            }
            set
            {
                this.transactionNumberField = value;
            }
        }

        /// <remarks/>
        public ulong sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public RetailTransactionType retailTransaction
        {
            get
            {
                return this.retailTransactionField;
            }
            set
            {
                this.retailTransactionField = value;
            }
        }

        /// <remarks/>
        public LocaleType locale
        {
            get
            {
                return this.localeField;
            }
            set
            {
                this.localeField = value;
            }
        }

        /// <remarks/>
        public PartyType party
        {
            get
            {
                return this.partyField;
            }
            set
            {
                this.partyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class DisposalType
    {

        private string methodField;

        private DisposalTypeNewItemId newItemIdField;

        private string epcField;

        private string[] referencesField;

        /// <remarks/>
        public string method
        {
            get
            {
                return this.methodField;
            }
            set
            {
                this.methodField = value;
            }
        }

        /// <remarks/>
        public DisposalTypeNewItemId newItemId
        {
            get
            {
                return this.newItemIdField;
            }
            set
            {
                this.newItemIdField = value;
            }
        }

        /// <remarks/>
        public string epc
        {
            get
            {
                return this.epcField;
            }
            set
            {
                this.epcField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("reference", IsNullable = false)]
        public string[] references
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    public partial class DisposalTypeNewItemId
    {

        private int itemIdField;

        private bool itemIdFieldSpecified;

        private BaseItemType baseItemTypeField;

        /// <remarks/>
        public int itemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool itemIdSpecified
        {
            get
            {
                return this.itemIdFieldSpecified;
            }
            set
            {
                this.itemIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public BaseItemType baseItemType
        {
            get
            {
                return this.baseItemTypeField;
            }
            set
            {
                this.baseItemTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class EwicPaymentProcessType
    {

        private string maskedAccountIdField;

        private string stateCodeField;

        /// <remarks/>
        public string maskedAccountId
        {
            get
            {
                return this.maskedAccountIdField;
            }
            set
            {
                this.maskedAccountIdField = value;
            }
        }

        /// <remarks/>
        public string stateCode
        {
            get
            {
                return this.stateCodeField;
            }
            set
            {
                this.stateCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class HouseAccountType
    {

        private string accountIDField;

        private string accountNameField;

        /// <remarks/>
        public string accountID
        {
            get
            {
                return this.accountIDField;
            }
            set
            {
                this.accountIDField = value;
            }
        }

        /// <remarks/>
        public string accountName
        {
            get
            {
                return this.accountNameField;
            }
            set
            {
                this.accountNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class PaymentLineItemSequenceLinkType
    {

        private bool isPaymentAllowedField;

        private bool isPaymentAllowedFieldSpecified;

        private ulong valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isPaymentAllowed
        {
            get
            {
                return this.isPaymentAllowedField;
            }
            set
            {
                this.isPaymentAllowedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isPaymentAllowedSpecified
        {
            get
            {
                return this.isPaymentAllowedFieldSpecified;
            }
            set
            {
                this.isPaymentAllowedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public ulong Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class WICCheckType
    {

        private System.DateTime firstDayToUseField;

        private bool firstDayToUseFieldSpecified;

        private System.DateTime lastDayToUseField;

        private bool lastDayToUseFieldSpecified;

        private AmountType ammountNotToExceedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime firstDayToUse
        {
            get
            {
                return this.firstDayToUseField;
            }
            set
            {
                this.firstDayToUseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool firstDayToUseSpecified
        {
            get
            {
                return this.firstDayToUseFieldSpecified;
            }
            set
            {
                this.firstDayToUseFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime lastDayToUse
        {
            get
            {
                return this.lastDayToUseField;
            }
            set
            {
                this.lastDayToUseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastDayToUseSpecified
        {
            get
            {
                return this.lastDayToUseFieldSpecified;
            }
            set
            {
                this.lastDayToUseFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AmountType ammountNotToExceed
        {
            get
            {
                return this.ammountNotToExceedField;
            }
            set
            {
                this.ammountNotToExceedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderGiftCardBase
    {

        private EntryMethodEnum entryMethodField;

        private bool entryMethodFieldSpecified;

        private ServiceTypeCodeEnum serviceTypeField;

        private bool serviceTypeFieldSpecified;

        private CardNumber cardNumberField;

        private System.DateTime dateSoldField;

        private bool dateSoldFieldSpecified;

        private System.DateTime dateActivatedField;

        private bool dateActivatedFieldSpecified;

        private System.DateTime expirationDateField;

        private bool expirationDateFieldSpecified;

        private AmountType initialBalanceField;

        private AmountType currentBalanceField;

        private bool isOpenAmountField;

        private bool isOpenAmountFieldSpecified;

        private TenderAuthorizationDomainSpecific authorizationField;

        /// <remarks/>
        public EntryMethodEnum entryMethod
        {
            get
            {
                return this.entryMethodField;
            }
            set
            {
                this.entryMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entryMethodSpecified
        {
            get
            {
                return this.entryMethodFieldSpecified;
            }
            set
            {
                this.entryMethodFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ServiceTypeCodeEnum serviceType
        {
            get
            {
                return this.serviceTypeField;
            }
            set
            {
                this.serviceTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool serviceTypeSpecified
        {
            get
            {
                return this.serviceTypeFieldSpecified;
            }
            set
            {
                this.serviceTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public CardNumber cardNumber
        {
            get
            {
                return this.cardNumberField;
            }
            set
            {
                this.cardNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime dateSold
        {
            get
            {
                return this.dateSoldField;
            }
            set
            {
                this.dateSoldField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateSoldSpecified
        {
            get
            {
                return this.dateSoldFieldSpecified;
            }
            set
            {
                this.dateSoldFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime dateActivated
        {
            get
            {
                return this.dateActivatedField;
            }
            set
            {
                this.dateActivatedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateActivatedSpecified
        {
            get
            {
                return this.dateActivatedFieldSpecified;
            }
            set
            {
                this.dateActivatedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expirationDateSpecified
        {
            get
            {
                return this.expirationDateFieldSpecified;
            }
            set
            {
                this.expirationDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public AmountType initialBalance
        {
            get
            {
                return this.initialBalanceField;
            }
            set
            {
                this.initialBalanceField = value;
            }
        }

        /// <remarks/>
        public AmountType currentBalance
        {
            get
            {
                return this.currentBalanceField;
            }
            set
            {
                this.currentBalanceField = value;
            }
        }

        /// <remarks/>
        public bool isOpenAmount
        {
            get
            {
                return this.isOpenAmountField;
            }
            set
            {
                this.isOpenAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isOpenAmountSpecified
        {
            get
            {
                return this.isOpenAmountFieldSpecified;
            }
            set
            {
                this.isOpenAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TenderAuthorizationDomainSpecific authorization
        {
            get
            {
                return this.authorizationField;
            }
            set
            {
                this.authorizationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CardNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderAuthorizationDomainSpecific : TenderAuthorizationBase
    {

        private bool hostAuthorizedField;

        private bool hostAuthorizedFieldSpecified;

        private bool electronicSignatureField;

        private bool electronicSignatureFieldSpecified;

        private bool forceOnlineField;

        private bool forceOnlineFieldSpecified;

        private TenderAuthorizationMethodTypeEnum authorizationMethod1Field;

        private bool authorizationMethod1FieldSpecified;

        private AmountType requestedChangeAmountField;

        private string terminalSoftwareVersionField;

        private EMVDebugType emvDebugField;

        private string applicationIDField;

        private AuthorizationCode diagnosticCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool hostAuthorized
        {
            get
            {
                return this.hostAuthorizedField;
            }
            set
            {
                this.hostAuthorizedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool hostAuthorizedSpecified
        {
            get
            {
                return this.hostAuthorizedFieldSpecified;
            }
            set
            {
                this.hostAuthorizedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool electronicSignature
        {
            get
            {
                return this.electronicSignatureField;
            }
            set
            {
                this.electronicSignatureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool electronicSignatureSpecified
        {
            get
            {
                return this.electronicSignatureFieldSpecified;
            }
            set
            {
                this.electronicSignatureFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public bool forceOnline
        {
            get
            {
                return this.forceOnlineField;
            }
            set
            {
                this.forceOnlineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool forceOnlineSpecified
        {
            get
            {
                return this.forceOnlineFieldSpecified;
            }
            set
            {
                this.forceOnlineFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("authorizationMethod", Order = 3)]
        public TenderAuthorizationMethodTypeEnum authorizationMethod1
        {
            get
            {
                return this.authorizationMethod1Field;
            }
            set
            {
                this.authorizationMethod1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool authorizationMethod1Specified
        {
            get
            {
                return this.authorizationMethod1FieldSpecified;
            }
            set
            {
                this.authorizationMethod1FieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public AmountType requestedChangeAmount
        {
            get
            {
                return this.requestedChangeAmountField;
            }
            set
            {
                this.requestedChangeAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string terminalSoftwareVersion
        {
            get
            {
                return this.terminalSoftwareVersionField;
            }
            set
            {
                this.terminalSoftwareVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public EMVDebugType emvDebug
        {
            get
            {
                return this.emvDebugField;
            }
            set
            {
                this.emvDebugField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string applicationID
        {
            get
            {
                return this.applicationIDField;
            }
            set
            {
                this.applicationIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public AuthorizationCode diagnosticCode
        {
            get
            {
                return this.diagnosticCodeField;
            }
            set
            {
                this.diagnosticCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class EMVDebugType
    {

        private CryptogramTypeCodeEnum cryptogramTypeCodeField;

        private TerminalActionCodeTypeCodeEnum terminalActionCodeField;

        private ApplicationIdentifier applicationIdentifierField;

        private ApplicationInterchangeProfile applicationInterchangeProfileField;

        private ApplicationTransactionCounter applicationTransactionCounterField;

        private ApplicationUsageControl applicationUsageControlField;

        private ApplicationVersionNumber applicationVersionNumberField;

        private AuthorizationResponseCode authorizationResponseCodeField;

        private CardholderVerificationMethodResults cardholderVerificationMethodResultsField;

        private Cryptogram cryptogramField;

        private CryptogramInformationData cryptogramInformationDataField;

        private IssuerApplicationData issuerApplicationDataField;

        private EntryMethodEnum posEntryModeField;

        private bool posEntryModeFieldSpecified;

        private PosEntryMode terminalCapabilitiesField;

        private TerminalType terminalTypeField;

        private TerminalVerificationResult terminalVerificationResultsField;

        private TranCryptogramType tranCryptogramTypeField;

        private TransactionStatusInformation transactionStatusInformationField;

        private UnpredictableNumber unpredictableNumberField;

        /// <remarks/>
        public CryptogramTypeCodeEnum cryptogramTypeCode
        {
            get
            {
                return this.cryptogramTypeCodeField;
            }
            set
            {
                this.cryptogramTypeCodeField = value;
            }
        }

        /// <remarks/>
        public TerminalActionCodeTypeCodeEnum terminalActionCode
        {
            get
            {
                return this.terminalActionCodeField;
            }
            set
            {
                this.terminalActionCodeField = value;
            }
        }

        /// <remarks/>
        public ApplicationIdentifier applicationIdentifier
        {
            get
            {
                return this.applicationIdentifierField;
            }
            set
            {
                this.applicationIdentifierField = value;
            }
        }

        /// <remarks/>
        public ApplicationInterchangeProfile applicationInterchangeProfile
        {
            get
            {
                return this.applicationInterchangeProfileField;
            }
            set
            {
                this.applicationInterchangeProfileField = value;
            }
        }

        /// <remarks/>
        public ApplicationTransactionCounter applicationTransactionCounter
        {
            get
            {
                return this.applicationTransactionCounterField;
            }
            set
            {
                this.applicationTransactionCounterField = value;
            }
        }

        /// <remarks/>
        public ApplicationUsageControl applicationUsageControl
        {
            get
            {
                return this.applicationUsageControlField;
            }
            set
            {
                this.applicationUsageControlField = value;
            }
        }

        /// <remarks/>
        public ApplicationVersionNumber applicationVersionNumber
        {
            get
            {
                return this.applicationVersionNumberField;
            }
            set
            {
                this.applicationVersionNumberField = value;
            }
        }

        /// <remarks/>
        public AuthorizationResponseCode authorizationResponseCode
        {
            get
            {
                return this.authorizationResponseCodeField;
            }
            set
            {
                this.authorizationResponseCodeField = value;
            }
        }

        /// <remarks/>
        public CardholderVerificationMethodResults cardholderVerificationMethodResults
        {
            get
            {
                return this.cardholderVerificationMethodResultsField;
            }
            set
            {
                this.cardholderVerificationMethodResultsField = value;
            }
        }

        /// <remarks/>
        public Cryptogram cryptogram
        {
            get
            {
                return this.cryptogramField;
            }
            set
            {
                this.cryptogramField = value;
            }
        }

        /// <remarks/>
        public CryptogramInformationData cryptogramInformationData
        {
            get
            {
                return this.cryptogramInformationDataField;
            }
            set
            {
                this.cryptogramInformationDataField = value;
            }
        }

        /// <remarks/>
        public IssuerApplicationData issuerApplicationData
        {
            get
            {
                return this.issuerApplicationDataField;
            }
            set
            {
                this.issuerApplicationDataField = value;
            }
        }

        /// <remarks/>
        public EntryMethodEnum posEntryMode
        {
            get
            {
                return this.posEntryModeField;
            }
            set
            {
                this.posEntryModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool posEntryModeSpecified
        {
            get
            {
                return this.posEntryModeFieldSpecified;
            }
            set
            {
                this.posEntryModeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public PosEntryMode terminalCapabilities
        {
            get
            {
                return this.terminalCapabilitiesField;
            }
            set
            {
                this.terminalCapabilitiesField = value;
            }
        }

        /// <remarks/>
        public TerminalType terminalType
        {
            get
            {
                return this.terminalTypeField;
            }
            set
            {
                this.terminalTypeField = value;
            }
        }

        /// <remarks/>
        public TerminalVerificationResult terminalVerificationResults
        {
            get
            {
                return this.terminalVerificationResultsField;
            }
            set
            {
                this.terminalVerificationResultsField = value;
            }
        }

        /// <remarks/>
        public TranCryptogramType tranCryptogramType
        {
            get
            {
                return this.tranCryptogramTypeField;
            }
            set
            {
                this.tranCryptogramTypeField = value;
            }
        }

        /// <remarks/>
        public TransactionStatusInformation transactionStatusInformation
        {
            get
            {
                return this.transactionStatusInformationField;
            }
            set
            {
                this.transactionStatusInformationField = value;
            }
        }

        /// <remarks/>
        public UnpredictableNumber unpredictableNumber
        {
            get
            {
                return this.unpredictableNumberField;
            }
            set
            {
                this.unpredictableNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ApplicationIdentifier
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ApplicationInterchangeProfile
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ApplicationTransactionCounter
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ApplicationUsageControl
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ApplicationVersionNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class AuthorizationResponseCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CardholderVerificationMethodResults
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class Cryptogram
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CryptogramInformationData
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class IssuerApplicationData
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class PosEntryMode
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public int Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TerminalType
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public int Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TerminalVerificationResult
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TranCryptogramType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TransactionStatusInformation
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class UnpredictableNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class AuthorizationCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TenderAuthorizationDomainSpecific))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderAuthorizationBase
    {

        private bool isPreAuthorizationField;

        private bool isPreAuthorizationFieldSpecified;

        private bool isVerifiedByPINField;

        private bool isVerifiedByPINFieldSpecified;

        private bool isSignatureRequiredField;

        private bool isSignatureRequiredFieldSpecified;

        private bool isCustomerPresentField;

        private bool isCustomerPresentFieldSpecified;

        private bool isElectronicSignatureField;

        private bool isElectronicSignatureFieldSpecified;

        private VerifiedByPINStatusTypeCodeEnum verifiedByPINStatusField;

        private bool verifiedByPINStatusFieldSpecified;

        private TenderAuthorizationMethodTypeEnum authorizationMethodField;

        private bool authorizationMethodFieldSpecified;

        private AmountType requestedAmountField;

        private AuthorizationCode authorizationCodeField;

        private ReferenceNumber referenceNumberField;

        private MerchantNumber merchantNumberField;

        private DateTimezoneType authorizationDateTimeField;

        private AmountType authorizedChangeAmountField;

        private AuthorizingTermID authorizingTermIDField;

        private PreAuthorizedID preAuthorizedIDField;

        private AuthorizationDescription authorizationDescriptionField;

        private ReversalType reversalField;

        private POSLogDeclineTypeEnum electronicSignatureDeclineField;

        private bool electronicSignatureDeclineFieldSpecified;

        private string receiptTextField;

        private Signature signatureField;

        private ProviderID providerIDField;

        private ProviderDataType providerDataField;

        private CardId cardIdField;

        private CardType cardTypeField;

        private AuthorizationType authorizationTypeField;

        private VoiceAuthorizationType voiceAuthorizationField;

        private AuthorizationResponseCode authorizationResponseCodeField;

        private ExtendedData extendedDataField;

        private PrioritizedGroupAmounts prioritizedGroupAmountsField;

        private string tokenDataField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool isPreAuthorization
        {
            get
            {
                return this.isPreAuthorizationField;
            }
            set
            {
                this.isPreAuthorizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isPreAuthorizationSpecified
        {
            get
            {
                return this.isPreAuthorizationFieldSpecified;
            }
            set
            {
                this.isPreAuthorizationFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool isVerifiedByPIN
        {
            get
            {
                return this.isVerifiedByPINField;
            }
            set
            {
                this.isVerifiedByPINField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isVerifiedByPINSpecified
        {
            get
            {
                return this.isVerifiedByPINFieldSpecified;
            }
            set
            {
                this.isVerifiedByPINFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public bool isSignatureRequired
        {
            get
            {
                return this.isSignatureRequiredField;
            }
            set
            {
                this.isSignatureRequiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isSignatureRequiredSpecified
        {
            get
            {
                return this.isSignatureRequiredFieldSpecified;
            }
            set
            {
                this.isSignatureRequiredFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool isCustomerPresent
        {
            get
            {
                return this.isCustomerPresentField;
            }
            set
            {
                this.isCustomerPresentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isCustomerPresentSpecified
        {
            get
            {
                return this.isCustomerPresentFieldSpecified;
            }
            set
            {
                this.isCustomerPresentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool isElectronicSignature
        {
            get
            {
                return this.isElectronicSignatureField;
            }
            set
            {
                this.isElectronicSignatureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isElectronicSignatureSpecified
        {
            get
            {
                return this.isElectronicSignatureFieldSpecified;
            }
            set
            {
                this.isElectronicSignatureFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public VerifiedByPINStatusTypeCodeEnum verifiedByPINStatus
        {
            get
            {
                return this.verifiedByPINStatusField;
            }
            set
            {
                this.verifiedByPINStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool verifiedByPINStatusSpecified
        {
            get
            {
                return this.verifiedByPINStatusFieldSpecified;
            }
            set
            {
                this.verifiedByPINStatusFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public TenderAuthorizationMethodTypeEnum authorizationMethod
        {
            get
            {
                return this.authorizationMethodField;
            }
            set
            {
                this.authorizationMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool authorizationMethodSpecified
        {
            get
            {
                return this.authorizationMethodFieldSpecified;
            }
            set
            {
                this.authorizationMethodFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public AmountType requestedAmount
        {
            get
            {
                return this.requestedAmountField;
            }
            set
            {
                this.requestedAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public AuthorizationCode authorizationCode
        {
            get
            {
                return this.authorizationCodeField;
            }
            set
            {
                this.authorizationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public ReferenceNumber referenceNumber
        {
            get
            {
                return this.referenceNumberField;
            }
            set
            {
                this.referenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public MerchantNumber merchantNumber
        {
            get
            {
                return this.merchantNumberField;
            }
            set
            {
                this.merchantNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public DateTimezoneType authorizationDateTime
        {
            get
            {
                return this.authorizationDateTimeField;
            }
            set
            {
                this.authorizationDateTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public AmountType authorizedChangeAmount
        {
            get
            {
                return this.authorizedChangeAmountField;
            }
            set
            {
                this.authorizedChangeAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public AuthorizingTermID authorizingTermID
        {
            get
            {
                return this.authorizingTermIDField;
            }
            set
            {
                this.authorizingTermIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public PreAuthorizedID preAuthorizedID
        {
            get
            {
                return this.preAuthorizedIDField;
            }
            set
            {
                this.preAuthorizedIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public AuthorizationDescription authorizationDescription
        {
            get
            {
                return this.authorizationDescriptionField;
            }
            set
            {
                this.authorizationDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public ReversalType reversal
        {
            get
            {
                return this.reversalField;
            }
            set
            {
                this.reversalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public POSLogDeclineTypeEnum electronicSignatureDecline
        {
            get
            {
                return this.electronicSignatureDeclineField;
            }
            set
            {
                this.electronicSignatureDeclineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool electronicSignatureDeclineSpecified
        {
            get
            {
                return this.electronicSignatureDeclineFieldSpecified;
            }
            set
            {
                this.electronicSignatureDeclineFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string receiptText
        {
            get
            {
                return this.receiptTextField;
            }
            set
            {
                this.receiptTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public Signature signature
        {
            get
            {
                return this.signatureField;
            }
            set
            {
                this.signatureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public ProviderID providerID
        {
            get
            {
                return this.providerIDField;
            }
            set
            {
                this.providerIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public ProviderDataType providerData
        {
            get
            {
                return this.providerDataField;
            }
            set
            {
                this.providerDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public CardId cardId
        {
            get
            {
                return this.cardIdField;
            }
            set
            {
                this.cardIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public CardType cardType
        {
            get
            {
                return this.cardTypeField;
            }
            set
            {
                this.cardTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public AuthorizationType authorizationType
        {
            get
            {
                return this.authorizationTypeField;
            }
            set
            {
                this.authorizationTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public VoiceAuthorizationType voiceAuthorization
        {
            get
            {
                return this.voiceAuthorizationField;
            }
            set
            {
                this.voiceAuthorizationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public AuthorizationResponseCode authorizationResponseCode
        {
            get
            {
                return this.authorizationResponseCodeField;
            }
            set
            {
                this.authorizationResponseCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public ExtendedData extendedData
        {
            get
            {
                return this.extendedDataField;
            }
            set
            {
                this.extendedDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public PrioritizedGroupAmounts prioritizedGroupAmounts
        {
            get
            {
                return this.prioritizedGroupAmountsField;
            }
            set
            {
                this.prioritizedGroupAmountsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 29)]
        public string tokenData
        {
            get
            {
                return this.tokenDataField;
            }
            set
            {
                this.tokenDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ReferenceNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class MerchantNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class AuthorizingTermID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class PreAuthorizedID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class AuthorizationDescription
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class Signature
    {

        private byte[] valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "base64Binary")]
        public byte[] Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ProviderID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ProviderDataType
    {

        private DateTimezoneType startDateTimeField;

        private string epsResponseCodeField;

        private ReasonCodeType errorReasonField;

        private string dataField;

        /// <remarks/>
        public DateTimezoneType startDateTime
        {
            get
            {
                return this.startDateTimeField;
            }
            set
            {
                this.startDateTimeField = value;
            }
        }

        /// <remarks/>
        public string epsResponseCode
        {
            get
            {
                return this.epsResponseCodeField;
            }
            set
            {
                this.epsResponseCodeField = value;
            }
        }

        /// <remarks/>
        public ReasonCodeType errorReason
        {
            get
            {
                return this.errorReasonField;
            }
            set
            {
                this.errorReasonField = value;
            }
        }

        /// <remarks/>
        public string data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CardId
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CardType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class AuthorizationType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class VoiceAuthorizationType
    {

        private bool approvedField;

        private bool approvedFieldSpecified;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool Approved
        {
            get
            {
                return this.approvedField;
            }
            set
            {
                this.approvedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ApprovedSpecified
        {
            get
            {
                return this.approvedFieldSpecified;
            }
            set
            {
                this.approvedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ExtendedData
    {

        private string dataField;

        private string tkeyField;

        /// <remarks/>
        public string data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }

        /// <remarks/>
        public string tkey
        {
            get
            {
                return this.tkeyField;
            }
            set
            {
                this.tkeyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class PrioritizedGroupAmounts
    {

        private string[] groupField;

        private AmountType[] amountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("group")]
        public string[] group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("amount")]
        public AmountType[] amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderChangeDomainSpecific : TenderChangeBase
    {

        private TenderVoucherBase voucherField;

        private TenderFoodStampBase foodStampField;

        /// <remarks/>
        public TenderVoucherBase voucher
        {
            get
            {
                return this.voucherField;
            }
            set
            {
                this.voucherField = value;
            }
        }

        /// <remarks/>
        public TenderFoodStampBase foodStamp
        {
            get
            {
                return this.foodStampField;
            }
            set
            {
                this.foodStampField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderVoucherBase
    {

        private string typeCodeField;

        private DescriptionCommonData descriptionField;

        private AmountType faceValueAmountField;

        private SerialNumber serialNumberField;

        private DateTimezoneType expirationDateField;

        private TenderVoucherBaseUnspentAmount unspentAmountField;

        private AmountType voucherAmountInChangeField;

        private int issuingStoreNumberIDField;

        private bool issuingStoreNumberIDFieldSpecified;

        /// <remarks/>
        public string typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        public DescriptionCommonData description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public AmountType faceValueAmount
        {
            get
            {
                return this.faceValueAmountField;
            }
            set
            {
                this.faceValueAmountField = value;
            }
        }

        /// <remarks/>
        public SerialNumber serialNumber
        {
            get
            {
                return this.serialNumberField;
            }
            set
            {
                this.serialNumberField = value;
            }
        }

        /// <remarks/>
        public DateTimezoneType expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        public TenderVoucherBaseUnspentAmount unspentAmount
        {
            get
            {
                return this.unspentAmountField;
            }
            set
            {
                this.unspentAmountField = value;
            }
        }

        /// <remarks/>
        public AmountType voucherAmountInChange
        {
            get
            {
                return this.voucherAmountInChangeField;
            }
            set
            {
                this.voucherAmountInChangeField = value;
            }
        }

        /// <remarks/>
        public int issuingStoreNumberID
        {
            get
            {
                return this.issuingStoreNumberIDField;
            }
            set
            {
                this.issuingStoreNumberIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool issuingStoreNumberIDSpecified
        {
            get
            {
                return this.issuingStoreNumberIDFieldSpecified;
            }
            set
            {
                this.issuingStoreNumberIDFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class SerialNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    public partial class TenderVoucherBaseUnspentAmount : AmountType
    {

        private string dispositionField;

        /// <remarks/>
        public string disposition
        {
            get
            {
                return this.dispositionField;
            }
            set
            {
                this.dispositionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderFoodStampBase
    {

        private FederalID federalIDField;

        private SerialNumber serialNumberField;

        private AmountType foodStampsChangeField;

        /// <remarks/>
        public FederalID federalID
        {
            get
            {
                return this.federalIDField;
            }
            set
            {
                this.federalIDField = value;
            }
        }

        /// <remarks/>
        public SerialNumber serialNumber
        {
            get
            {
                return this.serialNumberField;
            }
            set
            {
                this.serialNumberField = value;
            }
        }

        /// <remarks/>
        public AmountType foodStampsChange
        {
            get
            {
                return this.foodStampsChangeField;
            }
            set
            {
                this.foodStampsChangeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class FederalID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TenderChangeDomainSpecific))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderChangeBase
    {

        private TenderTypeCodeEnum tenderTypeField;

        private bool tenderTypeFieldSpecified;

        private TenderId tenderIDField;

        private AmountType amountField;

        private TenderForeignCurrencyBase foreignCurrencyField;

        private TenderGiftCardBase giftCardField;

        /// <remarks/>
        public TenderTypeCodeEnum tenderType
        {
            get
            {
                return this.tenderTypeField;
            }
            set
            {
                this.tenderTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool tenderTypeSpecified
        {
            get
            {
                return this.tenderTypeFieldSpecified;
            }
            set
            {
                this.tenderTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public TenderId tenderID
        {
            get
            {
                return this.tenderIDField;
            }
            set
            {
                this.tenderIDField = value;
            }
        }

        /// <remarks/>
        public AmountType amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public TenderForeignCurrencyBase foreignCurrency
        {
            get
            {
                return this.foreignCurrencyField;
            }
            set
            {
                this.foreignCurrencyField = value;
            }
        }

        /// <remarks/>
        public TenderGiftCardBase giftCard
        {
            get
            {
                return this.giftCardField;
            }
            set
            {
                this.giftCardField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderCheckBase
    {

        private TenderTypeCodeEnum typeCodeField;

        private BankID bankIDField;

        private CheckNumber checkNumberField;

        private AccountNumber accountNumberField;

        private CheckCardNumbe checkCardNumberField;

        private FullMicr fullMICRField;

        private Count countField;

        /// <remarks/>
        public TenderTypeCodeEnum typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        public BankID bankID
        {
            get
            {
                return this.bankIDField;
            }
            set
            {
                this.bankIDField = value;
            }
        }

        /// <remarks/>
        public CheckNumber checkNumber
        {
            get
            {
                return this.checkNumberField;
            }
            set
            {
                this.checkNumberField = value;
            }
        }

        /// <remarks/>
        public AccountNumber accountNumber
        {
            get
            {
                return this.accountNumberField;
            }
            set
            {
                this.accountNumberField = value;
            }
        }

        /// <remarks/>
        public CheckCardNumbe checkCardNumber
        {
            get
            {
                return this.checkCardNumberField;
            }
            set
            {
                this.checkCardNumberField = value;
            }
        }

        /// <remarks/>
        public FullMicr fullMICR
        {
            get
            {
                return this.fullMICRField;
            }
            set
            {
                this.fullMICRField = value;
            }
        }

        /// <remarks/>
        public Count count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class BankID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CheckNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class AccountNumber
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CheckCardNumbe
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class FullMicr
    {

        private string fullMicrField;

        private string countryField;

        /// <remarks/>
        public string fullMicr
        {
            get
            {
                return this.fullMicrField;
            }
            set
            {
                this.fullMicrField = value;
            }
        }

        /// <remarks/>
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class Count
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute(DataType = "negativeInteger")]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderCreditDebitDomainSpecific : TenderCreditDebitBase
    {

        private CreditPaymentMethod paymentMethodField;

        private decimal numberOfInstallmentsField;

        private bool numberOfInstallmentsFieldSpecified;

        private string frequencyOfBonusPerYearField;

        private PurChasedItem[] purchasedItemField;

        private FleetInfoBase fleetInformationField;

        /// <remarks/>
        public CreditPaymentMethod paymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
            }
        }

        /// <remarks/>
        public decimal numberOfInstallments
        {
            get
            {
                return this.numberOfInstallmentsField;
            }
            set
            {
                this.numberOfInstallmentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool numberOfInstallmentsSpecified
        {
            get
            {
                return this.numberOfInstallmentsFieldSpecified;
            }
            set
            {
                this.numberOfInstallmentsFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string frequencyOfBonusPerYear
        {
            get
            {
                return this.frequencyOfBonusPerYearField;
            }
            set
            {
                this.frequencyOfBonusPerYearField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("purchasedItem")]
        public PurChasedItem[] purchasedItem
        {
            get
            {
                return this.purchasedItemField;
            }
            set
            {
                this.purchasedItemField = value;
            }
        }

        /// <remarks/>
        public FleetInfoBase fleetInformation
        {
            get
            {
                return this.fleetInformationField;
            }
            set
            {
                this.fleetInformationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CreditPaymentMethod
    {

        private CreditDebitPaymentMethodCodeEnum paymentMethodCodeField;

        private int dividedField;

        private bool dividedFieldSpecified;

        private Count bonusCountField;

        private AmountType bonusPayAmountField;

        private string[] bonusMonthDayField;

        /// <remarks/>
        public CreditDebitPaymentMethodCodeEnum paymentMethodCode
        {
            get
            {
                return this.paymentMethodCodeField;
            }
            set
            {
                this.paymentMethodCodeField = value;
            }
        }

        /// <remarks/>
        public int divided
        {
            get
            {
                return this.dividedField;
            }
            set
            {
                this.dividedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dividedSpecified
        {
            get
            {
                return this.dividedFieldSpecified;
            }
            set
            {
                this.dividedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Count bonusCount
        {
            get
            {
                return this.bonusCountField;
            }
            set
            {
                this.bonusCountField = value;
            }
        }

        /// <remarks/>
        public AmountType bonusPayAmount
        {
            get
            {
                return this.bonusPayAmountField;
            }
            set
            {
                this.bonusPayAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("bonusMonthDay", DataType = "gMonthDay")]
        public string[] bonusMonthDay
        {
            get
            {
                return this.bonusMonthDayField;
            }
            set
            {
                this.bonusMonthDayField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class PurChasedItem
    {

        private string itemIdField;

        private string epcField;

        /// <remarks/>
        public string ItemId
        {
            get
            {
                return this.itemIdField;
            }
            set
            {
                this.itemIdField = value;
            }
        }

        /// <remarks/>
        public string epc
        {
            get
            {
                return this.epcField;
            }
            set
            {
                this.epcField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class FleetInfoBase
    {

        private DriverlID driverIDField;

        private VehiclelID vehicleIDField;

        private ulong odometerReadingField;

        private bool odometerReadingFieldSpecified;

        private JobID jobIDField;

        private LicensePlate licensePlateField;

        private bool temporaryCarField;

        private bool temporaryCarFieldSpecified;

        /// <remarks/>
        public DriverlID driverID
        {
            get
            {
                return this.driverIDField;
            }
            set
            {
                this.driverIDField = value;
            }
        }

        /// <remarks/>
        public VehiclelID vehicleID
        {
            get
            {
                return this.vehicleIDField;
            }
            set
            {
                this.vehicleIDField = value;
            }
        }

        /// <remarks/>
        public ulong odometerReading
        {
            get
            {
                return this.odometerReadingField;
            }
            set
            {
                this.odometerReadingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool odometerReadingSpecified
        {
            get
            {
                return this.odometerReadingFieldSpecified;
            }
            set
            {
                this.odometerReadingFieldSpecified = value;
            }
        }

        /// <remarks/>
        public JobID jobID
        {
            get
            {
                return this.jobIDField;
            }
            set
            {
                this.jobIDField = value;
            }
        }

        /// <remarks/>
        public LicensePlate licensePlate
        {
            get
            {
                return this.licensePlateField;
            }
            set
            {
                this.licensePlateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool TemporaryCar
        {
            get
            {
                return this.temporaryCarField;
            }
            set
            {
                this.temporaryCarField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TemporaryCarSpecified
        {
            get
            {
                return this.temporaryCarFieldSpecified;
            }
            set
            {
                this.temporaryCarFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class DriverlID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class VehiclelID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class JobID
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class LicensePlate
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TenderCreditDebitDomainSpecific))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderCreditDebitBase
    {

        private CardType cardTypeField;

        private SubTenderTypeCodesEnum typeCodeField;

        private bool isContactlessPaymentField;

        private IssuerIdentificationNumber issuerIdentificationNumberField;

        private CardHolderName cardHolderNameField;

        private AccountNumber primaryAccountNumberField;

        private ulong issueSequenceField;

        private bool issueSequenceFieldSpecified;

        private System.DateTime expirationDateField;

        private bool expirationDateFieldSpecified;

        private CreditDebitCardCompanyCode creditCardCompanyCodeField;

        private EncryptedDataType track1DataField;

        private EncryptedDataType track2DataField;

        private EncryptedDataType track3DataField;

        private EncryptedDataType track4DataField;

        private ReconciliationCode reconciliationCodeField;

        private System.DateTime startDateField;

        private bool startDateFieldSpecified;

        private ServiceCode serviceCodeField;

        private string maskedPANField;

        private string maskedAccountIdField;

        /// <remarks/>
        public CardType cardType
        {
            get
            {
                return this.cardTypeField;
            }
            set
            {
                this.cardTypeField = value;
            }
        }

        /// <remarks/>
        public SubTenderTypeCodesEnum typeCode
        {
            get
            {
                return this.typeCodeField;
            }
            set
            {
                this.typeCodeField = value;
            }
        }

        /// <remarks/>
        public bool isContactlessPayment
        {
            get
            {
                return this.isContactlessPaymentField;
            }
            set
            {
                this.isContactlessPaymentField = value;
            }
        }

        /// <remarks/>
        public IssuerIdentificationNumber issuerIdentificationNumber
        {
            get
            {
                return this.issuerIdentificationNumberField;
            }
            set
            {
                this.issuerIdentificationNumberField = value;
            }
        }

        /// <remarks/>
        public CardHolderName cardHolderName
        {
            get
            {
                return this.cardHolderNameField;
            }
            set
            {
                this.cardHolderNameField = value;
            }
        }

        /// <remarks/>
        public AccountNumber primaryAccountNumber
        {
            get
            {
                return this.primaryAccountNumberField;
            }
            set
            {
                this.primaryAccountNumberField = value;
            }
        }

        /// <remarks/>
        public ulong issueSequence
        {
            get
            {
                return this.issueSequenceField;
            }
            set
            {
                this.issueSequenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool issueSequenceSpecified
        {
            get
            {
                return this.issueSequenceFieldSpecified;
            }
            set
            {
                this.issueSequenceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expirationDateSpecified
        {
            get
            {
                return this.expirationDateFieldSpecified;
            }
            set
            {
                this.expirationDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public CreditDebitCardCompanyCode creditCardCompanyCode
        {
            get
            {
                return this.creditCardCompanyCodeField;
            }
            set
            {
                this.creditCardCompanyCodeField = value;
            }
        }

        /// <remarks/>
        public EncryptedDataType track1Data
        {
            get
            {
                return this.track1DataField;
            }
            set
            {
                this.track1DataField = value;
            }
        }

        /// <remarks/>
        public EncryptedDataType track2Data
        {
            get
            {
                return this.track2DataField;
            }
            set
            {
                this.track2DataField = value;
            }
        }

        /// <remarks/>
        public EncryptedDataType track3Data
        {
            get
            {
                return this.track3DataField;
            }
            set
            {
                this.track3DataField = value;
            }
        }

        /// <remarks/>
        public EncryptedDataType track4Data
        {
            get
            {
                return this.track4DataField;
            }
            set
            {
                this.track4DataField = value;
            }
        }

        /// <remarks/>
        public ReconciliationCode reconciliationCode
        {
            get
            {
                return this.reconciliationCodeField;
            }
            set
            {
                this.reconciliationCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startDateSpecified
        {
            get
            {
                return this.startDateFieldSpecified;
            }
            set
            {
                this.startDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ServiceCode serviceCode
        {
            get
            {
                return this.serviceCodeField;
            }
            set
            {
                this.serviceCodeField = value;
            }
        }

        /// <remarks/>
        public string maskedPAN
        {
            get
            {
                return this.maskedPANField;
            }
            set
            {
                this.maskedPANField = value;
            }
        }

        /// <remarks/>
        public string maskedAccountId
        {
            get
            {
                return this.maskedAccountIdField;
            }
            set
            {
                this.maskedAccountIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class IssuerIdentificationNumber
    {

        private int valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public int Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CardHolderName
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CreditDebitCardCompanyCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ReconciliationCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ServiceCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TenderRestrictionRuleType
    {

        private AmountType thresholdAmountField;

        private ulong[] lineItemLinksField;

        private PaymentLineItemSequenceLinkType[] manualEligibilityItemLinksField;

        /// <remarks/>
        public AmountType thresholdAmount
        {
            get
            {
                return this.thresholdAmountField;
            }
            set
            {
                this.thresholdAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("lineItemSequenceLink", IsNullable = false)]
        public ulong[] lineItemLinks
        {
            get
            {
                return this.lineItemLinksField;
            }
            set
            {
                this.lineItemLinksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("lineItemSequenceLink", IsNullable = false)]
        public PaymentLineItemSequenceLinkType[] manualEligibilityItemLinks
        {
            get
            {
                return this.manualEligibilityItemLinksField;
            }
            set
            {
                this.manualEligibilityItemLinksField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class ApplicationIDwhatisthis
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class DiagnosticCode
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class TerminalCapabilities
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AdjudicationCode))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class CodeType
    {

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "14.0.24730.2")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.wfm.com/Enterprise/TransactionMgmt/LineItemType/CommonRefTypes/V2", IsNullable = true)]
    public partial class AdjudicationCode : CodeType
    {
    }
}
