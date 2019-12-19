using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Web;
using OOSCommon.DataContext;

namespace OutOfStock.Models
{
    public class SKUCountModel
    {

        /// <summary>
        /// Get SKU count for store abbreviation, team name, and sub-team name
        /// The result is collapsed (summed) around any omitted value.
        /// </summary>
        /// <param name="storeAbbreviation"></param>
        /// <param name="teamName"></param>
        /// <param name="subteamName"></param>
        /// <returns></returns>
        public static int GetSKUCount(string storeAbbreviation, string teamName, string subteamName)
        {
            OOSEntities db = new OOSEntities();
            // Get store number
            int? storeId = null;
            if (!string.IsNullOrWhiteSpace(storeAbbreviation))
            {
                string ps_bu =
                    (from s in db.STORE
                     where s.STORE_ABBREVIATION.Equals(storeAbbreviation, StringComparison.OrdinalIgnoreCase)
                     select s.PS_BU).FirstOrDefault();
                int iVal = 0;
                if (!string.IsNullOrWhiteSpace(ps_bu) && int.TryParse(ps_bu, out iVal))
                    storeId = iVal;
            }
            // Get Team number (in the real world this depends on store)
            int? teamId = null;
            if (!string.IsNullOrWhiteSpace(teamName))
            {
                teamId =
                    (from t in db.TEAM_Interim
                     where t.teamName.Equals(teamName, StringComparison.OrdinalIgnoreCase)
                     select t.idTeam).FirstOrDefault();
            }
            // TODO -- Get Sub-Team number subteamId (in the real world this depends on store and team)
            int? subteamId = null;
            // Get the SKU counts
            int count = GetSKUCount(db, storeId, teamId, subteamId);
            // Defaults that are better than zero
            if (count == 0)
            {
                switch (teamName.ToLower())
                {
                    case "grocery":
                        count = 6000;
                        break;
                    case "whole body":
                        count = 1200;
                        break;
                }
            }
            return count;
        }

        /// <summary>
        /// Get SKU count for store id, team id, and sub-team id
        /// The result is collapsed (summed) around any omitted value.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="teamId"></param>
        /// <param name="subteamId"></param>
        /// <returns></returns>
        public static int GetSKUCount(int? storeId, int? teamId, int? subteamId)
        {
            OOSEntities db = new OOSEntities();
            return GetSKUCount(db, storeId, teamId, subteamId);
        }

        /// <summary>
        /// Given a data context, get SKU count for store id, team id, and sub-team id
        /// The result is collapsed (summed) around any omitted value.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storeId"></param>
        /// <param name="teamId"></param>
        /// <param name="subteamId"></param>
        /// <returns></returns>
        protected static int GetSKUCount(OOSEntities db, int? storeId, int? teamId, int? subteamId)
        {
            IQueryable<SKUCount> q =
                from s in db.SKUCount
                where s.numberOfSKUs.HasValue
                select s;
            if (storeId.HasValue)
                q = q.Where(s => s.STORE_PS_BU == storeId.Value);
            if (teamId.HasValue)
                q = q.Where(s => s.TEAM_ID == teamId.Value);
            // TODO -- handle subteamId when it comes to exist
            int count = 0;
            if (q.Any())
                count = q.Select(s => s.numberOfSKUs.Value).Sum();
            return count;
        }

    }
}