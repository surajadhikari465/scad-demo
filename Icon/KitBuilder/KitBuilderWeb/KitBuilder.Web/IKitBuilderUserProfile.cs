namespace KitBuilder.Web
{
    public interface IKitBuilderUserProfile
    {
        AdUserInformation GetUserInformation(string alias);
    }
}