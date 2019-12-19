namespace OutOfStock.Classes
{
    public class RegionData
    {
        public string Name;
        public string Abbreviation;

        public RegionData()
        {   
        }

        public RegionData(string name, string abbrev)
        {
            this.Name = name;
            this.Abbreviation = abbrev;
        }

    }
}