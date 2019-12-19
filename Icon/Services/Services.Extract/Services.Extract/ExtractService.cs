namespace Services.Extract
{
    public class ExtractService
    {

        private readonly ExtractServiceListener ListenerApplication;

        public ExtractService(ExtractServiceListener listenerApplication)
        {
            ListenerApplication = listenerApplication;
        }

        public void Start()
        {

            ListenerApplication.Run();

        }

        public void Stop()
        {
            ListenerApplication.Close();
        }
    }
}