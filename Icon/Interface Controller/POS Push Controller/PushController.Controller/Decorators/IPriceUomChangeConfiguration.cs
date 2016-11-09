
namespace PushController.Controller.Decorators
{
    public interface IPriceUomChangeConfiguration
    {
        string PriceUomChangeSubject { get; set; }
        string PriceUomChangeRecipients { get; set; }
        bool SendEmails { get; set; }
    }
}
