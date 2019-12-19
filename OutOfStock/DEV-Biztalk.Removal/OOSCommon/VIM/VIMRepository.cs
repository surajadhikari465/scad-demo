using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Text;
using OOSCommon.DataContext;

namespace OOSCommon.VIM
{
    public class VIMRepository : IVIMRepository
    {
        public static string oosVIMServiceName { get; set; }
        public static string oosConnectionString { get; set; }
        public IOOSLog oosLog { get; set; }

        public VIMRepository(string oosConnectionString, string oosVIMServiceName, IOOSLog oosLog)
        {
            VIMRepository.oosConnectionString = oosConnectionString;
            VIMRepository.oosVIMServiceName = oosVIMServiceName;
            this.oosLog = oosLog;
        }

        public Dictionary<string, ItemMasterModel> GetItemMasterModel(IEnumerable<string> upcs)
        {
            //oosLog.Trace("Enter");
            Dictionary<string, ItemMasterModel> result = OOSCommon.VIM.ItemMasterModel.RunQuery(upcs);
            //oosLog.Trace("Exit");
            return result;
        }

        public ItemMasterModel GetItemMasterModel(string upc)
        {
            //oosLog.Trace("Enter");
            ItemMasterModel result = OOSCommon.VIM.ItemMasterModel.RunQuery(upc);
            //oosLog.Trace("Exit");
            return result;
        }

        //public IEnumerable<ItemMasterModel> ItemMasterModel()
        //{
        //    //oosLog.Trace("Enter");
        //    IEnumerable<ItemMasterModel> result = OOSCommon.VIM.ItemMasterModel.RunQuery();
        //    //oosLog.Trace("Exit");
        //    return result;
        //}

        public IEnumerable<VIMOOSItemDataView> GetVIMOOSItemDataView(string upc, string ps_bu)
        {
           // oosLog.Trace("Enter");
            IEnumerable<VIMOOSItemDataView> result = null;
            try
            {
                result = VIMOOSItemDataView.GetVIMOOSItemDataView(upc, ps_bu);
            }
            catch (Exception ex)
            {
                oosLog.Warn("Exception: message=\"" + ex.Message + "\"" + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
            }
           // oosLog.Trace("Exit");
            return result;
        }

        public Dictionary<string, List<VIMOOSItemDataView>> GetVIMOOSItemDataView(List<string> upcs, string ps_bu)
        {
            //oosLog.Trace("Enter");
            Dictionary<string, List<VIMOOSItemDataView>> result = null;
            try
            {
                result = VIMOOSItemDataView.GetVIMOOSItemDataView(upcs, ps_bu);
            }
            catch (Exception ex)
            {
                oosLog.Warn("Exception: message=\"" + ex.Message + "\"" + (ex.InnerException == null ? string.Empty :
                    ", Inner=\"" + ex.InnerException.Message + "\""));
            }
           // oosLog.Trace("Exit");
            return result;
        }

        public IEnumerable<VimSubTeam> GetVimSubTeam(string region, List<string> storeNumbers, List<string> teams)
        {
            return VimSubTeam.RunQuery(region, storeNumbers, teams);
        }

        public IEnumerable<VimTeam> GetVimTeam(List<REGION> regions)
        {
            return VimTeam.RunQuery(regions);
        }

        public string GetVimUPC(string upc, string userRegion)
        {
            return VimUPC.GetVimUPC(upc, userRegion);
        }

        public HashSet<string> GetVimUPC(List<string> upcs, string userRegion)
        {
            return VimUPC.GetVimUPC(upcs, userRegion);
        }
    }
}
