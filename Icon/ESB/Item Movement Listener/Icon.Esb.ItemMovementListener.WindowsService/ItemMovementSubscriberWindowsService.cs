using System.ServiceProcess;

namespace Icon.Esb.ItemMovementListener.WindowsService
{
    public partial class ItemMovementSubscriberWindowsService : ServiceBase
    {
        private ItemMovementListener listener;

        public ItemMovementSubscriberWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            listener = ItemMovementListenerBuilder.Build();
            listener.Run();
        }

        protected override void OnStop()
        {
            listener.Close();
        }
    }
}
