using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using OOS.Model;
using OOSCommon;
using OOSCommon.DataContext;

namespace OutOfStock.Models
{

    public class UPCEditViewModel
    {
        public List<ProductStatus> ProductStatuses { get; set; }
        public List<string> UPCs { get; set; }
        public Dictionary<string, OOSCommon.VIM.ItemMasterModel> VimDetails { get; set; }
        public List<ProductStatusWithVIMDetails> CombinedResults { get; set; }


        public UPCEditViewModel(List<ProductStatus> productStatuses, List<string> upcs,
                                Dictionary<string, OOSCommon.VIM.ItemMasterModel> vim,
                                 List<ProductStatusWithVIMDetails> combined)
        {
            this.ProductStatuses = productStatuses;
            this.UPCs = upcs;
            this.VimDetails = vim;
            this.CombinedResults = combined;
        }

        public UPCEditViewModel()
        {
            
        }

        public class ProductStatusWithVIMDetails
        {
            public int Id { get; set; }
             
            public string UPC { get; set; }
            [UIHint("ProductStatus")]
            public string Status { get; set; }
            public DateTime StartDate { get; set; }

            [DisplayFormat(NullDisplayText = "", DataFormatString = "{0:yyyy-MM-dd}")]
            [UIHint("ExpirationDate")]
            public DateTime? ExpirationDate { get; set; }

            public string BrandName { get; set; }
            public string ItemDescription { get; set; }
            public string Size { get; set; }
            public string UOM { get; set; }
            public string Region { get; set; }

            public ProductStatusWithVIMDetails(int id, string upc, string region, string status, DateTime startdate,
                                               DateTime? expirationdate,
                                               string brandname, string description, string size, string uom)
            {
                this.Id = id;
                this.UPC = upc;
                this.Region = region;
                this.Status = status;
                this.StartDate = startdate;
                this.ExpirationDate = expirationdate;
                this.BrandName = brandname;
                this.ItemDescription = description;
                this.Size = size;
                this.UOM = uom;

            }

            public ProductStatusWithVIMDetails(int id, string upc, string status, DateTime? expirationdate)
            {
                this.Id = id;
                this.UPC = upc;
                this.Status = status;
                this.ExpirationDate = expirationdate;
            }

            public ProductStatusWithVIMDetails()
            {
            }

            public bool Save(out string errorMessage)
            {
                var result = false;
                using (var db = new OOSEntities())
                {
                    var itemToUpdate = (from ps in db.ProductStatus where ps.id == this.Id select ps).FirstOrDefault();
                    if (itemToUpdate == null)
                    {
                        result = false;
                        errorMessage = "Item not found.";
                    }
                    else
                    {
                        db.ProductStatus.Attach(itemToUpdate);
                        db.ObjectStateManager.ChangeObjectState(itemToUpdate, EntityState.Modified);
                        itemToUpdate.ExpirationDate = this.ExpirationDate;
                        itemToUpdate.ProductStatus = this.Status;
                        db.SaveChanges();
                        result = true;
                        errorMessage = string.Empty;
                    }
                }
                return result;
            }

            

            public bool Delete(out string errorMessage)
            {
                var result = false;
                using (var db = new OOSEntities())
                {
                    var itemToDelete = (from ps in db.ProductStatus where ps.id == this.Id select ps).FirstOrDefault();

                    if (itemToDelete == null)
                    {
                        result = false;
                        errorMessage = "Item not found.";
                    }
                    else
                    {
                        result = true;
                        db.ProductStatus.Attach(itemToDelete);
                        db.DeleteObject(itemToDelete);
                        db.SaveChanges();
                        errorMessage = string.Empty;
                    }
                }
                return result;
            }
        }
    }
}
