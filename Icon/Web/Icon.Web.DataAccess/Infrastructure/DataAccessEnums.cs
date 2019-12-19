
namespace Icon.Web.DataAccess.Infrastructure
{
	public enum SearchStatus
	{
        All,
		Loaded,
		Validated
	}
    public enum FoodStampEligible
    {
        Y,
        N
    }

    public enum HiddenStatus
    {
        Visible,
        Hidden,
        All
    }

    public enum PluRequestStatus
    {
        All,
        New,
        Approved,
        Rejected
    }

    public enum DataType
    {
        Text = 1,
        Number = 2,
        Boolean = 3,
        Date =4
    }

    public enum AttributeType
    {
        GlobalItem = 1,
        Nutrition = 2
    }
}
