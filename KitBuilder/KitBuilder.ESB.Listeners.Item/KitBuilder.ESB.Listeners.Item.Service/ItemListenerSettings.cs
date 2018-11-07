using Icon.Common;

namespace KitBuilder.ESB.Listeners.Item.Service
{
    public class ItemListenerSettings
    {
        public bool ValidateSequenceId { get; set; }
        

        public static ItemListenerSettings CreateFromConfig()
        {
            return new ItemListenerSettings
            {
            };
        }
    }
}