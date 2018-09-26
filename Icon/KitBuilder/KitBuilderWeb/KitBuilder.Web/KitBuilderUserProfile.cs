using System;
using System.DirectoryServices;
using Microsoft.Extensions.Logging;


namespace KitBuilder.Web
{
    public class KitBuilderUserProfile : IKitBuilderUserProfile
    {
        public KitBuilderUserProfile()
        {
        }

        public AdUserInformation GetUserInformation(string alias)
        {
            AdUserInformation userResult = null;
            var displayName = string.Empty;
            var storevalue = string.Empty;
            var regionvalue = string.Empty;


            // return new UserLocation() { FriendlyName = "Test User", Region = "SW", Store = "LMR"};
            if (string.IsNullOrEmpty(alias.Trim()))
                throw new Exception("alias cant be empty.");

            try
            {
                if (alias.StartsWith("WFM"))
                {
                    alias = alias.Split('\\')[1];
                }

                using (
                    DirectoryEntry rootEntry = new DirectoryEntry("LDAP://wfm.pvt")
                    {
                        AuthenticationType = AuthenticationTypes.None
                    })
                {
                    var searcher = new DirectorySearcher(rootEntry);
                    var queryFormat = "(&(objectClass=user)(objectCategory=person)(|(SAMAccountName={0})))";
                    searcher.Filter = string.Format(queryFormat, alias);
                    SearchResultCollection results = searcher.FindAll();


                    if (results.Count > 1)
                    {
                        throw new Exception("Found more than one AD user for [" + alias + "]. Unable to continue.");
                    }

                    else if (results.Count <= 0)
                    {
                        // none found
                        throw new Exception("Found no AD matches for [" + alias + "]. Unable to continue.");
                    }
                    else if (results.Count == 1)
                    {
                        //match
                        if (results[0].Properties["displayName"].Count > 0)
                            displayName = results[0].Properties["displayName"][0].ToString();


                        if (results[0].Properties["WFMDivisionCode"].Count > 0)
                            regionvalue = results[0].Properties["WFMDivisionCode"][0].ToString();


                        if (results[0].Properties["wfmlocationcode"].Count > 0)
                            storevalue = results[0].Properties["wfmlocationcode"][0].ToString();


                        userResult = new AdUserInformation()
                        {
                            FriendlyName = displayName,
                            Region = regionvalue,
                            Store = storevalue
                        };

                    }

                }
                return userResult;

            }
            catch (Exception ex)
            {
                // An ill-formed directory entry could cause null exceptions above but is survivable
                //Logger<>.Warn("==> " + ex.Message + ", stack=" + ex.StackTrace);
            }
            return new AdUserInformation();
        }
    }
}