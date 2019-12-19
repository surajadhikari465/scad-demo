using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OOSCommon.Import
{

    // Was OOS Database "SOURCE" table
    public enum eSources : int 
    {
        None = 0,
        STORE_UPLOAD = 1,
        VENDOR_COST_FILE = 2,
        UPLOAD = 3
    }

    // Was OOS Database "REASON" table
    public enum eReasons : int
    {
        None = 0, 
        SoonToBeDiscontinuedByUNFI = 1,
        SoonToBeDiscontinuedByVendor = 2,
        DiscontinuedByUNFI = 3,
        DiscontinuedByVendor = 4,
        SoonToBeDiscontinuedPackChange = 5,
        DiscontinuedPackChange = 6
    }

    public class OOSConstant
    {
        // Was OOS Database "SOURCE" table
        public const string sourceText =
            "," +                       // eSources.None
            "STORE UPLOAD," +           // eSources.STORE_UPLOAD
            "VENDOR COST FILE," +       // eSources.VENDOR_COST_FILE
            "UPLOAD";                   // eSources.UPLOAD
        public static string[] sources
        {
            get
            {
                if (_sources == null)
                    _sources = sourceText.Split(new char[] {','});
                return _sources;
            }
        } static string[] _sources = null;

        // Was OOS Database "REASON" table
        public const string reasonText =
            "," +                                       // eReasons.None
            "Soon to be discontinued by UNFI," +        // eReasons.SoonToBeDiscontinuedByUNFI
            "Soon to be discontinued by Vendor," +      // eReasons.SoonToBeDiscontinuedByVendor
            "Discontinued by UNFI," +                   // eReasons.DiscontinuedByUNFI
            "Discontinued by Vendor," +                 // eReasons.DiscontinuedByVendor
            "Soon to be discontinued Pack Change," +    // eReasons.SoonToBeDiscontinuedPackChange
            "Discontinued Pack Change";                 // eReasons.DiscontinuedPackChange
        public static string[] reasons
        {
            get
            {
                if (_reasons == null)
                    _reasons = reasonText.Split(new char[] {','});
                return _reasons;
            }
        } static string[] _reasons = null;
        
    }

}
