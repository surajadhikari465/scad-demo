
namespace PushController.Controller.Processors
{
    public interface IIconPosProcessor
    {
        void ProcessPosDataForEsb();
        void ProcessPosDataForUdm();
    }
}
