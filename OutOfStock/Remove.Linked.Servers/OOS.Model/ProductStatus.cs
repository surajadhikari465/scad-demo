using System;

namespace OOS.Model
{
    public class ProductStatus
    {
        private int id;
        private string upc;
        private string status;
        private DateTime? expirationDate;
        private string region;
        private string vendorKey;
        private string vin;
        private string reason;
        private DateTime startDate;
        
        //private const string FormatString = "Region '{0}' Vendor '{1}' Vin '{2}' Upc '{3}' Reason '{4}'"; 
        private const string FormatString = "Region '{0}' Upc '{1}' Status '{2}'";

        public ProductStatus(int id, string region, string vendorKey, string vin, string upc, string reason, DateTime startDate, string status, DateTime? expiryDate)
        {
            this.id = id;
            this.region = region;
            this.vendorKey = vendorKey;
            this.vin = vin;
            this.reason = reason;
            this.startDate = startDate;
            this.upc = upc;
            this.status = status;
            this.expirationDate = expiryDate;
        }

         public ProductStatus()
        {

        }

        public ProductStatus( string region, string vendorKey, string vin, string upc)

        {
            this.id = 0;
            this.region = region;
            this.vendorKey = vendorKey;
            this.vin = vin;
            this.upc = upc;
            this.status = string.Empty;
            this.reason = string.Empty;
            this.startDate = DateTime.MinValue;
            this.expirationDate = DateTime.MinValue;
        }






        public string Upc { get { return upc; } set { upc = value; } }
        public string Status { get { return status; } set { status = value; } }
        public DateTime? ExpirationDate { get { return expirationDate; } set { this.expirationDate = value; } }
        public string Region { get { return region; } set { region = value; } }
        public string VendorKey { get { return vendorKey; } set { vendorKey = value; } }
        public string Vin { get { return vin; } set { vin = value; } }
        public string Reason { get { return reason; } set { reason = value; } }
        public DateTime StartDate { get { return startDate; } set { startDate = value; } }
        public int Id { get { return id; } set { id = value; } }
        
        
        public string StatusForSQL
        {
            get { return status.Replace("'", "''"); }

        }


        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            var productStatus = obj as ProductStatus;
            return productStatus != null && Equals(productStatus);
        }

        public bool Equals(ProductStatus productStatus)
        {
            if (productStatus == null) return false;
            return upc == productStatus.upc && region == productStatus.region;
                //&& vendorKey == productStatus.vendorKey && vin == productStatus.vin;
        }

        public static bool operator ==(ProductStatus leftSide, ProductStatus rightSide)
        {
            if (ReferenceEquals(leftSide, rightSide)) return true;
            if (((object)leftSide == null) || ((object)rightSide == null)) return false;

            return leftSide.Equals(rightSide);
        }

        public static bool operator !=(ProductStatus leftSide, ProductStatus rightSide)
        {
            return !(leftSide == rightSide);
        }

        public override int GetHashCode()
        {
            return upc.GetHashCode() ^ region.GetHashCode(); // ^ vendorKey.GetHashCode() ^ vin.GetHashCode();
        }

        public override string ToString()
        {
            //return string.Format(FormatString, Region, VendorKey, Vin, Upc, Reason);
            return string.Format(FormatString, Region,  Upc, Status);
        }
    }
}
