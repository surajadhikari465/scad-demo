
namespace Icon.Web.DataAccess.Infrastructure
{
    public interface IManagerHandler<TManager>
    {
        void Execute(TManager data);
    }
}
