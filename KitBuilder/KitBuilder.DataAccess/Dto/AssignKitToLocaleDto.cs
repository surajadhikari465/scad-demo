namespace KitBuilder.DataAccess.Dto
{
    public class AssignKitToLocaleDto
    {
        public int LocaleId { get; set; }
        public int LocaleTypeId { get; set; }
        public string LocaleName { get; set; }
        public string LocaleAbbreviation { get; set; }
        public int ParentLocaleId { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsExcluded { get; set; }
    }
}