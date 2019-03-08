namespace InterfaceController.Common
{
    public static class Enums
    {
        public enum EventNames
        {
            IrmaToIconNewItem = 1,
            IconToIrmaItemUpdates = 2,
            IconToIrmaItemValidation = 3,
            IconToIrmaBrandNameUpdate = 4,
            IconToIrmaTaxClassUpdate = 5,
            IconToIrmaNewTaxClass = 6,
            IconToIrmaSubTeamUpdate = 7,
            IconToIrmaItemSubTeamUpdates = 8,
            IconToIrmaNutritionUpdate = 9,
            IconToIrmaNutritionAdd = 10,
            IconToIrmaBrandDelete = 11,
            IconToIrmaNationalHierarchyUpdate = 12,
            IconToIrmaNationalHierarchyDelete = 13,
						IconToIrmaNutritionDelete = 14
        }

        public enum IrmaRegion
        {
            FL,
            MA,
            MW,
            NA,
            NC,
            NE,
            PN,
            RM,
            SP,
            SO,
            SW,
            UK
        }
    }
}
