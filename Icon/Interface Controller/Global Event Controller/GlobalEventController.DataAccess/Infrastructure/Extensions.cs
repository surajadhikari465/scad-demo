using GlobalEventController.Common;
using InterfaceController.Common;
using System;
using System.Collections.Generic;

namespace GlobalEventController.DataAccess.Infrastructure
{
    public static class Extensions
    {
        private static IDictionary<string, string> eventMap = new Dictionary<string, string>
        {
            { EventConstants.IconToIrmaBrandNameUpdate, Enums.EventNames.IconToIrmaBrandNameUpdate.ToString() },
            { EventConstants.IconToIrmaTaxClassUpdate, Enums.EventNames.IconToIrmaTaxClassUpdate.ToString() },
            { EventConstants.IconToIrmaNewTaxClass, Enums.EventNames.IconToIrmaNewTaxClass.ToString() },
            { EventConstants.IconItemUpdatedEventName, Enums.EventNames.IconToIrmaItemUpdates.ToString() },
            { EventConstants.IconItemValidatedEventName, Enums.EventNames.IconToIrmaItemValidation.ToString() },
            { EventConstants.SubTeamUpdate, Enums.EventNames.IconToIrmaSubTeamUpdate.ToString() },
            { EventConstants.ItemSubTeamUpdate, Enums.EventNames.IconToIrmaItemSubTeamUpdates.ToString() },
            { EventConstants.ItemNutritionUpdate, Enums.EventNames.IconToIrmaNutritionUpdate.ToString() },
            { EventConstants.ItemNutritionAdd, Enums.EventNames.IconToIrmaNutritionAdd.ToString() },
            { EventConstants.IconToIrmaBrandDelete, Enums.EventNames.IconToIrmaBrandDelete.ToString() },
            { EventConstants.IcontoIrmaNationalHierarchyUpdate, Enums.EventNames.IconToIrmaNationalHierarchyUpdate.ToString() },
            { EventConstants.IconToIrmaNationalHierarchyDelete, Enums.EventNames.IconToIrmaNationalHierarchyDelete.ToString() },
        };

        private static IDictionary<string, string> reversedEventMap = new Dictionary<string, string>
        {
            { Enums.EventNames.IconToIrmaBrandNameUpdate.ToString(), EventConstants.IconToIrmaBrandNameUpdate },
            { Enums.EventNames.IconToIrmaTaxClassUpdate.ToString(), EventConstants.IconToIrmaTaxClassUpdate },
            { Enums.EventNames.IconToIrmaNewTaxClass.ToString(), EventConstants.IconToIrmaNewTaxClass },
            { Enums.EventNames.IconToIrmaItemUpdates.ToString(), EventConstants.IconItemUpdatedEventName },
            { Enums.EventNames.IconToIrmaItemValidation.ToString(), EventConstants.IconItemValidatedEventName },
            { Enums.EventNames.IconToIrmaSubTeamUpdate.ToString(), EventConstants.SubTeamUpdate },
            { Enums.EventNames.IconToIrmaItemSubTeamUpdates.ToString(), EventConstants.ItemSubTeamUpdate },
            { Enums.EventNames.IconToIrmaNutritionUpdate.ToString(), EventConstants.ItemNutritionUpdate },
            { Enums.EventNames.IconToIrmaNutritionAdd.ToString(), EventConstants.ItemNutritionAdd },
            { Enums.EventNames.IconToIrmaBrandDelete.ToString(), EventConstants.IconToIrmaBrandDelete },
            { Enums.EventNames.IconToIrmaNationalHierarchyUpdate.ToString(), EventConstants.IcontoIrmaNationalHierarchyUpdate },
            { Enums.EventNames.IconToIrmaNationalHierarchyDelete.ToString(), EventConstants.IconToIrmaNationalHierarchyDelete }
        };

        public static string MapToRegisteredEvent(this string eventName)
        {
            try
            {
                return eventMap[eventName];
            }
            catch (KeyNotFoundException)
            {
                return String.Empty;
            }
        }

        public static string MapToIconEvent(this string eventName)
        {
            try
            {
                return reversedEventMap[eventName];
            }
            catch (KeyNotFoundException)
            {
                return String.Empty;
            }
        }
    }
}
