namespace Icon.Monitoring.DataAccess
{
    public  interface IConnectionBuilder
    {
        string GetIconConnectionString();

        string GetIrmaConnectionStringForRegion(string region);

        string GetMammothConnectionString();
    }
}