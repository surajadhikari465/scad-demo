
namespace PushController.Controller.ProcessorModules
{
    public interface IIrmaPosDataProcessingModule
    {
        string CurrentRegion { get; }

        void Execute();
    }
}
